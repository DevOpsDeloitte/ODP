﻿<%@ Page Title="Abstract View List" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="True" CodeBehind="ViewAbstractList.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.ViewAbstractList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        View Abstract List
        <asp:DropDownList runat="server" ID="MainviewDDL" AutoPostBack="true" />
    </h2>
    <div>
        <asp:Panel runat="server" ID="SubviewPanel" Visible="false">
            <h3>
                <asp:Label runat="server" ID="SubviewLabel" Visible="false" />
                <asp:DropDownList runat="server" ID="SubviewDDL" AutoPostBack="true" Visible="false" />
            </h3>
        </asp:Panel>
        <div class="pager-size-wrapper">
            <asp:Label runat="server" AssociatedControlID="PagerSizeDDL">Show:</asp:Label>
            <asp:DropDownList runat="server" ID="PagerSizeDDL" AutoPostBack="true" OnSelectedIndexChanged="PagerSizeChangeHandler">
                <asp:ListItem Value="25" Text="25 Results" />
                <asp:ListItem Value="50" Text="50 Results" />
                <asp:ListItem Value="100" Text="100 Results" />
            </asp:DropDownList>
        </div>
        <asp:PlaceHolder runat="server" ID="AbstractViewPlaceHolder" />
    </div>
</asp:Content>
