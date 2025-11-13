
$('.EmployeeType').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var typeId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: typeId
    };
    SalarySetup.PopulateSalarySetupDropdownList(option);
});


var SalarySetup = {
    
    PopulateSalarySetupDropdownList: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { Id: option.data },
            success: function (response) {

                SalarySetup.ClearDropDownList(option.targetClass);

                if (response.Success == true && response.gradeList.length > 0) {

                    for (var i = 0; i < response.gradeList.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(response.gradeList[i].Name).attr('value', response.gradeList[i].Id));
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