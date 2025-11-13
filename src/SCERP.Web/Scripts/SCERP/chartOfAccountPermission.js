$('.tree li').each(function () {
    if ($(this).children('ul').length > 0) {
        $(this).addClass('parent');
    }
});

$('.tree li.parent > a').click(function () {
    $(this).parent().toggleClass('active');
    $(this).parent().children('ul').slideToggle('fast');
});
$('#all').click(function () {
    $('.tree li').each(function () {
        $(this).toggleClass('active');
        $(this).children('ul').slideToggle('fast');
    });
});

$('#saveChartOfAccountPermission').click(function () {
    var form = $('.ChartOfAccountPermission');

    if ($('#companySectorId').val() != "") {
        $.ajax({
            url: '/Accounting/ControlAccount/SavePermitedChartOfAccount',
            type: 'POST',
            dataType: 'json',
            data: form.serialize(),
            success: function (data) {
                if (data.Success) {
                    alert("Chart of accunt permission successfully saved");
                } else {
                    alert("Chart of accunt permission not  saved");
                }
            },
            error: function (request, status, error) {
                alert(request.responseText);
            }

        });
    } else {
        $('#companySectorId').css('border-color', 'red');
    }

});

function clearCheckBoxes() {
    $('.chartofAcc').prop("checked", false);
}
$('#companySectorId').change(function () {
    var companySectorId = $(this).val();
    $(this).css('border-color', '');
    if (companySectorId != '') {

        $.ajax({
            url: '/Accounting/ControlAccount/GetPermitedChartOfAccount/',
            type: 'GET',
            data: { "companySectorId": companySectorId },
            success: function (data) {
                clearCheckBoxes();
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        $('#' + data[i].ControlCode + '_' + data[i].ControlLevel).prop("checked", true);
                    }
                }
            },
            error: function (request, status, error) {
                alert(request.responseText);
            }
        });
    } else {
        clearCheckBoxes();
    }
});


//$('.chartofAcc').click(function () {
//    var response = $(this).attr('id').split('_')[0];
//    var firstIndexNumber = response.slice(0, 1);
//    if ($(this).is(':checked')) {
//        $('input:checkbox.chartofAcc').each(function () {
//            var checkedId = $(this).attr('id');
//            var firstIndexNumberOfcheckedId = $(this).attr('id').split('_')[0].slice(0, 1);
//            if (firstIndexNumber == firstIndexNumberOfcheckedId) {
//                $('#' + checkedId).prop("checked", true);
//            }
//        });
//    } else {
//        $('input:checkbox.chartofAcc').each(function () {
//            var checkedId = $(this).attr('id');
//            var firstIndexNumberOfcheckedId = $(this).attr('id').split('_')[0].slice(0, 1);
//            if (firstIndexNumber == firstIndexNumberOfcheckedId) {
//                $('#' + checkedId).prop("checked", false);
//            }
//        });
//    }


//});

//$('.chartofAcc').change( function(){
//    $(this).parent().siblings().find(':checkbox').attr('checked', this.checked);
//});​
$("input[type='checkbox'].chartofAcc").change(function () {
    $(this).siblings('ul')
           .find("input[type='checkbox']")
           .prop('checked', this.checked);
});