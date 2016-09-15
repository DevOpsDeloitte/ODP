<%@ Page Title="AnswerkeyUpdate" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="AnswerkeyUpdate.aspx.cs" Inherits="ODPTaxonomyTrainingAdminWebsite.AnswerkeyUpdate" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
          function ConfirmUpdateValue(objMsg) {

            var valUPEnvProdTrain = $('#<%= rdoValUpEnv.ClientID %> input:checked').val()

            if (confirm(objMsg)) {
                if (valUPEnvProdTrain == "1") //production
                    document.getElementById('<%=btn_prodMethod.ClientID%>').click();
                else if (valUPEnvProdTrain == "2") //training
                    document.getElementById('<%=btn_trainMethod.ClientID%>').click();
            }
            else {
                window.location.href = "AnswerkeyUpdate.aspx";
            }
        }
        function ConfirmRerunKappa(objMsg) {

            if (confirm(objMsg)) {
                document.getElementById('<%=btn_rkMethod.ClientID%>').click();
            }
            else {
                window.location.href = "AnswerkeyUpdate.aspx";
            }
        }
 </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <%--------------------------Value Updates---------------------------------------%>
    <asp:Panel runat="server"  ID="pnl_training" cssClass="panelAnswerUpdate" >
           
          <h2 style="color:black;">Value Updates <span style="font-size: 16px;font-weight: normal;">*All Fields Required</span> </h2>

     <table class="form">
         <tr>
             <td colspan = "9" > 
                 <asp:RadioButtonList ID="rdoValUpEnv" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdoValUpEnv_SelectedIndexChanged">
                <asp:ListItem Value="1"  Selected ="True">Production</asp:ListItem>
                <asp:ListItem Value="2">Training</asp:ListItem>
                </asp:RadioButtonList>
            </td>
         </tr>
          <tr><td colspan = "9" >&nbsp;</td></tr>
        <tr>
             <td style="width: 160px">
                 <asp:Label ID="lbl_file_name" runat="server" Text="*File Name" /><br />
                 <asp:TextBox ID="txt_file_name" runat="server" TextMode="SingleLine" MaxLength="500" Width="150px" ToolTip="Filename is required"  />
                <asp:RequiredFieldValidator ID="FilenameRequired" runat="server" ControlToValidate="txt_file_name" ErrorMessage="(Required)" ForeColor="Red"  Display="Dynamic" ValidationGroup="ValueUpdates"></asp:RequiredFieldValidator>
            </td>
            <td style="width: 160px">
                <asp:Label ID="lbl_app_id" runat="server" Text="*Application ID" /><br />
                <asp:TextBox ID="txt_app_id" runat="server" ToolTip="Application ID is required" Width="150px" />
                <asp:RequiredFieldValidator ID="AppIDRequired" runat="server" ControlToValidate="txt_app_id" ErrorMessage="(Required)"  ForeColor="Red" Display="Dynamic" ValidationGroup="ValueUpdates"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="ValChkAppId" runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txt_app_id" ErrorMessage="Value must be a whole number" Display="Dynamic" ForeColor="Red" ValidationGroup="ValueUpdates"/>
            </td>
            <td runat="server"  id ="tdDdlCons" style="width: 160px">
                <asp:Label ID="lbl_consensus" runat="server" Text="*Consensus" /><br />
                <asp:DropDownList ID="ddl_consensus" runat="server" Width="150px" >
                     <asp:ListItem Value="0">Options</asp:ListItem>
                     <asp:ListItem Value ="RC">Reconciliation</asp:ListItem>
                     <asp:ListItem Value ="CC">Coder Consensus</asp:ListItem>
                 </asp:DropDownList>
               
                <asp:RequiredFieldValidator ID="ConsensusRequired" runat="server" ControlToValidate="ddl_consensus" ErrorMessage="(Required)"  ForeColor="Red" SetFocusOnError="true" Display="Dynamic" InitialValue="0"  ValidationGroup="ValueUpdates"></asp:RequiredFieldValidator>
                
            </td>

            <td style="width: 160px">
                <asp:Label ID="lbl_frm_section" runat="server" Text="*Form Section" /><br />
                <asp:DropDownList ID="ddl_frm_section" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddl_frm_section_SelectedIndexChanged">
                     <asp:ListItem Value="0">Options</asp:ListItem>
                     <asp:ListItem Value ="A1">A1</asp:ListItem>
                     <asp:ListItem Value ="A2">A2</asp:ListItem>
                     <asp:ListItem Value ="A3">A3</asp:ListItem>
                     <asp:ListItem Value ="B">B</asp:ListItem>
                     <asp:ListItem Value ="C">C</asp:ListItem>
                     <asp:ListItem Value ="D">D</asp:ListItem>
                     <asp:ListItem Value ="E">E</asp:ListItem>
                     <asp:ListItem Value ="F">F</asp:ListItem>
                     <asp:ListItem Value ="E7F6">E7F6</asp:ListItem>
                 </asp:DropDownList>
                <asp:RequiredFieldValidator ID="FrmSectionRequired" runat="server" ControlToValidate="ddl_frm_section" ErrorMessage="(Required)" ForeColor="Red" InitialValue="0"   SetFocusOnError="true" Display="Dynamic" ValidationGroup="ValueUpdates"></asp:RequiredFieldValidator>
              
            </td>
  
            <td runat="server"  id ="tdTxtUpdVal" style="width: 160px">
                <asp:Label ID="lbl_upd_values" runat="server" Text="*Update Values" /><br />
                <asp:TextBox ID="txt_upd_values" runat="server" TextMode="SingleLine" ToolTip="Update Values is required (Ex.1,4,5)" MaxLength="200" Width="150px"  />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_upd_values" ErrorMessage="(Required)"  ForeColor="Red" Display="Dynamic" ValidationGroup="ValueUpdates"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="ValChkUpdVal" runat="server"  ValidationExpression="^[0-9,]*$"   ControlToValidate="txt_upd_values"  ErrorMessage="Invalid!(Ex.1,4,5)" ForeColor="Red" Display="Dynamic" ValidationGroup="ValueUpdates"> </asp:RegularExpressionValidator>
            </td>

        </tr>

        <tr>
            <td colspan = "9" style="text-align: right"> 
                <asp:Button ID="btn_update" runat="server" Text="Update" Width="153px" Height="32px" ValidationGroup="ValueUpdates" OnClick="btn_update_Click" />
                <div style="display: none;">
                   <asp:Button ID="btn_prodMethod" runat="server" OnClick="updValProdMethod" /> <%-- hiding method on btn clicking (javascript)--%>
                    <asp:Button ID="btn_trainMethod" runat="server" OnClick="updValTrainMethod" /> <%-- hiding method on btn clicking (javascript) --%>
                </div>
            </td>
        </tr>
    </table>
 
     </asp:Panel>
 

    <%--------------------------Value Report ---------------------------------------%>
     <asp:Panel runat="server"  ID="Panel1" cssClass="panelAnswerUpdate" Style="margin-top: 20px;">
          <h2 style="color:black;">Value Report</h2>

    <table class="form">
        <tr>
            <td colspan ="3"><asp:RadioButtonList ID="rdoValRepEnv" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdoValRepEnv_SelectedIndexChanged">
                <asp:ListItem Value="1"  Selected ="True">Production</asp:ListItem>
                <asp:ListItem Value="2">Training</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>            
            <td>
                <asp:Label ID="lbl_report" runat="server" Text="*Select an option" /><br />          
                <asp:DropDownList ID="ddlReport" runat="server" style="text-align: left" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged">
                    <asp:ListItem Value="0">Select Report By</asp:ListItem>
                    <asp:ListItem Value="1">ApplicationID</asp:ListItem>
                    <asp:ListItem Value="2">Date Range</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="ddlReportRequired" runat="server" ControlToValidate="ddlReport" ErrorMessage="(Required)" ForeColor="Red" InitialValue="0"  Display="Dynamic" ValidationGroup="ValueReport"></asp:RequiredFieldValidator>
            </td>  
            
            <td runat="server"  id ="tdDdlCons1">
                <asp:Label ID="lbl_consensus1" runat="server" Text="*Consensus" /><br />
                <asp:DropDownList ID="ddl_consensus1" runat="server" Width="150px">
                     <asp:ListItem Value="0">Options</asp:ListItem>
                     <asp:ListItem Value ="RC">Reconciliation</asp:ListItem>
                     <asp:ListItem Value ="CC">Coder Consensus</asp:ListItem>
                 </asp:DropDownList>
                <asp:RequiredFieldValidator ID="ConsensusRequired1" runat="server" ControlToValidate="ddl_consensus1" ErrorMessage="(Required)"  ForeColor="Red" SetFocusOnError="true" Display="Dynamic" InitialValue="0"  ValidationGroup="ValueReport"></asp:RequiredFieldValidator>
            </td>
            <td>&nbsp;</td>
           
        </tr>
        <tr runat="server" id="trDate">
           <td>
                <asp:Label ID="lbl_frm_date" runat="server" Text="From Date:" /><br />
                <asp:TextBox ID="txt_frm_date" runat="server"  Width="100px" CssClass="date"/><br />
                <asp:CompareValidator ID="frmDateValidator" runat="server" Type="Date"  Operator="DataTypeCheck"  ControlToValidate="txt_frm_date"  ForeColor="Red"  ErrorMessage="Please enter a valid date." ValidationGroup="ValueReport" SetFocusOnError="True"></asp:CompareValidator>
            </td> 
            <td>
                 <asp:Label ID="lbl_to_date" runat="server" Text="To Date:" /><br />
                 <asp:TextBox ID="txt_to_date" runat="server"  Width="100px"  CssClass="date"/><br />
                 <asp:CompareValidator ID="toDateValidator" runat="server" Type="Date"  Operator="DataTypeCheck"  ControlToValidate="txt_to_date"  ForeColor="Red"  ErrorMessage="Please enter a valid date." ValidationGroup="ValueReport" SetFocusOnError="True"></asp:CompareValidator>
            </td>      
            <td>&nbsp;</td>
        </tr>  
         <tr runat="server" id="trAppID">
            <td colspan="3">
                <asp:Label ID="lbl_app_id1" runat="server" Text="Application ID" /><br />
                <asp:TextBox ID="txt_app_id1" runat="server" TextMode="SingleLine" Width="150px" /><br />
                <asp:CompareValidator ID="ValChkAppId1" runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txt_app_id1" ErrorMessage="Value must be a whole number" Display="Dynamic" ForeColor="Red" ValidationGroup="ValueReport"/>
            </td>
       </tr>
        <tr>
        <td colspan = "3" style="text-align: right" >
            <asp:Button ID="btn_chk_val" runat="server" Text="Check Values" Height="32px" OnClick="btn_chk_val_Click"  ValidationGroup="ValueReport" Width="153px" />
        </td>
  
    </tr>
    </table>
          
    </asp:Panel>

    <%--------------------------Rerun KAPPA---------------------------------------%>
     <asp:Panel runat="server"  ID="Panel2" cssClass="panelAnswerUpdate" Style="margin-top: 20px;">
        <h2 style="color:black;">Rerun KAPPA <span style="font-size: 16px;font-weight: normal;">Production Only;</span> <span style="font-size: 13px;font-weight: normal;"> Kappa runs every 15 mins - check prod site</span> </h2>
    <table class="form">
        <tr>   
            <td>
                <asp:Label ID="lbl_consensus2" runat="server" Text="*Consensus" /><br />
                <asp:DropDownList ID="ddl_consensus2" runat="server" Width="150px" >
                     <asp:ListItem Value="0">Options</asp:ListItem>
                     <asp:ListItem Value ="RC">Reconciliation</asp:ListItem>
                     <asp:ListItem Value ="CC">Coder Consensus</asp:ListItem>
                 </asp:DropDownList>
               
                <asp:RequiredFieldValidator ID="ConsensusRequired2" runat="server" ControlToValidate="ddl_consensus2" ErrorMessage="(Required)"  ForeColor="Red" SetFocusOnError="true" Display="Dynamic" InitialValue="0"  ValidationGroup="RerunKappa"></asp:RequiredFieldValidator>
                
            </td>
            <td style="width: 236px">
                 <asp:Label ID="lbl_app_id2" runat="server" Text="*Application ID" /><br />
                <asp:TextBox ID="txt_app_id2" runat="server" TextMode="SingleLine" Width="150px" />
                <br />
                <asp:RequiredFieldValidator ID="AppIDRequired2" runat="server" ControlToValidate="txt_app_id2" ErrorMessage="(Required)"  ForeColor="Red" Display="Dynamic" ValidationGroup="RerunKappa" SetFocusOnError="True"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="ValChkAppId2" runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txt_app_id2" ErrorMessage="Value must be a whole number" Display="Dynamic" ForeColor="Red" ValidationGroup="RerunKappa" SetFocusOnError="True"/>
            </td>  
       
            <td style="vertical-align: middle">
                 <asp:Button ID="btn_rerun_kappa" runat="server" Text="Rerun Kappa" Width="153px" Height="32px"  OnClick="btn_rerun_kappa_Click" ValidationGroup="RerunKappa"/>
                <div style="display: none;">
                   <asp:Button ID="btn_rkMethod" runat="server" OnClick="rerunKappaMethod" /><%-- hiding method on btn clicking (javascript)--%>
                </div>
            </td>
        </tr>
    </table>
    </asp:Panel>

</asp:Content>