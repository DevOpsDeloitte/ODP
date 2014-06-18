<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditAccountConfirm.aspx.cs" Inherits="ODPTaxonomyWebsite.AccountManagement.EditAccountConfim" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:Panel ID="pnl_confirmation" runat="server" class="Message">
    <asp:Literal ID="ltl_message" runat="server" Text="Account Created Successfully." />
</asp:Panel>

<asp:Label ID="lbl_error_message" CssClass="errorMessage" runat="server" Visible="false" />

<asp:panel ID="pnl_account" runat="server">
<table class="form">
    <tr>
        <td colspan="2">User ID is: <strong><asp:Label ID="lbl_userName" runat="server" /></strong></td>
    </tr>
    <tr>
        <td colspan="2"><asp:Label id="lbl_name" runat="server" /></td>
    </tr>
    <tr>
        <td colspan="2"><asp:Label ID="lbl_email" runat="server" /></td>
    </tr>
</table>

<asp:LinkButton ID="lnkbtn_create_account" runat="server" Text="Create New Account" CssClass="button" OnClick="lnkbtn_createAccount_OnClick"/>
<asp:LinkButton ID="lnkbtn_manage_account" runat="server" Text="Return to Manage Accounts" CssClass="button" OnClick="lnkbtn_manageAccount_OnClick" />

</asp:panel>

</asp:Content>
