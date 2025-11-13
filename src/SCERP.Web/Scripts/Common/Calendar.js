
    //$('.datepicker').each(function () {
    //    $(this).datepicker({ dateFormat: 'dd-MM-yyyy' }).val();
//});

$(document).ready(function() {
    $('.timePicker').timepicker({
        showPeriod: true,
        showLeadingZero: true
    });

    $(".datepicker").datepicker({
        dateFormat: 'dd/mm/yy',
        yearRange: '1920:' + (new Date().getFullYear() + 5),
        changeMonth: true,
        changeYear: true,
    });
    
});
    
