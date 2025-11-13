
function InitializeSalarySetup()
{  
    $(".document-save").unbind("click").bind("click", function (e, item)
    {
        SaveSalaryHeadValues();
    });
}


function SaveSalaryHeadValues()
{
    var button = $('.document-save');
    var form = button.parents('form:first');
    var rows = $('.modelcount').val();
    var employeeType = $('#EmployeeType').val();
    var employeeGrade = $('#empGrade').val();

    var HeadId = [];
    var Amount = [];

    for (var i = 0; i < rows; i++) {
        HeadId[i] = $('.HeadId-' + i).val();
        Amount[i] = $('.SalaryHead-' + i).val();
    }

    jQuery.ajax({

        contentType: 'application/json; charset=utf-8'
       , dataType: 'json'
       , url: '/SalarySetup/SaveSalarySetup'
       , data: JSON.stringify({
           SalaryHeads: HeadId,
           Amounts: Amount,
           employeeTypeId: employeeType,
           employeeGradeId: employeeGrade
       })
       , type: 'POST'
       , success: function (r) {
           if (r.Success) {
               location.reload(true);
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