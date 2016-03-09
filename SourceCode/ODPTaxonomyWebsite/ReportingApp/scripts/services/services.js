(function () {
    'use strict';

    angular
      .module('reportingapp')
      .factory('report', ['$http', '$log', '$q', '$window', 'ROOT_URL', function ($http, $log, $q, $window, root_url) {

          var objService = {

              getDateRanges: function () {
                  var deferred = $q.defer();
                  $http({
                      method: 'GET',
                      url: root_url + '/ReportingApp/handlers/ReportService.ashx?type=' + 'dates'
                  })
                    .success(function (response) {
                        //if (response.Success == true) {
                            deferred.resolve({
                                data: response
                            });
                        //}
                    }).error(function (msg, code) {
                        deferred.reject({ msg: msg, code: code });
                        $log.error(msg, code);
                    });
                  return deferred.promise;
              }

          }

          return objService;

      }]);

})();