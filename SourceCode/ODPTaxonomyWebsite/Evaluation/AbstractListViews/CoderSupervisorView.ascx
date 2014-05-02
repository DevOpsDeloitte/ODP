<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoderSupervisorView.ascx.cs"
    Inherits="ODPTaxonomyWebsite.Evaluation.AbstractListViews.CoderSupervisorView" %>
<h2>
    View Coded Abstracts List</h2>
<asp:GridView runat="server" ID="AbstractView" AutoGenerateColumns="false" GridLines="None"
    CssClass="AbstractViewTable" OnRowDataBound="AbstractListRowBindingHandle">
    <Columns>
        <asp:BoundField DataField="AbstractID" HeaderText="ID" />
        <asp:BoundField DataField="ApplicationID" HeaderText="APPL ID" />
        <asp:BoundField DataField="StatusDate" HeaderText="Status Date" />
        <asp:TemplateField HeaderText="Title">
            <ItemTemplate>
                <span>
                    <%#Eval("ProjectTitle") %></span>
                <asp:HyperLink runat="server" ID="AbstractScanLink" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Comment" HeaderText="Comment" />
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
</asp:GridView>
