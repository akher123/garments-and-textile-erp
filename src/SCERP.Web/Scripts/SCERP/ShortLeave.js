
$(document).ready(function () {
    
    SearchEmployeeDataButton();
});


function SearchEmployeeDataButton() {

    var employeeCardId = $('.employeeCardId').val();
    if (employeeCardId == '')
        return;

    jQuery.ajax({
        //url: "/LeaveApplicationForWorker/GetEmployeeData",      
        url: "/ShortLeave/GetEmployeeDetailByEmployeeCadId",
        data: { "employeeCardId": employeeCardId },
        type: "POST",
        success: function(r) {

            if (!r.Success) {
                alert("Access denied Or Employee not found !");
            } else if (r.Success) {

                $('.Name').val(r.data["EmployeeName"]);
                $('.Company').val(r.data["CompanyName"]);
                $('.Branch').val(r.data["BranchName"]);
                $('.Unit').val(r.data["UnitName"]);
                $('.Department').val(r.data["DepartmentName"]);
                $('.Section').val(r.data["SectionName"]);
                $('.Line').val(r.data["LineName"]);
                $('.Type').val(r.data["EmployeeTypeTitle"]);
                $('.Grade').val(r.data["EmployeeGradeName"]);
                $('.Designation').val(r.data["Designation"]);
                $('#SubmitTo').html(r.sub);
            } else {
                self.DialogOpened();
            }
        }
    });
}


