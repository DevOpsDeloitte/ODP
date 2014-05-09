<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="ODPTaxonomyWebsite.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false" DisplayRememberMe="false" PasswordRecoveryText="Forgot Password" >
        <LayoutTemplate>
            
            
                <fieldset class="login">
                    <legend>Account Information</legend>
                    <span class="errorMessage">
                        <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                    </span>
                        <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="errorMessage" 
                        ValidationGroup="LoginUserValidationGroup"/>
                    
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName"></asp:Label>
                        <asp:TextBox ID="UserName" runat="server" CssClass="textEntry" placeholder="User Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" Display="Dynamic" 
                             CssClass="errorMessage" ErrorMessage="User ID is required." ToolTip="User Name is required." 
                             ValidationGroup="LoginUserValidationGroup"/>
                    
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password"></asp:Label>
                        <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password" placeholder="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"  Display="Dynamic"
                             CssClass="errorMessage" ErrorMessage="Password is required." ToolTip="Password is required." 
                             ValidationGroup="LoginUserValidationGroup"/>

                
                        <asp:Button ID="LoginButton" runat="server" class="button" CommandName="Login" Text="Log In" 
                        ValidationGroup="LoginUserValidationGroup" onclick="LoginButton_Click"/>

                        <asp:HyperLink ID="hl_forgot_password" runat="server" class="forgot-password" Text="Forgot your password?" NavigateUrl="ForgotPassword.aspx" />

                    
                </fieldset>


        </LayoutTemplate>
    </asp:Login>
</asp:Content>