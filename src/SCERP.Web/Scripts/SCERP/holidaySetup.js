function InitializeHolidaySetup() {
   // holidaySetup();
    $(".holidaySetUp-save").unbind("click").bind("click", function (e, item) {
        saveHoliday();
    });
    $("#weekends-update").unbind("click").bind("click", function (e, item) {
        updateWeekends();
    });
    $(".weekends-day").unbind("change").bind("change", function (e, item) {
        if ($(this).is(":checked")) {
            $('#weekends-update').val('Update');
        }

    });
}

function weekendsDay() {
    $('.weekends-day').each(function () {
        var dayName = $(this).attr('name');
        $('.fc-day.fc-' + dayName + '').css('color', 'green');
     
    });

    $('.weekends-day:checked').each(function () {
        var dayName = $(this).attr('name');
        $('.fc-day.fc-' + dayName + '').css('color', 'red');
      
    });
}


  



