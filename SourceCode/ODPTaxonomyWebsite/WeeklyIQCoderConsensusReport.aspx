<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeeklyIQCoderConsensusReport.aspx.cs" Inherits="ODPTaxonomyWebsite.WeeklyIQCoderConsensusReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Weekly IQ Coder Consensus Report</h1>
        <p>Report Parameters
            <ul>
                <li>StartDate=mmddyyyy</li>
                <li>EndDate=mmddyyyy</li>
            </ul>
            Ex: Get data for 7/1/2016 to 7/15/2016:<br /> WeeklyIQCoderConsensusReport.aspx?StartDate=07012016&EndDate=07152016
        </p>

        <asp:Label ID="lbl_errmsg" style="color:red" runat="server" />
    </div>
    </form>
</body>
</html>
