$('.YarnDyeingProgram_BuyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/YarnDyeingProgram/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        $('.YarnDyeingProgram_OrderNo').empty();
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
    $('.YarnDyeingProgram_OrderNo').append(
        $('<option/>').attr('value', obj.OrderNo).text(obj.RefNo)
    );
}

$('.YarnDyeingProgram_OrderNo').unbind('change').bind('change', function () {
    var orderNo = $(this).val();
    $.Ajax({
        url: '/YarnDyeingProgram/GetStyleByOrderNo/',
        data: { orderNo: orderNo }
    }).done(function (styleList) {
        $('.YarnDyeingProgram_StyleNo').empty();
        if (styleList.length > 0) {
            $('.YarnDyeingProgram_StyleNo').append(
                $('<option/>').attr('value', '').text("Select")
            );
            $.each(styleList, function (style) {
                $('.YarnDyeingProgram_StyleNo').append(
                    $('<option/>').attr('value', this.OrderStyleRefId).text(this.StyleNo)
                );
            });
        } else {
            $('.YarnDyeingProgram_StyleNo').append(
                $('<option/>').attr('value', '').text("No Order Found")
            );
        }
    });
});


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

$('.autocompliteColorWithBrandSerach').autocomplete({
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
                 return { label: "Lot No :" + color.ColorName + " Brand :" + color.BrandName, value: color.ColorName, ColorRefId: color.ColorRefId, datatarget: datatarget };
             }));

         });
    },
    select: function (event, ui) {
        var colorRefId = ui.item.ColorRefId;
        $(ui.item.datatarget).val(colorRefId);
        LoadLotDetails(colorRefId);
    },

});
function LoadLotDetails(lotId) {
    var option = {
        url: '/Inventory/Lot/GetLotDetails/',
        type: "GET",
        data: { lotId: lotId },
    };
    $.Ajax(option)
     .done(function (data) {
         var response = JSON.parse(data);
         $('#YarnItemName').val(response.ItemName);
         $('#yarnItemCode').val(response.ItemCode);
         $('#yarnCountSizeName').val(response.SizeName);
         $('#yarnCountSizeRefId').val(response.SizeRefId);
         $('#yarnColorName').val(response.FColorName);
         $('#yarnColorRefId').val(response.FColorRefId);
     });
}

$('.autocompliteLotWithBrandSerach').autocomplete({
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
                 return { label: "Lot No :" + color.ColorName + " Brand :" + color.BrandName, value: color.ColorName, ColorRefId: color.ColorRefId, datatarget: datatarget };
             }));

         });
    },
    select: function (event, ui) {
        var colorRefId = ui.item.ColorRefId;
        $(ui.item.datatarget).val(colorRefId);
        LoadLotOutputDetails(colorRefId);
    },

});
function LoadLotOutputDetails(lotId) {
    var option = {
        url: '/Inventory/Lot/GetLotDetails/',
        type: "GET",
        data: { lotId: lotId },
    };
    $.Ajax(option)
     .done(function (data) {
         var response = JSON.parse(data);
         $('#dyedYarnItemName').val(response.ItemName);
         $('#dyedYarnItemCode').val(response.ItemCode);
         $('#dyedYarnCountSizeName').val(response.SizeName);
         $('#dyedYarnCountSizeRefId').val(response.SizeRefId);
         $('#dyedYarnColorName').val(response.FColorName);
         $('#dyedYarnColorRefId').val(response.FColorRefId);
         $('#yarnQty').val(response.SQty);
     });
}


$('.autocompliteSizeSerach').autocomplete({
    source: function (request, response) {
        var datatarget = this.element.attr('data-target');
        var url = this.element.attr('action');
        var option = {
            url: url,
            type: "GET",
            datatype: "json",
            data: { serachString: request.term },
        };
        $.Ajax(option)
            .done(function (data) {
                $(datatarget).val('');
                response($.map(data, function (size) {
                    return { label: size.SizeName, value: size.SizeName, SizeRefId: size.SizeRefId, datatarget: datatarget };
                }));
            });
    },
    select: function (event, ui) {
        var sizeRefId = ui.item.SizeRefId;
        $(ui.item.datatarget).val(sizeRefId);
    },

});

$('.itemautocompliteItem').autocomplete({
    source: function (request, response) {
        var datatarget = this.element.attr('data-target');
        var url = this.element.attr('action');
        var option = {
            url: url,
            type: "GET",
            data: { SearchItemKey: request.term },
        };
        $.Ajax(option)
            .done(function (data) {
                $(datatarget).val('');
                response($.map(data, function (item) {
                    return { label: item.ItemName, value: item.ItemName, ItemCode: item.ItemCode, datatarget: datatarget };
                }));
            });
    },
    select: function (event, ui) {
        var itemCode = ui.item.ItemCode;
        $(ui.item.datatarget).val(itemCode);

    },
});
$('#btnOutputProgDtl').unbind('click').bind('click', function () {

    var dataContent = $('#tableOutputDetail :input');
    var table = $('table#outputProgramDtlTable tbody');
    var dyedyarnRate = $('#dyedyarnRate').val();
    if (dyedyarnRate.length > 0) {
        $.Ajax({
            url: '/YarnDyeingProgram/AddOutputDetailRow',
            type: 'POST',
            data: dataContent.serialize(),
        }).done(function (resp) {
            var itemCode = $('#dyedYarnItemCode').val();
            // var dyedYarnColorRefId = $('#dyedYarnColorRefId').val();
            //   $('table#outputProgramDtlTable tbody tr#OutRow-' + itemCode +'-'+dyedYarnColorRefId).remove();
            table.append(resp);
            dataContent.val('');
        });
    } else {
        alert("Rate missing ! Please put 100% actual rate");
    }
 
});
$('#btnInputProgDtl').unbind('click').bind('click', function () {
    var colorRefId = $('#yarnColorRefId').val();
    var dataContent = $('#tableInputDetail :input');
    var table = $('table#inputProgramDtlTable tbody');
    $.Ajax({
        url: '/YarnDyeingProgram/AddInputDetailRow',
        type: 'POST',
        data: dataContent.serialize(),
    }).done(function (resp) {
        var itemCode = $('#yarnItemCode').val();
      //  $('table#inputProgramDtlTable tbody tr#InRow-' + itemCode + "-" + colorRefId).remove();
        table.append(resp);
        dataContent.val('');
    });
});


$('#btnsSveDayedYarnProgram').unbind('click').bind('click', function () {
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

function removeRow(btnObj) {
    var $btnObj = $(btnObj);
    var cltr = $btnObj.closest('tr');
    jQuery.Confirm(SCERP.Messages.DeleteConfirmation, function (r) {
        if (r) {
            cltr.remove();
        }
    });

}


function editDayedYarnOutputProgram(key) {
    $('#tableOutputDetail :input').val('');
    $('#dyedYarnItemName').val($.trim($('#ItemName_' + key).text()));
    $('#dyedYarnItemCode').val($('#ItemCode_' + key).val());

    $('#dyedYarnColorName').val($.trim($('#ColorName_' + key).text()));
    $('#dyedYarnColorRefId').val($('#ColorRefId_' + key).val());

    $('#dyedYarnCountSizeName').val($.trim($('#SizeName_' + key).text()));
    $('#dyedYarnCountSizeRefId').val($('#SizeRefId_' + key).val());

    $('#dyedYarnLotName').val($.trim($('#LotName_' + key).text()));
    $('#dyedYarnLotRefId').val($('#LotRefId_' + key).val());

    $('#ComboSizeName').val($.trim($('#FinishSizeName_' + key).text()));
    $('#combotSizeId').val($('#FinishSizeRefId_' + key).val());

    $('#Pentone').val($('#GSM_' + key).val());
    $('#repeateMesue').val($('#SleeveLength_' + key).val());
    $('#numberOfCone').val($('#NoOfCone_' + key).val());
    
    $('#dyedyarnRate').val($('#Rate_' + key).val());
    $('#dyedYarnQty').val($('#Quantity_' + key).val());
    $('#dayedYanRemarks').val($('#Remarks_' + key).val());

}

function editDayedYarnInputProgram(key) {
    $('#tableInputDetail :input').val('');
    $('#YarnItemName').val($.trim($('#YarnItemName_' + key).text()));
    $('#yarnItemCode').val($('#YarnItemCode_' + key).val());

    $('#yarnCountSizeName').val($.trim($('#CountSizeName_' + key).text()));
    $('#yarnCountSizeRefId').val($('#CountSizeRefId_' + key).val());

    $('#yarnColorName').val($.trim($('#YarnColorName_' + key).text()));
    $('#yarnColorRefId').val($('#YarnColorRefId_' + key).val());

    $('#yarnLotName').val($.trim($('#LotName_' + key).text()));
    $('#yarnLotRefId').val($('#LotRefId_' + key).val());

    $('#yarnQty').val($('#Quantity_' + key).val());
}