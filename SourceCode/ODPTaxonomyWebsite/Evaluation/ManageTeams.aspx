<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageTeams.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.ManageTeams" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Manage Teams
    </h2>
    <p>
    <asp:Label runat="server" CssClass="errorMessage" ID="lbl_Error"></asp:Label>
    <asp:Label runat="server" CssClass="regularMessage" ID="lbl_messageUsers"></asp:Label>
    </p>
    <asp:HiddenField runat="server" ID="hf_teamTypeId" />
    <asp:Repeater runat="server" ID="rpt_users">
        <HeaderTemplate>
            <table class="teamUsers">
                <tr>
                    <th>User Name</th>
                    <th>Select</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# DataBinder.Eval(Container.DataItem, "UserFirstName") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "UserLastName") %></td>
                <td><asp:HiddenField runat="server" ID="hf_userID" Value='<%# DataBinder.Eval(Container.DataItem, "UserId")%>' />
                <asp:CheckBox runat="server" ID='checkbox' Checked="false" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <p>
    <asp:Button runat="server" ID="btn_saveteam" Text="Save Team" 
        onclick="btn_saveteam_Click" /></p>
        <p></p>
        <p></p>
        <asp:Repeater runat="server" ID="rpt_teams" OnItemDataBound="rp_teams_OnItemDataBound">
        <HeaderTemplate>
            <table class="teamUsers">
                <tr>
                    <th>Team</th>
                    <th>Action</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>Placeholder: <%# DataBinder.Eval(Container.DataItem, "TeamID")%></td>
                <td><asp:HiddenField runat="server" ID="hf_teamID" Value='<%# DataBinder.Eval(Container.DataItem, "TeamID")%>' />
                <asp:Button runat="server" ID="btn_deleteTeam" Text="Delete Team" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
