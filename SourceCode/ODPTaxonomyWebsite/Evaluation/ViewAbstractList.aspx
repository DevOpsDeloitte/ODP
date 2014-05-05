<%@ Page Title="Abstract View List" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="ViewAbstractList.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.ViewAbstractList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        View Abstract List
        <asp:DropDownList runat="server" ID="ViewDDL" AutoPostBack="true" />
    </h2>
    <div>
        <h3>
            <asp:Label runat="server" ID="SubviewLabel" Visible="false" />
            <asp:DropDownList runat="server" ID="SubviewDDL" AutoPostBack="true" Visible="false" />
        </h3>
        <asp:PlaceHolder runat="server" ID="AbstractViewPlaceHolder" />
    </div>
</asp:Content>
