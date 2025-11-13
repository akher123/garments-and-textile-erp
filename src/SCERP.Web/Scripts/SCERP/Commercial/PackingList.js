
$('#addNewRowButton').unbind('click').bind('click', function (e) {

    var form = $(this).parents('form:first');
    var data = form.serialize();
    $.Ajax({
        type: 'POST',
        url: '/Export/AddPackNewRow',
        data: data,
    }).done(function (response) {
        var $table = $('table#PackingTable tbody');
        $table.append(response);
        ClearDetail();
    }
    );
});

function getStyleNoByOrder(orderNo) { // This is for cascade dropdown

    var url = "/Export/GetStyleNoByOrderNo";
    $.ajax({
        url: url,
        type: "get",
        data: { "orderNo": orderNo },
        dataType: 'json',
        success: function (data) {
            if (data.length != 0) {

                var items = "<option value='" + 0 + "'>" + " -Select- " + "</option>";

                $.each(data, function (i, style) {

                    items += "<option value='" + style.OrderStyleRefId + "'>" + style.StyleRefId + "</option>";
                });
                $('#ExportDetail_OrderStyleRefId').html(items);
            } else {
                var items = '<option>Not found</option>';
                $('#ExportDetail_OrderStyleRefId').html(items);
            }
        }
    });
}

function ClearDetail() {

    $('#BuyerOrders').val('');
    $('#ExportDetail_OrderStyleRefId').val('0');
    $('#ExportDetail_CartonQuantity').val('');
    $('#ExportDetail_ItemQuantity').val('');
    $('#ExportDetail_Rate').val('');

    $('#ExportDetail_ItemDescription').val('');
    $('#ExportDetail_ExportCode').val('');
}


var ViewModel = { 
    DeleteTR: function(obj) {
        var $tr = $(obj).closest("tr");
        $tr.remove();
    },
}