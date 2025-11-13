function InitializeDocumetinfo() {
    $(".document-save").unbind("click").bind("click", function (e, item) {
        SaveDocumentinfo();
    });
}

function SaveDocumentinfo() {
    var button = $(".document-save");
    var form = button.parents("form:first");

    var filepath = 'mah';
    var title= $('.grouptitle').val();
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

