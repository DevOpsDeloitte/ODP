(function () {
    'use strict';

    angular
      .module('reportingapp', [
        'ngRoute'
      ])
      .config(configFunction);
      //.run(runFunction);

    configFunction.$inject = ['$routeProvider'];

    function configFunction($routeProvider) {
        $routeProvider.otherwise({
            redirectTo: '/report'
        });

        $routeProvider.when('/report', {
            templateUrl: 'scripts/templates/report.html',
            controller: 'ReportController',
            controllerAs: 'vm'
        });

        $routeProvider.when('/report2', {
            templateUrl: 'scripts/templates/report.html',
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