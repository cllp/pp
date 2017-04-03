define(['./module'], function (directives) {
    'use strict';


    directives.directive('ppSingleSelect', function () {

    return {
        restrict: 'E',
        template: '<div class="form-group" ng-hide="hide">' +
                      '<label ng-hide="{{label == null}}" class="control-label" for="{{id}}">{{label}}</label>' +
                        '<select id="{{id}}" ng-change="searchoptions()" ng-model="model" ng-options="x.Name for x in optionsmodel" chosen data-placeholder="{{placeholder}}" style="width: 100%">' +
                        '<option value=""></option>' +
                      '</select>' +
                    '</div>',
        scope: {
            id: '@',
            label: '@',
            placeholder: '@',
            accesstype: '@',
            model: '=',
            optionsmodel: '='
        },
        replace: true,
        link: function (scope, elem, attrs) {

            if (scope.accesstype == null) {
                scope.accesstype = 'read'; //setting default value to accesstype
            }

            scope.hide = scope.accesstype == 'hide';
            scope.read = scope.accesstype == 'read';
            scope.write = scope.accesstype == 'write';
        }
    }
    });

});