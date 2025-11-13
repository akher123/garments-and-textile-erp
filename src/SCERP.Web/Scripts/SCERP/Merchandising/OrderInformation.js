
$('.OrderInformation_Buyer').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var buyerclient = $(this).attr('BuyerClientclass');

    var buyerId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        buyerclientclass:buyerclient,
        data: buyerId
    };

    OrderInformation.PopulateSpecSheetDropdownList(option);
});

$('.OrderInformation_StyleNo').change(function () {

    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var styleNo = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: styleNo
    };

    OrderInformation.PopulateJobNumberDropdown(option);
});

$('.OrderInformation_JobNumber').change(function () {
    var action = $(this).attr('action');
    var targetClassName = $(this).attr('TargetClass');
    var specificationSheetId = $(this).val();
    var option = {
        url: action,
        targetClass: targetClassName,
        data: specificationSheetId
    };

    OrderInformation.PopulateOrderNumberDropdown(option);
});


var OrderInformation = {
    PopulateSpecSheetDropdownList: function(option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { id: option.data },
            success: function(resposns) {
                OrderInformation.ClearDropDownList(option.targetClass);
                OrderInformation.ClearDropDownList(option.buyerclientclass);

                if (resposns.Success == true && resposns.SpecificationSheet.length > 0) {

                    for (var i = 0; i < resposns.SpecificationSheet.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.SpecificationSheet[i].StyleNo).attr('value', resposns.SpecificationSheet[i].StyleNo));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }

                if (resposns.Success == true && resposns.buyerClients.length > 0) {

                    for (var i = 0; i < resposns.buyerClients.length; i++) {
                        $('.' + option.buyerclientclass)
                            .append($('<option>').text(resposns.buyerClients[i].ClientName).attr('value', resposns.buyerClients[i].BuyerClientId));
                    }

                } else {
                    $('.' + option.buyerclientclass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }
            }
        });
    },

    PopulateJobNumberDropdown: function(option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { styleNo: option.data },
            success: function(resposns) {
                OrderInformation.ClearDropDownList(option.targetClass);
                if (resposns.Success == true && resposns.JobNumbers.length > 0) {

                    for (var i = 0; i < resposns.JobNumbers.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.JobNumbers[i].JobNo).attr('value', resposns.JobNumbers[i].SpecificationSheetId));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }
            }
        });
    },

    PopulateOrderNumberDropdown: function(option) {
        $.ajax({
            url: option.url,
            type: "GET",
            dataType: "JSON",
            data: { specificationSheetId: option.data },
            success: function(resposns) {
                OrderInformation.ClearDropDownList(option.targetClass);
                if (resposns.Success == true && resposns.orderNumbers.length > 0) {

                    for (var i = 0; i < resposns.orderNumbers.length; i++) {
                        $('.' + option.targetClass)
                            .append($('<option>').text(resposns.orderNumbers[i].OrderNo).attr('value', resposns.orderNumbers[i].OrderId));
                    }

                } else {
                    $('.' + option.targetClass)
                        .append($('<option>').text("NotFound").attr('value', ""));
                }
            }
        });
    },

    ClearDropDownList: function(selector) {
        $('.' + selector)
            .find('option').remove()
            .end().append($('<option>').text("-select-").attr('value', ""));
    },
};


