<%@ Page Title="" Language="C#" MasterPageFile="~/Print.Master" AutoEventWireup="true" CodeBehind="PrintAbstract.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.PrintAbstract" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
    .printTitle
        {
            font-weight: bold;
            padding: 0px 0px 0px 0px;
        }
     div.row
     {
         margin: 8px 0px 0px 0px;
     }
    
    </style>

     <div class="row container">
        <div class="six columns">
            <span class="printTitle">User ID: </span>
            <span class="printValue"><%= userID %></span>
        </div>

         <div class="five columns">
            <span class="printTitle">Date : </span>
            <span class="printValue"><%= System.DateTime.Now.ToString("MM/dd/yyyy") %></span>
        </div>
    </div>

    <div class="row container">
         <div class="fifteen columns">
            <span class="printTitle">Project Title : </span>
            <span class="printValue"><%= projectTitle %></span>
        </div>
    </div>

    <div class="row container">
     <div class="six columns">
        <span class="printTitle">Administering IC: </span>
        <span class="printValue"><%= administeringIC %></span>
    </div>

    <div class="five columns">
        <span class="printTitle">Application ID: </span>
        <span class="printValue"><%= applicationID %></span>
    </div>
    </div>

     

    <div class="row container">

        <div class="six columns">
            <span class="printTitle">PI Project Leader: </span>
            <span class="printValue"><%= PIProjectLeader %></span>
        </div>
        <div class="three columns">     
            <span class="printTitle">FY: </span>
            <span class="printValue"><%= FY %></span>
        </div>
        <div class="six columns">  
            <span class="printTitle">Project Number: </span>
            <span class="printValue"><%= ProjectNumber  %></span>
         </div>
    </div>

<%--    <div>
        <span class="printTitle">Project Number: </span>
        <span class="printValue"><%= ProjectNumber  %></span>
    </div>--%>
    <hr />
    <div>
        <div class="printTitle">Abstract Description :</div>

        <div class="hdesc">
        <%= desc %>
        </div>
        <hr />
        <div class="printTitle">Public Health Relevance :</div>
        
        <div class="hpart">
        <%= healthpart %>
        </div>
    </div>


</asp:Content>
