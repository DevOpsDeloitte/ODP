<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewAbstractList.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.ViewAbstractList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        View Abstract List
    </h2>
    <p><asp:Label runat="server" CssClass="regularMessage" ID="lbl_messageUsers" Visible="true"></asp:Label></p>
    <p>
        <asp:Button runat="server" ID="btn_print" Text="Upload Coder Notes (Scanned)" />&nbsp;&nbsp;
        Stop Evaluation Process: <asp:Button runat="server" ID="btn_code" Text="Abstract Override" />&nbsp;&nbsp;
    </p>
</asp:Content>
