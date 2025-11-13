



function deleteOrderStyleSize(btn) {
    var button = $(btn);
    var orderStyleSizeId = button.attr('id');
    var orderStyleRefId = $(".Order_StyleRefId").val();
    var orderStyleSize = {
        OrderStyleSizeId: orderStyleSizeId,
        OrderStyleRefId: orderStyleRefId
    };
    var option = {
        url: "/BuyerOrderColorSize/DelteOrderStyleSize",
        type: "POST",
        data: { Size: orderStyleSize },
        load: SCERP.AjaxCompleted

    };
    document.title = "Style Size";
    jQuery.Confirm(SCERP.Messages.DeleteConfirmation, function (r) {
        if (r) {
            $.Ajax(option).done(function (result) {
                if (result.SuccessStatus) {
                    $("#buyerOrderDetail").tabs("load", 2);
                }

            });
        }
    });
    return false;
}

function deleteOrderStyleColor(btn) {
    var button = $(btn);
    var orderStyleColorId = button.attr('id');
    var orderStyleRefId = $(".Order_StyleRefId").val();
    var orderStyleColor = {
        OrderStyleColorId: orderStyleColorId,
        OrderStyleRefId: orderStyleRefId
    };
    var option = {
        url: "/BuyerOrderColorSize/DelteOrderStyleColor",
        type: "POST",
        data: { Color: orderStyleColor },
        load: SCERP.AjaxCompleted

    };
    document.title = "Style Color";
    jQuery.Confirm(SCERP.Messages.DeleteConfirmation, function (r) {
        if (r) {
            $.Ajax(option).done(function (result) {
                if (result.SuccessStatus) {
                    $("#buyerOrderDetail").tabs("load", 2);
                }

            });
        }
    });
    return false;
}




function ClearStyleSize() {
    $('.Size_OrderStyleStyleColor').val(0);
    $('.Size_SizeRef').val('');
    $('.Size_SearchString').val('');
    $('#SaveStyleSize').val('Add');
    
}
function ClearStyleColor() {
    $('.Color_OrderStyleStyleColor').val(0);
    $('.Color_ColorRef').val('');
    $('.Color_SearchString').val('');
    $('#SaveStyleColor').val('Add');
}
