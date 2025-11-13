$('#lineDropDown').unbind('change').bind('change', function (e) {
    jQuery.Ajax({
        url: '/TargetProduction/TargetDuration/',
        type: "GET",
        data: $('.TargetProductionform').serialize()
    }).done(function (target) {
        $('#startDateTextBox').val(ConVetDate(target.StartDate));
        $('#endDateTextBox').val(ConVetDate(target.EndDate));
    });


});

function ConVetDate(date) {
    var dateString = date.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    return day + "/" + month + "/" + year;
}

$('#showTrgProDtailButton').unbind('click').bind('click', function (e) {
    var form = $(this).parents('form:first');
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {
            e.preventDefault();
            return false;
        } else {
            jQuery.Ajax({
                url: '/TargetProduction/PalnedTagetProductionDetail/',
                type: "GET",
                data: $('.TargetProductionform').serialize(),
                container: '#targetDetailContainer'
            });
        }
    }
    e.preventDefault();
    return false;

});