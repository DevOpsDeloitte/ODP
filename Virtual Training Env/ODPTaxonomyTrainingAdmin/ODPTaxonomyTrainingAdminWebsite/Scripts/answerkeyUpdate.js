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

function ConfirmUpdateValue(objMsg) {

    var valUPEnvProdTrain = $('#<%= rdoValUpEnv.ClientID %> input:checked').val()

    if (confirm(objMsg)) {
        if (valUPEnvProdTrain == "1") //production
            document.getElementById('<%=btn_prodMethod.ClientID%>').click();
        else if (valUPEnvProdTrain == "2") //training
            document.getElementById('<%=btn_trainMethod.ClientID%>').click();
    }
    else {
        window.location.href = "AnswerkeyUpdate.aspx";
    }
}
function ConfirmRerunKappa(objMsg) {

    if (confirm(objMsg)) {
        document.getElementById('<%=btn_rkMethod.ClientID%>').click();
    }
    else {
        window.location.href = "AnswerkeyUpdate.aspx";
    }
}
