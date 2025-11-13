
$('.PackageInformation_Buyer').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var departmentId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: departmentId
    };

    PackageInformation.PopulateSpecSheetDropdownList(option);
});

$('.PackageInformation_StyleNo').change(function () {

    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var styleNo = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: styleNo
    };

    PackageInformation.PopulateJobNumberDropdown(option);
});

$('.PackageInformation_JobNumber').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var specificationSheetId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: specificationSheetId
    };
    
    PackageInformation.PopulateOrderNumberDropdown(option);
});

$('.PackageInformation_OrderNumber').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var jobNumberId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: jobNumberId
    };

    PackageInformation.PopulateOrderNumberDropdown(option);
});





var PackageInformation = {
   
    PopulateSpecSheetDropdownList: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { id: option.data },
            success: function (resposns) {
                PackageInformation.ClearDropDownList(option.targetClass);
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
                PackageInformation.ClearDropDownList(option.targetClass);
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
                PackageInformation.ClearDropDownList(option.targetClass);
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


