
var MrcSearch = new function () {
    SCERP.PopulateSampleTypeDropdownList = function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { jobNumberId: option.data },
            success: function (resposns) {
                SCERP.ClearDropDownList(option.targetClass);
                if (resposns.Success == true && resposns.DevelopmentSamples.length > 0) {

                    for (var i = 0; i < resposns.DevelopmentSamples.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.DevelopmentSamples[i].SampleName).attr('value', resposns.DevelopmentSamples[i].SampleTypeId));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }

            }
        });
    },
    SCERP.PopulateEmployeesDropdownList = function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { id: option.data },
            success: function (resposns) {
                SCERP.ClearDropDownList(option.targetClass);
                if (resposns.Success == true && resposns.EmployeesList.length > 0) {

                    for (var i = 0; i < resposns.EmployeesList.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.EmployeesList[i].Name).attr('value', resposns.EmployeesList[i].EmployeeId));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }

            }
        });
    };
    SCERP.PopulateSpecSheetDropdownList = function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { id: option.data },
            success: function (resposns) {
                SCERP.ClearDropDownList(option.targetClass);
                if (resposns.Success == true && resposns.SpecificationSheet.length > 0) {

                    for (var i = 0; i < resposns.SpecificationSheet.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.SpecificationSheet[i].StyleNo).attr('value', resposns.SpecificationSheet[i].StyleNo));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }

            }
        });
    };

    SCERP.PopulateJobNumberDropdown = function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { styleNo: option.data },
            success: function (resposns) {
                SCERP.ClearDropDownList(option.targetClass);
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

    };
    SCERP.PopulateSectionInDropdown = function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { departmentId: option.data },
            success: function (respons) {
                SCERP.ClearDropDownList(option.targetClass);
                if (respons.Success == true && respons.Sections.length > 0) {
                    for (var i = 0; i < respons.Sections.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(respons.Sections[i].Name).attr('value', respons.Sections[i].SectionId));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }
            }
        });
    };

    SCERP.PopulateBranchDropdown = function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { companyId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.targetClass);
                if (response.Success == true && response.Branches.length > 0) {
                    for (var i = 0; i < response.Branches.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(response.Branches[i].Name).attr('value', response.Branches[i].Id));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };

    SCERP.PopulateBranchDropdownList = function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { companyId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.targetClass);
                if (response.Success == true && response.Branches.length > 0) {
                    for (var i = 0; i < response.Branches.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(response.Branches[i].BranchName).attr('value', response.Branches[i].BranchId));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };

    SCERP.PopulateDistrictDropdown = function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { countryId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.targetClass);
                if (response.Success == true && response.Districts.length > 0) {
                    for (var i = 0; i < response.Districts.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(response.Districts[i].Name).attr('value', response.Districts[i].Id));
                    }
                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };

    SCERP.PopulatePoliceStationDropdown = function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { districtId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.targetClass);
                if (response.Success == true && response.PoliceStations.length > 0) {
                    for (var i = 0; i < response.PoliceStations.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(response.PoliceStations[i].Name).attr('value', response.PoliceStations[i].Id));
                    }
                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };

    SCERP.PopulateDepartmentDropdown = function (option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { id: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.targetClass);
                SCERP.ClearDropDownList(option.otherTargetClass);
                if (response.Success == true && response.Departments.length > 0) {
                    for (var i = 0; i < response.Departments.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(response.Departments[i].Title).attr('value', response.Departments[i].Id));
                    }
                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }

                if (response.Success == true && response.WorkGroups.length > 0) {
                    for (var i = 0; i < response.WorkGroups.length; i++) {
                        $('.' + option.otherTargetClass)
                            .append($('<option>').text(response.WorkGroups[i].WorkGroupName).attr('value', response.WorkGroups[i].Id));
                    }

                } else {
                    $('.' + option.otherTargetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };



    SCERP.PopulateBUnitDepartmentDropdownList = function (option) {
        $.Ajax({
            url: option.Url,
            type: "GET",
            data: { branchUnitId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.TargetClass);
                if (response.Success == true && response.UnitDepartments.length > 0) {
                    $.each(response.UnitDepartments, function (i, unitDepartment) {
                        var populatedData = { TargetClass: option.TargetClass, Text: unitDepartment.DepartmentName, Value: unitDepartment.UnitDepartmentId };
                        SCERP.BindDataToDropdown(populatedData);
                    });
                } else {
                    SCERP.NotFoundData(option.TargetClass);
                }


            }
        });
    };

    SCERP.PopulateBranchUnitDepartmentDropdownList = function (option) {
        $.ajax({
            url: option.Url,
            type: "GET",
            dataType: "JSON",
            data: { branchUnitId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.TargetClass);
                if (response.Success == true && response.BranchUnitDepartments.length > 0) {

                    for (var i = 0; i < response.BranchUnitDepartments.length; i++) {

                        $('.' + option.TargetClass)
                            .append($('<option>').text(response.BranchUnitDepartments[i].DepartmentName).attr('value', response.BranchUnitDepartments[i].BranchUnitDepartmentId));
                    }
                } else {
                    $('.' + option.TargetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };


    SCERP.PopulateBrancUnitDropdownList = function (option) {
        $.ajax({
            url: option.Url,
            type: "GET",
            dataType: "JSON",
            data: { branchId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.TargetClass);
                if (response.Success == true && response.BrancheUnits.length > 0) {

                    for (var i = 0; i < response.BrancheUnits.length; i++) {

                        $('.' + option.TargetClass)
                            .append($('<option>').text(response.BrancheUnits[i].UnitName).attr('value', response.BrancheUnits[i].BranchUnitId));
                    }
                } else {
                    $('.' + option.TargetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };

    SCERP.PopulateEmployeeGradeDropdownList = function (option) {
        $.ajax({
            url: option.Url,
            type: "GET",
            dataType: "JSON",
            data: { employeeTypeId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.TargetClass);
                if (response.Success == true && response.EmployeeGrades.length > 0) {

                    for (var i = 0; i < response.EmployeeGrades.length; i++) {

                        $('.' + option.TargetClass)
                            .append($('<option>').text(response.EmployeeGrades[i].Name).attr('value', response.EmployeeGrades[i].Id));
                    }
                } else {
                    $('.' + option.TargetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };

    SCERP.PopulateEmployeeDesignationDropdownList = function (option) {
        $.ajax({
            url: option.Url,
            type: "GET",
            dataType: "JSON",
            data: { employeeGradeId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.TargetClass);
                if (response.Success == true && response.EmployeeDesignations.length > 0) {
                    for (var i = 0; i < response.EmployeeDesignations.length; i++) {

                        $('.' + option.TargetClass)
                            .append($('<option>').text(response.EmployeeDesignations[i].Title).attr('value', response.EmployeeDesignations[i].Id));
                    }
                } else {
                    $('.' + option.TargetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };
    
    SCERP.PopulateEmployeeDesignationDropdownListByEmployeeType = function (option) {
        $.ajax({
            url: option.Url,
            type: "GET",
            dataType: "JSON",
            data: { employeeTypeId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.TargetClass);
                if (response.Success == true && response.EmployeeDesignations.length > 0) {
                    for (var i = 0; i < response.EmployeeDesignations.length; i++) {

                        $('.' + option.TargetClass)
                            .append($('<option>').text(response.EmployeeDesignations[i].Title).attr('value', response.EmployeeDesignations[i].Id));
                    }
                } else {
                    $('.' + option.TargetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };
    
    SCERP.PopulateDepartmentLineDropdownList = function (option) {
        $.ajax({
            url: option.Url,
            type: "GET",
            dataType: "JSON",
            data: { branchUnitDepartmentId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.TargetClass);
                if (response.Success == true && response.DataSources.length > 0) {

                    for (var i = 0; i < response.DataSources.length; i++) {

                        $('.' + option.TargetClass)
                            .append($('<option>').text(response.DataSources[i].DisplayMember).attr('value', response.DataSources[i].ValueMember));
                    }
                } else {
                    $('.' + option.TargetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };
    SCERP.PopulateWorkGroupDepartmentAndWorkShiftDropdownList = function (optionObj) {
        $.Ajax(optionObj.Option).done(function (response) {
            SCERP.ClearDropDownList(optionObj.TargetClass);
            if (response.Success == true && response.WorkGroups.length > 0) {
                $.each(response.WorkGroups, function (i, workGroup) {
                    var populatedData = { TargetClass: optionObj.TargetClass, Text: workGroup.Name, Value: workGroup.WorkGroupId };
                    SCERP.BindDataToDropdown(populatedData);
                });
            } else {
                SCERP.NotFoundData(optionObj.TargetClass);
            }
            
            if (response.Success == true && response.BranchUnitDepartments.length > 0) {
                SCERP.ClearDropDownList(optionObj.OtherTargetClass);
                $.each(response.BranchUnitDepartments, function (i, branchUnitDepartment) {
                    var populatedData = { TargetClass: optionObj.OtherTargetClass, Text: branchUnitDepartment.DepartmentName, Value: branchUnitDepartment.BranchUnitDepartmentId };
                    SCERP.BindDataToDropdown(populatedData);
                });
            } else {
                SCERP.NotFoundData(optionObj.OtherTargetClass);
            }
                         
            if (response.Success == true && response.BranchUnitWorkShifts.length > 0) {              
                SCERP.ClearDropDownList(optionObj.ThirdTargetClass);
                $.each(response.BranchUnitWorkShifts, function (i, branchUnitWorkShift) {
                    var populatedData = { TargetClass: optionObj.ThirdTargetClass, Text: branchUnitWorkShift.DisplayMember, Value: branchUnitWorkShift.ValueMember };
                    SCERP.BindDataToDropdown(populatedData);
                });
            } else {
                SCERP.NotFoundData(optionObj.OtherTargetClass);
            }
        });
    };
    SCERP.PopulateWorkShiftForBranchUnitDropdownList = function (optionObj) {
        $.Ajax(optionObj.Option).done(function (response) {
            SCERP.ClearDropDownList(optionObj.TargetClass);
            if (response.Success == true && response.DataSources.length > 0) {
                $.each(response.DataSources, function (i, dataSource) {
                    var populatedData = { TargetClass: optionObj.TargetClass, Text: dataSource.DisplayMember, Value: dataSource.ValueMember };
                    SCERP.BindDataToDropdown(populatedData);
                });
            } else {
                SCERP.NotFoundData(optionObj.TargetClass);
            }
            if (response.Success == true && response.BranchUnitDepartments.length > 0) {
                SCERP.ClearDropDownList(optionObj.OtherTargetClass);
                $.each(response.BranchUnitDepartments, function (i, branchUnitDepartment) {
                    var populatedData = { TargetClass: optionObj.OtherTargetClass, Text: branchUnitDepartment.DepartmentName, Value: branchUnitDepartment.BranchUnitDepartmentId };
                    SCERP.BindDataToDropdown(populatedData);
                });
            } else {
                SCERP.NotFoundData(optionObj.OtherTargetClass);
            }
        });
    };

    SCERP.PopulateDepartmentAndWorkShiftByBranchUnitDropdownList = function (optionObj) {
        $.Ajax(optionObj.Option).done(function (response) {
            SCERP.ClearDropDownList(optionObj.TargetClass);

            if (response.Success == true && response.BranchUnitDepartments.length > 0) {
                SCERP.ClearDropDownList(optionObj.OtherTargetClass);
                $.each(response.BranchUnitDepartments, function (i, branchUnitDepartment) {
                    var populatedData = { TargetClass: optionObj.TargetClass, Text: branchUnitDepartment.DepartmentName, Value: branchUnitDepartment.BranchUnitDepartmentId };
                    SCERP.BindDataToDropdown(populatedData);
                });
            } else {
                SCERP.NotFoundData(optionObj.OtherTargetClass);
            }

            if (response.Success == true && response.DataSources.length > 0) {
                $.each(response.DataSources, function (i, dataSource) {
                    var populatedData = { TargetClass: optionObj.OtherTargetClass, Text: dataSource.DisplayMember, Value: dataSource.ValueMember };
                    SCERP.BindDataToDropdown(populatedData);
                });
            } else {
                SCERP.NotFoundData(optionObj.TargetClass);
            }

        });
    };

    SCERP.PopulateBranchUnitDepartmentForDepartmentSectionAndLineDropdown = function (optionObject) {
        $.Ajax(optionObject.Option).done(function (response) {
            SCERP.ClearDropDownList(optionObject.TargetClass);
            SCERP.ClearDropDownList(optionObject.OtherTargetClass);
            SCERP.ClearDropDownList(optionObject.ThirdTargetClass);
            
            if (response.Success == true && response.DepartmentSections.length > 0) {
                SCERP.ClearDropDownList(optionObject.TargetClass);
                $.each(response.DepartmentSections, function (i, departmentSection) {
                    var populatedData = { TargetClass: optionObject.TargetClass, Text: departmentSection.DisplayMember, Value: departmentSection.ValueMember };
                    SCERP.BindDataToDropdown(populatedData);
                });
            } else {
                SCERP.NotFoundData(optionObject.TargetClass);
            }
            
            if (response.Success == true && response.DepartmentLines.length > 0) {
                $.each(response.DepartmentLines, function (i, departmentLine) {
                    var populatedData = { TargetClass: optionObject.OtherTargetClass, Text: departmentLine.DisplayMember, Value: departmentLine.ValueMember };
                    SCERP.BindDataToDropdown(populatedData);
                    
                    var populatedDataForThridTarget = { TargetClass: optionObject.ThirdTargetClass, Text: departmentLine.DisplayMember, Value: departmentLine.ValueMember };
                    SCERP.BindDataToDropdown(populatedDataForThridTarget);

                });
            } else {
                SCERP.NotFoundData(optionObject.OtherTargetClass);
                SCERP.NotFoundData(optionObject.ThirdTargetClass);
            }                  
        });
    };
    SCERP.PopulateBranchUnitByCompanyDropdownList = function (optionObj) {
        $.Ajax(optionObj.Option).done(function (response) {
            SCERP.ClearDropDownList(optionObj.TargetClass);
            if (response.Success == true && response.BranchUnits.length > 0) {
                $.each(response.BranchUnits, function (i, branchUnit) {
                    var populatedData = { TargetClass: optionObj.TargetClass, Text: branchUnit.UnitName, Value: branchUnit.BranchUnitId };
                    SCERP.BindDataToDropdown(populatedData);
                });
            } else {
                SCERP.NotFoundData(optionObj.TargetClass);
            }
        });
    };

    SCERP.PopulateAuthorizedPersonDropdownList = function (option) {
        $.ajax({
            url: option.Url,
            type: "GET",
            dataType: "JSON",
            data: { authorizationId: option.data },
            success: function (response) {
                SCERP.ClearDropDownList(option.TargetClass);
                if (response.Success == true && response.AuthorizedPersons.length > 0) {

                    for (var i = 0; i < response.AuthorizedPersons.length; i++) {

                        $('.' + option.TargetClass)
                            .append($('<option>').text(response.AuthorizedPersons[i].Name).attr('value', response.AuthorizedPersons[i].EmployeeId));
                    }
                } else {
                    $('.' + option.TargetClass)
                        .append($('<option>').text("Not Found").attr('value', ""));
                }
            }
        });
    };

    SCERP.ClearDropDownList = function (selector) {
        $('.' + selector)
            .find('option').remove()
            .end().append($('<option>').text("-Select-").attr('value', ""));
    };

    SCERP.NotFoundData = function (targetClass) {
        $('.' + targetClass)
                     .append($('<option>').text("Not Found").attr('value', ""));
    };
    SCERP.BindDataToDropdown = function (populatedData) {
        $('.' + populatedData.TargetClass)
                                  .append($('<option>').text(populatedData.Text).attr('value', populatedData.Value));
    };

};


