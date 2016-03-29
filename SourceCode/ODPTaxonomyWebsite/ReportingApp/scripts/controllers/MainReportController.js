(function () {
    'use strict';

    angular
      .module('reportingapp')
      .controller('MainReportController', MainReportController);

    MainReportController.$inject = ['$http', '$log', '$q', '$window', 'ROOT_URL', 'report', '$scope', '$timeout', '$state'];

    function MainReportController($http, $log, $q, $window, ROOT_URL, report, $scope, $timeout, $state) {
        var vm = this;

        vm.tabs = [

            { "type": "kappa", title: "Kappa Report", panestate: "kappareport" },
            { "type": "summary", title: "Abstract Summary Report", panestate: "summaryreport" }
        ];

        vm.Go = function (state) {
            $state.go(state);
        }

        vm.Active = function (state) {
            return $state.is(state);
        };

    }

})();