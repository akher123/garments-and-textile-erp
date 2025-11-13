////Tab Href
//var href = "/Merchandising/BuyerOrder/Index";
//$("#BuyerOrderHref").attr('href', href + "?SearchString=" + $('#OrderNoumberHiddenId').val());
function selectBuyerOrderStyleRow(orderstyleRefId,obj) {
    var $tab = $("#buyerOrderDetail");
    var button = $(obj);
    var value = orderstyleRefId;
    var $tr = button.closest('tr');
    $tr.addClass("selected").siblings().removeClass("selected");
    var tabAction = $('#buyerOrderStyleTabHref').attr("href").split('=')[0];
    tabAction += "=" + value;
    $('#buyerOrderStyleTabHref').attr('href', tabAction);
    var buyOrdShipTabHref = $('#BuyOrdShipTabHref').attr("href").split('=')[0];
    buyOrdShipTabHref += "=" + value;
    $('#BuyOrdShipTabHref').attr('href', buyOrdShipTabHref);
    $tab.tabs('enable', 2);
    $tab.tabs('enable', 3);
    colorSizeBrackdown(orderstyleRefId);
}


function colorSizeBrackdown(orderStyleRefId) {
    jQuery.Ajax({
        url: '/Merchandising/OmBuyOrdStyle/GetAssortedColorSize'
     , type: "GET",
     data: { OrderStyleRefId: orderStyleRefId }
     , container: '#AssortedColorSize'
    });
}


