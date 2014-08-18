app.controller("ODPFormCtrlRT", function ($rootScope, $scope, $http, $firebase, $timeout) {



    $scope.init = function () {

        var FIREBASE_LOCATION = "https://intense-fire-1108.firebaseio.com";
        window.MYSCOPE = $scope;
        $scope.mdata = {};
        $scope.local = {};
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

        $scope.showDescription = function (inID) {
            //console.log("show description.." + inID);
            window.open("./Glossary.aspx#" + inID, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, width=768, height=700");
        };

        $scope.printAbstract = function () {
            //console.log("print abstract :: " + window.location.href);
            window.open("./PrintAbstract.aspx?id=" + $scope.local.abstractid, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, width=768, height=700");
        };

        console.log(" model team id :: " + $scope.mdata.teamid);

        var teamRef = new Firebase(FIREBASE_LOCATION + "/teams" + "/" + $scope.mdata.teamid);
        var sync = $firebase(teamRef);
        var syncObject = sync.$asObject();
        syncObject.$bindTo($scope, "mdata");

        $scope.$watch("mdata", function () {

            //console.log(" mdata model changed : " + $scope.mdata.firebaseOn);
            if (!$scope.mdata.firebaseOn) {
                // console.log(" time to redirect -- watch over. ");
                //window.location = "Evaluation.aspx";
            }
        });

        // check & listen for team consensus in progress + exist otherwise.

        var firebasedetectURL = FIREBASE_LOCATION + "/teams"; //  +"/" + $scope.mdata.teamid;
        //console.log(firebasedetectURL);
        var teamdetectObj = new Firebase(firebasedetectURL);
        $timeout(function () {
            //Check for team id existence and show consensus button in view individual evaluation.
            //if ($scope.displaymode == "View" && $scope.mode.indexOf("Evaluation") != -1) {
            //console.log(" showing team id : " + $scope.mdata.teamid);
            teamdetectObj.child($scope.mdata.teamid).once('value', function (snapshot) {
                var exists = (snapshot.val() !== null);
                if (exists) {
                    console.log("Team Exists :: all good ");

                }
                else {
                    console.log(" time to redirect -- watch over. ");
                    window.location = "Evaluation.aspx";
                }
            });



            teamdetectObj.on("value", function (snap) {
                var teamexists = false;
                if (snap.val()) {
                    console.log(" team detect object :: value change " + snap.val());
                    $globdata = snap.val();
                    if (snap.val() !== undefined) {
                        console.log(typeof (snap.val()));
                        $team.keys = Object.keys(snap.val());
                        for (var i = 0; i < $team.keys.length; i++) {
                            console.log(" team keys " + i + "    " + $team.keys[i]);
                            if ($team.keys[i] == $scope.mdata.teamid) {
                                teamexists = true;
                            }
                        }
                    }
                }
                else {
                    teamexists = false;
                }
                if (!teamexists) {
                    console.log(" time to redirect -- watch over. ");
                    window.location = "Evaluation.aspx";
                }
            });
            // }

        }, 300);



    }

    $scope.showChange = function () {

        console.log("data changed : " + $scope.x);

    },

    $scope.init();









});