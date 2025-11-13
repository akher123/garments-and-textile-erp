function itemAutocomplite(obj) {
    $(obj).autocomplete({
        source: function (request, response) {
            var option = {
                url: "/MaterialIssue/AutocompliteItemByBranch",
                type: "POST",
                data: { itemName: request.term },
            };
            $.Ajax(option)
                .done(function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ItemCode+ "_" + item.ItemName + ", ROL :" + item.ReorderLevel, value: item.ItemName, ItemId: item.ItemId };
                    }));
                });
        },
        select: function (event, ui) {
            var itemId = ui.item.ItemId;
            $('.itemHiddenField').val(itemId);
            $.Ajax({
                type: 'GET',
                dataType: "json",
                url: '/MaterialIssue/StockStatys',
                data: { itemId: itemId },
                success: function (result) {
                    if (result.Success == false) {
                        alert(result.Message);
                    } else {
                        populateStockInfo(result);
                        $('.requiredQuantity').focus();
                    }

                },
                onError: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                }
            });
        },
    });
}

function deleteRow(buttonObj) {
    var $tr = $(buttonObj).closest("tr");
    $tr.remove();
}
$('.requiredQuantity').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.issuedQuantity').focus();
    }

});
$('.issuedQuantity').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.remarks').focus();
    }

});
$('.enterVent').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        addNewRow();
    }

});

function addNewRow() {
    var $ItemId = $('.itemHiddenField');
    var $mIDetailId = $('.mIDetailId-' + $ItemId.val());
    var $SizeId = $('.sizeId');
    var $BrandId = $('.brandId');
    var $OriginId = $('.originId');
    var $MachineId = $('.machineId');
    
    var $ItemName = $('.itemName');
    var $StockInHand = $('.stockInHand');
    var $IssuedQuantity = $('.issuedQuantity');
    var $IssuedItemRate = $('.issuedItemRate');
    var $requiredQuantity = $('.requiredQuantity');
    var $Remarks = $('.remarks');
    var viewmodel = {
        "IssueDetail": {
            "ItemId": $ItemId.val(),
            "MaterialIssueDetailId":$mIDetailId.val(),
            "SizeId": $SizeId.val(),
            "SizeName": $SizeId.val()!=""? $SizeId.find('option:selected').text():"",
            "BrandId":$BrandId.val(),
            "BrandName": $BrandId.val()!=""?$BrandId.find('option:selected').text():"",
            "OriginId": $OriginId.val(),
            "OriginName":$OriginId.val()!=""? $OriginId.find('option:selected').text():"",
            
            "MachineId": $MachineId.val(),
            "MachineName": $MachineId.val()!=""?$MachineId.find('option:selected').text():"",

            "ItemName": $ItemName.val(),
            "StockInHand": $StockInHand.val() == "" ? 0 : $StockInHand.val(),
            "IssuedItemRate": $IssuedItemRate.val() == "" ? 0 : $IssuedItemRate.val(),
            "RequiredQuantity": $requiredQuantity.val() == "" ? 0 : $requiredQuantity.val(),
            "IssuedQuantity": $IssuedQuantity.val() == "" ? 0 : $IssuedQuantity.val(),
            "Remarks": $Remarks.val() == "" ? 0 : $Remarks.val(),
        },
    };
    var $table = $('table#GeneralIssueDetail tbody');
    var option = {
        url: "/GeneralIssue/AddNewRow",
        type: "POST",
        datatype: "html",
        contentType: "application/json",
        data: JSON.stringify(viewmodel)
    };
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        $ItemName.validate();
        if (!$ItemName.valid()) {
            return false;
        } else {
            if (parseInt($ItemId.val()) > 0) {
                $.ajax(option).done(function (response) {
                    if (response.Success == false) {
                        alert(response.Message);
                    } else {
                
                        //if ($table.find('tr').length > 0) {
                        //    $('#newRow-' + $ItemId.val()).replaceWith(response);
                        //} else {
                        //    $('#newRow-' + $ItemId.val()).remove();
                        //    $table.append(response);
                        //}
                        $('#newRow-' + $ItemId.val()).remove();
                        $table.append(response);
                        $ItemId.val('-1'),
                        $ItemName.val('');
                        $StockInHand.val(0);
                        $IssuedItemRate.val(0);
                        $requiredQuantity.val('');
                        $IssuedQuantity.val('');
                        $MachineId.val('');
                        $Remarks.val('');
                        $SizeId.val('');
                        $BrandId.val('');
                        $OriginId.val(''),
                        $ItemName.focus();
                        RefrashModelState();
                    }

                });

            } else {
                alert("Invalid item name :" + $ItemName.val());
            }

        }
    }
}

function RefrashModelState() {
    $('form').removeData('validator');
    $('form').removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse('form');
}

$('.autocompliteEmployeeInfo').autocomplete({
    source: function (request, response) {
        var branchUnitDepartmentTextBox = $('#Branch_Unit_DepartmentId');
        var branchUnitDepartmentId = branchUnitDepartmentTextBox.val();
        var option = {
            url: "/MaterialIssueRequisition/AutocompliteGetEmployeeInfo",
            type: "POST",
            data: { employeeCardId: request.term, branchUnitDepartmentId: branchUnitDepartmentId },
        };
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            branchUnitDepartmentTextBox.validate();
            if (!branchUnitDepartmentTextBox.valid()) {
                return false;
            } else {
                $.Ajax(option)
                    .done(function (data) {
                        response($.map(data, function (employeeInfo) {
                            return { label: employeeInfo.EmployeeCardId + "_" + employeeInfo.EmployeeName, value: employeeInfo.EmployeeName, EmployeeId: employeeInfo.EmployeeId };
                        }));
                    });
            }
        }
    },
    select: function (event, ui) {
        $('.PreparedByIssueRequsition').val(ui.item.EmployeeId);
        $("#autocompliteEmployeeInfo").val(ui.item.value);

    },
    
});

function SizeWizeStockInHand(obj) {
    var $sizeDropdown = $(obj);
    var $ItemId = $('.itemHiddenField');
    var itemId = $ItemId.val();
    var sizeId = $sizeDropdown.val();
    $.Ajax({
        type: 'GET',
        dataType: "json",
        url: '/MaterialIssue/SizeWizeStockInHand',
        data: { itemId: itemId, sizeId: sizeId },
        success: function (result) {
            populateStockInfo(result);
            $('.requiredQuantity').focus();
        }, onError: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function BrandWizeStockInHand(obj) {
    var $brandDropdown = $(obj);
    var brandId = $brandDropdown.val();
    var itemId =$('.itemHiddenField').val();
    var sizeId = $('.sizeId').val();
    $.Ajax({
        type: 'GET',
        dataType: "json",
        url: '/MaterialIssue/BrandWizeStockInHand',
        data: { itemId: itemId, sizeId: sizeId, brandId: brandId },
        success: function (result) {
            populateStockInfo(result);
            $('.requiredQuantity').focus();
        }, onError: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function OriginWizeStockInHand(obj) {
    var $originDropdown = $(obj);
    var originId = $originDropdown.val();
    var itemId = $('.itemHiddenField').val();
    var sizeId = $('.sizeId').val();
    var brandId = $('.brandId').val();
    $.Ajax({
        type: 'GET',
        dataType: "json",
        url: '/MaterialIssue/OriginWizeStockInHand',
        data: { itemId: itemId, sizeId: sizeId, brandId: brandId, originId: originId },
        success: function (result) {
            populateStockInfo(result);
            $('.requiredQuantity').focus();
        }, onError: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function populateStockInfo(result) {
    $('.stockInHand').val(result.StockInHand);
    $('.issuedItemRate').val(result.Rate);
}





$('.OriginSerarcKey').unbind('keyup').bind('keyup', function () {
    var originSerarcKey = $(this).val();
    $.Ajax({
        url: '/Countries/OrigignAutocomplite/',
        data: { OriginSerarcKey: originSerarcKey }
    }).done(function (origins) {
        $('.originId').empty();
        if (origins.length > 0) {
            populateDropDown({ Id: "", CountryName: "-Select-" });
            $.each(origins, function (origin) {
                populateDropDown({ Id: this.Id, CountryName: this.CountryName });
            });
        } else {
            populateDropDown({ Id: "", CountryName: "No Origin Found" });
        }

    });
});
function populateDropDown(obj) {
    $('.originId').append(
              $('<option/>')
                  .attr('value', obj.Id)
                  .text(obj.CountryName)
          );
}