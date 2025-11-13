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
$('.chartofAcc').on("click", function () {
    //append code here
    var contentAccount = $(this).attr('id');
    $("#dialog_result").dialog({
        resizable: true,
        title: 'Do you want to ... ?',
        height: 'auto',
        width: 'auto',
        modal: true,
        buttons: {
            
            "Create": function () {
                $('#event_result').empty();
                $('#event_result').load('/Accounting/ControlAccount/Create', { contentAccount: contentAccount }).dialog({ width: 'auto', title: "Create Account head" });
                $(this).dialog("close");
            },
            "Edit": function () {
                $('#event_result').empty();
                $('#event_result').load('/Accounting/ControlAccount/Edit', { contentAccount: contentAccount }).dialog({ width: 'auto', title: "Edit Account head" });
                $(this).dialog("close");

            },
            "Delete": function () {
                if (!confirm("Do you want to delete ?")) {
                    $(this).dialog("close");
                    return false;
                } else {

                    $.getJSON("/Accounting/ControlAccount/Delete", { contentAccount: contentAccount }, function (data) {
                        if (data.Success === true) {
                            $("#dialog_result").dialog("close");
                            loadAction("/Accounting/ControlAccount/ChartOfAccount");
                        }
                        if (data.Success === false) {
                            $("#dialog_result").dialog("close");
                            alert(" Can not delete this item");
                        }

                    });
                }
            },
          
        }
    });
    if (contentAccount.split('_')[3] == 5) {
        $('.ui-dialog-buttonpane button:contains("Create")').button().hide();
       
    }
    
});

