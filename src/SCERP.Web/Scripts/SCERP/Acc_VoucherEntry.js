
var ViewModel = {
    totalDebit: function () {
        var total = 0;
        $('#VoucherDetailTable tbody tr').each(function () {
            var $tr = $(this);
            var debit = $tr.find('td:eq(2)').find("input[type=hidden]").val();
            total += parseFloat(debit);
        });
        $('.TotalDebitAmount').val(total.toFixed(2));

        return total;


    },
    totalCredit: function () {
        var total = 0;
        $('#VoucherDetailTable tbody tr').each(function () {
            var $tr = $(this);
            var credit = $tr.find('td:eq(3)').find("input[type=hidden]").val();
            total += parseFloat(credit);
        });

        $('.TotalCreditAmount').val(total.toFixed(2));

        return total;
    },
    totalAmount: function () {
        var total = (parseFloat(this.totalDebit()) - parseFloat(this.totalCredit())).toFixed(2);
        $('.TotalAmount').val(total);
    },
    Number2Word: function () {
        var value = this.totalDebit();
        var wordValue = number2text(value);
        $('.AmountInWords').val(wordValue);
    },

    DeleteDuplicateRow: function (glId) {
        $('#row-' + glId).remove();

    },
    DeleteTR: function (obj) {
        var $tr = $(obj).closest("tr");
        $tr.remove();
        this.totalDebit();
        this.totalCredit();
        this.totalAmount();
        this.Number2Word();
    },
    ClearVoucherDetail: function () {
        $('#vd_AccountName').val('');
        $('#vd_GLID').val('');
        $('#vd_Particulars').val('');
        $('#vd_Debit').val('');
        $('#vd_Credit').val('');

    },


};

function AddCashInHandRow() {
    $('#row-156').remove();
    ViewModel.totalAmount();
    var amount = $('.TotalAmount').val();
    if ($('.VoucherType').val() === 'CP' || $('.VoucherType').val() === 'CR') {
        var $table = $('table#VoucherDetailTable tbody');
        var voucherDetailModel = {
            "VoucherType": $('.VoucherType').val(),
            "VoucherDetail": {
                "GLID": 156,
                "Particulars": '',
                "Debit": parseFloat(amount) < 0 ? (0 - amount) : 0,
                "Credit": parseFloat(amount) > 0 ? amount : 0,
                //"CostCentreId": $(".CostCentreNew option:selected").val() + '-' + $(".CostCentreNew option:selected").text(),
            },
        };
        var option = {
            url: "/VoucherEntry/AddNewRow",
            type: "POST",
            datatype: "html",
            contentType: "application/json",
            data: JSON.stringify(voucherDetailModel)
        };
        if (amount != 0) {
            $.ajax(option).done(function (respons) {
                $table.append(respons);
                ViewModel.ClearVoucherDetail();
                ViewModel.totalDebit();
                ViewModel.totalCredit();
                ViewModel.totalAmount();
                ViewModel.Number2Word();
            });
        }
    }

}

function addNewRowButton() {
     
    var voucherType = $('.VoucherType').val();
    var $avdAcountName = $('#vd_AccountName');
    var $vdGLID = $('#vd_GLID');
    var $vdParticulars = $('#vd_Particulars');
    var voucherDetailModel = {
        "AccountName": $avdAcountName.val(),
        "VoucherDetail": {
            "GLID": $vdGLID.val(),
            "Particulars": $vdParticulars.val(),
            "Debit": $('#vd_Debit').val() == "" ? 0 : $('#vd_Debit').val(),
            "Credit": $('#vd_Credit').val() == "" ? 0 : $('#vd_Credit').val(),
            "CostCentreId": $(".CostCentreNew option:selected").val(),
        },
    }; 
    var $table = $('table#VoucherDetailTable tbody');
    var option = {
        url: "/VoucherEntry/AddNewRow",
        type: "POST",
        datatype: "html",
        contentType: "application/json",
        data: JSON.stringify(voucherDetailModel)
    };
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        $avdAcountName.validate();
        if (!$avdAcountName.valid()) {
            return false;
        } else {
            if (parseInt(voucherDetailModel.VoucherDetail.GLID) > 0) {

                $.ajax(option).done(function (respons) {
                    if (respons.Success == false && respons.Success != 'undefined') {
                        alert(respons.Message);
                    } else {

                        if ($table.find('tr').length > 0) {
                            if ($table.find('#row-' + voucherDetailModel.VoucherDetail.GLID).length > 0) {
                                $('#row-' + voucherDetailModel.VoucherDetail.GLID).replaceWith(respons);
                            } else {
                                $table.append(respons);
                            }

                        } else {

                            $table.append(respons);

                        }
                        if (voucherType !== 'CV') {
                            AddCashInHandRow();
                        }
                        // ViewModel.DeleteDuplicateRow(voucherDetailModel.VoucherDetail.GLID);
                        //   $table.append(respons);
                        ViewModel.ClearVoucherDetail();
                        ViewModel.totalDebit();
                        ViewModel.totalCredit();
                        ViewModel.totalAmount();
                        ViewModel.Number2Word();

                        $('#vd_AccountName').focus();

                    }
                });
            } else {
                alert("GL Account not correct" + voucherDetailModel.VoucherDetail.GLID);
            }
        }
    }
}


$('#addNewRowButton').click(addNewRowButton);

$('#vd_AccountName').autocomplete({
    source: function (request, response) {
        var option = {
            url: "/VoucherEntry/AutocompliteGLAccount",
            type: "POST",
            data: { accountName: request.term },
        };
        $.Ajax(option)
            .done(function (data) {
                response($.map(data, function (item) {
                    return { label: item.AccountName, value: item.AccountName + "-" + item.AccountCode, GLID: item.Id };
                }));
            });
    },
    select: function (event, ui) {
        $('#vd_GLID').val(ui.item.GLID);
        $('#vd_Particulars').focus();

    },
});

$('.VoucherPost').unbind('click').bind(function () {
    var button = $(this);
    var form = button.parents("form:first");
    var action = form.attr('action');
    document.title = form.attr('title');
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {
            return false;
        } else {
            jQuery.ajax({
                type: "post",
                url: action,
                data: form.serialize()
                , success: function (r) {
                    window.loadAction.reload();
                }
            });

        }
    }
});
$('.voucherRefNo').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;

    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('#vd_AccountName').focus();
    }

});

$('#vd_AccountName').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        var accName = $(this).val();
        var $vdGLID = $('#vd_GLID').val();

        if (accName.length > 0 && parseInt($vdGLID) > 0) {
            $('#vd_Particulars').focus();
        } else {

            $('.VoucherParticulars').focus();
        }
        e.preventDefault();
    }

});
$('#vd_Particulars').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('#vd_Debit').focus();
    }
});

$('#vd_Debit').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        var debitAmount = $(this).val();
        if (parseFloat(debitAmount) > 0) {
            addNewRowButton();
            $('#vd_AccountName').focus();
        } else {
            $('#vd_Credit').focus();
        }
        e.preventDefault();
    }



});
$('#vd_Credit').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        var debitAmount = $(this).val();
        if (parseFloat(debitAmount) > 0) {
            addNewRowButton();
            $('#vd_AccountName').focus();
        } else {
            $('#vd_AccountName').focus();
        }
        e.preventDefault();

    }

});

$('.VoucherParticulars').unbind("keypress").bind("keypress", function (e) {
    var key = e.which;
    if (key == 13)  //the enter key code
    {
        e.preventDefault();
        $('#btnVoucherSave').focus();
    }
});

function number2text(value) {
    var fraction = Math.round(frac(value) * 100);
    var f_text = "";

    if (fraction > 0) {
        f_text = "AND " + convert_number(fraction) + " PAISE";
    }

    var curr = $('#ActiveCurrencyId option:selected').text();

    if (curr == '')
        curr = "TAKA";
    else if (curr == 'BDT')
        curr = 'TAKA';
    else if (curr == 'EUR')
        curr = 'EURO';
    else if (curr == 'USD')
        curr = 'US DOLLAR';

    return convert_number(value) + " " + curr + " " + f_text + " ONLY";
}

function frac(f) {
    return f % 1;
}

function convert_number(number) {
    if ((number < 0) || (number > 999999999)) {
        return "NUMBER OUT OF RANGE!";
    }
    var Gn = Math.floor(number / 10000000);  /* Crore */
    number -= Gn * 10000000;
    var kn = Math.floor(number / 100000);     /* lakhs */
    number -= kn * 100000;
    var Hn = Math.floor(number / 1000);      /* thousand */
    number -= Hn * 1000;
    var Dn = Math.floor(number / 100);       /* Tens (deca) */
    number = number % 100;               /* Ones */
    var tn = Math.floor(number / 10);
    var one = Math.floor(number % 10);
    var res = "";

    if (Gn > 0) {
        res += (convert_number(Gn) + " CRORE");
    }
    if (kn > 0) {
        res += (((res == "") ? "" : " ") +
        convert_number(kn) + " LAKH");
    }
    if (Hn > 0) {
        res += (((res == "") ? "" : " ") +
            convert_number(Hn) + " THOUSAND");
    }

    if (Dn) {
        res += (((res == "") ? "" : " ") +
            convert_number(Dn) + " HUNDRED");
    }


    var ones = Array("", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN");
    var tens = Array("", "", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY");

    if (tn > 0 || one > 0) {
        if (!(res == "")) {
            res += " AND ";
        }
        if (tn < 2) {
            res += ones[tn * 10 + one];
        }
        else {

            res += tens[tn];
            if (one > 0) {
                res += ("-" + ones[one]);
            }
        }
    }

    if (res == "") {
        res = "zero";
    }
    return res;
}


function getVoucherNoByType(value) {

    var date = $('#VoucherDate').val();

    jQuery.ajax({
        url: "/VoucherEntry/GetVoucherNoByType",
        data: { "type": value, "date": date },
        type: "POST",
        success: function(r) {
            if (!r.Success) {
                alert("Voucher no did not found !");
            } else if (r.Success) {
                $('#VoucherRefNo').val(r.data);
            }
        }
    });
}


function GetCurrencyById(value) {

    jQuery.ajax({
        url: "/VoucherEntry/GetCurrencyById",
        data: { "id": value },
        type: "POST",
        success: function (r) {
            if (!r.Success) {

            } else if (r.Success) {
                $('#CurrencyRate').val(r.data);
            }
        }
    });
}