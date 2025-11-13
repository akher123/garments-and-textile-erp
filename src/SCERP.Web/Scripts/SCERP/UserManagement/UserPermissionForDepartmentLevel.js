
$('li :checkbox').unbind('click').bind('click', getCheckBoxSelection);
$('.UserPermissionForDepartmentLevel').unbind("change").bind("change", changeEventHendeler);
function changeEventHendeler() {
    var $this = $(this);
    var option = {
        url: $this.attr('action'),
        type: 'get',
        data: { UserName: $this.val() }
    };
    $.ajax(option).done(function (response) {
        if (response.length > 0) {
            uncheckedCheckBox();
            $.each(response, function (i, userPermissionSelector) {
                checkCheckBox(userPermissionSelector);
            });
        } else {
            uncheckedCheckBox();
        }

    });
}

function checkCheckBox(data) {
    $('#check-' + data.CompanySelector).prop('checked', true);
    $('#check-' + data.BranchSelector).prop('checked', true);
    $('#check-' + data.UnitSelector).prop('checked', true);
    $('#check-' + data.DepartmentSelector).prop('checked', true);
}

function uncheckedCheckBox() {
    $("input:checkbox").prop('checked', false);
}
function getCheckBoxSelection() {
    var $chk = $(this),
      $li = $chk.closest('li'),
      $ul, $parent;
    if ($li.has('ul')) {
        $li.find(':checkbox').not(this).prop('checked', this.checked);
    }
    do {
        $ul = $li.parent();
        $parent = $ul.siblings(':checkbox');
        if ($chk.is(':checked')) {
            $parent.prop('checked', $ul.has(':checkbox:not(:checked)').length == 0);
        } else {
            $parent.prop('checked', false);
        }
        $chk = $parent;
        $li = $chk.closest('li');
    } while ($ul.is(':not(.check)'));
}
