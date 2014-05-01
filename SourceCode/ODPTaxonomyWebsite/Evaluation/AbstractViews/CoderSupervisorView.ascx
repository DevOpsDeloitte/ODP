<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoderSupervisorView.ascx.cs"
    Inherits="ODPTaxonomyWebsite.Evaluation.AbstractViews.CoderSupervisorView" %>
<asp:GridView runat="server" ID="AbstractView" AutoGenerateColumns="false" GridLines="None"
    CssClass="AbstractViewTable">
    <Columns>
        <asp:BoundField DataField="ApplicationID" HeaderText="APPL ID" />
        <asp:BoundField DataField="StatusDate" HeaderText="Status Date" />
        <asp:BoundField DataField="ProjectTitle" HeaderText="Title" />
    </Columns>
</asp:GridView>
