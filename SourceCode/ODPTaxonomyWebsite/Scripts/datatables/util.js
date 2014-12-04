
var config = {};
var $opts = { selectedItems : [], xy : "", filterlist : "", actionlist : "", hiderowItems : [], lastfilterSelection : "", isGridDirty: false };


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



}