<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="ODPTaxonomyTrainingAdminWebsite.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="Scripts/jquery.js"></script>
    <script src="Scripts/jquery-ui.min.js"></script>
    <link href="Scripts/jquery-ui.min.css" rel="stylesheet" />
   <script src="Scripts/datePicker.js" type="text/javascript"></script> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table >
        <tr>
             <td>
                <asp:Label ID="lbl_frm_date" runat="server" Text="From Date:" />          
                 <br />
                 <asp:TextBox ID="txt_frm_date" runat="server"/>
            </td>

            <td>
                 <asp:Label ID="lbl_to_date" runat="server" Text="To Date:" /><br />
                 <asp:TextBox ID="txt_to_date" runat="server"/>
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            </td>
        </tr>
   </table>
    </div>
    </form>
</body>
</html>
