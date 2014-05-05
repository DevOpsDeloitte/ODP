<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageAccounts.aspx.cs" Inherits="ODPTaxonomyWebsite.AccountManagement.ManageAccounts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Manage Accounts
    </h2>

    <asp:Label ID="lbl_error_message" CssClass="errorMessage" runat="server" Visible="false" />

    <asp:LinkButton ID="lnkbtn_create_account" runat="server" Text="Create Account" OnClick="lnkbtn_createAccount_OnClick" />

    <asp:GridView ID="gvw_users" runat="server" AutoGenerateColumns="false"
        AllowPaging="true" PageSize="25" OnPageIndexChanging="gvw_users_OnPageIndexChanging"
        AllowSorting="true" OnSorting="gvw_users_OnSorting" 
        OnRowEditing="gvw_users_OnRowEditing" >
        <Columns>
            <asp:BoundField HeaderText="User ID" DataField="UserName" SortExpression="UserName" />
            <asp:BoundField HeaderText="First Name" DataField="UserFirstName" SortExpression="UserFirstName" />
            <asp:BoundField HeaderText="Last Name" DataField="UserLastName" SortExpression="UserLastName" />
            <asp:BoundField HeaderText="Email" DataField="Email"/>
            <asp:TemplateField HeaderText="Active">
                <ItemTemplate><%# (Boolean.Parse(Eval("IsApproved").ToString())) ? "Yes" : "No" %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Roles" >
                <ItemTemplate><%# GetUserRoles(Eval("UserName").ToString()) %></ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField EditText="Edit" ShowEditButton="true" HeaderText="Edit" />
        </Columns>
    </asp:GridView>
</asp:Content>
