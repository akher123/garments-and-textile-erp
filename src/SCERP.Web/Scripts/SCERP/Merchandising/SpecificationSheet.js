
$("#DocumentFileuploader").uploadFile({
    url: "/Merchandising/SpecificationSheet/DocumentFileUpload",
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
        $('.formSpecificationSheet').append('<input type="hidden" name="DocumentFilePath" value="' + response.DocumentFilePath + '" class="DocumentFilePath-' + response.DocumentFilePath + '"/>');

    },
    onError: function (files, status, message, pd) { },
    onCancel: function (files, pd) { },
    deleteCallback: function (data, pd) {
        SpecificationSheet.DeleteDocumnetFile(data, pd);
    },

});
$("#ImageFileUploader").uploadFile({
    url: "/Merchandising/SpecificationSheet/ImageFileUpload", // Server URL which handles File uploads
    method: "POST",
    enctype: "multipart/form-data",
    formData: null,
    returnType: null,
    allowedTypes: "*",
    fileName: "file",
    formData: {},
    dynamicFormData: function () {
        return {};
    },
    maxFileSize: -1,
    maxFileCount: -1,
    multiple: true,
    dragDrop: true,
    autoSubmit: true,
    showCancel: true,
    showAbort: true,
    showDone: false,
    showDelete: true,
    showError: true,
    showStatusAfterSuccess: true,
    showStatusAfterError: true,
    showFileCounter: true,
    fileCounterStyle: "). ",
    showProgress: true,
    nestedForms: true,
    showDownload: false,
    onLoad: function (obj) { },
    onSelect: function (files) {
        return true;
    },
    onSubmit: function (files, xhr) {

    },
    onSuccess: function (files, response, xhr, pd) {
        $('.formSpecificationSheet').append('<input type="hidden" name="ImageFilePath" value="' + response.ImageFilePath + '" class="ImageFilePath-' + response.ImageFilePath + '"/>');
    },
    onError: function (files, status, message, pd) { },
    onCancel: function (files, pd) { },
    deleteCallback: function (data, pd) {
        SpecificationSheet.DeleteImageFile(data, pd);
    },
    downloadCallback: true,
    afterUploadAll: false,
    uploadButtonClass: "ajax-file-upload",
    dragDropStr: "<span><b>Drag &amp; Drop Files</b></span>",
    abortStr: "Abort",
    cancelStr: "Cancel",
    deletelStr: "Delete",
    doneStr: "Done",
    multiDragErrorStr: "Multiple File Drag &amp; Drop is not allowed.",
    extErrorStr: "is not allowed. Allowed extensions: ",
    sizeErrorStr: "is not allowed. Allowed Max size: ",
    uploadErrorStr: "Upload is not allowed",
    maxFileCountErrorStr: " is not allowed. Maximum allowed files are:",
    downloadStr: "Download",
    showQueueDiv: true,
    statusBarWidth: 500,
    dragdropWidth: 500
});

$('.buyerDropDownList').change(function () {
    var buyerId = $(this).val();
    SpecificationSheet.PopulateBuyerContactPerson(buyerId);
});

$('.dropdownSpecificationSheet').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var styleNo = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: styleNo
    };

    SpecificationSheet.PopulateJobNumberDropdown(option);
});



var SpecificationSheet = {
    ClearDropDownList: function (selector) {
        $('.' + selector)
            .find('option').remove()
            .end().append($('<option>').text("-select-").attr('value', ""));
    },
    

    DeleteImageFile: function (data, pd) {
        $.ajax({
            url: "/Merchandising/SpecificationSheet/DeleteImageFile",
            type: "POST",
            dataType: "JSON",
            data: { filePath: data.ImageFilePath },
            success: function (respons) {
                if (respons.Success == true) {
                    $('input[type="hidden"][value="' + respons.FilePath + '"]').remove();
                    pd.statusbar.hide();
                } else if (respons.Success == false) {
                    alert("Image  not delte ");
                }
            }
        });
    },

    DeleteDocumnetFile: function (data, pd) {
        $.ajax({
            url: "/Merchandising/SpecificationSheet/DeleteDocumnetFile",
            type: "POST",
            dataType: "JSON",
            data: { filePath: data.DocumentFilePath },
            success: function (respons) {
                if (respons.Success == true) {
                    $('input[type="hidden"][value="' + respons.FilePath + '"]').remove();
                    pd.statusbar.hide();
                } else if (respons.Success == false) {
                    alert("Document not delete");
                }
            }
        });
    },

    PopulateBuyerContactPerson: function (buyerId) {
        $.ajax({
            url: "/Merchandising/SpecificationSheet/GetContactPersonByBuyerId",
            type: "GET",
            dataType: "JSON",
            data: { buyerId: buyerId },
            success: function (resposns) {
                SpecificationSheet.ClearDropDownList('contactPersonDropDownList');
                if (resposns.Success == true) {
                    $.each(resposns.buyerContactPersons, function (index, value) {
                        $('.contactPersonDropDownList')
                            .append($('<option>').text(value.Name).attr('value', value.Id));
                    });
                } else {
                    $('.contactPersonDropDownList')
                        .append($('<option>').text("NotFound").attr('value', ""));
                }

            }
        });
    },
    PopulateJobNumberDropdown: function (option) {
 
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { styleNo: option.data },
            success: function (resposns) {
                SpecificationSheet.ClearDropDownList(option.targetClass);
                if (resposns.Success == true && resposns.JobNumbers.length > 0) {

                    for (var i = 0; i < resposns.JobNumbers.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.JobNumbers[i].JobNo).attr('value', resposns.JobNumbers[i].SpecificationSheetId));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }

            }
        });

    },


};
