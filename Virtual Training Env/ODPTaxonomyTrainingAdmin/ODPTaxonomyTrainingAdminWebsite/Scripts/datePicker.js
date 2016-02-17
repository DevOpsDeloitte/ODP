// DOM Ready      
$(function () {
        $(".date").datepicker({
            showOn: 'both',
            buttonImage: "Images/Calendar.png",
            buttonImageOnly: true,
            dateFormat: 'mm/dd/yy',
            onClose: function () {
                $(this).focus();
            }
        });

        $("[id$=btn_chk_val]").click(function (e) {
            e.preventDefault();
            var startdate = new Date();
            startdate = document.getElementById('<%=txt_frm_date.ClientID%>').value;
            // startdate = Date.parse($("id$=txt_frm_date").val());
            var enddate = new Date();
            enddate = document.getElementById('<%=txt_to_date.ClientID%>').value;
            //enddate = Date.parse($("id$=txt_to_date").val());
            if (startdate > enddate) {
                alert('* To date must be after From date');
                //  $("id$=txt_frm_date").val('');
                // $("id$=txt_to_date").val('');
            }
        });
   
    
});