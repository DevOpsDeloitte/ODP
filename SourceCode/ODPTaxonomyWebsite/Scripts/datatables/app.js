﻿

var table;
var cellPadding = 20;

config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role;


function Utility() {
    // private member
//    var Rows = data;
//    this.RowsP = data;
    var Rows = null;
    this.RowsP = null;



    // public function
    this.getRows = function () {
        return Rows;
    };

    this.setRows = function (data) {
        this.RowsP = data;
        Rows = data;
    };

    this.coalesceCol = function (inv) {
        var out = "&nbsp;";
        if (inv !== undefined && inv !== null) {
            if (inv.length > 0) {
                out = inv
            }
        }
        return out;
    };

    this.selectAllRows = function (flag) {
        $("table tr td input[type=checkbox]").prop("checked", flag);
    };

    this.showOpenRows = function (flag) {

        if (flag) {
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
            var ctRow = '<tr>' +
                        '<td class="ccol" style="width:' + '16px' + '" >' + '&nbsp;' + '</td>' + // Col 1
                        '<td class="ccol" style="width:' + '16px' + '" >' + '&nbsp;' + '</td>' + // Col 1a
                    '<td class="ccol" style="width:' + ($($("#DTable th")[2]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].AbstractID + '</td>' + // Col 2
                    '<td class="ccol" style="width:' + ($($("#DTable th")[3]).outerWidth() - cellPadding).toString() + 'px" >' + '&nbsp;' + '</td>' + // Col 3
                    '<td class="ccol" style="width:' + ($($("#DTable th")[4]).outerWidth() - cellPadding).toString() + 'px" >' + '&nbsp;' + '</td>' + // Col 4
                    '<td class="ccol" style="width:' + ($($("#DTable th")[5]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].ProjectTitle + '</td>' + // Col 3
                    '<td class="ccol" style="width:' + ($($("#DTable th")[6]).outerWidth() - cellPadding).toString() + 'px" >' + this.coalesceCol(rowData.ChildRows[i].Flags) + '</td>' + // Col 4
                    '<td class="ccol" style="width:' + ($($("#DTable th")[7]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].A1 + '</td>' + // Col 5
                    '<td class="ccol" style="width:' + ($($("#DTable th")[8]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].A2 + '</td>' + // Col 6
                    '<td class="ccol" style="width:' + ($($("#DTable th")[9]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].A3 + '</td>' + // Col 7
                    '<td class="ccol" style="width:' + ($($("#DTable th")[10]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].B + '</td>' + // Col 8
                    '<td class="ccol" style="width:' + ($($("#DTable th")[11]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].C + '</td>' + // Col 9
                    '<td class="ccol" style="width:' + ($($("#DTable th")[12]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].D + '</td>' + // Col 10
                    '<td class="ccol" style="width:' + ($($("#DTable th")[13]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].E + '</td>' + // Col 11
                    '<td class="ccol" style="width:' + ($($("#DTable th")[14]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].F + '</td>' + // Col 12
                    '<td class="ccol" style="width:' + ($($("#DTable th")[15]).outerWidth() - cellPadding).toString() + 'px" >' + this.coalesceCol(rowData.ChildRows[i].G) + '</td>' + // Col 13

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


$(document).ready(function () {

    var spinneropts = {
        lines: 10, // The number of lines to draw
        length: 10, // The length of each line
        width: 3, // The line thickness
        radius: 5, // The radius of the inner circle
        corners: 1, // Corner roundness (0..1)
        rotate: 0, // The rotation offset
        direction: 1, // 1: clockwise, -1: counterclockwise
        color: '#373644', // #rgb or #rrggbb or array of colors
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false, // Whether to use hardware acceleration
        className: 'spinner', // The CSS class to assign to the spinner
        zIndex: 2e9, // The z-index (defaults to 2000000000)
        top: '35px', // Top position relative to parent
        left: '15px' // Left position relative to parent
    };

    var target = document.getElementById('spinner');
    var spinner = new Spinner(spinneropts).spin(target);


    util = new Utility();

    filtersManager(); //Init
    actionsManager(); // Init


    function InitializeTable() {

        $("div#spinner").show();
        $("div#tableContainer").show();

        table = $('#DTable').DataTable({

            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                $(nRow).addClass("closed");
            },
            "columnDefs": [
                     {
                         // show review list check box.
                         "render": function (data, type, row) {
                             // default condition add to review.
                             if ($opts.filterlist != "review") {
                                 return data == true ? "&nbsp;" : '<input type="checkbox" />';
                             }
                             else {
                                 return data == true ? '<input type="checkbox"/>' : "&nbsp;";
                             }
                             // in review list, remove.

                         },
                         "targets": 0
                     },
                    {
                        // The `data` parameter refers to the data for the cell (defined by the
                        // `data` option, which defaults to the column being worked with, in
                        // this case `data: 0`.
                        "render": function (data, type, row) {
                            return data.length == 0 ? "&nbsp;" : data;

                        },
                        "targets": 7
                    },

                    { "visible": true, "targets": [5] },
                    {

                        "render": function (data, type, row) {
                            var myDate = new Date(data);
                            //myDate = myDate.replace(/^(\d{4})\-(\d{2})\-(\d{2}).*$/, '$2/$3/$1');
                            return getFormattedDate(myDate);

                        },
                        "targets": 4 //date column
                    },

                    {

                        "render": function (data, type, row) {
                            var collink = "";
                            //console.log(row);
                            collink = "<a href='/Evaluation/ViewAbstract.aspx?AbstractID=" + row.AbstractID + "'>" + data + "</a>"; ;
                            var class1 = row.AbstractScan !== null ? "scan-file" : "";
                            var addImg = '<img class="scan-file" src="../Images/clip.png" alt="Attachment">';
                            //this.className = class1;
                            if (class1 != "") collink = '<div class="titleimg has-file" style="position: relative">' + collink + addImg + '</div>';
                            return collink;

                        },
                        "targets": 5 //title column
                    },
                    { "visible": false, "targets": [6]} // hide abstract scan}


                ],
            //iDisplayLength: 10,
            "processing": true,
            "ajax": config.baseURL,

            "columns": [
                 {
                     "class": 'checkbox-control',
                     "orderable": false,
                     "data": "InReview"//,
                     //"defaultContent": '<input type="checkbox" />'
                     // "defaultContent": ''
                 },
                {
                    "class": 'details-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": ''
                },
            { "data": "AbstractID", "class": "abstractid" },
            { "data": "ApplicationID" },
            { "data": "StatusDate" },
            { "data": "ProjectTitle", "width": "120px" },
            { "data": "AbstractScan" },
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


    }

    InitializeTable();



    table.on('draw.dt', function () {
        console.log('Redraw occurred at: ' + new Date().getTime());
        var alb = $("#allBox").is(':checked');
        var salb = $("#selectallBox").is(':checked');
        setTimeout(function () {
            util.showOpenRows(alb);
            util.selectAllRows(salb);
        }, 0);
    });

    //    table.on('preXhr.dt', function (e, settings, data) {
    //        $("div#spinner").show();
    //    });

    //    table.on('xhr.dt', function (e, settings, json) {
    //        $("div#spinner").hide();
    //    })

    table.on('processing.dt', function (e, settings, processing) {
        console.log(" processing.dt " + processing);
        $('div#spinner').css('display', processing ? 'block' : 'none');
        if (!processing) {
            // show it all
            $("#DTable_paginate").show();
            $("#DTable_info").show();
            $("#DTable").show();
            //reset submit button
            $("#subButton").removeClass("yes").addClass("no");


            $("div#tableContainer").show();

        }
        else {
            $("#DTable_paginate").hide();
            $("#DTable_info").hide();
            $("#DTable").hide();


        }
    })




    table.on('init.dt', function () {


        console.log("datable initialized :: init.dt ::");

        childrenRedraw(table.data());


    });

    table.on('page.dt', function () {
        var info = table.page.info();
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

    function filtersManager() {
        $("select#filterlist").empty();
        console.log(" filters manager :: " + config.role);
        switch (config.role) {

            case "CoderSupervisor":
                $("select#filterlist").append('<option selected="selected" value="coded">Coded Abstracts</option>');
                $("select#filterlist").append('<option value="open">Open Abstracts</option>');
                hideActionsInterface();
                setPageTitle("View Coded Abstracts");
                break;

            case "ODPSupervisor":

                break;



        }


        $("select#filterlist option:selected").each(function () {
            $opts.filterlist = $(this).val();
        });
        config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role + "&filter=" + $opts.filterlist;



    }

    function hideActionsInterface() {
        $(".actions.interface").hide();
    }
    function showActionsInterface() {
        $(".actions.interface").show();
    }
    function setPageTitle(title) {

    }

    function actionsManager() {
        $("select#actionlist").empty();
        switch ($opts.filterlist) {

            case "review":
                $("select#actionlist").append('<option selected="selected" value="removereview">Remove From Review List</option>');

                break;

            default:
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                break;


        }

    }

    function changeFilters() {
        config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role + "&filter=" + $opts.filterlist;
        table.ajax.url(config.baseURL);
        console.log(config.baseURL);
        table.ajax.reload(function (json) {
            childrenRedraw(table.data());
        });

    }

    function doSubmitChecks() {
        //alertify.success("showing...");
        $opts.actionlist = $("select#actionlist option:selected").val();

        if ($opts.selectedItems.length > 0 && $opts.actionlist != "") {

            $("#subButton").removeClass("no").addClass("yes");
        }
        else {
            $("#subButton").removeClass("yes").addClass("no");
        }

    }

    function resetSubmitBtnAndCheckboxes() {

        $("#subButton").removeClass("yes").addClass("no");
        $("table input[type=checkbox]").each(function (idx, val) {

            if ($(this).is(":checked")) {
                $(this).addClass("hideme");
            }

        });
    }


    function childrenRedraw(tdata) {
        util.setRows(tdata);

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

    }


    /* Event Registration */



    $("#allBox").on("click", function (evt) {
        util.showOpenRows(this.checked);

    });

    $("#selectallBox").on("click", function (evt) {
        util.selectAllRows(this.checked);

    });

    $("#tbutton").on("click", function (evt) {
        config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role;
        table.ajax.reload(function (json) {
            childrenRedraw(table.data());
        });
        //table.destroy();
    });




    $("body").on("click", "table.dataTable td input[type=checkbox]", function (evt) {
        var absid = $(this).parent().parent().find("td.abstractid").html();
        if ($(this).is(":checked")) {
            var i = _.indexOf($opts.selectedItems, absid);
            if (i == -1) {
                $opts.selectedItems.push(absid);
            }
            //console.log(" abstract id : " + $(this).parent().parent().find("td.abstractid").html());
        }
        else {
            var i = _.indexOf($opts.selectedItems, absid);
            if (i != -1) {
                $opts.selectedItems.splice(i, 1);
            }

        }
        doSubmitChecks();
        console.log("   doSubmitChecks(); checkbox clicked : " + $(this).is(":checked"));


    });




    $("input#subButton").on("click", function (evt) {
        if ($(this).hasClass("yes")) {
            console.log("enabled ::");

            switch ($opts.actionlist) {

                case "addreview":

                    $.ajax({
                        type: "GET",
                        url: "/Evaluation/Handlers/AbstractReview.ashx",
                        dataType: 'json',
                        data: { type: "add", abstracts: $opts.selectedItems.join(), guid: window.user.GUID }
                    })
                      .done(function (data) {
                          console.log(" add : " + data);
                          if (data.success == true) {
                              alertify.success("Abstract(s) added to review list.");
                              resetSubmitBtnAndCheckboxes();
                          }
                          else {
                              alertify.error("Failed to add in review list.");
                          }
                      });


                    break;

                case "removereview":

                    $.ajax({
                        type: "GET",
                        url: "/Evaluation/Handlers/AbstractReview.ashx",
                        dataType: 'json',
                        data: { type: "remove", abstracts: $opts.selectedItems.join(), guid: window.user.GUID }
                    })
                      .done(function (data) {
                          console.log(" remove : " + data);
                          if (data.success == true) {
                              alertify.success("Abstract(s) removed from review list.");
                              resetSubmitBtnAndCheckboxes();
                          }
                          else {
                              alertify.error("Failed to remove from review list.");
                          }
                      });


                    break;




            }

        }
        else {
            console.log("not enabled ::");

        }

    });

    $("select#filterlist").change(function () {
        //alert("Handler for .change() called.");
        var str = "";
        $("select#filterlist option:selected").each(function () {
            str = $(this).val();
            $opts.filterlist = $(this).val();
        });

        actionsManager();
        changeFilters();
    });

    $("select#actionlist").change(function () {
        //alert("Handler for .change() called.");
        var str = "";
        $("select#actionlist option:selected").each(function () {
            str = $(this).val();
            $opts.actionlist = $(this).val();
        });
    });



});
