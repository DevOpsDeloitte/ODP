<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ODPSupervisorView_Review_Uncoded.ascx.cs"
    Inherits="ODPTaxonomyWebsite.Evaluation.AbstractListViews.ODPSupervisorView_Review_Uncoded" %>
<%@ Register TagPrefix="odp" Namespace="ODPTaxonomyDAL_JY" Assembly="ODPTaxonomyDAL_JY" %>
<h3>
    View Abstracts in Review</h3>
      <div class="showingCounts">
            Showing : 
            <%  var totalCount = ((ICollection<AbstractListRow>)AbstractViewGridView.DataSource).Count;
                var showing = (AbstractViewGridView.PageIndex+1) * AbstractViewGridView.Rows.Count;
                if ((AbstractViewGridView.PageIndex + 1) == AbstractViewGridView.PageCount)
                {
                    showing = totalCount;
                }
            
            %>

            <span class="showing"><%= showing %></span> of <span class="showing"><%= totalCount %></span>
            </div>
<asp:Button runat="server" class="button" Text="Remove from Review" OnClick="RemoveFromReviewHandler" />
<odp:AbstractGridView runat="server" ID="AbstractViewGridView" AutoGenerateColumns="false"
    GridLines="None" CssClass="AbstractViewTable bordered zebra-striped">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HiddenField runat="server" ID="AbstractID" Value='<%#Eval("AbstractID") %>' />
                <asp:CheckBox runat="server" ID="Review" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="AbstractID" HeaderText="ID" />
        <asp:BoundField DataField="ApplicationID" HeaderText="APPL ID" SortExpression="ApplicationID" />
        <asp:BoundField DataField="StatusDateDisplay" HeaderText="Status Date" SortExpression="Date" />
        <asp:TemplateField HeaderText="Title" SortExpression="Title">
            <ItemTemplate>
                <asp:Panel runat="server" ID="TitleWrapper" CssClass="title-wrapper">
                    <asp:HyperLink runat="server" ID="AbstractTitleLink" Visible="false" />
                    <asp:Label runat="server" ID="AbstractTitleText" Visible="false" />
                    <asp:Image ID="AbstractScanClip" runat="server" ImageUrl="~/Images/clip.png" AlternateText="Attachment"
                        CssClass="scan-file" Visible="false" />
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Flags" HeaderText="Flags" />
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
    <PagerStyle CssClass="PagerContainer" />
</odp:AbstractGridView>
<asp:Button runat="server" class="button" Text="Remove from Review" OnClick="RemoveFromReviewHandler" />
