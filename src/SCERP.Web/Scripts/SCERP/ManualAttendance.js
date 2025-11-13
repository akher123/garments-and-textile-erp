
function SearchEmployeeData() {

    var employeeCardId = $('.EmployeeCardId').val();
    var dt = $('.InTime').val();

    InOutDropdownList();

    jQuery.ajax({

        url: "/EmployeeManualAttendance/GetEmployeeData"
     , data: { "employeeCardId": employeeCardId, "dt": dt }
     , type: "POST"
     , success: function (r) {
         if (!r.Success) {
             alert("Employee did not found");
         }
         else if (r.Success) {

             $('.EmployeeType').val(r.data[0]);
             $('.EmployeeGrade').val(r.data[1]);
             $('.EmployeeDepartment').val(r.data[2]);
             $('.Designation').val(r.data[3]);
             $('.EmployeeName').val(r.data[4]);
             $('.EmployeeId').val(r.data[5]);
             InTimeSetting(r.data[6]);
             OutTimeSetting(r.data[7]);
             $('.Id').val(r.data[8]);
             $('.Note').val(r.data[9]);

             var InDate = r.data[8];

             if (InDate > 0) {
                 $('.InTime').val(r.data[10]);
                 $('.OutTime').val(r.data[11]);
             }
         }

         else {
             self.DialogOpened();
         }
     }
    });
}

function SaveEmployeeAttendance() {

    var InPeriod = $('.InPeriod option:selected').val();
    var InHours = $('.InHours option:selected').val();
    var InMinutes = $('.InMinutes option:selected').val();

    if (InHours == 'Hour' || InMinutes == 'Minute' || InPeriod == 'AM/PM') {
        alert("Insert a valid In-Time");
        return;
    }

    if (InPeriod == 'PM' & InHours != '12')
        InHours = parseInt(InHours) + 12;

    if (InPeriod == 'AM' & InHours == '12')
        InHours = '00';

    var OutPeriod = $('.OutPeriod option:selected').val();
    var OutHours = $('.OutHours option:selected').val();
    var OutMinutes = $('.OutMinutes option:selected').val();

    var OutTimeValidation = OutHours + OutMinutes + OutPeriod;

    if (OutTimeValidation != 'HourMinuteAM/PM') {
        if (isNaN(OutHours) || isNaN(OutMinutes) || OutPeriod == 'AM/PM') {
            alert("Insert a valid Out-Time");
            return;
        }
    }

    if (OutPeriod == 'PM' & OutHours != '12')
        OutHours = parseInt(OutHours) + 12;

    if (OutPeriod == 'AM' & OutHours == '12')
        OutHours = '00';

    var values = [];
    values[0] = $('.EmployeeCardId').val();
    values[1] = $('.InTime').val();
    values[2] = InHours + ':' + InMinutes + ':00';
    values[3] = OutHours + ':' + OutMinutes + ':00';
    values[4] = $('.Note').val();
    values[5] = $('.EmployeeId').val();
    values[6] = $('.Id').val();
    values[7] = $('.OutTime').val();

    jQuery.ajax({

        contentType: 'application/json; charset=utf-8'
    , dataType: 'json'
    , url: "/EmployeeManualAttendance/SaveEmployeeManualAttendance"
    , data: JSON.stringify(values)
    , type: "POST"
    , success: function (r) {
        if (r.Success) {

            var now = new Date();
            var twoDigitMonth = ((now.getMonth().length + 1) === 1) ? (now.getMonth() + 1) : '0' + (now.getMonth() + 1);
            var currentDate = now.getDate() + "/" + twoDigitMonth + "/" + now.getFullYear();
            $('.InTime').val(currentDate);
            $('.OutTime').val(currentDate);

            InOutDropdownList();

            $('.EmployeeCardId').val('');

            alert(r.Message);
        }
        else {
            self.DialogOpened();
        }
    }
    });
}



function InTimeSetting(InTimeData) {

    if (InTimeData == '00:00:00')
        return;

    if (InTimeData != null)
        var InTime = InTimeData;

    var InHours = InTime.substring(0, 2);
    var Period = 'AM';

    if (InHours > 12) {
        InHours = InHours - 12;
        Period = 'PM';
    }
    else if (InHours == 12) {
        Period = 'PM';
    }
    else if (InHours == 0) {
        InHours = 12;
    }

    if (InHours.toString().length == 1)
        InHours = '0' + InHours;

    var Minutes = InTime.substring(3, 5);

    $('.InHours').val(InHours);
    $('.InMinutes').val(Minutes);
    $('.InPeriod').val(Period);
}

function OutTimeSetting(InTimeData) {

    if (InTimeData == '00:00:00')
        return;

    if (InTimeData != null)
        var InTime = InTimeData;

    var InHours = InTime.substring(0, 2);
    var Period = 'AM';

    if (InHours > 12) {
        InHours = InHours - 12;
        Period = 'PM';
    }
    else if (InHours == 12) {
        Period = 'PM';
    }
    else if (InHours == 0) {
        InHours = 12;
    }

    if (InHours.toString().length == 1)
        InHours = '0' + InHours;

    var Minutes = InTime.substring(3, 5);

    $('.OutHours').val(InHours);
    $('.OutMinutes').val(Minutes);
    $('.OutPeriod').val(Period);
}

function InOutDropdownList() {

    $('.EmployeeId').val('');
    $('.EmployeeType').val('');
    $('.EmployeeGrade').val('');
    $('.EmployeeDepartment').val('');
    $('.Designation').val('');
    $('.EmployeeName').val('');

    $('.InHours').val('Hour');
    $('.InMinutes').val('Minute');
    $('.InPeriod').val('AM/PM');

    $('.OutHours').val('Hour');
    $('.OutMinutes').val('Minute');
    $('.OutPeriod').val('AM/PM');

    $('.Note').val('');
}


