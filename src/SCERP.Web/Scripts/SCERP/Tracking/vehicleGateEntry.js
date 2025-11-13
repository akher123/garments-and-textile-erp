$('.ConfirmedByTextBoxFor').autocomplete({
    source: function (request, response) {
        var option = {
            url: '/VehicleGateEntry/GetEmployeesBySearchCharacter/',
            type: "GET",
            datatype: "html",
            data: { SearchCharacter: request.term },
        };

        $.Ajax(option)
            .done(function (data) {
                $('.ConfirmedByTextBoxFor').val('');
                response($.map(data, function (employee) {
                    return { label: employee.EmployeeCardId + "_" + employee.Name, value: employee.Name };
                }));
            });
    },
    select: function (event, ui) {
        $('.ConfirmedByTextBoxFor').val(ui.item.label);
    },

});


$('.ToWhomTextBoxFor').autocomplete({
    source: function (request, response) {
        var option = {
            url: '/VehicleGateEntry/GetEmployeesBySearchCharacter/',
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
        var employeedI = ui.item.EmployeeId;
        $('.EmployeeIdHiddenFor').val(employeedI);
        $('.ConfirmedByTextBoxFor').val(ui.item.label);
    },

});