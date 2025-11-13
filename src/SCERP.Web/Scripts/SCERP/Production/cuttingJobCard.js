
function addRollCutting(obj) {
    var data = $(obj).parents('form:first').serialize();
    $.Ajax({
        type: 'GET',
        url: '/JobCard/AddNewRoll/',
        data: data
    }).done(function (respons) {
        $('#rollCutting').html(respons);
    });;

}
function deleteRollCutting(buttonObj) {
    var $tr = $(buttonObj).closest("tr");
    $tr.remove();
    var data = $('.JobCardForm').serialize();
    $.Ajax({
        url: '/JobCard/DeleteRollCutting/',
        data: data
         , container: '#rollCutting'
    });

}

$('.CuttingBatch_buyerRefId').unbind('change').bind('change', function () {
    var buyerRefId = $(this).val();
    $.Ajax({
        url: '/JobCard/GetOrderByBuyer/',
        data: { buyerRefId: buyerRefId }
    }).done(function (orderList) {
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
        url: '/RejectAdjustment/GetJobNoByComponentRefId/',
        data: { ColorRefId: colorRefId, ComponentRefId: componentRefId, orderStyleRefId: orderStyleRefId, orderShipRefId:orderShipRefId }
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
function removeDependentDropdown() {
    $('.CuttingBatch_OrderNo').empty();
    $('.CuttingBatch_StyleNo').empty();
    $('.CuttingBatch_Color').empty();
    $('.CuttingBatch_sequence').empty();
    $('.CuttingBatch_OrderShipRefId').empty();
    
}



function bundleSlipReport() {
    var cuttingBatchRefId = $('#CuttingBacthrefId').val();
    var searchurl = '/Production/ProductionReport/BundleSlip?CuttingBatchRefId=' + cuttingBatchRefId;
    window.open(searchurl);

}
function bundleCharReport() {
    var cuttingBatchRefId = $('#CuttingBacthrefId').val();
    var searchurl = '/Production/ProductionReport/BundleChartReport?CuttingBatchRefId=' + cuttingBatchRefId;
    window.open(searchurl);
}

function jobWiseCuttingReport(obj) {
    var form = $(obj).parents('form:first');
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        form.validate();
        if (!form.valid()) {
            e.preventDefault();
            return false;
        } else {
            var data = form.serialize();
            var searchurl = '/Production/ProductionReport/JobWiseCuttingReport?' + data;
            window.open(searchurl);
        }
    }
}

function loadStyleWiseShipment(orderStyleRefId) {
    $.Ajax({
        url: '/JobCard/GetStyleWiseShipment/',
        data: { orderStyleRefId: orderStyleRefId }
    }).done(function (orderShips) {
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

