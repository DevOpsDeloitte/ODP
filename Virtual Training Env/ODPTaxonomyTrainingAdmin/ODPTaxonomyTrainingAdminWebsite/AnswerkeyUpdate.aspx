<%@ Page Title="AnswerkeyUpdate" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="AnswerkeyUpdate.aspx.cs" Inherits="ODPTaxonomyTrainingAdminWebsite.AnswerkeyUpdate" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <!-- ++++++++++++++++DatePicker+++++++++++++++++++++++++++-------- -->

    <script type="text/javascript">
        // DOM Ready      
        $(function () {
            //Turn off datepicker if the textbox has been disabled.
            if (document.getElementById('<%= txt_frm_date.ClientID %>').disabled == true) {
                $("[id$=txt_frm_date]").datepicker('disable');
            }
            else{
                $("[id$=txt_frm_date]").datepicker('enable');
                $("[id$=txt_frm_date]").datepicker({
                    showOn: 'both',
                    buttonImage: "Images/Calendar.png",
                    buttonImageOnly: true,
                    buttonText: "Select date",
                    dateFormat: 'mm/dd/yy',
                    onClose: function () {
                        $(this).focus();
                    }
                });
            }
            if (document.getElementById('<%= txt_to_date.ClientID %>').disabled == true) {
                $("[id$=txt_to_date]").datepicker('disable');
            }
            else {
                $("[id$=txt_to_date]").datepicker('enable');
                $("[id$=txt_to_date]").datepicker({
                    showOn: 'both',
                    buttonImage: "Images/Calendar.png",
                    buttonImageOnly: true,
                    buttonText: "Select date",
                    dateFormat: 'mm/dd/yy',
                    onClose: function () {
                        $(this).focus();
                    }
                });
            }
           
         /*   $(".date").datepicker({
                showOn: 'both',
                buttonImage: "Images/Calendar.png",
                buttonImageOnly: true,
                buttonText: "Select date",
                dateFormat: 'mm/dd/yy',
                onClose: function () {
                    $(this).focus();
                }
            });*/

          //  $("[id$=btn_chk_val]").click(function (e) {
         //       e.preventDefault();
         //       var startdate = new Date();
        //        startdate = document.getElementById('<%=txt_frm_date.ClientID%>').value;
        //        var enddate = new Date();
       //         enddate = document.getElementById('<%=txt_to_date.ClientID%>').value;
       //         if (startdate > enddate) {
       //             alert('* To date must be after From date');
       //         }
       //     });
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">


    <%--------------------------Value Updates---------------------------------------%>
    <div>
           
          <h2 style="color:black;">Value Updates <span style="font-size: 16px;font-weight: normal;">*All Fields Required</span> </h2>
          <p>
            <asp:RadioButtonList ID="rdoValUpEnv" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdoValUpEnv_SelectedIndexChanged">
            <asp:ListItem Value="1"  Selected ="True">Production</asp:ListItem>
            <asp:ListItem Value="2">Training</asp:ListItem>
        </asp:RadioButtonList>
          </p>
     <table class="form">
        <tr>
             <td>
                 <asp:Label ID="lbl_file_name" runat="server" Text="*File Name" /><br />
                 <asp:TextBox ID="txt_file_name" runat="server" TextMode="SingleLine" MaxLength="500" Width="300px" ToolTip="Filename is required"  />
                <asp:RequiredFieldValidator ID="FilenameRequired" runat="server" ControlToValidate="txt_file_name" ErrorMessage="Filename is required" ForeColor="Red"  Display="Dynamic" ValidationGroup="ValueUpdates"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Label ID="lbl_app_id" runat="server" Text="*Application ID" /><br />
                <asp:TextBox ID="txt_app_id" runat="server" ToolTip="Application ID is required" />
                <asp:RequiredFieldValidator ID="AppIDRequired" runat="server" ControlToValidate="txt_app_id" ErrorMessage="Application ID is required"  ForeColor="Red" Display="Dynamic" ValidationGroup="ValueUpdates"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="ValChkAppId" runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txt_app_id" ErrorMessage="Value must be a whole number" Display="Dynamic" ForeColor="Red" ValidationGroup="ValueUpdates"/>
            </td>
            <td>
                <asp:Label ID="lbl_consensus" runat="server" Text="*Consensus" /><br />
                <asp:DropDownList ID="ddl_consensus" runat="server" Width="150px" >
                     <asp:ListItem Value="0">Options</asp:ListItem>
                     <asp:ListItem Value ="RC">Reconaliation</asp:ListItem>
                     <asp:ListItem Value ="CC">Cdr Consensus</asp:ListItem>
                 </asp:DropDownList>
               
                <asp:RequiredFieldValidator ID="ConsensusRequired" runat="server" ControlToValidate="ddl_consensus" ErrorMessage="(Required)"  ForeColor="Red" SetFocusOnError="true" Display="Dynamic" InitialValue="0"  ValidationGroup="ValueUpdates"></asp:RequiredFieldValidator>
                
            </td>

            <td>
                <asp:Label ID="lbl_frm_section" runat="server" Text="*Form Section" /><br />
                <asp:DropDownList ID="ddl_frm_section" runat="server" Width="100px">
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
  
            <td>
                <asp:Label ID="lbl_upd_values" runat="server" Text="*Update Values" /><br />
                <asp:TextBox ID="txt_upd_values" runat="server" TextMode="SingleLine" ToolTip="Update Values is required (Ex.1,4,5)" MaxLength="200"  />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_upd_values" ErrorMessage="Update Values is required"  ForeColor="Red" Display="Dynamic" ValidationGroup="ValueUpdates"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="ValChkUpdVal" runat="server"  ValidationExpression="^[0-9,]*$"   ControlToValidate="txt_upd_values"  ErrorMessage="Invalid!(Ex.1,4,5)" ForeColor="Red" Display="Dynamic" ValidationGroup="ValueUpdates"> </asp:RegularExpressionValidator>
            </td>

        </tr>
         <tr><td colspan = "9" ></td></tr>
        <tr>
            <td colspan = "9" style="text-align: right">
                
                <asp:Button ID="btn_update" runat="server" Text="Update" Width="153px" Height="32px" ValidationGroup="ValueUpdates" OnClick="btn_update_Click" />
                
            </td>
        </tr>
    </table>
 
     </div>

     <hr/>
    <%--------------------------Value Report ---------------------------------------%>
      <div>
          <h2 style="color:black;">Value Report</h2>
    <table class="form" style="width: 1052px">
    <tr>
    <td style="width: 222px">
          <p>
            <asp:RadioButtonList ID="rdoValRepEnv" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdoValRepEnv_SelectedIndexChanged">
            <asp:ListItem Value="1"  Selected ="True">Production</asp:ListItem>
            <asp:ListItem Value="2">Training</asp:ListItem>
        </asp:RadioButtonList>

          </p>
     <table class="form">
        <tr>
             <td>
                <asp:Label ID="lbl_app_id1" runat="server" Text="Application ID" /><br />
                <asp:TextBox ID="txt_app_id1" runat="server" TextMode="SingleLine" /><br />
                <asp:CompareValidator ID="ValChkAppId1" runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txt_app_id1" ErrorMessage="Value must be a whole number" Display="Dynamic" ForeColor="Red" ValidationGroup="ValueReport"/>
            </td>   
        </tr>
       
    </table>
    </td>
    <td style="vertical-align: top; width: 208px;">
        <asp:Label ID="lbl_consensus1" runat="server" Text="*Consensus" /><br />
                <asp:DropDownList ID="ddl_consensus1" runat="server" Width="150px" >
                     <asp:ListItem Value="0">Options</asp:ListItem>
                     <asp:ListItem Value ="RC">Reconaliation</asp:ListItem>
                     <asp:ListItem Value ="CC">Cdr Consensus</asp:ListItem>
                 </asp:DropDownList><br />
                 
                <asp:RequiredFieldValidator ID="ConsensusRequired1" runat="server" ControlToValidate="ddl_consensus1" ErrorMessage="(Required)"  ForeColor="Red" SetFocusOnError="true" Display="Dynamic" InitialValue="0"  ValidationGroup="ValueReport"></asp:RequiredFieldValidator>

    </td>
    <td style="vertical-align: top">    
        <asp:Label ID="lbl_report" runat="server" Text="*Run Report by" /><br />          
        <asp:DropDownList ID="ddlReport" runat="server" style="text-align: left" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged">
            <asp:ListItem Value="0">Select Report By</asp:ListItem>
            <asp:ListItem Value="1">ApplicationID</asp:ListItem>
            <asp:ListItem Value="2">Date Range</asp:ListItem>
              </asp:DropDownList>
         <asp:RequiredFieldValidator ID="ddlReportRequired" runat="server" ControlToValidate="ddlReport" ErrorMessage="(Required)" ForeColor="Red" InitialValue="0"  Display="Dynamic" ValidationGroup="ValueReport"></asp:RequiredFieldValidator>

    </td>
    <td style="width: 310px">          
        
      <table >
        <tr><td colspan = "2">Report by Last Updated Date Range</td></tr>
        <tr>
             <td style="width: 125px">
                <asp:Label ID="lbl_frm_date" runat="server" Text="From Date:" /><br />
                 <asp:TextBox ID="txt_frm_date" runat="server"  Width="100px"/>
                 <asp:CompareValidator ID="frmDateValidator" runat="server" Type="Date"  Operator="DataTypeCheck"  ControlToValidate="txt_frm_date"  ForeColor="Red"  ErrorMessage="Please enter a valid date." ValidationGroup="ValueReport" SetFocusOnError="True"></asp:CompareValidator>
            </td>

            <td style="width: 125px">
                 <asp:Label ID="lbl_to_date" runat="server" Text="To Date:" /><br />
                 <asp:TextBox ID="txt_to_date" runat="server"  Width="100px"/>
                <asp:CompareValidator ID="toDateValidator" runat="server" Type="Date"  Operator="DataTypeCheck"  ControlToValidate="txt_to_date"  ForeColor="Red"  ErrorMessage="Please enter a valid date." ValidationGroup="ValueReport" SetFocusOnError="True"></asp:CompareValidator>
            </td>
        </tr>
   
    </table>
    </td>
    </tr>
        <tr><td colspan="4">
            </td></tr>
    <tr>
        <td colspan = "4" style="text-align: right" >
            <asp:Button ID="btn_chk_val" runat="server" Text="Check Values" Height="32px" OnClick="btn_chk_val_Click" ValidationGroup="ValueReport" Width="153px" />
        </td>
    </tr>
    </table>
          
     </div>
     <hr/>
    <%--------------------------Rerun KAPPA---------------------------------------%>
    <div>
        <h2 style="color:black;">Rerun KAPPA <span style="font-size: 16px;font-weight: normal;">Production Only;</span> <span style="font-size: 13px;font-weight: normal;"> Kappa runs every 15 mins - check prod site</span> </h2>
    <table>
        <tr>
            <td>
                 <asp:Label ID="lbl_app_id2" runat="server" Text="*Application ID" /><br />
                <asp:TextBox ID="txt_app_id2" runat="server" TextMode="SingleLine"  />
                <asp:RequiredFieldValidator ID="AppIDRequired2" runat="server" ControlToValidate="txt_app_id2" ErrorMessage="Application ID is required"  ForeColor="Red" Display="Dynamic" ValidationGroup="RerunKappa" SetFocusOnError="True"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="ValChkAppId2" runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txt_app_id2" ErrorMessage="Value must be a whole number" Display="Dynamic" ForeColor="Red" ValidationGroup="RerunKappa" SetFocusOnError="True"/>
            </td>  
             <td>
                <asp:Label ID="lbl_consensus2" runat="server" Text="*Consensus" /><br />
                <asp:DropDownList ID="ddl_consensus2" runat="server" Width="150px" >
                     <asp:ListItem Value="0">Options</asp:ListItem>
                     <asp:ListItem Value ="RC">Reconaliation</asp:ListItem>
                     <asp:ListItem Value ="CC">Cdr Consensus</asp:ListItem>
                 </asp:DropDownList>
               
                <asp:RequiredFieldValidator ID="ConsensusRequired2" runat="server" ControlToValidate="ddl_consensus2" ErrorMessage="(Required)"  ForeColor="Red" SetFocusOnError="true" Display="Dynamic" InitialValue="0"  ValidationGroup="RerunKappa"></asp:RequiredFieldValidator>
                
            </td>
               <td style="width: 236px;"></td> 
            <td style="vertical-align: middle">
                 <asp:Button ID="btn_rerun_kappa" runat="server" Text="Rerun Kappa" Width="153px" Height="32px" OnClick="btn_rerun_kappa_Click" ValidationGroup="RerunKappa"/>
            </td>
            
        </tr>
    </table>
    </div>

</asp:Content>