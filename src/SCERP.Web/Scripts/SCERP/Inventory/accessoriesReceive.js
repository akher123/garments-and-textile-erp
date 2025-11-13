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
            url: "/AccessoriesReceive/AddNewRow/",
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
$('#poByStyle').unbind('change').bind('change', function () {
    var accReceiveType = $('#accReceiveType').val();
    if (accReceiveType.length===0) {
        alert("Select Received Type");
    }
    var orderStyleRefId = $(this).val();

    $('#dropdownPiBookingContainner').load('/AccessoriesReceive/PiBooking', { RType: accReceiveType, orderStyleRefId: orderStyleRefId });
    loadPiBookingListByStyle(orderStyleRefId);
});

function loadPiBookingListByStyle(orderStyleRefId) {
  
    $.Ajax({
        url: '/AccessoriesReceive/PiBookingListByStyle',
        dataType: "JSON",
        type: "GET",
        data: { orderStyleRefId: orderStyleRefId }
    }).done(function (html) {
        $('table#ReceivedAgstPoDetail tbody').html(html);
    });


}

function loadPiBooking(obj) {
    var rtype = $("#accReceiveType").val();
    var piBookingRefId = $(obj).val();
    if (piBookingRefId.length > 0) {
        $.Ajax({
            url: '/AccessoriesReceive/PiBookingList',
            dataType: "JSON",
            type: "GET",
            data: { RType: rtype, PiBookingRefId: piBookingRefId }
        }).done(function(html) {
            $('table#ReceivedAgstPoDetail tbody').html(html);
        });
    } else {
        var orderStyleRefId = $('#poByStyle').val();
        loadPiBookingListByStyle(orderStyleRefId);
    }
   
  

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

$('.accIssue_BuyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/Inventory/StyleShipment/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        removeDependentDropdown();
        if (orderList.length > 0) {
            populateDropDown({ OrderNo: "", RefNo: "Select" });
            $.each(orderList, function (order) {
                populateDropDown({ OrderNo: this.OrderNo, RefNo: this.RefNo });
            });
        } else {
            populateDropDown({ RefNo: "No Order Found", OrderNo: "" });
        }
    });
});


function populateDropDown(obj) {
    $('.accIssue_OrderNo').append(
        $('<option/>')
        .attr('value', obj.OrderNo)
        .text(obj.RefNo)
    );
}

$('.accIssue_OrderNo').unbind('change').bind('change', function () {
    var orderNo = $(this).val();
    $.Ajax({
        url: '/Inventory/StyleShipment/GetStyleByOrderNo/',
        data: { orderNo: orderNo }
    }).done(function (styleList) {
        $('.accIssue_OrderStyleRefId').empty();
        if (styleList.length > 0) {
            $('.accIssue_OrderStyleRefId').append(
                $('<option/>')
                .attr('value', '')
                .text("Select")
            );
            $.each(styleList, function (style) {
                $('.accIssue_OrderStyleRefId').append(
                    $('<option/>')
                    .attr('value', this.OrderStyleRefId)
                    .text(this.StyleNo)
                );
            });
        } else {
            $('.accIssue_OrderStyleRefId').append(
                $('<option/>')
                .attr('value', '')
                .text("No Order Found")
            );
        }
    });
});


function removeDependentDropdown() {
    $('.accIssue_OrderNo').empty();
    $('.accIssue_OrderStyleRefId').empty();


}



$('#accSaveButton').unbind('click').bind('click', function (e) {
    var form = $(this).parents('form:first');
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {
            e.preventDefault();
            return false;
        } else {
            $.Ajax({
                url: form.attr('action'),
                data: form.serialize(),
                type: 'POST',
                dataType: "JSON"
            }).done(function (respons) {
                if (respons.IsFailed === true) {
                    $('#accRcvRefId').val(respons.RefNo);
                    alert(respons.Message);
                    $("#ReceivedAgstPoDetail> tbody").html("");
                  
                } else {
                    alert(respons.Message);
                }
             
            });
        }
    }
});