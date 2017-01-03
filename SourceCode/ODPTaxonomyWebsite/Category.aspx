<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="ODPTaxonomyWebsite.Category" %>


<asp:content id="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="../styles/alertify.css"> 
  <script src="../scripts/alertify.js"></script>
     <script type="text/javascript">

         function alertify_confirm() {
               window.alertify.set({
                 labels: {
                     ok: "Confirm Change",
                     cancel: "Reject Change"
                 },
                 delay: 5000,
                 buttonReverse: true,
                 buttonFocus: "none"
             });

               alertify.confirm("Category Change Confirmation?", function (e) {
                   if (e) {
                       //console.log(clickElem);
                       document.getElementById('<%=btn_catMethod.ClientID%>').click();
                   } else {
                       alertify.error("You've clicked Reject Change!");
                       return false;
                   }
             });

             return false;
         }
 </script>
</asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


              <h2>
               Category Abstract Selection
            </h2>
        <asp:Panel runat="server" ID="pnl_content">
        
          <div class="twelve columns alpha" style="overflow-x:auto;">
               <Table style="border:none;box-shadow:none;" ID="Tbl" runat="server" >
                    <tr >
                    <td > 
                   <asp:DropDownList ID="ddlCategory" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCategory_Change"></asp:DropDownList>
              </td>
                    <td style="border:none;" > 
                         <p>
                        <asp:Label runat="server" style="color: red; font-size:24px;" ID="lbl_message"></asp:Label>
                        </p>
                    <div style="color: #000;font-size:18px;"><strong>Currently Coding: <asp:Label runat="server"  ID="lbl_category"></asp:Label></strong></div><br />
                    <div style="color: #000;">Before submitting and changing to a new category, please make sure all users taht will be impacted have logged out of the system.</div><br />
                    <div class="col-lg-2 columns six">
                    <asp:Button runat="server" ID="btn_submit" Height="34px" Width="90px" Text="Submit"  OnClientClick="return alertify_confirm()"/> 
                     <div style="display: none;">
                       <asp:Button ID="btn_catMethod" runat="server" OnClick="switchCategoryMethod"/><%-- hiding method on btn clicking (javascript)--%>
                    </div>
                     <div style="float:right !important;"><asp:Button runat="server" ID="btn_reset" Height="34px"  Width="90px" Text="Reset" OnClick="btn_reset_Click" /></div>
                    
     
                </div></td>
                   </tr>
               </Table>
             
               
                </div>
 </asp:Panel>
</asp:content>
