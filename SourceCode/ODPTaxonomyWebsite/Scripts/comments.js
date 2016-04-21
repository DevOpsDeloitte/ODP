var menuRight = document.getElementById('cbp-spmenu-s2'),
 body = document.body;
var currentScrollPosition = 0;
var iOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;

showRight.onclick = function () {
    classie.toggle(this, 'active');
    classie.toggle(menuRight, 'cbp-spmenu-open');
    currentScrollPosition = $(document).scrollTop();
    //currentScrollPosition = $(document).scrollTop();
    //console.log(currentScrollPosition);
    //changeSyncClass();
    return false;
};

showRightPushed.onclick = function () {
    classie.toggle(this, 'active');
    classie.toggle(menuRight, 'cbp-spmenu-open');
    classie.remove(menuRight, 'expand');
    //console.log(currentScrollPosition);
    if (iOS) {
        console.log("is ios");
        $(document).scrollTop(currentScrollPosition);
    }
    //changeSyncClass();
    return false;
};

$(window).on('load resize', function () {
    $('.cbp-spmenu').height($(this).height());
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
        //console.log(" focusin focus " + currentScrollPosition);
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

        //$(".tab-content").not(tab).css("display", "none");
        //$(tab).fadeIn();

    });

    $(".expand-comments").click(function (event) {
        event.preventDefault();
        $(this).parent().parent().toggleClass("expand");
    });

});
