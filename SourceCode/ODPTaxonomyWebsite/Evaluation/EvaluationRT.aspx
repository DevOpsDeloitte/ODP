<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EvaluationRT.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.EvaluationRT" %>

<%@ Register Src="~/Evaluation/Controls/EvaluationControlRT.ascx" TagPrefix="uc1" TagName="EvaluationControlRT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Evaluation Form</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:EvaluationControlRT runat="server" ID="EvaluationControl" />
</asp:Content>
