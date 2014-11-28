

var table;
var cellPadding = 20;

config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role;


var util;


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

        $("div.progressBar").show();
        //$("div#tableContainer").show();

        table = $('#DTable').DataTable({

            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                console.log(" invoking fnRowCallback ::");
                $(nRow).addClass("closed");
                setTimeout(function () {
                    $("tr[role=row].selected").find("input").prop("checked", "checked");
                }, 0);
            },
            "rowCallback": function (row, data) {
                console.log(" invoking rowCallback ::");
                //                if ($.inArray(data.DT_RowId, $opts.hiderowItems) !== -1) {
                //                    $(row).addClass('selected');
                //                    //row.nodes().to$().find("input").prop("checked", "checked");
                //                   
                //                    //$(row).find("input").prop("checked", "checked");
                //                    //$("tr[role=row].selected").find("input").prop("checked", "checked");
                //                }
            },
            "bAutoWidth": false,
            
            "columnDefs": [
                     {
                         // show review list check box.
                         "render": function (data, type, row) {
                             // default condition add to review.
                             if (config.role == "ODPSupervisor") {
                                 if ($opts.filterlist != "review") {
                                     return data == true ? "&nbsp;" : '<input type="checkbox" />';
                                 }
                                 else {
                                     return data == true ? '<input type="checkbox"/>' : "&nbsp;";
                                 }
                             }
                             else {
                                 return "&nbsp;"
                             }
                             // in review list, remove.

                         },
                         "targets": 0,
                         "swidth": "10%"
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
                        "targets": 6 //title column
                    },
                    {

                        "render": function (data, type, row) {
                            if (data !== null) {
                                var myDate = new Date(data);
                                return getFormattedDate(myDate);
                            }
                            else {
                                return "";
                            }

                        },
                        "targets": 16 //date column
                    }

            //,
            //{ "visible": false, "targets": [6]} // hide abstract scan}


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
            { "data": "PIProjectLeader" },
            { "data": "ProjectTitle" },
            //   { "data": "AbstractScan" },
            {"data": "Flags" },
            { "data": "A1" },
            { "data": "A2" },
            { "data": "A3" },
            { "data": "B" },
            { "data": "C" },
            { "data": "D" },
            { "data": "E" },
            { "data": "F" },
            { "data": "LastExportDate" }
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
            // add function to remove rows that have been acted upon.
            //util.removeRows($opts.hideItems);
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
        $('div.progressBar').css('display', processing ? 'block' : 'none');
        if (!processing) {
            // show it all
            progressBarReset();
            $("#DTable_paginate").show();
            $("#DTable_info").show();
            $("#DTable").show();
            $("#tableContainer").show();
            //reset submit button
            $("#subButton").removeClass("yes").addClass("no");


            $("div#tableContainer").show();
            showActionsInterface();

            switch (config.role) {

                case "CoderSupervisor":
                    showActionsInterface();

                    break;

                case "ODPSupervisor":
                    showActionsInterface();
                    showSubActionsInterface();
                    break;

            }

        }
        else {
            $("#DTable_paginate").hide();
            $("#DTable_info").hide();
            $("#DTable").hide();
            $("#tableContainer").hide();

        }
    })




    table.on('init.dt', function () {


        console.log("datable initialized :: init.dt ::");

        childrenRedraw(table.data());


    });

    table.on('page.dt', function () {
        var info = table.page.info();
        console.log('Showing page: ' + '    ---  ' + info.page + ' of ' + info.pages);
        setTimeout(function () {
            $("tr[role=row].selected").find("input").prop("checked", "checked");
        }, 500);
    });


    // logic to open and close child rows.
    $('#DTable tbody').on('click', 'td.details-control', function (evt) {

        var tr = $(this).closest('tr');
        var child = $(tr).next();

        if ($(child).attr("role") == "row") {
            //console.log("child row missing. here ::" + evt);
            return; // child row missing.

        }

        if ($(tr).hasClass("open")) {
            $(child).addClass("hide").removeClass("show");
            //tr.removeClass('shown');
            tr.removeClass('open').addClass('closed');


        }

        else {
            $(child).addClass("show").removeClass("hide");
            //tr.addClass('shown');
            tr.removeClass('closed').addClass('open');
        }



    });

    function progressBarReset() {
        $("div.progressBar div.meter.animate").empty().append('<span style="width: 100%"><span></span></span>');
        setTimeout(function () {
            $(".meter > span").each(function () {
                $(this)
					.data("origWidth", "100%")
					.width(0)
					.animate({
					    width: "100%"
					}, 30000);
            });
        }, 0);

    }

    function filtersManager() {
        $("select#filterlist").empty();
        console.log(" filters manager :: " + config.role);
        hideSubActionsInterface();
        switch (config.role) {

            case "CoderSupervisor":
                //                $("select#filterlist").append('<option selected="selected" value="coded">Coded Abstracts</option>');
                //                $("select#filterlist").append('<option value="open">Open Abstracts</option>');
                showFiltersInterface();
                hideActionsInterface();
                setPageTitle("View Coded Abstracts");
                loadFilters();

                break;

            case "ODPSupervisor":
                //                $("select#filterlist").append('<option selected="selected" value="">Default View</option>');
                //                $("select#filterlist").append('<option value="open">Open Abstract</option>');
                //                $("select#filterlist").append('<option value="review">In Review List</option>');
                //                $("select#filterlist").append('<option value="uncoded">In Review List - Uncoded Only</option>');
                showFiltersInterface();
                hideActionsInterface();
                hideSubActionsInterface();
                setPageTitle("View Coded Abstracts");
                loadFilters();



                break;

            case "ODPStaff":
                //                $("select#filterlist").append('<option selected="selected" value="">Default View</option>');
                //                $("select#filterlist").append('<option value="open">Open Abstract</option>');
                //                $("select#filterlist").append('<option value="review">In Review List</option>');
                //                $("select#filterlist").append('<option value="uncoded">In Review List - Uncoded Only</option>');
                showFiltersInterface();
                hideActionsInterface();
                hideSubActionsInterface();
                setPageTitle("View Coded Abstracts");
                //                $("select#filterlist").append('<option selected="selected" value="coded">Default View</option>');
                //                $("select#filterlist").append('<option value="review">In Review List</option>');
                //                $("select#filterlist").append('<option value="uncoded">In Review List - Uncoded Only</option>');
                loadFilters();
                break;

            case "Admin":
                hideFiltersInterface();
                hideActionsInterface();
                hideSubActionsInterface();
                setPageTitle("View Abstracts");
                break;



        }


        $("select#filterlist option:selected").each(function () {
            $opts.filterlist = $(this).val();
        });
        config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role + "&filter=" + $opts.filterlist;



    }

    function loadFilters() {
        $.ajax({
            type: "GET",
            url: "/Evaluation/Handlers/Filters.ashx?role=" + config.role,
            dataType: 'json',
            data: { guid: window.user.GUID }
        })
                      .done(function (data) {
                          if (data.opts.length > 0) {
                              console.log(" filters received..");
                              $("select#filterlist").empty();
                              for (var i = 0; i < data.opts.length; i++) {
                                  $("select#filterlist").append('<option ' + (i == 0 ? 'selected="selected"' : '') + 'value="' + data.opts[i].option + '">' + data.opts[i].text + '</option>');
                              }

                          }
                      });
    }


    function hideFiltersInterface() {
        $(".filters.interface").hide();

    }
    function showFiltersInterface() {
        $(".filters.interface").show();
    }

    function hideActionsInterface() {
        $(".actions.interface").hide();
    }
    function showActionsInterface() {
        $(".actions.interface").show();
    }
    function hideSubActionsInterface() {
        $(".subactions.interface").hide();
    }
    function showSubActionsInterface() {
        $(".subactions.interface").show();
    }
    function setPageTitle(title) {
        $("#pagetitlebox span").text(title);
    }

    function actionsManager() {
        $("select#actionlist").empty();
        switch ($opts.filterlist) {

            case "review":
                $("select#actionlist").append('<option selected="selected" value="removereview">Remove From Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                break;
            case "codercompleted":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                break;
            case "activeabstracts":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                break;
            case "odpcompleted":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                break;
            case "odpcompletedwonotes":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                break;
            case "closed":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                break;
            case "exported":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                break;

            default:
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                break;


        }

    }

    function changeFilters() {
        config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role + "&filter=" + $opts.filterlist;
        table.ajax.url(config.baseURL);
        console.log(config.baseURL);
        switch ($opts.filterlist) {

            case "review":
                setPageTitle("View Coded Abstracts in Review");
                break;
            case "open":
                setPageTitle("View Open Abstracts");
                break;

            default:
                setPageTitle("View Coded Abstracts");
                break;


        }



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
        util.removeRowsV2($opts.hiderowItems);
        $opts.selectedItems = [];
        $opts.hiderowItems = [];
        $("#allBox").prop("checked", false);
        $("#selectallBox").prop("checked", false);

        return;

        $("table input[type=checkbox]").each(function (idx, val) {

            if ($(this).is(":checked")) {
                $(this).addClass("hideme");
                //
                /*
                table
                .row($(this).parents('tr'))
                .remove()
                .draw();

                */
            }

        });
    }


    function childrenRedraw(tdata) {
        util.setRows(tdata);
        table.rows().eq(0).each(function (rowIdx, val) {
            var childrows = util.getTableChildRows(rowIdx);
            //console.log(rowIdx + '    ' + val);
            if (childrows == '') {
                var rowx = table.row(rowIdx).nodes()
                .to$();     // Convert to a jQuery object

                rowx.addClass('nochildren');
                rowx.find("td.details-control").addClass('nodisplay');

                console.log(" here ::" + rowx);
            }

            table
        .row(rowIdx)
        .child(
            $(
                childrows
            ), "child hide"
        )
        .show();
        });

    }

    function doAllSubmitCheck(flag) {
        $opts.selectedItems = [];
        $opts.hiderowItems = [];
        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes()
                .to$();     // Convert to a jQuery object
            //console.log(rowx.find("input[type=checkbox]"));
            if (flag) {
                if (rowx.find("input[type=checkbox]").length > 0) {
                    console.log(rowx.find(".abstractid").html());
                    $opts.selectedItems.push(rowx.find(".abstractid").html());
                    $opts.hiderowItems.push(rowIdx);
                    rowx.addClass("selected");
                }
            }
            else {
                rowx.removeClass("selected");
            }
        });

        doSubmitChecks();

    }


    /* Event Registration */



    $("#allBox").on("click", function (evt) {
        util.showOpenRows(this.checked);

    });

    $("#selectallBox").on("click", function (evt) {
        util.selectAllRows(this.checked);
        doAllSubmitCheck(this.checked);

    });

    $("#tbutton").on("click", function (evt) {
        config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role;
        table.ajax.reload(function (json) {
            childrenRedraw(table.data());
        });
        //table.destroy();
    });



    // logic to select and unselect checkboxes
    $("body").on("click", "table.dataTable td input[type=checkbox]", function (evt) {

        var absid = $(this).parent().parent().find("td.abstractid").html();
        var rowIndex = table.row($(this).parent().parent()).index();
        var row = $(this).parents('tr');

        console.log('Row index: ' + table.row($(this).parent().parent()).index());
        if ($(this).is(":checked")) {
            var i = _.indexOf($opts.selectedItems, absid);
            $(row).addClass('selected');
            if (i == -1) {
                $opts.selectedItems.push(absid);
                $opts.hiderowItems.push(rowIndex);
            }

        }
        else {
            var i = _.indexOf($opts.selectedItems, absid);
            var rowi = _.indexOf($opts.hiderowItems, rowIndex);
            $(row).removeClass('selected');
            if (i != -1) {
                $opts.selectedItems.splice(i, 1);
                $opts.hiderowItems.splice(rowi, 1);
            }

        }
        doSubmitChecks();



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
                              alertify.success($opts.selectedItems.length + " " + "Abstract(s) added to review list.");
                              //$opts.hideItems = $opts.selectedItems;
                              resetSubmitBtnAndCheckboxes();
                              loadFilters();
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
                              alertify.success($opts.selectedItems.length + " " + "Abstract(s) removed from review list.");
                              //$opts.hideItems = $opts.selectedItems;
                              resetSubmitBtnAndCheckboxes();
                              loadFilters();
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
