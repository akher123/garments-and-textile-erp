$('#filterbyEmployeeCadrId').unbind("click").bind("click", clickEventHendeler);

function clickEventHendeler() {
  var employeeCadrIdTextBox= $('.employeeCardId');
  var employeeCardId = employeeCadrIdTextBox.val();
        var $this = $(this);
        var option = {
            url: $this.attr('action'),
            type: 'GET',
            data: { EmployeeCardId: employeeCardId }
        };
        if (jQuery.validator && jQuery.validator.unobtrusive) {
            employeeCadrIdTextBox.validate();
            if (!employeeCadrIdTextBox.valid()) {
                return false;
            }
            loadEmployeeDetail(option);
        }
}
    function loadEmployeeDetail(option) {
        return $.Ajax(option).done(function(respons) {
            if (respons.Success && respons.Success != 'undefined') {
                $('#employeeDetail_containner').html(respons.EmployeeDetailView);
                $("#displayButton").removeAttr("style");
            }
            
            if (respons.ValidStatus == false) {
              $("#displayButton").css("display","none");
              alert(respons.Message);
            }
        });
    }

