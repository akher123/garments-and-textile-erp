
    //var TypeId = $('#EmployeeType option:selected').val();
    //getEmpGradeByEmpType(TypeId);



    function getEmpGradeByEmpType(employeeTypeId) {// This is for cascade dropdown 
    var url = "/SalarySetup/GetEmpGradeByEmpType";
    $.ajax({
        url: url,
        type: "get",
        data: { "employeeTypeId": employeeTypeId },
        dataType: 'json',
        success: function (data) {
            if (data.length != 0) {
              
                var items = "<option value='" + 0 + "'>" + " -Select - " + "</option>";

                $.each(data, function (i, employeeGrade) {

                    items += "<option value='" + employeeGrade.Id + "'>" + employeeGrade.Name + "</option>";
                });
                $('.EmpGrade').html(items);
            } else {
                var items = '<option>Not found</option>';
                $('.EmpGrade').html(items);
            }
            var gradeId = $('.HiddenGradeId').val();  
            $('.EmpGrade').val(gradeId);
        }
    });
}


function getEmpGradeByEmpTypeNew(employeeTypeId) {   // This is for cascade dropdown 
    var url = "/SalarySetup/GetEmpGradeByEmpType";
    $.ajax({
        url: url,
        type: "get",
        data: { "employeeTypeId": employeeTypeId },
        dataType: 'json',
        success: function (data) {
            if (data.length != 0) {

                var items = "<option value='" + 0 + "'>" + " -Select - " + "</option>";

                $.each(data, function (i, employeeGrade) {

                    items += "<option value='" + employeeGrade.Id + "'>" + employeeGrade.Name + "</option>";
                });
                $('.EmpGradeNew').html(items);
            } else {
                var items = '<option>Not found</option>';
                $('.EmpGradeNew').html(items);
            }                  
        }
    });
}