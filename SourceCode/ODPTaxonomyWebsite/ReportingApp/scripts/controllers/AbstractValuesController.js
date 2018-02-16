(function () {
    'use strict';

    angular
      .module('reportingapp')
      .controller('AbstractValuesController', AbstractValuesController);

    AbstractValuesController.$inject = ['$http', '$log', '$q', '$window', 'ROOT_URL', 'report', '$scope', '$timeout', '$filter'];

    function AbstractValuesController($http, $log, $q, $window, ROOT_URL, report, $scope, $timeout, $filter) {
        var vm = this;

        vm.fySelect = "";

        vm.runAbstractValuesReport = function (e) {
            e.preventDefault();
            e.stopPropagation();

            report.runAbstractValuesReport(vm.fySelect)
                   .then(function (response) {

                   }, vm.onerror);

        };

        vm.checkFormValid = function () {
            return vm.fySelect.length > 0
        };
    }

})();