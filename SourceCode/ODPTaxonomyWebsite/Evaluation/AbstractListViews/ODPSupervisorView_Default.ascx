﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ODPSupervisorView_Default.ascx.cs"
    Inherits="ODPTaxonomyWebsite.Evaluation.AbstractListViews.ODPSupervisorView_Default" %>
<%@ Register TagPrefix="odp" Namespace="ODPTaxonomyDAL_JY" Assembly="ODPTaxonomyDAL_JY" %>
<h3>
    View Coded Abstracts</h3>
     <div class="showingCounts">
      <span  class="showing">Page&nbsp;:&nbsp;<%=  (AbstractViewGridView.PageIndex + 1) %></span><!-- of <span class="showing"><%=AbstractViewGridView.PageCount %></span>--><br />
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
<asp:Button runat="server" class="review button no" Text="Add to Review List" OnClick="AddtoReviewHandler" OnClientClick="return checkStatus();" />
<odp:AbstractGridView runat="server" ID="AbstractViewGridView" AutoGenerateColumns="false"
    GridLines="None" CssClass="AbstractViewTable bordered zebra-striped">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HiddenField runat="server" ID="AbstractID" Value='<%#Eval("AbstractID") %>' />
                <asp:CheckBox runat="server" CssClass="reviewcheck" ID="ToReview" />
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
<asp:Button class="review button no" runat="server" Text="Add to Review List" OnClick="AddtoReviewHandler" OnClientClick="return checkStatus();" />