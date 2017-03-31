(function () {
    'use strict';

    angular
      .module('reportingapp')
      .controller('ReportController', ReportController);

    ReportController.$inject = ['$http', '$log', '$q', '$window','ROOT_URL', 'report', '$scope', '$timeout', '$filter'];

    function ReportController($http, $log, $q, $window,  ROOT_URL, report, $scope, $timeout, $filter) {
        var vm = this;

        vm.defaultdateid = 1;
        vm.mechanisms = [];
        vm.selectedmechanisms = [];

        vm.errormessage = {};


        vm.startUp = function () {

            // complete both requests and proceed.
            $q.all({ dates: report.getDateRanges(), mechanisms: report.getMechanismTypes() })
            .then(function (result) {
                $log.info('promise result ', result);
                vm.dateranges = result.dates.data.map(function (x) {
                    x.id = x.QC_ID;
                    x.name = x.Dates_IQ_Coded;
                    return x;
                });
                vm.mechanismtypes = result.mechanisms.data.map(function (x) {
                    x.id = x.Mechanism_TypeID;
                    x.name = x.Mechanism_Type1;
                    x.selected = true;
                    return x;
                });

                vm.initForm();

            });


        }


        //$scope.$watch('vm.mechanismtypes|filter:{selected:true}', function (nv, ov) {
        $scope.$watch('vm.mechanismtypes', function (nv, ov) {
            vm.mechanismsall = true;
            vm.selectedmechanisms = [];
            if (nv === undefined) return;
            for(var i =0; i<nv.length; i++){
                if (nv[i].selected==false) {
                    vm.mechanismsall = false;
                    break;
                }
            }

            for (var i = 0; i < nv.length; i++) {
                if (nv[i].selected == true) {
                    vm.selectedmechanisms.push(nv[i].id + '-' + nv[i].name);
                }
            }

        }, true);


        vm.mechanismsValid = function () {
            if (vm.selectedmechanisms.length == 0) return false;
            else return true;
        };
 


        $scope.changeAll = function (m) {
            if (m === false) {
                //vm.mechanismsall = true; // commenting out temporarily.
                vm.selectedmechanisms = [];
                vm.mechanismsall = false;
                for (var i = 0; i < vm.mechanismtypes.length; i++) {
                    vm.mechanismtypes[i].selected = false;
                }

            }
            else {
                vm.selectedmechanisms = [];
                for (var i = 0; i < vm.mechanismtypes.length; i++) {
                    vm.mechanismtypes[i].selected = true;
                    vm.selectedmechanisms.push(vm.mechanismtypes[i].id + '-' + vm.mechanismtypes[i].name);
                }
            }
            }

        

        vm.startUp();

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

            report.runReport(vm.datestart.name, vm.dateend.name, vm.ktype, vm.selectedmechanisms.join(',') )
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

        vm.checkFormValid = function () {
            return vm.checkForm() && vm.selectedmechanisms.length > 0
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