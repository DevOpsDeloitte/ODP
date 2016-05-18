<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EvaluationRTB.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.EvaluationRTB" %>

<%@ Register Src="~/Evaluation/Controls/EvaluationControlRTB.ascx" TagPrefix="uc1" TagName="EvaluationControlRTB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Evaluation Form</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:EvaluationControlRTB runat="server" ID="EvaluationControl" />
</asp:Content>
