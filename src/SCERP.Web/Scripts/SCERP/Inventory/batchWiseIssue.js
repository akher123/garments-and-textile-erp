
$('#BatchNoTextBox').autocomplete({
    source: function (request, response) {
        var datatarget = this.element.attr('data-target');
        var url = this.element.attr('action');
        var option = {
            url: url,
            type: "GET",
            datatype: "json",
            data: { searchString: request.term },
        };
        $.Ajax(option)
            .done(function (data) {
                $(datatarget).val('');
                response($.map(data, function (batch) {
                    return { label: batch.BatchNo, value: batch.BatchNo, BtRefNo: batch.BtRefNo, datatarget: datatarget };
                }));
            });
    },
    select: function (event, ui) {
        var btRefNo = ui.item.BtRefNo;
        $(ui.item.datatarget).val(btRefNo);
        showBatchInfo(btRefNo);
    },

});

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
                        return { label: item .ItemCode+ "_" + item.ItemName + ", ROL :" + item.ReorderLevel, value: item.ItemName, ItemId: item.ItemId };
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
                        $('.stockInHand').val(result.StockInHand);
                        $('.issuedItemRate').val(result.Rate);
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


$('.bwi_remarks').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('.itemName').focus();
    }

});

$('.itemName').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('#saveBwIsButton').focus();
    }

});

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
    var $ItemName = $('.itemName');
    var $StockInHand = $('.stockInHand');
    var $IssuedQuantity = $('.issuedQuantity');
    var $IssuedItemRate = $('.issuedItemRate');
    var $requiredQuantity = $('.requiredQuantity');

    var $Remarks = $('.remarks');
    var viewmodel = {
        "IssueDetail": {
            "ItemId": $ItemId.val(),
            "MaterialIssueDetailId": $mIDetailId.val(),
            "ItemName": $ItemName.val(),
            "StockInHand": $StockInHand.val() == "" ? 0 : $StockInHand.val(),
            "IssuedItemRate": $IssuedItemRate.val() == "" ? 0 : $IssuedItemRate.val(),
            "RequiredQuantity": $requiredQuantity.val() == "" ? 0 : $requiredQuantity.val(),
            "IssuedQuantity": $IssuedQuantity.val() == "" ? 0 : $IssuedQuantity.val(),
            "Remarks": $Remarks.val() == "" ? 0 : $Remarks.val(),
        },
    };
    var $table = $('table#BatchWiseIssueDetail tbody');
    var option = {
        url: "/BatchWiseIssue/AddNewRow",
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
                    if (response.Success==false) {
                        alert(response.Message);
                    } else {
                        $('#newRow-' + $ItemId.val()).remove();
                        $table.append(response);
                        //if ($table.find('tr').length > 0) {
                        //    $('#newRow-' + $ItemId.val()).replaceWith(response);
                        //} else {
                        //    $table.append(response);
                        //}
                   
                        $ItemId.val('-1'),
                        $ItemName.val('');
                        $StockInHand.val('');
                        $IssuedItemRate.val('');
                        $requiredQuantity.val('');
                        $IssuedQuantity.val('');
                        $Remarks.val('');
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

function showBatchInfo(refId) {
    var batchRefNo = refId;
    $("#batchInfo").hide("slide", { direction: "right" }, 1000);
    var option = {
        url: "/Batch/GeBachInfo",
        type: "GET",
        contentType: "application/json",
        data: { btRefNo: batchRefNo }

    };
    $.Ajax(option).done(function (batchInfo) {
        var $divs = '<div>' + 'Party :' + batchInfo.PartyName + ' &nbsp; &nbsp;' + 'Machine Name :' + batchInfo.MachineName + '</div>';
        $divs += '<div>' + 'Color:' + batchInfo.GColorName + '  and ' + 'Quantity :' + batchInfo.BatchQty + '</div>';
        $('#batchInfo').html($divs);
        $("#batchInfo").show("slide", { direction: "left" }, 1000);
        // $("#batchInfo").show('slow');
        $('#batch_qty').val(batchInfo.BatchQty);
    });
}

$('#batch_topping').on('change', function() {
    var id = $(this).val();
    var input = $('#batch_qty');
    if (id==1) {
        input.prop('readonly', true);
        input.css("background-color", "pink");
    } else {
        input.prop('readonly', false);
        input.css("background-color", "");
    }
});
function RefrashModelState() {
    $('form').removeData('validator');
    $('form').removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse('form');
}

