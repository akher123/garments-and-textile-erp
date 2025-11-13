
function AbsentOTPenalty(reportTypeId) {
    var form = $('.AbsentOTPenaltySearchForm');
    var data = form.serialize();
    var searchurl = '/HRM/Report/AbsentOTPenaltyReport?' + data + '&reportTypeId=' + reportTypeId;
    window.open(searchurl);
}