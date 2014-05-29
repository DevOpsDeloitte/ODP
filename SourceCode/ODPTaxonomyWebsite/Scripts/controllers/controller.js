app.controller("ODPFormCtrl", function ($rootScope, $scope, $http) {



    $scope.init = function () {


        $scope.mdata = {};
        $scope.mdata.superusername = "";
        $scope.mdata.superpassword = "";
        $scope.mdata.formmode = "Consensus"; // $("input#mode").val();
        $rootScope.mode = "Consensus"; // $("input#mode").val();

        // Not the best way to extract these values. Needs refactoring.
        $scope.mdata.formmode = $("input#mode").val();
        $rootScope.mode = $("input#mode").val();

        $scope.mdata.displaymode = $("input#displaymode").val();
        $rootScope.displaymode = $("input#displaymode").val();

        $scope.mdata.showconsensusbutton = $("input#showc").val() == "True" ? true : false;

        //        var parent = $rootScope;
        //        var child = parent.$new();

        //        parent.salutation = "Hello";
        //        child.name = "World";

        $scope.disallowSave = true;

        if ($scope.mdata.showconsensusbutton) {
            $scope.showConsensusButton = true;
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
            console.log("show description.." + inID);
            window.open("./Glossary.aspx#" + inID, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, top=500, left=500, width=600, height=400");
        };

        $scope.startConsensus = function () {
            console.log("start consensus :: " + window.location.href);
            window.location.replace(window.location.href + "?startConsenus=true");
            //location.reload(true);
        };

        $scope.$watch("mdata", function () {
            //console.log("form data changed :: " + $scope.mdata);
            if ($scope.displaymode != "View") {
                $scope.$broadcast("disableboxes");
                $scope.$broadcast("validate.formdata");
            }
        }, true);



    };

    $scope.init();

    $scope.$on("disableboxes", function () {
        //console.log($scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length-1]);
        if ($scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length - 1] != undefined && $scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length - 1].isChecked) {
            for (i = 1; i < $scope.mdata.preventioncategory.length - 1; i++) {
                $scope.mdata.preventioncategory[i].resetBox();
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
        if ($scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length - 1] != undefined && $scope.mdata.preventioncategory[$scope.mdata.preventioncategory.length - 1].isChecked && $scope.mdata.studydesignpurpose[$scope.mdata.studydesignpurpose.length - 1] != undefined && $scope.mdata.studydesignpurpose[$scope.mdata.studydesignpurpose.length - 1].isChecked) {
            console.log(" new rule :: form is valid :: ");
            $scope.formIsValid = true;
            $scope.disallowSave = false;
            $scope.showSaveButton = true;
            return;
        }

        // For Consensus Mode -- Insert Validate all boxes are dark green or transparent.
        var boxColors = true; // all green
        if ($scope.displaymode == "Insert" && ($scope.mode == "Coder Consensus" || $scope.mode.indexOf("Comparison") != -1)) {
            for (i = 1; i < $scope.mdata.studyfocus[1].length; i++) {
                if ($scope.mdata.studyfocus[1][i].modelcolorState != "SolidGreen" && $scope.mdata.studyfocus[1][i].modelcolorState != "Transparent" && $scope.mdata.studyfocus[1][i].modelcolorState != "Disabled") {
                    boxColors = false;
                    //console.log(" bad box color : 1 :" + i);
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


        var studyfocuscol1 = studyfocuscol2 = studyfocuscol3 = false;
        for (i = 1; i < $scope.mdata.studyfocus[1].length; i++) {
            //console.log(i);
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

        if (studyfocuscol1 && studyfocuscol2 && studyfocuscol3 && boxColors) {
            console.log(" form is valid :: ");
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

        //$scope.$apply();



    };

    $scope.submitForm = function () {
        console.log("submit form reached..");
        var formArray = $("form").serializeArray();
        //var formArray = $("form").serialize();
        //var fa = JSON.stringify(formArray);
        $http({
            method: 'POST',
            url: 'Handlers/Evaluation.ashx',
            //data: $.param($scope.formData),  // pass in data as strings
            //data: { payload: JSON.stringify($scope.mdata) },  // pass in data as strings
            data: formArray,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded'}  // set the headers so angular passing info as form data (not request payload)
        })
           .success(function (data) {
               console.log("response received : " + data.success);

               if (!data.success) {
                   // if not successful, bind errors to error variables
                   $scope.errorName = data.errors.name;
                   $scope.errorSuperhero = data.errors.superheroAlias;
               } else {

                   // Logic for Supervisor User Auth Failure.
                   if (data.supervisorauth != undefined && data.supervisorauthfailed) {
                       $scope.errormessagesdisplay = "Supervisor authentication Failed!";
                       return;
                   }

                   // if successful, bind success message to message
                   $scope.postmessages = "Saved form successfully!";
                   // Put the form in View Mode::
                   $scope.showSaveButton = false;
                   $scope.mdata.displaymode = "View";
                   $scope.mdata.newsubmissionID = data.submissionID;
                   $rootScope.displaymode = "View";
                   $scope.showResetButton = false;
                   if (data.showConsensusButton) $scope.showConsensusButton = true;
                   //$scope.message = data.message;
               }
           });
    };

    $scope.processForm = function () {
        $scope.postmessages = "";
        console.log("process form clicked..");
        window.alertify.set({
            labels: {
                ok: "OK",
                cancel: "Cancel"
            },
            delay: 5000,
            buttonReverse: true,
            buttonFocus: "none"
        });

        alertify.confirm("Please confirm this", function (e) {
            if (e) {
                //window.location.href = "revise.html";
                //util.save();
                $scope.submitForm();
            } else {
                alertify.error("You've clicked Cancel");
            }
        });






    };



});