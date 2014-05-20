<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True"
    CodeBehind="Default.aspx.cs" Inherits="ODPTaxonomyWebsite._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<div class="sixteen columns sub-title"> 
    <span class="title">HOMEPAGE</span>
            

    <asp:Label runat="server" CssClass="errorMessage" ID="lbl_Error" Visible="false"></asp:Label>
    <asp:Label runat="server" CssClass="regularMessage" ID="lbl_messageUsers" Visible="false"></asp:Label>   
    
    <asp:Panel runat="server" ID="pnl_admin" Visible="false">
        <span class="subtitle">Role: Admin</span>
        <div class="center">
            <asp:Button class="button" runat="server" ID="btn_manageUserAccounts_admin" 
                    Text="Manage User Accounts" onclick="btn_manageUserAccounts_admin_Click" />
            <asp:Button class="button" runat="server" ID="btn_viewAbstractList_admin" 
                    Text="View Abstract List" onclick="btn_viewAbstractList_admin_Click" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_odpSup" Visible="false">
        <span class="subtitle">Role: ODP Supervisor</span>
        <div class="center">
            <asp:Button class="button" runat="server" ID="btn_manageTeams_odpSup" Text="Manage Teams" 
                    onclick="btn_manageTeams_odpSup_Click" />
            <asp:Button class="button" runat="server" ID="btn_viewAbstractList_odpSup" 
                    Text="View Abstract List" onclick="btn_viewAbstractList_odpSup_Click" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_odp" Visible="false">
        <span class="subtitle">Role: ODP Staff Member</span>
        <div class="center">
            <asp:Button class="button" runat="server" ID="btn_viewAbstractList_odp" 
                    Text="View Abstract List" onclick="btn_viewAbstractList_odp_Click" />
        </div>        
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_coderSup" Visible="false">
        <span class="subtitle">Role: Coder Supervisor</span>
        <div class="center">
            <asp:Button class="button" runat="server" ID="btn_manageTeams_coderSup" Text="Manage Teams" 
                    onclick="btn_manageTeams_coderSup_Click" />
            <asp:Button class="button" runat="server" ID="btn_viewAbstractList_coderSup" 
                    Text="View Abstract List" onclick="btn_viewAbstractList_coderSup_Click" />
            <asp:Button class="button" runat="server" ID="btn_viewAbstract_coderSup" 
                    Text="View/Code Abstract" onclick="btn_viewAbstract_coderSup_Click" />
        </div>        
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_coder" Visible="false">
        <span class="subtitle">Role: Coder</span>
        <div class="center">
        <asp:Button class="button" runat="server" Text="View/Code Abstract" 
            ID="btn_viewAbstract_coder" onclick="btn_viewAbstract_coder_Click" />
        <asp:Label runat="server" CssClass="regularMessage" ID="lbl_messCoder" Visible="false"></asp:Label>
        </div>
    </asp:Panel>
    </div>
        </div>
</asp:Content>
