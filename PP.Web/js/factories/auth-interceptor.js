define(['./module'], function (factories) {
    'use strict';

    factories.factory('AuthInterceptor', ['$rootScope', '$window', '$q', function ($rootScope, $window, $q) {

        return {
            request: function (config) {
                config.headers = config.headers || {};
                if (sessionStorage.getItem('token')) {
                    config.headers.Authorization = 'Bearer ' + sessionStorage.getItem('token');

                    //console.log('headers: ' + config.headers);

                    //config.headers.useXDomain = true;
                    //delete config.headers.common['X-Requested-With'];
                    //$httpProvider.defaults.useXDomain = true;
                    //delete $httpProvider.defaults.headers.common['X-Requested-With'];

                    //console.log('AuthInterceptor: token. ' + sessionStorage.getItem('token'));
                }
                else {
                    console.log('AuthInterceptor: No Token');
                }

                return config || $q.when(config);
            },
            response: function (response) {
                if (response.status === 401) {
                    // TODO: Redirect user to login page.
                }
                return response || $q.when(response);
            }
        };

    }]);
});