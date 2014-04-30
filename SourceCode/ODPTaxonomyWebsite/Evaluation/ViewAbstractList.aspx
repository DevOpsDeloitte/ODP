<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewAbstractList.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.ViewAbstractList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        View Abstract List as 
        <asp:DropDownList runat="server" ID="ViewDDL" 
        OnSelectedIndexChanged="ViewDDLIndexChangedHandle"
        AutoPostBack="true" />
    </h2>
</asp:Content>
