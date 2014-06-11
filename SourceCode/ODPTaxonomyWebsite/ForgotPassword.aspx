<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="ODPTaxonomyWebsite.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:Label ID="lbl_error_message" runat="server" Visible="false" CssClass="errorMessage" />

<asp:Panel ID="pnl_confirmation" runat="server" Visible="false">
    Your password has been reset and sent to your email address.  Please login and change your password.
</asp:Panel>

<asp:Panel ID="pnl_forgot_password" runat="server">
<p>Please enter your user name to receive your password.</p>
<table class="form">
    <tr>
        <td>
            <asp:Label ID="lbl_forgotpwd_username" runat="server" Text="User name:" AssociatedControlID="txt_forgotpwd_username" />
        </td>
        <td>
            <asp:TextBox ID="txt_forgotpwd_username" runat="server" TextMode="SingleLine" /><asp:RequiredFieldValidator ID="reqval_forgotpwd_username" runat="server" SetFocusOnError="true" ControlToValidate="txt_forgotpwd_username" ErrorMessage="User name is required" CssClass="errorMessage inline" />
        </td>
        <td>
            
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btn_forgotpwd_submit" runat="server" Text="Submit" OnClick="btn_forgotpwd_submit_Click" class="button" />
        </td>
        <td>&nbsp;</td>
    </tr>
</table>

</asp:Panel>
</asp:Content>
