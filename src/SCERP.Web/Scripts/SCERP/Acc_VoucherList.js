
$('#FromDate, #ToDate').attr('readonly', 'readonly');
//$(document).on('click', '.search', function () {
//    var fpId = $('#FpId option:selected').val();
//    var fromDate = $('#FromDate').val();
//    var toDate = $('#ToDate').val();
//});

var data = CatchFromToDateTime.GetDateTime(); //menu.js
if (data.Success) {
    $("#FromDate,#ToDate").datepicker({
        beforeShow: function () {
            return {
                dateFormat: 'dd/mm/yy',
                minDate: data.FromDate,
                maxDate: data.ToDate,
            };
        }
    });
}

function SetCalenderFixed(Id) {
    if (Id.length != 0) {
        $('#FromDate,#ToDate').datepicker("destroy");
        var url = "/VoucherList/GetPeriodFromToDate";
        $.ajax({
            url: url,
            type: "POST",
            data: { "Id": Id },
            dataType: 'json',
            success: function (data) {
                Report.PopulateDatePicker(data);
            }
        });
    } else {
        $('#FromDate,#ToDate').datepicker("destroy");
        $('#FromDate,#ToDate').val('');
    }
}

function SetCalenderFixedBalanceSheet(Id) {
    if (Id.length != 0) {
        $('#FromDate,#ToDate').datepicker("destroy");
        var url = "/VoucherList/GetPeriodFromToDateBalanceSheet";
        $.ajax({
            url: url,
            type: "POST",
            data: { "Id": Id },
            dataType: 'json',
            success: function (data) {
                Report.PopulateDatePicker(data);
            }
        });
    } else {
        $('#FromDate,#ToDate').datepicker("destroy");
        $('#FromDate,#ToDate').val('');
    }
}


var Report = {
    PopulateDatePicker: function (data) {
        if (data.Success) {
            CatchFromToDateTime.SetDateTime(data);
            $("#FromDate").val(data.FromDate),
            $("#ToDate").val(data.ToDate),
            $("#FromDate,#ToDate").datepicker({
                beforeShow: function () {
                    return {
                        dateFormat: 'dd/mm/yy',
                        //minDate: data.FromDate,
                        //maxDate: data.ToDate,
                    };
                }
            });
        } else {
            alert('From Date and To Date could not find');
            return false;
        }
        return false;
    },

};


