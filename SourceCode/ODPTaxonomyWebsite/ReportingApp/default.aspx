<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ODPTaxonomyWebsite.ReportingApp._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <!--Bootstrap latest compiled and minified CSS-->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
    <!--Optional theme-->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap-theme.min.css">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div ng-app="reportingapp">
    <h2>Reporting App</h2>
    <!--Main content that changes based on route-->
    <div ng-view class="container"></div>
    </div>

<%--    http://jsfiddle.net/cojahmetov/3DS49/ --%>
<script type="text/javascript" src="../scripts/alertify.js"></script>
<script type="text/javascript" src="../scripts/angular/angular-latest.min.js"></script>
<script src="https://code.angularjs.org/1.4.4/angular-route.js"></script>
<script type="text/javascript" src="scripts/app.js"></script>
<script type="text/javascript" src="scripts/controllers/ReportController.js"></script>

<%--<script src="../scripts/angular/firebase.js"></script>
<script src="../scripts/angular/angularfire.min.js"></script>--%>

</asp:Content>
