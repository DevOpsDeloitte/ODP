$(document).ready(function () {
    var table;
    var cellPadding = 20;
    var debounceVal = 950;
    var debounceValMobile = 2200;
    var util;

    var currentRow = null;
    var currentTR = null;

    $opts.totalRecords = null;
    $opts.totalRecordsSelected = null;
    $opts.allSelected = false;
    $opts.selectedItems = [];
    $opts.unselectedItems = [];
    $opts.generatingExportLink = false;



    // NOTE (TR):
    // This is temporary until I determine if the datatable caches the children or
    // I unify the $opts.selectedItems and $opts.selectedItemChildren arrays
    $opts.selectedItemChildrenCache = [];

    config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role;

    ///////////////////////////////////////////////////////////////////////////////////////////
    // Page and Datatable Events
    function setupTableEvents() {
        var mySearchFn = util.debounce(function () {
            var search = $('input[type=search]').val();

            if (search != null) {
                table.search(search).draw();
            }
        //}, util.getDebounceInterval(debounceVal, debounceValMobile));
        }, ( util.isMobile() ? debounceValMobile : debounceVal) );

        $('input[type=search]').off('keyup.DT input.DT');

        $('input[type=search]').on('keyup', mySearchFn);

        table.on('draw.dt', function () {
            console.log('draw.dt - Redraw occurred at: ' + new Date().getTime());

            var sab = $("#selectAllBox").is(':checked');    // Select/Unselect All Checkbox
            var ab = $("#allBox").is(':checked');           // Expand/Collapse All Checkbox

            childrenRedraw(table.data());

            setTimeout(function () {
                setTableState(sab, ab);
            }, 10);
        });


        table.on('processing.dt', function (e, settings, processing) {
            console.log("on processing.dt " + processing);

            $('div.progressBar').css('display', processing ? 'block' : 'none');

            util.hideBasicCB(processing);

            if (!processing) {
                // show it all
                progressBarReset();
                $("#DTable_paginate").show();
                $("#DTable_info").show();
                $("#DTable").show();
                $("#tableContainer").show();
                //reset submit button
                if ($opts.selectedItems.length == 0) {
                    $("#subButton").removeClass("yes").addClass("no");
                } else {

                }

                $("div#tableContainer").show();

                //util.showActionsInterface();   // NOTE (TR): This and the code below makes no sense.

                switch (config.role) {

                    case "CoderSupervisor":
                        util.showActionsInterface();

                        break;

                    case "ODPSupervisor":
                        util.showActionsInterface();

                        util.showSubActionsInterface();

                        break;

                }

            } else {
                util.disableInterface();

                $("#DTable_paginate").hide();
                $("#DTable_info").hide();
                $("#DTable").hide();
                $("#tableContainer").hide();
            }
        });


        //is this needed?
        table.on('init.dtx', function () {
            console.log("on init.dtx (datable initialized) :: init.dt ::");

            childrenRedraw(table.data());

            $opts.isGridDirty = false;

            if (config.role == "ODPSupervisor") {
                serverCheckForActions();
            }

            //util.enableFilters();
            util.enableInterface();
        });

        table.on('page.dt', function () {
            console.log("on page.dt (page navigation) ::");

            var info = table.page.info();

            if (config.role == "ODPSupervisor") {
                window.location.hash = $opts.filterlist + "|" + $opts.actionlist + "|" + $opts.codingType + "|" + info.page;
            } else {
                window.location.hash = $opts.filterlist + "|" + $opts.codingType + "|" + info.page;
            }

            console.log('Showing page: ', info.page + ' of ' + info.pages);
        });

        // logic to open and close child rows. // 1.3 dynamically load child rows.
        $('#DTable tbody').on('click', 'td.details-control', function (evt) {
            console.log("on click (details row clicked) :: ");

            if ($(this).parent().hasClass("haschildren")) {

                var absid = $(this).parent().find("td.abstractid").html();

                currentTR = $(this).closest('tr');  // Reference to the DOM TR
                currentRow = table.row(currentTR);  // Reference to the Datatable row ??

                if (currentRow.child.isShown()) {
                    // This row is already open - close it
                    currentRow.child.hide();

                    currentTR.removeClass('open').addClass('closed');

                    var rowDataNdx = util.findObjNdxChildCache(absid);

                    if (rowDataNdx > -1) {
                        $opts.selectedItemChildrenCache.splice(rowDataNdx, 1);
                    }
                } else {
                    var data = getDetailChildRow(currentRow, absid);
                }
                console.log('$opts.selectedItemChildrenCache: ', $opts.selectedItemChildrenCache);
            }
        });

        $("#cbBasicOnly").on("click", function (evt) {
            console.log($('#cbBasicOnly').is(":checked"));

            watchBasicOnlyHandler();
        });

        $("#allBox").on("click", function (evt) {
            util.showOpenRows(this.checked);
        });

        $("#selectAllBox").on("click", function (evt) {
            if (this.checked) {
                util.selectAllRows(table);

                $opts.allSelected = true;
            } else {
                util.unselectAllRows(table);

                $opts.allSelected = false;
            }

            doSubmitChecks();
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
            console.log('clicked checkbox');

            var absid = $(this).parent().parent().find("td.abstractid").html();
            var rowIndex = table.row($(this).parent().parent()).index();
            var row = $(this).parents('tr');
            var rowNdx = _.indexOf($opts.selectedItems, absid);

            if ($(this).is(":checked")) {
                $(row).addClass('selected');

                if (rowNdx == -1) {
                    $opts.selectedItems.push(absid);

                    $opts.totalRecordsSelected++;
                }
            } else {
                $(row).removeClass('selected');

                if ($opts.allSelected) {
                    $opts.unselectedItems.push(absid);

                    $opts.totalRecordsSelected--;
                }

                if (rowNdx != -1) {
                    $opts.selectedItems.splice(rowNdx, 1);
                }

                if ($opts.totalRecordsSelected == $opts.totalRecords) {
                    $opts.allSelected = true;
                }
            }

            $("span#recordCount").text($opts.totalRecordsSelected);

            doSubmitChecks();
        });

        $("input#subButton").on("click", function (evt) {
            console.log('Submit button clicked');

            var dataObj = {};

            if ($(this).hasClass("yes")) {
                console.log("submit button click enabled ::");

                util.disableInterface();
                $("div#generalProgressBox").show();

                $(this).addClass("no").removeClass("yes");

                switch ($opts.actionlist) {

                    case "addreportexclude":
                        dataObj = compileDataObject("add");

                        util.ajaxCall("/Evaluation/Handlers/ReportExclude.ashx", "POST", dataObj, function (data, textStatus, jqXHR) {
                            console.log(" add report exclude : " + data);

                            $("div#generalProgressBox").hide();

                            if (data.success == true) {
                                var num = null;

                                if ($opts.allSelected) {
                                    num = $opts.totalRecords - $opts.unselectedItems.length;
                                } else {
                                    num = $opts.selectedItems.length;
                                }
                                alertify.success(num + " " + "Abstract(s) added to report exclude list.");

                                resetSubmitBtnAndCheckboxes();

                                loadFilters();

                                $opts.isGridDirty = true;

                                resetSelections();
                            } else {
                                alertify.error("Failed to add in report exclude list.");
                            }
                        });


                        break;

                    case "removereportexclude":
                        dataObj = compileDataObject("remove");

                        util.ajaxCall("/Evaluation/Handlers/ReportExclude.ashx", "POST", dataObj, function (data, textStatus, jqXHR) {
                            console.log(" removereportexclude - data : " + data);

                            $("div#generalProgressBox").hide();

                            if (data.success == true) {
                                var num = null;

                                if ($opts.allSelected) {
                                    num = $opts.totalRecords - $opts.unselectedItems.length;
                                } else {
                                    num = $opts.selectedItems.length;
                                }
                                alertify.success(num + " " + "Abstract(s) removed from report exclude list.");

                                resetSubmitBtnAndCheckboxes();

                                loadFilters();

                                $opts.isGridDirty = true;

                                resetSelections();
                            } else {
                                alertify.error("Failed to remove from report exclude list.");
                            }
                        });

                        break;

                    case "addreview":
                        dataObj = compileDataObject("add");
console.log('/Evaluation/Handlers/AbstractReview.ashx', dataObj);


                        util.ajaxCall("/Evaluation/Handlers/AbstractReview.ashx", "POST", dataObj, function (data, textStatus, jqXHR) {
                            console.log(" addreview - data : " + data);

                            $("div#generalProgressBox").hide();

                            if (data.success == true) {
                                var num = null;

                                if ($opts.allSelected) {
                                    num = $opts.totalRecords - $opts.unselectedItems.length;
                                } else {
                                    num = $opts.selectedItems.length;
                                }
                                alertify.success(num + " " + "Abstract(s) added to review list.");

                                resetSubmitBtnAndCheckboxes();

                                loadFilters();

                                $opts.isGridDirty = true;

                                resetSelections();
                            } else {
                                alertify.error("Failed to add in review list.");
                            }
                        });

                        break;

                    case "removereview":
                        dataObj = compileDataObject("remove");

                        util.ajaxCall("/Evaluation/Handlers/AbstractReview.ashx", "POST", dataObj, function (data, textStatus, jqXHR) {
                            console.log(" remove - data: " + data);

                            $("div#generalProgressBox").hide();

                            if (data.success == true) {
                                var num = null;

                                if ($opts.allSelected) {
                                    num = $opts.totalRecords - $opts.unselectedItems.length;
                                } else {
                                    num = $opts.selectedItems.length;
                                }
                                alertify.success(num + " " + "Abstract(s) removed from review list.");

                                resetSubmitBtnAndCheckboxes();

                                loadFilters();

                                $opts.isGridDirty = true;

                                resetSelections();
                            } else {
                                alertify.error("Failed to remove from review list.");
                            }
                        });

                        break;

                    case "closeabstract":
                        dataObj = compileDataObject("close");

                        util.ajaxCall("/Evaluation/Handlers/AbstractClose.ashx", "GET", dataObj, function (data, textStatus, jqXHR) {
                            console.log(" closeabstract - data : " + data);

                            $("div#generalProgressBox").hide();
                            if (data.success == true) {
                                var num = null;

                                if ($opts.allSelected) {
                                    num = $opts.totalRecords - $opts.unselectedItems.length;
                                } else {
                                    num = $opts.selectedItems.length;
                                }
                                alertify.success(num + " " + "Abstract(s) have been closed.");

                                resetSubmitBtnAndCheckboxes();

                                loadFilters();

                                $opts.isGridDirty = true;

                                resetSelections();
                            } else {
                                alertify.error("Failed to close abstracts.");
                            }
                        });


                        break;

                    case "reopenabstracts":
                        dataObj = compileDataObject("open");

                        util.ajaxCall("/Evaluation/Handlers/AbstractClose.ashx", "GET", dataObj, function (data, textStatus, jqXHR) {
                            console.log(" reopenabstract - data : " + data);

                            $("div#generalProgressBox").hide();

                            if (data.success == true) {
                                var num = null;

                                if ($opts.allSelected) {
                                    num = $opts.totalRecords - $opts.unselectedItems.length;
                                } else {
                                    num = $opts.selectedItems.length;
                                }
                                alertify.success(num + " " + "Abstract(s) have been Re-opened.");

                                resetSubmitBtnAndCheckboxes();

                                loadFilters();

                                $opts.isGridDirty = true;

                                resetSelections();
                            } else {
                                alertify.error("Failed to Re-open abstracts.");
                            }
                        });


                        break;

                    case "exportabstracts":
                        dataObj = compileDataObject("");

                        $opts.generatingExportLink = true;

                        util.ajaxCall("/Evaluation/Handlers/AbstractExport.ashx", "POST", dataObj, function (data, textStatus, jqXHR) {
                            console.log(" reopen abstract - data : ", data);

                            $("div#generalProgressBox").hide();
                            $("div#downloadProgressBox").show();

                            if (data.success == true) {
                                var num = null;

                                if ($opts.allSelected) {
                                    num = $opts.totalRecords - $opts.unselectedItems.length;
                                } else {
                                    num = $opts.selectedItems.length;
                                }
                                alertify.success(num + " " + "Abstract(s) have been Exported. File being generated.");

                                dataObj = compileDataObject("");

                                // call the second handler
                                util.ajaxCall("/Evaluation/Handlers/GenerateExcelReport.ashx", "POST", dataObj, function (data, textStatus, jqXHR) {
                                    console.log(" generate excel report .ashx - data : " + data);

                                    if (data.success == true) {
                                        $opts.generatingExportLink = false;

                                        $("div#downloadProgressBox").hide();
                                        $("div#downloadLinkBox a").attr("href", data.filePath);
                                        $("div#downloadLinkBox").show();

                                        util.enableInterface();
                                    }
                                });

                                //for exporting abstracts.
                                //var iframe = $("<iframe id='export-frame' src='DataExportHandler.ashx?method=export' />").hide();
                                //$(this).parent().append(iframe);

                                resetSubmitBtnAndCheckboxes();

                                loadFilters();

                                $opts.isGridDirty = true;

                                resetSelections();
                            } else {
                                alertify.error("Failed to Export abstracts.");
                            }
                        });

                        break;
                }

            } else {
                //console.log("not enabled ::");
            }
        });
    }

    function compileDataObject(type) {
        var basicFlag = $opts.codingType == 'basic' ? true : false;

        console.log('basicFlag , $opts.codingType: ', basicFlag, $opts.codingType);

        var dataObj = {type: type, all: $opts.allSelected, guid: window.user.GUID, basic: basicFlag};

        if ($opts.allSelected) {
            $opts.selectedItems = [];
            dataObj.excludeList = $opts.unselectedItems.join();
        } else {
            $opts.unselectedItems = [];
            dataObj.includeList = $opts.selectedItems.join();
        }

        return dataObj;
    }

    function setupPageEvents() {
        $("select#filterlist").change(function () {
            console.log('filterlist changed');

            var str = "";

            util.disableInterface();

            $("select#filterlist option:selected").each(function () {
                str = $(this).val();
                $opts.filterlist = $(this).val();
            });

            if (config.role == "ODPSupervisor") {
                util.actionsManager();

                $opts.actionlist = "selectaction";
            }

            changeFilters();

            $("div#downloadLinkBox a").attr("href", '');
            $("div#downloadLinkBox").hide();
        });

        // Select Action List Event
        $("select#actionlist").change(function () {
            console.log('actionlist changed');

            $("div#downloadLinkBox a").attr("href", '');
            $("div#downloadLinkBox").hide();

            resetSelections();

            $("select#actionlist option:selected").each(function () {
                $opts.actionlist = $(this).val();
            });

            $opts.pageNumber = 0;

            window.location.hash = $opts.filterlist + "|" + $opts.actionlist + "|" + $opts.codingType + "|" + $opts.pageNumber;

            watchActionsHandler();
        });
    }

    // END: Page and Datatable Events
    ///////////////////////////////////////////////////////////////////////////////////////////


    ///////////////////////////////////////////////////////////////////////////////////////////
    // Supporting Methods
    function setTableState(selectAllCheckboxes, expandAllCheckoxes) {
        console.log('setTableState() ::');

        var numRows = table.rows().eq(0).length;
        var cnt = null;

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object
            var abstractId = rowx.find(".abstractid").html()

            if ($opts.actionlist === 'selectaction') {
                $(rowx).removeClass('selected');

                rowx.find("input[type=checkbox]").prop("checked", false);
                rowx.find("input[type=checkbox]").addClass("hidecheckbox");
            } else {
                if ($opts.allSelected) {
                    $("#selectAllBox").prop("checked", true);

                    if ($opts.unselectedItems.length > 0) {
                        var unselectedItemsNdx = _.indexOf($opts.unselectedItems, abstractId);

                        if (unselectedItemsNdx != -1) {
                            $(rowx).removeClass('selected');

                            $(rowx).find("input[type=checkbox]").prop("checked", false);
                        } else {
                            $(rowx).addClass('selected');

                            rowx.find("input[type=checkbox]").prop("checked", true);
                        }
                    } else {
                        $(rowx).addClass('selected');

                        rowx.find("input[type=checkbox]").prop("checked", true);
                    }
                } else {
                    var selectedItemsNdx = _.indexOf($opts.selectedItems, abstractId);

                    if (selectedItemsNdx != -1) {
                        $(rowx).addClass('selected');

                        rowx.find("input[type=checkbox]").prop("checked", true);

                        cnt++;
                    } else {
                        $(rowx).removeClass('selected');

                        $(rowx).find("input[type=checkbox]").prop("checked", false);
                    }
                }
            }

            // Reset previously expanded rows
            if ($opts.selectedItemChildrenCache.length > 0) {
                var rowDataObj = util.findObjInChildCache(abstractId);

                if (rowDataObj) {
                    console.log('getting children of ' + abstractId + ' from cache');
                    this.row(rowx).child(loadChildContainer(abstractId)).show();

                    content = unescape(util.showTableChildRows(rowDataObj.data));
                    $("div#" + abstractId).html(content);

                    currentTR = rowx.closest('tr');
                    currentTR.removeClass('closed').addClass('open');
                }
            }
        });
    }

    function loadChildContainer(abstractid) {
        console.log('loadChildContainer() :: ');
        //load initial child container with loading message.
        return '<div class="loadingAbstractDetail" id="' + abstractid + '"><div class="loader"></div><div class="text">loading...</div></div>';
    }

    // NOTE (TR): This pattern is being used until I Angular-ize the code to use Angular promises
    function callbackGetDetailChildRow(data, textStatus, jqXHR) {
        var rowNdx = null;

        if (data.data[0].ChildRows.length > 0) {
            data = data.data[0];
            console.log('callbackGetDetailChildRow() :: ', data.ChildRows);
            //currentRow.child(loadChildContainer(data.AbstractID)).show();

            content = unescape(util.showTableChildRows(data));
            $("div#" + data.AbstractID).html(content);

            currentTR.removeClass('closed').addClass('open');

            $opts.selectedItemChildrenCache.push({
                'abstractId': data.AbstractID,
                'data': data,
                'displContent': content
            });
        }
    }

    function getDetailChildRow(row, abstractId) {
        console.log('getDetailChildRow() :: ', abstractId);

        var content = "";
        var rowNdx = null;
        var cacheFound = false;
        var url = 'Handlers/AbstractDetail.ashx';
        var type = 'GET';
        var data = {'abstractId': abstractId};

        row.child(loadChildContainer(abstractId)).show();

        if ($opts.selectedItemChildrenCache.length > 0) {
            var rowDataObj = _.find($opts.selectedItemChildrenCache, function (obj) {
                return obj.abstractId === parseInt(abstractId);
            });

            if (rowDataObj == undefined) {
                console.log('calling to get children of ', abstractId);

                util.ajaxCall(url, type, data, callbackGetDetailChildRow);
            } else {
                console.log('getting children of ' + abstractId + ' from cache');

                content = unescape(util.showTableChildRows(rowDataObj.data));
                $("div#" + abstractId).html(content);

                currentTR.removeClass('closed').addClass('open');
            }
        } else {
            console.log('calling to get children of ', abstractId);

            util.ajaxCall(url, type, data, callbackGetDetailChildRow)
        }
    }

    function progressBarReset() {
        console.log('progressBarReset() ::');

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

        util.enableInterface();
    }

    function loadFilters() {
        $.ajax({
                type: "GET",
                url: "/Evaluation/Handlers/Filters.ashx?role=" + config.role,
                dataType: 'json',
                data: {guid: window.user.GUID}
            })
            .done(function (data) {
                if (data.opts.length > 0) {
                    //console.log(" filters received..");
                    $("select#filterlist").empty();

                    if ($opts.hashExists) {
                        //                              if (window.location.hash.replace("#", "") != "") {
                        //                                  var locationHash = window.location.hash.replace("#", "").split("|");
                        //                                  var filterVal = locationHash[0], actionVal = locationHash[1];
                        //                                  $opts.pageNumber = locationHash[2];
                        if ($opts.initialPageLoad) {
                            $opts.lastfilterSelection = $opts.filterHash;
                            //$opts.lastfilterSelection = filterVal;
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
            });
    }

    function changeFilters() {
        config.baseURL = "/Evaluation/Handlers/Abstracts.ashx?role=" + config.role + "&filter=" + $opts.filterlist + "&codingType=" + $opts.codingType;
        console.log("changeFilters() :: ", config.baseURL);

        table.ajax.url(config.baseURL);

        $opts.lastfilterSelection = $opts.filterlist;

        $("div#downloadLinkBox").hide();

        util.assignPageTitle($opts.filterlist);

        if (config.role == "ODPSupervisor") {
            if ($opts.initialPageLoad) {
                window.location.hash = $opts.filterlist + "|" + $opts.actionlist + "|" + $opts.codingType + "|" + $opts.pageNumber;
            } else {
                window.location.hash = $opts.filterlist + "|" + $opts.actionlist + "|" + $opts.codingType + "|" + "0";
            }
        } else {
            if ($opts.initialPageLoad) {
                window.location.hash = $opts.filterlist + "|" + $opts.codingType + "|" + $opts.pageNumber;
            } else {
                window.location.hash = $opts.filterlist + "|" + $opts.codingType + "|" + "0";
            }
        }
        $opts.hideboxes = [];

        if (!$opts.initialPageLoad) {
            table.ajax.reload(function (json) {
                childrenRedraw(table.data());
                if (config.role == "ODPSupervisor") {
                    // change check of action checkboxes happens here - when Filter re-loads.. same block repeated table.init.
                    serverCheckForActions();

                }
                else {
                    // for all other roles
                    if ($opts.initialPageLoad) {
                        if ($opts.hashExists) {
                            $opts.initialPageLoad = false;
                            table.page(parseInt($opts.pageNumber)).draw(false);
                        }
                    }


                }
                $opts.isGridDirty = false;

                util.enableInterface();

                clearSubmitBtnAndCheckboxes();
            });
        } else {
            $opts.initialPageLoad = false;
        }

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
                    $opts.initialPageLoad = false;
                    table.page(parseInt($opts.pageNumber)).draw(false);
                }
                break;
            default:
                $("th.col_select").children().show();

                break;
        }
    }

    function doSubmitChecks() {
        console.log('doSubmitChecks() ::');

        $opts.actionlist = $opts.actionlist ? $opts.actionlist : $("select#actionlist option:selected").val();
        //$opts.actionlist = $("select#actionlist option:selected").val();

        if ($opts.selectedItems.length > 0 && $opts.actionlist != "") {
            $("#subButton").removeClass("no").addClass("yes");
        } else {
            $("#subButton").removeClass("yes").addClass("no");
        }

        updateRecordsSelectedText();
    }

    // called on change of filter and action ::
    function clearSubmitBtnAndCheckboxes() {
        console.log('clearSubmitBtnAndCheckboxes() :: ');

        $opts.selectedItems = [];
        $opts.hiderowItems = [];
        $opts.selectedItemChildrenCache = [];

        $("#subButton").removeClass("yes").addClass("no");
        $("#selectAllBox").prop("checked", false);

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object

            rowx.find("input[type=checkbox]").prop("checked", false);
        });

        updateRecordsSelectedText();
    }

    //called after action submitted ::
    function resetSubmitBtnAndCheckboxes() {
        util.removeRowsV2(table, $opts.hiderowItems);

        clearSubmitBtnAndCheckboxes();

        return;
    }

    function resetSelections() {
        $opts.allSelected = false;
        $opts.selectedItems = [];
        $opts.unselectedItems = [];
        $opts.totalRecordsSelected = null;
    }

    function updateRecordsSelectedText() {
        if (!$opts.allSelected) {
            $("span#recordCount").text($opts.selectedItems.length);
        }

        if ($opts.selectedItems.length > 0 && config.role == "ODPSupervisor") {
            $("div#selectionsBox").removeClass("hidden");
        } else {
            $("div#selectionsBox").addClass("hidden");
        }
    }

    // in 1.3 we will not be pre-loading row data. childrenRedraw will add parent classes or nochildren/haschildren and return.
    function childrenRedraw(tdata) {
        util.setRows(tdata);

        table.rows().eq(0).each(function (rowIdx, val) {
            var childrows = util.getTableChildRowsV2(rowIdx);

            var rowx = table.row(rowIdx).nodes().to$(); // Convert to a jQuery object
            var kappaCount = table.row(rowIdx).data().KappaCount;

            if (kappaCount <= 1) {
                rowx.addClass('nochildren');
                rowx.find("td.details-control").addClass('nodisplay');
            } else {
                rowx.addClass('haschildren');
            }

            return;
        });
    }

    function doAllSubmitCheck(flag) {
        $opts.hiderowItems = [];

        var filteredRows = table.$('tr', {"filter": "applied"});
        var filteredRowsAbstractIDs = [];

        $(filteredRows).each(function (i, val) {
            //console.log(val);
            //console.log($(val).find(".abstractid").html());
            filteredRowsAbstractIDs.push($(val).find(".abstractid").html());
        });

        console.log(filteredRowsAbstractIDs);

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object

            if (flag) {
                if (!rowx.find("input[type=checkbox].hidecheckbox")) {
                    //console.log(rowx.find(".abstractid").html());
                    if (_.contains(filteredRowsAbstractIDs, rowx.find(".abstractid").html())) {
                        $opts.selectedItems.push(rowx.find(".abstractid").html());
                        $opts.hiderowItems.push(rowIdx);
                        rowx.addClass("selected");
                    }
                }
            } else {
                rowx.removeClass("selected");
            }
        });

        doSubmitChecks();
    }

    function turnOffSelectAll() {
        console.log('turnOffSelectAll() ::');

        if ($("#selectAllBox").is(":checked")) {
            $("#selectAllBox").prop("checked", false);
        }
    }

    function watchBasicOnlyHandler() {
        console.log('watchBasicOnlyHandler() :: ' + $opts.codingType);

        var isCheckedBasicOnly = $('#cbBasicOnly').is(":checked");

        if (isCheckedBasicOnly) {
            $opts.codingType = 'basic';
        } else {
            $opts.codingType = 'all';
        }

        $opts.pageNumber = 0;

        if (config.role == "ODPSupervisor") {
            window.location.hash = $opts.filterlist + "|" + $opts.actionlist + "|" + $opts.codingType + "|" + $opts.pageNumber;
        } else {
            window.location.hash = $opts.filterlist + "|" + $opts.codingType + "|" + $opts.pageNumber;
        }

        $("div#downloadLinkBox").hide();

        changeFilters();

        turnOffSelectAll();

        resetSelections();
    }

    function watchActionsHandler() {
        console.log(" watchActionsHandler() :: " + $opts.actionlist);
        $("div#downloadLinkBox").hide();

        switch ($opts.actionlist) {

            case "reopenabstracts":
                $("th.col_select").children().show();
                if ($opts.isGridDirty) {

                    util.disableFilters();
                    //util.actionsManager();
                    util.disableInterface();
                    changeFilters();
                    clearSubmitBtnAndCheckboxes();

                } else {
                    reopenListCheck();

                    clearSubmitBtnAndCheckboxes();
                }

                break;

            case "selectaction":
                $("th.col_select").children().hide();

                $opts.hideboxes = [];

                reloadForAction(null);

                clearSubmitBtnAndCheckboxes();

                hideAllCheckBoxes();

                break;

            default:
                $("th.col_select").children().show();

                reloadForAction($opts.actionlist);

                showAllCheckBoxes();

                break;

        }
    }

    function reloadForAction(type) {
        console.log('reloadForAction(type) ::');
        if ($opts.isGridDirty) {
            util.disableFilters();
            //util.actionsManager();
            util.disableInterface();
            changeFilters();
            clearSubmitBtnAndCheckboxes();

        } else {
            table.page(parseInt($opts.pageNumber)).draw(false);

            clearSubmitBtnAndCheckboxes();
        }
    }

    function reopenListCheck() {
        console.log('reopenListCheck() ::');
        $.ajax({
                type: "GET",
                url: "/Evaluation/Handlers/AbstractsReopen.ashx",
                dataType: 'json',
                data: {guid: window.user.GUID}
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
        console.log('ListCheck(type) ::');
        switch (type) {

            case "":
                break;
            default:
                $.ajax({
                        type: "GET",
                        url: "/Evaluation/Handlers/AbstractsListCheck.ashx",
                        dataType: 'json',
                        data: {guid: window.user.GUID, action: type}
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
        console.log('hideAllCheckBoxes() :: ');
        // NOTE (TR): Why ???
        // table.draw();

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object

            rowx.find("input[type=checkbox]").addClass("hidecheckbox") // hide all boxes.
            rowx.find("input[type=checkbox]").prop("checked", false);
            rowx.removeClass("selected");
        });
    }

    function showAllCheckBoxes() {
        console.log('showAllCheckBoxes() :: ');
        // NOTE (TR): Why ??
        // table.draw();

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object

            rowx.find("input[type=checkbox]").removeClass("hidecheckbox") // hide all boxes.
            rowx.find("input[type=checkbox]").prop("checked", false);
            rowx.removeClass("selected");
        });
    }

    function hideCheckBoxes(inArr) {
        console.log('hideCheckBoxes() :: ');
        table.draw();

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object

            rowx.find("input[type=checkbox]").addClass("visiblecheckbox").removeClass("hidecheckbox"); // make all visible
            rowx.find("input[type=checkbox]").prop("checked", false);
            rowx.removeClass("selected");
        });
    }

    // END: Supporting Methods
    ///////////////////////////////////////////////////////////////////////////////////////////


    ///////////////////////////////////////////////////////////////////////////////////////////
    // Initialization Methods
    function retrievePageHash() {
        console.log('retrievePageHash() :: ');
        if (window.location.hash.replace("#", "") != "") {
            var locationHash = window.location.hash.replace("#", "").split("|");

            $opts.filterHash = locationHash[0];
            $opts.actionHash = locationHash[1];
            $opts.codingType = locationHash[2];
            $opts.pageNumber = locationHash[3];

            $opts.filterlist = $opts.filterHash;
            $opts.hashExists = true;
        }
    }

    function filtersManager(filter) {
        // calling syntax = ([filter selected], [callback method])
        util.filtersManager(filter, loadFilters);
    }

    function InitializeTable(inData) {
        console.log("invoking InitializeTable() :: ");

        $("div.progressBar").show();

        util.hideBasicCB(true);

        // Datatable Definition
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
                //console.log('fnRowCallback');
            },
            "fnDrawCallback": function (settings) {
                //console.log('fnDrawCallback: ', settings);

                $opts.totalRecords = settings._iRecordsTotal;
                if($opts.totalRecordsSelected == null){
                    $opts.totalRecordsSelected = 0;
                }
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
                            return "&mdash;";
                            return "&nbsp;";
                            return data.length == 0 ? "&nbsp;" : data.replace(", E7F6", "").replace("E7F6", "");
                        }
                        else {
                            return data.length == 0 ? "&nbsp;" : data;
                        }

                    },
                    "targets": 7
                },

                {
                    //mask out odp staff role kappa values
                    "render": function (data, type, row) {
                        if (config.role == "ODPStaff") {
                            return "&mdash;";
                        }
                        else {
                            return data;
                        }

                    },
                    "targets": [8, 9, 10, 11, 12, 13, 14, 15, 16, 17]
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
                        if (util.isMobile()) {
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
                    "targets": 18 //date column
                }
            ],
            // searchDelay is only applicable for client side. see debounce function for server side delays.
            "searchDelay": 1000,
            "processing": true,
            "serverSide": true,
            "ajax": {
                "url": config.baseURL + "&filter=" + $opts.filterlist + "&codingType=" + $opts.codingType,
                "type": "POST",
                "data": function (data) {
                    if (config.role == "ODPSupervisor") {
                        data.action = $opts.actionlist;
                    }
                }
            },
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
                { "data": "AbstractID", "class": "abstractid hide_column" },
                { "data": "ApplicationID" },
                { "data": "StatusDate" },
                { "data": "PIProjectLeader" },
                { "data": "ProjectTitle" },
                { "data": "Flags" },
                { "data": "A1" },
                { "data": "A2" },
                { "data": "A3" },
                { "data": "A4" },
                { "data": "B", "class": "hide_column" },
                { "data": "C", "class": "hide_column" },
                { "data": "D" },
                { "data": "E" },
                { "data": "F" },
                { "data": "G" },
                { "data": "LastExportDate" }
            ]
        });
        // END: Datatable Definition

console.log('table: ', table);

        setupTableEvents();
    }

    function init() {
        util = new Utility();

        var target = document.getElementById('spinner');                        // NOTE (TR): Obsolete ???
        var downloadSpin = document.getElementById('downloadSpin');             // NOTE (TR): Obsolete ???
        var spinner = new Spinner(spinneropts).spin(target);                    // NOTE (TR): Obsolete ???
        var spinnerdownloadSpin = new Spinner(spinneropts2).spin(downloadSpin); // NOTE (TR): Obsolete ???

        util.disableInterface();

        util.hideBasicCB(true);

        $opts.codingType = $opts.codingType ? $opts.codingType : "all";

        // set search placeholder
        $('.dataTables_filter input').attr('placeholder', 'Search');

        // ODPStaff role starts out with review list;
        if (config.role == "ODPStaff") {
            $opts.lastfilterSelection = "review";
        }

        retrievePageHash(); //Init

        filtersManager($opts.filterlist);   //Init

        if (config.role == "ODPSupervisor") {
            util.actionsManager();
        }

        if($opts.codingType == "basic") {
            $("input#cbBasicOnly").prop( "checked", true );
        }

        util.disableFilters();

        InitializeTable();

        setupPageEvents();
    }
    // END: Initialization Methods
    ///////////////////////////////////////////////////////////////////////////////////////////

    init();

});
