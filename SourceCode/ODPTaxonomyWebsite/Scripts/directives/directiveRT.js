
app.directive("outcomeBox", function ($rootScope) {

    var boxTemplate = '<div class="select-box {{view.colorstate}}" ng-class="view.classValue"  ng-click="updatestate()" >' +
       '<div class="select-box-container">' +
       '<input type="checkbox" name="{{view.iname}}"  ng-model="view.modelValue" ng-disabled="view.disabled" ng-checked="view.checked" />' +
       //'<span ng-show="view.consensus">{{view.codervals}}</span>' +
       '<div ng-show="view.consensus"><div class="codercbox coder1">{{view.displaycoders[0]}}</div> <div class="codercbox coder2">{{view.displaycoders[1]}}</div> <div class="codercbox coder3">{{view.displaycoders[2]}}</div> </div>' +
       '<div ng-show="view.comparers"><div class="comparisoncbox ccoder">{{view.displaycoders[0]}}</div> <div class="comparisoncbox codp">{{view.displaycoders[1]}}</div> </div>' +
       '<div class="codercount">{{view.displaycoderscount}}</div>' +
      //'<span>{{$rootScope.mdata.formmode}}</span>' +
       '<div>' +
       '</div>';

    return {
        restrict: "A",
        replace: true,
        template: boxTemplate,
        scope: {
            value: "=outcomeBox"
        },

        link: function (scope, elem, attrs) {
         //scope.value = {};
         scope.view.disabled = false;
         scope.view.checked = true;
         //console.log(" link fired " + scope.value);

         

           scope.$watch('value', function(newValue, oldValue) {
           
           //console.log( " link function old value : " + oldValue.codercount + "     new value : " + newValue.codercount );
                //if (newValue)

                try{
                    scope.view.checked = newValue.isChecked;
                }catch(e) {};

                try{
                    
                     scope.view.colorstate = newValue.colorstatecopy;
                }catch(e) {};

                try{
                    if(scope.view.checked){
                       // scope.view.colorstate = "onstate icheckbox_line-tax checked";
                    }
                    else{
                        //scope.view.colorstate = "";
                        //scope.view.colorstate = newValue.colorstatecopy;
                    }
                }catch(e) {};

                try{
                if(newValue.modelcolorState == "Disabled"){
                    scope.view.colorstate = "select-box box-disabled";
                }
                }catch(e) {};

                try{
                  if(newValue.codercount){
                    scope.view.displaycoderscount = newValue.codercount;
                }
                }catch(e) {};

                //if($rootScope.mdata.formmode.indexOf('Consensus') != -1) scope.view.consensus = true;
                //if($rootScope.mdata.formmode.indexOf('Comparison') != -1) scope.view.comparers = true;
                //testX = $rootScope.mdata;
                 //scope.view.comparers = true;
                 //console.log(scope.$parent.mdata.formmode);
                  try{
                 
                    //if(MYSCOPE.mdata.formmode.indexOf('Consensus') != -1) scope.view.consensus = true;
                    //if(MYSCOPE.mdata.formmode.indexOf('Comparison') != -1) scope.view.comparers = true;
                    if(scope.$parent.mdata.formmode.indexOf('Consensus') != -1) scope.view.consensus = true;
                    if(scope.$parent.mdata.formmode.indexOf('Comparison') != -1) scope.view.comparers = true;
                }catch(e) {};

                  try{
                     scope.view.displaycoders = newValue.displaycoders;
                }catch(e) {};
                 
            });

        },

        controller: function ( $scope, $element, $attrs) {
             //console.log($scope.codercount);
              $scope.view = {
                modelValue: $scope.value,
                editorEnabled: false,
                hitCount: 0,
                enabled: true

            };

            
           $scope.updatestate = function(){
                return;
           };

        }

        }


});


app.directive("outcomeBoxOld", function ($rootScope) {

    var boxTemplate = '<div class="select-box {{view.colorstate}}" ng-class="view.classValue"  ng-click="updatestate()" >' +
       '<div class="select-box-container">' +
       '<input type="checkbox" name="{{view.iname}}"  ng-model="view.modelValue" ng-disabled="view.disabled" ng-checked="view.checked" />' +
       //'<span ng-show="view.consensus">{{view.codervals}}</span>' +
       '<div ng-show="view.consensus"><div class="codercbox coder1">{{view.displaycoders[0]}}</div> <div class="codercbox coder2">{{view.displaycoders[1]}}</div> <div class="codercbox coder3">{{view.displaycoders[2]}}</div> </div>' +
       '<div ng-show="view.comparers"><div class="comparisoncbox ccoder">{{view.displaycoders[0]}}</div> <div class="comparisoncbox codp">{{view.displaycoders[1]}}</div> </div>' +
       '<div class="codercount">{{view.displaycoderscount}}</div>' +
      //'<span>{{$rootScope.mdata.formmode}}</span>' +
       '<div>' +
       '</div>';

    return {
        restrict: "A",
        replace: true,
        template: boxTemplate,
        scope: {
            value: "=outcomeBoxOld"
            //count: "=hitCount",
            //isEnabled: "=isEnabled"
        },

        link: function (scope, elem, attrs) {
            // this works.
            //console.log(" In link : " + $rootScope.mode);
            //console.log(" In link : " + $scope.mdata.formmode);
            scope.view.presscount = 0;
            scope.view.iname = attrs.name;
            scope.view.enabled = attrs.isEnabled == "yes" ? true : false;
            scope.view.isChecked = attrs.isChecked == "yes" ? true : false;
            scope.value = {};
            scope.value.isChecked = attrs.isChecked == "yes" ? true : false;

            // modelcolorState is used as a model tracker for validation etc. It Uses literal values like "Transparent", "DarkGreen", "Red", "Yellow" and "LightGreen"
            // originalcolorState is used for rendering the right CSS and maintaining the original color in Consensus 
            // codercount is also maintained for consensus.
            scope.value.modelcolorState = "";
            scope.value.originalcolorState = "";

            if(scope.view.isChecked){
                scope.view.checked = true;
                scope.view.colorstate = "onstate icheckbox_line-tax checked";
               
            }
            else{

            }


           
           
            scope.isEnabled = attrs.isEnabled;
            if (scope.isEnabled == "no") {
                scope.view.disabled = true;
                scope.view.classValue = "box-disabled";
                scope.value.modelcolorState = "Disabled";
            }
            
            scope.getTupleCount = function(codervals) 
            {
                if(codervals !== undefined){
                x = codervals.split(",");
                retval = (x.length>0) ? x.length : 0;
                }
                else{
                    retval = 0;
                }
                return retval;
            }

            scope.getComparisonTupleCount = function(codervals) 
            {
              retval = 0;
                if(codervals !== undefined){
                x = codervals.split(",");
                if(x.length ==1 ){
                    if(x[0].indexOf("o-") != -1) retval = 1;
                    if(x[0].indexOf("c-") != -1) retval = 2;
                    }
                 if(x.length ==2 ){
                    retval = 3;
                    }

                }
                return retval;
            }

            scope.getComparisonColor = function( countval )
            {
                retval = "";
                switch ( countval )
                {
                    case 1 :
                        retval = "Blue";
                        break;
                    case 2 :
                        retval = "Orange";
                        break;
                    case 3 :
                        retval = "LightGreen";
                        break;

                        }
                    return retval;             
            }

            scope.displayComparisons = function ( codervals )
            {
                //retval = ["1","2"]; // testing only.
                retval = ["",""];
                if(codervals !== undefined){
                x = codervals.split(",");
                
                if(x.length ==1 ){
                    if(x[0].indexOf("o-") != -1) {
                        retval[0] = x[0].replace("o-","");
                        retval[1] = "";
                        }
                    if(x[0].indexOf("c-") != -1) {
                        retval[0] = "";
                        retval[1] = x[0].replace("c-","");
                        }
                    }
                 if(x.length ==2 ){

                        if(x[0].indexOf("o-") != -1) {
                         retval[0] = x[0].replace("o-","");
                         }
                         else{
                         retval[1] = x[0].replace("c-","");
                         }

                         if(x[1].indexOf("o-") != -1) {
                         retval[0] = x[1].replace("o-","");
                         }
                         else{
                         retval[1] = x[1].replace("c-","");
                         }


                    }

                }

                //console.log( x + '       ' + retval);
                if (retval[0] != "") retval[0] = "ODP";
                if (retval[1] != "") retval[1] = "CODER";


                return retval;


            }


            scope.getColor = function( countval )
            {
                retval = "";
                switch ( countval )
                {
                    case 1 :
                        retval = "Red";
                        break;
                    case 2 :
                        retval = "Yellow";
                        break;
                    case 3 :
                        retval = "LightGreen";
                        break;

                        }
                    return retval;             
            }

            scope.value.resetBox = function()
            {

                if( scope.value.modelcolorState != "Disabled"){
                    codercount = scope.value.codercount;
                    scope.value.isChecked = false;
                    scope.view.checked = false;
                    scope.view.colorstate = scope.value.originalcolorState;
                    if( codercount > 0){

                       if($rootScope.mode.indexOf("Consensus") != -1) {
                        scope.value.modelcolorState = scope.getColor( codercount );}

                        if($rootScope.mode.indexOf("Comparison") != -1) {
                        scope.value.modelcolorState = scope.getComparisonColor( codercount );}

                    }
                 }


            }

            // this happens in the link phase.
            // this where the initial color states are being set for consensus.
            if($rootScope.mode.indexOf("Consensus") != -1 && !scope.view.disabled /* && $rootScope.displaymode != "View" */)
            {
                if( attrs.showCoders != "" && attrs.showCoders !== undefined){
                     //console.log(" show coders attribute : " + attrs.showCoders );
                     codercount = scope.getTupleCount( attrs.showCoders);
                     scope.view.consensus = true;
                     scope.view.comparers = false;
                     scope.view.codervals = attrs.showCoders + codercount.toString();
                     scope.view.displaycoders = attrs.showCoders.split(",");
                     scope.view.displaycoderscount = codercount;
                     if($rootScope.displaymode != "View")
                     {
                         scope.view.colorstate = "outcomeBoxColor-"+ codercount;
                         scope.value.originalcolorState = "outcomeBoxColor-"+ codercount;
                         // model value being set ::
                         scope.value.codercount = codercount;
                         scope.value.modelcolorState = scope.getColor( codercount );
                     }

                 }
                 else
                 {
                      //console.log("No show coders attributes being set.");
                      //console.log(" show coders attribute : " + attrs.showCoders );
                      scope.value.modelcolorState = "Transparent";
                      scope.value.originalcolorState = "";
                      scope.value.codercount = 0;
                 }
            }

            // this happens in the link phase.
            // this where the initial color states are being set for comparison.
            if($rootScope.mode.indexOf("Comparison") != -1 && !scope.view.disabled /*&& $rootScope.displaymode != "View" */)
            {
                if( attrs.showComparers != "" && attrs.showComparers !== undefined){
                     //console.log(" show coders attribute : " + attrs.showCoders );
                     codercount = scope.getComparisonTupleCount( attrs.showComparers);
                     scope.view.consensus = false;
                     scope.view.comparers = true;
                     scope.view.codervals = attrs.showComparers + codercount.toString();
                     scope.view.displaycoders = scope.displayComparisons( attrs.showComparers );
                     scope.view.displaycoderscount = codercount;
                     if($rootScope.displaymode != "View")
                     {
                         scope.view.colorstate = "comparisonoutcomeBoxColor-"+ codercount;
                         scope.value.originalcolorState = "comparisonoutcomeBoxColor-"+ codercount;
                         // model value being set ::
                         scope.value.codercount = codercount;
                         scope.value.modelcolorState = scope.getComparisonColor( codercount );
                     }

                 }
                 else
                 {
                      //console.log("No show coders attributes being set.");
                      //console.log(" show coders attribute : " + attrs.showCoders );
                      scope.value.modelcolorState = "Transparent";
                      scope.value.originalcolorState = "";
                      scope.value.codercount = 0;
                 }
            }




        },

        controller: function ( $scope, $element, $attrs) {
           
            $scope.view = {
                modelValue: $scope.value,
                editorEnabled: false,
                hitCount: 0,
                enabled: true

            };
            // this works.
            

          

            $scope.updatestate = function () {
                console.log("update state ::" + $attrs.isEnabled);
                // this works.
                console.log(" In controller : " + $rootScope.mode);
                console.log(" In controller : " + $rootScope.displaymode);
                if( $rootScope.displaymode == "View" ){
                        return; // return to update states allowed.
                        }
                if ($scope.view.enabled) {

                    $scope.view.presscount++;

                    if($rootScope.mode.indexOf("Consensus") != -1){
                        if($scope.value.modelcolorState == "Transparent" && $scope.value.codercount !=0 ){
                             $scope.value.isChecked = false;
                             $scope.view.checked = false;
                             $scope.value.modelcolorState = $scope.getComparisonColor( codercount );
                             $scope.view.colorstate = $scope.value.originalcolorState;
                             return;
                        }
                    }

                     if($rootScope.mode.indexOf("Comparison") != -1){
                        if($scope.value.modelcolorState == "Transparent" && $scope.value.codercount !=0 ){
                             $scope.value.isChecked = false;
                             $scope.view.checked = false;
                             $scope.value.modelcolorState = $scope.getColor( codercount );
                             $scope.view.colorstate = $scope.value.originalcolorState;
                             return;
                        }
                    }

                    $scope.view.checked = !$scope.view.checked;
                    if ($scope.view.checked) {
                        $scope.view.colorstate = "onstate icheckbox_line-tax checked";
                         $scope.value.isChecked = true;
                         $scope.value.modelcolorState = "SolidGreen";
                    }
                    else {
                        $scope.view.colorstate = "";
                        $scope.value.isChecked = false;
                        $scope.value.modelcolorState = "Transparent";
                    }

                }
               

            };
        }
    };
});

app.directive("clickToEditArea", function() {
    var editorTemplate = '<div class="click-to-edit">' +
        '<div ng-hide="view.editorEnabled">' +
            '{{value}} ' +
            '<a ng-click="enableEditor()">Edit</a>' +
        '</div>' +
        '<div ng-show="view.editorEnabled">' +
            '<input ng-model="view.editableValue">' +
            '<a href="#" ng-click="save()">Save</a>' +
            ' or ' +
            '<a ng-click="disableEditor()">cancel</a>.' +
        '</div>' +
    '</div>';

    return {
        restrict: "A",
        replace: true,
        template: editorTemplate,
        scope: {
            value: "=clickToEdit",
        },
        controller: function($scope) {
            $scope.view = {
                editableValue: $scope.value,
                editorEnabled: false
            };

            $scope.enableEditor = function() {
                $scope.view.editorEnabled = true;
                $scope.view.editableValue = $scope.value;
            };

            $scope.disableEditor = function() {
                $scope.view.editorEnabled = false;
            };

            $scope.save = function() {
                $scope.value = $scope.view.editableValue;
                $scope.disableEditor();
            };
        }
    };
});



app.directive("clickToEdit", function() {
    var editorTemplate = '<div class="click-to-edit">' +
        '<div ng-hide="view.editorEnabled">' +
            '{{value}} ' +
            '<a ng-click="enableEditor()">Edit</a>' +
        '</div>' +
        '<div ng-show="view.editorEnabled">' +
            '<input ng-model="view.editableValue">' +
            '<a href="#" ng-click="save()">Save</a>' +
            ' or ' +
            '<a ng-click="disableEditor()">cancel</a>.' +
        '</div>' +
    '</div>';

    return {
        restrict: "A",
        replace: true,
        template: editorTemplate,
        scope: {
            value: "=clickToEdit",
        },
        controller: function($scope) {
            $scope.view = {
                editableValue: $scope.value,
                editorEnabled: false
            };

            $scope.enableEditor = function() {
                $scope.view.editorEnabled = true;
                $scope.view.editableValue = $scope.value;
            };

            $scope.disableEditor = function() {
                $scope.view.editorEnabled = false;
            };

            $scope.save = function() {
                $scope.value = $scope.view.editableValue;
                $scope.disableEditor();
            };
        }
    };
});