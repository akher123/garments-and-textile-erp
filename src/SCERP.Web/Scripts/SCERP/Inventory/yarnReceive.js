
$('.autocompliteColorSerach').autocomplete({
    source: function (request, response) {
        var datatarget = this.element.attr('data-target');
        var url = this.element.attr('action');
        var option = {
            url: url,
            type: "GET",
            data: { serachString: request.term },
        };

        $.Ajax(option)
            .done(function (data) {
                $(datatarget).val('');
                response($.map(data, function (color) {
                    return { label: color.ColorName, value: color.ColorName, ColorRefId: color.ColorRefId, datatarget: datatarget };
                }));
            });
    },
    select: function (event, ui) {
        var colorRefId = ui.item.ColorRefId;
        $(ui.item.datatarget).val(colorRefId);
    },
});

//$('.Color_SearchString').autocomplete({
//    source: function (request, response) {
//        var option = {
//            // url: "/BuyerOrderColorSize/ColorAutoComplite",
//            url: "/Lot/LotAutocomplite",
//            type: "POST",
//            datatype: "JSON",
//            data: { serachString: request.term,typeId:'02' },
//        };

//        $.Ajax(option)
//            .done(function (data) {
//                $('.Color_ColorRef').val('');
//                response($.map(data, function (color) {
//                    // return { label: color.ColorName, value: color.ColorName, ColorRefId: color.ColorRefId };
//                    return { label: "Lot No :" + color.ColorName + " Brand :" + color.BrandName, value: color.ColorName, ColorRefId: color.ColorRefId, Brand: color.BrandName };
//                }));
//            });
//    },
//    select: function (event, ui) {
//        var colorRefId = ui.item.ColorRefId;
//        $('.Color_ColorRef').val(colorRefId);
//    },
//});

$('.Color_SearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/Lot/LotAutocomplite",
            type: "POST",
            datatype: "json",
            data: { serachString: request.term, typeId: '02' },
        };

        $.Ajax(option)
            .done(function (data) {
                $('#Color_ColorRef').val('');
                response($.map(data, function (color) {
                    return { label: "Lot No :" + color.ColorName + " Brand :" + color.BrandName, value: color.ColorName, ColorRefId: color.ColorRefId, Brand: color.BrandName };
                }));
            });
    },
    select: function (event, ui) {
        var colorRefId = ui.item.ColorRefId;
        $('#Color_ColorRef').val(colorRefId);

        $.Ajax({
            // url: "/YarnDelivery/YarnStockStatus",
            url: "/YarnDelivery/YarnStockStatusByLot",
            type: 'GET',
            dataType: 'JSON',
            data: { colorRefId: colorRefId }
        }).done(function (response) {
         
            var colorRefId = ui.item.ColorRefId;
            $('.Color_ColorRef').val(colorRefId);
            $('.Size_SearchString').val(response.SizeName);
            $('.Size_SizeRef').val(response.SizeRefId);
            $('.autocompliteColorSerach').val(response.FColorName);
            $('#Color_FColorRef').val(response.FColorRefId);
            $('.Received_Rat').val(response.Rate);
            $('.itemName').val(response.ItemName);
            $('.ReceivedtemHiddenField').val(response.ItemId);

            $('.Received_QuantityEnterEvent').focus();
        });

    },
});

$('.Size_SearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/BuyerOrderColorSize/SizeAutoComplite",
            type: "POST",
            datatype: "JSON",
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

    if (key == 13||key==1)  //the enter key code
    {
        e.preventDefault();
        var $table = $('table#ReceivedAgstPoDetail tbody');
        var addNewItem = $('#ReceivedDetailTable :input');
        var itemId = $('.ReceivedtemHiddenField').val();
        var lotNo = $('.Color_ColorRef').val();
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
                        $('table#ReceivedAgstPoDetail tbody tr#newRow-' + lotNo).remove();
                        $table.append(htmlResponse);
                        addNewItem.val('');
                        $('#btn_add_item').val('+');
                        $('.itemName').focus();
                    });
                }
            }

        }

    }

});

$('#btn_add_item').unbind("click").bind("click", function (e) {
    var key = e.which;

    if ( key == 1)  //the enter key code
    {
        e.preventDefault();
        var $table = $('table#ReceivedAgstPoDetail tbody');
        var addNewItem = $('#ReceivedDetailTable :input');
        var itemId = $('.ReceivedtemHiddenField').val();
        var lotNo = $('.Color_ColorRef').val();
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
                        $('table#ReceivedAgstPoDetail tbody tr#newRow-' + lotNo).remove();
                        $table.append(htmlResponse);
                        addNewItem.val('');
                        $('#btn_add_item').val('+');
                        $('.itemName').focus();
                    });
                }
            }

        }

    }

});
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
    }).done(function (model) {
       // alert(new Date(parseInt(model.booking.BookingDate.replace(/\/Date\((.*?)\)\//gi, "$1"))));
        $('table#ReceivedAgstPoDetail tbody').html(model.RowString);
        if (piBookingRefId.length > 0) {
          
            $('#receiveSupplierId').val(model.booking.SupplierId).prop("disable", true).css('background-color', 'goldenrod');
            $('#receiveOrderNo').val(model.booking.OrderNo).prop("disable", true).css('background-color', 'goldenrod');
            $('#receiveBuyerId').val(model.booking.BuyerId).prop("disable", true).css('background-color', 'goldenrod');
            $('#receiveStyleNo').val(model.booking.StyleNo).prop("disable", true).css('background-color', 'goldenrod');
            $('#ActualOrderNo').val(model.booking.Remarks).prop("disable", true).css('background-color', 'goldenrod');
            $('#hdnOrderStyleRefId').val(model.booking.OrderStyleRefId);
            
        } else {
            $('#receiveSupplierId').val('').prop("disable", false).css('background-color', '');
            $('#receiveOrderNo').val('').prop("disable", false).css('background-color', '');
            $('#receiveBuyerId').val('').prop("disable", false).css('background-color', '');
            $('#receiveStyleNo').val('').prop("disable", false).css('background-color', '');
            $('#ActualOrderNo').val('').prop("disable", false).css('background-color', '');
            $('#hdnOrderStyleRefId').val('');
        }
    });
  
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

$('.StoreType').unbind('change').bind('change', function () {
    var storeId = $(this).val();
    if (storeId == 1) {
        $('.ColorLabelId').text('Brand');
        $('.SizeLabelId').text('Count');
    } else {
        $('.ColorLabelId').text('Color');
        $('.SizeLabelId').text('Size');
    }
});


function getLot(obj, colorRefId) {
    $(obj).autocomplete({
        source: function(request, response) {
            var option = {
                // url: "/BuyerOrderColorSize/ColorAutoComplite",
                url: "/Lot/LotAutocomplite",
                type: "GET",
                datatype: "JSON",
                data: { serachString: request.term,typeId:'02' },
            };

            $.Ajax(option)
                .done(function(data) {
                    $('#lot_' + colorRefId).val('');
                    response($.map(data, function(color) {
                        //  return { label: color.ColorName, value: color.ColorName, ColorRefId: color.ColorRefId };
                        return { label: "Lot No :" + color.ColorName + " Brand :" + color.BrandName, value: color.ColorName, ColorRefId: color.ColorRefId, Brand: color.BrandName };
                    }));
                });
        },
        select: function(event, ui) {
            var value = ui.item.ColorRefId;
            $('#lot_' + colorRefId).val(value);
        },
    });
}