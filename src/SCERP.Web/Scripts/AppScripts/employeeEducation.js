$(document).ready(function (evt) {
    $('#dialog-confirm').hide();
});

function loadEmployeeEducationCreateForm() {
    var url = '/EmployeeEducation/Create';
    $('#formContainnerTitle').text(' Create Education ');
    $('#formContainner').load(url);
    $('#formContainnerModal').modal('show');
}

function loadEmployeeEducationEditForem(educationId) {
    var url = '/EmployeeEducation/Edit';
    $('#formContainnerTitle').text(' Edit Education ');
    $.get(url, { id: educationId }, function (resposns, evt) {
        $('#formContainner').empty();
        $('#formContainner').append(resposns);
        $('#formContainnerModal').modal('show');
        evt.preventDefault();
    });

}


function editEmployeEeducation() {
    var employeeeducation = {
        "Id": $('#educationId').val(),
        "EmployeeId": $('#EmployeeId').val(),
        "EducationLevelId": $('#EducationLevelId').val(),
        "ExamTitle": $('#ExamTitle').val(),
        "Institute": $('#Institute').val(),
        "Result": $('#Result').val(),
        "PassingYear": $('#PassingYear').val(),
    };
    $.ajax({
        url: '/EmployeeEducation/EditEducation',
        type: "POST",
        data: employeeeducation,
        dataType: 'json',
        success: function (data, evt) {

            var tr = $('<tr  id="' + data.Id + '" >');

            tr.append('<td>' + data.EducationLevelTitle + '</td>');
            tr.append('<td>' + data.ExamTitle + '</td>');
            tr.append('<td>' + data.Institute + '</td>');
            tr.append('<td>' + data.Result + '</td>');
            tr.append('<td>' + data.PassingYear + '</td>');
            tr.append('<td><div class="btn btn-link" onClick="loadEmployeeEducationEditForem(' + data.Id + ')">Edit</div></td>');
            tr.append('<td><div  class="btn btn-link" onClick="loadEmployeeEducationDeleteForem(' + data.Id + ')">Delete</div></td>')
            $('table#employeeEducationTable tr#' + data.Id + '').replaceWith(tr);
            $('#formContainnerModal').modal('hide');
            evt.preventDefault();
        }
    });

}

function loadEmployeeEducationDeleteForem(educationId) {
    $('#dialog-confirm').show();
    var url = '/EmployeeEducation/Delete';
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 140,
        width: 400,
        modal: true,
        buttons: {
            "Yes": function () {
                $(this).dialog("close");
                $.getJSON(url, { id: educationId }, function (rowId) {
                    $('table#employeeEducationTable tr#' + rowId + '').remove();
                });
            },
            "No": function () {
                $(this).dialog("close");
            }
        }
    });
}

function loadEmployeeEducationCreateForm() {
    var url = '/EmployeeEducation/Create';
    $('#formContainnerTitle').text(' Create Education ');
    $.get(url, null, function (resposns, evt) {
        $('#formContainner').empty();
        $('#formContainner').append(resposns);
        $('#formContainnerModal').modal('show');
        evt.preventDefault();
    });
}
function saveEmployeeEducation() {
    var employeeeducation = {
        "EmployeeId": $('#EmployeeId').val(),
        "EducationLevelId": $('#EducationLevelId').val(),
        "ExamTitle": $('#ExamTitle').val(),
        "Institute": $('#Institute').val(),
        "Result": $('#Result').val(),
        "PassingYear": $('#PassingYear').val(),
    };
    $.ajax({
        url: '/EmployeeEducation/CreateEducation',
        type: "POST",
        data: employeeeducation,
        dataType: 'json',
        success: function (data) {

            var tr = $('<tr  id="' + data.Id + '" >');
            tr.append('<td>' + data.EducationLevelTitle + '</td>');
            tr.append('<td>' + data.ExamTitle + '</td>');
            tr.append('<td>' + data.Institute + '</td>');
            tr.append('<td>' + data.Result + '</td>');
            tr.append('<td>' + data.PassingYear + '</td>');
            tr.append('<td><div class="btn btn-link" onClick="loadEmployeeEducationEditForem(' + data.Id + ')">Edit</div></td>');
            tr.append('<td><div  class="btn btn-link" onClick="loadEmployeeEducationDeleteForem(' + data.Id + ')">Delete</div></td>')
            $('#employeeEducationTable').append(tr);
            $('#formContainnerModal').modal('hide');
        }

    });
}
  $(function () {
            $("#tabs").tabs({
                collapsible: true,
                beforeLoad: function (event, ui) {
                    ui.jqXHR.error(function () {
                        ui.panel.html(
                            "Couldn't load this tab. We'll try to fix this as soon as possible. " +
                                "If this wouldn't be a demo.");
                    });
                }
            });
        });
        function loadEmployeeBankInfoEditForem() {
            $('#employeeBankInfoPopupId').modal('show');
        }
        function editEmployeeBankInfo() {
            var employeeBankInfo = {
                "Id": $('#Id').val(),
                "BankName": $('#BankName').val(),
                "BranchName": $('#BranchName').val(),
                "AccountType": $("#AccountType").val(),
                "AccountName": $('#AccountName').val(),
                "AccountNumber": $('#AccountNumber').val()
            };
            var url = '/EmployeeBankInfo/Edit';
            $.ajax({
                type: 'POST',
                url: url,
                dataType: 'json',
                data: employeeBankInfo,
                success: function (data) {
                    $('#bankName').text(data.employeeBankInfoObj.BankName);
                    $('#branchName').text(data.employeeBankInfoObj.BranchName);
                    $('#accountType').text(data.accountType);
                    $('#accountName').text(data.employeeBankInfoObj.AccountName);
                    $('#accountNumber').text(data.employeeBankInfoObj.AccountNumber);
                }
            });
        }