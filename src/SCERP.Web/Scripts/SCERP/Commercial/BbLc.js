$('#SearchByLcId').unbind('change').bind('change', function () {
    var LcId = $(this).val();
    $.Ajax({
        url: '/Commercial/Export/GetSalesContacByLcId',
        data: { LcId: LcId }
    }).done(function (salseContact) {
        $('.SalesContactId').empty();
        if (salseContact.length > 0) {
            $('.SalesContactId').append(

                    $('<option/>')
                        .attr('value', '')
                        .text("-Select-")
                );
            $.each(salseContact, function (salseContact) {
                $('.SalesContactId').append(

                    $('<option/>')
                        .attr('value', this.SalseContactId)
                        .text(this.LcNo)
                );
            });
        } else {
            $('.SalesContactId').append(
                $('<option/>')
                    .attr('value', "")
                    .text("No SalesContact Found")
            );
        }

    });


});
$('.ThreadCons_buyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/Inventory/StyleShipment/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        $('.ThreadCons_OrderNo').empty();
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
    $('.ThreadCons_OrderNo').append(
        $('<option/>').attr('value', obj.OrderNo).text(obj.RefNo)
    );
}

$('.ThreadCons_OrderNo').unbind('change').bind('change', function () {
    var orderNo = $(this).val();
    $.Ajax({
        url: '/Inventory/StyleShipment/GetStyleByOrderNo/',
        data: { orderNo: orderNo }
    }).done(function (styleList) {
        $('.ThreadCons_StyleNo').empty();
        if (styleList.length > 0) {
            $('.ThreadCons_StyleNo').append(
                $('<option/>').attr('value', '').text("Select")
            );
            $.each(styleList, function (style) {
                $('.ThreadCons_StyleNo').append(
                    $('<option/>').attr('value', this.OrderStyleRefId).text(this.StyleNo)
                );
            });
        } else {
            $('.ThreadCons_StyleNo').append(
                $('<option/>').attr('value', '').text("No Order Found")
            );
        }
    });
});

$('.ThreadCons_StyleNo').unbind('change').bind('change', function () {
    var orderStyleRefId = $(this).val();
    $.Ajax({
        url: '/ThreadConsumption/GetSizeListByOrderStyleRefId/',
        data: { orderStyleRefId: orderStyleRefId }
    }).done(function (colors) {
        $('.ThreadCons_SizeRefId').empty();
        if (colors.length > 0) {
            $('.ThreadCons_SizeRefId').append(
                $('<option/>')
                    .attr('value', '')
                    .text("Select")
            );
            $.each(colors, function (size) {
                $('.ThreadCons_SizeRefId').append(
                    $('<option/>')
                        .attr('value', this.SizeRefId)
                        .text(this.SizeName)
                );
            });
        } else {
            $('.ThreadCons_SizeRefId').append(
                $('<option/>')
                    .attr('value', "No Order Found")
                    .text("Select")
            );
        }
    });
});
$('#addCashBbLcDetailButton').unbind("keypress").bind("click", function (e) {
    var addNewItem = $('#cashBbLcDetail :input');
    var $table = $('table#cashBbLcDetailTable tbody');
    var option = {
        url: "/BbLc/AddNewRow/",
        type: "POST",
        data: addNewItem.serialize()
    };
    $.Ajax(option).done(function (htmlResponse) {
        $table.append(htmlResponse);
        addNewItem.val('');
        $('#addCashBbLcDetailButton').val('+');
        $('#threadItem').focus();
    });

});
function deleteRow(buttonObj) {
    var $btnObj = $(buttonObj);
    var cltr = $btnObj.closest('tr');
    jQuery.Confirm(SCERP.Messages.DeleteConfirmation, function (r) {
        if (r) {
            cltr.remove();
        }
    });
}


$('#cashBbLcSaveButton').unbind('click').bind('click', function () {
    var button = $(this);
    var form = button.parents("form:first");
    var url = form.attr('action');
    var data = form.serialize();
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {
            return false;
        } else {
            jQuery.Ajax({
                url: url,
                type: "POST",
                data: data,
                container: "#contentRight"
            });
        }

    }
});

function itemAutocomplite(obj) {
    $(obj).autocomplete({
        source: function (request, response) {
            var option = {
                url: "/MaterialIssue/AutocompliteItemByBranch",
                type: "POST",
                data: { itemName: request.term },
            };
            $.Ajax(option)
                .done(function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ItemCode + "_" + item.ItemName + ", ROL :" + item.ReorderLevel, value: item.ItemName, ItemId: item.ItemId };
                    }));
                });
        },
        select: function (event, ui) {
            var itemId = ui.item.ItemId;
            $('.itemHiddenField').val(itemId);
            $.Ajax({
                type: 'GET',
                dataType: "json",
                url: '/MaterialIssue/StockStatys',
                data: { itemId: itemId },
                success: function (result) {
                    if (result.Success == false) {
                        alert(result.Message);
                    } else {
                        populateStockInfo(result);
                        $('.requiredQuantity').focus();
                    }

                },
                onError: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                }
            });
        },
    });
}