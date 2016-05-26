
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
    this.RowsP = null;

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
    }

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

    this.selectAllRows = function (table) {
console.log('utils.selectAllRows() :: ');
        // $opts.selectedItems = [];

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object
            var abstractId = rowx.find(".abstractid").html()

            $opts.selectedItems.push(rowx.find(".abstractid").html());

            rowx.addClass("selected");
            rowx.find("input[type=checkbox]").prop("checked", true);
        });
    };

    this.unselectAllRows = function (table) {
console.log('utils.unselectAllRows() :: ');
        // $opts.selectedItems = [];

        table.rows().eq(0).each(function (rowIdx, val) {
            var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object
            var abstractId = rowx.find(".abstractid").html()

            var rowNdx = _.indexOf($opts.selectedItems, abstractId);
            $opts.selectedItems.splice(rowNdx, 1);

            rowx.removeClass("selected");
            rowx.find("input[type=checkbox]").prop("checked", false);
        });
    };

    this.checkIfAllBoxesChecked = function (table) {
console.log('utils.checkIfAllBoxesChecked() :: ');
        if($opts.actionlist !== 'selectaction') {
            var allselected = true;

            table.rows().eq(0).each(function (rowIdx, val) {
                var rowx = table.row(rowIdx).nodes().to$();     // Convert to a jQuery object
                if (!rowx.find("input[type=checkbox]").parents("tr").hasClass("selected")) {
                    allselected = false;

                    return;
                }
            });

            if (allselected) {
                $("#selectAllBox").prop("checked", true);
            } else {
                $("#selectAllBox").prop("checked", false);
            }
        }
    }

    this.removeRows = function (hideItems) {

        for (var i = 0; i < hideItems.length; i++) {
            var ele = $("table td:contains('" + hideItems[i] + "')");
            table
                .row($(ele).parents('tr'))
                .remove()
                .draw();
        }

    };

    this.removeRowsV2 = function (hiderowItems) {

//        for (var i = 0; i < hiderowItems.length; i++) {
//            console.log(" remove row :: " + hiderowItems[i]);
//            table.row(hiderowItems[i]).remove();
//        }

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
                    '<td class="ccol" style="width:' + ($($("#DTable th")[11]).outerWidth() - cellPadding).toString() + 'px" >' + rowData.ChildRows[i].B + '</td>' + // Col 8
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
                    '<td class="ccol col_kappa">' + rowData.ChildRows[i].B + '</td>' + // Col 8
                    '<td class="ccol col_kappa">' + rowData.ChildRows[i].C + '</td>' + // Col 9
                    '<td class="ccol col_kappa" >' + rowData.ChildRows[i].D + '</td>' + // Col 10
                    '<td class="ccol col_kappa">' + rowData.ChildRows[i].E + '</td>' + // Col 11
                    '<td class="ccol col_kappa">' + rowData.ChildRows[i].F + '</td>' + // Col 12
                    '<td class="ccol col_exportdate">' + this.coalesceCol(rowData.ChildRows[i].LastExportDate) + '</td>' + // Col 13

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


    this.getTableChildRowsV3 = function (parentRowData) {
        var rowData = parentRowData;
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
                    '<td class="ccol col_flags">' + this.hideE7F6(this.coalesceCol(rowData.ChildRows[i].Flags)) + '</td>' + // Col 4
                    '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].A1) + '</td>' + // Col 5
                    '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].A2) + '</td>' + // Col 6
                    '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].A3) + '</td>' + // Col 7
                    '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].B) + '</td>' + // Col 8
                    '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].C) + '</td>' + // Col 9
                    '<td class="ccol col_kappa" >' + this.MaskKappa(rowData.ChildRows[i].D) + '</td>' + // Col 10
                    '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].E) + '</td>' + // Col 11
                    '<td class="ccol col_kappa">' + this.MaskKappa(rowData.ChildRows[i].F) + '</td>' + // Col 12
                    '<td class="ccol col_exportdate">' + this.coalesceCol(rowData.ChildRows[i].LastExportDate) + '</td>' + // Col 13

                '</tr>'
            childTable = childTable + ctRow;
        }
        if (rowData.ChildRows.length > 0) {
            childTable = '<table class="childtable">' + childTable + '</table>';
        }

        return childTable;
    };



}