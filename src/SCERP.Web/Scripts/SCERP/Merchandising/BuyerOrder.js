


function changedOrderStataus(obj) {
  
    var buyerOrderId = $(obj).attr('id');
    var closed = $(obj).val();

    $.Ajax({
        url: '/Merchandising/BuyerOrder/UpdateOrderStatus',
        data: { closed: closed, buyerOrderId: buyerOrderId },
        type: 'POST',
    }).done(function(resp) {
        if (resp) {
            alert('Update Order Status Successfully');
            var whichtr = $(obj).closest("tr");
            whichtr.remove();
       
        } else {
            alert('Faild to update Order Status');
        }
    });
  

}
function selectBuyerOrderRow(orderNo, obj) {
    var button = $(obj);
    var value = orderNo;
    var $tr = button.closest('tr');
    $tr.addClass("selected").siblings().removeClass("selected");
    var $tab = $("#buyerOrderDetail");
    var tabAction = $('#buyerOrderTabHref').attr("href").split('=')[0];
    tabAction += "=" + value;
    $('#buyerOrderTabHref').attr('href', tabAction);
    var orderSheetAction = $('#buyerOrderSheet').attr("href").split('=')[0];
    orderSheetAction += "=" + value;
    $('#buyerOrderSheet').attr('href', orderSheetAction);
    var documentAction = $('#OrderDocumentHref').attr("href").split('=')[0];
    documentAction += "=" + value;
    $('#OrderDocumentHref').attr('href', documentAction);
    $tab.tabs('enable', 1);
    $tab.tabs('enable', 4);
    $tab.tabs('enable', 5);
    $('#BuyerOrderHref').attr('href','#temp');
}

$('#BuyerOrderSearch').unbind('click').bind('click', function () {
    var button = $(this);
    var form = button.parents('form:first');
    var data = form.serialize();
    var url = form.attr('action');
    jQuery.Ajax({
        url: url
        , type: "GET"
        , data: data
        , container: '#buyerOrderIndex'
    });
    $("#buyerOrderDetail").tabs({
        load: SCERP.AjaxCompleted,
        disabled: [1, 2, 3, 4, 5]
    });
});