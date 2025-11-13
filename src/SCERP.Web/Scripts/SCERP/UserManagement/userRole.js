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


$('.check').change(function () {
    var checkedId = $(this).attr('id');
    var id = checkedId.split('__')[0];

    if ($(this).val().split('_')[1] == 1) {

        $('#' + 'check__' + $(this).val().split('_')[0]).prop("checked", $(this).is(':checked'));
    }
    if ($(this).val().split('_')[1] == 2) {
        $('#' + id + '__1').prop("checked", $(this).is(':checked'));
        $('#' + 'check__' + $(this).val().split('_')[0]).prop("checked", $(this).is(':checked'));
    }
    if ($(this).val().split('_')[1] == 3) {
        $('#' + id + '__1').prop("checked", $(this).is(':checked'));
        $('#' + id + '__2').prop("checked", $(this).is(':checked'));
        $('#' + 'check__' + $(this).val().split('_')[0]).prop("checked", $(this).is(':checked'));
    }
    if ($(this).attr('id') == 'check__' + $(this).val()) {

        $('#' + 'check_' + $(this).val() + '__1').prop("checked", $(this).is(':checked'));
    }
});

$('#saveUserRole').click(function () {

    var form = $('.userRoleForm');
    if ($('#UserName').val() == "") {
        $('#UserName').css('border-color', "red");
    } else {


        if (form.validate()) {
            jQuery.Confirm(SCERP.Messages.SendConfirmation, function (r) {
                if (r) {
                    $.ajax({
                        url: '/UserManagement/UserRole/Save/',
                        type: 'POST',

                        data: form.serialize(),
                        success: function (data) {
                            if (data.Success === true) {
                                alert('User role  saved');
                            } else {
                                alert('Error');
                            }

                        },
                        error: function (request, status, error) {
                            alert(request.responseText);
                        }
                    });
                }
            });

        } else {
       
            alert('Invalid');
        }
    }
});


$('#saveNewUserRole').click(function () {

    var form = $('.userRoleForm');
    if ($('#UserName').val() == "") {
        $('#UserName').css('border-color', "red");
    } else {
        

        if (form.validate()) {
            jQuery.Confirm(SCERP.Messages.CopyPastConfirmation, function (r) {
                if (r) {
                    $.ajax({
                        url: '/UserManagement/UserRole/CopySave/',
                        type: 'POST',

                        data: form.serialize(),
                        success: function (data) {
                            if (data.Success === true) {
                                alert('User role  saved');
                            } else {
                                alert('Error');
                            }

                        },
                        error: function (request, status, error) {
                            alert(request.responseText);
                        }
                    });
                }
            });

         
        } else {

            alert('Invalid');
        }
    }
});

function clearCheckBoxes() {
    $('.check').prop("checked", false);
}
$('#UserName').change(function () {
    var userName = $(this).val();
    $(this).css('border-color', "#696969");
    $.ajax({
        url: '/UserManagement/UserRole/GetUserRole/',
        type: 'GET',
        data: { "userName": userName },
        success: function (data) {
            clearCheckBoxes();
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    $('#' + 'check__' + data[i].ModuleFeatureId).prop("checked", true);
                    if (data[i].AccessLevel == 1) {
                        $('#check_' + data[i].ModuleFeatureId + '__' + data[i].AccessLevel).prop("checked", true);
                    }
                    if (data[i].AccessLevel == 2) {
                        $('#check_' + data[i].ModuleFeatureId + '__' + 1).prop("checked", true);
                        $('#check_' + data[i].ModuleFeatureId + '__' + data[i].AccessLevel).prop("checked", true);
                    }
                    if (data[i].AccessLevel == 3) {
                        $('#check_' + data[i].ModuleFeatureId + '__' + 1).prop("checked", true);
                        $('#check_' + data[i].ModuleFeatureId + '__' + 2).prop("checked", true);
                        $('#check_' + data[i].ModuleFeatureId + '__' + data[i].AccessLevel).prop("checked", true);
                    }
                }
            }
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }
    });

});

function getUsersByCompany(obj) {
    var ddr = $(obj);
    var compId = ddr.val();
    $.Ajax({
        url: '/UserManagement/User/GetUsersByCompany',
        type: "GET",
        dataType: 'JSON',
        data: { compId: compId }
    }).done(function (data) {
        $('.user_name').empty();
        $('.user_name').append('<option Value="">-Select-</option>');
        $.each(data, function () {
            $('.user_name').append('<option value=' + this.Value + '>' + this.Text + '</option>');
        });

    });
}