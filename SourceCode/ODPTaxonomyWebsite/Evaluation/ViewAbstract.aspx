<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewAbstract.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.ViewAbstract" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        View Abstract
    </h2>
    <p>
        <asp:Button runat="server" ID="btn_print" Text="Print Abstract" />&nbsp;&nbsp;
        <asp:Button runat="server" ID="btn_code" Text="Code Abstract" />&nbsp;&nbsp;
    </p>
</asp:Content>
