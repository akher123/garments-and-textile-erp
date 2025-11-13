function InitializeBasicInfo() {

}

function InitializePersonalinfo() { }

function InitializeAddress() { }

function InitializeEducation() { }

function InitializeEmployment() { }

function InitializeBankInfo() { }

function InitializeSalaryInfo() {

}

function InitializeEntitlements() {

    GetEntitlementValues();

    $(".document-save").unbind("click").bind("click", function (e, item) {
        SaveEntitlementValues();
    });

}

function InitializeDocumetinfo() {

    $(".document-save").unbind("click").bind("click", function (e, item) {
        SaveDocumentinfo();
    });
}

function getGradeByEmployeeTypeIed(employeeTypeId) {
    $('#EmployeeDetail_EmployeeGradeId').html('');
    if (employeeTypeId != "") {
        $.getJSON('/Employees/GetGradeByGradeId', { id: employeeTypeId }).done(function (data) {
            if (data.length != 0) {
                var items = '<option >-Select-</option>';
                $.each(data, function (i, designation) {
                    items += "<option value='" + designation.Id + "'>" + designation.Name + "</option>";
                });
                $('#EmployeeDetail_EmployeeGradeId').html(items);
            } else {
                var items = '<option>Not found</option>';
                $('#EmployeeDetail_EmployeeDesignationId').html(items);
            }

        });

    } else {

        $('#EmployeeDetail_EmployeeGradeId option').remove();
        $('#EmployeeDetail_EmployeeDesignationId option').remove();
    }
}

function getDesignationByGradId(gradeId) {
    $('#EmployeeDetail_EmployeeDesignationId option').remove();
    if (gradeId != "") {
        $.getJSON('/Employees/GetDesignationByGradeId', { id: gradeId }).done(function (data) {
            if (data.length != 0) {
                var items = '<option>-Select-</option>';
                $.each(data, function (i, designation) {
                    items += "<option value='" + designation.Id + "'>" + designation.Title + "</option>";
                });
                $('#EmployeeDetail_EmployeeDesignationId').html(items);
            } else {
                var items = '<option>Not found</option>';
                $('#EmployeeDetail_EmployeeDesignationId').html(items);
            }

        });
    } else {
        var items = '<option>-Select-</option>';
        $('#EmployeeDetail_EmployeeDesignationId').html(items);
    }
}

//Entitlement 
function GetEntitlementValues() {

    jQuery.Ajax({
        url: "/EmployeeEntitlement/GetEmployeeEntitlementsValue"
    , data: null
    , type: "POST"
    , success: function (r) {
        if (r.Success) {

            var num = $('.modelcount').val();

            for (var i = 0; i < num; i++) {

                for (var j = 0; j < num; j++) {

                    if ($('.TestClass' + i).val() == r.data[j])

                        $('#CheckBox-' + i).prop('checked', true);
                }
            }
        }
        else {
            self.DialogOpened();
        }
    }
    });
}

function SaveEntitlementValues() {

    var i;
    var count = 0;
    var rows = $('.modelcount').val();
    var Status = [];

    for (i = 0; i < rows; i++) {

        var temp = $('#CheckBox-' + i).prop('checked');

        if (temp) {

            Status[count] = $('.TestClass' + i).val();
            count++;
        }
    }

    jQuery.ajax({
        contentType: 'application/json; charset=utf-8'
      , dataType: 'json'
      , url: "/EmployeeEntitlement/SaveEmployeeEntitlement"
      , data: JSON.stringify(Status)
      , type: "POST"
      , success: function (r) {
          if (r.Success) {
              alert('Data save successfully !');
          }
          else {
              self.DialogOpened();
          }
      }
    });
}

// Employee document
function SaveDocumentinfo() {
    var button = $(".document-save");
    var form = button.parents("form:first");

    var filepath = $('.uploadfilepath').val();
    var title = $('.grouptitle').val();
    var description = $('.uploader-description').val();

    var inputdata = { filepath: filepath, title: title, description: description };
    jQuery.Ajax({
        url: "/EmployeeDocuments/Save"
       , data: inputdata
       , type: "POST"
       , success: function (r) {
           if (r.Success) {
               button.DialogClose();
               var action = button.attr("action");
               var container = form;

               jQuery.Ajax({
                   url: action
                   , container: container
               });
           }
           else {
               self.DialogOpened();
           }
       }
    });
}

function uploadFile() {

    var form = new FormData(document.getElementById('imageform'));
    var file = document.getElementById('photoimg').files[0];
    if (file) {
        form.append('photoimg', file);
    }
    $.ajax({
        url: "/Employees/Upload",
        type: 'POST',
        data: form,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.Success === true) {
                $('#imageContainer').html('');
                $('<img src="' + data.url + '" />').appendTo('#imageContainer');
            } else if (data.Success === true) {
                alert(data.Message);
            } else {
                alert(data.Message);
            }
        },
        error: function () {
            alert("ERROR");
        }
    });

}

function getDesignationByEmployeeTypeId(employeeTypeId) {
    $('#employeeDesignationId option').remove();
    if (employeeTypeId != null) {
        $.getJSON('/EmployeeManualAllowances/GetDesignationByEmployeeTypeId', { id: employeeTypeId }).done(function (data) {
            if (data.length > 0) {
                var items = '<option>-Select-</option>';
                $.each(data, function (i, designation) {
                    items += "<option value='" + designation.Id + "'>" + designation.Title + "</option>";
                });
                $('#employeeDesignationId').html(items);
            } else {
                var items = '<option>Not found</option>';
                $('#employeeDesignationId').html(items);
            }

        });
    } else {
        var items = '<option>-Select-</option>';
        $('#employeeDesignationId').html(items);
    }
}