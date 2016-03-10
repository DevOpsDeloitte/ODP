(function () {
    'use strict';

    angular
      .module('reportingapp')
      .controller('ReportController', ReportController);

    ReportController.$inject = ['$http', '$log', '$q', '$window','ROOT_URL', 'report'];

    function ReportController($http, $log, $q, $window,  ROOT_URL, report) {
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
                        
                        $log.info("dates loaded ::" + JSON.stringify(response.data));
                        
                    }, onerror);

        vm.initForm = function () {

            vm.dateranges.unshift({ id: 0, name: 'Select' });
            vm.datestart = vm.dateranges[0];
            vm.dateend = vm.dateranges[0];

            if (vm.datestart.id != 0) {
                vm.formready = true;
            }

            vm.loading = false;

        };

       





        function onerror(response) {

            if (response.code == 404) {
                //alert('Invalid URl')
                return;
            }

        }

    }

    

})();