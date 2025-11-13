
var fromDate = '01/01/2014';
var toDate = '31/12/2014';

$(document).ready(function () {
    $("#FromDate, #ToDate").datepicker({
        beforeShow: function () {
            return {
                dateFormat: 'dd/mm/yy',
                minDate: fromDate,
                maxDate: toDate,
            };
        }
    });

    $('#grid tr').find('th:last, td:last').hide();
    $('#FromDate, #ToDate').attr('readonly', 'readonly');
    $('#voucherId').val();
    //alert('page reloaded');
});


$('.addTableRow').click(function () {

    $('.tableRow').load('/Accounting/BankReconcilation/Edit').dialog({
        resizable: true,
        title: 'Receipt/Payment Entry',
        height: 'auto',
        width: 'auto',
        modal: true,
        buttons: {
            "Add": function () {
                AddRows();
                $(this).dialog("close");
            }
        }
    });
});


function AddRows() {

    var SL = '1';
    var t = '';

    $('#ManualVoucher tr').each(function (row, tr) {
        t = $(tr).find('td:eq(0)').text();
    });

    if (t != '')
        SL = parseInt(t) + 1;

    var tr = '';
    tr = $('<tr  id="' + SL + '" >');
    tr.append('<td>' + SL + '</td>');
    tr.append('<td>' + $('#Date').val() + '</td>');
    tr.append('<td>' + $('#Particulars').val() + '</td>');
    tr.append('<td>' + $('#CheckNo').val() + '</td>');
    tr.append('<td>' + $('#CheckDate').val() + '</td>');
    tr.append('<td>' + $('#Amount').val() + '</td>');
    tr.append('<td>' + $('#Type option:selected').val() + '</td>');
    tr.append('<td><input type="button" class="del"  value="Delete" /></td>');
    tr.append('</tr>');

    $('#ManualVoucher').append(tr);
}

var check;

function SaveBankReconMaster() {

    var count = 0;
    $('#voucherId').val('');
    
    $('#grid tr').each(function(row, tr) {

        var t = $(tr).find('.chk:checked').val();
        if (t == 'Recon')
            count++;
    });
    if (count == 0) {
        alert('Please Select a voucher');
        return false;
    }
   
    var sectorId = $('#SectorId option:selected').val();
    var fpId = $('#FpId option:selected').val();
    var accountName = $('#AccountName').val();
    var fromDate = $('#FromDate').val();
    var toDate = $('#ToDate').val();

    jQuery.ajax({
        url: "/BankReconcilation/SaveBankReconMaster",
        data: { "sectorId": sectorId, "fpId": fpId, "accountName": accountName, "fromDate": fromDate, "toDate": toDate },
        type: "POST",
        success: function(r) {
            if (!r.Success) {
                alert("Data could not save");
            } else if (r.Success) {

                var i = -1;
                var count = 0;
                var BankReconDetail = [];

                $('#grid tr').each(function(row, tr) {

                    var t = $(tr).find('.chk:checked').val();

                    if (i >= 0) {

                        if (t == 'Recon') {
                            BankReconDetail[count] = $(tr).find('td:eq(7)').text();
                            count++;
                        }
                    }
                    i++;
                });

                SaveBankReconDetail(BankReconDetail);
            }
        }
    });
}


function SaveBankReconDetail(detail) {

    jQuery.ajax({
        url: "/BankReconcilation/SaveBankReconDetail",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ detail: detail }),
        type: "POST",
        success: function(r) {
            if (r.Success == true) {
               
                alert('Data saved Successfully');
                var v = r.voucherId;
                $('#voucherId').val(v);               
            }
            if (r.Success == false) {

                alert('Could not save. duplicate data exists !');
            }
        }
    });
}


function RemoveRowsMaster() {

    var i = -1;

    $('#grid tr').each(function(row, tr) {

        var t = $(tr).find('.chk:checked').val();

        if (i >= 0) {
            if (t == 'Recon') {
                $(tr).remove();
            }
        }
        i++;
    });
}


$(document).on('click', '.search', function () {

    var fpId = $('#FpId option:selected').val();
    var fromDate = $('#FromDate').val();
    var toDate = $('#ToDate').val();

    if (new Date(fromDate) > new Date(toDate)) {

        alert('FromDate can not be greater than ToDate');
        return false;
    }
});


function SetCalenderFixed(Id) { // This is for Calender

    var url = "/VoucherList/GetPeriodFromToDate";

    $.ajax({
        url: url,
        type: "POST",
        data: { "Id": Id },
        dataType: 'json',
        success: function (data) {
            if (data.Success) {
                fromDate = data.FromDate;
                toDate = data.ToDate;
            } else {
                alert('From Date and To Date could not find');
                return false;
            }
        }
    });
}


function SaveBankVoucherManual() {

    $('#ManualVoucher tr').each(function (row, tr) {

        if (row > 0) {

            var voucherManual = {
                RefId: $('#voucherId').val(),
                Date: $(tr).find('td:eq(1)').text(),
                Particulars: $(tr).find('td:eq(2)').text(),
                CheckNo: $(tr).find('td:eq(3)').text(),
                CheckDate: $(tr).find('td:eq(4)').text(),
                Amount: $(tr).find('td:eq(5)').text(),
                Type: $(tr).find('td:eq(6)').text()
            };

            jQuery.ajax({
                url: "/BankReconcilation/SaveBankManual",
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ voucherManual: voucherManual }),
                type: "POST",
                success: function(r) {
                    if (r.Success) {
                        RemoveRowsManual();
                        $('#voucherId').val();
                        alert('Data saved Successfully');
                    }
                }
            });
        }
    });
}


function RemoveRowsManual() {

    var i = -1;

    $('#ManualVoucher tr').each(function(row, tr) {
        if (i >= 0) {
            $(tr).remove();
        }
        i++;
    });
}


$(document).on('click', '#ManualVoucher td:nth-child(8)', function() {

    $(this).closest('tr').remove();

    var serial = 0;
    $('#ManualVoucher tr').each(function(row, tr) {
        $(tr).find('td:eq(0)').html(serial);
        serial++;
    });
});
