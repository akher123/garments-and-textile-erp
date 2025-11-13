
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
    },

});

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
    //var itemCode = $('#fabricItemCode').val();
    //if ($("table#outputProgramDtlTable tbody tr").length===0) {
    //    crateRow();
    //} else {
    //    if ($('table#outputProgramDtlTable tbody tr').hasClass(itemCode)==='') {
    //        crateRow();
    //    } else {
    //        alert('Fabric Already added');
    //    }
    //}
    var fabricRate = $('#fabricRate').val();
    if (fabricRate.length > 0) {
        crateRow();
    } else {
        alert("Knitting production rate missing ! Please put 100% currect rate");
    }
});
function crateRow() {
    var dataContent = $('#tableOutputDetail :input');
    var table = $('table#outputProgramDtlTable tbody');
    $.Ajax({
        url: '/KnittingOrderProgram/AddOutputDetailRow',
        type: 'POST',
        data: dataContent.serialize(),
    }).done(function (resp) {
        //var itemCode = $('#fabricItemCode').val();
        //$('table#outputProgramDtlTable tbody tr#OutRow-' + itemCode).remove();
        table.append(resp);
        dataContent.val('');
    });
}


$('#btnInputProgDtl').unbind('click').bind('click', function () {
    var colorRefId = $('#yarnColorRefId').val();
    var dataContent = $('#tableInputDetail :input');
    var table = $('table#inputProgramDtlTable tbody');
    $.Ajax({
        url: '/KnittingOrderProgram/AddInputDetailRow',
        type: 'POST',
        data: dataContent.serialize(),
    }).done(function (resp) {
       // var itemCode = $('#yarnItemCode').val();
       // $('table#inputProgramDtlTable tbody tr#InRow-' + itemCode + "-" + colorRefId).remove();
        table.append(resp);
        dataContent.val('');
    });
});


$('#btnsSveKnittingProgram').unbind('click').bind('click', function() {
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


function editFabric(key) {
    $('#tableOutputDetail :input').val('');
    $('#fabricItemName').val($.trim($('#ItemName_' + key).text()));
    $('#fabricItemCode').val($('#ItemCode_' + key).val());
    
    $('#fabricColorName').val($.trim($('#ColorName_' + key).text()));
    $('#fabricColorRefId').val($('#ColorRefId_' + key).val());
    
    $('#machineDialSizeName').val($.trim($('#SizeName_' + key).text()));
    $('#machineDialSizeId').val($('#SizeRefId_' + key).val());
    
    $('#finishDialSizeName').val($.trim($('#FinishSizeName_' + key).text()));
    $('#finishDialSizeId').val($('#FinishSizeRefId_' + key).val());
    
    $('#fabricGsm').val($('#GSM_' + key).val());
    $('#fabricStitchLength').val($('#SleeveLength_' + key).val());
    
    $('#fabricRate').val($('#Rate_' + key).val());
    $('#fabricQty').val($('#Quantity_' + key).val());
    $('#fabricRemarks').val($('#Remarks_' + key).val());
    
}

function editInputProgram(key) {
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