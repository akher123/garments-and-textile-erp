// Tab href

function SelectYarnConsumptionRow(consRefId, tQty, gColorRefId,obj) {
    var button = $(obj);
    var $tr = button.closest('tr');
    var $href = $('#YarnConsumptionTabHref');
    var url = '/Merchandising/YarnConsumption/Index';
    url += "?ConsRefId=" + consRefId;
    url += "&TQty=" + tQty;
    url += "&GrColorRefId=" + gColorRefId;
    $tr.addClass("selected").siblings().removeClass("selected");
    
    $href.attr('href', url);
    $('#YarnStyleConsumptionTabHref').attr('href', '#temp');
    $("#YarnConsumptionTab").tabs('enable', 2);
}

