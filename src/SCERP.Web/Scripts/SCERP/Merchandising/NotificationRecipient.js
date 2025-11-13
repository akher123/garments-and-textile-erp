
$('.NotificationRecipient_Buyer').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var buyerId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: buyerId
    };

    NotificationRecipient.PopulateSpecSheetDropdownList(option);
});


$('.NotificationRecipient_StyleNo').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var styleNo = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: styleNo
    };

    NotificationRecipient.PopulateJobDropdownList(option);
});

$('.NotificationRecipient_JobNo').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var specSheetId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: specSheetId
    };

    NotificationRecipient.PopulateKeyProcessDropdownList(option);
});

var NotificationRecipient =
{
        PopulateSpecSheetDropdownList: function (option) {
            $.ajax({
                url: option.url,
                type: "GET",
                dataType: "JSON",
                data: { buyerId: option.data },
                success: function (resposns) {
                    NotificationRecipient.ClearDropDownList(option.targetClass);
                    if (resposns.Success == true && resposns.StyleNumbers.length > 0) {

                        for (var i = 0; i < resposns.StyleNumbers.length; i++) {
                            $('.' + option.targetClass)
                          .append($('<option>').text(resposns.StyleNumbers[i].StyleNo).attr('value', resposns.StyleNumbers[i].StyleNo));
                        }

                    } else {
                        $('.' + option.targetClass)
                            .append($('<option>').text("NotFound").attr('value', ""));
                    }

                }
            });
        },
        
        PopulateJobDropdownList: function (option) {
            $.ajax({
                url: option.url,
                type: "GET",
                dataType: "JSON",
                data: { styleNo: option.data },
                success: function (resposns) {
                    NotificationRecipient.ClearDropDownList(option.targetClass);
                    if (resposns.Success == true && resposns.JobNumbers.length > 0) {

                        for (var i = 0; i < resposns.JobNumbers.length; i++) {
                            $('.' + option.targetClass)
                          .append($('<option>').text(resposns.JobNumbers[i].JobNo).attr('value', resposns.JobNumbers[i].SpecificationSheetId));
                        }

                    } else {
                        $('.' + option.targetClass)
                            .append($('<option>').text("NotFound").attr('value', ""));
                    }

                }
            });
        },
        
        PopulateKeyProcessDropdownList: function (option) {
            $.ajax({
                url: option.url,
                type: "GET",
                dataType: "JSON",
                data: { specSheetId: option.data },
                success: function (resposns) {
                    NotificationRecipient.ClearDropDownList(option.targetClass);
                    if (resposns.Success == true && resposns.KeyProcesses.length > 0) {
                        for (var i = 0; i < resposns.KeyProcesses.length; i++) {
                            $('.' + option.targetClass)
                          .append($('<option>').text(resposns.KeyProcesses[i].KeyProcessName).attr('value', resposns.KeyProcesses[i].TimeAndActionId));
                        }

                    } else {
                        $('.' + option.targetClass)
                            .append($('<option>').text("NotFound").attr('value', ""));
                    }

                }
            });
        },
        
        ClearDropDownList: function (selector) {
            $('.' + selector)
                .find('option').remove()
                .end().append($('<option>').text("-select-").attr('value', ""));
        },
    }