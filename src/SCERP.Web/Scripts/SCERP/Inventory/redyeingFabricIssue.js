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
        if (partyId != 'undefined' && partyId != '') {
            $.Ajax(option)
                .done(function (data) {
                    $(datatarget).val('');
                    response($.map(data, function (batch) {
                        return { label: batch.Text, value: batch.Text, BatchId: batch.Value, datatarget: datatarget };
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
        url: '/RedyeingFabricIssue/GetRedyeingReceivedBatchDetailByBatchId',
        data: { batchId: batchId }
    }).done(function (itemList) {
        var items = JSON.parse(itemList);
        $('#ddlBatchDetailItem').empty();
        if (items.length > 0) {
            $('#ddlBatchDetailItem').append(
                $('<option/>')
                .attr('value', '')
                .text('-Select-')
            );
            $.each(items, function (item) {
               
                $('#ddlBatchDetailItem').append(
                    $('<option/>')
                    .attr('value', this.BatchDetailId)
                    .attr('data-balanceqty', this.BalanceQty)
                   
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

$('#addRedyeingFabricIssueDetailButton').unbind("click").bind("click", function () {
    var addNewItem = $('#RedyeingFabricIssueDetail :input');
    var $table = $('table#redyeingFabricReceiveDetailTable tbody');
    var $batchId = $('#BtRefNoHiddenText').val();
    var $ddlItem = $('#ddlItem').val();
    var option = {
        url: "/RedyeingFabricIssue/AddNewRow/",
        type: "POST",
        data: addNewItem.serialize()
    };

    $.Ajax(option).done(function (htmlResponse) {
        if ($('table#redyeingFabricReceiveDetailTable tbody #NewRow_' + $batchId + "_" + $ddlItem).length > 0) {
            $('table#redyeingFabricReceiveDetailTable tbody #NewRow_' + $batchId + "_" + $ddlItem).replaceWith(htmlResponse);
        } else {
            $table.append(htmlResponse);
        }

        addNewItem.val('');
        $('#addRedyeingFabricIssueDetailButton').val('+');
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


$('#ddlBatchDetailItem').unbind('change').bind('change', function () {
    $('#txtItemName').val($('#ddlBatchDetailItem :selected').text());
    var selected = $(this).find('option:selected');
    var balanceqty = selected.data('balanceqty');
    $('#BalanceQty').val(balanceqty);

});


