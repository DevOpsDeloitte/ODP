
var config = {};
var $opts = { selectedItems : [], xy : "", filterlist : "", actionlist : "" };


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