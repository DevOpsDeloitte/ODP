<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="ODPTaxonomyWebsite.AccountManagement.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2>
    Change Password</h2>
<asp:Label ID="lbl_error_message" runat="server" Visible="false" CssClass="errorMessage" />

<asp:Panel ID="pnl_confirmation" runat="server" Visible="false" class="Message">
    Your password is successfully changed.
</asp:Panel>

<asp:Panel ID="pnl_change_password" runat="server">

<div class="eight columns view-panel"> 
    <strong>Password must be:</strong>
    <ol>
        <li>Minimum of 8 characters.</li>
        <li>Must contain at least 1 uppercase letter, 1 lowercase letter, 1 special character, and 1 number.</li>
    </ol>
</div>

<div><asp:ValidationSummary ID="valsum_errors" runat="server" CssClass="errorMessage inline" HeaderText="<strong>Please fix error(s) below:</strong>" /></div>

<table class="form">
    <tr>
        <td><asp:Label ID="lbl_new_password" runat="server" Text="*Password:" AssociatedControlID="txt_new_password" /></td>
        <td>
            <asp:TextBox ID="txt_new_password" runat="server" TextMode="Password" MaxLength="50"  />
            <asp:RequiredFieldValidator ID="reqval_newPassword" runat="server" ErrorMessage="Password is required." Display="Dynamic" ControlToValidate="txt_new_password"  CssClass="errorMessage" />
            <asp:RegularExpressionValidator ID="regval_newPassword" runat="server" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$" Display="Dynamic" CssClass="errorMessage" 
                ErrorMessage="Password does not meet requirements, please enter a new password." ControlToValidate="txt_new_password" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lbl_confirm_password" runat="server" Text="*Confirm Password:" AssociatedControlID="txt_confirm_password" /></td>
        <td>
            <asp:TextBox ID="txt_confirm_password" runat="server" TextMode="Password" MaxLength="50"  />
            <asp:RequiredFieldValidator ID="reqval_NewPasswordConfirm" runat="server" ErrorMessage="Confirm password is required." Display="Dynamic" ControlToValidate="txt_confirm_password"  CssClass="errorMessage" />
            <asp:RegularExpressionValidator ID="regval_NewPasswordConfirm" runat="server" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$" Display="Dynamic" CssClass="errorMessage"
                ErrorMessage="Password does not meet requirements, please enter a new password." ControlToValidate="txt_confirm_password" />
            <asp:CompareValidator ID="cmpval_password" runat="server" ErrorMessage="Passwords do not match, please reenter the password." CssClass="errorMessage" Display="Dynamic" ControlToCompare="txt_new_password" ControlToValidate="txt_confirm_password" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button class="button yes" id="btn_change_password" runat="server" OnClick="btn_changePassword_OnClick" Text="Update" />
        </td>
        <td>
            <asp:Button class="button no" id="btn_cancel" runat="server" OnClick="btn_cancel_OnClick" Text="Cancel" CausesValidation="false" />
        </td>
    </tr>
</table>

</asp:Panel>

</asp:Content>
