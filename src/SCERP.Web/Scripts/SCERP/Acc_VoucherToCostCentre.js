
var rowId = 1;
$('#Amount').val('');

function AppendRow() {

    var amountCheck = $('#Amount').val();

    if (isNaN(amountCheck) || amountCheck == '' || !amountCheck.trim() || parseFloat(amountCheck) < 1) {
        alert('Please insert a valid amount');
        return;
    }

    var SL = '1';
    var t = '';
    var tableAmount = 0;
    $('#VoucherToCostCentreTable tr').each(function (row, tr) {
        t = $(tr).find('td:eq(0)').text();
        var id = $(tr).find('td:eq(0)').text();
        if (id > 0) {
            tableAmount = tableAmount + parseFloat($(tr).find('td:eq(8)').text());
        }
    });

    var totalAmount =parseFloat( $('#TotalAmount').val());

    if (parseFloat(amountCheck) + tableAmount > totalAmount) {
        alert('Total amount exceeded !');
        return;
    }

    if (t != '') SL = parseInt(t) + 1;
        
    var amount = 0.00;

    if (($('#Amount').val()) != '')
        amount = parseFloat($('#Amount').val()).toFixed(2);

    var tr = '';
    tr = $('<tr  id="' + rowId + '" >');
    tr.append('<td>' + SL + '</td>');
    tr.append('<td>' + $('#VoucherDate').val() + '</td>');
    tr.append('<td>' + $('#VoucherRefNo').val() + '</td>');
    tr.append('<td>' + $('#VoucherNo').val() + '</td>');    
    tr.append('<td>' + $('#AccountHeadId option:selected').val() + '</td>');
    tr.append('<td>' + $('#AccountHeadId option:selected').text() + '</td>'); 
    tr.append('<td>' + $('.CostCentre option:selected').text() + '</td>');
    tr.append('<td>' + $('.CostCentre option:selected').val() + '</td>');
    tr.append('<td>' + amount + '</td>');
    tr.append('<td><input type="button" class="del"  value="Delete" /></td>');
    tr.append('</tr>');

    $('#VoucherToCostCentreTable').append(tr);
    $('#Amount').val('');
}


$(document).on('click', '#VoucherToCostCentreTable td:nth-child(10)', function () {  // row delete

    $(this).closest('tr').remove();

    var serial = 0;
    
    $('#VoucherToCostCentreTable tr').each(function(row, tr) {
        $(tr).find('td:eq(0)').html(serial);
        serial++;
    });

    return false;
});


function Save() {

    var voucherToCostCentre = [];
    var voucherNo = $('#VoucherNo').val();
    Delete(voucherNo);

    setTimeout(delay, 3000);

    $('#VoucherToCostCentreTable tr').each(function(row, tr) {

        var id = $(tr).find('td:eq(0)').text();

        if (id > 0) {

            var voucherNo = $(tr).find('td:eq(3)').text();
            var voucherRefNo = $(tr).find('td:eq(2)').text();
            var costCentreId = $(tr).find('td:eq(7)').text();
            var accountCode = $(tr).find('td:eq(4)').text();
            var amount = $(tr).find('td:eq(8)').text();
            var date = $(tr).find('td:eq(1)').text();

            var costCentreDetail = {
                Id: id,
                VoucherNo: voucherNo,
                VoucherRefNo: voucherRefNo,
                CostCentreId: costCentreId,
                AccountCode: accountCode,
                Amount: amount,
                Date: date,
                IsActive: true,
            };
            voucherToCostCentre.push(costCentreDetail);

            SaveDetail(costCentreDetail);
        }
    });    
}

function delay() {

}

function SaveDetail(costCentreDetail) {

    jQuery.ajax({
        url: "/VoucherSagregationToCostCentre/Save",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ model: costCentreDetail }),
        type: "POST",
        success: function(r) {
            if (r.data == '0') {
                alert('Error has created !');
            }
        }
    });
}


function Delete(voucherNo) {
    
    jQuery.ajax({
        url: "/VoucherSagregationToCostCentre/Delete",
        data: { "VoucherNo": voucherNo },
        type: "GET",
        success: function(r) {
            
        }
    }); 
}

