
$('.Color_SearchString').autocomplete({
    source: function (request, response) {
       
        var option = {
            url: "/BuyerOrderColorSize/ColorAutoComplite",
            type: "POST",
            datatype: "html",
            data: { serachString: request.term},
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
            data: { serachString: request.term},
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
                        return { label: +item.ItemCode + "_" + item.ItemName, value: item.ItemName, ItemId: item.ItemId };
                    }));
                });
        },
        select: function (event, ui) {
            $('.AdvanceAccesDeliveryitemHiddenField').val(ui.item.ItemId);

        },
    });
}

function deleteRow(buttonObj) {
    var $tr = $(buttonObj).closest("tr");
    $tr.remove();
}

$('.issue_QtyInBag').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        var $table = $('table#accessoriesIssueDetail tbody');
        var addNewItem = $('#accessoriesIssueTable :input');
        var itemId = $('.AdvanceAccesDeliveryitemHiddenField').val();

        var option = {
            url: "/AccessoriesIssue/AddNewRow/",
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
                if ($('.issue_Rat').val().length <= 0) {
                    message += "Item Rate Required";
                }
                if ($(this).val().length <= 0) {

                    message += "tem Quantity Required";
                }
                if (message.length > 0) {
                    alert(message);
                }
                else {
                    $.Ajax(option).done(function (htmlResponse) {
                        $('table#acessoriesIssueTable tbody tr#newRow-' + itemId).remove();
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
        $('.issue_Rat').focus();
    }
});
$('.issue_Rat').unbind('keypress').bind('keypress', function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.issue_QuantityEnterEvent').focus();
    }
});

$('.issue_QuantityEnterEvent').unbind('keypress').bind('keypress', function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.issue_QtyInBag').focus();
    }
});

$('.autocompliteEmployeeInfo').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/MaterialIssueRequisition/AutocompliteGetEmployeeInfo",
            type: "GET",
            data: { employeeCardId: request.term },
        };

        $.Ajax(option)
            .done(function (data) {
                response($.map(data, function (employeeInfo) {
                    return { label: employeeInfo.EmployeeCardId + "_" + employeeInfo.EmployeeName, value: employeeInfo.EmployeeName, EmployeeId: employeeInfo.EmployeeId };
                }));
            });
    },
    select: function (event, ui) {
        $('.issued_EmployeeId').val(ui.item.EmployeeId);
        $(".autocompliteEmployeeInfo").val(ui.item.value);
    },

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