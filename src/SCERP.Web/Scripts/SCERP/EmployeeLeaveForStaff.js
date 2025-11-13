
$(document).ready(function () {

    if ($('#AppliedTotalDays').val() == '')
        $('#AppliedTotalDays').val('1');

    $('.tblStaff').css('visibility', 'hidden');
    $('.tblWorkder').css('visibility', 'hidden');

    SearchEmployeeDataButton();
});

function visiblePanel(leaveFor) {

    if (leaveFor == 'Staff') {

        $('.leaveStatusDisplay').hide();

    } else if (leaveFor == 'Worker') {

        $('.leaveStatusDisplay').show();
    } else {
        $('.leaveStatusDisplay').hide();
    }
}

function VisibleSearchButton(ApplicationType) {

    if (ApplicationType == 'General') {

        $('.Search').css('visibility', 'visible');
        $('#EmployeeCardId').removeAttr('readonly');
        $('#EmployeeCardId').css('background-color', 'transparent');
        ClearEmployeeData();
        $('#tab').hide();
        $('#EmployeeCardId').val('');
    } else if (ApplicationType == 'Personal') {

        $('.Search').css('visibility', 'hidden');
        $('#EmployeeCardId').attr('readonly', 'readonly');
        $('#EmployeeCardId').css('background-color', '#F2F2F2');
        var EmployeeCardId = $('#EmployeeCardHidden').val();

        ClearEmployeeData();

        $('#EmployeeCardId').val(EmployeeCardId);
        $('#LeaveTypeId').val();
        EmployeeData(EmployeeCardId);
    } else {
        ClearEmployeeData();
        $('#tab').hide();
        $('#EmployeeCardId').val('');
        $('#LeaveTypeId').val();
    }
}

function VisibleSubmitTo(LeaveStatus) {

    if (LeaveStatus == 'Forward') {
        $('#Forward').css('visibility', 'visible');
        $('#ForwardToId').css('visibility', 'visible');
    } else {
        $('#Forward').css('visibility', 'hidden');
        $('#ForwardToId').css('visibility', 'hidden');
    }
}

function SearchEmployeeDataButton() {

    var employeeCardId = $('.employeeCardId').val();
    if (employeeCardId == '') return;
    
    var year = $('.year').val();
    if (year == '') return;

    $('#tab').show();
    //ClearEmployeeData();

    jQuery.ajax({
        url: "/LeaveApplicationOfSelf/GetEmployeeData",
        data: {
            "employeeCardId": employeeCardId,
            "year": year
        },
        type: "POST",
        success: function (r) {

            if (!r.Success) {
                $('#tab').hide();
                alert("Employee can not be found !");
            } else if (r.Success) {

                $('.Name').val(r.data[0]);
                $('.Company').val(r.data[1]);
                $('.Branch').val(r.data[2]);
                $('.Unit').val(r.data[3]);
                $('.Department').val(r.data[4]);
                $('.Designation').val(r.data[5]);
                $('.Grade').val(r.data[6]);
                $('.Line').val(r.data[7]);
                $('.Section').val(r.data[8]);
                $('.Type').val(r.data[9]);
                $('#AddressDuringLeave').val(r.data[10]);
                $('#EmergencyPhoneNo').val(r.data[11]);
                $('.JoiningDate').val(r.data[12]);
                $('#tab').html(r.leavedata);
                $('#tab').show();
                $('#SubmitTo').html(r.sub);
            } else {
                self.DialogOpened();
            }
        }
    });
}

function SearchEmployeeDataHistory() {

    var employeeCardId = $('.EmployeeCardId').val();
    if (employeeCardId == '')
        return;

    $('#tab').show();
    $('#EmployeeCardId').removeAttr('readonly');
    $('#EmployeeCardId').css('background-color', 'transparent');

    jQuery.ajax({
        url: "/EmployeeLeave/GetEmployeeData",
        data: { "employeeCardId": employeeCardId },
        type: "POST",
        success: function (r) {

            if (!r.Success) {
                alert("Employee did not found !");
            } else if (r.Success) {

                $('.Type').val(r.data[0]);
                $('.Grade').val(r.data[1]);
                $('.Department').val(r.data[2]);
                $('.Designation').val(r.data[3]);
                $('.Name').val(r.data[4]);
                $('.EmployeeId').val(r.data[5]);

                $('#tab').html(r.leavedata);

                $('#SubmitTo').html(r.sub);
            } else {
                self.DialogOpened();
            }
        }
    });
}

function EmployeeData(employeeCardId) {

    if (employeeCardId == '')
        return;

    ClearEmployeeData();

    $('#tab').show();

    jQuery.ajax({
        url: "/EmployeeLeave/GetEmployeeData",
        data: { "employeeCardId": employeeCardId },
        type: "POST",
        success: function (r) {

            if (!r.Success) {
                alert("Employee did not found !");
            } else if (r.Success) {

                $('.Type').val(r.data[0]);
                $('.Grade').val(r.data[1]);
                $('.Department').val(r.data[2]);
                $('.Designation').val(r.data[3]);
                $('.Name').val(r.data[4]);
                $('.EmployeeId').val(r.data[5]);

                $('#tab').html(r.leavedata);
                $('#SubmitTo').html(r.sub);
            } else {
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
    $('#AddressDuringLeave').val('');
    $('#EmergencyPhoneNo').val('');
    $('#LeavePurpose').val('');

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    }

    var today = dd + '/' + mm + '/' + yyyy;
    $('#FromDate').val(today);
    $('#ToDate').val(today);
    $('#TotalDays').val(1);

    $('#SubmitTo').empty();
    var newOption = $('<option value="0">- Submit To -</option>');
    $('#SubmitTo').append(newOption);
}

function DayCalculation() {

    var fromDate = $('#AppliedFromDate').val();
    var ToDate = $('#AppliedToDate').val();

    fromDate = fromDate.substring(3, 5) + '/' + fromDate.substring(0, 2) + '/' + fromDate.substring(6, 10);
    ToDate = ToDate.substring(3, 5) + '/' + ToDate.substring(0, 2) + '/' + ToDate.substring(6, 10);
    var dateDiff = ((new Date(ToDate) - new Date(fromDate)) / 86400000) + 1;

    if (dateDiff < 1) {
        $('#AppliedTotalDays').val('');
        alert("Please select valid From Date and To Date");
        return;
    }

    $('#AppliedTotalDays').val(dateDiff);
}

function AuthorDayCalculation() {

    var fromDate = $('#AuthorizedFromDate').val();
    var ToDate = $('#AuthorizedToDate').val();

    fromDate = fromDate.substring(3, 5) + '/' + fromDate.substring(0, 2) + '/' + fromDate.substring(6, 10);
    ToDate = ToDate.substring(3, 5) + '/' + ToDate.substring(0, 2) + '/' + ToDate.substring(6, 10);
    var dateDiff = ((new Date(ToDate) - new Date(fromDate)) / 86400000) + 1;

    if (dateDiff < 1) {
        $('#AuthorizedTotalDays').val('');
        alert("Please select valid From Date and To Date");
        return;
    }

    $('#AuthorizedTotalDays').val(dateDiff);
}

function ResumeDateCalculation() {


    //var joinBefore = parseInt($('#JoinBefore').val());
    //var authorToDate = $('#AuthorizedToDate').val();

    //authorToDate = authorToDate.substring(3, 5) + '/' + authorToDate.substring(0, 2) + '/' + authorToDate.substring(6, 10);

    //var newDate = new Date(authorToDate);
    //newDate.setDate(newDate.getDate() - joinBefore + 1);

    //var dd = newDate.getDate();
    //var mm = newDate.getMonth() + 1;
    //var y = newDate.getFullYear();

    //var someFormattedDate = dd + '/' + mm + '/' + y;

    //$('#ResumeDate').val(someFormattedDate);
}
