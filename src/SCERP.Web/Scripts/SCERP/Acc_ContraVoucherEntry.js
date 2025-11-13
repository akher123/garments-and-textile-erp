
$(document).ready(function () {

    $('#FinancialPeriodId').attr('readonly', 'readonly');
    $('#VoucherType').attr('readonly', 'readonly');
    $('#VoucherDate').attr('readonly', 'readonly');
    $('#CheckDate').attr('readonly', 'readonly');

    jQuery('.Amount').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });   
});

function AppendTable() {

    var label = ' Bank';
    if ($('.VoucherType option:selected').val() == 'CB')
        label = ' Cash';
        
    var accName = $('.AccountName').val();
    var ContraAccountHead = $('.ContraAccountHead').val();
    
    var accCode = accName.split('-');
    var contraAccCode = ContraAccountHead.split('-');

    if (contraAccCode[0].length != 10) {
        alert('Please select a valid' + label + ' Account Name');
        return;
    }
    
    if (accCode[0].length != 10) {
        alert('Please select a valid Account Name');
        return;
    }
  
    if (($('.Amount').val().trim()) == '') {
        alert('Please Insert Amount value !');
        return;
    }

    jQuery.ajax({
        url: "/ContraVoucherEntry/CheckGLHead",
        data: { "glHead": accName },
        type: "POST",
        success: function (r) {
            if (r.data == '0') {
                alert("Please select a valid Account Name");
            } else if (r.data == '1') {
                AppendRow();
            }
        }
    });     
}

function AppendRow() {
    var SL = '1';
    var t = '';

    $('#VoucherDetail tr').each(function (row, tr) {
        t = $(tr).find('td:eq(0)').text();
    });

    if (t != '')
        SL = parseInt(t) + 1;

    var amount = 0.0;

    if (($('.Amount').val()) != '')
        amount = parseFloat($('.Amount').val()).toFixed(2);

    var rowCount = 0;
    $('#VoucherDetail tr').each(function (row, tr) {
        rowCount++;
    });

  if (rowCount > 2) {
      alert('You can not add more Account Names');
      return;
  }
    AddRows();
     
    function AddRows() {

        var tr = '';
        tr = $('<tr  id="' + SL + '" >');
        tr.append('<td>' + SL + '</td>');
        tr.append('<td>' + $('.ContraAccountHead').val() + '</td>');
        tr.append('<td>' + $('.Particulars').val() + '</td>');
        tr.append('<td>' + '0.00' + '</td>');
        tr.append('<td>' + amount + '</td>');
        // tr.append('<td><input type="button" class="edit" onClick="showReportjoining(' + '1' + ')"  value="Edit" /></td>');
        tr.append('<td><input type="button" class="del"  value="Delete" /></td>');
        tr.append('</tr>');

        $('#VoucherDetail').append(tr);

        var tr = '';
        tr = $('<tr  id="' + (parseInt(SL) + 1) + '" >');
        tr.append('<td>' + (parseInt(SL) + 1) + '</td>');
        tr.append('<td>' + $('.AccountName').val() + '</td>');
        tr.append('<td>' + $('.Particulars').val() + '</td>');
        tr.append('<td>' + amount + '</td>');
        tr.append('<td>' + '0.00' + '</td>');
        // tr.append('<td><input type="button" class="edit" onClick="showReportjoining(' + '1' + ')"  value="Edit" /></td>');
        tr.append('<td><input type="button" class="del"  value="Delete" /></td>');
        tr.append('</tr>');
        $('#VoucherDetail').append(tr);
    }
          
    var totalDebit = 0.0;
    var totalCredit = 0.0;

    $('#VoucherDetail tr').each(function () {

        if (!this.rowIndex) return; // skip first row

        var debit = this.cells[3].innerHTML;
        totalDebit = totalDebit + parseFloat(debit);
        var credit = this.cells[4].innerHTML;
        totalCredit = totalCredit + parseFloat(credit);
    });

    $('.TotalDebitAmount').val(totalDebit.toFixed(2));
    $('.TotalCreditAmount').val(totalCredit.toFixed(2));
    $('.TotalAmount').val((totalDebit - totalCredit).toFixed(2));

    jQuery.ajax({
        url: "/ContraVoucherEntry/GetInWords",
        data: { "amount": totalDebit },
        type: "POST",
        success: function (r) {
            if (!r.Success) {
                alert("Amount did not find !");
            } else if (r.Success) {
                $('.AmountInWords').val(r.data);
            }
        }
    });

    $('.AccountName').val('');
    $('.ContraAccountHead').val('');
    $('.Particulars').val('');
    $('.Amount').val('');
}

function GetAmountInWord(totalDebit) {

    jQuery.ajax({
        url: "/ContraVoucherEntry/GetInWords",
        data: { "amount": totalDebit },
        type: "POST",
        success: function (r) {
            if (!r.Success) {
                alert("Amount did not find !");
            } else if (r.Success) {
                $('.AmountInWords').val(r.data);
            }
        }
    });
}

// Remove two rows from Table
$(document).on('click', '#VoucherDetail td:nth-child(6)', function () {
    
    var serial = 0;
    $('#VoucherDetail tr').each(function (row, tr) {
        if (serial > 0) {
            $(tr).remove();
            $('.TotalDebitAmount').val('');
            $('.TotalCreditAmount').val(''); 
            $('.TotalAmount').val('');
            $('.AmountInWords').val('');
        }
        serial++;
    });

    return false;
});

function getCostCentreBySector(sectorId) {   // This is for cascade dropdown 

    var url = "/ContraVoucherEntry/GetCostCentreBySector";
    $.ajax({
        url: url,
        type: "get",
        data: { "sectorId": sectorId },
        dataType: 'json',
        success: function (data) {
            if (data.length != 0) {

                var items = "<option value='" + 0 + "'>" + " -Select - " + "</option>";

                $.each(data, function (i, costCentre) {

                    items += "<option value='" + costCentre.Id + "'>" + costCentre.CostCentreName + "</option>";
                });
                $('.CostCentre').html(items);
            } else {
                var items = '<option>Not found</option>';
                $('.CostCentre').html(items);
            }
        }
    });
}

$('.cashbutton').click(function () {

    var dataObj = {
        SectorId: $('#SectorId option:selected').val(),
        CostCentre: "0",
        FpId: "",
        GlId: "269",
        StartDate: $('#VoucherDate').val(),
        EndDate: $('#VoucherDate').val(),
    };

    jQuery.ajax({
        url: "/CashVoucherEntry/GetCashInHandBalance",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ Obj: dataObj }),
        type: "POST",
        success: function (r) {
            if (r.Success) {

                alert('Cash In Hand : ' + r.Data);
            }
        }
    });
});

function SaveContraVoucher() {

    if ($('#SectorId option:selected').val() == 0) { // Input data validation
        alert('Please select a valid Sector');
        return;
    }

    if ($('#CostCentreId option:selected').val() == 0) {
        alert('Please select a valid CostCentre');
        return;
    }

    var rowCount = 0;
    $('#VoucherDetail tr').each(function(row, tr) {
        rowCount++;
    });

    if (rowCount < 2) {
        alert('Please Insert Account related Information');
        return;
    }

    if (parseFloat($('.TotalDebitAmount').val()) != parseFloat($('.TotalCreditAmount').val())) { // End of input data validation
        alert('Debit and Credit should be same amount');
        return;
    }
  
    var dataObj = {
        SectorId: $('#SectorId option:selected').val(),
        CostCentre: "0",
        FpId: "",
        GlId: "269",
        StartDate: $('.VoucherDate').val(),
        EndDate: $('.VoucherDate').val(),
        VoucherType: $('.VoucherType option:selected').val(),
        VoucherTypeName: $('.VoucherType option:selected').text(),
        Amount:  $('.TotalDebitAmount').val()
    };

    jQuery.ajax({
        url: "/ContraVoucherEntry/GetCashInHandBalance",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ Obj: dataObj }),
        type: "POST",
        success: function(r) {
            if (r.Success) {

                if (r.Data == '1')
                    alert('Voucher amount exceeded limit !');
                
                else if (r.Data == '2')
                    alert('Voucher amount exceeded cash in hand amount');
                
                else {
                    SaveMaster();
                }
            }
        }
    });   
}

function SaveMaster() {

    var voucherMaster = {
        Id: $('#Id').val(),
        VoucherType: $('.VoucherType option:selected').val(),
        VoucherNo: "1",
        VoucherDate: $('#VoucherDate').val(),
        CheckNo: $('.CheckNo').val(),
        CheckDate: $('#CheckDate').val(),
        Particulars: $('.VoucherParticulars').val(),
        SectorId: $('#SectorId option:selected').val(),
        CostCentreId: $('#CostCentreId option:selected').val(),
        FinancialPeriodId: "",
        IsActive: "true"
    };

    jQuery.ajax({
        url: "/ContraVoucherEntry/SaveMaster",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ voucherMaster: voucherMaster }),
        type: "POST",
        success: function(r) {
            if (r.data == '0') {
                alert('Please Insert a valid Account Name');
            }
            SaveDetail();
            alert('Data saved successfully !');
        }
    });
}

function SaveDetail() {

    var voucherDetailTotal = [];

    $('#VoucherDetail tr').each(function (row, tr) {

        var accountCode = $(tr).find('td:eq(1)').text();
        accountCode = accountCode.substr(0, 10);

        jQuery.ajax({
            url: "/ContraVoucherEntry/GetAccountId",
            data: { "accountCode": accountCode },
            type: "POST",
            success: function (r) {
                if (r.Success) {

                    var voucherDetail = {
                        RefId: $('#Id').val(),
                        GLID: r.data,
                        Particulars: $(tr).find('td:eq(2)').text(),
                        Debit: $(tr).find('td:eq(3)').text(),
                        Credit: $(tr).find('td:eq(4)').text()
                    };
                    voucherDetailTotal.push(voucherDetail);
                    SaveDetailMore(voucherDetail);
                }
            }
        });
    });
}

function SaveDetailMore(voucherDetail) {

    jQuery.ajax({
        url: "/ContraVoucherEntry/SaveDetail",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ voucherDetail: voucherDetail }),
        type: "POST",
        success: function(r) {
            if (r.data == '0') {
                alert('Please Insert a valid Account Name');
            }
        }
    });
}
