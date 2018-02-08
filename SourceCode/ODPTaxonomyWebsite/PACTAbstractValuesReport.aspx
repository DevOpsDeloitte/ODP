<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PACTAbstractValuesReport.aspx.cs" Inherits="ODPTaxonomyWebsite.PACTAbstractValuesReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
 <div>
        <h1>PACT Abstract Values Report</h1>
        <p>Report Parameters
            <ul>
                <li>FY=yyyy</li>
            </ul>
            Ex: Get data for 2017:<br /> PACTAbstractValuesReport.aspx?FY=2017
        </p>

        <asp:Label ID="lbl_errmsg" style="color:red" runat="server" />
    </div>
    </form>
</body>
</html>
