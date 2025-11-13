
function getCostCentreBySector(sectorId) {   // This is for cascade dropdown 

    for (var i = 2; i < 10; i++) {
        var name = '#SalHead-' + i;
        $(name).val('');
    }

    var url = "/JournalVoucherEntry/GetCostCentreBySector";
    $.ajax({
        url: url,
        type: "get",
        data: { "sectorId": sectorId },
        dataType: 'json',
        success: function (data) {
            if (data.length != 0) {

                var items = "<option value='" + 0 + "'>" + " -Select - " + "</option>";

                $.each(data, function (i, costCentre) {

                    items += "<option value='" + costCentre.Id + "'>" + costCentre.CostCentreName + "</option>";
                });
                $('.CostCentre').html(items);
            } else {
                var items = '<option>Not found</option>';
                $('.CostCentre').html(items);
            }
        }
    });
}

function getSalaryMappingInfo(costCentreId) {

    var sectorId = $('#SectorId option:selected').val();
    
    var url = "/SalaryMapping/GetSalaryMapping";
    $.ajax({
        url: url,
        type: "get",
        data: { "sectorId": sectorId, "costCentreId": costCentreId },
        dataType: 'json',
        success: function(data) {
            if (data.length != 0) {
                
                var id = 2;
                $.each(data, function(i, lt) {
                    var name = '#SalHead-' + id;
                    $(name).val(lt);
                    id++;
                });
            }
        }
    });
}

function SaveSalaryValues() {

    var sectorId = $('#SectorId option:selected').val();
    var costCentreId = $('#CostCentreId option:selected').val();

    if (sectorId == null || sectorId < 1) {
        alert('Please select a Sector Name');
        return false;
    }

    if (costCentreId == null || costCentreId < 1) {
        alert('Please select a costCentre Name');
        return false;
    }

    var values = [];
    values[0] = sectorId;
    values[1] = costCentreId;

    for (var i = 2; i < 10; i++) {
        var Id = '#SalHead-' + i;
        values[i] = $(Id).val();
    }

    jQuery.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        url: "/SalaryMapping/SaveSalaryValues",
        data: JSON.stringify(values),
        type: "POST",
        success: function(r) {
            if (r.Success) {

                alert(r.Message);

            } else {
                self.DialogOpened();
            }
        }
    });
}