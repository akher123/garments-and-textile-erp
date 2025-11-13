
$('.Department').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var departmentId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: departmentId
    };

    EmployeeSalarySearch.PopulateEmployeeDropdownList(option);
});


var EmployeeSalarySearch = {
    PopulateEmployeeDropdownList: function(option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { Id: option.data },
            success: function(response) {

                EmployeeSalarySearch.ClearDropDownList(option.targetClass);

                if (response.Success == true && response.EmployeesList.length > 0) {
          

                    for (var i = 0; i < response.EmployeesList.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(response.EmployeesList[i].EmployeeCardId).attr('value', response.EmployeesList[i].EmployeeId));
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
}