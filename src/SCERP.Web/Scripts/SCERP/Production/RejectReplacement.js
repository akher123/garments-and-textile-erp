
$('.CuttingBatch_buyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();

    $.Ajax({
        url: '/JobCard/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
        ClearCutttingJob();
        removeDependentDropdown();
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


function ClearCutttingJob() {
    $('.CuttingBatch_JobNo').empty();
}
function removeDependentDropdown() {
    $('.CuttingBatch_OrderNo').empty();
    $('.CuttingBatch_StyleNo').empty();
    $('.CuttingBatch_Color').empty();
    $('.CuttingBatch_sequence').empty();
    $('.CuttingBatch_JobNo').empty();
}

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
        ClearCutttingJob();
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
            loadStyleWiseShipment(orderStyleRefId);
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
        ClearCutttingJob();
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
    var orderShipRefId = $('.CuttingBatch_OrderShipRefId').val();
    
    $.Ajax({
        url: '/RejectReplacement/GetJobNoByComponentRefId/',
        data: { ColorRefId: colorRefId, ComponentRefId: componentRefId, orderStyleRefId: orderStyleRefId, orderShipRefId: orderShipRefId }
    }).done(function (jobNoList) {
        $('.CuttingBatch_JobNo').empty();
        if (jobNoList.length > 0) {
            $('.CuttingBatch_JobNo').append(
             $('<option/>')
                 .attr('value', '')
                 .text("Select")
         );
            $.each(jobNoList, function (jobNo) {
                $('.CuttingBatch_JobNo').append(
                    $('<option/>')
                        .attr('value', this.CuttingBatchId)
                        .text(this.JobNo)
                );
            });
        } else {
            $('.CuttingBatch_JobNo').append(
                $('<option/>')
                    .attr('value', "No Order Found")
                    .text("Select")
            );
        }
    });
});


$('.showRdlcReport').unbind('click').bind('click', function (e) {
    var orderStyleRefId = $('.CuttingBatch_StyleNo').val();
    if (orderStyleRefId.length > 0) {
        var searchurl = '/Production/ProductionReport/CuttingBodyReplaceReport?orderStyleRefId=' + orderStyleRefId;
        window.open(searchurl, "_blank");
    } else {
        alert("Please select Buyer , Order and Style !!");
    }
  

});

function loadStyleWiseShipment(orderStyleRefId) {
    $.Ajax({
        url: '/JobCard/GetStyleWiseShipment/',
        data: { orderStyleRefId: orderStyleRefId }
    }).done(function (orderShips) {
        ClearCutttingJob();
        $('.CuttingBatch_OrderShipRefId').empty();
        if (orderShips.length > 0) {
            $('.CuttingBatch_OrderShipRefId').append(
                $('<option/>')
                    .attr('value', '')
                    .text("Select")
            );

            $.each(orderShips, function (orderShip) {
                $('.CuttingBatch_OrderShipRefId').append(
                    $('<option/>')
                        .attr('value', this.OrderShipRefId)
                        .text(this.CountryName)
                );
            });
        } else {
            $('.CuttingBatch_OrderShipRefId').append(
                $('<option/>')
                    .attr('value', "No Order Found")
                    .text("Select")
            );
        }
    });

   
}