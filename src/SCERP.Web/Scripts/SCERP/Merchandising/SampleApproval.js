

$('.SampleApproval_JobNumber').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var jobNumberId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: jobNumberId
    };

    SampleApproval.PopulateSampleTypeDropdownList(option);
});

$('.SampleApproval_Buyer').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var departmentId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: departmentId
    };

    SampleApproval.PopulateSpecSheetDropdownList(option);
});

$('.SampleApproval_StyleNo').change(function () {

    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var styleNo = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: styleNo
    };

    SampleApproval.PopulateJobNumberDropdown(option);
});

$('.SampleApproval_department').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var departmentId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: departmentId
    };

    SampleApproval.PopulateEmployeesDropdownList(option);
});






var SampleApproval = {

    PopulateSampleTypeDropdownList: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { jobNumberId: option.data },
            success: function (resposns) {
                SampleApproval.ClearDropDownList(option.targetClass);
                if (resposns.Success == true && resposns.DevelopmentSamples.length > 0) {

                    for (var i = 0; i < resposns.DevelopmentSamples.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.DevelopmentSamples[i].SampleName).attr('value', resposns.DevelopmentSamples[i].SampleTypeId));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }

            }
        });
    },
    PopulateEmployeesDropdownList: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { id: option.data },
            success: function (resposns) {
                SampleApproval.ClearDropDownList(option.targetClass);
                if (resposns.Success == true && resposns.EmployeesList.length > 0) {

                    for (var i = 0; i < resposns.EmployeesList.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.EmployeesList[i].Name).attr('value', resposns.EmployeesList[i].EmployeeId));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }

            }
        });
    },
    PopulateSpecSheetDropdownList: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { id: option.data },
            success: function (resposns) {
                SampleApproval.ClearDropDownList(option.targetClass);
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
                SampleApproval.ClearDropDownList(option.targetClass);
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
    ClearDropDownList: function (selector) {
        $('.' + selector)
            .find('option').remove()
            .end().append($('<option>').text("-select-").attr('value', ""));
    },
};


