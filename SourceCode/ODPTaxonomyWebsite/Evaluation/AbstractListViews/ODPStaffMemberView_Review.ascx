<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ODPStaffMemberView_Review.ascx.cs"
    Inherits="ODPTaxonomyWebsite.Evaluation.AbstractListViews.ODPStaffMemberView_Review" %>
<%@ Register TagPrefix="odp" Namespace="ODPTaxonomyDAL_JY" Assembly="ODPTaxonomyDAL_JY" %>
<h3>
    View Abstracts in Review</h3>
  <div class="showingCounts">
  <span  class="showing">Page&nbsp;:&nbsp;<%=  (AbstractViewGridView.PageIndex + 1) %></span><br />
            Showing : 
            <%  var totalCount = 0;
                var showing = 0;
                var displayCounts = false;
                try
                {
                    totalCount = ((ICollection<AbstractListRow>)AbstractViewGridView.DataSource).Where(x => x.ApplicationID > 0).Select(x => x).Count();
                    //showing = (AbstractViewGridView.PageIndex + 1) * AbstractViewGridView.Rows.Count;
                    var RowSshowing = AbstractViewGridView.Rows.Cast<GridViewRow>().Where(x => x.Cells[2].Text.ToString().Replace("&nbsp;","").Trim().Length > 3);
                    showing = RowSshowing.Count() ;
                    if ((AbstractViewGridView.PageIndex + 1) == AbstractViewGridView.PageCount)
                    {
                        //showing = totalCount;
                    }
                    displayCounts = true;
                }
                catch (Exception ex)
                {
                    

                }
            
            %>
            <% if (displayCounts)
               { %>
            <span class="showing"><%= showing%></span> of <span class="showing"><%= totalCount%></span> Abstracts.
            <% } %>
            </div>
<odp:AbstractGridView runat="server" ID="AbstractViewGridView" AutoGenerateColumns="false"
    GridLines="None" CssClass="AbstractViewTable bordered zebra-striped">
    <Columns>
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
             <asp:BoundField DataField="Flags" HeaderText="Flags" ItemStyle-CssClass="hidestaffcols" />
        <asp:BoundField HeaderText="A1" DataField="A1" ItemStyle-CssClass="hidestaffcols"></asp:BoundField>
        <asp:BoundField HeaderText="A2" DataField="A2" ItemStyle-CssClass="hidestaffcols"></asp:BoundField>
        <asp:BoundField HeaderText="A3" DataField="A3" ItemStyle-CssClass="hidestaffcols"></asp:BoundField>
        <asp:BoundField HeaderText="B" DataField="B" ItemStyle-CssClass="hidestaffcols"></asp:BoundField>
        <asp:BoundField HeaderText="C" DataField="C" ItemStyle-CssClass="hidestaffcols"></asp:BoundField>
        <asp:BoundField HeaderText="D" DataField="D" ItemStyle-CssClass="hidestaffcols"></asp:BoundField>
        <asp:BoundField HeaderText="E" DataField="E" ItemStyle-CssClass="hidestaffcols"></asp:BoundField>
        <asp:BoundField HeaderText="F" DataField="F" ItemStyle-CssClass="hidestaffcols"></asp:BoundField>
        <asp:BoundField HeaderText="G" DataField="G" ItemStyle-CssClass="hidestaffcols"></asp:BoundField>
    </Columns>
    <PagerStyle CssClass="PagerContainer" />
</odp:AbstractGridView>