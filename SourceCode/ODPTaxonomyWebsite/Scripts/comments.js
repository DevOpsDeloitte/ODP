var menuRight = document.getElementById('cbp-spmenu-s2'),
 body = document.body;
var currentScrollPosition = 0;
var iOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;

function getBottom() {
    var $el = $('#tabcontainer');
    var bottom = $el.offset().top + $el.outerHeight(true)
    return bottom < 120 ? 120 : bottom;
};

$(function () {
    var $log = $("#log");

    function updateLog(type, x, y) {
        $log.html('Type: ' + type + '; X: ' + x + '; Y: ' + y);
        console.log('Type: ' + type + '; X: ' + x + '; Y: ' + y);
    };



    function onLoadSetCommentsBox() {
        var b = getBottom();
        console.log(" bottom val :: " + b);
        if (b > 0) {
            $("textarea#comments").css({ top: b });
        }
        //else {
        //    $("textarea#comments").css({ top: '120px' });
        //}
    };
    onLoadSetCommentsBox();

    document.addEventListener('touchstart', function (e) {
        updateLog('touchstart event', e.changedTouches[0].pageX, e.changedTouches[0].pageY);
        //var $el = $('#tabcontainer');
        //var bottom = $el.offset().top + $el.outerHeight(true)
        if (e.targetTouches[0].pageY > getBottom() ) {
            $("textarea#comments").css({ top: e.targetTouches[0].pageY });
            //$("div#commentsInsertBox").css({ top: e.targetTouches[0].pageY });
        }
        return true;
    });

    document.addEventListener('touchmove', function (e) {
        //e.preventDefault();
        //console.log('touchmove', e.targetTouches[0].pageY);
        //updateLog(e.targetTouches[0].pageX, e.targetTouches[0].pageY);
        //$("textarea#comments").css({ top: e.targetTouches[0].pageY });
        //$("div#commentsInsertBox").css({ top: e.targetTouches[0].pageY });
    }, false);

    $("textarea#comments").on("click touchstart", function (e) {
        updateLog("comments event "+e, 0, 0);
        e.stopPropagation();
    });


    menuRight.onclick = function (e) {
        if (iOS) {
            if ($(e.target).is('p') || $(e.target).is('h5')) {
                //e.preventDefault();
                return true;
            }
        }

        if (iOS) {
            updateLog("focus event ", 0, 0);
            //$("textarea#comments").css({ top: e.targetTouches[0].pageY });
            $("textarea#comments").focus();
        }
        return true;
    };

});



showRight.onclick = function () {
    classie.toggle(this, 'active');
    classie.toggle(menuRight, 'cbp-spmenu-open');
    currentScrollPosition = $(document).scrollTop();
    //currentScrollPosition = $(document).scrollTop();
    //console.log(currentScrollPosition);
    //changeSyncClass();
    return false;
};

showRightPushed.onclick = function (e) {
    e.stopPropagation();
    classie.toggle(this, 'active');
    classie.toggle(menuRight, 'cbp-spmenu-open');
    classie.remove(menuRight, 'expand');

    if (iOS) {
        //console.log("is ios");
        //$(document).scrollTop(currentScrollPosition);
    }
    //changeSyncClass();
    return false;
};

$(window).on('load resize', function () {
    //$('.cbp-spmenu').height($(this).height());
    //$('.cbp-spmenu').height($(document).height());
    $('.cbp-spmenu').height("100%");
});

function changeSyncClass() {
    var appElement = document.querySelector('#tax-form');
    var $scope = angular.element(appElement).scope();
    var classList = document.getElementById('cbp-spmenu-s2').getAttribute("class");
    console.log(" classes : " + classList);
    $scope.$apply(function () {
        $scope.mdata.matchclass = classList;
    });
}


//$(document).scroll(function () {
//    currentScrollPosition = $(this).scrollTop();
//});

$(document).ready(function () {


    $('textarea').on('focusin focus', function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        currentScrollPosition = $(document).scrollTop();
        console.log(" focusin focus " + currentScrollPosition);
        //$(document).scrollTop(currentScrollPosition);
    });


    $('textarea#comments').autogrow({ vertical: true, horizontal: false, flickering: false });

    $(".comments-display").show();

    $(".tabs-menu a").click(function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();
        $(this).parent().addClass("current");
        $(this).parent().siblings().removeClass("current");
        var tab = $(this).attr("href");

        $(".tab-content").not(tab).removeClass("current");
        $(tab).addClass("current");


        // recompute the textarea overlap area.
        if (iOS) {
            var b = getBottom();
            //console.log(" bottom val :: " + b);
            if (b > 0) {
                $("textarea#comments").css({ top: b });
            }
        }

        //$(".tab-content").not(tab).css("display", "none");
        //$(tab).fadeIn();

    });

    $(".expand-comments").click(function (event) {
        event.preventDefault();
        $(this).parent().parent().toggleClass("expand");
    });

});
