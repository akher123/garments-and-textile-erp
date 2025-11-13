function EditComConsumption(compConsumptionId) {
    var option = {
        url: '/FabConsumption/Edit',
        type: "GET",
        data: { CompConsumptionId: compConsumptionId, OrderNo: $('#OrderNumber').val() },
        container: '#CompConsumtionContainerEntyForm'
    };
    jQuery.Ajax(option);
}

function selectComConsumptionRow(compConsumptionId, obj) {
    var button = $(obj);
    var compType = button.attr('data');
    var $tr = button.closest('tr');
    $tr.addClass("selected").siblings().removeClass("selected");
       if (compType==1) {
          
           $tr.addClass("selected").siblings().removeClass("selected");
           var $href = $('#comConsumptionDetailHref');
           var url = '/Merchandising/CompConsumptionDetail/Index';
           url += "?CompConsumptionId=" + compConsumptionId;
       
           $("#FabricComplonetTab").tabs('enable', 2);
           $("#FabricComplonetTab").tabs('disable', 3);
           $href.attr('href', url);
           $('#styleFabConsumptionTabHref').attr('href', '#temp');
       } else {
           //Colar and Cuff Consumtionlink
           var $href1 = $('#CollarCuffconsDetailHref');
           var url1 = '/Merchandising/ConsCollarCuff/Index';
           url1 += "?CompConsumptionId=" + compConsumptionId;
           $href1.attr('href', url1);
           $("#FabricComplonetTab").tabs('disable', 2);
           $("#FabricComplonetTab").tabs('enable', 3);
       }
}

$('.buyerOrderStyle-row').unbind('click').bind('click', function (event) {
    event.preventDefault();
    var $tr = $(this);
    var compConsumptionId = $tr.attr('id');
    var option = {
        url: '/FabConsumption/Edit',
        type: "GET",
        data: { CompConsumptionId: compConsumptionId, OrderNo: $('#OrderNumber').val() },
        container: '#CompConsumtionContainerEntyForm'
    };
    jQuery.Ajax(option);
    $tr.addClass("selected").siblings().removeClass("selected");
    var $href = $('#comConsumptionDetailHref');
    var url = '/Merchandising/CompConsumptionDetail/Index';
    url += "?CompConsumptionId=" + compConsumptionId;
    $tr.addClass("selected").siblings().removeClass("selected");
    $("#FabricComplonetTab").tabs('enable', 2);
    $href.attr('href', url);
});
$('#refreshButton').unbind('click').bind('click', function (event) {
    event.preventDefault();
    var option = {
        url: '/FabConsumption/Edit',
        type: "GET",
        data: { OrderStyleRefId: $('#OrderStyleRefId').val(), OrderNo: $('#OrderNumber').val() },
        container: '#CompConsumtionContainerEntyForm'
    };
    jQuery.Ajax(option);
});
