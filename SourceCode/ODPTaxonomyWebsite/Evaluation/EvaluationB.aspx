<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EvaluationB.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.EvaluationB" %>

<%@ Register Src="~/Evaluation/Controls/EvaluationControlB.ascx" TagPrefix="uc1" TagName="EvaluationControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Evaluation Form</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:EvaluationControl runat="server" ID="EvaluationControl" />





</asp:Content>
