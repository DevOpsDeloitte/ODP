<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ODPTaxonomyWebsite._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Homepage
    </h2>
    <p><asp:Label runat="server" CssClass="errorMessage" ID="lbl_Error" Visible="false"></asp:Label></p>
    <p><asp:Label runat="server" CssClass="regularMessage" ID="lbl_messageUsers" Visible="false"></asp:Label></p>

    <asp:Panel runat="server" ID="pnl_coder" Visible="false">
        <h3>Role: Coder</h3>
        <p><asp:Button runat="server" Text="View/Code Abstract" 
            ID="btn_viewAbstract_coder" onclick="btn_viewAbstract_coder_Click" /></p>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_coderSup" Visible="false">
        <h3>Role: Coder Supervisor</h3>
        <p><asp:Button runat="server" ID="btn_manageTeams_coderSup" Text="Manage Teams" 
                onclick="btn_manageTeams_coderSup_Click" /></p>
        <p><asp:Button runat="server" ID="btn_viewAbstractList_coderSup" 
                Text="View Abstract List" onclick="btn_viewAbstractList_coderSup_Click" /></p>
        <p><asp:Button runat="server" ID="btn_viewAbstract_coderSup" 
                Text="View/Code Abstract" onclick="btn_viewAbstract_coderSup_Click" /></p>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_odp" Visible="false">
        <h3>Role: ODP Staff Member</h3>
        <p><asp:Button runat="server" ID="btn_viewAbstractList_odp" 
                Text="View Abstract List" onclick="btn_viewAbstractList_odp_Click" /></p>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_odpSup" Visible="false">
        <h3>Role: ODP Supervisor</h3>
        <p><asp:Button runat="server" ID="btn_manageTeams_odpSup" Text="Manage Teams" 
                onclick="btn_manageTeams_odpSup_Click" /></p>
        <p><asp:Button runat="server" ID="btn_viewAbstractList_odpSup" 
                Text="View Abstract List" onclick="btn_viewAbstractList_odpSup_Click" /></p>
        <p><asp:Button runat="server" ID="btn_viewAbstract_odpSup" 
                Text="View/Code Abstract" onclick="btn_viewAbstract_odpSup_Click" /></p>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_admin" Visible="false">
        <h3>Role: Admin</h3>
        <p><asp:Button runat="server" ID="btn_manageUserAccounts_admin" 
                Text="Manage User Accounts" onclick="btn_manageUserAccounts_admin_Click" /></p>
        <p><asp:Button runat="server" ID="btn_viewAbstractList_admin" 
                Text="View Abstract List" onclick="btn_viewAbstractList_admin_Click" /></p>
    </asp:Panel>
</asp:Content>
