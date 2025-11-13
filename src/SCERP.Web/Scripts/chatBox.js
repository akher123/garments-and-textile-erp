function openForm() {
    document.getElementById("myForm").style.display = "block";
}

function closeForm() {
    document.getElementById("myForm").style.display = "none";
}
$("#search").autocomplete({
    source: function (request, response) {
        var option = {
            url: "/UserManagement/User/GetActiveUsers",
            type: "GET",
            data: { empName: request.term },
        };
        $.Ajax(option)
            .done(function (data) {
                response($.map(data, function (item) {
                    return item;
                }));
            });
    },
    select: function (event, ui) {
        $("#search").val(ui.item.label);
        $("#message_ReceiverId").val(ui.item.value);
        return false;
    },
});
$("#search").data("ui-autocomplete")._renderItem = function (ul, item) {
    return $('<li/>', { 'data-value': item.label }).append($('<a/>', { href: "#" })
        .append($('<img/>', { src: item.icon, alt: item.label })).append(item.label))
        .appendTo(ul);
};
$('#message_send').unbind('click').bind('click', function () {
    var $button = $(this);
    var $from = $button.parent('form:first');
    var option = {
        url: $from.attr('action'),
        type: "POST",
        data: $from.serialize(),
    };
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        $from.validate();
        if (!$from.valid()) {
            return false;
        }
        $.Ajax(option).done(function (resp) {
            if (resp > 0) {
                $from.find("input[type=text],input[type=search], textarea,input[type=hidden]").val("");
            } else {
                alert('Message Send Successfully !!');
            }

        });
    }
});

