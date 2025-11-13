$('.SewingInputProcess_buyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/JobCard/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        $('.SewingInputProcess_OrderNo').empty();
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
    $('.SewingInputProcess_OrderNo').append(
        $('<option/>')
            .attr('value', obj.OrderNo)
            .text(obj.RefNo)
    );
}


$('.SewingInputProcess_OrderNo').unbind('change').bind('change', function () {
    var orderNo = $(this).val();
    $.Ajax({
        url: '/JobCard/GetStyleByOrderNo/',
        data: { orderNo: orderNo }
    }).done(function (styleList) {
        $('.SewingInputProcess_StyleNo').empty();
        if (styleList.length > 0) {
            $('.SewingInputProcess_StyleNo').append(
                $('<option/>')
                    .attr('value', '')
                    .text("Select")
            );
            $.each(styleList, function (style) {
                $('.SewingInputProcess_StyleNo').append(
                    $('<option/>')
                        .attr('value', this.OrderStyleRefId)
                        .text(this.StyleNo)
                );
            });
        } else {
            $('.SewingInputProcess_StyleNo').append(
                $('<option/>')
                    .attr('value', '')
                    .text("No Order Found")
            );
        }
    });
});

$('.SewingInputProcess_StyleNo').unbind('change').bind('change', function () {
    var orderStyleRefId = $(this).val();
    $.Ajax({
        url: '/JobCard/GetColorsByOrderStyleRefId/',
        data: { orderStyleRefId: orderStyleRefId }
    }).done(function (colors) {
        $('.SewingInputProcess_Color').empty();
        if (colors.length > 0) {
            $('.SewingInputProcess_Color').append(
                $('<option/>')
                    .attr('value', '')
                    .text("Select")
            );
            $.each(colors, function (color) {
                $('.SewingInputProcess_Color').append(
                    $('<option/>')
                        .attr('value', this.ColorRefId)
                        .text(this.ColorName)
                );
            });
        } else {
            $('.SewingInputProcess_Color').append(
                $('<option/>')
                    .attr('value', "No Order Found")
                    .text("Select")
            );
        }
    });

    loadStyleWiseShipment(orderStyleRefId);
});

function removeDependentDropdown() {
    $('.SewingInputProcess_buyerRefId').empty();
    $('.SewingInputProcess_OrderNo').empty();
    $('.SewingInputProcess_StyleNo').empty();
    $('.SewingInputProcess_Color').empty();
}

function StyleAndColorWiseInput(reportTypeId) {
    var form = $('.SewingInputProcessForm');
         var data = form.serialize();
         var searchurl = '/Production/ProductionReport/StyleAndColorWiseInput?' + data + '&reportTypeId=' + reportTypeId;
            window.open(searchurl);
        }

function loadStyleWiseShipment(orderStyleRefId) {
    $.Ajax({
        url: '/JobCard/GetStyleWiseShipment/',
        data: { orderStyleRefId: orderStyleRefId }
    }).done(function (orderShips) {
        $('.SewingInputProcess_OrderShipRefId').empty();
        if (orderShips.length > 0) {
            $('.SewingInputProcess_OrderShipRefId').append(
                $('<option/>')
                .attr('value', '')
                .text("Select")
            );

            $.each(orderShips, function (orderShip) {
                $('.SewingInputProcess_OrderShipRefId').append(
                    $('<option/>')
                    .attr('value', this.OrderShipRefId)
                    .text(this.CountryName)
                );
            });
        } else {
            $('.SewingInputProcess_OrderShipRefId').append(
                $('<option/>')
                .attr('value', "No Country Found")
                .text("Select")
            );
        }
    });
}