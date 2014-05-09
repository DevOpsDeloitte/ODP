<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoderSupervisorView_Open.ascx.cs"
    Inherits="ODPTaxonomyWebsite.Evaluation.AbstractListViews.CoderSupervisorView_Open" %>
<%@ Register TagPrefix="odp" Namespace="ODPTaxonomyWebsite.Evaluation.AbstractListViews"
    Assembly="ODPTaxonomyWebsite" %>
<h2>
    View Open Abstracts</h2>
<odp:AbstractGridView runat="server" ID="AbstractViewGridView" CssClass="AbstractViewTable"
    OnRowDataBound="AbstractListRowBindingHandle">
    <Columns>
        <asp:BoundField DataField="ApplicationID" HeaderText="APPL ID" SortExpression="ApplicationID" />
        <asp:BoundField DataField="StatusDate" HeaderText="Status Date" SortExpression="Date" />
        <asp:BoundField DataField="ProjectTitle" HeaderText="Title" SortExpression="Title" />
        <asp:TemplateField HeaderText="A1"></asp:TemplateField>
        <asp:TemplateField HeaderText="A2"></asp:TemplateField>
        <asp:TemplateField HeaderText="A3"></asp:TemplateField>
        <asp:TemplateField HeaderText="B"></asp:TemplateField>
        <asp:TemplateField HeaderText="C"></asp:TemplateField>
        <asp:TemplateField HeaderText="D"></asp:TemplateField>
        <asp:TemplateField HeaderText="E"></asp:TemplateField>
        <asp:TemplateField HeaderText="F"></asp:TemplateField>
        <asp:TemplateField HeaderText="G"></asp:TemplateField>
    </Columns>
</odp:AbstractGridView> 