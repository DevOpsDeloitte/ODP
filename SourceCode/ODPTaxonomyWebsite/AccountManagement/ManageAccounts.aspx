<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageAccounts.aspx.cs" Inherits="ODPTaxonomyWebsite.AccountManagement.ManageAccounts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="sixteen columns sub-title"> 
            <h2>MANAGE ACCOUNTS</h2>
    </div>

    <asp:Label ID="lbl_error_message" CssClass="errorMessage" runat="server" Visible="false" />

    <asp:LinkButton class="button right" ID="lnkbtn_create_account" runat="server" Text="Create New Account" OnClick="lnkbtn_createAccount_OnClick" />

    <asp:GridView ID="gvw_users" class="bordered zebra-striped clearbothf" runat="server" AutoGenerateColumns="false" PagerStyle-CssClass="pagination"
        AllowPaging="true" PageSize="25" OnPageIndexChanging="gvw_users_OnPageIndexChanging"
        AllowSorting="true" OnSorting="gvw_users_OnSorting" 
        OnRowEditing="gvw_users_OnRowEditing" HeaderStyle-CssClass="persist-header" >
        <Columns>
            <asp:BoundField HeaderText="User ID" DataField="UserName" SortExpression="UserName" />
            <asp:TemplateField HeaderText="First Name" SortExpression="UserFirstName">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkbtn_editUser_fname" runat="server" CommandArgument='<%#Eval("UserName") %>' Text='<%#Eval("UserFirstName") %>' OnClick="lnkbtn_editUser_OnClick" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Name" SortExpression="UserLastName">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkbtn_editUser_lname" runat="server" CommandArgument='<%#Eval("UserName") %>' Text='<%#Eval("UserLastName") %>' OnClick="lnkbtn_editUser_OnClick" />
                </ItemTemplate>
            </asp:TemplateField>
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
