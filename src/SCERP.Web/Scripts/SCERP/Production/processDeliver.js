$("#toggoleButton").click(function () {
    var value = $(this).val();
    if (value == '+') {
        $(this).val("-");
    } else {
        $(this).val("+");
    }
    $("#cuttingBatchToggle").toggle("1000");

});
$("#cuttingBatchToggle").show();


$('.CuttingBatch_factory').unbind('change').bind('change', function () {
    var partyId = $(this).val();
    $.Ajax({
        url: '/EmbroideryProcess/GetBuyerByPartyId/',
        data: { partyId: partyId }
    }).done(function (buyerLsit) {
        removeDependentDropdown();

        if (buyerLsit.length > 0) {
            $('.CuttingBatch_buyerRefId').append(
                $('<option/>')
                    .attr('value', '')
                    .text("Select")
            );
            $.each(buyerLsit, function (buyer) {
                $('.CuttingBatch_buyerRefId').append(
                    $('<option/>')
                        .attr('value', this.BuyerRefId)
                        .text(this.BuyerName)
                );
            });
        } else {
            $('.CuttingBatch_buyerRefId').append(
                $('<option/>')
                    .attr('value', '')
                    .text("No Order Found")
            );
        }
    });

});

$('.CuttingBatch_buyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/Production/JobCard/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        $('.CuttingBatch_OrderNo').empty();
        if (orderList.length > 0) {
            populateDropDown({ OrderNo: "", RefNo: "Select" });
            $.each(orderList, function (order) {
                populateDropDown({ OrderNo: this.OrderNo, RefNo: this.RefNo });
            });
        } else {
            populateDropDown({ RefNo: "No Order Found", OrderNo: "" });
        }
    });
});


function populateDropDown(obj) {
    $('.CuttingBatch_OrderNo').append(
        $('<option/>').attr('value', obj.OrderNo).text(obj.RefNo)
    );
}

$('.CuttingBatch_OrderNo').unbind('change').bind('change', function () {
    var orderNo = $(this).val();
    $.Ajax({
        url: '/JobCard/GetStyleByOrderNo/',
        data: { orderNo: orderNo }
    }).done(function (styleList) {
        $('.CuttingBatch_StyleNo').empty();
        if (styleList.length > 0) {
            $('.CuttingBatch_StyleNo').append(
                $('<option/>').attr('value', '').text("Select")
            );
            $.each(styleList, function (style) {
                $('.CuttingBatch_StyleNo').append(
                    $('<option/>').attr('value', this.OrderStyleRefId).text(this.StyleNo)
                );
            });
        } else {
            $('.CuttingBatch_StyleNo').append(
                $('<option/>').attr('value', '').text("No Order Found")
            );
        }
    });
});

$('.CuttingBatch_StyleNo').unbind('change').bind('change', function () {
    var orderStyleRefId = $(this).val();
    $.Ajax({
        url: '/JobCard/GetColorsByOrderStyleRefId/',
        data: { orderStyleRefId: orderStyleRefId }
    }).done(function (colors) {
        $('.CuttingBatch_Color').empty();
        if (colors.length > 0) {
            $('.CuttingBatch_Color').append(
             $('<option/>').attr('value', '').text("Select")
         );
            $.each(colors, function (color) {
                $('.CuttingBatch_Color').append(
                    $('<option/>').attr('value', this.ColorRefId).text(this.ColorName)
                );
            });
        } else {
            $('.CuttingBatch_Color').append(
                $('<option/>').attr('value', "No Order Found").text("Select")
            );
        }
    });
});
$('.CuttingBatch_Color').unbind('change').bind('change', function () {
    var colorRefId = $(this).val();
    var orderStyleRefId = $('.CuttingBatch_StyleNo').val();
    $.Ajax({
        url: '/JobCard/GetComponentByColor/',
        data: { ColorRefId: colorRefId, orderStyleRefId: orderStyleRefId }
    }).done(function (components) {
        $('.CuttingBatch_sequence').empty();
        if (components.length > 0) {
            $('.CuttingBatch_sequence').append(
             $('<option/>')
                 .attr('value', '')
                 .text("-Select-")
         );
            $.each(components, function (component) {
                $('.CuttingBatch_sequence').append(
                    $('<option/>')
                        .attr('value', this.ComponentRefId)
                        .text(this.ComponentName)
                );
            });
        } else {
            $('.CuttingBatch_sequence').append(
                $('<option/>')
                    .attr('value', "No Order Found")
                    .text("Select")
            );
        }
    });
});
function removeDependentDropdown() {
    $('.CuttingBatch_buyerRefId').empty();
    $('.CuttingBatch_OrderNo').empty();
    $('.CuttingBatch_StyleNo').empty();
    $('.CuttingBatch_Color').empty();
    $('.CuttingBatch_sequence').empty();

}


$('#findButtion').unbind('click').bind('click', function (e) {

    var div = $('#findAssignedFactoryCuttingJob:input,select');
    var data = div.serialize();
    var url = $(this).attr('action');
    $.Ajax({
        type: "GET",
        url: url,
        data: data,
        container: "#cuttingBatchToggle"
    });


});
function processDeliverySummary() {
    var partyId = $('#partyId').val();
    var searchString = $('#searchString').val();
    var searchurl = '/Production/ProductionReport/ProcessDeliverySummary?PartyId=' + partyId + '&SearchString=' + searchString;
    window.open(searchurl);
}

function processDeliveryDetail() {
    var orderStyleRefId = $('.CuttingBatch_StyleNo').val();
    var searchurl = '/Production/ProductionReport/ProcessDeliveryDetailReport?orderStyleRefId=' + orderStyleRefId;
    window.open(searchurl);
}