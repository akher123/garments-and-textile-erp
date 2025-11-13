$('#BatchNoTextBox').autocomplete({
    source: function (request, response) {
        var datatarget = this.element.attr('data-target');
        var partyId = $('#ddlParyId').val();
        var url = this.element.attr('action');
        var option = {
            url: url,
            type: "GET",
            datatype: "json",
            data: { searchString: request.term, partyId: partyId },
        };
        if (partyId != 'undefined' && partyId!= '') {
            $.Ajax(option)
            .done(function (data) {
                $(datatarget).val('');
                response($.map(data, function (batch) {
                    return { label: batch.BatchNo, value: batch.BatchNo, BatchId: batch.BatchId, datatarget: datatarget };
                }));
            });
        } else {
            alert('Please Select Party !!');
        }
    },
    select: function (event, ui) {
        var batchId = ui.item.BatchId;
        $(ui.item.datatarget).val(batchId);
         showBatchDetailInfo(batchId);
    },

});


function showBatchDetailInfo(batchId) {

    $.Ajax({
        url: '/FinishFabricIssue/GetBatchDeailsById',
        data: { batchId: batchId }
    }).done(function (itemList) {

        $('#ddlBatchDetailItem').empty();
        if (itemList.length > 0) {
            $('#ddlBatchDetailItem').append(
                  $('<option/>')
                      .attr('value', '')
                      .text('-Select-')
              );
            $.each(itemList, function (item) {
                $('#ddlBatchDetailItem').append(
                    $('<option/>')
                        .attr('value', this.BatchDetailId)
                        .text(this.ItemName)
                );
            });
        } else {
            $('#ddlBatchDetailItem').append(
                $('<option/>')
                    .attr('value', "No Item Found")
                    .text("Select")
            );
        }
    });
}

$('#addFabricIssueDetailButton').unbind("click").bind("click", function () {
    var addNewItem = $('#finishFabricIssueDetail :input');
    var $table = $('table#finishFabricIssueDetailTable tbody');
    var $batchId = $('#BtRefNoHiddenText').val();
    var $ddlItem = $('#ddlItem').val();
    var option = {
        url: "/FinishFabricIssue/AddNewRow/",
        type: "POST",
        data: addNewItem.serialize()
    };

    $.Ajax(option).done(function (htmlResponse) {
        if ($('table#finishFabricIssueDetailTable tbody #NewRow_' + $batchId + "_" + $ddlItem).length > 0) {
            $('table#finishFabricIssueDetailTable tbody #NewRow_' + $batchId + "_" + $ddlItem).replaceWith(htmlResponse);
        } else {
            $table.append(htmlResponse);
        }

        addNewItem.val('');
        $('#addFabricIssueDetailButton').val('+');
        $('#backButton').val('Back');
        $('#BatchNoTextBox').focus();
    });

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

$('#finishFabricIssueSaveButton').unbind('click').bind('click', function () {
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

$('#ddlBatchDetailItem').unbind('change').bind('change', function () {
    var batchDetailId = $(this).val();
    $('#txtItemName').val($('#ddlBatchDetailItem :selected').text());
    GetBatchbatchQtyByBatchDetailId(batchDetailId);
});

function GetBatchbatchQtyByBatchDetailId(batchDetailId) {
    $.Ajax({
        url: '/FinishFabricIssue/GetBatchbatchQtyByBatchDetailId',
        type: 'GET',
        dataType: 'JSON',
        data: { batchDetailId: batchDetailId }
    }).done(function (fabIssue) {
        $('#IssueQty').val(fabIssue.IssueQty);
        $('#FabQty').val(fabIssue.FabQty);
        $('#GreyWt').val(fabIssue.GreyWt);
        $('#CcuffQty').val(fabIssue.CcuffQty);
    });
}
