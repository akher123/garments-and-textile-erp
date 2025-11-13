
var fromDate = '01/01/2014';
var toDate = '31/12/2014';

$(document).ready(function () {
    $("#FromDate, #ToDate").datepicker({
        beforeShow: function () {
            return {
                dateFormat: 'dd/mm/yy',
                minDate: fromDate,
                maxDate: toDate,
            };
        }
    });

    $('#grid tr').find('th:last, td:last').hide();
    $('#FromDate, #ToDate').attr('readonly', 'readonly');
    $('#voucherId').val();   
});


function SetCalenderFixed(Id) { // This is for Calender

    var url = "/BankReconcilationList/GetPeriodFromToDate";

    $.ajax({
        url: url,
        type: "POST",
        data: { "Id": Id },
        dataType: 'json',
        success: function (data) {
            if (data.Success) {
                fromDate = data.FromDate;
                toDate = data.ToDate;
            } else {
                alert('From Date and To Date could not find');
                return false;
            }
        }
    });
}