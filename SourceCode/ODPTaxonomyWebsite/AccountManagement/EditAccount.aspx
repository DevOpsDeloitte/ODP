<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditAccount.aspx.cs" Inherits="ODPTaxonomyWebsite.AccountManagement.EditAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<script language="javascript" type="text/javascript">

    function EnableDisableRole(rolegroup) {
        // get checkboxes
        var objAdmin = document.getElementById("<%=cbx_Admin.ClientID %>");
        var objODPSuper = document.getElementById("<%=cbx_ODPStaffSupervisor.ClientID %>");
        var objODPStaff = document.getElementById("<%= cbx_ODPStaffMember.ClientID%>");
        var objCoderSuper = document.getElementById("<%=cbx_CoderSupervisor.ClientID %>");
        var objCoder = document.getElementById("<%=cbx_Coder.ClientID %>");

        // get checked state
        var isAdmin = objAdmin.checked;
        var isODPSuper = objODPSuper.checked;
        var isODPStaff = objODPStaff.checked;
        var isCoderSuper = objCoderSuper.checked;
        var isCoder = objCoder.checked;

        // if admin, ODPStaffSupervisor, ODPStaffMember is checked, disable coder
        if (rolegroup == "admin") {
            // check state of other item in the group
            if (isAdmin || isODPSuper || isODPStaff) {
                objCoderSuper.checked = false;
                objCoderSuper.disabled = true;

                objCoder.checked = false;
                objCoder.disabled = true;
            }
            else {
                objCoderSuper.disabled = false;
                objCoder.disabled = false;
            }
        }
        else {
            if (isCoder || isCoderSuper) {
                objAdmin.checked = false;
                objAdmin.disabled = true;

                objODPSuper.checked = false;
                objODPSuper.disabled = true;

                objODPStaff.checked = false;
                objODPStaff.disabled = true;
            }
            else {
                objAdmin.disabled = false;
                objODPSuper.disabled = false;
                objODPStaff.disabled = false;
            }
        }
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2><asp:Literal ID="ltl_page_title" runat="server" Text="Edit Account" /></h2>


<asp:Label ID="lbl_confirmation_message" runat="server" Visible = "false" Text="User account successfully saved." />

<asp:Label ID="lbl_error_message" CssClass="errorMessage" runat="server" Visible="false" />

<asp:Panel ID="pnl_account" runat="server">

<asp:ValidationSummary ID="valsum_errors" runat="server" CssClass="errorMessage" HeaderText="Data entry error(s) occurred.  Please fix error(s) below." />

<table class="form">
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
        <td valign="top">
            <asp:Label ID="lbl_roles" runat="server" Text="*Role(s):" />
        </td>
        <td>
            <asp:CheckBox ID="cbx_Admin" runat="server" Text="Admin" onclick="EnableDisableRole('admin')" /><br />
            <asp:CheckBox ID="cbx_ODPStaffSupervisor" runat="server" Text="ODP Supervisor" onclick="EnableDisableRole('admin')"  /><br />
            <asp:CheckBox ID="cbx_ODPStaffMember" runat="server" Text="ODP Staff" onclick="EnableDisableRole('admin')"  /><br />
            <asp:CheckBox ID="cbx_CoderSupervisor" runat="server" Text="Coder Supervisor" onclick="EnableDisableRole('coder')"  /><br />
            <asp:CheckBox ID="cbx_Coder" runat="server" Text="Coder" onclick="EnableDisableRole('coder')" /><br />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            Password must be:
            <ul>
                <li>Minimum of 8 characters.</li>
                <li>Must contain at least 1 uppercase letter, 1 lowercase letter, 1 special character, and 1 number.</li>
            </ul>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lbl_new_password" runat="server" Text="*Password:" AssociatedControlID="txt_new_password" /></td>
        <td>
            <asp:TextBox ID="txt_new_password" runat="server" TextMode="Password" MaxLength="50"  />
            <asp:RequiredFieldValidator ID="reqval_newPassword" runat="server" ErrorMessage="Password is required." Display="Dynamic" ControlToValidate="txt_new_password"  CssClass="errorMessage" Enabled="false" />
            <asp:RegularExpressionValidator ID="regval_newPassword" runat="server" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$" Display="Dynamic" CssClass="errorMessage" 
                ErrorMessage="Password does not meet requirements, please enter a new password." ControlToValidate="txt_new_password" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lbl_confirm_password" runat="server" Text="*Confirm Password:" AssociatedControlID="txt_confirm_password" /></td>
        <td>
            <asp:TextBox ID="txt_confirm_password" runat="server" TextMode="Password" MaxLength="50"  />
            <asp:RequiredFieldValidator ID="reqval_NewPasswordConfirm" runat="server" ErrorMessage="Confirm password is required." Display="Dynamic" ControlToValidate="txt_confirm_password"  CssClass="errorMessage" Enabled="false" />
            <asp:RegularExpressionValidator ID="regval_NewPasswordConfirm" runat="server" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$" Display="Dynamic" CssClass="errorMessage"
                ErrorMessage="Password does not meet requirements, please enter a new password." ControlToValidate="txt_confirm_password" />
            <asp:CompareValidator ID="cmpval_password" runat="server" ErrorMessage="Passwords do not match, please reenter the password." CssClass="errorMessage" Display="Dynamic" ControlToCompare="txt_new_password" ControlToValidate="txt_confirm_password" />
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
        <td class="form-button">
            <asp:Button class="button" ID="btn_save" runat="server" OnClick="btn_save_OnClick" Text="Save" />
        </td>
        <td>
            <asp:Button class="button" ID="btn_cancel" runat="server" OnClick="btn_cancel_OnClick" Text="Cancel" CausesValidation="false" />
        </td class="form-button">
    </tr>
</table>
</asp:Panel>


</asp:Content>


