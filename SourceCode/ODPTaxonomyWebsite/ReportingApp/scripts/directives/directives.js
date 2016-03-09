(function () {
    'use strict';
    // http://jsfiddle.net/cojahmetov/3DS49/
    angular
      .module('reportingapp')
      .directive('bsDropdown', function ($compile) {
          return {
              restrict: 'E',
              scope: {
                  items: '=dropdownData',
                  doSelect: '&selectValx',
                  selectedItem: '=preselectedItem',
                  dropText: '='
              },
              link: function (scope, element, attrs) {
                  var html = '';
                  switch (attrs.menuType) {
                      case "button":
                          html += '<div class="btn-group"><button class="btn btn-default btn-main" data-toggle="dropdown">{{dropText}}</button><button class="btn btn-info btn-default dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>';
                          break;
                      default:
                          html += '<div class="dropdown"><a class="dropdown-toggle" role="button" data-toggle="dropdown"  href="javascript:;">Dropdown<b class="caret"></b></a>';
                          break;
                  }
                  html += '<ul class="dropdown-menu"><li ng-repeat="item in items"><a tabindex="-1" ng-click="selectVal(item)">{{item.name}}</a></li></ul></div>';
                  element.append($compile(html)(scope));
                  if (scope.items) {
                      for (var i = 0; i < scope.items.length; i++) {
                          if (scope.items[i].id === scope.selectedItem) {
                              scope.bSelectedItem = scope.items[i];
                              break;
                          }
                      }
                      scope.selectVal(scope.bSelectedItem);
                  }
                  scope.selectVal = function (item) {
                      switch (attrs.menuType) {
                          case "button":
                              $('button.btn-main', element).html(item.name);
                              break;
                          default:
                              $('a.dropdown-toggle', element).html('<b class="caret"></b> ' + item.name);
                              break;
                      }
                      scope.doSelect({
                          selectedID: item.id, selectedVal : item.name
                      });
                  };
                  
              }
          };
      });

})();