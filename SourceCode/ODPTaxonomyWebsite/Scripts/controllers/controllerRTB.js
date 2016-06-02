app.controller("ODPFormCtrlRT", function ($rootScope, $scope, $http, $firebase, $firebaseObject, $timeout) {



    $scope.init = function () {

        var FIREBASE_LOCATION;
        window.MYSCOPE = $scope;
        $scope.mdata = {};
        $scope.local = {};
        $scope.FIREBASE_LOCATION = window.FIREBASE_CONFIG;
        FIREBASE_LOCATION = $scope.FIREBASE_LOCATION;
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

        $scope.mdata.abstractid = $("input#abstractid").val();
        $scope.local.abstractid = $("input#abstractid").val();
        $scope.mdata.teamid = $("input#teamid").val();
        $scope.teamid = $("input#teamid").val();

        $scope.showDescription = function (inID) {
            //console.log("show description.." + inID);
            window.open("./GlossaryB.aspx#" + inID, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, width=768, height=700");
        };

        $scope.printAbstract = function () {
            //console.log("print abstract :: " + window.location.href);
            window.open("./PrintAbstract.aspx?id=" + $scope.local.abstractid, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, width=768, height=700");
        };

        console.log(" model team id :: " + $scope.mdata.teamid);

        var teamRef = new Firebase(FIREBASE_LOCATION + "/teams" + "/" + $scope.mdata.teamid);
        //var sync = $firebase(teamRef);
        var sync = $firebaseObject(teamRef);

        sync.$bindTo($scope, "mdata").then(function () {
            
        });

        //var syncObject = sync.$asObject();
        //syncObject.$bindTo($scope, "mdata");

        $scope.$watch("mdata", function (newValue, oldValue) {
            if (newValue.comments !== oldValue.comments && newValue.comments !== undefined) {
                //trigger open
                if (newValue.comments.length > 0) {
                    var menuElement = document.querySelector('#cbp-spmenu-s2');
                    if (!angular.element(menuElement).hasClass('cbp-spmenu-open')) {
                        angular.element(menuElement).addClass('cbp-spmenu-open');
                        //newValue.comments = newValue.comments.replace(/\n\r?/g, '<br />');
                        //console.log(" mdata model changed : " + newValue.comments + " old value :: " + oldValue.comments);
                    }
                }

            }
            
            //if (!$scope.mdata.comments) {
                //console.log(" mdata model changed : " + newValue.comments + " old value :: " + oldValue.comments);
                // console.log(" time to redirect -- watch over. ");
                //window.location = "Evaluation.aspx";
            // }

        }, true);


        $scope.detectTeam();



    }

    $scope.detectTeam = function () {
        var firebasedetectURL = $scope.FIREBASE_LOCATION + "/presence"  +"/" + $scope.mdata.teamid;
        var teamdetectObj = new Firebase(firebasedetectURL);

        $timeout(function () {

            teamdetectObj.on("value", function (snap) {
                var teamexists = false;
                if (snap.val()) {
                    $globdata = snap.val();
                    if (snap.val() !== undefined) {

                        $team.keys = Object.keys(snap.val());
                        //var $teamRT = {};
                        $teamRT.vals = $.map(snap.val(), function (value, key) {
                            return value;
                        });
                        for (var i = 0; i < $team.keys.length; i++) {
                            console.log(" team keys " + i + "    " + $teamRT.vals[i].teamid + "     " + $scope.teamid);
                            if ($teamRT.vals[i].teamid == $scope.teamid) {
                                teamexists = true;
                            }
                        }
                    }
                }
                else {
                    teamexists = false;
                }

                if (!teamexists) {
                    console.log(" time to redirect -- watch over. Back to B Process Form. ");
                    window.location = "EvaluationB.aspx";
                }




            });




        }, 500);



    },

    $scope.showChange = function () {

        console.log("data changed : " + $scope.x);

    },

    $scope.init();

    $scope.loadComments = function () {
         window.CoderComments= $scope.mdata.CoderComments;
    };

    $scope.showIQSCoders = function () {
        //angular.element(document.getElementById('IQS')).scope().showIQSCoders()
        if ($scope.mdata.formmode === undefined) return false;
        //return $scope.mdata.formmode == "Coder Consensus" || $scope.mdata.formmode.indexOf("Comparison") != -1;
        return $scope.mdata.formmode == "Coder Consensus" || $scope.mdata.formmode.indexOf("Comparison") != -1 || $scope.mdata.formmode == "ODP Staff Member Consensus";
    };

    $scope.showODPCoders = function () {
        if ($scope.mdata.formmode === undefined) return false;
        return $scope.mdata.formmode != "Coder Consensus" || $scope.mdata.formmode.indexOf("Comparison") != -1;
    };

    $scope.showComments = function () {
        //angular.element(document.getElementById('IQS')).scope().showComments()
        //return ($scope.mdata.displaymode != 'Insert') && ($scope.mdata.formmode.indexOf("Evaluation") == -1);
        if ($scope.mdata.formmode === undefined) return false;
        return ($scope.mdata.formmode.indexOf("Consensus") != -1 || $scope.mdata.formmode.indexOf("Comparison") != -1);
    };

    $scope.showODPDefault = function () {
        if ($scope.mdata.formmode === undefined) return false;
        return $scope.mdata.formmode.indexOf("ODP") != -1
    };

    $scope.showCoderDefault = function () {
        if ($scope.mdata.formmode === undefined) return false;
        return $scope.mdata.formmode.indexOf("Coder") != -1
    };

    //$scope.loadComments();









});