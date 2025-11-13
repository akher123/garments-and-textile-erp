$('.knittingProgrmAutocomplite').autocomplete({
    source: function (request, response) {
        var datatarget = this.element.attr('data-target');
        var processRefId = '002';//Yarn Dyeing Processor
        $(datatarget).val('');
        var url = this.element.attr('action');
        var partyId = 1; //PlummyFashions Limited Party DataBase Id=1
        var option = {
            url: url,
            type: "GET",
            data: { searchString: request.term, PartyId: partyId, processRefId: processRefId },
        };
        $.Ajax(option)
            .done(function (data) {
                $(datatarget).val('');
                response($.map(data, function (item) {
                    return { label: item.ProgramRefId, value: item.ProgramRefId, ProgramRefId: item.ProgramRefId,object:item, datatarget: datatarget };
                }));
            });
    },
    select: function (event, ui) {
        var programRefId = ui.item.object.ProgramRefId;
        $('#Issue_OrderStyleRefId').val(ui.item.object.OrderStyleRefId);
        $('#Issue_OrderNo').val(ui.item.object.OrderName);
        $('#Issue_StyleName').val(ui.item.object.StyleName);
        $('#Issue_BuyerRefId').val(ui.item.object.BuyerRefId);
        $('#Issue_BuyerName').val(ui.item.object.BuyerName);
        $('#Issue_PartyId').val(ui.item.object.PartyId);

        $('#Issue_PartyName').val(ui.item.object.PartyName);
        $(ui.item.datatarget).val(programRefId);
        LodProgramYarnWithdrow(programRefId);
    },
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

$('#Color_SearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/Lot/LotAutocomplite",
            type: "POST",
            datatype: "html",
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
            $('#AdvanceYarnIssueItemld').val(response.ItemId);
            $('#Color_SearchString').val(response.ColorName);
            $('#Color_ColorRef').val(response.ColorRefId);
      

            $('#Size_SearchString').val(response.SizeName);
            $('#Size_SizeRef').val(response.SizeRefId);

            $('#FColorName').val(response.FColorName);
            $('#Color_FColorRef').val(response.FColorRefId);

            $('#lotBrand').val(response.Brand);
            $('#Issue_qty').focus();
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
            $('#Issue_qty').focus();
        });
    } else {
        clearTextBox({ SinH: '', Rate: '', ItemName: '', ItemId: '', ColorName: '', ColorRefId: '', SizeName: '', SizeRefId: '', FColorName: '', FColorRefId: '', Brand: '' });
    }
    $('input[type="checkbox"]').not(chk).prop('checked', false);
}

function clearTextBox(response) {
    $('#yarnSinH').val(response.SinH);
    $('#issue_Rat').val(response.Rate);
    $('#itemName').val(response.ItemName);
    $('#AdvanceYarnIssueItemld').val(response.ItemId);
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

$('.issue_QuantityEnterEvent').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;

    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        var $table = $('table#yarnIssueDetail tbody');
        var addNewItem = $('#yarnDeliveryTable :input');
        var itemId = $('#AdvanceYarnIssueItemld').val();

        var option = {
            url: "/YarnIssue/AddNewRow",
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
                if ($('#issue_Rat').val().length <= 0) {
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
                        $('table#yarnDeliveryTable tbody tr#newRow-' + itemId).remove();
                        $table.append(htmlResponse);
                        addNewItem.val('');
                        $('#Color_SearchString').focus();
                    });
                }
            }

        }

    }

});


$('#Issue_qty').unbind('keypress').bind('keypress', function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.issue_QuantityEnterEvent').focus();
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