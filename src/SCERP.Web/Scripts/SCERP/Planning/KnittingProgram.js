$('.KnittingProgram_BuyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/KnittingProgram/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        $('.KnittingProgram_OrderNo').empty();
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
    $('.KnittingProgram_OrderNo').append(
        $('<option/>').attr('value', obj.OrderNo).text(obj.RefNo)
    );
}

$('.KnittingProgram_OrderNo').unbind('change').bind('change', function () {
    var orderNo = $(this).val();
    $.Ajax({
        url: '/KnittingProgram/GetStyleByOrderNo/',
        data: { orderNo: orderNo }
    }).done(function (styleList) {
        $('.KnittingProgram_StyleNo').empty();
        $('.KnittingProgram_StyleNo').append(
            $('<option/>').attr('value', '').text("-Select-")
        );
        if (styleList.length > 0) {
       
            $.each(styleList, function (style) {
                $('.KnittingProgram_StyleNo').append(
                    $('<option/>').attr('value', this.OrderStyleRefId).text(this.StyleNo)
                );
            });
        
        } else {
            $('.KnittingProgram_StyleNo').append(
                $('<option/>').attr('value', '').text("No Order Found")
            );
        }
    });
});
$('#KnittingProgram_ColorRefId').on('change', function () {
    var name = $('#KnittingProgram_ColorRefId option:selected').text();
    $('#KnittingProgram_ColorName').val(name);
});
$('#KnittingProgram_FabricName').on('change', function () {
    var name = $('#KnittingProgram_FabricName option:selected').text();
    $('#knittinFabricName').val(name);
});

showHideFabricTextBox($('#programType'));
$('#programType').on('change', function () {
    showHideFabricTextBox($(this));
});
function showHideFabricTextBox($this){
    if ($this.val() === 'BP') {
        $('.showHide_SP').hide();
        $('.showHide_BP').show();
    } else {
        $('.showHide_BP').hide();
        $('.showHide_SP').show();
    }
}

$('.KnittingProgram_StyleNo').unbind('change').bind('change', GetFabricColorNameByStyle);
function GetFabricColorNameByStyle() {
    var $drd = $('#KnittingProgram_ColorRefId');
    var orderStyleRefId = $(this).val();
    $.Ajax({
        url: '/KnittingProgram/GetFabricColorNameByStyle/',
        data: { orderStyleRefId: orderStyleRefId }
    }).done(function (fabrics) {
        $drd.empty();
        $drd.append(
            $('<option/>').attr('value', '').text("-Select-")
        );
        if (fabrics.length > 0) {
       
            $.each(fabrics, function (fab) {
                $drd.append(
                    $('<option/>').attr('value', this.Id).text(this.Value)
                );
            });
        } else {
            drd.append(
                $('<option/>').attr('value', '').text("Color Not Found")
            );
        }
    });
}

$('#KnittingProgram_ColorRefId').unbind('change').bind('change', GetFabricNameByStyle);
function GetFabricNameByStyle() {
    var $drd = $('#KnittingProgram_FabricName');
    var orderStyleRefId = $('.KnittingProgram_StyleNo').val();
    var colorRefId = $(this).val();
    $.Ajax({
        url: '/KnittingProgram/GetFabricNameByStyle/',
        data: { orderStyleRefId: orderStyleRefId, colorRefId: colorRefId }
    }).done(function (fabrics) {
        $drd.empty();
        $drd.append(
            $('<option/>').attr('value', '').text("-Select-")
        );
        if (fabrics.length > 0) {

            $.each(fabrics, function (fab) {
                $drd.append(
                    $('<option/>').attr('value', this.Id).text(this.Value)
                );
            });
        } else {
            drd.append(
                $('<option/>').attr('value', '').text("Color Not Found")
            );
        }
    });
}

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
    var itemCode = $('#fabricItemCode').val();
    var fabricRate = $('#fabricRate').val();

    if ($("table#outputProgramDtlTable tbody tr").length === 0) {
        if (fabricRate.length > 0) {
            crateRow();
        } else {
            alert("Knitting production rate missing ! Please put 100% currect rate");
        }
       
    } else {
        if ($('table#outputProgramDtlTable tbody tr').hasClass(itemCode)) {
            crateRow();
        } else {
            alert('Multiple fabric not allowed !!');
        }
    }

});
function crateRow() {
    var dataContent = $('#tableOutputDetail :input');
    var table = $('table#outputProgramDtlTable tbody');
    $.Ajax({
        url: '/KnittingProgram/AddOutputDetailRow',
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
        url: '/KnittingProgram/AddInputDetailRow',
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