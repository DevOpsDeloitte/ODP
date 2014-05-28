<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="Evaluation.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.Evaluation" %>

<%@ Register Src="~/Evaluation/Controls/EvaluationControl.ascx" TagPrefix="uc1" TagName="EvaluationControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Evaluation Form</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:EvaluationControl runat="server" ID="EvaluationControl" />
    <!--<p><asp:Label runat="server" CssClass="regularMessage" ID="lbl_messageUsers" Visible="false"></asp:Label></p>-->
</asp:Content>
