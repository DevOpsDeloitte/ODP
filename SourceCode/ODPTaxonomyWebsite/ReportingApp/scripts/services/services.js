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
              },

              getMechanismTypes: function () {
                  var deferred = $q.defer();
                  $http({
                      method: 'GET',
                      url: root_url + '/ReportingApp/handlers/ReportService.ashx?type=' + 'mechanismtypes'
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
              },

              runReport: function (start, end, ktype, mechanisms) {
                  var deferred = $q.defer();
                  var fileName = "test.csv";
                  //http://stackoverflow.com/questions/20904151/download-text-csv-content-as-files-from-server-in-angular
                  //if (window.navigator.msSaveOrOpenBlob) {
                 //     var blob = new Blob([response]);  //csv data string as an array.
                      // IE hack; see http://msdn.microsoft.com/en-us/library/ie/hh779016.aspx
                  //    window.navigator.msSaveBlob(blob, fileName);
                  //} else {
                      var anchor = angular.element('<a/>');
                      anchor.css({ display: 'block' }); // Make sure it's not visible
                      //$log.info(response);
                      angular.element(document.body).append(anchor); // Attach to document for FireFox

                      anchor.attr({
                          href: root_url + '/ReportingApp/handlers/ReportService.ashx?type=' + 'run' + '&start=' + start + '&end=' + end + '&ktype=' + ktype + '&mechanisms='+mechanisms
                          //target: '_blank'
                          
                      })[0].click();
                      deferred.resolve({
                                   success: true
                               });
                      //anchor.remove();
                  
                  return deferred.promise;
              },

              runAbstractSummaryReport: function () {
                  var deferred = $q.defer();
                  var fileName = "test.csv";
                  //http://stackoverflow.com/questions/20904151/download-text-csv-content-as-files-from-server-in-angular
                
                  var anchor = angular.element('<a/>');
                  anchor.css({ display: 'block' }); // Make sure it's not visible

                  angular.element(document.body).append(anchor); // Attach to document for FireFox

                  anchor.attr({
                      href: root_url + '/ReportingApp/handlers/ReportService.ashx?type=' + 'avgreport'
                      //target: '_blank'

                  })[0].click();
                  deferred.resolve({
                      success: true
                  });
                  
                  return deferred.promise;
              }



          }

          return objService;

      }]);

})();