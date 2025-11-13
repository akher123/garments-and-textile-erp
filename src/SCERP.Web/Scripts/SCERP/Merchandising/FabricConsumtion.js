
$('#btnBodyFabricsConsumtion').click(function () {
  var  BodyFabricConsumtion = {
        BodyLength: $(".BodyLength").val(),
        SleevLength: $(".SleevLength").val(),
        LengthWiseAllowance: $(".LengthWiseAllowance").val(),
        HalfChest: $(".HalfChest").val(),
        WidthWiseAllowance: $(".WidthWiseAllowance").val(),
        GSM: $(".bdfGSM").val(),
        Wastage: $(".bdfWastage").val()
    };
  var bodyFabConsumtion = FabricConsumtion.GetBodyFabricConsumtion(BodyFabricConsumtion);
  $('.BodyFabConsumtion').val((parseFloat(bodyFabConsumtion, 10).toFixed(2)));
});

$('#btnRibFabricsConsumtion').click(function () {
 
    var RibFabricConsumtion = {
        NeekWidth: $('.NeekWidth').val(),
        RibHeight: $('.RibHeight').val(),
        GSM: $('.RibGSM').val(),
        Allowance: $('.RibAllowance').val(),
        Wastage: $('.RibWastage').val(),
        FontNeekDrop: $('.RibFontNeekDrop').val()
    };

   var ribFabConsumtion = FabricConsumtion.GetRibFabricConsumtion(RibFabricConsumtion);
   $('.RibFabConsumtion').val((parseFloat(ribFabConsumtion, 10).toFixed(2)));

});


$('#btnMiscllFabricsConsumtion').click(function (parameters) {
   var MiscellaneousFabricConsumtion = {
        Length: $('.Length').val(),
        Width: $('.Width').val(),
        GSM: $('.MiscllGSM').val(),
        NumberOfPcs: $('.NumberOfPcs').val(),
        Allowance: $('.MiscllAllowance').val(),
        Wastage: $('.MiscllWastage').val(),
    };
    var miscFabConsumtion = FabricConsumtion.GetMiscellaneousFabricConsumtion(MiscellaneousFabricConsumtion);
    $('.MiscellaneousFabConsumtion').val((parseFloat(miscFabConsumtion, 10).toFixed(2)));
});

$('#btnTotalFabricsConsumtion').click(function () {
   var ribCons=  $('.RibFabConsumtion').val();
   var bodyFac= $('.BodyFabConsumtion').val();
   var misFab = $('.MiscellaneousFabConsumtion').val();
   var totalFab = (parseFloat(misFab, 10) + parseFloat(ribCons, 10) + parseFloat(bodyFac, 10));

   $('.TotalFabConsumtion').val(totalFab.toFixed(2));

});

 var  FabricConsumtion = {

        GetBodyFabricConsumtion: function(bodyFabric) {
            return this.GetBodyFabricConsumtionWithoutWstage(bodyFabric) + this.GetBodyFabricConsumtionWithoutWstage(bodyFabric) * parseFloat(bodyFabric.Wastage, 10) / 100;
        },
        GetRibFabricConsumtion: function (ribFabric) {
            return this.GetRibFabricConsumtionWithoutWastage(ribFabric) + this.GetRibFabricConsumtionWithoutWastage(ribFabric) * parseFloat(ribFabric.Wastage, 10) / 100;
        },
        GetMiscellaneousFabricConsumtion: function(misFabric) {
            return this.GetMiscellaneousFabricConsumtionWithoutWastage(misFabric) + this.GetMiscellaneousFabricConsumtionWithoutWastage(misFabric) * parseFloat(misFabric.Wastage, 10) / 100;
        },

        GetBodyFabricConsumtionWithoutWstage: function(bodyFabric) {
            return (parseFloat(bodyFabric.BodyLength,10) +parseFloat(bodyFabric.SleevLength,10) +parseFloat(bodyFabric.LengthWiseAllowance,10)) *
                (parseFloat(bodyFabric.HalfChest, 10) * parseFloat(bodyFabric.WidthWiseAllowance, 10)) * (parseFloat(bodyFabric.GSM, 10) * 2 * 12) / 10000000;
        },
        GetRibFabricConsumtionWithoutWastage: function (ribFabric) {
          
            return ((parseFloat(ribFabric.NeekWidth, 10) * parseFloat(ribFabric.RibHeight, 10) * parseFloat(ribFabric.FontNeekDrop, 10) * 12 * parseFloat(ribFabric.GSM, 10)) +
                parseFloat(ribFabric.Allowance,10)) / 10000000;
        },
        GetMiscellaneousFabricConsumtionWithoutWastage: function(misFabric) {
            return (parseFloat(misFabric.Length, 10) * parseFloat(misFabric.Width, 10) * misFabric.GSM *
                 parseFloat(misFabric.NumberOfPcs, 10) * 12 + parseFloat(misFabric.Allowance, 10)) / 10000000;
        }
    };




