
function editRow(key) {
     $('#BatchNoTextBox').val($('#BatchNo_' + key).val());
     $('#BtRefNoHiddenText').val($('#BatchId_' + key).val());
     $('#ddlItem').val($('#BatchDetailId_' + key).val());
     $('#txtItemName').val($('#ItemName_' + key).val());
     $('#ddlSpGroup').val($('#SpGroupId_' + key).val());
     $('#txtGroupName').val($('#GroupName_' + key).val());
     $('#GreyWeight').val($('#GreyWeight_' + key).val());
     $('#FinishWeight').val($('#FinishWeight_' + key).val());
     $('#Rate').val($('#Rate_' + key).val());
     $('#CcuffQty').val($('#CcuffQty_' + key).val());
     $('#Remarks').val($('#Remarks_' + key).val());
    }


$('#btnSaveDyeingSubprocess').unbind('click').bind('click', function () {
    var button = $(this);
    var form = button.parents("form:first");
    var url = form.attr('action');
    var data = form.serialize();
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {
            return false;
        } else {
            jQuery.Ajax({
                url: url,
                type: "POST",
                data: data,
                container: "#contentRight"
            });
        }

    }
});

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
                    return { label: batch.BatchNo, value: batch.BatchNo, BatchId: batch.BatchId, datatarget: datatarget };
                }));
            });
    },
    select: function (event, ui) {
        var batchId = ui.item.BatchId;
        $('#BatchIdNoHiddenText').val(batchId);
      
        showBatchDetailInfo(batchId);
    },

});


function showBatchDetailInfo(batchId) {
   
    $.Ajax({
        url: '/DyeingSpChallan/GetBatchById',
        data: { batchId: batchId }
    }).done(function (itemList) {

        $('#ddlItem').empty();
        if (itemList.length > 0) {
            $('#ddlItem').append(
                  $('<option/>')
                      .attr('value','')
                      .text('-Select-')
              );
            $.each(itemList, function (item) {
                $('#ddlItem').append(
                    $('<option/>')
                        .attr('value', this.BatchDetailId)
                        .text(this.ItemName)
                );
            });
        } else {
            $('#ddlItem').append(
                $('<option/>')
                    .attr('value', "No Item Found")
                    .text("Select")
            );
        }
    });
}

$('#ddlItem').unbind('change').bind('change', function () {
    var batchDetailId = $(this).val();
    $('#txtItemName').val($('#ddlItem :selected').text());
    GetBatchDetailItemQty(batchDetailId);
});

function GetBatchDetailItemQty(batchDetailId) {
    $.Ajax({
        url: '/DyeingSpChallan/GetBatchItemQtyByBatchDetailId',
        data: { batchDetailId: batchDetailId }
    }).done(function (itemQty) {
        var items = JSON.parse(itemQty);
        $('#GreyWeight').val(items[0].BalanceWt);
        $('#FinishWeight').val(items[0].BalanceWt);
        $('#CcuffQty').val(items[0].CcuffQty);
        
    });
}

$('#ddlSpGroup').unbind('change').bind('change', function () {
    $('#txtGroupName').val($('#ddlSpGroup :selected').text());
});

function deleteRow(buttonObj) {
    var $btnObj = $(buttonObj);
    var cltr = $btnObj.closest('tr');
    jQuery.Confirm(SCERP.Messages.DeleteConfirmation, function (r) {
        if (r) {
            cltr.remove();
        }
    });
}

$('input[type=text]').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13) //the enter key code
    {
        e.preventDefault();
    }

});


$('#addBactchButton').unbind("click").bind("click", function () {
    var addNewItem = $('#DyeingSpChallanInput :input');
    var $table = $('table#dyeingSpChallanListTable tbody');
    var $batchId = $('#BtRefNoHiddenText').val();
    var $ddlItem = $('#ddlItem').val();
    var option = {
        url: "/DyeingSpChallan/AddNewRow/",
        type: "POST",
        data: addNewItem.serialize()
    };

    $.Ajax(option).done(function (htmlResponse) {
        if ($('table#dyeingSpChallanListTable tbody #NewRow_' + $batchId + "_" + $ddlItem).length>0) {
            $('table#dyeingSpChallanListTable tbody #NewRow_' + $batchId + "_" + $ddlItem).replaceWith(htmlResponse);
        } else {
            $table.append(htmlResponse);
        }
  
        addNewItem.val('');
        $('#BatchNoTextBox').focus();
    });

});