$('.consDetailUpdateButton').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        var $button = $(this);
        var $form = $button.parents('form:first');
        var url = $form.attr('action');
        var option = {
            url: url,
            type: 'POST',
            data: $form.serialize(),
            container: '#ConsumptionDetailList'
        };
        $.Ajax(option);
    }

});

$('.Color_SearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/BuyerOrderColorSize/ColorAutoComplite",
            type: "POST",
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
            type: "POST",
            datatype: "html",
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
$('#UpdateProductionSize').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        var $button = $(this);
        var $form = $button.parents('form:first');
        var url = '/ConsumptionDetail/UpdateProductionSize';
        var option = {
            url: url,
            type: 'POST',
            data: $form.serialize(),
            container: '#ConsumptionDetailList'
        };
        $.Ajax(option);
    }

});
$('#UpdateProductionColor').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        var $button = $(this);
        var $form = $button.parents('form:first');
        var url = '/ConsumptionDetail/UpdateProductionColor';
        var option = {
            url: url,
            type: 'POST',
            data: $form.serialize(),
            container: '#ConsumptionDetailList'
        };
        $.Ajax(option);
    }

});

$('#UpdateRemarks').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        var $button = $(this);
        var $form = $button.parents('form:first');
        var url = '/ConsumptionDetail/UpdateRemarks';
        var option = {
            url: url,
            type: 'POST',
            data: $form.serialize(),
            container: '#ConsumptionDetailList'
        };
        $.Ajax(option);
    }

});

$('#UpdateProductQty').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        var $button = $(this);
        var $form = $button.parents('form:first');
        var url = '/ConsumptionDetail/UpdateProductQty';
        var option = {
            url: url,
            type: 'POST',
            data: $form.serialize(),
            container: '#ConsumptionDetailList'
        };
        $.Ajax(option);
    }

});