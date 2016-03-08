(function () {
    'use strict';

    angular
      .module('reportingapp')
      .controller('ReportController', ReportController);

    ReportController.$inject = [];

    function ReportController() {
        var vm = this;

        vm.statuses = [{
            id: 1,
            name: "Low"
        }, {
            id: 2,
            name: "Normal"
        }, {
            id: 3,
            name: "High"
        }, {
            id: 4,
            name: "Urgent"
        }, {
            id: 5,
            name: "Immediate"
        }];
        vm.selected_status = 3;

        //alert("hello");
        //vm.parties = partyService.getPartiesByUser(user.uid);
    }

})();