define(['./module'], function (directives) {
    'use strict';
    
    /* Textbox directive for readonly mode */
    directives.directive('cmtTextbox', function () {
        return {
            restrict: 'E',
            template: '<div class="form-group" ng-hide="hide">' +
                          '<label ng-hide="{{label == null}}" class="control-label" for="{{id}}">{{label}}</label>' +
                          '<input ng-show="write" class="form-control" id="{{id}}" type="text" value="{{value}}" placeholder="{{placeholder}}">' +
                          '<span ng-show="read" class="cmt-textbox-readonly">{{value}}</span>' +
                        '</div>',
            scope: {
                id: '@',
                label: '@',
                value: '@',
                placeholder: '@',
                accesstype: '@',
                datatype: '@'
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