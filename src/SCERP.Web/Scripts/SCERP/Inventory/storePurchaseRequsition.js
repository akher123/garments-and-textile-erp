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
                        return { label: item.ItemCode + "_" + item.ItemName + ", ROL :" + item.ReorderLevel, value: item.ItemName, ItemId: item.ItemId };
                    }));
                });
        },
        select: function (event, ui) {
            var itemId = ui.item.ItemId;
            $('.itemHiddenField').val(itemId);
            $.Ajax({
                type: 'GET',
                dataType: "json",
                url: '/StorePurchaseRequisition/PresentStockInfo',
                data: { itemId: itemId },
                success: function (result) {
                    if (result.Success == false) {
                        alert(result.Message);
                    } else {
                        populateStockeInfo(result);
                        $('.description').focus();

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
$('.description').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.requiredQuantity').focus();
    }

});
$('.requiredQuantity').unbind("keypress").bind("keypress", function (e) {

    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.approvedQuantity').focus();
    }

});
$('.approvedQuantity').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.desiredDate').focus();
    }
});
$('.desiredDate').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.functionalArea').focus();
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
    var $ItemName = $('.itemName');
    var $SuppliedUptoDate = $(".suppliedUptoDate");
    var $StockInHand = $('.stockInHand');
    var $Description = $('.description');
    var $DesiredDate = $('.desiredDate');

    var $ApprovedQuantity = $('.approvedQuantity');
    var $requiredQuantity = $('.requiredQuantity');
    var $LastUnitPrice = $('.lastUnitPrice');
    var $FunctionalArea = $('.functionalArea');
    var $table = $('table#GeneralIssueDetail tbody');
    var option = {
        url: "/StorePurchase/AddNewRow",
        type: "POST",
        datatype: "html",
        //  contentType: "application/json",
        data: $('.StorePurchase').serialize(),
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
                        //    $table.append(response);
                        //}
                        $('#newRow-' + $ItemId.val()).remove();
                        $table.append(response);
                        $ItemId.val('-1'),
                        $DesiredDate.val('');
                        $ItemName.val('');
                        $StockInHand.val('');
                        $LastUnitPrice.val('');
                        $SuppliedUptoDate.val('');
                        $requiredQuantity.val('');
                        $Description.val('');
                        $ApprovedQuantity.val('');
                        $FunctionalArea.val('');
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
        var branchUnitDepartmentTextBox = $('#branch_Unit_DepartmentId');
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
        $('.RequsitionPerson').val(ui.item.EmployeeId);
        $("#autocompliteEmployeeInfo").val(ui.item.value);

    },

});

function SizePresentStockInfo(obj) {
    var $ItemId = $('.itemHiddenField');
    var $SizeId = $('.sizeId');

    $.Ajax({
        type: 'GET',
        dataType: "json",
        url: '/StorePurchaseRequisition/PresentStockInfo',
        data: { itemId: $ItemId.val(), sizeId: $SizeId.val() },
        success: function (result) {
            populateStockeInfo(result);
            $('.description').focus();
        }, onError: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function BrandPresentStockInfo(obj) {
    var $ItemId = $('.itemHiddenField');
    var $SizeId = $('.sizeId');
    var $BrandId = $('.brandId');
    $.Ajax({
        type: 'GET',
        dataType: "json",
        url: '/StorePurchaseRequisition/PresentStockInfo',
        data: { itemId: $ItemId.val(), sizeId: $SizeId.val(), brandId: $BrandId.val() },
        success: function (result) {
            populateStockeInfo(result);
            $('.description').focus();
        }, onError: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function OriginPresentStockInfo(obj) {
    var $ItemId = $('.itemHiddenField');
    var $SizeId = $('.sizeId');
    var $BrandId = $('.brandId');
    var $OriginId = $('.originId');
    $.Ajax({
        type: 'GET',
        dataType: "json",
        url: '/StorePurchaseRequisition/PresentStockInfo',
        data: { itemId: $ItemId.val(), sizeId: $SizeId.val(), brandId: $BrandId.val(), originId: $OriginId.val() },
        success: function (result) {
            populateStockeInfo(result);
        }, onError: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function populateStockeInfo(result) {
    $('.stockInHand').val(result.StockInHand);
    $('.suppliedUptoDate').val(result.SuppliedUptoDate);
    $('.lastUnitPrice').val(result.LastUnitPrice);
}
//$('.originId').combobox();




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