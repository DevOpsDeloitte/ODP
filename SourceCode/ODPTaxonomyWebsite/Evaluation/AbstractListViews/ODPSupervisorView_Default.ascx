﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ODPSupervisorView_Default.ascx.cs"
    Inherits="ODPTaxonomyWebsite.Evaluation.AbstractListViews.ODPSupervisorView_Default" %>
<%@ Register TagPrefix="odp" Namespace="ODPTaxonomyDAL_JY" Assembly="ODPTaxonomyDAL_JY" %>
<h3>
    View Coded Abstracts</h3>

    <script type="text/javascript">

        var table;

        function Utility(data) {
            // private member
            var Rows = data;
            this.RowsP = data;

            // public function
            this.getRows = function () {
                return Rows;
            };

            this.getTableChildRows = function (id) {
                var rowData = Rows[id];
                var childTable = '';
                for (var i = 0; i < rowData.ChildRows.length; i++) {
                    console.log(rowData.ChildRows[i]);
                    var ctRow = '<tr>' +
                                '<td>' + '</td>' + // Col 1
                                '<td style="width:'+ $($("#DTable th")[1]).css("width") +'" >' + rowData.ChildRows[i].AbstractID + '</td>' + // Col 2
                                '<td style="width:' + $($("#DTable th")[2]).css("width") + '" >' + '&nbsp;' + '</td>' + // Col 3
                                '<td style="width:'+ $($("#DTable th")[3]).css("width") +'" >' + '&nbsp;' + '</td>' + // Col 4
                                '<td style="width:' + $($("#DTable th")[4]).css("width") + '" >' + rowData.ChildRows[i].ProjectTitle + '</td>' + // Col 3
                                '<td style="width:' + $($("#DTable th")[5]).css("width") + '" >' + rowData.ChildRows[i].Flags + '</td>' + // Col 4
                                '<td style="width:' + $($("#DTable th")[6]).css("width") + '" >' + rowData.ChildRows[i].A1 + '</td>' + // Col 5
                                '<td style="width:' + $($("#DTable th")[7]).css("width") + '" >' + rowData.ChildRows[i].A2 + '</td>' + // Col 6
                                '<td style="width:' + $($("#DTable th")[8]).css("width") + '" >' + rowData.ChildRows[i].A3 + '</td>' + // Col 7
                                '<td style="width:' + $($("#DTable th")[9]).css("width") + '" >' + rowData.ChildRows[i].B + '</td>' + // Col 8
                                '<td style="width:' + $($("#DTable th")[10]).css("width") + '" >' + rowData.ChildRows[i].C + '</td>' + // Col 9
                                '<td style="width:' + $($("#DTable th")[11]).css("width") + '" >' + rowData.ChildRows[i].D + '</td>' + // Col 10
                                '<td style="width:' + $($("#DTable th")[12]).css("width") + '" >' + rowData.ChildRows[i].E + '</td>' + // Col 11
                                '<td style="width:' + $($("#DTable th")[13]).css("width") + '" >' + rowData.ChildRows[i].F + '</td>' + // Col 12
                                '<td style="width:' + $($("#DTable th")[14]).css("width") + '" >' + rowData.ChildRows[i].G + '</td>' + // Col 13
                               
                            '</tr>'
                    childTable = childTable + ctRow;
                }
                if (rowData.ChildRows.length > 0) {
                    childTable = '<table>' + childTable + '</table>';
                }

                return childTable;
            };

        }
        var util; //= new Utility();

        /* Formatting function for row details - modify as you need */
        function format(d) {
            // `d` is the original data object for the row
            console.log(" format d:: " + d);
                return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
            '<tr>' +
                '<td>Full name:</td>' +
                '<td>' + 'test' + '</td>' +
            '</tr>' +
            '<tr>' +
                '<td>Extension number:</td>' +
                '<td>' + 'test2' + '</td>' +
            '</tr>' +
            '<tr>' +
                '<td>Extra info:</td>' +
                '<td>And any further details here (images etc)...</td>' +
            '</tr>' +
        '</table>';
            }

            $(document).ready(function () {
                //$('#example').DataTable();
                table = $('#DTable').DataTable({
                    "processing": true,
                    "ajax": "/Evaluation/Handlers/Abstracts.ashx",
                    "columns": [
                 {
                     "class": 'details-control',
                     "orderable": false,
                     "data": null,
                     "defaultContent": ''
                 },
            { "data": "AbstractID" },
            { "data": "ApplicationID" },
            { "data": "StatusDate" },
            { "data": "ProjectTitle" },
            { "data": "Flags" },
            { "data": "A1" },
            { "data": "A2" },
            { "data": "A3" },
            { "data": "B" },
            { "data": "C" },
            { "data": "D" },
            { "data": "E" },
            { "data": "F" },
            { "data": "G" }
            ]


                });

                table.on('draw.dt', function () {
                    console.log('Redraw occurred at: ' + new Date().getTime());
                });

                table.on('init.dt', function () {
                    console.log("loaded...");
                    util = new Utility(table.data());

                    table.rows().eq(0).each(function (rowIdx) {
                        
                        var childrows = util.getTableChildRows(rowIdx);
                        console.log(rowIdx+ '    ' + childrows);
                        table
                    .row(rowIdx)
                    .child(
                        $(
                           childrows
                        )
                    )
                    .show();
                    });

                });

                // $('#MainContent_ODPSupervisorView_Default_AbstractViewGridView').prepend($("<thead></thead>").append($(this).find("tr:first"))).dataTable();

                //             $('#DTable tbody').on('click', 'td.details-control', function (evt) {
                //                 console.log("here ::" + evt);
                //                 var tr = $(this).closest('tr');
                //                 var row = table.row(tr);

                //                 if (row.child.isShown()) {
                //                     // This row is already open - close it
                //                     row.child.hide();
                //                     tr.removeClass('shown');
                //                 }
                //                 else {
                //                     // Open this row
                //                     row.child(format(row.data())).show();
                //                     tr.addClass('shown');
                //                 }
                //             });

            });

</script>


    <table id="DTable" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th></th>
                <th>Abstract ID</th>
                <th>Application ID</th>
                <th>Status Date</th>
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
                <th>G</th>
                
            </tr>
        </thead>

           <tfoot>
            <tr>
            <th></th>
               <th>Abstract ID</th>
               <th>Application ID</th>
               <th>Status Date</th>
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
                <th>G</th>
            </tr>
        </tfoot>
  </table>


     <div class="showingCounts">
      <span  class="showing">Page&nbsp;:&nbsp;<%=  (AbstractViewGridView.PageIndex + 1) %></span><!-- of <span class="showing"><%=AbstractViewGridView.PageCount %></span>--><br />
            Showing : 
            <%  var totalCount = 0;
                var showing = 0;
                var displayCounts = false;
                try
                {
                    totalCount = ((ICollection<AbstractListRow>)AbstractViewGridView.DataSource).Where(x => x.ApplicationID > 0).Select(x => x).Count();
                    //showing = (AbstractViewGridView.PageIndex + 1) * AbstractViewGridView.Rows.Count;
                    var RowSshowing = AbstractViewGridView.Rows.Cast<GridViewRow>().Where(x => x.Cells[2].Text.ToString().Replace("&nbsp;","").Trim().Length > 3);
                    showing = RowSshowing.Count() ;
                    if ((AbstractViewGridView.PageIndex + 1) == AbstractViewGridView.PageCount)
                    {
                        //showing = totalCount;
                    }
                    displayCounts = true;
                }
                catch (Exception ex)
                {
                    

                }
            
            %>
            <% if (displayCounts)
               { %>
            <span class="showing"><%= showing%></span> of <span class="showing"><%= totalCount%></span> Abstracts.
            <% } %>
            </div>
<asp:Button runat="server" class="review button no" Text="Add to Review List" OnClick="AddtoReviewHandler" OnClientClick="return checkStatus();" />
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
<asp:Button class="review button no" runat="server" Text="Add to Review List" OnClick="AddtoReviewHandler" OnClientClick="return checkStatus();" />






