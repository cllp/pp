define(['./module'], function (factories) {
    'use strict';
    factories.factory('DataFactory', ['$http', function ($http, $scope) {
        return {
            Get: function(id) {
                $http.get('http://localhost/input/get').success(function(data, status) {
                    console.log(data);
                });
                return 'return from function: ' + id;
            }
        };
    }]);
});