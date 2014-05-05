<%@ Page Title="Edit Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="ODPTaxonomyWebsite.AccountManagement.EditProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:Panel ID="pnl_confirmation" runat="server" Visible="false">
    Profile successfully saved.
</asp:Panel>

<asp:Label ID="lbl_error_message" CssClass="errorMessage" runat="server" Visible="false" />

<asp:Panel ID="pnl_edit_profile" runat="server">

<asp:ValidationSummary ID="valsum_errors" runat="server" CssClass="errorMessage" HeaderText="Data entry error occurred.  Please fix." />

<table>
    <tr>
        <td><asp:Label ID="lbl_fname" runat="server" AssociatedControlID="txt_fname" Text="First Name: " /></td>
        <td>
            <asp:TextBox ID="txt_fname" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="reqval_firstName" runat="server" Display="Dynamic" ErrorMessage="First Name is required." ControlToValidate="txt_fname" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lbl_lname" runat="server" AssociatedControlID="txt_lname" Text="Last Name: " /></td>
        <td>
            <asp:TextBox ID="txt_lname" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="reqval_lastName" runat="server" Display="Dynamic" ErrorMessage="Last Name is required." ControlToValidate="txt_lname" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lbl_email" runat="server" AssociatedControlID="txt_email" Text="Email Address: " /></td>
        <td>
            <asp:TextBox ID="txt_email" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="reqval_email" runat="server" Display="Dynamic" ErrorMessage="Email Address is required." ControlToValidate="txt_email" CssClass="errorMessage" />
            <asp:RegularExpressionValidator ID="regex_email" runat="server" Display="Dynamic" ErrorMessage="Email Address format is invalid." ControlToValidate="txt_email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="errorMessage" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btn_save_profile" runat="server" OnClick="btn_saveProfile_OnClick" CausesValidation="true" Text="Save" />
        </td>
        <td>
            <asp:Button ID="btn_cancel" runat="server" OnClick="btn_cancel_OnClick" CausesValidation="false" Text="Cancel" />
        </td>
    </tr>
</table>
</asp:Panel>




</asp:Content>
