app.controller("ODPFormCtrlRT", function ($rootScope, $scope, $http, $firebase, $timeout) {



    $scope.init = function () {

        var FIREBASE_LOCATION = "https://intense-fire-1108.firebaseio.com";
        window.MYSCOPE = $scope;
        $scope.mdata = {};
        $scope.mdata.studyfocus = [];
        $scope.mdata.studyfocus[0] = [];
        $scope.mdata.studyfocus[1] = [];
        $scope.mdata.studyfocus[2] = [];
        $scope.mdata.studyfocus[3] = [];

        $scope.mdata.entitiesstudied = [];
        $scope.mdata.studysetting = [];
        $scope.mdata.populationfocus = [];
        $scope.mdata.studydesignpurpose = [];
        $scope.mdata.preventioncategory = [];
        $scope.mdata.teamid = $("input#teamid").val();

        console.log(" model team id :: " + $scope.mdata.teamid);

        var teamRef = new Firebase(FIREBASE_LOCATION + "/teams" + "/" + $scope.mdata.teamid);
        var sync = $firebase(teamRef);
        var syncObject = sync.$asObject();
        syncObject.$bindTo($scope, "mdata");



    }

    $scope.showChange = function () {

        console.log("data changed : " + $scope.x);

    },

    $scope.init();









});