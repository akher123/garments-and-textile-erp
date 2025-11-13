

//$(document).ready(function () {

    $('#FinancialPeriodId').attr('readonly', 'readonly');
    $('#VoucherType').attr('readonly', 'readonly');
    $('#VoucherDate').attr('readonly', 'readonly');
    $('#VoucherDate').datepicker({                
    });

    $('.AccountName').autocomplete(
        {
            source: '@Url.Action("TagSearch", "JournalVoucherEntry")'
        });

    jQuery('.Debit').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });

    jQuery('.Credit').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });
   
//});

var rowId = 1;

function AppendTable() {

    var accName = $('.AccountName').val();
    var accCode = accName.split('-');

    if (accCode[0].length != 10) {
        alert('Please select a valid Account Name');
        return;
    }

    if (($('.Debit').val().trim()) == '' && ($('.Credit').val().trim()) == '') {
        alert('Please Insert Debit or Credit value !');
        return;
    }

    if (($('.Debit').val().trim()) != '' && ($('.Credit').val().trim()) != '') {
        alert('Please Insert only Debit or Credit value !');
        return;
    }

    jQuery.ajax({
        url: "/JournalVoucherEntry/CheckGLHead",
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

    var debit = 0.0;
    var credit = 0.00;

    if (($('.Debit').val()) != '')
        debit = parseFloat($('.Debit').val()).toFixed(2);

    if (($('.Credit').val()) != '')
        credit = parseFloat($('.Credit').val()).toFixed(2);

    var tr = '';
    tr = $('<tr  id="' + rowId + '" >');
    tr.append('<td>' + SL + '</td>');
    tr.append('<td>' + $('.AccountName').val() + '</td>');
    tr.append('<td>' + $('.Particulars').val() + '</td>');
    tr.append('<td>' + debit + '</td>');
    tr.append('<td>' + credit + '</td>');
    // tr.append('<td><input type="button" class="edit" onClick="showReportjoining(' + '1' + ')"  value="Edit" /></td>');
    tr.append('<td><input type="button" class="del"  value="Delete" /></td>');
    tr.append('</tr>');

    $('#VoucherDetail').append(tr);

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
        url: "/JournalVoucherEntry/GetInWords",
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
    $('.Debit').val('');
    $('.Credit').val('');
}

function GetAmountInWord(totalDebit) {

    jQuery.ajax({
        url: "/JournalVoucherEntry/GetInWords",
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
    var totalAmount = 0.0;

    var tr = $(this).closest('tr');

    debit = $(tr).find('td:eq(3)').text();
    credit = $(tr).find('td:eq(4)').text();

    totalDebit = parseFloat($('.TotalDebitAmount').val()) - parseFloat(debit);
    $('.TotalDebitAmount').val(totalDebit.toFixed(2));

    GetAmountInWord(totalDebit);

    totalCredit = parseFloat($('.TotalCreditAmount').val()) - parseFloat(credit);
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

function getCostCentreBySector(sectorId) {   // This is for cascade dropdown 

    var url = "/JournalVoucherEntry/GetCostCentreBySector";
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

function SaveJournalVoucher() {

    if ($('#SectorId option:selected').val() == 0) { // Input data validation
        alert('Please select a valid Sector');
        return;
    }

    if ($('#CostCentreId option:selected').val() == 0) {
        alert('Please select a valid CostCentre');
        return;
    }

    var rowCount = 0;
    $('#VoucherDetail tr').each(function (row, tr) {
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

    SaveMaster();
}

function SaveMaster() {

    var voucherMaster = {
        Id: $('#Id').val(),   ////////////////should be added
        VoucherType: "JV",
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
        url: "/JournalVoucherEntry/SaveMaster",
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
            url: "/JournalVoucherEntry/GetAccountId",
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
        url: "/JournalVoucherEntry/SaveDetail",
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