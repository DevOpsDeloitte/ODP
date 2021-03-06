app.controller("ODPFormCtrl", function ($rootScope, $scope, $http, $firebase, $firebaseObject, $timeout) {



    $scope.init = function () {


        var FIREBASE_LOCATION;

        $scope.mdata = {};
        $scope.FIREBASE_LOCATION = window.FIREBASE_CONFIG;
        FIREBASE_LOCATION = $scope.FIREBASE_LOCATION;
        $scope.mdata.Role = window.Role;
        $scope.mdata.superusername = "";
        $scope.mdata.superpassword = "";
        $scope.mdata.formmode = "Consensus"; // $("input#mode").val();
        $rootScope.mode = "Consensus"; // $("input#mode").val();

        // Not the best way to extract these values. Needs refactoring.
        $scope.mdata.formmode = $("input#mode").val();
        $rootScope.mode = $("input#mode").val();


        $rootScope.displaymode = $("input#displaymode").val();
        $scope.mdata.displaymode = $rootScope.displaymode;

        $scope.mdata.showconsensusbutton = $("input#showc").val() == "True" ? true : false;
        $scope.mdata.showcomparisonbutton = $("input#showcomp").val() == "True" ? true : false;
        $scope.mdata.unabletocode = $("input#isunable").val() == "checked" ? true : false;

        $scope.mdata.consensusalreadystarted = $("input#consensusalreadystarted").val() == "True" ? true : false;
        $scope.mdata.unablecodersval = $("input#hiddenUnableCoders").val();
        //console.log(" New Value : " + $scope.mdata.consensusalreadystarted);

        if ($scope.mdata.consensusalreadystarted) {
            $scope.errormessagesdisplay = "Consensus already started!";
        }


        $scope.mdata.abstractid = $("input#abstractid").val();
        $scope.mdata.teamid = $("input#teamid").val();

        $scope.showWatchConsensusButton = false;
        // foe new comments feature.
        //$scope.mdata.comments = "";


        //$scope.$apply();




        $scope.disallowSave = true;
        $scope.saveButtonClicked = false;

        if ($scope.mdata.showconsensusbutton) {
            $scope.showConsensusButton = true;
        }
        if ($scope.mdata.showcomparisonbutton) {
            $scope.showComparisonButton = true;
        }

        // Init for View mode
        if ($rootScope.displaymode == "View") {
            $scope.showSaveButton = false;
            $scope.showResetButton = false;
        }
        else {
            $scope.showSaveButton = true;
            $scope.showResetButton = true;
        }

        $scope.formIsValid = false;
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



        $scope.showDescription = function (inID) {
            //console.log("show description.." + inID);
            window.open("./Glossary.aspx#" + inID, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, width=768, height=700");
        };

        $scope.cleanURL = function (instr) {
            return instr.replace("?startConsenus=true", "").replace("?startComparison=true", "");
        }

        $scope.startConsensus = function () {
            
            window.location.replace($scope.cleanURL(window.location.href) + "?startConsenus=true");
           

        };

        $scope.watchConsensus = function () {

            window.location.replace("/Evaluation/EvaluationRT.aspx?modetype=watchconsensus&teamid=" + $scope.mdata.teamid + "&abstractid=" + $scope.mdata.abstractid);
        }

        $scope.watchComparison = function () {

            window.location.replace("/Evaluation/EvaluationRT.aspx?modetype=watchcomparison&teamid=" + $scope.mdata.teamid + "&abstractid=" + $scope.mdata.abstractid);
        }

        $scope.startComparison = function () {
            //console.log("start consensus :: " + window.location.href);
            window.location.replace($scope.cleanURL(window.location.href) + "?startComparison=true");
            //location.reload(true);
        };

        $scope.printAbstract = function () {
            //console.log("print abstract :: " + window.location.href);
            window.open("./PrintAbstract.aspx?id=" + $scope.mdata.abstractid, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, width=768, height=700");
        };

        $scope.$watch("mdata", function () {
            //console.log("form data changed :: " + $scope.mdata);
            if ($scope.displaymode != "View") {
                $scope.$broadcast("disableboxes");
                $scope.$broadcast("validate.formdata");

                // sync mdata with data - remove all function properties
                if ($scope.mode.indexOf("Consensus") != -1 || $scope.mode.indexOf("Comparison") != -1) {
                    //console.log(" watch fired to sync mdata - data :: 1 ");
                    if ($watchSyncCounter < 5) {
                        $timeout(function () {
                            $scope.data = $scope.syncObjects($scope.mdata);
                            $watchSyncCounter++;
                        }, 250);
                    }
                    else {
                        $scope.data = $scope.syncObjects($scope.mdata);
                        $watchSyncCounter++;
                    }
                    $globdata = JSON.parse(JSON.stringify($scope.mdata));
                }
            }
            else {

                // sync mdata with data - remove all function properties
                if ($scope.mode.indexOf("Consensus") != -1 || $scope.mode.indexOf("Comparison") != -1) {
                    $scope.mdata.displaymode = $scope.displaymode;
                    console.log(" watch fired to sync mdata - data :: 2 ");
                    if ($watchSyncCounter < 5) {
                        $timeout(function () {
                            $scope.data = $scope.syncObjects($scope.mdata);
                            $watchSyncCounter++;
                        }, 250);
                    }
                    else {
                        $scope.data = $scope.syncObjects($scope.mdata);
                        $watchSyncCounter++;
                    }
                    $globdata = JSON.parse(JSON.stringify($scope.mdata));
                }

            }
        }, true);

        $scope.syncObjects = function (inData) {
            return JSON.parse(JSON.stringify(inData));
        };




        $scope.mdata.firebaseOn = true;
        $scope.data = $scope.syncObjects($scope.mdata);

        $scope.checkForConsensusProgress2();

        $scope.turnOnSyncing();


    };



    $scope.turnOnSyncing = function () {

        if (($scope.mode.indexOf("Consensus") != -1 || $scope.mode.indexOf("Comparison") != -1) && $scope.displaymode == "Insert") {
            // Add ourselves to presence list when online.
            var listRef = firebase.database().ref().child("/presence/" + $scope.mdata.teamid);
            var presenceRef = firebase.database().ref().child("/.info/connected");
            listRef.onDisconnect().remove();

            presenceRef.on("value", function (snap) {
                if (snap.val()) {
                    listRef.remove();
                    var teamPushed = listRef.push({ teamid: $scope.mdata.teamid, abstractid: $scope.mdata.abstractid, mode: $scope.mode, datetime: new Date().toLocaleString() });
                    // Remove ourselves when we disconnect.
                    listRef.onDisconnect().remove();
                }
                else {

                }
            });

            var teamRef = firebase.database().ref().child("/teams" + "/" + $scope.mdata.teamid);
            teamRef.onDisconnect().remove();

            var sync = $firebaseObject(teamRef);
            sync.$bindTo($scope, "data").then(function () {
                $scope.mdata.firebaseOn = true;
                $scope.data = $scope.syncObjects($scope.mdata);
            });
        }

    };

    $scope.checkForConsensusProgress2 = function () {

        // check for team consensus in progress
        $scope.showWatchConsensusButton = false;
        var teamdetectObj = firebase.database().ref().child("/presence" + "/" + $scope.mdata.teamid);
        $timeout(function () {
            //Check for team id existence and show consensus & show comparison button in view/insert individual evaluation.
            if ($scope.mode.indexOf("Evaluation") != -1) {

                teamdetectObj.on('value',function (snap) {
                    var teamexists = false;
                    var modeval = "";
                 
                    if (snap.val()) {
                        console.log(" checkForConsensusProgress2 team detect object :: value change " + snap.val());
                        $globdata = snap.val();
                        if (snap.val() !== undefined) {
                            $team.keys = Object.keys(snap.val());
                            $teamRT.vals = $.map(snap.val(), function (value, key) {
                                return value;
                            });
                            for (var i = 0; i < $team.keys.length; i++) {
                                console.log(" team keys " + i + "    " + $teamRT.vals[i].teamid);
                                if ($teamRT.vals[i].teamid == $scope.mdata.teamid && $teamRT.vals[i].abstractid == $scope.mdata.abstractid) {
                                    teamexists = true;
                                    modeval = $teamRT.vals[i].mode;
                                }
                            }
                        }
                    }
                    else {
                        teamexists = false;
                    }

                    console.log(" team exists :: " + teamexists);
                    $scope.$apply(function () {
                        if (modeval == "") {
                            $scope.showWatchConsensusButton = false;
                            $scope.showWatchComparisonButton = false;
                        }
                        if (modeval.indexOf("Consensus") != -1) {
                            $scope.showWatchConsensusButton = teamexists;
                        }
                        if (modeval.indexOf("Comparison") != -1) {
                            $scope.showWatchComparisonButton = teamexists;
                        }

                    });
                });
            }

        }, 300);

    };

    $scope.init();


    $scope.$on("disableboxes", function () {
        //console.log($scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length-1]);
        // Rule for F. Prevention Research Category
        if ($scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length - 1] != undefined && $scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length - 1].isChecked) {
            for (i = 1; i < $scope.mdata.preventioncategory.length - 1; i++) {
                $scope.mdata.preventioncategory[i].resetBoxCC();
            }
        }
        // Adding Rule for E. StudyDesign Purpose
        if ($scope.mdata.studydesignpurpose[$scope.mdata.studydesignpurpose.length - 1] != undefined && $scope.mdata.studydesignpurpose[$scope.mdata.studydesignpurpose.length - 1].isChecked) {
            for (i = 1; i < $scope.mdata.studydesignpurpose.length - 1; i++) {
                $scope.mdata.studydesignpurpose[i].resetBoxCC();
            }
        }


    });

    $scope.$on("validate.formdata", function () {
        // No need to validate if user has checked unable to code.
        if ($scope.mdata.unabletocode) {

            if ($scope.mdata.superusername != "" && $scope.mdata.superpassword != "") {
                //console.log(" unable to code :: form is valid :: ");
                $scope.formIsValid = true;
                $scope.disallowSave = false;
                $scope.showSaveButton = true;
            }
            else {
                //console.log(" unable to code :: form is not valid :: ");
                $scope.formIsValid = false;
                $scope.disallowSave = true;
                $scope.showSaveButton = false;

            }
            return;
        }

        //if ($scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length - 1] != undefined && $scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length - 1].isChecked && $scope.mdata.studydesignpurpose[$scope.mdata.studydesignpurpose.length - 1] != undefined && $scope.mdata.studydesignpurpose[$scope.mdata.studydesignpurpose.length - 1].isChecked) {
        //    console.log(" new rule :: form is valid :: ");
        //    $scope.formIsValid = true;
        //    $scope.disallowSave = false;
        //    $scope.showSaveButton = true;
        //    return;
        //}

        // For Consensus Mode -- Insert Validate all boxes are dark green or transparent.
        var boxColors = true; // all green
        if ($scope.displaymode == "Insert" && ($scope.mode.indexOf("Consensus") != -1 || $scope.mode.indexOf("Comparison") != -1)) {
            for (i = 1; i < $scope.mdata.studyfocus[1].length; i++) {
                if ($scope.mdata.studyfocus[1][i].modelcolorState != "SolidGreen" && $scope.mdata.studyfocus[1][i].modelcolorState != "Transparent" && $scope.mdata.studyfocus[1][i].modelcolorState != "Disabled") {
                    boxColors = false;
                    //console.log(" bad box color : 1 :" + i + "    " + $scope.mdata.studyfocus[1][i].modelcolorState);
                    break;
                }
            }

            for (i = 1; i < $scope.mdata.studyfocus[2].length; i++) {
                if ($scope.mdata.studyfocus[2][i].modelcolorState != "SolidGreen" && $scope.mdata.studyfocus[2][i].modelcolorState != "Transparent" && $scope.mdata.studyfocus[2][i].modelcolorState != "Disabled") {
                    boxColors = false;
                    //console.log(" bad box color : 2 :" + i);
                    break;
                }
            }

            for (i = 1; i < $scope.mdata.studyfocus[3].length; i++) {
                if ($scope.mdata.studyfocus[3][i].modelcolorState != "SolidGreen" && $scope.mdata.studyfocus[3][i].modelcolorState != "Transparent" && $scope.mdata.studyfocus[3][i].modelcolorState != "Disabled") {
                    boxColors = false;
                    //console.log(" bad box color : 3 :" + i);
                    break;
                }
            }

            for (i = 1; i < $scope.mdata.entitiesstudied.length; i++) {
                if ($scope.mdata.entitiesstudied[i].modelcolorState != "SolidGreen" && $scope.mdata.entitiesstudied[i].modelcolorState != "Transparent" && $scope.mdata.entitiesstudied[i].modelcolorState != "Disabled") {
                    boxColors = false;
                    //console.log(" bad box color : entities studied :" + i);
                    break;
                }
            }

            for (i = 1; i < $scope.mdata.studysetting.length; i++) {
                if ($scope.mdata.studysetting[i].modelcolorState != "SolidGreen" && $scope.mdata.studysetting[i].modelcolorState != "Transparent" && $scope.mdata.studysetting[i].modelcolorState != "Disabled") {
                    boxColors = false;
                    //console.log(" bad box color : studysetting :" + i);
                    break;
                }
            }


            for (i = 1; i < $scope.mdata.populationfocus.length; i++) {
                if ($scope.mdata.populationfocus[i].modelcolorState != "SolidGreen" && $scope.mdata.populationfocus[i].modelcolorState != "Transparent" && $scope.mdata.populationfocus[i].modelcolorState != "Disabled") {
                    boxColors = false;
                    //console.log(" bad box color : populationfocus :" + i);
                    break;
                }
            }

            for (i = 1; i < $scope.mdata.studydesignpurpose.length; i++) {
                if ($scope.mdata.studydesignpurpose[i].modelcolorState != "SolidGreen" && $scope.mdata.studydesignpurpose[i].modelcolorState != "Transparent" && $scope.mdata.studydesignpurpose[i].modelcolorState != "Disabled") {
                    boxColors = false;
                    //console.log(" bad box color : studydesignpurpose :" + i);
                    break;
                }
            }

            for (i = 1; i < $scope.mdata.preventioncategory.length; i++) {
                if ($scope.mdata.preventioncategory[i].modelcolorState != "SolidGreen" && $scope.mdata.preventioncategory[i].modelcolorState != "Transparent" && $scope.mdata.preventioncategory[i].modelcolorState != "Disabled") {
                    boxColors = false;
                    //console.log(" bad box color : preventioncategory :" + i);
                    break;
                }
            }

        }


        var studyfocuscol1 = studyfocuscol2 = studyfocuscol3 = entitiesstudiedBox = studysettingBox = populationfocusBox = studydesignpurposeBox = preventioncategoryBox = false;
        for (i = 1; i < $scope.mdata.studyfocus[1].length; i++) {
            if ($scope.mdata.studyfocus[1][i].isChecked) {
                studyfocuscol1 = true;
                break;
            }
        }
        for (i = 1; i < $scope.mdata.studyfocus[2].length; i++) {
            if ($scope.mdata.studyfocus[2][i].isChecked) {
                studyfocuscol2 = true;
                break;
            }
        }
        for (i = 1; i < $scope.mdata.studyfocus[3].length; i++) {
            if ($scope.mdata.studyfocus[3][i].isChecked) {
                studyfocuscol3 = true;
                break;
            }
        }

        // Adding business logic for A - F Mandatory Coding.

        //for (i = 1; i < $scope.mdata.entitiesstudied.length; i++) {
        //    if ($scope.mdata.entitiesstudied[i].isChecked) {
        //        entitiesstudiedBox = true;
        //        break;
        //    }
        //}
        //for (i = 1; i < $scope.mdata.studysetting.length; i++) {
        //    if ($scope.mdata.studysetting[i].isChecked) {
        //        studysettingBox = true;
        //        break;
        //    }
        //}

        entitiesstudiedBox = true; studysettingBox = true;

        for (i = 1; i < $scope.mdata.populationfocus.length; i++) {
            if ($scope.mdata.populationfocus[i].isChecked) {
                populationfocusBox = true;
                break;
            }
        }
        for (i = 1; i < $scope.mdata.studydesignpurpose.length; i++) {
            if ($scope.mdata.studydesignpurpose[i].isChecked) {
                studydesignpurposeBox = true;
                break;
            }
        }
        for (i = 1; i < $scope.mdata.preventioncategory.length; i++) {
            if ($scope.mdata.preventioncategory[i].isChecked) {
                preventioncategoryBox = true;
                break;
            }
        }

        if (studyfocuscol1 || studyfocuscol2 || studyfocuscol3 || populationfocusBox) {
            //console.log("E7 / F6 only bypassed ");
            // bypass E7 / F6 Only. As this will now be treated as a regular coding. Another category selection was made.

        }
        else {

            // Do the E7 / F6 Rule
            if ($scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length - 1] != undefined && $scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length - 1].isChecked && $scope.mdata.studydesignpurpose[$scope.mdata.studydesignpurpose.length - 1] != undefined && $scope.mdata.studydesignpurpose[$scope.mdata.studydesignpurpose.length - 1].isChecked && boxColors) {
                //console.log(" new rule :: form is valid :: ");
                $scope.formIsValid = true;
                $scope.disallowSave = false;
                $scope.showSaveButton = true;
                return;
            }
        }

        if (studyfocuscol1 && studyfocuscol2 && studyfocuscol3 && entitiesstudiedBox && studysettingBox && populationfocusBox && studydesignpurposeBox && preventioncategoryBox && boxColors) {
            //console.log(" form is valid :: ");
            $scope.formIsValid = true;
            $scope.disallowSave = false;
            $scope.showSaveButton = true;
        }
        else {
            $scope.formIsValid = false;
            $scope.disallowSave = true;
            $scope.showSaveButton = false;
        }




        //
        // console.log("event form data changed :: " + studyfocuscol1);

    });

    $scope.resetFormStart = function () {

        window.alertify.set({
            labels: {
                ok: "OK",
                cancel: "Cancel"
            },
            delay: 5000,
            buttonReverse: true,
            buttonFocus: "none"
        });

        alertify.confirm("Are you sure you want to reset?", function (e) {
            if (e) {
                //window.location.href = "revise.html";
                //util.save();

                $scope.resetForm();
            } else {
                alertify.error("You've clicked Cancel");
            }
        });

    };

    $scope.resetForm = function () {



        for (i = 1; i < $scope.mdata.studyfocus[1].length; i++) {
            $scope.mdata.studyfocus[1][i].resetBox();
            //console.log(i + "    " + $scope.mdata.studyfocus[1][i].modelcolorState);
        }
        for (i = 1; i < $scope.mdata.studyfocus[2].length; i++) {
            $scope.mdata.studyfocus[2][i].resetBox();
        }
        for (i = 1; i < $scope.mdata.studyfocus[3].length; i++) {
            $scope.mdata.studyfocus[3][i].resetBox();
        }
        for (i = 1; i < $scope.mdata.entitiesstudied.length; i++) {
            $scope.mdata.entitiesstudied[i].resetBox();
        }
        for (i = 1; i < $scope.mdata.studysetting.length; i++) {
            $scope.mdata.studysetting[i].resetBox();
        }
        for (i = 1; i < $scope.mdata.populationfocus.length; i++) {
            $scope.mdata.populationfocus[i].resetBox();
        }
        for (i = 1; i < $scope.mdata.studydesignpurpose.length; i++) {
            $scope.mdata.studydesignpurpose[i].resetBox();
        }
        for (i = 1; i < $scope.mdata.preventioncategory.length; i++) {
            $scope.mdata.preventioncategory[i].resetBox();
        }

        // reset unable to code & super username / password.
        $scope.mdata.superusername = "";
        $scope.mdata.superpassword = "";
        $scope.mdata.unabletocode = false;
        $scope.mdata.comments = "";

        $scope.$apply();

        if ($scope.mode.indexOf("Consensus") != -1) {
            //console.log(" watch fired to sync mdata - data ");
            //$scope.data = $scope.syncObjects($scope.mdata);
        }

        //console.log(" reset triggered..");



    };

    $scope.submitForm = function () {
        console.log("submit form reached..");
        var formArray = $("form").serializeArray();
        //var formArray = $("form").serialize();
        //var fa = JSON.stringify(formArray);
        $scope.showSaveButton = false; // for concurrency issue.

        $scope.showResetButton = false;
        $scope.mdata.displaymode = "View";
        $rootScope.displaymode = "View";

        $http({
            method: 'POST',
            url: 'Handlers/Evaluation.ashx',
            //data: $.param($scope.formData),  // pass in data as strings
            //data: { payload: JSON.stringify($scope.mdata) },  // pass in data as strings
            data: formArray,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded'}  // set the headers so angular passing info as form data (not request payload)
        })
           .then(function (dataRecieved) {
               //console.log("response received : " + JSON.stringify(dataRecieved));
               var data = dataRecieved.data;
               if (!data.success) {
                   $scope.showSaveButton = true;

                   $scope.showResetButton = true;
                   $scope.mdata.displaymode = "Insert";
                   $rootScope.displaymode = "Insert";

                   // Logic for Supervisor User Auth Failure.
                   if (data.supervisorauthfailed != undefined && data.supervisorauthfailed) {
                       $scope.errormessagesdisplay = "Supervisor authentication Failed!";
                       return;
                   }

                   // Logic for Multiple consensus failure.
                   if (data.multipleconsensusexists != undefined && data.multipleconsensusexists) {
                       $scope.errormessagesdisplay = "Consensus record already exists. Save Failed!";
                       return;
                   }

                   if (data.submissionexists != undefined && data.submissionexists) {
                       $scope.errormessagesdisplay = "Submission record already exists. Save Failed!";
                       return;
                   }

                   if (data.unabletocreatesubmission != undefined && data.unabletocreatesubmission) {
                       $scope.errormessagesdisplay = "Unable to create submission. Save Failed, Try Again!";
                       return;
                   }

               } else {


                   // if successful, bind success message to message
                   $scope.postmessages = "Saved form successfully!";
                   // Put the form in View Mode::
                   $scope.showSaveButton = false;
                   $scope.mdata.displaymode = "View";
                   $rootScope.displaymode = "View";

                   $scope.mdata.newsubmissionID = data.submissionID;
                   $scope.showResetButton = false;

                   if (data.showConsensusButton) $scope.showConsensusButton = true;
                   if (data.showComparisonButton) $scope.showComparisonButton = true;

                   
               }
           });


    };

    $scope.processForm = function () {
        $scope.postmessages = "";
        $scope.errormessagesdisplay = "";
        //console.log("process form clicked..");

        window.alertify.set({
            labels: {
                ok: "OK",
                cancel: "Cancel"
            },
            delay: 5000,
            buttonReverse: true,
            buttonFocus: "none"
        });

        alertify.confirm("Please confirm save?", function (e) {
            if (e) {
                window.scrollTo(0, 0);
                $scope.submitForm();
            } else {
                alertify.error("You've clicked cancel Save..");
            }
        });


    };


    $scope.loadComments = function () {
        $scope.mdata.CoderComments = window.CoderComments;
        $scope.mdata.comments = window.Comments;
    };

    $scope.showIQSCoders = function () {
        //angular.element(document.getElementById('IQS')).scope().showIQSCoders()
        return $scope.mdata.formmode == "Coder Consensus" || $scope.mdata.formmode.indexOf("Comparison") != -1 || $scope.mdata.formmode == "ODP Staff Member Consensus";
    };

    $scope.showODPCoders = function () {
        return $scope.mdata.formmode != "Coder Consensus" || $scope.mdata.formmode.indexOf("Comparison") != -1;
    };

    $scope.showComments = function () {
        // for console.log debugging, hook element and view scope.
        //angular.element(document.getElementById('IQS')).scope().showComments()
        //return ($scope.mdata.displaymode != 'Insert') && ($scope.mdata.formmode.indexOf("Evaluation") == -1);
        return ($scope.mdata.formmode.indexOf("Consensus") != -1 || $scope.mdata.formmode.indexOf("Comparison") != -1);
    };

    $scope.showODPDefault = function () {
        return $scope.mdata.formmode.indexOf("ODP") != -1
    };

    $scope.showCoderDefault = function () {
        return $scope.mdata.formmode.indexOf("Coder") != -1
    };

    $scope.loadComments();





});