
//---------------------


$('#KnittingPartyId').on('change', function () {
    $('#programSearchString').val('');
    $('#programRefId').val();
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

$('.knittingProgrmAutocomplite').autocomplete({
    source: function (request, response) {
        var processRefId = $('#processRefId').val();
        var datatarget = this.element.attr('data-target');
        $(datatarget).val('');
        var url = this.element.attr('action');
        var partyId = 0;
        var option = {
            url: url,
            type: "GET",
            data: { searchString: request.term, processRefId: processRefId, PartyId: partyId },
        };
        $.Ajax(option)
            .done(function (data) {
                $(datatarget).val('');
                response($.map(data, function (item) {
                    return { label: item.ProgramRefId, value: item.ProgramRefId, object: item, datatarget: datatarget };
                }));
            });
    },
    select: function (event, ui) {
        var programRefId = ui.item.object.ProgramRefId;
        $('#delivery_OrderStyleRefId').val(ui.item.object.OrderStyleRefId);
        $('#delivery_orderName').val(ui.item.object.OrderName);
        $('#delivery_styleName').val(ui.item.object.StyleName);
        $('#delivery_BuyerRefId').val(ui.item.object.BuyerRefId);
        $('#delivery_BuyerName').val(ui.item.object.BuyerName);
        $('#delivery_PartyName').val(ui.item.object.PartyName);
        $('#KnittingPartyId').val(ui.item.object.PartyId);
        $(ui.item.datatarget).val(programRefId);
        LodProgramYarnWithdrow(programRefId);
    },
});


$('#Color_SearchString').autocomplete({
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
            $('#yarnSinH').val(response.SinH);
            $('#issue_Rat').val(response.Rate);
            $('#itemName').val(response.ItemName);
            $('#AdvanceYarnDeliveryitemHiddenField').val(response.ItemId);
            $('#Color_SearchString').val(response.ColorName);
            $('#Color_ColorRef').val(response.ColorRefId);

            $('#Size_SearchString').val(response.SizeName);
            $('#Size_SizeRef').val(response.SizeRefId);

            $('#FColorName').val(response.FColorName);
            $('#Color_FColorRef').val(response.FColorRefId);
            $('#lotBrand').val(response.Brand);

            $('#GColorName').focus();


        });

    },
});


function LodProgramYarnWithdrow(programRefId) {

    var option = {
        url: "/YarnDelivery/ProgramYarnWithdrow/",
        type: "GET",
        data: { programRefId: programRefId }
    };
    $.Ajax(option).done(function (htmlResponse) {
        $('#divProgramYarnWithdrow').html(htmlResponse);
    });
}

function SelectYarnLot(lotRefId, lotName, chk) {
    if ($(chk).is(':checked')) {
        var colorRefId = lotRefId;
        $.Ajax({
            url: "/YarnDelivery/YarnStockStatusByLot",
            type: 'GET',
            dataType: 'JSON',
            data: { colorRefId: colorRefId }
        }).done(function (response) {
            clearTextBox(response);
            $('#GColorName').focus();
        });
    } else {
        clearTextBox({ SinH: '', Rate: '', ItemName: '', ItemId: '', ColorName: '', ColorRefId: '', SizeName: '', SizeRefId: '', FColorName: '', FColorRefId: '', Brand:''});
    }
    $('input[type="checkbox"]').not(chk).prop('checked', false);   
}

function clearTextBox(response) {
    $('#yarnSinH').val(response.SinH);
    $('#issue_Rat').val(response.Rate);
    $('#itemName').val(response.ItemName);
    $('#AdvanceYarnDeliveryitemHiddenField').val(response.ItemId);
    $('#Color_SearchString').val(response.ColorName);
    $('#Color_ColorRef').val(response.ColorRefId);

    $('#Size_SearchString').val(response.SizeName);
    $('#Size_SizeRef').val(response.SizeRefId);

    $('#FColorName').val(response.FColorName);
    $('#Color_FColorRef').val(response.FColorRefId);
    $('#lotBrand').val(response.Brand);
}


function deleteRow(buttonObj) {
    var $tr = $(buttonObj).closest("tr");
    $tr.remove();
}

$('#issue_QtyInBag').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;

    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        var $table = $('table#yarnIssueDetail tbody');
        var addNewItem = $('#yarnDeliveryTable :input');
        var itemId = $('#AdvanceYarnDeliveryitemHiddenField').val();
        var option = {
            url: "/YarnDelivery/AddNewRow/",
            type: "POST",
            data: addNewItem.serialize()
        };
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            addNewItem.validate();
            if (!addNewItem.valid()) {
                return false;
            } else {
                var message = "";
                var colorRefId = $('.Color_ColorRef').val();
                var fColorRefId = $('#Color_FColorRef').val();
                if (itemId.length <= 0) {
                    message += "Invalid Item Name";
                }
                if ($('#issue_Rat').val().length <= 0) {
                    message += "Item Rate Required";
                }
                if ($(this).val().length <= 0) {

                    message += "tem Quantity Required";
                }
                if (fColorRefId.length <= 0) {

                    message += "Invalid Finish Color Name";
                }
                if (message.length > 0) {
                    alert(message);
                }
                else {
                    $.Ajax(option).done(function (htmlResponse) {
                        $('table #yarnIssueDetail #newRow-' + itemId + "-" + colorRefId + "-" + fColorRefId).remove();
                        $table.append(htmlResponse);
                        addNewItem.val('');
                        $('#Color_SearchString').focus();
                    });
                }
            }

        }

    }

})
    ;

$('#btnadd_item').on("click", function (e) {
    var key = e.which;
    if (key == 1)  //the enter key code
    {
        e.preventDefault();
        var $table = $('table#yarnIssueDetail tbody');
        var addNewItem = $('#yarnDeliveryTable :input');
        var itemId = $('#AdvanceYarnDeliveryitemHiddenField').val();
        var option = {
            url: "/YarnDelivery/AddNewRow/",
            type: "POST",
            data: addNewItem.serialize()
        };
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            addNewItem.validate();
            if (!addNewItem.valid()) {
                return false;
            } else {
                var message = "";
                var colorRefId = $('.Color_ColorRef').val();
                var fColorRefId = $('#Color_FColorRef').val();
                if (itemId.length <= 0) {
                    message += "Invalid Item Name";
                }
                if ($('#issue_Rat').val().length <= 0) {
                    message += "Item Rate Required";
                }
                if ($(this).val().length <= 0) {

                    message += "tem Quantity Required";
                }
                if (fColorRefId.length <= 0) {

                    message += "Invalid Finish Color Name";
                }
                if (message.length > 0) {
                    alert(message);
                }
                else {
                    $.Ajax(option).done(function (htmlResponse) {
                        $('table #yarnIssueDetail #newRow-' + itemId + "-" + colorRefId + "-" + fColorRefId).remove();
                        $table.append(htmlResponse);
                        addNewItem.val('');
                        $('#btnadd_item').val('+');
                        $('#Color_SearchString').focus();
                    });
                }
            }

        }

    }

})
    ;


$('#GColorName').unbind('keypress').bind('keypress', function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('#issue_QuantityEnterEvent').focus();
    }
});


$('#issue_QuantityEnterEvent').unbind('keypress').bind('keypress', function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('#Wrapper_dr').focus();
    }
});
$('#Wrapper_dr').unbind('keypress').bind('keypress', function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('#issue_QtyInBag').focus();
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