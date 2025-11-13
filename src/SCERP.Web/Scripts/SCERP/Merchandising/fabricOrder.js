function findStyleListByOrder(obj) {
    var button = $(obj);
    var form = button.parents('form:first');
    $.Ajax({
        url: '/FabricOrder/StyleListByOrder',
        type: 'get',
        data: form.serialize(),
        container: '#styleListdiv'
    });
}
$('.fabricOrder_buyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/BuyerOrder/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        $('.fabricOrder_OrderNo').empty();
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
    $('.fabricOrder_OrderNo').append(
        $('<option/>')
            .attr('value', obj.OrderNo)
            .text(obj.RefNo)
    );
}
