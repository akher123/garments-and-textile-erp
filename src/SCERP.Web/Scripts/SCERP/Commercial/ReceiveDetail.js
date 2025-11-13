
function addNewRowButton() {

    var itemCode = $('#rd_ItemCode').val();
    var quantity = $('#rd_Quantity').val();
    var rate = $('#rd_Rate').val();

    var receiveDetailModel = {
        "ReceiveDetail": {
            "ItemCode": itemCode,
            "Quantity": quantity,
            "Rate": rate,
        },
    };

    var $table = $('table#ReceiveDetailTable tbody');

    var option = {
        url: "/Receive/AddNewRow",
        type: "POST",
        datatype: "html",
        contentType: "application/json",
        data: JSON.stringify(receiveDetailModel)
    };

    if (jQuery.validator && jQuery.validator.unobtrusive) {

        if (parseInt(receiveDetailModel.ReceiveDetail.Quantity) > 0) {
            $.ajax(option).done(function(respons) {
                if (respons.Success == false && respons.Success != 'undefined') {
                    alert(respons.Message);
                } else {

                    if ($table.find('tr').length > 0) {
                        if ($table.find('#row-' + receiveDetailModel.ReceiveDetail.ReceiveId).length > 0) {
                            $('#row-' + receiveDetailModel.ReceiveDetail.ReceiveId).replaceWith(respons);
                        } else {
                            $table.append(respons);
                        }
                    } else {
                        $table.append(respons);
                    }
                    ViewModel.ClearVoucherDetail();
                }
            });
        } else {
            alert("Receive Item is not Correct  !");
        }
    }
}


$('#rd_Rate').unbind("keypress").bind("keypress", function(e) {

    var key = e.which;

    if (key == 13) {
        var debitAmount = $(this).val();
        if (parseFloat(debitAmount) > 0) {
            addNewRowButton();
            $('#rd_Rate').focus();
        } else {
            $('#rd_Rate').focus();
        }
        e.preventDefault();
    }
});


var ViewModel = {    
    DeleteDuplicateRow: function(glId) {
        $('#row-' + glId).remove();
    },

    DeleteTR: function(obj) {
        var $tr = $(obj).closest("tr");
        $tr.remove();
    },

    ClearVoucherDetail: function() {
        $('#rd_ItemCode').val('');
        $('#rd_Quantity').val('');
        $('#rd_Rate').val('');
        $('#rd_ItemCode').focus();
    },
};