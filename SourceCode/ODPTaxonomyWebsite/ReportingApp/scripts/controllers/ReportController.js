(function () {
    'use strict';

    angular
      .module('reportingapp')
      .controller('ReportController', ReportController);

    ReportController.$inject = ['$http', '$log', '$q', '$window','ROOT_URL', 'report'];

    function ReportController($http, $log, $q, $window,  ROOT_URL, report) {
        var vm = this;

        vm.defaultdateid = 1;
        //vm.dateranges = [{
        //    id: 1,
        //    name: "Low"
        //}];
        report.getDateRanges()
                    .then(function (response) {                    
                        vm.dateranges = response.data.map(function (x)
                        {
                            x.id = x.QC_ID;
                            x.name = x.Dates_IQ_Coded;
                            return x;
                        });

                        $log.info("dates loaded ::" + JSON.stringify(response.data));
                        vm.loading = false;
                    }, onerror);

    }

    function onerror(response) {

        if (response.code == 404) {
            //alert('Invalid URl')
            return;
        }

    }

})();