$('.StyleShipment_buyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/Inventory/StyleShipment/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        $('.StyleShipment_OrderNo').empty();
        if (orderList.length > 0) {
            populateDropDown({ OrderNo: "", RefNo: "Select" });
            $.each(orderList, function (order) {
                populateDropDown({ OrderNo: this.OrderNo, RefNo: this.RefNo });
            });
        } else {
            populateDropDown({ RefNo: "No Order Found", OrderNo: "" });
        }
    });
});



function populateDropDown(obj) {
    $('.StyleShipment_OrderNo').append(
        $('<option/>').attr('value', obj.OrderNo).text(obj.RefNo)
    );
}

$('.StyleShipment_OrderNo').unbind('change').bind('change', function () {
    var orderNo = $(this).val();
    $.Ajax({
        url: '/Inventory/StyleShipment/GetStyleByOrderNo/',
        data: { orderNo: orderNo }
    }).done(function (styleList) {
        $('.StyleShipment_StyleNo').empty();
        if (styleList.length > 0) {
            $('.StyleShipment_StyleNo').append(
                $('<option/>').attr('value', '').text("Select")
            );
            $.each(styleList, function (style) {
                $('.StyleShipment_StyleNo').append(
                    $('<option/>').attr('value', this.OrderStyleRefId).text(this.StyleNo)
                );
            });
        } else {
            $('.StyleShipment_StyleNo').append(
                $('<option/>').attr('value', '').text("No Order Found")
            );
        }
    });
});

function removeDependentDropdown() {
    $('.CuttingBatch_buyerRefId').empty();
    $('.CuttingBatch_OrderNo').empty();
    $('.CuttingBatch_StyleNo').empty();
}

$('#findButtion').unbind('click').bind('click', function (e) {
    var orderStyleRefId = $('#shipStyleRefId').val();
    if ($('#' + orderStyleRefId).length > 0) {
        alert('This Style is added');
        $('#' + orderStyleRefId).css('color', 'red');
    } else {
        var div = $('#findAssignedFactoryCuttingJob:input,select');
        var data = div.serialize();
        var url = $(this).attr('action');
        $.Ajax({
            type: "GET",
            url: url,
            data: data,
            container: "#shipmentMatrix"
        });
    }
  


});

function removeMatrix(divId) {
    document.title = "Shipment Size and Color";
    jQuery.Confirm(SCERP.Messages.DeleteConfirmation, function (r) {
        if (r) {
            $('#' + divId).remove().show('slow');
        }
    });

}

$('#addStyleShipment').click(function () {
    var button = $(this);
    var url = button.attr('action');
    //var cuttingBatchId = $('#cuttingBatchId').val();
    //var cuttingTagId = $('#cuttingTagId').val();
    //$('#newRow_' + cuttingBatchId + '-' + cuttingTagId).remove();
    $.Ajax({
        url: url,
        type: "POST",
        data: $('#shipmentMatrix :input').serialize()
    }).done(function (responsHtml) {
        $('#matrixContainer').append(responsHtml);
        $('#shipmentMatrix').html('');
    });
});


