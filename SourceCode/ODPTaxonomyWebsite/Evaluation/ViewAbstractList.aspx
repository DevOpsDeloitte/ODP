﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
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
<link type="text/css" rel="Stylesheet" href="/Styles/evaluation.css" />
<link type="text/css" rel="Stylesheet" href="http://cdn.datatables.net/1.10.3/css/jquery.dataTables.min.css" />
<script type="text/javascript" src="http://cdn.datatables.net/1.10.3/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="http://underscorejs.org/underscore-min.js"></script>
<script src="../scripts/alertify.js"></script>
<script src="../scripts/datatables/spin.min.js"></script>
<!--
<script type="text/javascript" src="http://cdn.datatables.net/plug-ins/a5734b29083/integration/bootstrap/3/dataTables.bootstrap.js"></script>
<link type="text/css" rel="Stylesheet" href="http://cdn.datatables.net/plug-ins/a5734b29083/integration/bootstrap/3/dataTables.bootstrap.css" />

-->


 <script type="text/javascript">

     window.user = {};
     window.user.GUID = '<%= userGUID %>';

     function checkStatus() {

         if ($("input.review").hasClass("no")) {

             return false;
         }

         return true;

     }

//     $(document).ready(function () {



//         $("input[type=checkbox]").click(function () {
//             var keepno = true;
//             $("input[type=checkbox]").each(function () {
//                 if (this.checked) keepno = false;
//             });

//             if (keepno) {
//                 $("input.review").addClass("no").removeClass("yes");
//             }
//             else {
//                 $("input.review").removeClass("no").addClass("yes");
//             }

//         });


//     });
</script>
    <h2>
    </h2>
    
    </div>
    <div>
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
