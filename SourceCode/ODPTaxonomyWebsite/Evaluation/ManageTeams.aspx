﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageTeams.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.ManageTeams" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <link rel="stylesheet" href="../styles/alertify.css"> 
  <script src="../scripts/alertify.js"></script>
  <style>
  input.novisibility
  {
      display: none;
  }
  </style>
  <script type="text/javascript">


      function alertify_confirm( x ) {
          var clickElem = x.substr(0, x.lastIndexOf("_")) + "2" + x.substr(x.lastIndexOf("_"));
          window.alertify.set({
              labels: {
                  ok: "OK",
                  cancel: "Cancel"
              },
              delay: 5000,
              buttonReverse: true,
              buttonFocus: "none"
          });

          alertify.confirm("Please confirm team delete?", function (e) {
              if (e) {
                  //console.log(clickElem);
                  $("#" + clickElem).click();
              } else {
                  alertify.error("You've clicked cancel Delete..");
                  return false;
              }
          });

          return false;
      }
  </script>
    <h2>
        Manage Teams
    </h2>
    <p>
    <asp:Label runat="server" CssClass="errorMessage" ID="lbl_Error"></asp:Label></p>
    <asp:Panel runat="server" ID="pnl_content">
    <p>
    <asp:Label runat="server" CssClass="regularMessage" ID="lbl_messageUsers"></asp:Label>
    </p>
    <div class="six columns alpha">
    <asp:HiddenField runat="server" ID="hf_teamTypeId" />
    <h3>Available Team Members</h3>
    <p runat="server" id="gc_noUsers" class="regularMessage" >No users are currently available for you to select for a new team.</p>
    <p></p>

    <asp:Button runat="server" ID="Button1" Text="Save Team" 
        onclick="btn_saveteam_Click" class="button yes" />

    <asp:Repeater runat="server" ID="rpt_users">
        <HeaderTemplate>
            <table class="bordered zebra-striped">
                <tr>
                    <th scope="col">User Name</th>
                    <th scope="col">Select</th>
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
   
    <asp:Button runat="server" ID="btn_saveteam" Text="Save Team" 
        onclick="btn_saveteam_Click" class="button yes" />
</div>
<div class="six columns omega">
        
        <h3>Active Teams</h3>
        <p runat="server" id="gc_noTeam" class="regularMessage">No Active Teams currently exist.</p>
        <p></p>
        <asp:Repeater runat="server" ID="rpt_teams" OnItemDataBound="rp_teams_OnItemDataBound">
        <HeaderTemplate>
            <table class="bordered zebra-striped">
                <tr>
                    <th scope="col">Team</th>
                    <th scope="col">Action</th>
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
                <asp:Button runat="server" ID="btn_deleteTeam"  class="button no" OnClick="DeleteTeam_Click" OnClientClick="return alertify_confirm($(this).attr('id'));" Text="Remove Team" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TeamID")%>' />
                <asp:Button runat="server" ID="btn_deleteTeam2"  class="novisibility" OnClick="DeleteTeam_Click" Text="Remove Team" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TeamID")%>' />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    </div>
    </asp:Panel>
</asp:Content>
