function addPart(obj) {
    var data = $(obj).parents('form:first').serialize();
    $.Ajax({
        type: 'GET',
        url: '/CuttingSequence/AddNewPart/',
        data: data
    }).done(function (respons) {
        $('#part').html(respons);
    });;
}

function deletePart(buttonObj) {
    var $tr = $(buttonObj).closest("tr");
    $tr.remove();
    var data = $('.CuttingSequenceForm').serialize();
    $.Ajax({
        url: '/CuttingSequence/DeletePart/',
        data: data,
        container: '#part'
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
                 .text("ALL")
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
                    .attr('value', "No Color Found")
                    .text("Select")
            );
        }
    });
});
function removeDependentDropdown() {
    $('.CuttingBatch_OrderNo').empty();
    $('.CuttingBatch_StyleNo').empty();
    $('.CuttingBatch_Color').empty();

}