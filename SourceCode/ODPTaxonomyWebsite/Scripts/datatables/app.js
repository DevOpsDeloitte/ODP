

var table;
var cellPadding = 20;

config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role;


var util;

function isMobile() {
    if (navigator.userAgent.match(/Android/i)
            || navigator.userAgent.match(/iPhone/i)
            || navigator.userAgent.match(/iPad/i)
            || navigator.userAgent.match(/iPod/i)
            || navigator.userAgent.match(/BlackBerry/i)
            || navigator.userAgent.match(/Windows Phone/i)
            || navigator.userAgent.match(/Opera Mini/i)
            || navigator.userAgent.match(/IEMobile/i)
            ) {
        return true;
    }
}


$(document).ready(function () {


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
        //$("div#selectionsBox").removeClass("hidden");
    }



    function InitializeTable() {

        $("div.progressBar").show();
        //$("div#tableContainer").show();
        console.log(" invoking InitializeTable() ::");
        table = $('#DTable').DataTable({

            "stateSave": true,
            "stateSaveParams": function (settings, data) {
                //console.log(" saving state params " + JSON.stringify(data));
                data.search.search = "";
            },

            "stateLoadParams": function (settings, data) {
                //data.search.search = "";
                //console.log(" saving load params " + JSON.stringify(data));
            },

            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                //console.log(" invoking fnRowCallback ::");
                //$(nRow).addClass("closed");
                setTimeout(function () {
                    $("tr[role=row].selected").find("input").prop("checked", "checked");
                }, 0);
            },
            "aLengthMenu": [[10, 25, 50, 100, 250, 500, -1], ["Display 10", "Display 25", "Display 50", "Display 100", "Display 250", "Display 500", "Display All"]],
            "sDom": '<"filter-wrap"f><"length-wrap"l><"paginate-wrap"p><"table-wrap"t>ip',
            "rowCallback": function (row, data) {

            },
            "bAutoWidth": false,
            "language": {
                "lengthMenu": "_MENU_",
                "zeroRecords": "Sorry. No Abstracts found!",
                //                    "info": "Showing page _PAGE_ of _PAGES_",
                "infoEmpty": "Sorry. No Abstracts found!",
                "infoFiltered": "(filtered from _MAX_ total records)",
                "sSearch": ""
            },

            "columnDefs": [
                     {
                         // show review list check box.
                         "render": function (data, type, row) {
                             // default condition add to review.
                             if (config.role == "ODPSupervisor") {
                                 //console.log(" show row data : " + JSON.stringify(row));
                                 return '<input type="checkbox" id="rowabs-' + row.AbstractID + '" /><label for="rowabs-' + row.AbstractID + '"></label>';

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
                            if (config.role == "ODPStaff") {
                                return data.length == 0 ? "&nbsp;" : data.replace(", E7F6", "").replace("E7F6", "");
                            }
                            else {
                                return data.length == 0 ? "&nbsp;" : data;
                            }

                        },
                        "targets": 7
                    },

                    { "visible": true, "targets": [5] },
                    {

                        "render": function (data, type, row) {
                            var myDate = new Date(data);
                            return getFormattedDate(myDate);

                        },
                        "targets": 4 //date column
                    },

                    {

                        "render": function (data, type, row) {
                            var collink = "";
                            if (isMobile()) {
                                //collink = "<a href='/Evaluation/ViewAbstract.aspx?AbstractID=" + row.AbstractID + "'" + " ontouchstart=\"return showRedirectMessage(" + row.AbstractID + ")\">" + data + "</a>";
                                collink = "<a target='_blank' href='/Evaluation/ViewAbstract.aspx?AbstractID=" + row.AbstractID + "'" + ">" + data + "</a>";
                            }
                            else {
                                collink = "<a href='/Evaluation/ViewAbstract.aspx?AbstractID=" + row.AbstractID + "'" + " onclick=\"return showRedirectMessage(" + row.AbstractID + ")\">" + data + "</a>";
                            }
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
            //"ajax": config.baseURL,
            "order": [[4, "desc"]],
            "columns": [
                 {
                     "class": 'checkbox-control',
                     "orderable": false,
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

        setupTableEvents();
    }

    filtersManager(); //Init
    actionsManager(); // Init
    disableFilters();


    InitializeTable();

    $.fn.dataTableExt.afnFiltering.push(function (oSettings, aData, iDataIndex) {

        if (_.contains($opts.hideboxes, Number(aData[2]))) {
            return false;
        }
        else {
            return true;
        }

    });
    // set search placeholder
    $('.dataTables_filter input').attr('placeholder', 'Search');


    function setupTableEvents() {

        table.on('draw.dt', function () {
            console.log('Redraw occurred at: ' + new Date().getTime());
            var alb = $("#allBox").is(':checked');
            var salb = $("#selectallBox").is(':checked');
            setTimeout(function () {
                //util.showOpenRows(alb);  // Not needed Datatables is maintaining state on table draws.
                util.selectAllRows(salb);
            }, 0);

            // added for search event :: to make sure checkboxes are selected when items have been selected. On search there is a re-draw.
            setTimeout(function () {
                $("tr[role=row].selected").find("input").prop("checked", "checked");
            }, 100);
        });


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



        //is this needed?
        table.on('init.dtx', function () {


            console.log("datable initialized :: init.dt ::");

            childrenRedraw(table.data());
            $opts.isGridDirty = false;
            if (config.role == "ODPSupervisor") {
                console.log("datable initialized :: serverCheckForActions() ::");
                serverCheckForActions();

            }
            //enableFilters();
            enableInterface();


        });

        table.on('page.dt', function () {
            var info = table.page.info();

            window.location.hash = $opts.filterlist + "|" + $opts.actionlist + "|" + info.page;

            console.log('Showing page: ' + '    ---  ' + info.page + ' of ' + info.pages);
            setTimeout(function () {
                $("tr[role=row].selected").find("input").prop("checked", "checked");
            }, 500);
        });

    };

    function loadChildContainer(abstractid) {
        //load initial child container with loading message.
        return '<div class="loadingAbstractDetail" id="' + abstractid + '"><div class="loader"></div><div class="text">loading...</div></div>';
    };

    function getDetailChildRow(abstractid) {
        var content = "";

        $.ajax('Handlers/AbstractDetail.ashx', {
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            data: { role: config.role, 'abstractId': abstractid },
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data) {
                    console.log(data.data[0].ChildRows);
                    content = unescape(util.getTableChildRowsV3(data.data[0]));
                    $("div#" + abstractid).html(content);
                }
            }

        });




    };


    // logic to open and close child rows. // 1.3 dynamically load child rows.
    $('#DTable tbody').on('click', 'td.details-control', function (evt) {

        var tr = $(this).closest('tr');
        var row = table.row(tr);
        //        var child = $(tr).next();

        //        if ($(child).attr("role") == "row") {
        //            return; // child row missing.
        //        }
        console.log(" details row clicked :: ");
        var absid = $(this).parent().find("td.abstractid").html();
        var data = getDetailChildRow(absid);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('open').addClass('closed');
        }
        else {
            // Open this row
            row.child(loadChildContainer(absid)).show();
            tr.removeClass('closed').addClass('open');
        }

        //        if ($(tr).hasClass("open")) {
        //            //$(child).addClass("hide").removeClass("show");
        //            tr.removeClass('open').addClass('closed');

        ////            var i = _.indexOf($opts.openItems, absid);
        ////            if (i != -1) {
        ////                $opts.openItems.splice(i, 1);
        ////            }
        //        }
        //        else {
        //            //$(child).addClass("show").removeClass("hide");
        //            tr.removeClass('closed').addClass('open');
        //            //$opts.openItems.push(absid);
        //        }



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

    // sets up the filters and does the load filters.
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

        //        $("select#filterlist option:selected").each(function () {
        //            $opts.filterlist = $(this).val();
        //        });
        //        config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role + "&filter=" + $opts.filterlist;
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
                              //console.log(" filters received..");
                              $("select#filterlist").empty();

                              if (window.location.hash.replace("#", "") != "") {
                                  var locationHash = window.location.hash.replace("#", "").split("|");
                                  var filterVal = locationHash[0], actionVal = locationHash[1];
                                  $opts.pageNumber = locationHash[2];
                                  if ($opts.initialPageLoad) {
                                      $opts.lastfilterSelection = filterVal;
                                      //$opts.initialPageLoad = false;
                                  }
                              }

                              for (var i = 0; i < data.opts.length; i++) {
                                  if ($opts.lastfilterSelection == '') {
                                      $("select#filterlist").append('<option ' + (i == 0 ? 'selected="selected"' : '') + 'value="' + data.opts[i].option + '">' + data.opts[i].text + '</option>');
                                  }
                                  else {
                                      $("select#filterlist").append('<option ' + ($opts.lastfilterSelection == data.opts[i].option ? 'selected="selected"' : '') + 'value="' + data.opts[i].option + '">' + data.opts[i].text + '</option>');
                                  }
                              }

                          }



                          $("select#filterlist option:selected").each(function () {
                              $opts.filterlist = $(this).val();
                          });
                          config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role + "&filter=" + $opts.filterlist;
                          changeFilters();

                          //                          table.ajax.url(config.baseURL);
                          //                          setTimeout(function () {
                          //                              table.ajax.reload(function (json) {

                          //                              });
                          //                          }, 0);
                          //InitializeTable();
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
        $("#selectallBox").addClass("hidecheckbox");
    }
    function showSubActionsInterface() {
        $(".subactions.interface").show();
        $("#selectallBox").removeClass("hidecheckbox");
    }
    function setPageTitle(title) {
        $("#pagetitlebox span").text(title);
    }

    function actionsManager() {
        $("select#actionlist").empty();
        switch ($opts.filterlist) {

            case "review":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="removereview">Remove From Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $opts.actionlist = "removereview";
                $opts.actionlist = "selectaction";
                break;
            case "reviewuncoded":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="removereview">Remove From Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $opts.actionlist = "removereview";
                $opts.actionlist = "selectaction";
                break;
            case "codercompleted":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";
                break;
            case "activeabstracts":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";
                break;
            case "odpcompleted":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";
                break;
            case "odpcompletedwonotes":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";
                break;
            case "closed":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";
                break;
            case "exported":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";
                break;

            case "default": // default = all
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $opts.actionlist = "selectaction";
                break;

            case "all":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $opts.actionlist = "selectaction";
                break;

            default: // currently same as odpcompleted, being the default.

                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";

                break;


        }
        //Intercept if first load to check for hash location based -- previous action.

        if (window.location.hash.replace("#", "") != "") {
            var locationHash = window.location.hash.replace("#", "").split("|");
            var filterVal = locationHash[0], actionVal = locationHash[1];
            $opts.pageNumber = locationHash[2];
            if ($opts.initialPageLoad) {
                if (actionVal != "selectaction") {
                    $("select#actionlist option").each(function (idx, val) {
                        $(this).attr('selected', false);
                        if ($(this).val() == actionVal) {
                            $(this).attr('selected', true);
                        }
                    });
                }

                $opts.actionlist = actionVal;
                //$opts.initialPageLoad = false;
            }
        }

    }

    function changeFilters() {
        config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role + "&filter=" + $opts.filterlist;
        table.ajax.url(config.baseURL);
        $opts.lastfilterSelection = $opts.filterlist;
        console.log(config.baseURL);
        $("div#downloadLinkBox").hide();
        assignPageTitle();

        window.location.hash = $opts.filterlist + "|" + $opts.actionlist + "|" + $opts.pageNumber;
        $opts.hideboxes = [];



        table.ajax.reload(function (json) {
            childrenRedraw(table.data());
            if (config.role == "ODPSupervisor") {
                // change check of action checkboxes happens here - when Filter re-loads.. same block repeated table.init.
                serverCheckForActions();

            }
            $opts.isGridDirty = false;
            enableInterface();
            clearSubmitBtnAndCheckboxes();
        });

    }

    function serverCheckForActions() {

        switch ($opts.actionlist) {

            case "reopenabstracts":
                $("th.col_select").children().show();
                reopenListCheck();
                break;
            case "selectaction":
                $("th.col_select").children().hide();
                hideAllCheckBoxes();
                if ($opts.initialPageLoad) {


                }
                break;
            default:
                $("th.col_select").children().show();
                ListCheck($opts.actionlist);
                break;

        }
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

                switch (config.role) {
                    case "ODPSupervisor":
                        setPageTitle("View Abstracts");
                        break;

                    case "ODPStaff":
                        setPageTitle("View Abstracts");
                        break;

                    case "Admin":
                        setPageTitle("View Abstracts");
                        break;
                }

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


    }

    function updateSelectedList() {


        $("span#recordCount").text($opts.selectedItems.length);
        if ($opts.selectedItems.length > 0 && config.role == "ODPSupervisor") {
            $("div#selectionsBox").removeClass("hidden");
        }
        else {
            $("div#selectionsBox").addClass("hidden");
        }
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

    // in 1.3 we will not be pre-loading row data. childrenRedraw will add parent classes or nochildren/haschildren and return.
    function childrenRedraw(tdata) {
        util.setRows(tdata);
        table.rows().eq(0).each(function (rowIdx, val) {
            var childrows = util.getTableChildRowsV2(rowIdx);

            var rowx = table.row(rowIdx).nodes()
                .to$();
            var kappaCount = table.row(rowIdx).data().KappaCount;
            // Convert to a jQuery object
            //console.log(" ROW :: " + rowIdx + '    ' + JSON.stringify(table.row(rowIdx).data().KappaCount));
            if (kappaCount <= 1) {
                //                var rowx = table.row(rowIdx).nodes()
                //                .to$();     // Convert to a jQuery object

                rowx.addClass('nochildren');
                rowx.find("td.details-control").addClass('nodisplay');

            }
            else {

                rowx.addClass('haschildren');

            }

            return;
            // Not needed anymore for 1.3.
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
        var filteredRows = table.$('tr', { "filter": "applied" });
        var filteredRowsAbstractIDs = [];
        $(filteredRows).each(function (i, val) {
            //console.log(val);
            //console.log($(val).find(".abstractid").html());
            filteredRowsAbstractIDs.push($(val).find(".abstractid").html());
        });
        //        
        //         filteredRows.map(function (frow) {
        //            console.log($(frow));
        //            return $(frow).find(".abstractid").html();

        //        });

        console.log(filteredRowsAbstractIDs);
        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes()
                .to$();     // Convert to a jQuery object
            //console.log(rowx.find("input[type=checkbox]"));
            if (flag) {
                if (rowx.find("input[type=checkbox].visiblecheckbox").length > 0) {
                    //console.log(rowx.find(".abstractid").html());
                    if (_.contains(filteredRowsAbstractIDs, rowx.find(".abstractid").html())) {
                        $opts.selectedItems.push(rowx.find(".abstractid").html());
                        $opts.hiderowItems.push(rowIdx);
                        rowx.addClass("selected");
                    }
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

        //console.log('Row index: ' + table.row($(this).parent().parent()).index());
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


    function bindSelections() {

        $("select#filterlist").change(function () {
            var str = "";
            //disableFilters();
            disableInterface();
            $("select#filterlist option:selected").each(function () {
                str = $(this).val();
                $opts.filterlist = $(this).val();
            });

            actionsManager();
            $opts.actionlist = "selectaction";
            changeFilters();
            $("div#downloadLinkBox").hide();
        });

        $("select#actionlist").change(function () {
            var str = "";
            $("select#actionlist option:selected").each(function () {
                str = $(this).val();
                $opts.actionlist = $(this).val();
            });
            $opts.pageNumber = 0;
            window.location.hash = $opts.filterlist + "|" + $opts.actionlist + "|" + $opts.pageNumber;
            watchactionsHandler();
        });

    };

    bindSelections();


    function watchactionsHandler() {
        $("div#downloadLinkBox").hide();
        console.log(" watchactionsHandler() ::" + $opts.actionlist);
        switch ($opts.actionlist) {

            case "reopenabstracts":
                $("th.col_select").children().show();
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

            case "selectaction":
                $("th.col_select").children().hide();
                //for minor release.
                //                if ($opts.isGridDirty) {
                //                    reloadForAction($opts.actionlist);
                //                }
                $opts.hideboxes = [];
                clearSubmitBtnAndCheckboxes();
                hideAllCheckBoxes();

                break;

            default:
                $("th.col_select").children().show();
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
                              $opts.hideboxes = data.nottoreopen;
                              hideCheckBoxes(data.nottoreopen);
                              setTimeout(function () {
                                  if ($opts.initialPageLoad) {
                                      $opts.initialPageLoad = false;
                                      table.page(parseInt($opts.pageNumber)).draw(false);
                                  }
                              }, 200);

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
                                $opts.hideboxes = data.hideboxes;
                                hideCheckBoxes(data.hideboxes);
                                setTimeout(function () {
                                    if ($opts.initialPageLoad) {
                                        $opts.initialPageLoad = false;
                                        table.page(parseInt($opts.pageNumber)).draw(false);
                                    }
                                }, 200);

                            }
                            else {
                                //alertify.error("Failed to get reopen data");
                            }
                        });

                break;

        }

    }

    function hideAllCheckBoxes() {

        table.draw();

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes()
                .to$();     // Convert to a jQuery object
            rowx.find("input[type=checkbox]").addClass("hidecheckbox").removeClass("visiblecheckbox"); // hide all boxes.
            rowx.find("input[type=checkbox]").prop("checked", false);
            rowx.removeClass("selected");

        });


    }

    function hideCheckBoxes(inArr) {

        table.draw();

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes()
                .to$();     // Convert to a jQuery object
            rowx.find("input[type=checkbox]").addClass("visiblecheckbox").removeClass("hidecheckbox"); // make all visible
            rowx.find("input[type=checkbox]").prop("checked", false);
            rowx.removeClass("selected");

            // instead of hiding checkboxes, we are hiding the row completely.

            //            var absid = rowx.find(".abstractid").html().trim();
            //            if (_.contains(inArr, Number(absid))) {
            //                rowx.find("input[type=checkbox]").addClass("hidecheckbox").removeClass("visiblecheckbox");
            //                //rowx.addClass("nodisplay");
            //            }
            //            else {
            //                rowx.find("input[type=checkbox]").addClass("visiblecheckbox").removeClass("hidecheckbox");
            //                //rowx.removeClass("nodisplay");
            //            }

        });


    }



});
