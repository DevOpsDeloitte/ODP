<%@ Page Title="" Language="C#" MasterPageFile="~/Print.Master" AutoEventWireup="true" CodeBehind="Glossary.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.Glossary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script src="../scripts/jquery.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            var hash = window.location.hash.replace("#", '');
            console.log(hash);
            $("a[name*='" + hash + "'").parents("div.topicitem").addClass("selected");

        });
    
    </script>
    <div id="mainGlossary">


        <%=studyFocus %>
        <%=entitiesStudied %>
        <%=studySettings %>
        <%=populationFocus %>
        <%=studyDesignPurpose %>
        <%=preventionCategory %>
    
    
    </div>
</asp:Content>
