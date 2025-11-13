function AddRows(obj, subGroupId,branchId) {
    var $tr = $(obj).closest("tr").clone();
    var itemCode = $tr.find('td:eq(4)').html();
   // var incrementedItemCode = parseInt(itemCode) + 1;
    $.Ajax({
        url: "/InventoryItem/GetItemCode",
        type: "GET",
        dataType: "json",
        data: { subGroupId: subGroupId, branchId: branchId },
        success: function(respons) {
            $tr.find('td:eq(4)').html(respons.ItemCode);
            $tr.find('td:eq(7)').find("input[type=button]").attr('onclick', 'SaveItem(this,' + 0 + ',' + 0 + ')');
            $tr.find('td:eq(8)').find("input[type=button]").attr('data', '0');
            
            $tr.find('td:eq(5)').find("input").val('');
            $tr.find('td:eq(5)').find("input").attr('class', 'ControlItemAutocomplite rediousTextBox');
            $tr.insertAfter($(obj).closest("tr"));
        }
       
    });
  

}
function GroupAutoComplite(obj, branchId) {
    var $tr = $(obj).closest("tr");
    $tr.find('td:eq(7)').find("input[type=button]").val('Save');
    $(obj).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/InventoryItem/AutocompliteGroup",
                type: "POST",
                dataType: "json",
                data: { groupName: request.term, branchId: branchId },
                success: function (data) {

                    response($.map(data, function (item) {

                        return { label: item.GroupName, value: item.GroupName };
                    }));

                }
            });
        },
    });
}

function SubGroupAutoComplite(obj, branchId, groupId) {

    var $tr = $(obj).closest("tr");
    var value = $tr.children('td:first').find("input").val();

    if ($(obj).attr('id') >= 0) {

        $tr.find('td:eq(2)').html(value + '001');
    }
    $tr.find('td:eq(7)').find("input[type=button]").val('Save');
    $(obj).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/InventoryItem/AutocompliteSubGroup",
                type: "POST",
                dataType: "json",
                data: { subGroupName: request.term, branchId: branchId, groupId: groupId },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.SubGroupName, value: item.SubGroupName };
                    }));

                }
            });
        },

    });
}
function ControlItemAutoComplite(obj, subGroupId) {
    var $tr = $(obj).closest("tr");
    var subGroupCode = $tr.find('td:eq(2)').html();
    var $subGroupButton = $tr.find('td:eq(8)').find("input[type=button]");
    var isGroup = $subGroupButton.attr('data');
    if ($(obj).attr('id') == 0) {
        $(obj).attr('id', '1');
        $tr.find('td:eq(4)').html(subGroupCode + '001');
    }
    if (isGroup != 0) {
        $subGroupButton.attr('data', '1');
        $tr.find('td:eq(4)').html(subGroupCode + '001');
    }
    $tr.find('td:eq(7)').find("input[type=button]").val('Save');
    $(".ControlItemAutocomplite").autocomplete({
        source: function (request, response) {
            var option = {
                url: "/InventoryItem/AutocompliteItem",
                type: "POST",
                data: { itemName: request.term, subGroupId: subGroupId },
            };
            $.Ajax(option)
               .done(function (data) {
                   response($.map(data, function (item) {
                       return { label: item.ItemCode + "_" + item.ItemName, value: item.ItemName, ItemId: item.ItemId };
                   }));
               });
        },

    });
}

function SaveItem(obj, groupId, subGroupId) {
    var itemId = $(obj).attr('data');
    var $tr = $(obj).closest("tr");
    var groupCode = $tr.find('td:eq(0)').find("input").val();
    var groupName = $tr.find('td:eq(1)').find("input").val();
    var subGroupCode = $tr.find('td:eq(2)').html();
    var subGroupName = $tr.find('td:eq(3)').find("input").val();
    var itemCode = $tr.find('td:eq(4)').html();
    var itemName = $tr.find('td:eq(5)').find("input").val();
    var branchId = $('.Inventory_SearchBranchByCompany').val();
    var model = {
        "ItemId": itemId,
        "GroupCode": groupCode,
        "GroupName": groupName,
        "SubGroupCode": subGroupCode,
        "SubGroupName": subGroupName,
        "ItemCode": itemCode,
        "ItemName": itemName,
        "SubGroupId": subGroupId,
        "GroupId": groupId,
        "BranchId": branchId

    };

    var option = {
        url: '/InventoryItem/Save',
        data: model
    };
    if (model.BranchId != 0) {
        $.Ajax(option).done(function (response) {
            $tr.find('td:eq(8)').find("input[type=button]").attr('data', '0');
            if (response.Status) {
                alert(response.MessageResult);
                var formClssName = requseturl.split('/')[2] + 'SearchForm';
                search(requseturl, formClssName);
            }
      
        });
    } else {
        alert("Select Baranch ");
    }
}

$('.addNewGroup').unbind('click').bind('click', function () {
    var $branchDropDownList = $('.Inventory_SearchBranchByCompany');
    var branchId = $branchDropDownList.val();
    var option = {
        data: { branchId: branchId },
        url: '/InventoryItem/GetMaxGroupCode',
    };
    if (jQuery.validator && jQuery.validator.unobtrusive) {
        $branchDropDownList.validate();
        if (!$branchDropDownList.valid()) {
            e.preventDefault();
            return false;
        } else {

            $.Ajax(option).done(function (respons) {
                var $trLast = $('#grid').find("tr:last"),
                    $trNew = $trLast.clone();
                $trNew.find('td:eq(0)').find("input").val(respons);
                $trNew.find('td:eq(1)').find("input").val('');
                $trNew.find('td:eq(2)').html('');
                $trNew.find('td:eq(3)').find("input").val('');
                $trNew.find('td:eq(4)').html('');
                $trNew.find('td:eq(5)').find("input").attr('id', '0');
                $trNew.find('td:eq(5)').find("input").val('');
                $trNew.find('td:eq(5)').find("input").attr('class', 'ControlItemAutocomplite rediousTextBox');
                $trNew.find('td:eq(3)').find("input").attr('data', '1');
                $trNew.find('td:eq(7)').find("input[type=button]").attr('data', '0');
                $trNew.find('td:eq(7)').find("input[type=button]").attr('onclick', '').attr('onclick', 'SaveItem(this,' + 0 + ',' + 0 + ')');
                $trNew.find("td:lt(10)").css("backgroundColor", "#add8e6");
                $('#grid').append($trNew);
            });
        }
    }
});
function DeleteTR(obj) {
    var $tr = $(obj).closest("tr");
    $tr.find('td:eq(3)').find("input").attr('data', '1');
    $tr.remove();
}
function AddSubGroup(obj, GroupId, branchId) {
    var isGroup = 1;
    var $tr = $(obj).closest("tr");
    var $newtr = $tr.clone();
    $.Ajax({
        url: "/InventoryItem/GetMaxSubGroupCode",
        type: "GET",
        dataType: "json",
        data: { groupId: GroupId, branchId: branchId },
        success: function (respons) {
            $newtr.find('td:eq(2)').html(respons.SubGroupCode);
            $newtr.find('td:eq(3)').find('input').attr('id', '-1');
            var groupId = $newtr.find('td:eq(7)').find("input[type=button]").attr('id').split('-')[0];
            $newtr.find('td:eq(7)').find("input[type=button]").attr('onclick', 'SaveItem(this,' + groupId + ',' + 0 + ')');
            $newtr.find('td:eq(5)').find("input").val('');
            $newtr.find('td:eq(5)').find("input").attr('class', 'ControlItemAutocomplite rediousTextBox');
            $newtr.find('td:eq(3)').find("input").val('');
            $newtr.find('td:eq(4)').html('');
            $(obj).attr('data', isGroup);
            $newtr.find("td:lt(10)").css("backgroundColor", "#afeeee");
            $newtr.insertAfter($tr);
        }

    });

}

$(".SearchGroupAutocomplite").autocomplete({
    source: function (request, response) {
        var $branchDropDownList = $('.Inventory_SearchBranchByCompany');
        var branchId = $branchDropDownList.val();
        var option = {
            url: "/InventoryItem/AutocompliteGroup",
            type: "POST",
            data: { groupName: request.term, branchId: branchId }
        };
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            $branchDropDownList.validate();
            if (!$branchDropDownList.valid()) {
                e.preventDefault();
                return false;
            }
            $.Ajax(option)
                .done(function (data) {
                    response($.map(data, function (item) {

                        return { label: item.GroupCode + "_" + item.GroupName, value: item.GroupName, GroupId: item.GroupId };
                    }));

                });

        }
    },
    select: function (event, ui) {
        $('.SelectedGroupId').val('');
        $('.SelectedSubGroupId').val('');
        $('.SearchSubGroupAutocomplite').val('');
        $('.SearchItemAutocomplite').val('');
        $('.SelectedItemId').val('');
        $('.SelectedGroupId').val(ui.item.GroupId);
    },
});

$(".SearchSubGroupAutocomplite").autocomplete({
    source: function (request, response) {
        var $branchDropDownList = $('.Inventory_SearchBranchByCompany');
        var $group = $('.SearchGroupAutocomplite');
        var $groupHiddenField = $('.SelectedGroupId');
        var branchId = $branchDropDownList.val();
        var groupId = $groupHiddenField.val();
        var option = {
            url: "/InventoryItem/AutocompliteSubGroup",
            type: "POST",
            data: { subGroupName: request.term, branchId: branchId, groupId: groupId },
        };
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            $branchDropDownList.validate();
            $group.validate();
            if (!$branchDropDownList.valid() && !$group.valid()) {
                e.preventDefault();
                return false;
            } else {
                $.Ajax(option)
                .done(function (data) {
                    response($.map(data, function (item) {
                        return { label: item.SubGroupCode + "_" + item.SubGroupName, value: item.SubGroupName, SubGroupId: item.SubGroupId };
                    }));

                });
            }


        }
    },
    select: function (event, ui) {
        $('.SelectedSubGroupId').val('');
        $('.SelectedItemId').val('');
        $('.SearchItemAutocomplite').val('');
        $('.SelectedSubGroupId').val(ui.item.SubGroupId);
    },

});

$(".SearchItemAutocomplite").autocomplete({
    source: function (request, response) {
        var $branchDropDownList = $('.Inventory_SearchBranchByCompany');
        var $subgroup = $('.SearchSuGroupAutocomplite');
        var $subGroupHiddenField = $('.SelectedSubGroupId');
        var subgroupId = $subGroupHiddenField.val();
        var option = {
            url: "/InventoryItem/AutocompliteItem",
            type: "POST",
            data: { itemName: request.term, subGroupId: subgroupId },
        };
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            $branchDropDownList.validate();
            $subgroup.validate();
            if (!$branchDropDownList.valid() && !$subgroup.valid()) {
                e.preventDefault();
                return false;
            } else {
                $.Ajax(option)
                .done(function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ItemCode + "_" + item.ItemName, value: item.ItemName, ItemId: item.ItemId };
                    }));
                });
            }

        }
    },
    select: function (event, ui) {
        $('.SelectedItemId').val('');
        $('.SelectedItemId').val(ui.item.ItemId);
    },

});


