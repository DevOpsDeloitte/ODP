(function () {
    'use strict';

    angular
      .module('reportingapp', [
         'ui.router'
      ])
      .config(configFunction)
      .constant('ROOT_URL', '');

      //.run(runFunction);

    configFunction.$inject = ['$stateProvider', '$urlRouterProvider'];

    function configFunction($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/kappareport');
        //console.log(" routes setup ::");

        $stateProvider.state('kappareport', {
            url: '/kappareport',
            templateUrl: 'scripts/templates/report.html',
            controller: 'ReportController',
            controllerAs: 'vm'
        });

        $stateProvider.state('summaryreport', {
            url : '/summaryreport',
            templateUrl: 'scripts/templates/summary.html',
            controller: 'AbstractSummaryReportController',
            controllerAs: 'vm'
        });
    }

    //runFunction.$inject = ['$rootScope', '$location'];

    //function runFunction($rootScope, $location) {
    //    $rootScope.$on('$routeChangeError', function (event, next, previous, error) {
    //        if (error === "AUTH_REQUIRED") {
    //            $location.path('/');
    //        }
    //    });
    //}

})();