<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ViewAbstractList.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.ViewAbstractList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<%@ Register TagPrefix="odp" TagName="ODPSupervisorView_Default" Src="~/Evaluation/AbstractListViews/ODPSupervisorView_Default.ascx" %>
<%@ Register TagPrefix="odp" TagName="ODPSupervisorView_Open" Src="~/Evaluation/AbstractListViews/ODPSupervisorView_Open.ascx" %>
<%@ Register TagPrefix="odp" TagName="ODPSupervisorView_Review" Src="~/Evaluation/AbstractListViews/ODPSupervisorView_Review.ascx" %>
<%@ Register TagPrefix="odp" TagName="ODPSupervisorView_Review_Uncoded" Src="~/Evaluation/AbstractListViews/ODPSupervisorView_Review_Uncoded.ascx" %>
<%@ Register TagPrefix="odp" TagName="ODPStaffView_Default" Src="~/Evaluation/AbstractListViews/ODPStaffMemberView_Default.ascx" %>
<%@ Register TagPrefix="odp" TagName="ODPStaffView_Review" Src="~/Evaluation/AbstractListViews/ODPStaffMemberView_Review.ascx" %>
<%@ Register TagPrefix="odp" TagName="ODPStaffView_Review_Uncoded" Src="~/Evaluation/AbstractListViews/ODPStaffMemberView_Review_Uncoded.ascx" %>
<%@ Register TagPrefix="odp" TagName="CoderSupervisorView_Coded" Src="~/Evaluation/AbstractListViews/CoderSupervisorView_Coded.ascx" %>
<%@ Register TagPrefix="odp" TagName="CoderSupervisorView_Open" Src="~/Evaluation/AbstractListViews/CoderSupervisorView_Open.ascx" %>
<%@ Register TagPrefix="odp" TagName="AdminView" Src="~/Evaluation/AbstractListViews/AdminView.ascx" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<link rel="stylesheet" href="../styles/alertify.css">    
<link type="text/css" rel="Stylesheet" href="../styles/jquery.dataTables.min.css" /> 
<link type="text/css" rel="Stylesheet" href="/Styles/evaluation.css" />
<script type="text/javascript" src="../scripts/datatables/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="../scripts/datatables/underscore-min.js"></script>
<script src="../scripts/alertify.js"></script>
<script src="../scripts/datatables/spin.min.js"></script>
<script type="text/javascript" src="/Scripts/datatables/util.js"></script>


 <script type="text/javascript">

     window.user = {};
     window.user.GUID = '<%= userGUID %>';
     //window.config.role = "ODPSupervisor";
     window.config.role = '<%= userROLE %>';


     function checkStatus() {

         if ($("input.review").hasClass("no")) {

             return false;
         }

         return true;

     }
    

</script>

 <script type="text/javascript">
    var time = new Date().getTime();
    $(document.body).bind("mousemove keypress", function (e) {
        time = new Date().getTime();
        //console.log("timer updated :: " + time);
    });

    function refresh() {
        //console.log("refresh timer ::");
        if (new Date().getTime() - time >= 900000)
            window.location.reload(true);
        else
            setTimeout(refresh, 30000);
    }

    setTimeout(refresh, 30000);
</script>

<script type="text/javascript" src="/Scripts/datatables/app.js"></script>
    <h2>
    </h2>
    
    <!--</div>-->
    <div>   
   
    
            <asp:Label runat="server" ID="lbl_exportError" ForeColor="Red"></asp:Label>
        <asp:DropDownList runat="server" ID="MainviewDDL" AutoPostBack="true" OnSelectedIndexChanged="MainviewChangeHandler" Visible="false" />
        <asp:Panel runat="server" ID="SubviewPanel" Visible="false">
            <h3>
                <asp:Label runat="server" ID="SubviewLabel" Visible="false" />
                <asp:DropDownList runat="server" ID="SubviewDDL" AutoPostBack="true" OnSelectedIndexChanged="SubviewChangeHandler"
                    Visible="false" />
            </h3>
        </asp:Panel>
<%--        <asp:Panel runat="server" ID="PagerWrapper" Visible="false" class="pager-size-wrapper">
            <asp:Label runat="server" AssociatedControlID="PagerSizeDDL"></asp:Label>
            <asp:DropDownList runat="server" ID="PagerSizeDDL" AutoPostBack="true" OnSelectedIndexChanged="PagerSizeChangeHandler">
                <asp:ListItem Value="25" Text="Show: 25 Results" />
                <asp:ListItem Value="50" Text="Show: 50 Results" />
                <asp:ListItem Value="100" Text="Show: 100 Results" />
            </asp:DropDownList>
        </asp:Panel>
        --%>
        <odp:ODPSupervisorView_Default runat="server" ID="ODPSupervisorView_Default" Visible="false" />
        <odp:ODPSupervisorView_Open runat="server" ID="ODPSupervisorView_Open" Visible="false" />
        <odp:ODPSupervisorView_Review runat="server" ID="ODPSupervisorView_Review" Visible="false" />
        <odp:ODPSupervisorView_Review_Uncoded runat="server" ID="ODPSupervisorView_Review_Uncoded"
            Visible="false" />
        <odp:ODPStaffView_Default runat="server" ID="ODPStaffView_Default" Visible="false" />
        <odp:ODPStaffView_Review runat="server" ID="ODPStaffView_Review" Visible="false" />
        <odp:ODPStaffView_Review_Uncoded runat="server" ID="ODPStaffView_Review_Uncoded"
            Visible="false" />
        <odp:CoderSupervisorView_Coded runat="server" ID="CoderSupervisor_Coded" Visible="false" />
        <odp:CoderSupervisorView_Open runat="server" ID="CoderSupervisor_Open" Visible="false" />
        <odp:AdminView runat="server" ID="AdminView" Visible="false" />
    </div>   

</asp:Content>
