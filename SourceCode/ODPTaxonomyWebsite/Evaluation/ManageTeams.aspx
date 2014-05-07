<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageTeams.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.ManageTeams" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Manage Teams
    </h2>
    <p>
    <asp:Label runat="server" CssClass="errorMessage" ID="lbl_Error"></asp:Label></p>
    <asp:Panel runat="server" ID="pnl_content">
    <p>
    <asp:Label runat="server" CssClass="regularMessage" ID="lbl_messageUsers"></asp:Label>
    </p>
    <asp:HiddenField runat="server" ID="hf_teamTypeId" />
    <h3>Users available for team to pick up</h3>
    <p runat="server" id="gc_noUsers">No users are currently available for you to select for a new team.</p>
    <p></p>
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
                <asp:HiddenField runat="server" ID="hf_userInitials" Value='<%# GetUserInitials(DataBinder.Eval(Container.DataItem, "UserFirstName"), DataBinder.Eval(Container.DataItem, "UserLastName"))%>' />
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
        <h3>Active Teams</h3>
        <p runat="server" id="gc_noTeam">No Active Teams currently exist.</p>
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
                <td><!--<%# DataBinder.Eval(Container.DataItem, "TeamCode")%>:<br />-->
                    <asp:Repeater runat="server" ID="rpt_teamMembers">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "UserFirstName")%>&nbsp;<%# DataBinder.Eval(Container.DataItem, "UserLastName")%><br />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td><asp:HiddenField runat="server" ID="hf_teamID" Value='<%# DataBinder.Eval(Container.DataItem, "TeamID")%>' />
                <asp:Button runat="server" ID="btn_deleteTeam" OnClick="DeleteTeam_Click" OnClientClick="return confirm('Are you sure you would like to Remove Team?');" Text="Remove Team" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TeamID")%>' />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    </asp:Panel>
</asp:Content>
