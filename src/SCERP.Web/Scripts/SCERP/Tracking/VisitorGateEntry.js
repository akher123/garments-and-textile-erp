$('.PhoneTextBoxFor').unbind('change').bind('change', function () {
    var phone = $(this).val();
    $.Ajax({
        url: '/VisitorGateEntry/GetVisitorGateEntryByPhone/',
        data: { Phone: phone }
    }).done(function (visitor) {
        $('.VisitorNameTextBoxFor').val(visitor.VisitorName);
        $('.AddressTextBoxFor').val(visitor.Address);
    });
});

$('.ToWhomTextBoxFor').autocomplete({
    source: function (request, response) {
        var option = {
            url: '/VisitorGateEntry/GetEmployeesBySearchCharacter/',
            type: "GET",
            datatype: "html",
            data: { SearchCharacter: request.term },
        };

        $.Ajax(option)
            .done(function (data) {
                $('.EmployeeIdHiddenFor').val('');
                response($.map(data, function (employee) {
                    return { label: employee.EmployeeCardId + "_" + employee.Name, value: employee.Name, EmployeeId: employee.EmployeeId };
                }));
            });
    },
    select: function (event, ui) {
        var employeedId = ui.item.EmployeeId;
        $('.EmployeeIdHiddenFor').val(employeedId);
        $('.ConfirmedByTextBoxFor').val(ui.item.label);
    },

});


$('.ConfirmedByTextBoxFor').autocomplete({
    source: function (request, response) {
        var option = {
            url: '/VisitorGateEntry/GetEmployeesBySearchCharacter/',
            type: "GET",
            datatype: "html",
            data: { SearchCharacter: request.term },
        };

        $.Ajax(option)
            .done(function (data) {
                $('.ConfirmedByTextBoxFor').val('');
                response($.map(data, function (employee) {
                    return { label: employee.Name, value: employee.Name};
                }));
            });
    },
    select: function (event, ui) {
        $('.ConfirmedByTextBoxFor').val(ui.item.label);
    },

});


var pos = 0, ctx = null, saveCB, image = [];
var canvas = document.createElement('canvas');
canvas.setAttribute('width', 320);
canvas.setAttribute('height', 240);
ctx = canvas.getContext('2d');
image = ctx.getImageData(0, 0, 320, 240);
 saveCB = function (data) {
    var col = data.split(';');
    var img = image;
    for (var i = 0; i < 320; i++) {
        var tmp = parseInt(col[i]);
        img.data[pos + 0] = (tmp >> 16) & 0xff;
        img.data[pos + 1] = (tmp >> 8) & 0xff;
        img.data[pos + 2] = tmp & 0xff;
        img.data[pos + 3] = 0xff;
        pos += 4;
    }

    if (pos >= 4 * 320 * 240) {
        ctx.putImageData(img, 0, 0);

        $.post('/VisitorGateEntry/Upload', { type: 'data', image: canvas.toDataURL("image/png") }, function (result) {
            if (result.Success) {
                $("#imgCapture").css("visibility", "visible");
                $("#imgCapture").attr("src", result.ImagePath);
                $("#hidden_imageId").val(result.ImagePath);
                
            }
        });
        pos = 0;
    }
};

$('#webcam').webcam({
    width: 320,
    height: 240,
    mode: 'callback',
    swffile: '/Scripts/Webcam/jscam_canvas_only.swf',
    onSave: saveCB,
    onCapture: function () {
        webcam.save();
    }
});

$('#upload').click(function () {
    webcam.capture();
    return false;
});