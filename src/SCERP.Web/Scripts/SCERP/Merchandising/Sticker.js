
$('.Sticker_Buyer').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var buyerId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: buyerId
    };

    OrderInformation.PopulateSpecSheetDropdownList(option);
});

$('.Sticker_StyleNo').change(function () {

    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var styleNo = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: styleNo
    };

    OrderInformation.PopulateJobNumberDropdown(option);
});

$('.Sticker_JobNumber').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var specificationSheetId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: specificationSheetId
    };

    OrderInformation.PopulateOrderNumberDropdown(option);
});


var OrderInformation = {
    PopulateSpecSheetDropdownList: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { id: option.data },
            success: function (resposns) {
                OrderInformation.ClearDropDownList(option.targetClass);

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
                OrderInformation.ClearDropDownList(option.targetClass);
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
                OrderInformation.ClearDropDownList(option.targetClass);
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








$('.Sticker_OrderNumber').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var orderId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: orderId
    };

    sticker.PopulatePackageDropdownList(option);
});


var sticker = {
    PopulatePackageDropdownList: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { orderId: option.data },
            success: function(resposns) {
                sticker.ClearDropDownList(option.targetClass);
                if (resposns.Success == true && resposns.package.length > 0) {

                    for (var i = 0; i < resposns.package.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.package[i].PackageName).attr('value', resposns.package[i].PackageInfoId));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }

            }
        });
    },

    ClearDropDownList: function(selector) {
        $('.' + selector)
            .find('option').remove()
            .end().append($('<option>').text("-select-").attr('value', ""));
    },
};


