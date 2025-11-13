$('.txtSearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: '/ReturnableChallanReceive/GetRefNoBySearchCharacter/',
            type: "GET",
            datatype: "html",
            data: { SearchCharacter: request.term },
        };

        $.Ajax(option)
            .done(function (data) {
                $('.txtReturnableChallanId').val('');
                response($.map(data, function (returnableChallan) {
                    return { label: returnableChallan.ReturnableChallanRefId, value: returnableChallan.ReturnableChallanRefId, ReturnableChallanId: returnableChallan.ReturnableChallanId };
                }));
            });
    },
    select: function (event, ui) {
        var returnableChallanId = ui.item.ReturnableChallanId;
        $('.txtReturnableChallanId').val(returnableChallanId);
    },

});