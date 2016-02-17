<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="WebForm1.aspx.cs" Inherits="ODPTaxonomyTrainingAdminWebsite.WebForm1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        </asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <div>
    <table >
        <tr>
             <td style="width: 172px">
                <asp:Label ID="lbl_frm_date" runat="server" Text="From Date:" />          
                 <br />        

                 <asp:TextBox ID="txt_frm_date" runat="server"  />
            </td>

            <td style="width: 227px">
                 <asp:Label ID="lbl_to_date" runat="server" Text="To Date:" /><br />
                 <asp:TextBox ID="txt_to_date" runat="server" CssClass="date"/>
  
                 <br />             

            </td>
            <td>
                <asp:Button ID="Button3" runat="server" Text="Button" OnClick="Button3_Click" />
            </td>
        </tr>
   </table>
    </div>

    </asp:Content>