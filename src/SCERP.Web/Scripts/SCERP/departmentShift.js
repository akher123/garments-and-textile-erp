//$("input[type=button]")
//     .button()
//     .click(function (event) {
//         event.preventDefault();
//     });

function saveDepartmentShif(deparmentId) {
    var checkBoxName = "checkBoxName-" + deparmentId;
    var departmentShifs = [];
    $('input[name="' + checkBoxName + '"]:checked').each(function () {
        var departmentShif = {
            DepartmentId: deparmentId,
            WorkShiftId: this.value
        };
        departmentShifs.push(departmentShif);
    });
    if (departmentShifs.length == 0) {
        var departmentShif = {
            DepartmentId: deparmentId,
            WorkShiftId: 0,
        };
        departmentShifs.push(departmentShif);
    }

    var url = 'DepartmentShift/Save';
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: url,
        data: JSON.stringify(departmentShifs),
        success: function (data) {
            if (data.Success === true) {
                alert("Saved successfully");
            } else if (data.Success === false) {
                alert(data.Errors);
            }
        }
    });
}