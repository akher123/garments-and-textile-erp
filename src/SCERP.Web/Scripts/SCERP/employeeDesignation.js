$('.HrmEmployeeTypeId').unbind('change').bind('change', function () {
    var employeeTypeId = $(this).val();
    getEmployeeGradeByEmployeeTypeId(employeeTypeId);
});
function getEmployeeGradeByEmployeeTypeId(employeeTypeId) {
    $('.Hrm_GradeId').html('');
    if (employeeTypeId != "") {
        $.getJSON('/HRM/EmployeeDesignation/GetGradeByEmployeeTypeId', { id: employeeTypeId }).done(function (data) {
            if (data.length != 0) {
                var items = '<option >-Select-</option>';
                $.each(data, function (i, grade) {
                    items += "<option value='" + grade.Id + "'>" + grade.Name + "</option>";
                });
                $('.Hrm_GradeId').html(items);
            } else {
                var items = '<option>Not found</option>';
                $('.Hrm_GradeId').html(items);
            }

        });

    } else {
        var items = '<option>-Select-</option>';
        $('.Hrm_GradeId').html(items);


    }
}