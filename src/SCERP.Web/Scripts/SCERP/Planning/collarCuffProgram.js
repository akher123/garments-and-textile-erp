$('.CollarCuffProgram_BuyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/CollarCuffProgram/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        $('.CollarCuffProgram_OrderNo').empty();
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
    $('.CollarCuffProgram_OrderNo').append(
        $('<option/>').attr('value', obj.OrderNo).text(obj.RefNo)
    );
}

$('.CollarCuffProgram_OrderNo').unbind('change').bind('change', function () {
    var orderNo = $(this).val();
    $.Ajax({
        url: '/CollarCuffProgram/GetStyleByOrderNo/',
        data: { orderNo: orderNo }
    }).done(function (styleList) {
        $('.CollarCuffProgram_StyleNo').empty();
        if (styleList.length > 0) {
            $('.CollarCuffProgram_StyleNo').append(
                $('<option/>').attr('value', '').text("Select")
            );
            $.each(styleList, function (style) {
                $('.CollarCuffProgram_StyleNo').append(
                    $('<option/>').attr('value', this.OrderStyleRefId).text(this.StyleNo)
                );
            });
        } else {
            $('.CollarCuffProgram_StyleNo').append(
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
    $('#CollarCuffComponentName').val($("#CollarCuffComponentRefId option:selected").text());
    var dataContent = $('#tableOutputDetail :input');
    var table = $('table#outputProgramDtlTable tbody');
    var fabricRate = $('#cllorCuffRate').val();
    if (fabricRate.length > 0) {
        $.Ajax({
            url: '/CollarCuffProgram/AddOutputDetailRow',
            type: 'POST',
            data: dataContent.serialize(),
        }).done(function (resp) {
            var itemCode = $('#collarCuffItemCoded').val();
            var finishSizeId = $('#finishSizeId').val();
            var collarCuffColorRefId = $('#collarCuffColorRefId').val();
            // $('table#outputProgramDtlTable tbody tr#OutRow-' + itemCode + '-' + collarCuffColorRefId +'-'+finishSizeId).remove();
            table.append(resp);
            dataContent.val("");
        });
    } else {
        alert("Knitting production rate missing ! Please put 100% currect rate");
    }
  
});
$('#btnInputProgDtl').unbind('click').bind('click', function () {
    var colorRefId = $('#yarnColorRefId').val();
    var yarnLotRefId = $('#yarnLotRefId').val();
    var dataContent = $('#tableInputDetail :input');
    var table = $('table#inputProgramDtlTable tbody');
    $.Ajax({
        url: '/CollarCuffProgram/AddInputDetailRow',
        type: 'POST',
        data: dataContent.serialize(),
    }).done(function (resp) {
        var itemCode = $('#yarnItemCode').val();
     //   $('table#inputProgramDtlTable tbody tr#InRow-' + itemCode + "-" + colorRefId + "-" + yarnLotRefId).remove();
        table.append(resp);
        dataContent.val('');
    });
});


$('#btnsSveCollarCuffProgram').unbind('click').bind('click', function () {
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


function editCollarCuffOutputProgram(key) {
    $('#tableOutputDetail :input').val('');
    $('#collarCuffItemName').val($.trim($('#ItemName_' + key).text()));
    $('#collarCuffItemCode').val($('#ItemCode_' + key).val());

    $('#collarCuffColorName').val($.trim($('#ColorName_' + key).text()));
    $('#collarCuffColorRefId').val($('#ColorRefId_' + key).val());

    $('#FinishSizeName').val($.trim($('#FinishSizeName_' + key).text()));
    $('#finishSizeId').val($('#FinishSizeRefId_' + key).val());

    $('#machineGauge').val($('#GSM_' + key).val());
    $('#stretchLength').val($('#SleeveLength_' + key).val());
    $('#numberOfcollorCuff').val($('#NoOfCone_' + key).val());

    $('#cllorCuffRate').val($('#Rate_' + key).val());
    $('#collarCuffYarnQty').val($('#Quantity_' + key).val());
    $('#collarCufRemarks').val($('#Remarks_' + key).val());

}

function editCollarCuffInputProgram(key) {
   
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