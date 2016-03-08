(function () {
    'use strict';

    angular
      .module('reportingapp')
      .controller('ReportController', ReportController);

    ReportController.$inject = [];

    function ReportController() {
        var vm = this;
        //alert("hello");
        //vm.parties = partyService.getPartiesByUser(user.uid);
    }

})();