﻿


(function ($) {
    $.fn.extend({
        check: function () {
            return this.filter(":radio, :checkbox").attr("checked", true);
        },
        uncheck: function () {
            return this.filter(":radio, :checkbox").removeAttr("checked");
        }
    });
} (jQuery));

getFormattedDate = function (date) {
    var year = date.getFullYear();
    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    // return year + '/' + month + '/' + day;
    return month + '/' + day + '/' + year;
};

var table;
var cellPadding = 20;

function Utility(data) {
    // private member
    var Rows = data;
    this.RowsP = data;

    // public function
    this.getRows = function () {
        return Rows;
    };

    this.coalesceCol = function (inv) {
        var out = "&nbsp;";
        //console.log(" val :: " + inv);
        if (inv.length > 0 && inv !== undefined) {
            out = inv
        }
        return out;
    };

    this.showOpenRows = function (flag) {

        if (flag) {
            //console.log("All box checked..");
            $("#DTable tr").each(function (idx, value) {
                // parent rows
                if (!$(this).hasClass("shown")) {
                    $(this).addClass("shown");
                }
                // child rows
                if ($(this).hasClass("hide")) {
                    $(this).removeClass("hide").addClass("show");
                }

            });

        }
        else {

            //console.log("All box unchecked..");

            $("#DTable tr").each(function (idx, value) {
                // parent rows
                if ($(this).hasClass("shown")) {
                    $(this).removeClass("shown");
                }
                // child rows
                if ($(this).hasClass("show")) {
                    $(this).removeClass("show").addClass("hide");
                }

            });
        }


    };

    this.getTableChildRows = function (id) {
        var rowData = Rows[id];
        var childTable = '';
        for (var i = 0; i < rowData.ChildRows.length; i++) {
            //console.log(rowData.ChildRows[i]);
            var ctRow = '<tr>' +
                        '<td class="ccol" style="width:' + '16px' + '" >' + '&nbsp;' + '</td>' + // Col 1
                    '<td class="ccol" style="width:' + ($($("#DTable th")[1]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].AbstractID + '</td>' + // Col 2
                    '<td class="ccol" style="width:' + ($($("#DTable th")[2]).outerWidth() - cellPadding).toString() + 'px" >' + '&nbsp;' + '</td>' + // Col 3
                    '<td class="ccol" style="width:' + ($($("#DTable th")[3]).outerWidth() - cellPadding).toString() + 'px" >' + '&nbsp;' + '</td>' + // Col 4
                    '<td class="ccol" style="width:' + ($($("#DTable th")[4]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].ProjectTitle + '</td>' + // Col 3
                    '<td class="ccol" style="width:' + ($($("#DTable th")[5]).outerWidth() - cellPadding).toString() + 'px" >' + this.coalesceCol(rowData.ChildRows[i].Flags) + '</td>' + // Col 4
                    '<td class="ccol" style="width:' + ($($("#DTable th")[6]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].A1 + '</td>' + // Col 5
                    '<td class="ccol" style="width:' + ($($("#DTable th")[7]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].A2 + '</td>' + // Col 6
                    '<td class="ccol" style="width:' + ($($("#DTable th")[8]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].A3 + '</td>' + // Col 7
                    '<td class="ccol" style="width:' + ($($("#DTable th")[9]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].B + '</td>' + // Col 8
                    '<td class="ccol" style="width:' + ($($("#DTable th")[10]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].C + '</td>' + // Col 9
                    '<td class="ccol" style="width:' + ($($("#DTable th")[11]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].D + '</td>' + // Col 10
                    '<td class="ccol" style="width:' + ($($("#DTable th")[12]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].E + '</td>' + // Col 11
                    '<td class="ccol" style="width:' + ($($("#DTable th")[13]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].F + '</td>' + // Col 12
                    '<td class="ccol" style="width:' + ($($("#DTable th")[14]).outerWidth() - cellPadding).toString() + 'px" >' + this.coalesceCol(rowData.ChildRows[i].G) + '</td>' + // Col 13

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
        //"bAutoWidth": false,
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).addClass("closed");
            //$('td:eq(0)', nRow).addClass( "avo-lime-h avo-heading-white" );
            //$('td:eq(1),td:eq(2),td:eq(3)', nRow).addClass( "avo-light" );
        },
        "columnDefs": [
            {
                // The `data` parameter refers to the data for the cell (defined by the
                // `data` option, which defaults to the column being worked with, in
                // this case `data: 0`.
                "render": function (data, type, row) {
                    return data.length == 0 ? "&nbsp;" : data;

                },
                "targets": 5
            },

            { "visible": true, "targets": [4] },
            {

                "render": function (data, type, row) {
                    var myDate = new Date(data);
                    //myDate = myDate.replace(/^(\d{4})\-(\d{2})\-(\d{2}).*$/, '$2/$3/$1');
                    return getFormattedDate(myDate);

                },
                "targets": 3
            }

        ],
        //iDisplayLength: 10,
        "processing": true,
        //"ajax": "/Evaluation/Handlers/Abstracts.ashx",
        "ajax": " /Evaluation/Handlers/json.js",

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
    { "data": "ProjectTitle", "width": "120px" },
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
        var alb = $("#allBox").is(':checked');
        setTimeout(function () {
            util.showOpenRows(alb);
        }, 0);
    });

    table.on('init.dt', function () {
        console.log("loaded...");
        util = new Utility(table.data());

        table.rows().eq(0).each(function (rowIdx) {

            var childrows = util.getTableChildRows(rowIdx);
            //console.log(rowIdx+ '    ' + childrows);
            table
        .row(rowIdx)
        .child(
            $(
                childrows
            ), "hide"
        )
        .show();

        });

    });

    table.on('page.dt', function () {
        var info = table.page.info();
        //                    var alb = $("#allBox").is(':checked');
        //                    setTimeout(function () {
        //                        util.showOpenRows(alb);
        //                    }, 100);
        console.log('Showing page: ' + '    ---  ' + info.page + ' of ' + info.pages);
    });

    $('#DTable tbody').on('click', 'td.details-control', function (evt) {
        console.log("here ::" + evt);
        var tr = $(this).closest('tr');
        var child = $(tr).next();

        if (!$(tr).hasClass("shown")) {
            $(child).addClass("show").removeClass("hide");
            tr.addClass('shown');
            //tr.removeClass('closed').addClass('open');
        }

        else {
            $(child).addClass("hide").removeClass("show");
            tr.removeClass('shown');
            //tr.removeClass('open').addClass('closed');
        }



    });



    $("#allBox").on("click", function (evt) {
        util.showOpenRows(this.checked);

    });


});
