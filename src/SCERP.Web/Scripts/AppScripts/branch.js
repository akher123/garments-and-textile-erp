function getPoliceStationByDistrict(districtId) {
    var url = "/Branch/GetPoliceStationByDistrict";
    $.ajax({
        url: url,
        type: "get",
        data: { "districtId": districtId },
        dataType: 'json',
        success: function (data) {
            if (data.length!=0) {
                var items = '<option>-Select-</option>';

                $.each(data, function (i, district) {

                    items += "<option value='" + district.Id + "'>" + district.Name + "</option>";
                });
                $('#policeStation').html(items);
            } else {
                var items = '<option>Not found</option>';
                $('#policeStation').html(items);
            }
           
        }
    });
}