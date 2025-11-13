$('.webgrid-table  thead tr th:first').html(  
$('<input/>', {
    type: 'checkbox',
    click: function () {
        var checkboxes = $(this).closest('table').find('tbody tr td input[type="checkbox"]');
        checkboxes.prop('checked', $(this).is(':checked'));
    }
})
);