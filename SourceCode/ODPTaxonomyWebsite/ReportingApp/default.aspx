<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ODPTaxonomyWebsite.ReportingApp._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <!--Bootstrap latest compiled and minified CSS-->
   
    <link rel="stylesheet" href="css/report.css">
    <link rel="stylesheet" href="css/chosen.min.css"> 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div ng-app="reportingapp">

        <div ng-controller="MainReportController as vm">


            <div class="row rowsp">

                <div class="col-lg-12 col-md-12 sixteen columns">
                    <div id="pagetitlebox">
                        <h2>Kappa Reporting</h2>
                    </div>

                    <div class="report-spmenu">
                        <ul class="tabs-menu">
                            <li ng-repeat="t in vm.tabs" ng-click="vm.Go(t.panestate)" ng-class="{ 'current' : vm.Active(t.panestate) }">{{t.title}}</li>
                        </ul>
                    </div>

                    <!--Main content that changes based on route-->
                    <div ui-view class="container"></div>
                </div>
            </div>

        </div>

    </div>

<%--    http://jsfiddle.net/cojahmetov/3DS49/ --%>

<script type="text/javascript" src="../scripts/alertify.js"></script>
<script type="text/javascript" src="../scripts/angular/angular-latest.min.js"></script>
<script type="text/javascript" src="scripts/angular-ui-router.min.js"></script>
<script type="text/javascript" src="scripts/chosen.jquery.min.js"></script>
<script type="text/javascript" src="scripts/app.js"></script>
<script type="text/javascript" src="scripts/directives/directives.js"></script>
<script type="text/javascript" src="scripts/services/services.js"></script>
<script type="text/javascript" src="scripts/controllers/MainReportController.js"></script>
<script type="text/javascript" src="scripts/controllers/ReportController.js"></script>
<script type="text/javascript" src="scripts/controllers/AbstractSummaryReportController.js"></script>
<script type="text/javascript" src="scripts/controllers/AbstractValuesController.js"></script>

<%--<script src="../scripts/angular/firebase.js"></script>
<script src="../scripts/angular/angularfire.min.js"></script>--%>

</asp:Content>
