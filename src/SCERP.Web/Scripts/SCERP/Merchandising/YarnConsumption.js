$('.KColor_SearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/OmColor/ColorAutoComplite",
            type: "GET",
            data: { searchString: request.term },
        };

        $.Ajax(option)
            .done(function (data) {
                $('.KColor_KSizeRefId').val('');

                response($.map(data, function (color) {
                    return { label: color.ColorName, value: color.ColorName, ColorRefId: color.ColorRefId };
                }));
            });
    },
    select: function (event, ui) {
        var colorRefId = ui.item.ColorRefId;
        $('.KColor_KSizeRefId').val(colorRefId);

    },

});

$('.KSize_SearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/OmSize/SizeAutoComplite",
            type: "GET",
            data: { searchString: request.term },
        };
        $.Ajax(option)
            .done(function (data) {
                $('.KSize_SizeRefId').val('');
                response($.map(data, function (size) {
                    return { label: size.SizeName, value: size.SizeName, SizeRefId: size.SizeRefId };
                }));
            });
    },
    select: function (event, ui) {
        var sizeRefId = ui.item.SizeRefId;
        $('.KSize_SizeRefId').val(sizeRefId);


    },

});
$('.Item_ItemName').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/Consumption/AutocompliteItem",
            type: "GET",
            data: { SearchItemKey: request.term },
        };
        $.Ajax(option)
             .done(function (data) {
                 $('.Item_ItemCode').val('');
                 response($.map(data, function (item) {
                     return { label: item.ItemName, value: item.ItemName, ItemCode: item.ItemCode };
                 }));
             });
    },
    select: function (event, ui) {
        var itemCode = ui.item.ItemCode;
        $('.Item_ItemCode').val(itemCode);


    },

});
function itemAutocomplite(obj) {
    $(obj).autocomplete({
        source: function (request, response) {
            var option = {
                url: "/Consumption/AutocompliteItem",
                type: "POST",
                data: { searchItemKey: request.term },
            };
            $.Ajax(option)
                .done(function (data) {
                    response($.map(data, function (item) {
                        return { label: +item.ItemCode + "_" + item.ItemName, value: item.ItemName, ItemCode: item.ItemCode };
                    }));
                });
        },
        select: function (event, ui) {
            var itemCode = ui.item.ItemCode;
            $('.Item_ItemCode').val(itemCode);


        },
    });
}