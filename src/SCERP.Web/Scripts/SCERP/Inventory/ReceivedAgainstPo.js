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

function itemAutocomplite(obj, index) {
    $(obj).autocomplete({
        source: function (request, response) {
            var option = {
                url: "/ItemStore/AutocompliteItemByBranch",
                type: "POST",
                data: { itemName: request.term },
            };
            $.Ajax(option)
                .done(function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ItemCode + "_" + item.ItemName, value: item.ItemName, ItemId: item.ItemId };
                    }));
                });
        },
        select: function (event, ui) {
            $('.ReceivedtemHiddenField').val(ui.item.ItemId);

        },
    });
}

function deleteRow(buttonObj) {
    var $tr = $(buttonObj).closest("tr");
    $tr.remove();
}

$('.Received_QuantityEnterEvent').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;

    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        var $table = $('table#ReceivedAgstPoDetail tbody');
        var addNewItem = $('#ReceivedDetailTable :input');
        var itemId = $('.ReceivedtemHiddenField').val();

        var option = {
            url: "/YarnReceive/AddNewRow/",
            type: "POST",
            data: addNewItem.serialize()
        };
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            addNewItem.validate();
            if (!addNewItem.valid()) {
                return false;
            } else {
                var message = "";
                if (itemId.length <= 0) {
                    message += "Invalid Item Name";
                }
                if ($('.Received_Rat').val().length <= 0) {
                    message += "Item Rate Required";
                }
                if ($(this).val().length <= 0) {

                    message += "Item Quantity Required";
                }
                if (message.length > 0) {
                    alert(message);
                }
                else {
                    $.Ajax(option).done(function (htmlResponse) {
                        $('table#ReceivedAgstPoDetail tbody tr#newRow-' + itemId).remove();
                        $table.append(htmlResponse);
                        addNewItem.val('');
                        $('.itemName').focus();
                    });
                }
            }

        }

    }

})
;
$('#YarnReceiveType').unbind('change').bind('change', function () {
    var rtype = $(this).val();

    $('#dropdownPiBookingContainner').load('/YarnReceive/PiBooking', { RType: rtype });

});

function loadPiBooking(obj) {
    var rtype = $("#YarnReceiveType").val();
    var piBookingRefId = $(obj).val();
    $.Ajax({
        url: '/YarnReceive/PiBookingList',
        dataType: "JSON",
        type: "GET",
        data: { RType: rtype, PiBookingRefId: piBookingRefId }
    }).done(function(model) {
        $('table#ReceivedAgstPoDetail tbody').html(model.RowString);
        $('#receiveSupplierId').val(model.booking.SupplierId).prop("disable", true).css('background-color', 'goldenrod');
        $('#receiveOrderNo').val(model.booking.OrderNo).prop("disable", true).css('background-color', 'goldenrod');
        $('#receiveBuyerId').val(model.booking.BuyerId).prop("disable", true).css('background-color', 'goldenrod');
        $('#receiveStyleNo').val(model.booking.StyleNo).prop("disable", true).css('background-color', 'goldenrod');
     
    });
    //$('table#ReceivedAgstPoDetail tbody').load('/YarnReceive/PiBookingList', { RType: rtype, PiBookingRefId: piBookingRefId });
    
}
$('.itemName').unbind('keypress').bind('keypress', function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.Color_SearchString').focus();
    }
});

$('.Color_SearchString').unbind('keypress').bind('keypress', function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.Size_SearchString').focus();
    }
});
$('.Size_SearchString').unbind('keypress').bind('keypress', function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.Received_Rat').focus();
    }
});
$('.Received_Rat').unbind('keypress').bind('keypress', function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.Received_QuantityEnterEvent').focus();
    }
});

$('.StoreType').unbind('change').bind('change', function() {
    var storeId = $(this).val();
    if (storeId==1) {
        $('.ColorLabelId').text('Brand');
        $('.SizeLabelId').text('Count');
    } else {
        $('.ColorLabelId').text('Color');
        $('.SizeLabelId').text('Size');
    }
});