<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ODPStaffMemberView_Review_Uncoded.ascx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.AbstractListViews.ODPStaffMemberView_Review_Uncoded" %>
<%@ Register TagPrefix="odp" Namespace="ODPTaxonomyWebsite.Evaluation.AbstractListViews"
    Assembly="ODPTaxonomyWebsite" %>
<h2>
    View Abstracts in Review</h2>
<odp:AbstractGridView runat="server" ID="AbstractViewGridView" AutoGenerateColumns="false"
    GridLines="None" CssClass="AbstractViewTable" OnRowDataBound="AbstractListRowBindingHandle">
    <Columns>
        <asp:BoundField DataField="ApplicationID" HeaderText="APPL ID" SortExpression="ApplicationID" />
        <asp:BoundField DataField="StatusDate" HeaderText="Status Date" SortExpression="Date" />
        <asp:TemplateField HeaderText="Title" SortExpression="Title">
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