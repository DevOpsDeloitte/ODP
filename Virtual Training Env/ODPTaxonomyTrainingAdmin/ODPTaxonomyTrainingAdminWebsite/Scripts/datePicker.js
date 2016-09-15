// DOM Ready      
$(function () {
    $(".date").datepicker({
        showOn: 'both',
        buttonImage: "/Images/Calendar.png",
        buttonImageOnly: true,
        dateFormat: 'mm/dd/yy',
        onClose: function () {
            $(this).focus();
        }
    });

});
