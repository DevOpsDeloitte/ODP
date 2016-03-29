(function () {
    'use strict';

    angular
      .module('reportingapp')
      .controller('AbstractSummaryReportController', AbstractSummaryReportController);

    AbstractSummaryReportController.$inject = ['$http', '$log', '$q', '$window', 'ROOT_URL', 'report', '$scope', '$timeout'];

    function AbstractSummaryReportController($http, $log, $q, $window, ROOT_URL, report, $scope, $timeout) {
        var vm = this;

        vm.defaultdateid = 1;

        vm.errormessage = {};

        report.getDateRanges()
                    .then(function (response) {                    
                        vm.dateranges = response.data.map(function (x)
                        {
                            x.id = x.QC_ID;
                            x.name = x.Dates_IQ_Coded;
                            return x;
                        });

                        vm.initForm();
                        
                        //$log.info("dates loaded ::" + JSON.stringify(response.data));
                        
                    }, vm.onerror);

        vm.initForm = function () {

            vm.dateranges.unshift({ id: 0, name: 'Select' });
            vm.datestart = vm.dateranges[0];
            vm.dateend = vm.dateranges[0];
            vm.ktype = "K9";

            if (vm.datestart.id != 0) {
                vm.formready = true;
            }

            vm.loading = false;

        };


        vm.runReport = function (e) {
            e.preventDefault();
            e.stopPropagation();

            report.runReport(vm.datestart.name, vm.dateend.name, vm.ktype)
                   .then(function (response) {
                      
                   }, vm.onerror);

        };

        vm.runAbsSummaryReport = function (e) {
            e.preventDefault();
            e.stopPropagation();

            report.runAbstractSummaryReport()
                   .then(function (response) {

                   }, vm.onerror);

        };

        vm.checkForm = function () {
            //$log.info("check form ::");
            var retVal;


            if (vm.datestart !== undefined || vm.dateend !== undefined) {

                if (vm.datestart.id == 0) {
                    vm.errormessage = {};
                    vm.errormessage.enterstartandend = true;
                    vm.errormessage.enterstart = true; // set to true when there is an error, removed otherwise.
                    retVal = false;
                    return retVal;
                    
                }

                if (vm.dateend.id == 0) {
                    vm.errormessage = {};
                    vm.errormessage.enterstartandend = true;
                    vm.errormessage.enterend = true;
                    retVal = false;
                    return retVal;
                }

                if ((vm.datestart.id <= vm.dateend.id)) {
                    vm.errormessage = {};
                    
                    retVal = true;
                    return retVal;
                }
                else {
                    vm.errormessage = {};
                    vm.errormessage.startgreaterthanend = true;
                    retVal = false;
                    return retVal;
                }
            }
            else {
                vm.errormessage = {};
                vm.errormessage.enterstartandend = true;
                retVal = false;
                return retVal;
            }

            return retVal;
        };





        vm.onerror = function(response) {

            if (response.code == 404) {
                //alert('Invalid URl')
                return;
            }

        }

    }

    

})();