
var config = {};
var $opts = { selectedItems : [], xy : "", filterlist : "", actionlist : "", hiderowItems : [], lastfilterSelection : "", isGridDirty: false, openItems : [], initialPageLoad: true, hideboxes : [], pageNumber : 0, hashExists : false, actionHash : "", filterHash : "" };
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

$(function () {
    $(".meter > span").each(function () {
        $(this)
            .data("origWidth", $(this).width())
            .width(0)
            .animate({
                width: $(this).data("origWidth")
            }, 30000);
    });
});


function Utility() {
    // private member
    //    var Rows = data;
    //    this.RowsP = data;
    var Rows = null;
    var utils = this;

    this.RowsP = null;

    // Public Functions
    this.isMobile = function() {
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
    };

    this.getDebounceInterval = function (desktop, mobile) {

        if (this.isMobile()) {
            return mobile;
        }
        else {
            return desktop;
        }
    };

    this.ajaxCall = function(url, type, data, callback) {
        var newURL = url + "?role=" + config.role + "&filter=" + $opts.filterlist + "&action=" + $opts.actionlist;

        $.ajax(newURL, {
            type: type,
            data: data,
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                callback(data, textStatus, jqXHR);
            }
        });
    };

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

    this.selectAllRows = function (table) {
        console.log('utils.selectAllRows() :: ');

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object
            var abstractId = rowx.find(".abstractid").html()

            if(_.indexOf($opts.selectedItems, abstractId) == -1) {
                $opts.selectedItems.push(rowx.find(".abstractid").html());
            }

            rowx.addClass("selected");
            rowx.find("input[type=checkbox]").prop("checked", true);
        });

        $opts.totalRecordsSelected = $opts.totalRecords;
        $("span#recordCount").text($opts.totalRecordsSelected);
        //$("span#recordCount").text($opts.selectedItems.length);
    };

    this.unselectAllRows = function (table) {
        console.log('utils.unselectAllRows() :: ');

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object
            var abstractId = rowx.find(".abstractid").html()

            var rowNdx = _.indexOf($opts.selectedItems, abstractId);
            $opts.selectedItems.splice(rowNdx, 1);

            rowx.removeClass("selected");
            rowx.find("input[type=checkbox]").prop("checked", false);
        });

        $opts.allSelected = false;
        $opts.selectedItems = [];
        $opts.unselectedItems = [];
        $opts.totalRecordsSelected = null;

        $("span#recordCount").text("0");
    };

    this.checkIfAllBoxesChecked = function (table) {
        console.log('utils.checkIfAllBoxesChecked() :: ');

        if($opts.actionlist !== 'selectaction') {
            if($opts.allSelected) {
                $("#selectAllBox").prop("checked", true);

                if($opts.unselectedItems.length > 0){
                    table.rows().eq(0).each(function (rowIdx, val) {
                        var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object
                        var abstractId = rowx.find(".abstractid").html()

                        if(_.indexOf($opts.unselectedItems, abstractId) > -1) {
                            rowx.addClass("selected");
                            rowx.find("input[type=checkbox]").prop("checked", true);
                        }
                    });
                } else {
                    table.rows().eq(0).each(function (rowIdx, val) {
                        rowx.addClass("selected");
                        rowx.find("input[type=checkbox]").prop("checked", true);
                    });
                }
            } else {
                $("#selectAllBox").prop("checked", false);
            }


            //var allselected = true;
            //
            //table.rows().eq(0).each(function (rowIdx, val) {
            //    var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object
            //    if (!rowx.find("input[type=checkbox]").parents("tr").hasClass("selected")) {
            //        allselected = false;
            //
            //        return;
            //    }
            //});

            //if (allselected) {
            //    $("#selectAllBox").prop("checked", true);
            //} else {
            //    $("#selectAllBox").prop("checked", false);
            //}
        }
    };

    this.debounce = function(func, wait, immediate) {
        var timeout;
        return function() {
            var context = this, args = arguments;
            var later = function() {
                timeout = null;
                if (!immediate) func.apply(context, args);
            };
            var callNow = immediate && !timeout;
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
            if (callNow) func.apply(context, args);
        };
    };

    this.removeRowsV2 = function (table, hiderowItems) {
        table
            .rows('.selected')
            .remove()
            .draw();
    };

    this.showOpenRows = function (flag) {
        if (flag) {
            $("#DTable tr[role='row'].haschildren").removeClass("closed").addClass("open");
            $("#DTable tr.child").removeClass("hide").addClass("show");
        }
        else {

            $("#DTable tr[role='row'].haschildren").removeClass("open").addClass("closed");
            $("#DTable tr.child").removeClass("show").addClass("hide");
        }

        $opts.openItems = [];
    };

    this.getTableChildRows = function (id) {
        var rowData = Rows[id];
        var childTable = '';
        for (var i = 0; i < rowData.ChildRows.length; i++) {
            var ctRow = '<tr>' +
                '<td class="ccol" style="width:' + ($($("#DTable th")[0]).outerWidth() - cellPadding).toString() + 'px" >' + '&nbsp;' + '</td>' + // Col 1
                '<td class="ccol" style="width:' + ($($("#DTable th")[1]).outerWidth() - cellPadding).toString() + 'px" >' + '&nbsp;' + '</td>' + // Col 1a
                '<td class="ccol" style="width:' + ($($("#DTable th")[2]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].AbstractID + '</td>' + // Col 2
                '<td class="ccol" style="width:' + ($($("#DTable th")[3]).outerWidth() - cellPadding).toString() + 'px" >' + '&nbsp;' + '</td>' + // Col 3
                '<td class="ccol" style="width:' + ($($("#DTable th")[4]).outerWidth() - cellPadding).toString() + 'px" >' + '&nbsp;' + '</td>' + // Col 4
                '<td class="ccol" style="width:' + ($($("#DTable th")[5]).outerWidth() - cellPadding).toString() + 'px" >' + '&nbsp;' + '</td>' + // Col 4
                '<td class="ccol" style="width:' + ($($("#DTable th")[6]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].ProjectTitle + '</td>' + // Col 3
                '<td class="ccol" style="width:' + ($($("#DTable th")[7]).outerWidth() - cellPadding).toString() + 'px" >' + this.coalesceCol(rowData.ChildRows[i].Flags) + '</td>' + // Col 4
                '<td class="ccol" style="width:' + ($($("#DTable th")[8]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].A1 + '</td>' + // Col 5
                '<td class="ccol" style="width:' + ($($("#DTable th")[9]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].A2 + '</td>' + // Col 6
                '<td class="ccol" style="width:' + ($($("#DTable th")[10]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].A3 + '</td>' + // Col 7
                '<td class="ccol" style="width:' + ($($("#DTable th")[10]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].A4 + '</td>' + // Col 8
                '<td class="ccol" style="width:' + ($($("#DTable th")[11]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].B + '</td>' + // Col 9
                '<td class="ccol" style="width:' + ($($("#DTable th")[12]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].C + '</td>' + // Col 9
                '<td class="ccol" style="width:' + ($($("#DTable th")[13]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].D + '</td>' + // Col 10
                '<td class="ccol" style="width:' + ($($("#DTable th")[14]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].E + '</td>' + // Col 11
                '<td class="ccol" style="width:' + ($($("#DTable th")[15]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].F + '</td>' + // Col 12
                '<td class="ccol" style="width:' + ($($("#DTable th")[16]).outerWidth() - cellPadding).toString() + 'px" >' + this.coalesceCol(rowData.ChildRows[i].LastExportDate) + '</td>' + // Col 13

                '</tr>'
            childTable = childTable + ctRow;
        }
        if (rowData.ChildRows.length > 0) {
            childTable = '<table>' + childTable + '</table>';
        }

        return childTable;
    };

    this.getTableChildRowsV2 = function (id) {
        var rowData = Rows[id];
        var childTable = '';
        for (var i = 0; i < rowData.ChildRows.length; i++) {
            var ctRow = '<tr>' +
                '<td class="ccol col_select">' + '&nbsp;' + '</td>' + // Col 1
                '<td class="ccol col_openclose">' + '&nbsp;' + '</td>' + // Col 1a
                '<td class="ccol col_abstractid">' + rowData.ChildRows[i].AbstractID + '</td>' + // Col 2
                '<td class="ccol col_applicationid">' + '&nbsp;' + '</td>' + // Col 3
                '<td class="ccol col_statusdate">' + '&nbsp;' + '</td>' + // Col 4
                '<td class="ccol col_piname">' + '&nbsp;' + '</td>' + // Col 4
                '<td class="ccol col_title"><div class="titlebox">' + rowData.ChildRows[i].ProjectTitle + '</div></td>' + // Col 3
                '<td class="ccol col_flags">' + this.coalesceCol(rowData.ChildRows[i].Flags) + '</td>' + // Col 4
                '<td class="ccol col_kappa">' + rowData.ChildRows[i].A1 + '</td>' + // Col 5
                '<td class="ccol col_kappa">' + rowData.ChildRows[i].A2 + '</td>' + // Col 6
                '<td class="ccol col_kappa">' + rowData.ChildRows[i].A3 + '</td>' + // Col 7
                '<td class="ccol col_kappa">' + rowData.ChildRows[i].A4 + '</td>' + // Col 8
                '<td class="ccol col_kappa">' + rowData.ChildRows[i].B + '</td>' + // Col 9
                '<td class="ccol col_kappa">' + rowData.ChildRows[i].C + '</td>' + // Col 10
                '<td class="ccol col_kappa" >' + rowData.ChildRows[i].D + '</td>' + // Col 11
                '<td class="ccol col_kappa">' + rowData.ChildRows[i].E + '</td>' + // Col 12
                '<td class="ccol col_kappa">' + rowData.ChildRows[i].F + '</td>' + // Col 13
                '<td class="ccol col_exportdate">' + this.coalesceCol(rowData.ChildRows[i].LastExportDate) + '</td>' + // Col 14

                '</tr>'
            childTable = childTable + ctRow;
        }
        if (rowData.ChildRows.length > 0) {
            childTable = '<table class="childtable">' + childTable + '</table>';
        }

        return childTable;
    };

    this.hideE7F6 = function (inVal) {

        if (config.role == "ODPStaff") {
            return "&mdash;";
            return inVal.replace(", E7F6", "").replace("E7F6", "");
        }
        else {
            return inVal;
        }

    };

    this.MaskKappa = function (inVal) {

        if (config.role == "ODPStaff") {
            return "&mdash;";
        }
        else {
            return inVal;
        }

    };

    // NOTE (TR) Use this method
    this.showTableChildRows = function (parentRowData) {
        console.log('util.showTableChildRows() :: ');
        var rowData = parentRowData;
        var childTable = '';

        for (var i = 0; i < rowData.ChildRows.length; i++) {
            var ctRow = '<tr>' +
                '<td class="ccol col_select">' + '&nbsp;' + '</td>' + // Col 1
                '<td class="ccol col_openclose">' + '&nbsp;' + '</td>' + // Col 1a
                //'<td class="ccol col_abstractid">' + rowData.ChildRows[i].AbstractID + '</td>' + // Col 2
                '<td class="ccol col_applicationid">' + '&nbsp;' + '</td>' + // Col 3
                '<td class="ccol col_projectnumber">' + '&nbsp;' + '</td>' + // Col 3

                '<td class="ccol col_fy">' + '&nbsp;' + '</td>' + // Col 3
                '<td class="ccol col_type">' + '&nbsp;' + '</td>' + // Col 3
                '<td class="ccol col_mechanism">' + '&nbsp;' + '</td>' + // Col 3

                '<td class="ccol col_statusdate">' + '&nbsp;' + '</td>' + // Col 4
                '<td class="ccol col_piname">' + '&nbsp;' + '</td>' + // Col 4
                '<td class="ccol col_title"><div class="titlebox">' + rowData.ChildRows[i].ProjectTitle + '</div></td>' + // Col 3
                '<td class="ccol col_flags">' + this.hideE7F6(this.coalesceCol(rowData.ChildRows[i].Flags)) + '</td>' + // Col 4
                '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].A1) + '</td>' + // Col 5
                '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].A2) + '</td>' + // Col 6
                '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].A3) + '</td>' + // Col 7
                '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].A4) + '</td>' + // Col 8
                //'<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].B) + '</td>' + // Col 9
                //'<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].C) + '</td>' + // Col 10
                '<td class="ccol col_kappa" >' + this.MaskKappa(rowData.ChildRows[i].D) + '</td>' + // Col 11
                '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].E) + '</td>' + // Col 12
                '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].F) + '</td>' + // Col 13
                '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].G) + '</td>' + // Col 14
                //'<td class="ccol col_exportdate">' + this.coalesceCol(rowData.ChildRows[i].LastExportDate) + '</td>' + // Col 15

                '</tr>';

            childTable = childTable + ctRow;
        }
        if (rowData.ChildRows.length > 0) {
            childTable = '<table class="childtable">' + childTable + '</table>';
        }

        return childTable;
    };

    this.findObjInChildCache = function(abstractId) {
        var rowDataObj = _.find($opts.selectedItemChildrenCache, function(obj) {
            return obj.abstractId === parseInt(abstractId);
        });

        return rowDataObj;
    };

    this.findObjNdxChildCache = function(abstractId) {

        var rowDataNdx;
        _.find($opts.selectedItemChildrenCache, function(obj, ndx) {
            if(obj.abstractId === parseInt(abstractId)){
                rowDataNdx = ndx;
                return true;
            }
        });

        return rowDataNdx;
    };

    /* *************************************** */
    /* Refactoring Aditions                    */
    /* *************************************** */

    /* DOM Manipulation Methods ************** */
    function setPageTitle(title) {
        $("#pagetitlebox span").text(title);
    }

    function hideSubActionsInterface() {
        $(".subactions.interface").hide();
        $("#selectAllBox").addClass("hidecheckbox");
    }

    function showFiltersInterface() {
        $(".filters.interface").show();
    }

    function hideActionsInterface() {
        $(".actions.interface").hide();
    }


    this.showActionsInterface = function() {
        $(".actions.interface").show();
    };

    this.showSubActionsInterface = function() {
        $(".subactions.interface").show();
        $("#selectAllBox").removeClass("hidecheckbox");
    };

    this.disableFilters = function() {
        $("select#filterlist").attr("disabled", true);
    };

    this.enableFilters = function() {
        $("select#filterlist").attr("disabled", false);
    };

    this.hideFiltersInterface = function() {
        $(".filters.interface").hide();

    };

    this.disableInterface = function() {
        console.log('disableInterface() ::');

        $("select#filterlist").attr("disabled", true);
        $("select#actionlist").attr("disabled", true);

        $("#submitLinkBox").hide();

        utils.hideBasicCB(true);

        $("input").attr("disabled", true);

        $("div#downloadLinkBox").hide();
    };

    this.enableInterface = function() {
        console.log('enableInterface() ::');

        if (!$opts.generatingExportLink) {
            $("select#filterlist").attr("disabled", false);
            $("select#actionlist").attr("disabled", false);

            if (config.role == "ODPSupervisor") {
                $("#submitLinkBox").show();
            }

            utils.hideBasicCB(false);

            $("input").attr("disabled", false);
        }

        if ($("div#downloadLinkBox a").attr("href")) {
            $("div#downloadLinkBox").show();
        }
    };

    this.hideBasicCB = function(mode) {
        $("input#cbBasicOnly").parent().css('display', mode ? 'none' : 'inline');
    };

    this.actionsManager = function() {
        console.log('actionsManager() ::');

        $("select#actionlist").empty();

        switch ($opts.filterlist) {

            case "review":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="removereview">Remove From Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "removereview";
                $opts.actionlist = "selectaction";

                break;
            case "reviewuncoded":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="removereview">Remove From Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "removereview";
                $opts.actionlist = "selectaction";

                break;
            case "codercompleted":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";

                break;
            case "activeabstracts":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";

                break;
            case "odpcompleted":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";

                break;
            case "odpcompletedwonotes":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";

                break;
            case "closed":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";

                break;
            case "exported":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";

                break;
            case "reportexclude":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="removereportexclude">Remove Report Exclude</option>');
                $opts.actionlist = "selectaction";

                break;

            case "default": // default = all
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "selectaction";

                break;

            case "all":
                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="reopenabstracts">Reopen Abstracts</option>');
                $("select#actionlist").append('<option value="exportabstracts">Export Abstracts</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "selectaction";

                break;

            default: // currently same as odpcompleted, being the default.

                $("select#actionlist").append('<option selected="selected" value="selectaction">Select Action</option>');
                $("select#actionlist").append('<option value="addreview">Add to Review List</option>');
                $("select#actionlist").append('<option value="closeabstract">Close Abstracts</option>');
                $("select#actionlist").append('<option value="addreportexclude">Add Report Exclude</option>');
                $opts.actionlist = "addreview";
                $opts.actionlist = "selectaction";

                break;
        }
        //Intercept if first load to check for hash location based -- previous action.

        if ($opts.hashExists) {
            //if (window.location.hash.replace("#", "") != "") {
            //var locationHash = window.location.hash.replace("#", "").split("|");
            var actionVal = $opts.actionHash;
            //$opts.pageNumber = locationHash[2];
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

    };

    // sets up the filters and does the load filters.
    this.filtersManager = function(filter, cb) {
        console.log('filtersManager() :: ' + config.role);

        $("select#filterlist").empty();

        hideSubActionsInterface();

        utils.assignPageTitle(filter);

        showFiltersInterface();

        hideActionsInterface();

        switch (config.role) {
            case "ODPSupervisor":
            case "ODPStaff":
            case "Admin":

                hideSubActionsInterface();

                break;
        }

        cb();   //should call loadFilters();
    };

    this.assignPageTitle = function(filter) {
        var title, pageTitle;

        var pageTitles = {
            "review": "View Review List",
            "reviewuncoded": "View Review List Uncoded",
            "open": "View Open Abstracts",
            "coded": "View Coded Abstracts",
            "closed": "View Closed Abstracts",
            "odpcompleted": "View ODP Completed Abstracts",
            "codercompleted": "View Coder Completed Abstracts",
            "activeabstracts": "View Active Abstracts",
            "exported": "View Exported Abstracts",
            "odpcompletedwonotes": "View ODP Completed Without Notes Abstracts",
            "reportexclude": "View Report Excluded List",
            "default": "View Abstracts"
        };

        pageTitle = pageTitles[filter];
        filter = pageTitle ? pageTitle : "default";

        setPageTitle(pageTitles[filter]);
    }

}