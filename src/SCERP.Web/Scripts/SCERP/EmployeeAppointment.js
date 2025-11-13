
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

$('.employee').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var Id = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: Id
    };

    EmployeeNameSearch.ShowEmployeeName(option);
});

var EmployeeSalarySearch = {
    PopulateEmployeeDropdownList: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { Id: option.data },
            success: function (response) {

                EmployeeSalarySearch.ClearDropDownList(option.targetClass);
                $('.employeeName').val('');

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
    ClearDropDownList: function (selector) {
        $('.' + selector)
            .find('option').remove()
            .end().append($('<option>').text("-select-").attr('value', ""));
    },
};


var EmployeeNameSearch = {
    ShowEmployeeName: function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { Id: option.data },
            success: function (response) {

                $('.' + option.targetClass).val('');

                {
                    $('.' + option.targetClass).val(response.EmployeesList.Name);
                }
            }
        });
    },
}