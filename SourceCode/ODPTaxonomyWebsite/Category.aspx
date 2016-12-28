﻿<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="ODPTaxonomyWebsite.Category" %>


<asp:content id="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script type="text/javascript">
         function ConfirmCategory(objMsg) {

             if (confirm(objMsg)) {
                 document.getElementById('<%=btn_catMethod.ClientID%>').click();
            }
            else {
                window.location.href = "Category.aspx";
            }
        }
 </script>
</asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


              <h2>
               Category Abstract Selection
            </h2>
        <asp:Panel runat="server" ID="pnl_content">
           <div class="twelve columns alpha">
               <Table style="border:none;box-shadow:none;" ID="Tbl" runat="server">
                    <tr >
                    <td > 
                   <asp:DropDownList ID="ddlCategory" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCategory_Change"></asp:DropDownList>
              </td>
                    <td style="border:none;" > 
                         <p>
                        <asp:Label runat="server" style="color: red; font-size:24px;" ID="lbl_message"></asp:Label>
                        </p>
                    <div style="color: #000;font-size:18px;"><strong>Currently Coding: <asp:Label runat="server"  ID="lbl_category"></asp:Label></strong></div>
                    <div style="color: #000;">Before submitting and changing to a new category, please make sure all users taht will be impacted have logged out of the system.</div><br />
                    <div class="col-lg-2 columns four">
                    <asp:Button runat="server" ID="btn_submit" Height="34px" Width="90px" Text="Submit"  OnClick="btn_submit_Click" />
                     <div style="display: none;">
                       <asp:Button ID="btn_catMethod" runat="server" OnClick="switchCategoryMethod"/><%-- hiding method on btn clicking (javascript)--%>
                    </div>
                    <asp:Button runat="server" ID="btn_reset" Height="34px"  Width="90px" Text="Reset" OnClick="btn_reset_Click" />
                    
     
                </div></td>
                   </tr>
               </Table>
             
               
                </div>
 </asp:Panel>
</asp:content>
