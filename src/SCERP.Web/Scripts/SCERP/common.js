var SCERP = new function () {
    var self = this;
    var requseturl = "";
    self.AjaxCompleted = function () {
        var optionObject = {
            OtherTargetClass: "",
            TargetClass: '',
            Option: {}
        };
        $("form:not(.submit-allow)").unbind("submit").bind("submit", self.ValidateForm);
        $('.innerTabSaveButton').unbind('click').bind('click', self.SavInnerTabeForm);
        $('.btnAction').unbind('click').bind('click', self.GetEditView);
        $('.btnDetail').unbind('click').bind('click', self.GetEditView);
        $('.FindPartial').unbind('click').bind('click', self.FindPartialView);
        $('.deleteAction').unbind('click').bind('click', self.DelteGetListView);
        $('.dialogClose').unbind('click').bind('click', self.DialogCloseWithReload);
        $("form.submit-allow").unbind("submit").bind("submit", self.FormSubmit);
        $('.tableFocusInputText').formNavigation();
        jQuery.validator.unobtrusive.parse(document);

        self.GridInitialize();

        $("input[type='date']").datepicker($.datepicker.regional["en-US"]).datepicker("option", {
            dateFormat: 'dd/mm/yy',
            yearRange: '1920:' + (new Date().getFullYear() + 5),
            changeMonth: true,
            changeYear: true,
        });
        $(".datepicker").datepicker($.datepicker.regional["en-US"]).datepicker("option", {
            dateFormat: 'dd/mm/yy',
            yearRange: '1920:' + (new Date().getFullYear() + 5),
            changeMonth: true,
            changeYear: true,
        });
        $('.timePicker').timepicker({
            showPeriod: true,
            showLeadingZero: true,
            timeFormat: 'hh:mm:ss'
        });

        $(".pagination a[href], .webgrid th a[href]").unbind("click").bind("click", function (e) {
            $("body").showLoading();
        });
        $('.searchAction').unbind("click").bind("click", function (e) {
            e.preventDefault();
            $(this).searchAction();


        });
        $('.searchEnterKeyAction').unbind("keypress").bind("keypress", function (e) {
            var key = e.which;
            if (key == 13)  //the enter key code
            {
                e.preventDefault();
                $(this).searchAction();
            }

        });


        $('.selectAllCheckBox').unbind("click").bind("click", self.SelectCheckBoxInTable);

        $(".AjaxPopup").unbind('click').bind('click', self.AddNewClick);


        $(".AjaxPopupNoPostBack").unbind('click').bind('click', self.AddtempClick);
        $(".AjaxPopupPostBack").unbind('click').bind('click', self.AddNewForGridUpdateClick);

        $(".SubmitButton").unbind('click').bind('click', self.SubmitPartialChange);
        $(".SubmitConfirmButton").unbind('click').bind('click', self.SubmitWithConfirmation);
        $(".SubmitWithTableButton").unbind('click').bind('click', self.SubmitBuyerPartialChange);


        //self.Uploadfile();

        $(".SubmitWithTableButton").unbind('click').bind('click', self.SubmitBuyerPartialChange);

        $(".printReport").unbind('click').bind('click', self.PrintReport);
        $(".printspecificpagereport").unbind('click').bind('click', self.SpecificPageReportPrint);
        $("#OpenReport").unbind('click').bind('click', self.OpenReport);

        self.UploadImage();
        //Start mrc search

        $('.mrc_Buyer').unbind().bind('change', function (e) {
            e.preventDefault();
            var action = $(this).attr('action');
            var targetClassName = $(this).attr('TargetClass');
            var departmentId = $(this).val();
            var option = {
                url: action,
                targetClass: targetClassName,
                data: departmentId
            };
            self.PopulateSpecSheetDropdownList(option);
        });

        $('.mrc_StyleNo').unbind().bind('change', function (e) {
            e.preventDefault();
            var action = $(this).attr('action');
            var targetClassName = $(this).attr('TargetClass');
            var styleNo = $(this).val();
            var option = {
                url: action,
                targetClass: targetClassName,
                data: styleNo
            };

            self.PopulateJobNumberDropdown(option);
        });
        $('.mrc_JobNumber').unbind().bind('change', function (e) {
            e.preventDefault();
            var action = $(this).attr('action');
            var targetClassName = $(this).attr('TargetClass');
            var jobNumberId = $(this).val();
            var option = {
                url: action,
                targetClass: targetClassName,
                data: jobNumberId
            };

            self.PopulateSampleTypeDropdownList(option);
        });

        $('.mrc_Department').unbind().bind('change', function (e) {
            e.preventDefault();
            var action = $(this).attr('action');
            var targetClassName = $(this).attr('TargetClass');
            var departmentId = $(this).val();
            var option = {
                url: action,
                targetClass: targetClassName,
                data: departmentId
            };

            self.PopulateEmployeesDropdownList(option);
        });


        $('.Hrm_DepartmentId').unbind().bind('change', function (e) {
            e.preventDefault();
            var action = $(this).attr('action');
            var targetClassName = $(this).attr('TargetClass');
            var departmentId = $(this).val();
            var option = {
                url: action,
                targetClass: targetClassName,
                data: departmentId
            };

            self.PopulateSectionInDropdown(option);
        });

        $('.Hrm_CompanyId').unbind().bind('change', function (e) {
            e.preventDefault();
            var action = $(this).attr('action');
            var targetClassName = $(this).attr('TargetClass');
            var companyId = $(this).val();
            var option = {
                url: action,
                targetClass: targetClassName,
                data: companyId
            };
            self.PopulateBranchDropdown(option);
        });

        $('.SearchByCompanyId').unbind().bind('change', function (e) {
            e.preventDefault();
            var action = $(this).attr('action');
            var targetClassName = $(this).attr('TargetClass');

            var companyId = $(this).val();
            var option = {
                url: action,
                targetClass: targetClassName,
                data: companyId
            };

            self.PopulateBranchDropdownList(option);
            //self.PopulateBranchDropdown(option);
        });

        $('.Hrm_BranchId').unbind().bind('change', function (e) {
            e.preventDefault();
            var action = $(this).attr('action');
            var targetClassName = $(this).attr('TargetClass');
            var otherTargetClass = $(this).attr('OtherTargetClass');
            var branchId = $(this).val();
            var option = {
                url: action,
                targetClass: targetClassName,
                otherTargetClass: otherTargetClass,
                data: branchId
            };
            SCERP.PopulateDepartmentDropdown(option);
        });

        $('.Common_CountryId').unbind().bind('change', function (e) {
            e.preventDefault();
            var action = $(this).attr('action');
            var targetClassName = $(this).attr('TargetClass');
            var countryId = $(this).val();
            var option = {
                url: action,
                targetClass: targetClassName,
                data: countryId
            };
            self.PopulateDistrictDropdown(option);
        });

        $('.Common_DistrictId').unbind().bind('change', function (e) {
            e.preventDefault();
            var action = $(this).attr('action');
            var targetClassName = $(this).attr('TargetClass');
            var districtId = $(this).val();
            var option = {
                url: action,
                targetClass: targetClassName,
                data: districtId
            };
            self.PopulatePoliceStationDropdown(option);
        });
        $('.SearchByBranchUnitId').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var branchUnitId = $(this).val();
            var option = {
                Url: url,
                TargetClass: targetClass,
                data: branchUnitId
            };

            self.PopulateBranchUnitDepartmentDropdownList(option);
        });

        $('.SearchUnitDepartmentByBranchUnitId').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var branchUnitId = $(this).val();
            var option = {
                Url: url,
                TargetClass: targetClass,
                data: branchUnitId
            };
            self.PopulateBUnitDepartmentDropdownList(option);

        });


        $('.SearchByBranchId').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var branchId = $(this).val();
            var option = {
                Url: url,
                TargetClass: targetClass,
                data: branchId
            };
            self.PopulateBrancUnitDropdownList(option);
        });
        $('.Hrm_ByBranchUnitDepartment').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var branchUnitDepartmentId = $(this).val();
            var option = {
                Url: url,
                TargetClass: targetClass,
                data: branchUnitDepartmentId
            };
            self.PopulateDepartmentLineDropdownList(option);
        });

        $('.Hrm_BranchUnitDepartmentForDepartmentSectionAndLine').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var otherTargetClass = $(this).attr('OtherTargetClass');
            var thirdTargetClass = $(this).attr('ThirdTargetClass');
            var branchUnitDepartmentId = $(this).val();
            var option = {
                url: url,
                data: { branchUnitDepartmentId: branchUnitDepartmentId }
            };
            optionObject.Option = option;
            optionObject.TargetClass = targetClass;
            optionObject.OtherTargetClass = otherTargetClass;
            optionObject.ThirdTargetClass = thirdTargetClass;
            self.PopulateBranchUnitDepartmentForDepartmentSectionAndLineDropdown(optionObject);
        });


        $('.Hrm_ByBranchUnitDepartmentForDepartmentSection').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');

            var branchUnitDepartmentId = $(this).val();

            var option = {
                Url: url,
                TargetClass: targetClass,
                data: branchUnitDepartmentId
            };
            self.PopulateDepartmentSectionDropdownList(option);
        });

        $('.Hrm_EmployeeTypeId').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var employeeTypeId = $(this).val();
            var option = {
                Url: url,
                TargetClass: targetClass,
                data: employeeTypeId
            };
            self.PopulateEmployeeGradeDropdownList(option);
        });

        $('.Hrm_EmployeeTypeIdForDesignation').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var employeeTypeId = $(this).val();
            var option = {
                Url: url,
                TargetClass: targetClass,
                data: employeeTypeId
            };
            self.PopulateEmployeeDesignationDropdownListByEmployeeType(option);
        });

        $('.Hrm_EmployeeGradeId').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var employeeGradeId = $(this).val();
            var option = {
                Url: url,
                TargetClass: targetClass,
                data: employeeGradeId
            };
            self.PopulateEmployeeDesignationDropdownList(option);
        });
        $('.Hrm_BranchUnitIdForWorkGroup').unbind('change').bind('change', function () {
            var url = $(this).attr('action');

            var targetClass = $(this).attr('TargetClass');
            var otherTargetClass = $(this).attr('OtherTargetClass');
            var thirdTargetClass = $(this).attr('ThirdTargetClass');
            var branchUnitId = $(this).val();
            var option = {
                url: url,
                data: { branchUnitId: branchUnitId }
            };
            optionObject.Option = option;
            optionObject.TargetClass = targetClass;
            optionObject.OtherTargetClass = otherTargetClass;
            optionObject.ThirdTargetClass = thirdTargetClass;
            self.PopulateWorkGroupDepartmentAndWorkShiftDropdownList(optionObject);
        });
        $('.Hrm_BranchUnitIdForWorkShift').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var otherTargetClass = $(this).attr('OtherTargetClass');
            var branchUnitId = $(this).val();
            var option = {
                url: url,
                data: { branchUnitId: branchUnitId }
            };
            optionObject.Option = option;
            optionObject.TargetClass = targetClass;
            optionObject.OtherTargetClass = otherTargetClass;
            self.PopulateWorkShiftForBranchUnitDropdownList(optionObject);
        });

        $('.serchByCompanyForBranchUnit').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var companyId = $(this).val();
            var option = {
                url: url,
                data: { companyId: companyId }
            };
            optionObject.Option = option;
            optionObject.TargetClass = targetClass;
            self.PopulateBranchUnitByCompanyDropdownList(optionObject);
        });


        $('.Common_ProcessKeyId').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var authorizationId = $(this).val();
            var option = {
                Url: url,
                TargetClass: targetClass,
                data: authorizationId
            };

            self.PopulateAuthorizedPersonDropdownList(option);
        });

        $('.Hrm_BranchUnitIdForDepartmentAndWorkShift').unbind('change').bind('change', function () {
            var url = $(this).attr('action');
            var targetClass = $(this).attr('TargetClass');
            var otherTargetClass = $(this).attr('OtherTargetClass');
            var branchUnitId = $(this).val();
            var option = {
                url: url,
                data: { branchUnitId: branchUnitId }
            };
            optionObject.Option = option;
            optionObject.TargetClass = targetClass;
            optionObject.OtherTargetClass = otherTargetClass;
            self.PopulateDepartmentAndWorkShiftByBranchUnitDropdownList(optionObject);
        });

        //End mrc search

    };
    self.PrintReport = function () {
        $(this).PrintReport();
    };

    self.SpecificPageReportPrint = function () {

        $(this).SpecificPageReportPrint();
    };

    self.Uploadfile = function (e) {
        var hasError = false;

        $('.uploader').each(function (i) {
            var file = $(this);
            var action = file.attr("action");
            $('.uploader').uploadify({
                formData: { 'ASPSESSID': ASPSESSID, 'AUTHID': AUTHID },
                uploader: '/Scripts/Plugins/uploadify/uploadify.swf',
                script: action,
                folder: '/Documents',
                sizeLimit: UploadSizeLimit,
                cancelImg: '/Content/images/uploadify-cancel.png',
                auto: true,
                wmode: 'window',
                buttonText: 'browse ...',
                method: 'post',
                width: 32,
                height: 32,
                fileExt: "jpg",
                //fileDesc: fileDesc,
                simUploadLimit: 3,
                removeCompleted: true,
                //queueID: queueId,
                onInit: function () {
                },
                onError: function (event, ID, fileObj, errorObj) {
                    alert(errorObj.type + ' Error: ' + errorObj.info);
                    hasError = true;
                },
                onSelectOnce: function (event, data) {

                },
                onSelect: function (event, ID, fileObj) {

                },
                onComplete: function (event, ID, fileObj, response, data) {
                    //$("#uploaded").append("<img src='" + data + "' alt='Uploaded Image' />");
                    $(".uploadfilepath").val(fileObj.filePath);
                    //$("#uploaded").append("<img src='/Documents/" + fileObj.name + "' alt='Uploaded Image' />");
                    return true;
                },
                onAllComplete: function (event, data) {
                    return true;
                },
                onCancel: function (event, ID, fileObj, data) {
                    return true;
                }
            });
            file.removeClass("uploader");
        });
    };

    self.UploadImage = function (e) {
        var hasError = false;

        $('.image-upload-wrapper').each(function (i) {
            var container = $(this);
            var action = $(".image-upload", container).attr("action");

            var queue = $('.uploadifyQueue', container);
            var queueId = queue.attr("id");
            if ($(".uploadifyQueue", container).size() == 0) {
                $(".image-upload", container).uploadify({
                    'hideButton': true,
                    'wmode': 'transparent',
                    'uploader': '/Scripts/Plugins/uploadify/uploadify.swf',
                    'script': action,
                    'fileDataName': 'file',
                    'buttonText': "Upload Image",
                    'multi': false,
                    'sizeLimit': UploadSizeLimit,
                    'simUploadLimit': 1,
                    'fileExt': '*.jpg;*.gif;*.png',
                    'fileDesc': 'Image Files',
                    'cancelImg': '/Scripts/Plugins/uploadify/cancel.png',
                    'auto': true,
                    'scriptData': { 'ASPSESSID': ASPSESSID, 'AUTHID': AUTHID },
                    'onError': function (a, b, c, d) {

                        var errMsg = '';
                        if (d.status == 404) {
                            errMsg = "Could not find upload script. Use a path relative to: " + "<?= getcwd() ?>";
                        }
                        else if (d.type === "HTTP") {
                            errMsg = "error " + d.type + ": " + d.status;
                        }
                        else if (d.type === "File Size") {
                            errMsg = c.name + " " + d.type + " Limit: " + Math.round(d.info / (1024 * 1024)) + "MB";
                        }
                        else {
                            errMsg = "error " + d.type + ": " + d.text;
                        }

                        if (errMsg != '') {
                            alert(errMsg);
                        }

                    },

                    'onComplete': function (event, queueId, fileObj, response, data) {
                        if (response != "") {
                            // imgResposnTxt = response;
                            var rsponse = jQuery.parseJSON(response);
                            $(".photographpath").val(rsponse.fullpath);
                            $('#imageContainer').html('');
                            $('#imageContainer').html('<img src="' + rsponse.thumbnailpath + '" />');

                            //$('#imageContainer').prepend('<img src="' + rsponse.thumbnailpath + '" />');
                            //$('<img src="' + rsponse.thumbnailpath + '" />').appendTo('#imageContainer');
                        }
                    },
                    'onAllComplete': function (event, data) {
                        var msg = data.filesUploaded + ' files uploaded successfully!';
                    }

                });
            }
        });
    };
    self.SelectCheckBoxInTable = function (e) {
        var table = $(e.target).closest('table');
        $('td input:checkbox', table).prop('checked', this.checked);
    };
    self.FormSubmit = function (e) {
        var form = $(this);

        $(".validation-summary-errors li", form).remove();

        var ignoreValidation = form.hasClass("ignore-validation");
        if (!ignoreValidation) {
            if (jQuery.validator && jQuery.validator.unobtrusive) {
                form.validate();
                if (!form.valid()) {
                    e.preventDefault();
                    return false;
                }
            }
        }
        $("body").showLoading();
    };

    self.SubmitPartialChange = function (e) {
        var button = $(this);
        var form = button.parents("form:first");
        form.Post({
            success: function (Jsondata) {
                if (Jsondata.Success) {
                    alert("Data Saved Successfully");
                } else {
                    if (typeof (Jsondata.Success) != "undefined") {
                        alert("Unable to save data");
                    }
                }
            }
        });
    };

    self.ValidateForm = function (e) {
        var form = $(this);

        $(".validation-summary-errors li", form).remove();

        var ignoreValidation = form.hasClass("ignore-validation");
        if (!ignoreValidation) {
            if (jQuery.validator && jQuery.validator.unobtrusive) {
                form.validate();
                if (!form.valid()) {
                    e.preventDefault();
                    return false;
                }
            }
        }
        e.preventDefault();
        return false;
    };

    self.SavInnerTabeForm = function (e) {
        var button = $(this);
        var form = button.parents("form:first");
        var url = form.attr('action');
        var targentClass = button.attr('TargentClass');
        var data = form.serialize();
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            form.validate();
            if (!form.valid()) {
                // e.preventDefault();
                return false;
            } else {
                jQuery.Ajax({
                    url: url,
                    type: "POST",
                    data: data,
                }).done(function (respons) {
                    if (respons.Success == false) {
                        //  alert(respons.Message);
                    } else {
                        $('.' + targentClass).html(respons);
                        form.DialogClose();
                    }
                });
            }

        }

    };
    self.GetEditView = function (e) {
        var button = $(this);
        var $tr = button.closest('tr');
        $tr.addClass("selected").siblings().removeClass("selected");
        var url = button.attr('action');
        var targentClass = button.attr('targentclass');
        jQuery.Ajax({
            url: url,
            type: "GET",
            container: '.' + targentClass
        });
    };
    self.FindPartialView = function (e) {

        var button = $(this);
        var target = button.attr('target');
        var url = button.attr('action');
        var form = button.parents('form:first');
        var data = form.serialize();
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            form.validate();
            if (!form.valid()) {
                e.preventDefault();
                return false;
            } else {
                $.Ajax({
                    type: "POST",
                    url: url
                    , loader: { loader: "body" },
                    data: data,
                    container: target
                });
            }
        }
    };
    self.DelteGetListView = function (e) {
        var button = $(this);
        var url = button.attr('action');
        var targentClass = button.attr('targentclass');
        jQuery.Confirm(SCERP.Messages.DeleteConfirmation, function (r) {
            if (r) {
                jQuery.Ajax({
                    url: url,
                    type: "GET",
                    container: '.' + targentClass
                });
            }
        });
        e.preventDefault();
        return false;
    };
    self.DialogCloseWithReload = function (e) {
        var button = $(this);
        jQuery.Ajax({
            url: button.attr('action')
            , type: "GET"
            , container: '#contentRight'
        });
        button.DialogClose();
        e.preventDefault();
    }
    self.SearchInitialize = function () {
        $(".search").unbind("click").bind("click", function (e) {
            var button = $(this);
            var form = button.parents("form:first");
            //var data = form.SerializeData();
            form.submit();
            e.preventDefault();
            return false;
        });
    };

    self.GridInitialize = function () {

        $('button.new-window', '.webgrid').unbind('click').bind('click', self.GridShowInNewWindow);

        $('button.edit, button.view', '.webgrid').unbind('click').bind('click', self.GridShowInPopup);

        $('button.editCallBack, button.view', '.webgrid').unbind('click').bind('click', self.AddtempEditClick);

        $('button.deleteCallBack', '.webgrid').unbind('click').bind('click', self.GridDeleteClickCallBack);
        $('button.delete', '.webgrid').unbind('click').bind('click', self.GridDeleteClick);
        $('button.deleteRowZeroId', '.webgrid').unbind('click').bind('click', self.DeleteRowZeroId);

        $("button.add-new", 'ul.search').unbind('click').bind('click', self.AddNewClick);

        $("button.action", '.webgrid').unbind('click').bind('click', self.GridActionButtonClick);
        $(".Edit_Button", '.webgrid').unbind('click').bind('click', self.GridShowInPopup);
        $(".page", '.webgrid').unbind('click').bind('click', function (e) {
            location.href = $(this).action();
            $("body").showLoading();
        });

        /*
        $('th a', '.webgrid').unbind('click').bind('click', function () {
            $("body").showLoading();
        });*/

        var grid = $('.webgrid');
        var sort = (grid.attr("sort") || "none").toLowerCase();
        var sortdir = (grid.attr("sortdir") || "none").toLowerCase();

        var sortColumn = grid.find("tbody td." + sort + ":first").index();
        if (sortColumn >= 0) {
            // grid.find("thead th:eq(" + sortColumn + ")").addClass(sortdir);
            //  $("body").hideLoading();
        }
    };

    self.GridShowInNewWindow = function (e) {
        var action = $(this).attr("action");

        var feature = "height=" + $(window).height() + ", width=" + $(window).width() + "";
        if ($(this).hasClass("pdf")) {
            feature = "height=800,width=600,scrollbars=1,resizable=1,location=0,left=0";
        }
        var popup = window.open(action, "_blank", feature);
        if (popup && !$(this).hasClass("pdf")) {
            try {
                popup.moveTo(0, 0);
                popup.resizeTo(screen.width, screen.height);
            }
            catch (ex) { }
        }
        e.preventDefault();
        return false;
    };

    self.GridShowInPopup = function () {
        var action = $(this).attr("action");
        jQuery.Ajax({
            url: action
            , type: "GET"
            , dialog: {}
            , success: self.DialogOpened
        });
    };

    self.OpenReport = function () {
        var reportLink = $(this);
        reportLink.attr("target", "_blank");
        //window.open(reportLink.attr("href"));
        e.preventDefault();
        return false;
    };

    self.AddNewClick = function (e) {
        var action = $(this).attr("action");
        if (action) {

            jQuery.Ajax({
                url: action
                , type: "GET"
                , dialog: {}
                , loader: { loader: "body" }
                , success: self.DialogOpened
            });
        }
        e.preventDefault();
        return false;
    };



    self.DialogOpened = function (r) {
        $(":submit", ".dialog-ontainer").unbind("click").bind("click", function (e) {
            var button = $(this);
            var form = button.parents("form:first");
            form.Post({
                success: function (r) {
                    if (r.Success) {
                        button.DialogClose();
                    }
                    else {
                        button.parents(".ui-dialog-content:first").html(r.Content);
                        self.DialogOpened(r);
                    }
                }
            });
        });
    };
    self.GridDeleteClickCallBack = function (e) {
        var button = $(this);
        var action = $(this).attr("action");
        document.title = action.split('/')[2];
        jQuery.Confirm(self.Messages.DeleteConfirmation, function (r) {
            if (r) {
                jQuery.Ajax({
                    url: action
                    , loader: {}
                    , success: function (r) {
                        if (r.Success == true) {
                            $(button).closest('tr').remove();
                        } else if (r.id === 0) {
                            $(button).closest('tr').remove();
                        }

                    }
                });
            }
        });
        e.preventDefault();
        return false;
    };
    self.SubmitWithConfirmation = function (e) {
        var button = $(this);
        var form = button.parents("form:first");
        var action = form.attr('action');
        document.title = form.attr('title');
        jQuery.Confirm(SCERP.Messages.SendConfirmation, function (r) {
            if (r) {
                jQuery.Ajax({
                    type: "post",
                    url: action,
                    data: form.serialize()
                    , loader: {}
                    , success: function (r) {

                    }
                });
            }
        });
        e.preventDefault();
        return false;

    },
    self.GridDeleteClick = function (e) {
        var action = $(this).attr("action");
        document.title = action.split('/')[2];
        jQuery.Confirm(SCERP.Messages.DeleteConfirmation, function (r) {
            if (r) {
                jQuery.Ajax({
                    url: action
                    , loader: {}
                    , success: function (r) {

                    }
                });
            }
        });
        e.preventDefault();
        return false;
    };


    self.GridActionButtonClick = function (e) {
        var action = $(this).attr("action");
        jQuery.Ajax({
            url: action
            , loader: {}
            , success: function (r) {
            }
        });
        e.preventDefault();
        return false;
    };

    self.ForgotPasswordLoad = function (r) {
        $("input[type='submit']", ".forgot-password-form").unbind("click").bind("click", function (e) {
            var button = $(this);
            var form = $(".forgot-password-form");
            form.Post({
                success: function (r) {
                    if (r.Success) {
                        button.DialogClose();
                        if (r.Message) {
                            alert(r.Message);
                        }
                    }
                    else if (r.IsHtml) {
                        $(".ui-dialog-content").html(r.Content);
                        self.ForgotPasswordLoad(r);
                    }

                }
            });

            e.preventDefault();
            return false;
        });
    };

    self.AjaxError = function (e, xhr) {
        if (xhr.status == 401)
            window.location = "/Home/Login";
        else if (xhr.status == 403)
            alert("You do not have enough permission to access this resource.");
    };
};

SCERP.Messages = new function () {
    var self = this;

    self.DeleteConfirmation = "Do you want to delete?";
    self.SendConfirmation = "Do you want to Send?";
    self.CopyPastConfirmation = "Do you want to Copy and paste ?";

};
var app = {};
if (typeof (angular) != 'undefined') {
    app = angular.module('app', ['ui.bootstrap']);
    app.filter('startFrom', function () {
        return function (input, start) {
            start = +start; //parse to int
            return input.slice(start);
        };
    });
}

$(document).ajaxStop(function () {
    $("body").hideLoading();
});

$(document).ready(function () {

    $("body").hideLoading();

    $(document).ajaxComplete(SCERP.AjaxCompleted);
    $(document).ajaxError(SCERP.AjaxError);
    SCERP.AjaxCompleted();
    SCERPAjaxOptions.Dialog.Hide = "scale";
    SCERPAjaxOptions.Dialog.Show = "puff";

    $(".logoff").click(function (e) {
        var form = $("#logoutForm");
        form.submit();
        e.preventDefault();
        return false;
    });

    $(".forgot-password").unbind("click").bind("click", function (e) {
        jQuery.Ajax({
            url: "/Account/ForgotPassword"
            , type: "GET"
            , dialog: {}
            , success: SCERP.ForgotPasswordLoad
        });
        e.preventDefault();
        return false;
    });
    fixedFotter();
});

function fixedFotter() {
    var h = $(window).height();
    var n = $('.footer').css('height');
    var c = parseInt(n);
    $('.footer').css('margin-top', h - c);
}

(function ($) {
    $.fn.toggleHighlight = function () {
        $(this).toggleClass("highlight");
        //$(this).effect("highlight", {}, 3000);
    };

    $.fn.DialogClose = function () {
        var dialogWindow = $(this).parents(".ui-dialog-content:first");
        $("iframe", dialogWindow).attr("src", "about:blank"); // IE9 fix
        dialogWindow.dialog("close");
    };

    $.fn.action = function () {
        return $(this).attr("action") || "";

    };

    $.fn.PrintReport = function () {
        var preintContainerId = $(this).attr('action');
        var prtContent = document.getElementById(preintContainerId);
        var data = prtContent.innerHTML;
        var popupWindow = window.open('', '', 'width=810,height=520,left=0,top=0,toolbar=0,status=0');
        popupWindow.document.open();
        popupWindow.document.write('<HTML>\n<HEAD>\n');
        //popupWindow.document.write('<link rel="stylesheet" type="text/css" href="~/Content/Common/PdfReport.css"/>');
        popupWindow.document.write('</HEAD>\n');
        popupWindow.document.write('<BODY >\n');
        popupWindow.document.write(data);
        popupWindow.document.write('</BODY>\n');
        popupWindow.document.write('</HTML>\n');
        popupWindow.document.close();
        popupWindow.focus();
        popupWindow.print();
        popupWindow.close();

    };

    $.fn.SpecificPageReportPrint = function () {
        var $this = $(this);
        var url = $this.attr('action');
        jQuery.ajax({
            url: url,
            dataType: 'html',
            type: "GET",
            success: function (response) {
                var data = response;
                var popupWindow = window.open('', '', 'width=810,height=520,left=0,top=0,toolbar=0,status=0');
                popupWindow.document.open();
                popupWindow.document.write('<HTML>\n<HEAD>\n');
                popupWindow.document.write('<link rel="stylesheet" type="text/css" href="~/Content/Common/PdfReport.css"/>');
                popupWindow.document.write('</HEAD>\n');
                popupWindow.document.write('<BODY >\n');
                popupWindow.document.write(data);
                popupWindow.document.write('</BODY>\n');
                popupWindow.document.write('</HTML>\n');
                popupWindow.document.close();
                popupWindow.focus();
                popupWindow.print();
                popupWindow.close();
            }
        });
    };

    //Accounting report 

    $.fn.openingOption = function (columns) {

        $(this).change(function () {
            if ($(this).val() == 1) {
                columns.show();

            } else {
                columns.hide();


            }

        });
    };
    $.fn.openingOptionState = function (columns) {
        if ($(this).val() == 1) {
            columns.show();
        } else {
            columns.hide();
        }
    };
    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
    $.fn.searchAction = function (e) {
        var form = $(this).parents("form:first");
        var url = form.attr('action');
        var data = form.serialize();
        var ignoreValidation = form.hasClass("ignore-validation");
        if (!ignoreValidation) {
            if (jQuery.validator && jQuery.validator.unobtrusive) {
                form.validate();
                if (!form.valid()) {
                    // e.preventDefault();
                    return false;
                }
                SearchResult(url, data);
            }
        } else {
            SearchResult(url, data);
        }
    };
}(jQuery));

function SearchResult(url, data) {
    $("form").showLoading();
    if (url != undefined && url != "") {
        requseturl = url;
        jQuery.Ajax({
            url: requseturl
               , type: "GET",
            data: data
               , container: '#contentRight'
        });
    } else {
        requseturl = "";
    }
}

function GetMenu(menuname) {
    if (menuname == 'companies')
        return "company";
    else
        return menuname;
}

function search(url, formClass) {

    if (url != undefined && url != "") {
        requseturl = url;
        jQuery.Ajax({
            url: requseturl
               , type: "POST",
            data: $('.' + formClass + '').serialize()
               , container: '#contentRight'
        });
    } else {
        requseturl = "";
    }

}
//Accounting  Report
var CatchFromToDateTime = {
    NewDateTime: {},
    SetDateTime: function (data) {
        this.NewDateTime = data;
    },
    GetDateTime: function () {
        return this.NewDateTime;
    }
};
function LogOut() {
    Redirect("/Home/LogOff");
}

function Redirect(url) {
    if (url) {
        window.top.location.href = url;
    }
}

jQuery(function ($) {
    $.validator.addMethod('date',
        function (value, element) {
            if (this.optional(element)) {
                return true;
            }

            var ok = true;
            try {
                $.datepicker.parseDate('dd/mm/yy', value);
            } catch (err) {
                ok = false;
            }
            return ok;
        });
});

(function ($) {
    $.fn.formNavigation = function () {
        $(this).each(function () {
            $(this).find('input[type=text]').on('keyup', function (e) {
                switch (e.which) {
                    case 39:
                        $(this).closest('td').next().find('input').focus(); break;
                    case 37:
                        $(this).closest('td').prev().find('input').focus(); break;
                    case 40:
                        $(this).closest('tr').next().children().eq($(this).closest('td').index()).find('input').focus(); break;
                    case 38:
                        $(this).closest('tr').prev().children().eq($(this).closest('td').index()).find('input').focus(); break;
                }
            });
        });
    };
})(jQuery);


//Collapse Div Script
function toggleDiv() {
    var x = document.getElementById("collapseDiv");
    if (x.style.display === "block") {
        x.style.display = "none";
    } else {
        x.style.display = "block";
    }

    var open = document.getElementById("open");
    var close = document.getElementById("close");

    if (open.style.display === "none") {
        open.style.display = "block";
        close.style.display = "none";
    } else {
        open.style.display = "none";
        close.style.display = "block";
    }
}