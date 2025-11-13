$('#RollIsue_BuyerId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/Inventory/StyleShipment/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        removeDependentDropdown();
        if (orderList.length > 0) {
            populateDropDown({ OrderNo: "", RefNo: "-Select Order-" });
            $.each(orderList, function (order) {
                populateDropDown({ OrderNo: this.OrderNo, RefNo: this.RefNo });
            });
        } else {
            populateDropDown({ RefNo: "No Order Found", OrderNo: "" });
        }
    });
});
function populateDropDown(obj) {
    $('#RollIsue_OrderNo').append(
        $('<option/>').attr('value', obj.OrderNo).text(obj.RefNo)
    );
}
$('#RollIsue_OrderNo').unbind('change').bind('change', function () {
    var orderNo = $(this).val();
    $.Ajax({
        url: '/Inventory/StyleShipment/GetStyleByOrderNo/',
        data: { orderNo: orderNo }
    }).done(function (styleList) {
        $('#RollIsue_StyleNo').empty();
        if (styleList.length > 0) {
            $('#RollIsue_StyleNo').append(
                $('<option/>').attr('value', '').text("-Select Style-")
            );
            $.each(styleList, function (style) {
                $('#RollIsue_StyleNo').append(
                    $('<option/>').attr('value', this.OrderStyleRefId).text(this.StyleNo)
                );
            });
        } else {
            $('#RollIsue_StyleNo').append(
                $('<option/>').attr('value', '').text("No Order Found")
            );
        }
    });
});
function removeDependentDropdown() {

    $('#RollIsue_OrderNo').empty();
    $('#RollIsue_StyleNo').empty();
}



$('#RollIsue_StyleNo').unbind('change').bind('change', function () {
    var orderStyleRefId = $(this).val();
    $.Ajax({
        url: '/KnittingRollIssue/GetProgramByOrderStyleRefId/',
        data: { orderStyleRefId: orderStyleRefId }
    }).done(function (styleList) {
        $('#RollIsue_ProgramRefId').empty();
        if (styleList.length > 0) {
            $('#RollIsue_ProgramRefId').append(
                $('<option/>').attr('value', '').text("-Select Program-")
            );
            $.each(styleList, function (program) {
                $('#RollIsue_ProgramRefId').append(
                    $('<option/>').attr('value', this.ProgramRefId).text(this.ProgramRefId)
                );
            });
        } else {
            $('#RollIsue_ProgramRefId').append(
                $('<option/>').attr('value', '').text("No Program Found")
            );
        }
    });
});

$('.knittingProgrmAutocomplite').autocomplete({
    source: function (request, response) {
        var datatarget = this.element.attr('data-target');
        var url = this.element.attr('action');
        var challanType = $('#ChallanTypeId').val();
        if (challanType == 2) {
            url = "/KnittingRoll/ProgramCollarCuffAutocomplite";
        }
        var option = {
            url: url,
            type: "GET",
            data: { serachString: request.term },
        };
        $.Ajax(option)
            .done(function (data) {
                $(datatarget).val('');

                response($.map(data, function (item) {
                    return { label: item.ProgramRefId, value: item.ProgramRefId, BuyerRefId: item.BuyerRefId, OrderNo: item.OrderNo, OrderStyleRefId: item.OrderStyleRefId, ProgramRefId: item.ProgramRefId, datatarget: datatarget };
                }));
            });
    },
    select: function (event, ui) {
        var programRefId = ui.item.ProgramRefId;
        $(ui.item.datatarget).val(programRefId);
        $('#KnittingRollIssue_BuyerRefId').val(ui.item.BuyerRefId);
        $('#KnittingRollIssue_OrderNo').val(ui.item.OrderNo);
        $('#KnittingRollIssue_OrderStyleRefId').val(ui.item.OrderStyleRefId);
    },
});