
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
$('#addThreadConsumptionDetailButton').unbind("keypress").bind("click", function (e) {
    var addNewItem = $('#threadConsumptionDetail :input');
    var $table = $('table#threadConsumptionDetailTable tbody');
    var option = {
        url: "/ThreadConsumption/AddNewRow/",
        type: "POST",
        data: addNewItem.serialize()
    };
    $.Ajax(option).done(function (htmlResponse) {
        $table.append(htmlResponse);
        addNewItem.val('');
        $('#addThreadConsumptionDetailButton').val('+');
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


$('#threadConsumptionSaveButton').unbind('click').bind('click', function () {
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
