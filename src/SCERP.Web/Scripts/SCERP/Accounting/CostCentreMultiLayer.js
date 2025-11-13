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

$('.costCentre').click(function() {
    var costCentre = $(this).attr('id');
    $("#dialog_result").dialog({
        resizable: true,
        title: 'Do you want to ... ?',
        height: 'auto',
        width: 'auto',
        modal: true,
        buttons: {
            "Create": function() {
                $('#event_result').empty();
                $('#event_result').load('/Accounting/CostCentreMultiLayer/Create', { costCentre: costCentre }).dialog({ width: 'auto', title: "Create Cost Centre" });
                $(this).dialog("close");
            },

            "Edit": function() {
                $('#event_result').empty();
                $('#event_result').load('/Accounting/CostCentreMultiLayer/Edit', { costCentre: costCentre }).dialog({ width: 'auto', title: "Edit Cost Centre" });
                $(this).dialog("close");

            },

            "Delete": function() {
                if (!confirm("Do you want to delete ?")) {
                    $(this).dialog("close");
                    return false;
                } else {

                    $.getJSON("/Accounting/CostCentreMultiLayer/Delete", { costCentre: costCentre }, function(data) {
                        if (data.Success == true) {
                            loadAction("/Accounting/CostCentreMultiLayer/CostCentreMultiLayer");
                      
                        }
                        if (data.Success === false) {
                            alert("Not delete");
                        }
                    });
                }
            },
        }
    });

    if (costCentre.split('_')[3] == 5) {
        $('.ui-dialog-buttonpane button:contains("Create")').button().hide();
    }
});

