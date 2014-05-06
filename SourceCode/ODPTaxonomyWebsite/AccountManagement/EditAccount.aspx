<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditAccount.aspx.cs" Inherits="ODPTaxonomyWebsite.AccountManagement.EditAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<script language="javascript" type="text/javascript">

    function CheckRoleList(source, arguments) {
        arguments.IsValid = IsCheckBoxChecked() ? true : false;

    }

    function IsCheckBoxChecked() {
        var isChecked = false;
        var list = document.getElementById('<%= cbl_roles.ClientID %>');
        if (list != null) {
            for (var i = 0; i < list.rows.length; i++) {
                for (var j = 0; j < list.rows[i].cells.length; j++) {
                    var listControl = list.rows[i].cells[j].childNodes[0];
                    if (listControl.checked) {
                        isChecked = true;
                    }
                }
            }
        }
        return isChecked;

    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2><asp:Literal ID="ltl_page_title" runat="server" Text="Edit Account" /></h2>


<asp:Label ID="lbl_confirmation_message" runat="server" Visible = "false" Text="User account successfully saved." />

<asp:Label ID="lbl_error_message" CssClass="errorMessage" runat="server" Visible="false" />

<asp:Panel ID="pnl_account" runat="server">

<asp:ValidationSummary ID="valsum_errors" runat="server" CssClass="errorMessage" HeaderText="Data entry error(s) occurred.  Please fix error(s) below." />

<table>
    <asp:Panel ID="pnl_username" runat="server">
    <tr>
        <td>
            <asp:Label ID="lbl_username" runat="server" Text="User ID:" AssociatedControlID="txt_username" />
        </td>
        <td>
            <asp:TextBox ID="txt_username" runat="server" Enabled="false" />
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td>
            <asp:Label ID="lbl_first_name" runat="server" Text="*First Name:" AssociatedControlID="txt_firstname" />
        </td>
        <td>
            <asp:TextBox ID="txt_firstName" runat="server" />
            <asp:RequiredFieldValidator ID="reqval_fname" runat="server" Display="Dynamic" CssClass="errorMessage" ControlToValidate="txt_firstname" ErrorMessage="First Name is required." />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lbl_last_name" runat="server" Text="*Last Name:" AssociatedControlID="txt_lastname" />
        </td>
        <td>
            <asp:TextBox ID="txt_lastName" runat="server"  />
            <asp:RequiredFieldValidator ID="reqval_lname" runat="server" Display="Dynamic" CssClass="errorMessage" ControlToValidate="txt_lastname" ErrorMessage="Last Name is required." />
        </td>
    </tr>
     <tr>
        <td>
            <asp:Label ID="lbl_Email" runat="server" Text="*Email:" AssociatedControlID="txt_email" />
        </td>
        <td>
            <asp:TextBox ID="txt_email" runat="server" />
            <asp:RequiredFieldValidator ID="reqval_email" runat="server" Display="Dynamic" CssClass="errorMessage" ControlToValidate="txt_email" ErrorMessage="Email is required." />
            <asp:RegularExpressionValidator ID="regex_email" runat="server" Display="Dynamic" ErrorMessage="Email Address format is invalid." ControlToValidate="txt_email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="errorMessage" />
        </td>
    </tr>
    <tr>
        <td valign="top">
            <asp:Label ID="lbl_roles" runat="server" AssociatedControlID="cbl_roles" Text="*Role(s):" />
        </td>
        <td>
            <asp:CheckBoxList ID="cbl_roles" runat="server" DataTextField="RoleDesc" DataValueField="RoleName" />
            <asp:CustomValidator ID="cusval_roles" runat="server" ErrorMessage="Select at least one role." CssClass="errorMessage" Display="Dynamic" ClientValidationFunction="CheckRoleList" />
        </td>
    </tr>
        <tr>
        <td><asp:Label ID="lbl_new_password" runat="server" Text="*Password:" AssociatedControlID="txt_new_password" /></td>
        <td>
            <asp:TextBox ID="txt_new_password" runat="server" TextMode="Password" MaxLength="50"  />
            <asp:RequiredFieldValidator ID="reqval_newPassword" runat="server" ErrorMessage="Password is required." Display="Dynamic" ControlToValidate="txt_new_password"  CssClass="errorMessage" Enabled="false" />
            <asp:RegularExpressionValidator ID="regval_newPassword" runat="server" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$" Display="Dynamic" CssClass="errorMessage" 
                ErrorMessage="Password must be at least 8 characters in length, one uppercase letter, one lowercase letter, one number, and one special characters." ControlToValidate="txt_new_password" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lbl_confirm_password" runat="server" Text="*Confirm Password:" AssociatedControlID="txt_confirm_password" /></td>
        <td>
            <asp:TextBox ID="txt_confirm_password" runat="server" TextMode="Password" MaxLength="50"  />
            <asp:RequiredFieldValidator ID="reqval_NewPasswordConfirm" runat="server" ErrorMessage="Confirm password is required." Display="Dynamic" ControlToValidate="txt_confirm_password"  CssClass="errorMessage" Enabled="false" />
            <asp:RegularExpressionValidator ID="regval_NewPasswordConfirm" runat="server" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$" Display="Dynamic" CssClass="errorMessage"
                ErrorMessage="Password must be at least 8 characters in length, one uppercase letter, one lowercase letter, one number, and one special characters." ControlToValidate="txt_confirm_password" />
            <asp:CompareValidator ID="cmpval_password" runat="server" ErrorMessage="Password must be the same." CssClass="errorMessage" Display="Dynamic" ControlToCompare="txt_new_password" ControlToValidate="txt_confirm_password" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lbl_activeYN" runat="server" AssociatedControlID="rdl_activeYN" Text="User Status:" />
        </td>
        <td>
            <asp:RadioButtonList ID="rdl_activeYN" runat="server" RepeatDirection="Horizontal" >
                <asp:ListItem Value="1" Text="Activate" />
                <asp:ListItem Value="0" Text="Deactivate"/>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btn_save" runat="server" OnClick="btn_save_OnClick" Text="Save" />
        </td>
        <td>
            <asp:Button ID="btn_cancel" runat="server" OnClick="btn_cancel_OnClick" Text="Cancel" CausesValidation="false" />
        </td>
    </tr>
</table>
</asp:Panel>


</asp:Content>


