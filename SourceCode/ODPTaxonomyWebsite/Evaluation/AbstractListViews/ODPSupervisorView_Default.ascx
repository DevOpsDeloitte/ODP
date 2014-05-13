﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ODPSupervisorView_Default.ascx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.AbstractListViews.ODPSupervisorView_Default" %>
<%@ Register TagPrefix="odp" Namespace="ODPTaxonomyWebsite.Evaluation.AbstractListViews"
    Assembly="ODPTaxonomyWebsite" %>
<h2>
    View Coded Abstracts</h2>
<odp:AbstractGridView runat="server" ID="AbstractViewGridView" AutoGenerateColumns="false"
    GridLines="None" CssClass="AbstractViewTable" OnRowDataBound="AbstractListRowBindingHandle">
    <Columns>
        <asp:BoundField DataField="ApplicationID" HeaderText="APPL ID" />
        <asp:BoundField DataField="StatusDate" HeaderText="Status Date" />
        <asp:TemplateField HeaderText="Title">
            <ItemTemplate>
                <asp:Panel runat="server" ID="TitleWrapper" CssClass="title-wrapper">
                    <a href='ViewAbstract.aspx?AbstractID=<%#Eval("AbstractID") %>'><span>
                        <%#Eval("ProjectTitle") %></span></a>
                    <asp:HyperLink runat="server" ID="AbstractScanLink" CssClass="scan-file">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/clip.png" AlternateText="Attachment" />
                    </asp:HyperLink>
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Comment" HeaderText="Comment" />
        <asp:BoundField HeaderText="A1" DataField="A1"></asp:BoundField>
        <asp:BoundField HeaderText="A2" DataField="A2"></asp:BoundField>
        <asp:BoundField HeaderText="A3" DataField="A3"></asp:BoundField>
        <asp:BoundField HeaderText="B" DataField="B"></asp:BoundField>
        <asp:BoundField HeaderText="C" DataField="C"></asp:BoundField>
        <asp:BoundField HeaderText="D" DataField="D"></asp:BoundField>
        <asp:BoundField HeaderText="E" DataField="E"></asp:BoundField>
        <asp:BoundField HeaderText="F" DataField="F"></asp:BoundField>
        <asp:BoundField HeaderText="G" DataField="G"></asp:BoundField>
    </Columns>
</odp:AbstractGridView>