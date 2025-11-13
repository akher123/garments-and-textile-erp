$('#filterbyReturnableChallanId').unbind("click").bind("click", clickEventHendeler);
function clickEventHendeler() {
    var returnableChallanId = $('.txtReturnableChallanId');
    var rChallanId = returnableChallanId.val();
    var $this = $(this);
    var option = {
        url: $this.attr('action'),
        type: 'GET',
        data: { ReturnableChallanId: rChallanId }
    };
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        returnableChallanId.validate();
        if (!returnableChallanId.valid()) {
            return false;
        }
        loadReturnableChallan(option);
    }
}
function loadReturnableChallan(option) {
    return $.Ajax(option).done(function (respons) {
        if (respons.Success && respons.Success != 'undefined') {
            $('#FabSubProcessReceive_containner').html(respons.returnableChallanReceive);
            $("#displayButton").removeAttr("style");
        }
        if (respons.ValidStatus == false) {
            $("#displayButton").css("display", "none");
            alert(respons.Message);
        }
    });
}

$('.txtSearchString').autocomplete({
    source: function (request, response) {
        var option = {
            url: '/FabSubProcessReceive/GetRefNoBySearchkey/',
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
function deleteRow(buttonObj) {
    var $tr = $(buttonObj).closest("tr");
    $tr.remove();
}