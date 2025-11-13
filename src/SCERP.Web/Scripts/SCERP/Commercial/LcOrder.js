
$('#LcOrderSearch').unbind('click').bind('click', function () {
    var button = $(this);
    var form = button.parents('form:first');
    var data = form.serialize();
    var url = form.attr('action');
    jQuery.Ajax({
        url: url
        , type: "GET"
        , data: data
        , container: '#lcOrderIndex'
    });    
});

function save() {
    var stringArray = new Array();
    var concat;
    var count = 0;

    $('#grid tr').each(function () {

        if (!this.rowIndex) return; // skip first row

        var orderId = this.cells[0].innerHTML;
        var lcNo = null;
        if (count > 0)
            lcNo = this.cells[11].getElementsByClassName('LcNo')[0].value;

        concat = orderId + '^' + lcNo;

        stringArray[count] = concat;
        count++;
    });

    var postData = { values: stringArray };

    $.Ajax({
        dataType: "json",
        type: "POST",
        url: "/LcOrder/Save",
        data: postData,
        container: '#lcOrderIndex',               
    });
}

