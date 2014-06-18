<%@ Page Title="Edit Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="ODPTaxonomyWebsite.AccountManagement.EditProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2>
    Edit Profile</h2>
<asp:Button ID="btn_change_password" runat="server" CausesValidation="false" OnClick="btn_changePassword_OnClick" class="button" Text="Change Password" />

<asp:Panel ID="pnl_confirmation" runat="server" Visible="false" CssClass="Message">
    Profile successfully saved.
</asp:Panel>

<asp:Label ID="lbl_error_message" CssClass="errorMessage" runat="server" Visible="false" />

<asp:Panel ID="pnl_edit_profile" runat="server">

<asp:ValidationSummary ID="valsum_errors" runat="server" CssClass="errorMessage" HeaderText="<strong>Please fix error(s) below:</strong>" />

<table class="form">
    <tr>
        <td><asp:Label ID="lbl_fname" runat="server" AssociatedControlID="txt_fname" Text="*First Name: " /></td>
        <td>
            <asp:TextBox ID="txt_fname" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="reqval_firstName" runat="server" Display="Dynamic" ErrorMessage="First Name is required." ControlToValidate="txt_fname" CssClass="errorMessage" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lbl_lname" runat="server" AssociatedControlID="txt_lname" Text="*Last Name: " /></td>
        <td>
            <asp:TextBox ID="txt_lname" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="reqval_lastName" runat="server" Display="Dynamic" ErrorMessage="Last Name is required." ControlToValidate="txt_lname" CssClass="errorMessage" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lbl_email" runat="server" AssociatedControlID="txt_email" Text="*Email Address: " /></td>
        <td>
            <asp:TextBox ID="txt_email" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="reqval_email" runat="server" Display="Dynamic" ErrorMessage="Email Address is required." ControlToValidate="txt_email" CssClass="errorMessage" />
            <asp:RegularExpressionValidator ID="regex_email" runat="server" Display="Dynamic" ErrorMessage="Email Address format is invalid." ControlToValidate="txt_email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="errorMessage" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lbl_confirm_email" runat="server" Text="*Confirm Email:" AssociatedControlID="txt_confirm_email" />
        </td>
        <td>
            <asp:TextBox ID="txt_confirm_email" runat="server" />
            <asp:RequiredFieldValidator ID="reqval_confirm_email" runat="server" Display="Dynamic" CssClass="errorMessage" ControlToValidate="txt_confirm_email" ErrorMessage="Confirm Email is required." Enabled="false" />
            <asp:RegularExpressionValidator ID="regex_confirm_email" runat="server" Display="Dynamic" ErrorMessage="Confirm Email Address format is invalid." ControlToValidate="txt_confirm_email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="errorMessage" />
            <asp:CompareValidator ID="cmpval_email" runat="server" ErrorMessage="Emails do not match, please reenter the email." CssClass="errorMessage" Display="Dynamic" ControlToCompare="txt_email" ControlToValidate="txt_confirm_email" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btn_save_profile" runat="server" OnClick="btn_saveProfile_OnClick" CausesValidation="true" class="button yes" Text="Save" />
        </td>
        <td>
            <asp:Button ID="btn_cancel" runat="server" OnClick="btn_cancel_OnClick" CausesValidation="false" class="button no" Text="Cancel" />
        </td>
    </tr>
</table>
</asp:Panel>




</asp:Content>
