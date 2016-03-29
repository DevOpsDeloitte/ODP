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

        $urlRouterProvider.otherwise('/report');
        console.log(" routes setup ::");

        $stateProvider.state('report', {
            url: '/report',
            templateUrl: 'scripts/templates/report.html',
            controller: 'ReportController',
            controllerAs: 'vm'
        });

        $stateProvider.state('report2', {
            url : '/report2',
            templateUrl: 'scripts/templates/summary.html',
            controller: 'ReportController',
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