$("#uploadFile").uploadFile({
    url: "/Merchandising/TrimsAndAccessoriesDevelopmentDetail/DocumentFileUpload",
    fileName: "file",
    method: "POST",
    enctype: "multipart/form-data",
    formData: null,
    returnType: null,
    allowedTypes: "jpeg,jpg,png,gif",
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
        $('#ImageUrl').val(response.DocumentFilePath);
        $('#trimsAndAccessoriesImageUrl').attr("src", response.DocumentFilePath);
    },
    onError: function (files, status, message, pd) { },
    onCancel: function (files, pd) { },
    deleteCallback: function (data, pd) {
        TrimsAndAccessoriesDevelopmentDetails.DeleteDocumnetFile(data, pd);
    },
});

var TrimsAndAccessoriesDevelopmentDetails = {
    DeleteDocumnetFile: function(data, pd) {
        $.ajax({
            url: "/Merchandising/TrimsAndAccessoriesDevelopmentDetail/DeleteDocumnetFile",
            type: "POST",
            dataType: "JSON",
            data: { filePath: data.DocumentFilePath },
            success: function(respons) {
                if (respons.Success == true) {
                    $('input[type="hidden"][value="' + respons.FilePath + '"]').remove();
                    $('#trimsAndAccessoriesImageUrl').attr("src", "");
                    pd.statusbar.hide();
                } else if (respons.Success == false) {
                    alert("Document not delete");
                }
            }
        });
    },
};

