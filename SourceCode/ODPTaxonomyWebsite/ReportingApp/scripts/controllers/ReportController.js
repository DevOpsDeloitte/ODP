(function () {
    'use strict';

    angular
      .module('reportingapp')
      .controller('ReportController', ReportController);

    ReportController.$inject = ['$http', '$log', '$q', '$window','ROOT_URL', 'report', '$scope'];

    function ReportController($http, $log, $q, $window,  ROOT_URL, report, $scope) {
        var vm = this;

        vm.defaultdateid = 1;

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

            if (vm.datestart.id != 0) {
                vm.formready = true;
            }

            vm.loading = false;

        };

        //$scope.$watchCollection("vm.datestart", function (n, o) {
        //    if (n !== undefined) {
        //        if (n.id != 0) {
        //            vm.formready = true;
        //        }
        //    }
        //});

        vm.runReport = function () {

            report.runReport(vm.datestart.name, vm.dateend.name,'K9')
                   .then(function (response) {
                       
                   }, vm.onerror);

        };

        vm.checkForm = function () {
            //$log.info("check form ::")
            if (vm.datestart !== undefined && vm.dateend !== undefined) {
                if (vm.datestart.id == 0 || vm.dateend.id == 0) {
                    return false;
                }
                if (vm.datestart.id <= vm.dateend.id) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        };





        vm.onerror = function(response) {

            if (response.code == 404) {
                //alert('Invalid URl')
                return;
            }

        }

    }

    

})();