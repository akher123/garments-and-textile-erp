var Buyer = new function () {
    SCERP.SubmitBuyerPartialChange = function (e) {
        var getContactPerson = function () {
            var buyerContactPersonList = [];
            var table = $('.formBuyer #grid tbody');
            table.find('tr').each(function (i) {
                var $tds = $(this).find('td');
                var contactPerson = {
                    "Id": $tds.eq(0).text(),
                    "Name": $tds.eq(1).text(),
                    "Address": $tds.eq(2).text(),
                    "Email": $tds.eq(3).text(),
                    "Phone": $tds.eq(4).text(),
                };
                buyerContactPersonList.push(contactPerson);
            });
            return buyerContactPersonList;
        };

        var button = $(this);
        var form = button.parents("form:first");
        var action = form.attr("action");
        var buyer = form.serializeObject();

        var options = {
            success: function (data) {
                if (data.Success == true) {
                    button.DialogClose();
                    loadAction('/Merchandising/Buyer/Index/');

                } else {
                    alert(data.Message);
                }

            }
        };
        options.url = action;
        options.type = "POST";
        options.dataType = "JSON";
        options.contentType = "application/json",
        options.data = JSON.stringify({ "Buyer": buyer, "ContactPersons": getContactPerson() });
        $.ajax(options);
    };


    SCERP.AddtempClick = function (e) {
        var action = $(this).attr("action");
        if (action) {
            jQuery.Ajax({
                url: action,
                type: "GET",
                dialog: {title:"Add Buyer Contact Person"},
                loader: { loader: "body" },
                success: SCERP.DialogTempOpened
            });
        }
        e.preventDefault();
        return false;
    };

    SCERP.DialogTempOpened = function (r) {
    
        $(":submit", ".dialog-ontainer").unbind("click").bind("click", function (e) {
            var button = $(this);
            SCERP.AddGridRow(button);
        });
    };

    SCERP.AddGridRow = function (r) {
        var contactPreson = $('.formBuyerContactPerson').serializeObject();
        var editpannel = "<td class='buttons'>" +
            "<button class='editCallBack' data-status='true' type='submit' title='Edit'  action='/Merchandising/BuyerContactPerson/Edit/0'></button>" + "</td>" +
            "<td class='buttons'>" +
            "<button class='deleteCallBack' type='submit' title='deleteCallBack' action='/Merchandising/BuyerContactPerson/Delete/0'></button>" +
            "</td>";
        var form = $(":submit", ".dialog-ontainer").parents("form:first");
        $('#grid tbody', form).append('<tr><td>' + 0 + '</td><td>' + contactPreson.Name + '</td><td>' + contactPreson.Address + '</td><td>'
            + contactPreson.Email + '</td><td>' + contactPreson.Phone + '</td>' + editpannel + '</tr>');
        r.DialogClose();
        SCERP.GridInitialize();
    };
    

    SCERP.AddtempEditClick = function (e) {
        var action = $(this).attr("action");
    
        var $tr = $(this).closest('tr');
        var $tds = $tr.find('td');
        var contactPerson = {
            "Id": $tds.eq(0).text(),
            "Name": $tds.eq(1).text(),
            "Address": $tds.eq(2).text(),
            "Email": $tds.eq(3).text(),
            "Phone": $tds.eq(4).text(),
        };


        if (action) {
            jQuery.Ajax({
                url: action,
                type: "POST",
                dataType: "Json",
                data: contactPerson ,
                dialog: { },
                loader: { loader: "body" },
                success: SCERP.DialogTempEditOpened
            });
           
        }
        e.preventDefault();
        return false;
    };

    SCERP.DialogTempEditOpened = function (r) {
        $(":submit", ".dialog-ontainer").unbind("click").bind("click", function (e) {
            var button = $(this);
            jQuery.Ajax({
             
                url: ""
           , success: SCERP.AddGridRowEdit(button)
            });
           
        });
    };
    SCERP.AddGridRowEdit = function (r) {
        var contactPreson = $('.formBuyerContactPerson').serializeObject();
        var editpannel = "<td class='buttons'>" +
            "<button class='editCallBack' data-status='true' type='submit' title='Edit'  action='/Merchandising/BuyerContactPerson/Edit/0'></button>" + "</td>" +
            "<td class='buttons'>" +
            "<button class='deleteCallBack' type='submit' title='deleteCallBack' action='/Merchandising/BuyerContactPerson/Delete/0'></button>" +
            "</td>";
        var form = $(":submit", ".dialog-ontainer").parents("form:first");
        $('#grid tbody', form).append('<tr><td>' + 0 + '</td><td>' + contactPreson.Name + '</td><td>' + contactPreson.Address + '</td><td>'
            + contactPreson.Email + '</td><td>' + contactPreson.Phone + '</td>' + editpannel + '</tr>');
        r.DialogClose();
    };
};

