$('.ModuleId').unbind('change').bind('change', function () {
    var moduleId = $(this).val();
    $.Ajax({
        url: '/Task/GetSubjectsByModelId/',
        data: { ModuleId: moduleId }
    }).done(function (subjectList) {
        $('.SubjectId').empty();
        if (subjectList.length > 0) {
            populateDropDown({ SubjectId: "", SubjectName: "Select" });
            $.each(subjectList, function (order) {
                populateDropDown({ SubjectId: this.SubjectId, SubjectName: this.SubjectName });
            });
        } else {
            populateDropDown({ SubjectId:"", SubjectName: "No FeatureName Found" });
        }
    });
});
function populateDropDown(obj) {
    $('.SubjectId').append(
        $('<option/>')
            .attr('value', obj.SubjectId)
            .text(obj.SubjectName)
    );
}




function removeDependentDropdown() {
    $('.SubjectId').empty();
    //$('.SewingInputProcess_OrderNo').empty();
    //$('.SewingInputProcess_StyleNo').empty();
    //$('.SewingInputProcess_Color').empty();
}

