
//$(document).ready(function () {

    var monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];

    var d = new Date();
    var month = monthNames[d.getMonth()];

    $('.Month').val(month);
    $('.Year').val('2014');

//});


function ProcessSalary() {

    var month = $('.Month option:selected').val();
    var year = $('.Year option:selected').val();

    jQuery.ajax({

        url: "/EmployeeSalaryProcess/ProcessAll"
     , data: { "Month": month, "Year": year }
     , type: "POST"
     , success: function (r) {
         if (r.Success) {
             loadAction(r.RedirectTo);
             //window.location.href = r.RedirectTo;
         }
         else {
             self.DialogOpened();
         }
     }
    });
}

function ConfirmSalary() {

    jQuery.ajax({

        url: "/EmployeeSalaryProcess/ConfirmSalary"
     , type: "POST"
     , success: function (r) {

         if (r.Success) {

             alert("Data Save Successfully !");
         }

         else {
             self.DialogOpened();
         }
     }
    });
}