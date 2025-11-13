
$('.PColor_SearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/BuyerOrderColorSize/ColorAutoComplite",
            type: "GET",
            datatype: "html",
            data: { serachString: request.term },
        };

        $.Ajax(option)
            .done(function (data) {
                $('.Color_PColorRefId').val('');

                response($.map(data, function (color) {
                    return { label: color.ColorName, value: color.ColorName, ColorRefId: color.ColorRefId };
                }));
            });
    },
    select: function (event, ui) {
        var colorRefId = ui.item.ColorRefId;
        $('.Color_PColorRefId').val(colorRefId);

    },

});
$('.PColorColorUpdate').unbind('keypress').bind('keypress', function (e) {
    var consTypeRefId = $('#ConsTypeRefId').val();
    var key = e.which;
    if (key == 13) {
        if (consTypeRefId.length > 0) {
            var form = $(this).parents('form:first');
            var data = form.serialize();
            var option = {
                url: '/ConsCollarCuff/UpdatePColor',
                data: data,
                type: 'post',
                container: '#CollarCuffConsumptionDetailList'
            };
            $.Ajax(option);

        } else {
            alert('Select Update Type !!!');
        }
    }
});
$('.Color_SearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/BuyerOrderColorSize/ColorAutoComplite",
            type: "GET",
            datatype: "html",
            data: { serachString: request.term },
        };

        $.Ajax(option)
            .done(function (data) {
                $('.Color_ColorRef').val('');

                response($.map(data, function (color) {
                    return { label: color.ColorName, value: color.ColorName, ColorRefId: color.ColorRefId };
                }));
            });
    },
    select: function (event, ui) {
        var colorRefId = ui.item.ColorRefId;
        $('.Color_ColorRef').val(colorRefId);

    },

});

$('.Size_SearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/BuyerOrderColorSize/SizeAutoComplite",
            type: "GET",
            datatype: "GET",
            data: { serachString: request.term },
        };
        $.Ajax(option)
            .done(function (data) {
                $('.Size_SizeRef').val('');
                response($.map(data, function (size) {
                    return { label: size.SizeName, value: size.SizeName, SizeRefId: size.SizeRefId };
                }));
            });
    },
    select: function (event, ui) {
        var sizeRefId = ui.item.SizeRefId;
        $('.Size_SizeRef').val(sizeRefId);


    },

});
$('.SearchSize_TableWidthID').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/BuyerOrderColorSize/SizeAutoComplite",
            type: "GET",
            datatype: "html",
            data: { serachString: request.term },
        };
        $.Ajax(option)
            .done(function (data) {
                $('.Size_TableWidthID').val('');
                response($.map(data, function (size) {
                    return { label: size.SizeName, value: size.SizeName, SizeRefId: size.SizeRefId };
                }));
            });
    },
    select: function (event, ui) {
        var sizeRefId = ui.item.SizeRefId;
        $('.Size_TableWidthID').val(sizeRefId);


    },

});

$('.compConsUpdate').unbind('keypress').bind('keypress', function (e) {
    var updateKey = $(this).attr('name');
    var consTypeRefId = $('#ConsTypeRefId').val();
    var key = e.which;
    if (key == 13) {
        if (consTypeRefId.length > 0) {
            var form = $(this).parents('form:first');
            var data = form.serialize();
            var option = {
                url: '/ConsCollarCuff/UpdateCollarCuffConsDetail',
                data: data + "&UpdateKey=" + updateKey,
                type: 'post',
                container: '#CollarCuffConsumptionDetailList'
            };
            $.Ajax(option);
        } else {
            alert('Select Update Type !!!');
        }

    }
});


$('.Size_SearchString').unbind('keypress').bind('keypress', function (e) {
    var consTypeRefId = $('#ConsTypeRefId').val();
    var key = e.which;
    if (key == 13) {
        if (consTypeRefId.length > 0) {
            var form = $(this).parents('form:first');
            var data = form.serialize();
            var option = {
                url: '/ConsCollarCuff/UpdateFabricSize',
                data: data,
                type: 'post',
                container: '#CollarCuffConsumptionDetailList'
            };
            $.Ajax(option);
        } else {
            alert('Select Update Type !!!');
        }

    }
});
$('.SearchSize_TableWidthID').unbind('keypress').bind('keypress', function (e) {
    var consTypeRefId = $('#ConsTypeRefId').val();
    var key = e.which;
    if (key == 13) {
        if (consTypeRefId.length > 0) {
            var form = $(this).parents('form:first');
            var data = form.serialize();
            var option = {
                url: '/ConsCollarCuff/UpdateGrWidh',
                data: data,
                type: 'post',
                container: '#CollarCuffConsumptionDetailList'
            };
            $.Ajax(option);
        } else {
            alert('Select Update Type !!!');
        }

    }
});

$('.Color_SearchString').unbind('keypress').bind('keypress', function (e) {
    var consTypeRefId = $('#ConsTypeRefId').val();
    var key = e.which;
    if (key == 13) {
        if (consTypeRefId.length > 0) {
            var form = $(this).parents('form:first');
            var data = form.serialize();
            var option = {
                url: '/ConsCollarCuff/UpdateGrColor',
                data: data,
                type: 'post',
                container: '#CollarCuffConsumptionDetailList'
            };
            $.Ajax(option);

        } else {
            alert('Select Update Type !!!');
        }
    }
});



$('.updateFabricConsQty').unbind('click').bind('click', function (e) {
    var form = $(this).parents('form:first');
    var data = form.serialize();
    var option = {
        url: '/ConsCollarCuff/UpdateFabricQty',
        data: data,
    };
    $.Ajax(option);
});
