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
         margin: 10px 0px 0px 0px;
     }
    
    </style>
<div>
    <div class="row">
        <span class="printTitle">User ID: </span>
        <span class="printValue"><%= userID %></span>
    </div>

     <div class="row">
        <span class="printTitle">Date : </span>
        <span class="printValue"><%= System.DateTime.Now.ToString("MM/dd/yyyy") %></span>
    </div>

     <div class="row">
        <span class="printTitle">Project Title : </span>
        <span class="printValue"><%= projectTitle %></span>
    </div>

     <div class="row">
        <span class="printTitle">Administering IC: </span>
        <span class="printValue"><%= administeringIC %></span>
    </div>

    <div class="row">
        <span class="printTitle">Application ID: </span>
        <span class="printValue"><%= applicationID %></span>
    </div>

     <div class="row">
        <span class="printTitle">PI Project Leader: </span>
        <span class="printValue"><%= PIProjectLeader %></span>
    </div>

    <div class="row">
        <span class="printTitle">FY: </span>
        <span class="printValue"><%= FY %></span>
    </div>

    <div>
        <span class="printTitle">Project Number: </span>
        <span class="printValue"><%= ProjectNumber  %></span>
    </div>

    <div>
        <div class="printTitle">Description :</div>
        <div>
        <%= desc %>
        </div>

        <div class="printTitle">Public Health Part :</div>
        <div>
        <%= healthpart %>
        </div>
    </div>

</div>
</asp:Content>
