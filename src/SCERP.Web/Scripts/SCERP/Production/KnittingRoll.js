
$('#RollSaveButton').hide();

$('#IsContinueAdd').click(function () {
    var checked = $(this).is(':checked');
    if (checked) {
        $('#RollSaveButton').show();
        $('#RollSubmitButton').hide();
    } else {
        $('#RollSubmitButton').show();
        $('#RollSaveButton').hide();
    }
});

$('#RollSaveButton').unbind('click').bind('click', function (e) {
    var form = $(this).parents('form:first');
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {
            e.preventDefault();
            return true;
        } else {
            $.Ajax({
                url: form.attr('action'),
                data: form.serialize(),
                type: 'POST',
                dataType: "JSON"
            }).done(function (data) {
             //   var role = JSON.parse(data.RoleData)[0];
              //  alert(JSON.stringify(role));

                $('#rolpdf').html(data.RoleData);
                $('#RollRefNo').val(data.RollRefNo);
                alert('Saved Successfully' + role.RollRefNo);
            });
        }
    }
});




$('#RollBulkSaveButton').unbind('click').bind('click', function (e) {
    var $table = $('#rollDetailTable > tbody:first');
    var form = $(this).parents('form:first');
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {
            e.preventDefault();
            return false;
        } else {
            $.Ajax({
                url: form.attr('action'),
                data: form.serialize(),
                type: 'POST',
                dataType: "JSON"
            }).done(function (respons) {
                $table.html('');
            });
        }
    }
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

$('#KnittingPartyId').on('change', function () {
    $('#kanittingPartyNameId').val($("#KnittingPartyId option:selected").text());
    $('.knittingProgrmAutocomplite').val('');
    $('#KnittingProgramId').val();
});

$("#machineId").on('change', function () {
    $("#machineId option:selected").text();
    $('#machineNameId').val($("#machineId option:selected").text());

});

$('.knittingProgrmAutocomplite').autocomplete({
    source: function (request, response) {
        var datatarget = this.element.attr('data-target');
        var url = this.element.attr('action');
      //  var partyId = $('#KnittingPartyId').val();
        var option = {
            url: url,
            type: "GET",
            data: { serachString: request.term},
        };
        $.Ajax(option)
            .done(function (data) {
                $(datatarget).val('');

                response($.map(data, function (item) {
                    return { label: item.ProgramRefId, value: item.ProgramRefId, PartyId: item.PartyId, PartyName: item.PartyName, ProgramId: item.ProgramId, datatarget: datatarget };
                }));
            });
    },
    select: function (event, ui) {
        var programId = ui.item.ProgramId;
        $(ui.item.datatarget).val(programId);
        $('#KnittingPartyId').val(ui.item.PartyId);
        $('#KnittingPartyName').val(ui.item.PartyName);

            SetProgramOutput(programId);
        
   
    },
});

$('.knittingCollarCuffProgrmAutocomplite').autocomplete({
    source: function (request, response) {
        var datatarget = this.element.attr('data-target');
        var url = this.element.attr('action');
        //  var partyId = $('#KnittingPartyId').val();
        var option = {
            url: url,
            type: "GET",
            data: { serachString: request.term },
        };
        $.Ajax(option)
            .done(function (data) {
                $(datatarget).val('');

                response($.map(data, function (item) {
                    return { label: item.ProgramRefId, value: item.ProgramRefId, PartyId: item.PartyId, PartyName: item.PartyName, ProgramId: item.ProgramId, datatarget: datatarget };
                }));
            });
    },
    select: function (event, ui) {
        var programId = ui.item.ProgramId;
        $(ui.item.datatarget).val(programId);
        $('#KnittingPartyId').val(ui.item.PartyId);
        $('#KnittingPartyName').val(ui.item.PartyName);

        var componentRefId = $('#CollarCuffComponentRefId').val();

            SetProgramCollarCuffOutput(programId, componentRefId);
      

    },
});
function SetProgramOutput(programId) {
    var option = {
        url: "/KnittingRoll/GetProgramOutput",
        type: "GET",
        data: { programId: programId },
    };
    $.Ajax(option)
        .done(function (data) {
            $('#sizeNameId').val(data.SizeName);
            $('#machineDial').val(data.SizeRefId);

            $('#fabricTypeNameId').val(data.ItemName);
            $('#fabricItemCode').val(data.ItemCode);



            $('#gsmId').val(data.GSM);

            $('#colorNameId').val(data.ColorName);
            $('#fabricColorName').val(data.ColorRefId);

            $('#finishSizeNameId').val(data.FinishSizeName);
            $('#finishDial').val(data.FinishSizeRefId);
            $('#Roll_StLength').val(data.SleeveLength);
            
            //if ($('#KnittingPartyId').val() === '1') {
            //    $('#KnittingRoleCharllRollNoId').val('IN-');
            //} else {
            //    $('#KnittingRoleCharllRollNoId').val('OUT-');
                
            //}
          
        });
}

function SetProgramCollarCuffOutput(programId, componentRefId) {
   
    var option = {
        url: "/KnittingRoll/GetProgramCollarCuffOutput",
        type: "GET",
        data: { programId: programId, componentRefId: componentRefId },
    };
    $.Ajax(option)
        .done(function (data) {
            $('#sizeNameId').val(data.SizeName);
            $('#machineDial').val(data.SizeRefId);

            $('#fabricTypeNameId').val(data.ItemName);
            $('#fabricItemCode').val(data.ItemCode);


            $('#gsmId').val(data.GSM);

            $('#colorNameId').val(data.ColorName);
            $('#fabricColorName').val(data.ColorRefId);

            $('#finishSizeNameId').val(data.FinishSizeName);
            $('#finishDial').val(data.FinishSizeRefId);
            $('#Roll_StLength').val(data.SleeveLength);

            //if ($('#KnittingPartyId').val() === '1') {
            //    $('#KnittingRoleCharllRollNoId').val('IN-');
            //} else {
            //    $('#KnittingRoleCharllRollNoId').val('OUT-');

            //}

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


function DailyKnittingRoll(reportTypeId) {
    var form = $('.KnittingBatchForm');
    var data = form.serialize();
    var searchurl = '/Production/ProductionReport/DailyKnittingRollDetail?' + data + '&reportTypeId=' + reportTypeId;
    window.open(searchurl);
}

function PartyWiseDailyKnittingRoll(reportTypeId) {
    var form = $('.KnittingBatchForm');
    var data = form.serialize();
    var searchurl = '/Production/ProductionReport/DailyKnittingRollSummary?' + data + '&reportTypeId=' + reportTypeId;
    window.open(searchurl);
}

//$('.tableFocusInputText').formNavigation();
refreshValidation();
function refreshValidation() {
    var form = $('.StyleShipmentForm');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
}



function addNewRoll() {

    var $table = $('#rollDetailTable > tbody:first');
    var option = {
        url: "/KnittingRoll/AddNewRow",
        type: "GET",
        data: $('.KnittingRollform').serialize()
    };
    $.Ajax(option)
        .done(function (htmlResponse) {
            $table.append(htmlResponse);
        });


}
function deleteRow(buttonObj) {
    var $tr = $(buttonObj).closest("tr");
    jQuery.Confirm(SCERP.Messages.DeleteConfirmation, function (r) {
        if (r) {
        }
        $tr.remove();
    });
      
}