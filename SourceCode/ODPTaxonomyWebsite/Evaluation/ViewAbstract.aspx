<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewAbstract.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.ViewAbstract" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        View Abstract
    </h2>
    <p><asp:Label runat="server" CssClass="regularMessage" ID="lbl_messageUsers" Visible="false"></asp:Label></p>
    <asp:HiddenField runat="server" ID="hf_abstractId" />
    <asp:HiddenField runat="server" ID="hf_evaluationId_coder" />
    <asp:HiddenField runat="server" ID="hf_evaluationId_odp" />
    <asp:HiddenField runat="server" ID="hf_currentRole" />
    <asp:HiddenField runat="server" ID="hf_userId" />
    <asp:HiddenField runat="server" ID="hf_evaluationId" />
    <asp:HiddenField runat="server" ID="hf_submissionTypeId" />
    <asp:Panel runat="server" ID="pnl_printBtns" ClientIDMode="Static" Visible="false">
    <script type="text/javascript">
        window.absid = <%= absid %>;
    </script>
        <asp:Button runat="server" ID="btn_print" Text="Print Abstract" Visible="true" 
             OnClientClick=" window.open('./PrintAbstract.aspx?id='+absid, '_blank', 'toolbar=yes, scrollbars=yes, resizable=yes, top=500, left=500, width=1200, height=700'); return false;"  />&nbsp;&nbsp;
        <asp:Button runat="server" ID="btn_code" Text="Code Abstract" 
            onclick="btn_code_Click" Visible="false" />&nbsp;&nbsp;
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_overrideBtns" ClientIDMode="Static" Visible="false">
        <asp:Button runat="server" ID="btn_override" Text="Abstract Override" Visible="true" 
            onclick="btn_override_Click" 
            OnClientClick="return confirm('You are about to force close this abstract. Would you like to proceed?');" /><br /><br />
        
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnl_odpValues" ClientIDMode="Static" Visible="false">
    <asp:LinkButton runat="server" ID="link_odpCompare" CssClass="viewSubmissionLink"  onclick="link_Submission_Click"
         CommandArgument='' Visible="false">ODP vs. Coder Compare</asp:LinkButton>
        <asp:LinkButton runat="server" ID="link_odpConsensus" CssClass="viewSubmissionLink"  onclick="link_Submission_Click"
         CommandArgument='' Visible="false">ODP Consensus</asp:LinkButton>
        <asp:LinkButton runat="server" ID="link_odpNotes" CssClass="viewSubmissionLink" Visible="false"  
            onclick="link_Notes_Click" CommandArgument=''>ODP Notes(PDF)</asp:LinkButton>
        <asp:Repeater runat="server" ID="rpt_odpSubmissions">
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="link_Submission"  CommandArgument='<%# GetValue(DataBinder.Eval(Container.DataItem, "EvaluationId"), DataBinder.Eval(Container.DataItem, "SubmissionTypeId"), DataBinder.Eval(Container.DataItem, "UserId"))%>' 
                 CssClass="viewSubmissionLink"  onclick="link_Submission_Click">ID: <%# DataBinder.Eval(Container.DataItem, "UserName")%> Selections</asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnl_coderValues" ClientIDMode="Static" Visible="false">        
        <asp:LinkButton runat="server" ID="link_coderConsensus" CssClass="viewSubmissionLink" onclick="link_Submission_Click" 
            CommandArgument='' Visible="false">Code Consensus</asp:LinkButton>
        <asp:LinkButton runat="server" ID="link_coderNotes" CssClass="viewSubmissionLink" Visible="false"  
            onclick="link_Notes_Click" CommandArgument=''>Coder Notes(PDF)</asp:LinkButton>
        <asp:Repeater runat="server" ID="rpt_coderSubmissions">
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="link_Submission"  CommandArgument='<%# GetValue(DataBinder.Eval(Container.DataItem, "EvaluationId"), DataBinder.Eval(Container.DataItem, "SubmissionTypeId"), DataBinder.Eval(Container.DataItem, "UserId"))%>' 
                 CssClass="viewSubmissionLink"  onclick="link_Submission_Click">ID: <%# DataBinder.Eval(Container.DataItem, "UserName")%> Selections</asp:LinkButton>                 
            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnl_uploadNotes" ClientIDMode="Static" Visible="false">
        <asp:Button runat="server" ID="btn_notes" Text="Upload Notes" Visible="true" 
            onclick="btn_notes_Click" /><br />
        <asp:FileUpload runat="server" ID="fu_notes" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_extraData" ClientIDMode="Static" Visible="false">
        <p>User ID: <span runat="server" id="userId"></span></p>
        <p>Date: <span runat="server" id="date"></span></p>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_abstract" ClientIDMode="Static" Visible="false">
     <p>ProjectTitle: <span runat="server" id="ProjectTitle"></span></p>
        <p>AdministeringIC: <span runat="server" id="AdministeringIC"></span></p>
        <p>ApplicationID: <span runat="server" id="ApplicationID"></span></p>
        <p>PIProjectLeader: <span runat="server" id="PIProjectLeader"></span></p>
        <p>FY: <span runat="server" id="FY"></span></p>
        <p>ProjectNumber: <span runat="server" id="ProjectNumber"></span></p>
        <p>AbstractDescPart: <span runat="server" id="AbstractDescPart"></span></p>
        <p>AbstractPublicHeathPart: <span runat="server" id="AbstractPublicHeathPart"></span></p>
         

    </asp:Panel>
</asp:Content>
