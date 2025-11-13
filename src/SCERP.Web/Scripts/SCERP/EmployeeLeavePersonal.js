
//$(document).ready(function () {

    if ($('#TotalDays').val() == '')
        $('#TotalDays').val('1');

    var temp = $('#Id').val();

    if (temp == '0')
        EmployeeDataNew();
    else
        EmployeeDataEdit();
//});

function EmployeeDataEdit() {

    var employeeCardId = $('.EmployeeCardId').val();
    if (employeeCardId == '')
        return;

    ClearEmployeeData();

    jQuery.ajax({

        url: "/EmployeeLeavePersonal/GetEmployeeData"
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
         }
         else {
             self.DialogOpened();
         }
     }
    });
}

function EmployeeDataNew() {

    var employeeCardId = $('.EmployeeCardId').val();
    if (employeeCardId == '')
        return;

    ClearEmployeeData();

    jQuery.ajax({

        url: "/EmployeeLeavePersonal/GetEmployeeData"
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

function ClearEmployeeData() {

    $('.Type').val('');
    $('.Grade').val('');
    $('.Department').val('');
    $('.Designation').val('');
    $('.Name').val('');
    $('.EmployeeId').val('');
}

function DayCalculation() {

    var fromDate = $('#FromDate').val();
    var ToDate = $('#ToDate').val();

    fromDate = fromDate.substring(3, 5) + '/' + fromDate.substring(0, 2) + '/' + fromDate.substring(6, 10);
    ToDate = ToDate.substring(3, 5) + '/' + ToDate.substring(0, 2) + '/' + ToDate.substring(6, 10);
    var dateDiff = ((new Date(ToDate) - new Date(fromDate)) / 86400000) + 1;

    if (dateDiff < 1) {
        $('#TotalDays').val('');
        alert("Please select valid From Date and To Date");
        return;
    }

    $('#TotalDays').val(dateDiff);
}

function SaveApproval() {

    var Id = $('#Id').val();
    var Approvestatus = $('.Approvestatus option:selected').val();
    var Comment = $('.comment').val();

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