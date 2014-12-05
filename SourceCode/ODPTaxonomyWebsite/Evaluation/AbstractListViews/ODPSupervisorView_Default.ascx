<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ODPSupervisorView_Default.ascx.cs"
    Inherits="ODPTaxonomyWebsite.Evaluation.AbstractListViews.ODPSupervisorView_Default" %>
<%@ Register TagPrefix="odp" Namespace="ODPTaxonomyDAL_JY" Assembly="ODPTaxonomyDAL_JY" %>


<div class="sixteen columns" id="functionsbox"> 
            
        <div class="three columns filters interface">     
        <label for="filterlist">Filters List</label>
        <select name="filterlist"  id="filterlist">
		        <option selected="selected" value="">Default View</option>
		        <option value="open">Open Abstract</option>
		        <option value="review">In Review List</option>
		        <option value="uncoded">In Review List - Uncoded Only</option>
        </select>

        </div>   

        <div class="three columns subactions interface">  
        <label for="actionlistlist">Actions List</label>
        <select name="actionlist"  id="actionlist">
		        <option selected="selected" value="addtoreview">Add to Review List</option>
		        <option value="closeabstracts">Close Abstracts</option>
		        <option value="reopenabstracts">Reopen Abstracts</option>
		        <option value="exportabstracts">Export Abstracts</option>
        </select>

        </div>
        <div class="two columns subactions interface">
            <input type="button" name="subButton" id="subButton" value="Submit" class="review button no" />
        </div>
        <div class="three columns">
           
        </div>
        <div class="two columns actions interface">  
            <label><input type="checkbox" name="allBox" id="allBox" value="expandall" class="cboxes"> Expand All</label>
        </div>
        <div class="two columns subactions interface">  
            <label><input type="checkbox" name="selectallBox" id="selectallBox" value="selectall" class="cboxes"> Select All</label>
        </div>

    

</div>

<div class="sixteen columns">
    <div class="eight columns" id="pagetitlebox"> 
        <span>Title</span>
    </div>
    <div class="five columns hidden" id="selectionsBox"> 
        <span id="recordCount">0</span><span> Records selected</span>
    </div>
</div>





<script type="text/javascript">

   // window.config.role = "ODPSupervisor";

</script>


<%--<div class="filterBoxes hide">

    
  
    <input type="button" name="tbutton" id= "tbutton" value="try" class="review button yes" />

</div>--%>

<div class="progressBar">
     <div id="spinner"></div>
     <div class="progressText">
         <span>Loading records...</span>
         </div>
    <div class="meter animate">
	<span style="width: 100%"><span></span></span>
    </div>
</div>

<div id="tableContainer">
        <table id="DTable" class="display" cellspacing="0" width="100%">
            <thead>
                <tr>
                    <th class="col_select"></th>
                    <th class="col_openclose"></th>
                    <th class="col_abstractid">Abstract ID</th>
                    <th class="col_applicationid">Application ID</th>
                    <th class="col_statusdate">Status Date</th>
                    <th class="col_piname">PI Name</th>
                    <th class="col_title">Title</th>        
                    <th class="col_flags">Flags</th>
                    <th class="col_kappa">A1</th>
                    <th class="col_kappa">A2</th>
                    <th class="col_kappa">A3</th>
                    <th class="col_kappa">B</th>
                    <th class="col_kappa">C</th>
                    <th class="col_kappa">D</th>
                    <th class="col_kappa">E</th>
                    <th class="col_kappa">F</th>
                    <th class="col_exportdate">Exported Date</th>
                
                </tr>
            </thead>

               <tfoot>
                <tr>
                    <th></th>
                    <th></th>
                   <th>Abstract ID</th>
                   <th>Application ID</th>
                   <th>Status Date</th>
                   <th>PI Name</th>
                   <th>Title</th>
                   <th>Flags</th>
                    <th>A1</th>
                    <th>A2</th>
                    <th>A3</th>
                    <th>B</th>
                    <th>C</th>
                    <th>D</th>
                    <th>E</th>
                    <th>F</th>
                    <th>Exported Date</th>
                </tr>
            </tfoot>
      </table>
  </div>

<%--<asp:Button runat="server" class="review button no" Text="Add to Review List" OnClick="AddtoReviewHandler" OnClientClick="return checkStatus();" />--%>
<odp:AbstractGridView runat="server" ID="AbstractViewGridView" AutoGenerateColumns="false"
    GridLines="None" CssClass="AbstractViewTable bordered zebra-striped" AllowPaging="false">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HiddenField runat="server" ID="AbstractID" Value='<%#Eval("AbstractID") %>' />
                <asp:CheckBox runat="server" CssClass="reviewcheck" ID="ToReview" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="AbstractID" HeaderText="ID" />
        <asp:BoundField DataField="ApplicationID" HeaderText="APPL ID" SortExpression="ApplicationID" />
        <asp:BoundField DataField="StatusDateDisplay" HeaderText="Status Date" SortExpression="Date" />
        <asp:TemplateField HeaderText="Title" SortExpression="Title">
            <ItemTemplate>
                <asp:Panel runat="server" ID="TitleWrapper" CssClass="title-wrapper">
                    <asp:HyperLink runat="server" ID="AbstractTitleLink" Visible="false" />
                    <asp:Label runat="server" ID="AbstractTitleText" Visible="false" />
                    <asp:Image ID="AbstractScanClip" runat="server" ImageUrl="~/Images/clip.png" AlternateText="Attachment"
                        CssClass="scan-file" Visible="false" />
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Flags" HeaderText="Flags" />
        <asp:BoundField HeaderText="A1" DataField="A1"></asp:BoundField>
        <asp:BoundField HeaderText="A2" DataField="A2"></asp:BoundField>
        <asp:BoundField HeaderText="A3" DataField="A3"></asp:BoundField>
        <asp:BoundField HeaderText="B" DataField="B"></asp:BoundField>
        <asp:BoundField HeaderText="C" DataField="C"></asp:BoundField>
        <asp:BoundField HeaderText="D" DataField="D"></asp:BoundField>
        <asp:BoundField HeaderText="E" DataField="E"></asp:BoundField>
        <asp:BoundField HeaderText="F" DataField="F"></asp:BoundField>
        <asp:BoundField HeaderText="G" DataField="G"></asp:BoundField>
    </Columns>
    <PagerStyle CssClass="PagerContainer" />
</odp:AbstractGridView>
<%--<asp:Button class="review button no" runat="server" Text="Add to Review List" OnClick="AddtoReviewHandler" OnClientClick="return checkStatus();" />--%>






