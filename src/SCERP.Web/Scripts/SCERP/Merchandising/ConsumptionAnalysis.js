function EditConsumption(consumptionId) {
    var option = {
        url: '/Consumption/Edit',
        type: "GET",
        data: { ConsumptionId: consumptionId },
        container: '#consumtionContainerEntyForm'
    };
    jQuery.Ajax(option);
}


function selectConsumptionRow(consumptionId, obj) {
    var button = $(obj);
    var $tr = button.closest('tr');
    $tr.addClass("selected").siblings().removeClass("selected");
    var $href = $('#ConsumptionDetailHref');
    var url = '/Merchandising/ConsumptionDetail/Index';
    url += "?ConsumptionId=" + consumptionId;
    $("#buyerOrderDetail").tabs('enable', 2);
    $href.attr('href', url);
    $('#styleConsumptionTabHref').attr('href', '#temp');
}

$('#refreshButton').unbind('click').bind('click', function (event) {
    event.preventDefault();
    var form = $(this).parents('form:first');
    var option = {
        url: '/Consumption/Refresh',
        type: "GET",
        data: form.serialize(),
        container: '#consumtionContainerEntyForm'
    };
    jQuery.Ajax(option);
});
function itemAutocomplite(obj) {
    $('.itemCodeHiddenField').val('');
    $(obj).autocomplete({
        source: function (request, response) {
            var option = {
                url: "/Consumption/AutocompliteItem",
                type: "GET",
                data: { SearchItemKey: request.term },
            };
            $.Ajax(option)
                .done(function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ItemCode+"--"+item.ItemName, value: item.ItemName, ItemCode: item.ItemCode };
                    }));
                });
        },
        select: function (event, ui) {
            var itemCode = ui.item.ItemCode;
            $('.itemCodeHiddenField').val(itemCode);

        },
    });
}
