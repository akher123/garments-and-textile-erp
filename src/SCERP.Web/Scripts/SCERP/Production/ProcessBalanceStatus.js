
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
        url: '/JobCard/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        $('.CuttingBatch_OrderNo').empty();
        $('.CuttingBatch_StyleNo').empty();
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
        $('<option/>')
            .attr('value', obj.OrderNo)
            .text(obj.RefNo)
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
                $('<option/>')
                    .attr('value', '')
                    .text("Select")
            );
            $.each(styleList, function (style) {
                $('.CuttingBatch_StyleNo').append(
                    $('<option/>')
                        .attr('value', this.OrderStyleRefId)
                        .text(this.StyleNo)
                );
            });
        } else {
            $('.CuttingBatch_StyleNo').append(
                $('<option/>')
                    .attr('value', '')
                    .text("No Order Found")
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
                $('<option/>')
                    .attr('value', '')
                    .text("Select")
            );
            $.each(colors, function (color) {
                $('.CuttingBatch_Color').append(
                    $('<option/>')
                        .attr('value', this.ColorRefId)
                        .text(this.ColorName)
                );
            });
        } else {
            $('.CuttingBatch_Color').append(
                $('<option/>')
                    .attr('value', "No Order Found")
                    .text("Select")
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

$('.CuttingBatch_sequence').unbind('change').bind('change', function () {
    var componentRefId = $(this).val();
    var orderStyleRefId = $('.CuttingBatch_StyleNo').val();
    var colorRefId = $('.CuttingBatch_Color').val();
    $.Ajax({
        url: '/CuttingTag/GetGetCuttingTagBySequence/',
        data: { ColorRefId: colorRefId, ComponentRefId: componentRefId, orderStyleRefId: orderStyleRefId }
    }).done(function (jobNoList) {
        $('.cuttingTagId').empty();
        if (jobNoList.length > 0) {
            $('.cuttingTagId').append(
                $('<option/>')
                    .attr('value', '')
                    .text("Select")
            );
            $.each(jobNoList, function (jobNo) {
                $('.cuttingTagId').append(
                    $('<option/>')
                        .attr('value', this.CuttingTagId)
                        .text(this.ComponentName)
                );
            });
        } else {
            $('.cuttingTagId').append(
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


function processBalanceStatus(reportType) {
    var form = $('.BalanceStatusReportForm');
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {

            return false;
        } else {
            var data = form.serialize();
            var searchurl = '/Production/ProductionReport/PrintBalanceStatusReport?' + data + '&ReportType=' + reportType;
            window.open(searchurl);
        }
    }




}
function FactoryStyleWiseBalance(reportType) {
    
    var form = $('.BalanceStatusReportForm');
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {

            return false;
        } else {
            var data = form.serialize();
            var searchurl = '/ProductionReport/FactoryStyleWiseBalanceReport?' + data + '&ReportType=' + reportType;
            window.open(searchurl);
        }
    }

        
}
function PrintEmbroideryBalanceSummary(reportType) {

    var form = $('.BalanceStatusReportForm');
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {

            return false;
        } else {
            var data = form.serialize();
            var searchurl = '/ProductionReport/PrintEmbroideryBalanceSummary?' + data + '&ReportType=' + reportType;
            window.open(searchurl);
        }
    }

}

function ProcessReceiveDetailsReport() {

    var form = $('.BalanceStatusReportForm');
    var orderStyleRefId = $('.CuttingBatch_StyleNo').val();
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {

            return false;
        } else {
            var data = form.serialize();
            var searchurl = '/ProductionReport/ProcessReceiveDetailReport?orderStyleRefId=' + orderStyleRefId;
            window.open(searchurl);
        }
    }

}


