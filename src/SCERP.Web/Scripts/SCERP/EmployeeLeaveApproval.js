
$(document).ready(function () {

    var employeeCardId = $('.EmployeeCardId').val();    
    EmployeeData(employeeCardId);
});

function EmployeeData(employeeCardId) {

    if (employeeCardId == '')
        return;

    jQuery.ajax({

        url: "/EmployeeLeave/GetEmployeeData"
     , data: { "employeeCardId": employeeCardId }
     , type: "POST"
     , success: function (r) {

         if (!r.Success) {
             alert("Employee did not found !");
         }
         else if (r.Success) {

             $('.Type').val(r.data[0]);
             $('.Grade').val(r.data[1]);
             $('.Department').val(r.data[2]);
             $('.Designation').val(r.data[3]);
             $('.Name').val(r.data[4]);
             $('.EmployeeId').val(r.data[5]);

             $('#tab').html(r.leavedata);
             $('#SubmitTo').html(r.sub);
         }
         else {
             self.DialogOpened();
         }
     }
    });
}

function SaveApproval() {

    var Id = $('#Id').val();
    var Approvestatus = $('.Approvestatus option:selected').val();
    var Comment = $('.comment').val();

    if (Approvestatus == 'Forward')
        Comment = $('#ForwardToId').val();

    jQuery.ajax({

        url: "/EmployeeLeaveApproval/SaveLeaveApprove"
     , data: { "Id": Id, "Approvestatus": Approvestatus, "Comment": Comment }
     , type: "POST"
     , success: function (r) {

         if (!r.Success) {

             alert("Approve Status did not save !");
         }
         else if (r.Success) {

             window.location.reload();
         }
         else {

             self.DialogOpened();
         }
     }
    });
}