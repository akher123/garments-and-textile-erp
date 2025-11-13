$("#EmployeeDocumentUploadFile").uploadFile({
    url: "/HRM/EmployeeDocuments/DocumentFileUpload",
    fileName: "file",
    method: "POST",
    enctype: "multipart/form-data",
    formData: null,
    returnType: null,
    allowedTypes: "*",
    maxFileSize: -1,
    maxFileCount: -1,
    autoSubmit: true,
    showDone: false,
    showDelete: true,
    onSelect: function (files) {
        return true;
    },
    onSubmit: function (files, xhr) {

    },
    onSuccess: function (files, response, xhr, pd) {
        $('#Path').val("");
        $('#Path').val(response.DocumentFilePath);
    },
    onError: function (files, status, message, pd) { },
    onCancel: function (files, pd) { },
    deleteCallback: function (data, pd) {
        EmployeeDocument.DeleteDocumnetFile(data, pd);
    },
});

var EmployeeDocument = {
    DeleteDocumnetFile: function (data, pd) {
        $.ajax({
            url: "/HRM/EmployeeDocuments/DeleteDocumnetFile",
            type: "POST",
            dataType: "JSON",
            data: { filePath: data.DocumentFilePath },
            success: function (respons) {
                if (respons.Success == true) {
                    $('input[type="hidden"][value="' + respons.FilePath + '"]').remove();
                    pd.statusbar.hide();
                } else if (respons.Success == false) {
                    alert("Failed to delete document");
                }
            }
        });
    },
}