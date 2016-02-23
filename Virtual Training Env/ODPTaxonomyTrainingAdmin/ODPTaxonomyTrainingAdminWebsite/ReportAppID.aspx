<%@ Page Language="C#"  MasterPageFile="~/Site.Master"AutoEventWireup="true" CodeBehind="ReportAppID.aspx.cs" Inherits="ODPTaxonomyTrainingAdminWebsite.ReportAppID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- <h2><asp:Literal ID="ltl_page_title" runat="server" Text="Value Report by Application ID"  /></h2>-->
              <h2 style="color:black;">Value Report by Application ID</h2>

<asp:HyperLink ID="hl_return_home" runat="server" Text="Return to AnswerkeyUpdate Page" CssClass="button" NavigateUrl="~/AnswerkeyUpdate.aspx" />

 <asp:GridView ID="gvw_list" class="bordered zebra-striped clearbothf" runat="server" PagerStyle-CssClass="pagination" PageSize="25"  HeaderStyle-CssClass="persist-header" >

<HeaderStyle CssClass="persist-header"></HeaderStyle>

<PagerStyle CssClass="pagination"></PagerStyle>
    </asp:GridView>

</asp:Content>

