<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ODPTaxonomyWebsite.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="sixteen columns sub-title"> 
            <span class="title center">LOGIN</span>
            <span class="subtitle center">Please enter your user name and password.</span>
    </div>
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false" DisplayRememberMe="false" PasswordRecoveryText="Forgot Password" >
        <LayoutTemplate>
            <span class="errorMessage">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="errorMessage" 
                 ValidationGroup="LoginUserValidationGroup"/>
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Account Information</legend>
                    
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                        <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" Display="Dynamic" 
                             CssClass="errorMessage" ErrorMessage="User ID is required." ToolTip="User Name is required." 
                             ValidationGroup="LoginUserValidationGroup"/>
                    
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                        <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"  Display="Dynamic"
                             CssClass="errorMessage" ErrorMessage="Password is required." ToolTip="Password is required." 
                             ValidationGroup="LoginUserValidationGroup"/>

                        <asp:Button ID="LoginButton" runat="server" class="button" CommandName="Login" Text="Log In" 
                        ValidationGroup="LoginUserValidationGroup" onclick="LoginButton_Click"/>

                        <asp:HyperLink ID="hl_forgot_password" runat="server" class="forgot-password" Text="Forgot your password?" NavigateUrl="ForgotPassword.aspx" />
                    
                </fieldset>
                
                
            </div>
        </LayoutTemplate>
    </asp:Login>
</asp:Content>
