angular.element(document).ready(function() {
    angular.bootstrap(document, ["formApp"]);
    console.log("angular app started ...");
});

var $globdata;
var $globDebug;
var $team = {};
var $teamRT = {};
var $watchSyncCounter = 0;