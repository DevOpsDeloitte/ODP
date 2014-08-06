<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="ODPTaxonomyWebsite.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:Label ID="lbl_error_message" runat="server" Visible="false" CssClass="errorMessage" />

<asp:Panel ID="pnl_confirmation" runat="server" Visible="false" CssClass="Message">
    Your password has been reset and sent to your email address.  Please login and change your password.
</asp:Panel>

<asp:Button ID="btn_return" runat="server" Text="Return to Login" OnClick="btn_return_Click" class="button center" CausesValidation="false" Visible="false" />

<asp:Panel ID="pnl_forgot_password" runat="server">
<p>Please enter your user name to receive your password.</p>
<table class="form">
    <tr>
        <td>
            <asp:Label ID="lbl_forgotpwd_username" runat="server" Text="User name:" AssociatedControlID="txt_forgotpwd_username" />
        </td>
        <td>
            <asp:TextBox ID="txt_forgotpwd_username" runat="server" TextMode="SingleLine" />
            <asp:RequiredFieldValidator ID="reqval_forgotpwd_username" runat="server" SetFocusOnError="true" ControlToValidate="txt_forgotpwd_username" Display="Dynamic" ErrorMessage="User name is required." CssClass="errorMessage" />
        </td>
        <td>
            
        </td>
    </tr>
    <tr>
        <td class="form-button">
            <asp:Button ID="btn_forgotpwd_submit" runat="server" Text="Submit" OnClick="btn_forgotpwd_submit_Click" class="button yes" />
        </td>
        <td class="form-button">
            <asp:Button ID="btn_forgotpwd_cancel" runat="server" Text="Cancel" OnClick="btn_forgotpwd_cancel_Click" class="button no" CausesValidation="false" />
        </td>
    </tr>
</table>

</asp:Panel>
</asp:Content>
