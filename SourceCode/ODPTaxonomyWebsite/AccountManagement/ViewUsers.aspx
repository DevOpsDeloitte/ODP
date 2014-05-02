<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewUsers.aspx.cs" Inherits="ODPTaxonomyWebsite.AccountManagement.ViewUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        User List
    </h2>

    <asp:Label ID="lbl_error_message" CssClass="errorMessage" runat="server" Visible="false" />

    <asp:GridView ID="gvw_users" runat="server" AutoGenerateColumns="false" 
        AllowSorting="true" OnSorting="gvw_users_OnSorting" >
        <Columns>
            <asp:BoundField HeaderText="UserID" DataField="UserID" Visible="false" />
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
        </Columns>
    </asp:GridView>
</asp:Content>
