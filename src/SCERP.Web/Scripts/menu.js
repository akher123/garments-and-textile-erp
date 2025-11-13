
$(".treeview li>ul").css('display', 'none');
$(".collapsible").click(function (e) {
    e.preventDefault();
    $(this).toggleClass("collapse expand");
    $(this).closest('li').children('ul').slideToggle();
});
var url = '';
function loadAction(url) {
 
    if (url.length > 0) {
        $("body").showLoading();
        if (url != undefined && url != "") {
            requseturl = url;
            jQuery.Ajax({
                url: url,
                container: '#contentRight'
            });

        } else {
            requseturl = "";
        }
    } else {
        return false;
    }
 

}

//Merchandising 

$('.ajaxLoad').click(function () {
    var url = $(this).attr('action');
    loadAction(url);
});







