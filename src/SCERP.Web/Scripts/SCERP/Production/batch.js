
    $('input[type=text]').unbind("keypress").bind("keypress", function(e) {
        var key = e.which;
        if (key == 13) //the enter key code
        {
            e.preventDefault();
        }

    });


    $('.Color_SearchString').autocomplete({
        source: function(request, response) {
            var option = {
                url: "/Batch/AutoCompliteColor",
                type: "GET",

                data: { searchString: request.term },
            };

            $.Ajax(option)
                .done(function(data) {
                    $('.Color_ColorId').val('');
                    response($.map(data, function(color) {
                        return { label: color.ColorName, value: color.ColorName, ColorId: color.ColorId };
                    }));
                });
        },
        select: function(event, ui) {
            var colorId = ui.item.ColorId;
            $('.Color_ColorId').val(colorId);
        },

    });
    $('.Color_FColorName').autocomplete({
        source: function(request, response) {
            var option = {
                url: "/BuyerOrderColorSize/ColorAutoComplite",
                type: "GET",
                datatype: "html",
                data: { serachString: request.term },
            };

            $.Ajax(option)
                .done(function(data) {
                    $('.Color_FColorRefId').val('');

                    response($.map(data, function(color) {
                        return { label: color.ColorName, value: color.ColorName, ColorRefId: color.ColorRefId };
                    }));
                });
        },
        select: function(event, ui) {
            var colorRefId = ui.item.ColorRefId;
            $('.Color_FColorRefId').val(colorRefId);

        },

    });

    $('.Size_GSizeName').autocomplete({
        source: function(request, response) {
            var option = {
                url: "/BuyerOrderColorSize/SizeAutoComplite",
                type: "GET",
                datatype: "html",
                data: { serachString: request.term },
            };
            $.Ajax(option)
                .done(function(data) {
                    $('.Size_GSizeRefId').val('');
                    response($.map(data, function(size) {
                        return { label: size.SizeName, value: size.SizeName, SizeRefId: size.SizeRefId };
                    }));
                });
        },
        select: function(event, ui) {
            var sizeRefId = ui.item.SizeRefId;
            $('.Size_GSizeRefId').val(sizeRefId);
        },

    });

    $('.Size_FSizeName').autocomplete({
        source: function(request, response) {
            var option = {
                url: "/BuyerOrderColorSize/SizeAutoComplite",
                type: "GET",
                datatype: "html",
                data: { serachString: request.term },
            };
            $.Ajax(option)
                .done(function(data) {
                    $('.Size_FSizeRefId').val('');
                    response($.map(data, function(size) {
                        return { label: size.SizeName, value: size.SizeName, SizeRefId: size.SizeRefId };
                    }));
                });
        },
        select: function(event, ui) {
            var sizeRefId = ui.item.SizeRefId;
            $('.Size_FSizeRefId').val(sizeRefId);


        },

    });
    $('.OrderStyle_StyleName').autocomplete({
        source: function(request, response) {
            var option = {
                url: "/OmBuyOrdStyle/StyleAutocomplite",
                type: "GET",
                datatype: "html",
                data: { serachString: request.term },
            };
            $.Ajax(option)
                .done(function(data) {
                    $('.OrderStyle_OrderStyleRefId').val('');
                    response($.map(data, function(style) {
                        return { label: style.StyleName, value: style.StyleName, OrderStyleRefId: style.OrderStyleRefId };
                    }));
                });
        },
        select: function(event, ui) {
            var OrderStyleRefId = ui.item.OrderStyleRefId;
            $('.OrderStyle_OrderStyleRefId').val(OrderStyleRefId);


        },

    });

    function ColorAutocomplite(obj) {
        $('.Color_GrColorRefId').val('');
        $(obj).autocomplete({
            source: function(request, response) {
                var option = {
                    url: "/Batch/AutoCompleteColor",
                    type: "GET",
                    datatype: "html",
                    data: { searchString: request.term }
                };
                $.Ajax(option)
                    .done(function(data) {
                        $('.Color_GrColorRefId').val('');
                        response($.map(data, function(color) {
                            return { label: color.ColorName, value: color.ColorName, ColorRefId: color.ColorRefId };
                        }));
                    });
            },
            select: function(event, ui) {
                var colorRefId = ui.item.ColorRefId;
                $('.Color_GrColorRefId').val(colorRefId);

            },

        });
    }

    function itemAutocomplite(obj) {
        $('.Item_ItemId').val('');
        $(obj).autocomplete({
            source: function(request, response) {
                var option = {
                    url: "/Consumption/AutocompliteItem",
                    type: "GET",
                    data: { SearchItemKey: request.term },
                };
                $.Ajax(option)
                    .done(function(data) {
                        response($.map(data, function(item) {
                            return { label: item.ItemName, value: item.ItemName, ItemId: item.ItemId };
                        }));
                    });
            },
            select: function(event, ui) {
                var ItemId = ui.item.ItemId;
                $('.Item_ItemId').val(ItemId);

            },
        });
    }

    function componentAutocomplite(obj) {
        $('.Component_ComponentRefId').val('');
        $(obj).autocomplete({
            source: function(request, response) {
                var option = {
                    url: "/Batch/AutoCompliteComponent",
                    type: "GET",
                    data: { searchString: request.term }
                };
                $.Ajax(option)
                    .done(function(data) {
                        response($.map(data, function(item) {
                            return { label: item.ComponentRefId + "_" + item.ComponentName, value: item.ComponentName,CompType:item.CompType, ComponentRefId: item.ComponentRefId };
                        }));
                    });
            },
            select: function(event, ui) {
                var componentRefId = ui.item.ComponentRefId;
                if (ui.item.CompType === 3 || ui.item.CompType === 2) {
                    $('#CollarCuffLbl').text('Collar Cuff');
                    $('#FLength').attr("placeholder", "PCS");
                } else {
                    $('#CollarCuffLbl').text('Length(Woven)');
                    $('#FLength').attr("placeholder", "YARDS ");
                }
                $('.Component_ComponentRefId').val(componentRefId);
            },
        });
    }


    $('.autocompliteSizeSerach').autocomplete({
        source: function(request, response) {
            var datatarget = this.element.attr('data-target');
            var url = this.element.attr('action');
            var option = {
                url: url,
                type: "GET",
                datatype: "json",
                data: { serachString: request.term },
            };
            $.Ajax(option)
                .done(function(data) {
                    $(datatarget).val('');
                    response($.map(data, function(size) {
                        return { label: size.SizeName, value: size.SizeName, SizeRefId: size.SizeRefId, datatarget: datatarget };
                    }));
                });
        },
        select: function(event, ui) {
            var sizeRefId = ui.item.SizeRefId;
            $(ui.item.datatarget).val(sizeRefId);

        },

    });


    $('.batch_buyerRefId').unbind('change').bind('change', function() {
        var buyerRefId = $(this).val();
        $.Ajax({
            url: '/Inventory/StyleShipment/GetOrderByBuyer/',
            data: { buyerRefId: buyerRefId }
        }).done(function(orderList) {
            removeDependentDropdown();
            if (orderList.length > 0) {
                populateDropDown({ OrderNo: "", RefNo: "Select" });
                $.each(orderList, function(order) {
                    populateDropDown({ OrderNo: this.OrderNo, RefNo: this.RefNo });
                });
            } else {
                populateDropDown({ RefNo: "No Order Found", OrderNo: "" });
            }
        });
    });


    function populateDropDown(obj) {
        $('.batch_OrderNo').append(
            $('<option/>')
            .attr('value', obj.OrderNo)
            .text(obj.RefNo)
        );
    }

    $('.batch_OrderNo').unbind('change').bind('change', function() {
        var orderNo = $(this).val();
        $.Ajax({
            url: '/Inventory/StyleShipment/GetStyleByOrderNo/',
            data: { orderNo: orderNo }
        }).done(function(styleList) {
            $('.batch_StyleNo').empty();
            if (styleList.length > 0) {
                $('.batch_StyleNo').append(
                    $('<option/>')
                    .attr('value', '')
                    .text("Select")
                );
                $.each(styleList, function(style) {
                    $('.batch_StyleNo').append(
                        $('<option/>')
                        .attr('value', this.OrderStyleRefId)
                        .text(this.StyleNo)
                    );
                });
            } else {
                $('.batch_StyleNo').append(
                    $('<option/>')
                    .attr('value', '')
                    .text("No Order Found")
                );
            }
        });
    });

    $('.batch_StyleNo').unbind('change').bind('change', function() {
        var orderStyleRefId = $(this).val();
        $.Ajax({
            url: '/JobCard/GetColorsByOrderStyleRefId/',
            data: { orderStyleRefId: orderStyleRefId }
        }).done(function(colors) {
            $('.batch_Color').empty();
            if (colors.length > 0) {
                $('.batch_Color').append(
                    $('<option/>')
                    .attr('value', '')
                    .text("-Select-")
                );
                $.each(colors, function(color) {
                    $('.batch_Color').append(
                        $('<option/>')
                        .attr('value', this.ColorRefId)
                        .text(this.ColorName)
                    );
                });
            } else {
                $('.batch_Color').append(
                    $('<option/>')
                    .attr('value', "No Color Found")
                    .text("Select")
                );
            }
        });
    });

    function removeDependentDropdown() {
        $('.batch_OrderNo').empty();
        $('.batch_StyleNo').empty();
        $('.batch_Color').empty();

    }

// Pertial View against ddl
    $('.batchTypeDdl').unbind('change').bind('change', function() {
        var batchType = $(this).val();
        $.Ajax({
            url: '/Batch/OrderInfo/',
            data: { BatchType: batchType }
        }).done(function(response) {
            $('#batchTypeDiv').html(response);
        });
    });
// Detail
    $('#addBactchButton').unbind("click").bind("click", function() {
        var addNewItem = $('#batchDetailInput :input');
        var $table = $('table#batchDetailListingTable tbody');
        var $item = $('.Item_ItemId').val();
        var $omponentRefId = $('.Component_ComponentRefId').val();
        var option = {
            url: "/Batch/AddNewRow/",
            type: "POST",
            data: addNewItem.serialize()
        };

        $.Ajax(option).done(function(htmlResponse) {
            $('table#batchDetailListingTable tbody #NewRow_' + $item + "_" + $omponentRefId).remove();
            $table.append(htmlResponse);
            addNewItem.val('');
            $('#Quantity').val('0');
            $('#Item').focus();
        });

    });


    $("#Quantity").each(function() {
        $(this).keyup(function() {
            this.value = this.value.replace(/[^0-9.]/g, '');
        });
    });


    function deleteRow(buttonObj) {
        var $btnObj = $(buttonObj);
        var cltr = $btnObj.closest('tr');
        jQuery.Confirm(SCERP.Messages.DeleteConfirmation, function(r) {
            if (r) {
                cltr.remove();
            }
        });
    }

    function editRow(key) {
        $('#Item').val($('#Item_' + key).val());
        $('.Item_ItemId').val($('#Item_ItemId_' + key).val());
        $('#Component').val($('#Component_' + key).val());
        $('.Component_ComponentRefId').val($('#Component_ComponentRefId_' + key).val());
        $('#Machinedia').val($('#Machinedia_' + key).val());
        $('#machineDialSizeId').val($('#machineDialSizeId_' + key).val());
        $('#FinishDia').val($('#FinishDia_' + key).val());
        $('#finishDialSizeId').val($('#finishDialSizeId_' + key).val());
        
        $('#GSM').val($('#GSM_' + key).val());
        $('#Quantity').val($('#Quantity_' + key).val());
        $('#ReceivedQty').val($('#ReceivedQty_' + key).val());

        $('#RollQty').val($('#RollQty_' + key).val());
        $('#StLength').val($('#StLength_' + key).val());
        $('#FLength').val($('#FLength_' + key).val());

        $('#Remarks').val($('#Remarks_' + key).val());

    }


$('#Batch_PartyId').unbind('change').bind('change', function () {
    var partyId = $(this).val();
    $.Ajax({
        url: '/Production/Batch/GetDyeingJobOrderByPartyId/',
        data: { partyId: partyId }
    }).done(function (orderList) {
        $('#Batch_JobRefId').empty();
        if (orderList.length > 0) {
            $('#Batch_JobRefId').append(
                $('<option/>')
                    .attr('value', '')
                    .text("Select")
            );
            $.each(orderList, function (job) {
                $('#Batch_JobRefId').append(
                    $('<option/>')
                        .attr('value', this.Id)
                        .text(this.Value)
                );
            });
        } else {
            $('#Batch_JobRefId').append(
                $('<option/>')
                    .attr('value', '')
                    .text("No Job Order Found")
            );
        }
    });
});