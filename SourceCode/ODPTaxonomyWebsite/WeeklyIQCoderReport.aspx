<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeeklyIQCoderReport.aspx.cs" Inherits="ODPTaxonomyWebsite.WeeklyIQCoderReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Weekly IQ Coder Report</h1>
        <p>Report Parameters
            <ul>
                <li>StartDate=mmddyyyy</li>
                <li>EndDate=mmddyyyy</li>
            </ul>
            Ex: Get data for 11/5/2015 to 11/10/2015:<br /> WeeklyIQCoderReport.aspx?StartDate=11052015&EndDate=11102015
        </p>

        <asp:Label ID="lbl_errmsg" style="color:red" runat="server" />
    </div>
    </form>
</body>
</html>
