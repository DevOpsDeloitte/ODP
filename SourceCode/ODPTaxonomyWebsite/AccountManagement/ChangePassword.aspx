<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="ODPTaxonomyWebsite.AccountManagement.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:Label ID="lbl_error_message" runat="server" Visible="false" CssClass="errorMessage" />

<asp:Panel ID="pnl_confirmation" runat="server" Visible="false">
    Your password is successfully changed.
</asp:Panel>

<asp:Panel ID="pnl_change_password" runat="server">
<table>
    <tr>
        <td><asp:Label ID="lbl_new_password" runat="server" Text="New Password:" AssociatedControlID="txt_new_password" /></td>
        <td>
            <asp:TextBox ID="txt_new_password" runat="server" TextMode="Password" MaxLength="50"  />
            <asp:RequiredFieldValidator ID="reqval_newPassword" runat="server" ErrorMessage="Password is required." Display="Dynamic" ControlToValidate="txt_new_password"  CssClass="errorMessage" />
            <asp:RegularExpressionValidator ID="regval_newPassword" runat="server" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$" Display="Dynamic" CssClass="errorMessage" 
                ErrorMessage="Password must be at least 8 characters in length, one uppercase letter, one lowercase letter, one number, and one special characters." ControlToValidate="txt_new_password" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lbl_confirm_password" runat="server" Text="Confirm New Password:" AssociatedControlID="txt_confirm_password" /></td>
        <td>
            <asp:TextBox ID="txt_confirm_password" runat="server" TextMode="Password" MaxLength="50"  />
            <asp:RequiredFieldValidator ID="reqval_NewPasswordConfirm" runat="server" ErrorMessage="Confirm password is required." Display="Dynamic" ControlToValidate="txt_confirm_password"  CssClass="errorMessage" />
            <asp:RegularExpressionValidator ID="regval_NewPasswordConfirm" runat="server" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$" Display="Dynamic" CssClass="errorMessage"
                ErrorMessage="Password must be at least 8 characters in length, one uppercase letter, one lowercase letter, one number, and one special characters." ControlToValidate="txt_confirm_password" />
            <asp:CompareValidator ID="cmpval_password" runat="server" ErrorMessage="Password must be the same." CssClass="errorMessage" Display="Dynamic" ControlToCompare="txt_new_password" ControlToValidate="txt_confirm_password" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button id="btn_change_password" runat="server" OnClick="btn_changePassword_OnClick" Text="Change Password" />
        </td>
    </tr>
</table>

</asp:Panel>

</asp:Content>
