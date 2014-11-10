<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ODPTaxonomyTrainingAdminWebsite._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<div class="sixteen columns sub-title"> 
    <h2>HOMEPAGE</h2>
</div>            
<div class="sixteen columns"> 
    <asp:Label runat="server" CssClass="errorMessage" ID="lbl_Error" class="panel" Visible="false"></asp:Label>
    <asp:Label runat="server" CssClass="regularMessage" ID="lbl_messageUsers" class="panel" Visible="false"></asp:Label>   
    
    <asp:Panel runat="server" ID="pnl_training" class="panel">
        <span class="subtitle center">Training</span>
        <div class="center">
            <asp:DropDownList ID="ddl_instances" runat="server" Width="250px" />
            <asp:Button class="button" runat="server" ID="btn_push" Text="Push Trainee Data" onclick="btn_push_Click" />
            <asp:Button class="button" runat="server" ID="btn_pull" Text="Pull Trainee KAPPA" onclick="btn_pull_Click" />
        </div>
    </asp:Panel>
    </div>
</asp:Content>
