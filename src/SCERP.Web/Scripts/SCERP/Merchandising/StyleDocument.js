
$('.batch_buyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/Inventory/StyleShipment/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        removeDependentDropdown();
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
    $('.batch_OrderNo').append(
        $('<option/>')
            .attr('value', obj.OrderNo)
            .text(obj.RefNo)
    );
}

$('.batch_OrderNo').unbind('change').bind('change', function () {
    var orderNo = $(this).val();
    $.Ajax({
        url: '/Inventory/StyleShipment/GetStyleByOrderNo/',
        data: { orderNo: orderNo }
    }).done(function (styleList) {
        $('.batch_StyleNo').empty();
        if (styleList.length > 0) {
            $('.batch_StyleNo').append(
                $('<option/>')
                    .attr('value', '')
                    .text("Select")
            );
            $.each(styleList, function (style) {
                $('.batch_StyleNo').append(
                    $('<option/>')
                        .attr('value', this.OrderStyleRefId)
                        .text(this.StyleNo)
                );
            });
        } else {
            $('.batch_StyleNo').append(
                $('<option/>')
                    .attr('value', '')
                    .text("No Order Found")
            );
        }
    });
});

function removeDependentDropdown() {
    $('.batch_OrderNo').empty();
    $('.batch_StyleNo').empty();
    $('.batch_Color').empty();

}




