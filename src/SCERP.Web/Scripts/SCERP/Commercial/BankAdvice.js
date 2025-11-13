
$('#BankAdviceTableFixed').append('<tr>' +
    '<td> </td>' +
    '<td> </td>' +
    '<td> </td>' +
    '<td> </td>' +
    '<td> </td>' +
    '<td> </td>' +
    '<td> <b>Balance : </b></td>' +
    '<td> <b> <div id="total"> </div></b></td>' +
    '</tr>'
);

$('#BankAdviceTable').append('<tr>' +
    '<td> </td>' +
    '<td> </td>' +
    '<td> </td>' +
    '<td> </td>' +
    '<td> <b>Balance : </b></td>' +
    '<td> <b> <div id="total2"> </div></b></td>' +
    '</tr>'
);


function Calculate() {

    var TotalAmount = 0;

    $('#BankAdviceTableFixed tr').each(function () {

        if (!this.rowIndex) return; // skip first row
               
        var amount = this.cells[5].getElementsByClassName('amountcc')[0].value;
        var rate = this.cells[6].getElementsByClassName('ratecc')[0].value;
                  
        var amountInTaka = amount * rate;
        TotalAmount = -amountInTaka - TotalAmount;

        this.cells[7].getElementsByClassName('amountInTakacc')[0].value = amountInTaka.toFixed(2);
        document.getElementById('total').innerHTML = TotalAmount.toFixed(2);
        document.getElementById('total2').innerHTML = TotalAmount.toFixed(2);
    });
}

function Calculate2() {

    var TotalAmount = 0;
    
    $('#BankAdviceTable tr').each(function () {

        if (!this.rowIndex) return; // skip first row

        var amount = this.cells[5].getElementsByClassName('amountInTakacc2')[0].value;
        
        //if (amount.length > 0)
        //    TotalAmount = TotalAmount + parseFloat(amount);
    });

    document.getElementById('total2').innerHTML = "how are you?";
}

jQuery('.amountcc').on('input', function() {
              
    Calculate();
});


jQuery('.ratecc').on('input', function() {
  
    Calculate();
});

jQuery('.amountInTakacc2').on('input', function () {

    Calculate2();
});


function addNewRowButton() {

    var headName = $('#rd_AccHeadName').val();
    var particulars = $('#rd_Particulars').val();
    var bankRefNo = $('#rd_BankRefNo').val();
    var amount = $('#rd_Amount').val();

    var bankAdviceModel = {
        "BankAdvice": {
            "AccHeadId": 1,
            "ExportId":2,
            "AccHeadName": headName,
            "Particulars": particulars,
            "BankRefNo": bankRefNo,
            "Amount": amount               
        },
    };    

    var $table = $('table#BankAdviceTable tbody');

    var option = {
        url: "/BankAdvice/AddNewRow",
        type: "POST",
        datatype: "html",
        contentType: "application/json",
        data: JSON.stringify(bankAdviceModel)
    };

    if (jQuery.validator && jQuery.validator.unobtrusive) {

        if (parseInt(bankAdviceModel.BankAdvice.Amount) > 0) {

            $.ajax(option).done(function(respons) {
                if (respons.Success == false && respons.Success != 'undefined') {
                    alert(respons.Message);
                } else {

                    if ($table.find('tr').length > 0) {
                        if ($table.find('#row-' + bankAdviceModel.BankAdvice.AccHeadId).length > 0) {
                            $('#row-' + bankAdviceModel.BankAdvice.AccHeadId).replaceWith(respons);
                        } else {
                            $table.append(respons);
                        }
                    } else {
                        $table.append(respons);
                    }
                    ViewModel.ClearBankAdvice();
                }
            });
        } else {
            alert("Head Information not Correct  !");
        }
    }
}

$('#rd_Amount').unbind("keypress").bind("keypress", function(e) {

    var key = e.which;

    if (key == 13) {

        var amount = $(this).val();

        if (parseFloat(amount) > 0) {          
            addNewRowButton();
            $('#rd_AccHeadName').focus();
        } else {
            $('#rd_AccHeadName').focus();
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

    ClearBankAdvice: function() {

        $('#rd_AccHeadName').val('');
        $('#rd_Particulars').val('');
        $('#rd_BankRefNo').val('');
        $('#rd_Amount').val('');
    },
};