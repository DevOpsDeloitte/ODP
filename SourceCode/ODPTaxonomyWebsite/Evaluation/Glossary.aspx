<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Glossary.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.Glossary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="../scripts/jquery.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            var hash = window.location.hash.replace("#", '');
            console.log(hash);
            $("a[name*='" + hash + "'").parents("div.topicitem").addClass("selected");

        });
    
    </script>
    <style type="text/css">
    div.selected
    {
        background-color: #ccc;
        
    }
    div.topicitem
    {
        margin: 10px 0px 10px 0px;
    }
    </style>

    <div>


        <%=studyFocus %>
        <%=entitiesStudied %>
        <%=studySettings %>
        <%=populationFocus %>
        <%=studyDesignPurpose %>
        <%=preventionCategory %>
    
    
    </div>
</asp:Content>
