define(['./module', 'ngStorage'], function (providers) {
    'use strict';

    return providers.provider('authentication', ['$httpProvider', function ($httpProvider) {

        var BaseUrl = '';
        var tokenNamespace = 'token';
        var loggedInUserNamespace = 'loggedInUser';

        $httpProvider.interceptors.push(function ($q, $injector) {
            return {
                request: function (cfg) {
                    var token = $injector.get('authentication').getToken();
                    var matchesAPIUrl = cfg.url.substr(0, BaseUrl.length) === BaseUrl;

                    if (token && matchesAPIUrl) {
                        cfg.headers['Authorization'] = 'Bearer ' + token;
                    }
                    return cfg || $q.when(cfg);
                }
            };
        });

        function errorCallback(response) {
            var message = (response && response.data && response.data.message) ? response.data.message : '';
            console.log(message);
            //deferred.reject('Could not log you in. ' + message);
        }

        function finallyCallback() {
            console.log('Log in request finished.');
        }

        this.$get = function ($http, $localStorage, $log, $q) {
            var _logout = function () {
                delete $localStorage[tokenNamespace];
                delete $localStorage[loggedInUserNamespace];

                $log.info($localStorage[tokenNamespace]);

            },
                _getToken = function () {
                    return $localStorage[tokenNamespace];
                },
                _getUser = function () {
                    return $localStorage[loggedInUserNamespace];
                },
                _authenticate = function (username) {

                    var deferred = $q.defer();

                    var cfg = {
                        //method: 'POST',
                        //url: BaseUrl + 'token',
                        //data: 'grant_type=' + grantType + '&username=' + username + '&password=' + password,
                        method: 'GET',
                        url: 'api/account/authenticate/' + username,
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                    };

                    $http(cfg).then(function (response) {
                        if (response && response.data) {
                            var data = response.data;

                            $localStorage[loggedInUserNamespace] = data.User;
                            $localStorage[tokenNamespace] = data.AccessToken;
                            deferred.resolve(true);
                        }
                        else {
                            deferred.reject('No data received');
                        }
                    })
                    ['catch'](errorCallback);

                    return deferred.promise;

                },
                _isLoggedIn = function () {
                    return typeof $localStorage[tokenNamespace] == 'string';
                };

            return {
                isLoggedIn: _isLoggedIn,
                authenticate: _authenticate,
                getToken: _getToken,
                getUser: _getUser,
                logout: _logout
            }
        }
    }]);
});
