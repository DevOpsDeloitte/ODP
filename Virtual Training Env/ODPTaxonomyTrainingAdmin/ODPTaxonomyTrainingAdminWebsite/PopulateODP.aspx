<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PopulateODP.aspx.cs" Inherits="ODPTaxonomyTrainingAdminWebsite.PopulateODP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2><asp:Literal ID="ltl_page_title" runat="server" Text="ODP Population" /></h2>

<asp:HyperLink ID="hl_return_home" runat="server" Text="Return to Home Page" CssClass="button" NavigateUrl="~/Default.aspx" />

<asp:Label runat="server" CssClass="errorMessage" ID="lbl_Error" class="panel" Visible="false"></asp:Label>
<asp:Label runat="server" CssClass="regularMessage" ID="lbl_message" class="panel" Visible="false"></asp:Label>   

<asp:GridView ID="gvw_list" class="bordered zebra-striped clearbothf" runat="server" AutoGenerateColumns="false" PagerStyle-CssClass="pagination"
    AllowPaging="true" PageSize="25" Visible="false" HeaderStyle-CssClass="persist-header" >
    <Columns>
        <asp:BoundField HeaderText="AbstractID" DataField="AbstractID" Visible="true"/>
        <asp:BoundField HeaderText="Application ID" DataField="ApplicationID" SortExpression="ApplicationID" HeaderStyle-Width="100px"/>
        <asp:BoundField HeaderText="PI Name" DataField="PIProjectLeader" SortExpression="PIProjectLeader"/>
        <asp:BoundField HeaderText="Title" DataField="ProjectTitle" SortExpression="ProjectTitle"/>
    </Columns>
</asp:GridView>

</asp:Content>
