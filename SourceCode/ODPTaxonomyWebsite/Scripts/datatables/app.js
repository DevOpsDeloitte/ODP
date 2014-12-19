

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

    var spinneropts2 = {
        lines: 9, // The number of lines to draw
        length: 8, // The length of each line
        width: 3, // The line thickness
        radius: 3, // The radius of the inner circle
        corners: 1, // Corner roundness (0..1)
        rotate: 0, // The rotation offset
        direction: 1, // 1: clockwise, -1: counterclockwise
        color: '#000', // #rgb or #rrggbb or array of colors
        speed: 1, // Rounds per second
        trail: 30, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false, // Whether to use hardware acceleration
        className: 'spinner', // The CSS class to assign to the spinner
        zIndex: 2e9, // The z-index (defaults to 2000000000)
        top: '0px', // Top position relative to parent
        left: '0px' // Left position relative to parent
    };

    var target = document.getElementById('spinner');
    var downloadSpin = document.getElementById('downloadSpin');
    var spinner = new Spinner(spinneropts).spin(target);
    var spinnerdownloadSpin = new Spinner(spinneropts2).spin(downloadSpin);

    util = new Utility();

    // ODPStaff role starts out with review list;
    if (config.role == "ODPStaff") {
        $opts.lastfilterSelection = "review";
    }
    if (config.role == "ODPSupervisor") {
        $("div#selectionsBox").removeClass("hidden");
    }

    filtersManager(); //Init
    actionsManager(); // Init
    disableFilters();


    function InitializeTable() {

        $("div.progressBar").show();
        //$("div#tableContainer").show();

        table = $('#DTable').DataTable({

            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                //console.log(" invoking fnRowCallback ::");
                $(nRow).addClass("closed");
                setTimeout(function () {
                    $("tr[role=row].selected").find("input").prop("checked", "checked");
                }, 0);
            },
            "aLengthMenu": [[10, 25, 50, 100], ["Display 10", "Display 25", "Display 50", "Display 100"]],
            "sDom" : 'lfitp',
            "rowCallback": function (row, data) {
                //console.log(" invoking rowCallback ::");
                //                if ($.inArray(data.DT_RowId, $opts.hiderowItems) !== -1) {
                //                    $(row).addClass('selected');
                //                    //row.nodes().to$().find("input").prop("checked", "checked");
                //                   
                //                    //$(row).find("input").prop("checked", "checked");
                //                    //$("tr[role=row].selected").find("input").prop("checked", "checked");
                //                }
            },
            "bAutoWidth": false,
            "language": {
                "lengthMenu": "_MENU_",
                "zeroRecords": "Sorry. No Abstracts found!",
                //                    "info": "Showing page _PAGE_ of _PAGES_",
                "infoEmpty": "Sorry. No Abstracts found!",
                "infoFiltered": "(filtered from _MAX_ total records)"
            },

            "columnDefs": [
                     {
                         // show review list check box.
                         "render": function (data, type, row) {
                             // default condition add to review.
                             if (config.role == "ODPSupervisor") {
                                 return '<input type="checkbox"/>';
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
                         className: "test1tscolxxxxxxxxxx",
                         "targets": [0]

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
                            collink = "<a href='/Evaluation/ViewAbstract.aspx?AbstractID=" + row.AbstractID + "'" + " onclick=\"return showRedirectMessage(" + row.AbstractID + ")\">" + data + "</a>";
                            var class1 = row.AbstractScan !== null ? "scan-file" : "";
                            var addImg = '<img class="scan-file" src="../Images/clip.png" alt="Attachment">';
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


                ],

            "processing": true,
            "ajax": config.baseURL,
            "order": [[4, "desc"]],
            "columns": [
                 {
                     "class": 'checkbox-control',
                     "orderable": false,
                     //"data": "InReview"//,
                     "data": null,
                     "defaultContent": ''

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
            { "data": "Flags" },
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
        }, 0);

        // added for search event :: to make sure checkboxes are selected when items have been selected. On search there is a re-draw.
        setTimeout(function () {
            $("tr[role=row].selected").find("input").prop("checked", "checked");
        }, 100);
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
        $opts.isGridDirty = false;
        if (config.role == "ODPSupervisor") {
            if ($opts.actionlist == "reopenabstracts") {
                reopenListCheck();
            }
            else {
                ListCheck($opts.actionlist);
            }
        }
        //enableFilters();
        enableInterface();


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
        assignPageTitle();

        switch (config.role) {

            case "CoderSupervisor":

                showFiltersInterface();
                hideActionsInterface();
                loadFilters();
                break;

            case "ODPSupervisor":

                showFiltersInterface();
                hideActionsInterface();
                hideSubActionsInterface();
                loadFilters();
                break;

            case "ODPStaff":

                showFiltersInterface();
                hideActionsInterface();
                hideSubActionsInterface();
                loadFilters();
                break;

            case "Admin":
                showFiltersInterface();
                hideActionsInterface();
                hideSubActionsInterface();
                loadFilters();
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
                                  if ($opts.lastfilterSelection == '') {
                                      $("select#filterlist").append('<option ' + (i == 0 ? 'selected="selected"' : '') + 'value="' + data.opts[i].option + '">' + data.opts[i].text + '</option>');
                                  }
                                  else {
                                      $("select#filterlist").append('<option ' + ($opts.lastfilterSelection == data.opts[i].option ? 'selected="selected"' : '') + 'value="' + data.opts[i].option + '">' + data.opts[i].text + '</option>');
                                  }
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
                $opts.actionlist = "removereview";
                break;
            case "reviewuncoded":
                $("select#actionlist").append('<option selected="selected" value="removereview">Remove From Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $opts.actionlist = "removereview";
                break;
            case "codercompleted":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $opts.actionlist = "addreview";
                break;
            case "activeabstracts":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $opts.actionlist = "addreview";
                break;
            case "odpcompleted":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $opts.actionlist = "addreview";
                break;
            case "odpcompletedwonotes":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $opts.actionlist = "addreview";
                break;
            case "closed":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $opts.actionlist = "addreview";
                break;
            case "exported":
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $opts.actionlist = "addreview";
                break;

            default:
                $("select#actionlist").append('<option selected="selected" value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $opts.actionlist = "addreview";
                break;


        }

    }

    function changeFilters() {
        config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role + "&filter=" + $opts.filterlist;
        table.ajax.url(config.baseURL);
        $opts.lastfilterSelection = $opts.filterlist;
        console.log(config.baseURL);
        $("div#downloadLinkBox").hide();
        assignPageTitle();




        table.ajax.reload(function (json) {
            childrenRedraw(table.data());
            if (config.role == "ODPSupervisor") {
                if ($opts.actionlist == "reopenabstracts") {
                    reopenListCheck();
                }
                else {
                    ListCheck($opts.actionlist);
                }
            }
            $opts.isGridDirty = false;
            //enableFilters();
            enableInterface();
            //resetSubmitBtnAndCheckboxes();
            clearSubmitBtnAndCheckboxes();
        });

    }

    function assignPageTitle() {

        switch ($opts.filterlist) {

            case "review":
                setPageTitle("View Review List");
                break;
            case "reviewuncoded":
                setPageTitle("View Review List Uncoded");
                break;
            case "open":
                setPageTitle("View Open Abstracts");
                break;
            case "coded":
                setPageTitle("View Coded Abstracts");
                break;
            case "closed":
                setPageTitle("View Closed Abstracts");
                break;
            case "odpcompleted":
                setPageTitle("View ODP Completed Abstracts");
                break;
            case "codercompleted":
                setPageTitle("View Coder Completed Abstracts");
                break;
            case "activeabstracts":
                setPageTitle("View Active Abstracts");
                break;
            case "exported":
                setPageTitle("View Exported Abstracts");
                break;
            case "odpcompletedwonotes":
                setPageTitle("View ODP Completed Without Notes Abstracts");
                break;

            default:
                setPageTitle("View All Abstracts");
                break;


        }

    }

    function disableFilters() {
        $("select#filterlist").attr("disabled", true);
    }
    function enableFilters() {
        $("select#filterlist").attr("disabled", false);
    }

    function disableInterface() {
        $("select#filterlist").attr("disabled", true);
        $("select#actionlist").attr("disabled", true);
        $("input").attr("disabled", true);
    }

    function enableInterface() {
        $("select#filterlist").attr("disabled", false);
        $("select#actionlist").attr("disabled", false);
        $("input").attr("disabled", false);
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

        updateSelectedList();

    }
    // called on change of filter and action ::
    function clearSubmitBtnAndCheckboxes() {
        $("#subButton").removeClass("yes").addClass("no");
        $opts.selectedItems = [];
        $opts.hiderowItems = [];
        //$("#allBox").prop("checked", false);
        $("#selectallBox").prop("checked", false);

        $opts.selectedItems = [];
        $opts.hiderowItems = [];
        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes()
                .to$();     // Convert to a jQuery object

            if (rowx.find("input[type=checkbox].visiblecheckbox").length > 0) {
                rowx.find("input[type=checkbox].visiblecheckbox").prop("checked", false);
                rowx.removeClass("selected");
            }
        });
        // table redraw occurs, slight time delay introduced.
        setTimeout(function () {
            //util.selectAllRows(false);
            //util.showOpenRows(false);
        }, 400);

        updateSelectedList();
    }

    //called after action submitted ::
    function resetSubmitBtnAndCheckboxes() {

        util.removeRowsV2($opts.hiderowItems);
        clearSubmitBtnAndCheckboxes();
        return;

        //        $("#subButton").removeClass("yes").addClass("no");

        //        $opts.selectedItems = [];
        //        $opts.hiderowItems = [];
        //        $("#allBox").prop("checked", false);
        //        $("#selectallBox").prop("checked", false);

        //        // table redraw occurs, slight time delay introduced.
        //        setTimeout(function () {
        //            util.selectAllRows(false);
        //            util.showOpenRows(false);
        //        }, 400);

        //        updateSelectedList();

        //        return;

    }

    function updateSelectedList() {


        $("span#recordCount").text($opts.selectedItems.length);

        //doAllCheck();


    }

    function doAllCheckDataBinding() {

        // do all check to simulate two way data-binding.
        var allselected = true;
        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object
            if (rowx.find("input[type=checkbox].visiblecheckbox").length > 0) {
                if (rowx.find("input[type=checkbox].visiblecheckbox").parents("tr").hasClass("selected")) {
                }
                else {
                    allselected = false;
                }
            }

        });
        if (allselected) $("#selectallBox").prop("checked", true);

    }


    function childrenRedraw(tdata) {
        util.setRows(tdata);
        table.rows().eq(0).each(function (rowIdx, val) {
            var childrows = util.getTableChildRowsV2(rowIdx);
            //console.log(rowIdx + '    ' + val);
            var rowx = table.row(rowIdx).nodes()
                .to$();     // Convert to a jQuery object
            if (childrows == '') {
                //                var rowx = table.row(rowIdx).nodes()
                //                .to$();     // Convert to a jQuery object

                rowx.addClass('nochildren');
                rowx.find("td.details-control").addClass('nodisplay');

                //console.log(" here ::" + rowx);
            }
            else {
                //                var rowx = table.row(rowIdx).nodes()
                //                .to$();     // Convert to a jQuery object
                rowx.addClass('haschildren');

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
                if (rowx.find("input[type=checkbox].visiblecheckbox").length > 0) {
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
            // check to turn off all 
            turnOffSelectAll();


        }

        doAllCheckDataBinding();
        doSubmitChecks();



    });

    function turnOffSelectAll() {
        if ($("#selectallBox").is(":checked")) {
            $("#selectallBox").prop("checked", false);
        }
    }




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
                              resetSubmitBtnAndCheckboxes();
                              loadFilters();
                              $opts.isGridDirty = true;

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
                              resetSubmitBtnAndCheckboxes();
                              loadFilters();
                              $opts.isGridDirty = true;
                          }
                          else {
                              alertify.error("Failed to remove from review list.");
                          }
                      });


                    break;

                case "closeabstract":

                    $.ajax({
                        type: "GET",
                        url: "/Evaluation/Handlers/AbstractClose.ashx",
                        dataType: 'json',
                        data: { type: "close", abstracts: $opts.selectedItems.join(), guid: window.user.GUID }
                    })
                      .done(function (data) {
                          console.log(" closed abstract : " + data);
                          if (data.success == true) {
                              alertify.success($opts.selectedItems.length + " " + "Abstract(s) have been closed.");
                              resetSubmitBtnAndCheckboxes();
                              loadFilters();
                              $opts.isGridDirty = true;

                          }
                          else {
                              alertify.error("Failed to close abstracts.");
                          }
                      });


                    break;

                case "reopenabstracts":

                    $.ajax({
                        type: "GET",
                        url: "/Evaluation/Handlers/AbstractClose.ashx",
                        dataType: 'json',
                        data: { type: "open", abstracts: $opts.selectedItems.join(), guid: window.user.GUID }
                    })
                      .done(function (data) {
                          console.log(" reopen abstract : " + data);
                          if (data.success == true) {
                              alertify.success($opts.selectedItems.length + " " + "Abstract(s) have been Re-opened.");
                              resetSubmitBtnAndCheckboxes();
                              loadFilters();
                              $opts.isGridDirty = true;

                          }
                          else {
                              alertify.error("Failed to Re-open abstracts.");
                          }
                      });


                    break;

                case "exportabstracts":

                    // show progress indicator
                    $("div#downloadProgressBox").show();

                    $.ajax({
                        type: "GET",
                        url: "/Evaluation/Handlers/AbstractExport.ashx",
                        dataType: 'json',
                        data: { abstracts: $opts.selectedItems.join(), guid: window.user.GUID }
                    })
                      .done(function (data) {
                          console.log(" reopen abstract : " + data);
                          if (data.success == true) {
                              alertify.success($opts.selectedItems.length + " " + "Abstract(s) have been Exported. File being generated.");


                              // call the second handler
                              $.ajax({
                                  type: "POST",
                                  url: "/Evaluation/Handlers/GenerateExcelReport.ashx",
                                  dataType: 'json',
                                  data: { abstracts: $opts.selectedItems.join(), guid: window.user.GUID }
                              })
                              .done(function (data) {
                                  console.log(" generate excel report .ashx : " + data);
                                  if (data.success == true) {
                                      $("div#downloadProgressBox").hide();
                                      $("div#downloadLinkBox a").attr("href", data.filePath);
                                      $("div#downloadLinkBox").show();
                                  }
                              });



                              //for exporting abstracts.
                              //var iframe = $("<iframe id='export-frame' src='DataExportHandler.ashx?method=export' />").hide();
                              //$(this).parent().append(iframe);

                              resetSubmitBtnAndCheckboxes();
                              loadFilters();
                              $opts.isGridDirty = true;


                          }
                          else {
                              alertify.error("Failed to Export abstracts.");
                          }
                      });


                    break;





            }

        }
        else {
            //console.log("not enabled ::");

        }

    });

    $("select#filterlist").change(function () {
        var str = "";
        //disableFilters();
        disableInterface();
        $("select#filterlist option:selected").each(function () {
            str = $(this).val();
            $opts.filterlist = $(this).val();
        });

        actionsManager();
        changeFilters();
        $("div#downloadLinkBox").hide();
    });

    $("select#actionlist").change(function () {
        var str = "";
        $("select#actionlist option:selected").each(function () {
            str = $(this).val();
            $opts.actionlist = $(this).val();
        });

        watchactionsHandler();
    });


    function watchactionsHandler() {
        $("div#downloadLinkBox").hide();
        console.log(" watchactionsHandler() ::" + $opts.actionlist);
        switch ($opts.actionlist) {

            case "reopenabstracts":

                if ($opts.isGridDirty) {

                    disableFilters();
                    //actionsManager();
                    disableInterface();
                    changeFilters();
                    clearSubmitBtnAndCheckboxes();

                }
                else {
                    reopenListCheck();
                    clearSubmitBtnAndCheckboxes();
                }

                break;

            default:
                reloadForAction($opts.actionlist);
                break;

        }
    }

    function reloadForAction(type) {

        if ($opts.isGridDirty) {

            disableFilters();
            //actionsManager();
            disableInterface();
            changeFilters();
            clearSubmitBtnAndCheckboxes();

        }
        else {
            ListCheck(type);
            clearSubmitBtnAndCheckboxes();
        }


    }

    function reopenListCheck() {
        $.ajax({
            type: "GET",
            url: "/Evaluation/Handlers/AbstractsReopen.ashx",
            dataType: 'json',
            data: { guid: window.user.GUID }
        })
                      .done(function (data) {
                          console.log(" reopen : " + data);
                          if (data.success) {
                              //alertify.success(" reopen data :: " + data.nottoreopen);
                              hideCheckBoxes(data.nottoreopen);

                          }
                          else {
                              //alertify.error("Failed to get reopen data");
                          }
                      });

    }

    function ListCheck(type) {

        switch (type) {

            case "":
                break;
            default:
                $.ajax({
                    type: "GET",
                    url: "/Evaluation/Handlers/AbstractsListCheck.ashx",
                    dataType: 'json',
                    data: { guid: window.user.GUID, action: type }
                })
                        .done(function (data) {
                            console.log(" reopen : " + data);
                            if (data.success) {
                                //alertify.success(" reopen data :: " + data.nottoreopen);
                                hideCheckBoxes(data.hideboxes);

                            }
                            else {
                                //alertify.error("Failed to get reopen data");
                            }
                        });

                break;

        }

    }

    function hideCheckBoxes(inArr) {

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes()
                .to$();     // Convert to a jQuery object
            rowx.find("input[type=checkbox]").addClass("visiblecheckbox").removeClass("hidecheckbox"); // make all visible
            rowx.find("input[type=checkbox]").prop("checked", false);
            rowx.removeClass("selected");


            var absid = rowx.find(".abstractid").html().trim();
            if (_.contains(inArr, Number(absid))) {
                rowx.find("input[type=checkbox]").addClass("hidecheckbox").removeClass("visiblecheckbox");
            }
            else {
                rowx.find("input[type=checkbox]").addClass("visiblecheckbox").removeClass("hidecheckbox");
            }

        });


    }



});
