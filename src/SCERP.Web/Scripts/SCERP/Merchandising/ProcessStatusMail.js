jQuery('#grid tbody tr').each(function () {

    var mailStatus = $(this).find('td:eq(5)').text();
    var SMSStatus = $(this).find('td:eq(6)').text();

    if (mailStatus == 'False')
        jQuery(this).find('td:eq(7)').find('input[type="button"]').hide();

    if (SMSStatus == 'False')
        jQuery(this).find('td:eq(8)').find('input[type="button"]').hide();
});

$('#grid tr').find('th:nth-child(1), td:nth-child(1)').hide();
$('#grid tr').find('th:nth-child(6), td:nth-child(6)').hide();
$('#grid tr').find('th:nth-child(7), td:nth-child(7)').hide();

$('.btnMail').click(function () {

    var recipientId = $(this).closest('td').siblings(':first-child').text();
    var url = "/Merchandising/ProcessStatusMail/SendMail";

    $.ajax({
        url: url,
        type: "POST",
        data: { "notificationId": recipientId },
        dataType: 'json',
        success: function (data) {
            if (data.Success) {
                alert('Message has been sent !');
            } else {
                alert('Can not send message !');
            }
        }
    });
});

$('.btnSMS').click(function () {

    var recipientId = $(this).closest('td').siblings(':first-child').text();
    var url = "/Merchandising/ProcessStatusMail/SendSMS";

    $.ajax({
        url: url,
        type: "POST",
        data: { "notificationId": recipientId },
        dataType: 'json',
        success: function (data) {
            if (data.Success) {
                alert('Message has been sent !');
            } else {
                alert('Can not send message !');
            }
        }
    });
});


$('.ProcessMail_Buyer').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var buyerId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: buyerId
    };

    ProcessMail.PopulateSpecSheetDropdownList(option);
});

$('.ProcessMail_StyleNo').change(function () {

    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var styleNo = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: styleNo
    };

    ProcessMail.PopulateJobNumberDropdown(option);
});

$('.ProcessMail_JobNumber').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var specificationSheetId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: specificationSheetId
    };

    ProcessMail.PopulateOrderNumberDropdown(option);
});


var ProcessMail = {
    PopulateSpecSheetDropdownList: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { id: option.data },
            success: function (resposns) {
                ProcessMail.ClearDropDownList(option.targetClass);

                if (resposns.Success == true && resposns.SpecificationSheet.length > 0) {

                    for (var i = 0; i < resposns.SpecificationSheet.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.SpecificationSheet[i].StyleNo).attr('value', resposns.SpecificationSheet[i].StyleNo));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }
            }
        });
    },

    PopulateJobNumberDropdown: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { styleNo: option.data },
            success: function (resposns) {
                ProcessMail.ClearDropDownList(option.targetClass);
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

    PopulateOrderNumberDropdown: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { specificationSheetId: option.data },
            success: function (resposns) {
                ProcessMail.ClearDropDownList(option.targetClass);
                if (resposns.Success == true && resposns.orderNumbers.length > 0) {

                    for (var i = 0; i < resposns.orderNumbers.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.orderNumbers[i].OrderNo).attr('value', resposns.orderNumbers[i].OrderId));
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
};


