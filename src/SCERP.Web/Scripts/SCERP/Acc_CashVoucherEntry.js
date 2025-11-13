

    $('#FinancialPeriodId').attr('readonly', 'readonly');
    $('#VoucherType').attr('readonly', 'readonly');
    $('#VoucherDate').attr('readonly', 'readonly');

    jQuery('.Amount').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });

    jQuery.ajax({
        url: "/CashVoucherEntry/GetCashInHand",
        type: "POST",
        success: function (r) {
            if (!r.Success) {
                alert("Cash In Hand did not find !");
            } else if (r.Success) {
                $('.CashAccountHead').val(r.data);
            }
        }
    });

    $('.cashbutton').click(function() {

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
            success: function(r) {
                if (r.Success) {

                    alert('Cash In Hand : ' + r.Data);
                }
            }
        });
    });

function AppendTable() {

    var accName = $('.AccountName').val();
    var accCode = accName.split('-');

    if (accCode[0].length != 10) {
        alert('Please select a valid Account Name');
        return;
    }

    if (($('.Amount').val().trim()) == '') {
        alert('Please Insert Amount value !');
        return;
    }

    jQuery.ajax({
        url: "/CashVoucherEntry/CheckGLHead",
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

    if ($('.VoucherType').val() == 'CP') {

        if (rowCount == 1) {
            AddCPOne();
        } else if (rowCount > 1) {
            AddCPTwo();
        }
    }
    else if ($('.VoucherType').val() == 'CR') {

        if (rowCount == 1) {
            AddCROne();
        } else if (rowCount > 1) {
            AddCRTwo();
        }
    }

    function AddCPOne() {

        var tr = '';
        tr = $('<tr  id="' + SL + '" >');
        tr.append('<td>' + SL + '</td>');
        tr.append('<td>' + $('.CashAccountHead').val() + '</td>');
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

    function AddCPTwo() {

        var temp = parseFloat($('#VoucherDetail').find('tr#1').find('td:eq(4)').text());
        var newAmount = temp + parseFloat(amount);

        $('#VoucherDetail').find('tr#1').find('td:eq(4)').html(newAmount + '.00');

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

    function AddCROne() {

        var tr = '';
        tr = $('<tr  id="' + SL + '" >');
        tr.append('<td>' + SL + '</td>');
        tr.append('<td>' + $('.CashAccountHead').val() + '</td>');
        tr.append('<td>' + $('.Particulars').val() + '</td>');
        tr.append('<td>' + amount + '</td>');
        tr.append('<td>' + '0.00' + '</td>');
        // tr.append('<td><input type="button" class="edit" onClick="showReportjoining(' + '1' + ')"  value="Edit" /></td>');
        tr.append('<td><input type="button" class="del"  value="Delete" /></td>');
        tr.append('</tr>');

        $('#VoucherDetail').append(tr);

        var tr = '';
        tr = $('<tr  id="' + (parseInt(SL) + 1) + '" >');
        tr.append('<td>' + (parseInt(SL) + 1) + '</td>');
        tr.append('<td>' + $('.AccountName').val() + '</td>');
        tr.append('<td>' + $('.Particulars').val() + '</td>');
        tr.append('<td>' + '0.00' + '</td>');
        tr.append('<td>' + amount + '</td>');
        // tr.append('<td><input type="button" class="edit" onClick="showReportjoining(' + '1' + ')"  value="Edit" /></td>');
        tr.append('<td><input type="button" class="del"  value="Delete" /></td>');
        tr.append('</tr>');
        $('#VoucherDetail').append(tr);
    }

    function AddCRTwo() {

        var temp = parseFloat($('#VoucherDetail').find('tr#1').find('td:eq(3)').text());
        var newAmount = temp + parseFloat(amount);

        $('#VoucherDetail').find('tr#1').find('td:eq(3)').html(newAmount + '.00');

        var tr = '';
        tr = $('<tr  id="' + (parseInt(SL) + 1) + '" >');
        tr.append('<td>' + (parseInt(SL) + 1) + '</td>');
        tr.append('<td>' + $('.AccountName').val() + '</td>');
        tr.append('<td>' + $('.Particulars').val() + '</td>');
        tr.append('<td>' + '0.00' + '</td>');
        tr.append('<td>' + amount + '</td>');
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
        url: "/CashVoucherEntry/GetInWords",
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
    $('.Particulars').val('');
    $('.Amount').val('');
}

function GetAmountInWord(totalDebit) {

    jQuery.ajax({
        url: "/CashVoucherEntry/GetInWords",
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

$(document).on('click', '#VoucherDetail td:nth-child(6)', function () {

    var debit = '';
    var credit = '';
    var totalDebit = 0.0;
    var totalCredit = 0.0;

    var tr = $(this).closest('tr');

    debit = $(tr).find('td:eq(3)').text();
    credit = $(tr).find('td:eq(4)').text();

    if ($('.VoucherType').val() == 'CP') {

        var TCredit = parseFloat($('#VoucherDetail').find('tr#1').find('td:eq(4)').text());
        var TDebit = parseFloat($(tr).find('td:eq(3)').text());
        var newAmount = TCredit - TDebit;

        $('#VoucherDetail').find('tr#1').find('td:eq(4)').html(newAmount + '.00');
    }
    else if ($('.VoucherType').val() == 'CR') {

        var TCredit = parseFloat($('#VoucherDetail').find('tr#1').find('td:eq(3)').text());
        var TDebit = parseFloat($(tr).find('td:eq(4)').text());
        var newAmount = TCredit - TDebit;

        $('#VoucherDetail').find('tr#1').find('td:eq(3)').html(newAmount + '.00');
    }

    totalDebit = parseFloat($('.TotalDebitAmount').val()) - parseFloat(debit) - parseFloat(credit);
    $('.TotalDebitAmount').val(totalDebit.toFixed(2));

    GetAmountInWord(totalDebit);

    totalCredit = parseFloat($('.TotalCreditAmount').val()) - parseFloat(credit) - parseFloat(debit);
    $('.TotalCreditAmount').val(totalCredit.toFixed(2));

    $('.TotalAmount').val((totalDebit - totalCredit).toFixed(2));

    $(this).closest('tr').remove();

    var serial = 0;
    $('#VoucherDetail tr').each(function (row, tr) {
        $(tr).find('td:eq(0)').html(serial);
        serial++;
    });

    return false;
});

function ChangePaidTo(voucherType) {

    if (voucherType == 'CP')
        $('.PaidLabel').text('Paid To:');
    else if (voucherType == 'CR')
        $('.PaidLabel').text('Ref From:');
    
    $('.CashAccountHead').val('');
    $('.AccountName').val('');

    var count = 0;
    $('#VoucherDetail tr').each(function (row, tr) {
        if (count > 0)
            $(this).closest('tr').remove();

        $('.TotalCreditAmount').val('');
        $('.TotalDebitAmount').val('');
        $('.TotalAmount').val('');
        $('.AmountInWords').val('');
        $('.VoucherParticulars').val(''); 
        $('.PaidTo').val('');
        
        count++;
    });
}

function getCostCentreBySector(sectorId) {   // This is for cascade dropdown 

    var url = "/CashVoucherEntry/GetCostCentreBySector";
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

function SaveCashVoucher() {

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
    };

    var voucherAmount = $('.TotalDebitAmount').val();
    var voucherType = $('.VoucherType').val();

    jQuery.ajax({
        url: "/CashVoucherEntry/GetCashInHandBalance",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ Obj: dataObj }),
        type: "POST",
        success: function(r) {
            if (r.Success) {

                if (parseFloat(voucherAmount) > parseFloat(r.Data) && voucherType == 'CP') {
                    alert('Voucher amount can not be greater than Cash In hand Amount');
                } else {
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
        CheckNo: "",
        CheckDate: "",
        Particulars: $('.VoucherParticulars').val(),
        SectorId: $('#SectorId option:selected').val(),
        CostCentreId: $('#CostCentreId option:selected').val(),
        FinancialPeriodId: "",
        IsActive: "true"
    };

    jQuery.ajax({
        url: "/CashVoucherEntry/SaveMaster",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ voucherMaster: voucherMaster }),
        type: "POST",
        success: function (r) {
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
            url: "/CashVoucherEntry/GetAccountId",
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
        url: "/CashVoucherEntry/SaveDetail",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ voucherDetail: voucherDetail }),
        type: "POST",
        success: function (r) {
            if (r.data == '0') {
                alert('Please Insert a valid Account Name');
            }
        }
    });
}
