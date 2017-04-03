define(['./module'], function(factories) {
  'use strict';
  factories.factory('AccountFactory', ['$http', '$log', 'ApplicationService',
    function($http, $log, appsvc) {

      return {

        Authenticate: function(username, password) {
          $log.info('Authenticating user: ' + username + '. Password: ' + password);
          $log.info('ApiRoot: ' + appsvc.apiroot());

          var cfg = {
            method: 'POST',
            url: appsvc.apiroot() + 'token',
            headers: {
              'Content-Type': 'application/x-www-form-urlencoded'
            },
            data: 'grant_type=password&username=' + username + '&password=' + password
          };

          return $http(cfg);
        },

        GetProfile: function() {
          $log.info('Retreiving user profile from API');

          return $http.get(appsvc.apiroot() + 'account/profile');
        },

        GetUsername: function() {
          $log.info('Retreiving username from Auth API');

          return $http.get('auth/user/username');
        },

        RequestPasswordChange: function(email) {
          $log.info('RequestPasswordChange');
          return $http.post(appsvc.apiroot() + 'application/requestchangepassword/', email);
        },

        GetStorage: function(item) {
          $log.info('GetStorage: ' + item);

          if (item === 'token' || item === 'username' || item === 'password') {
            return sessionStorage.getItem(item);
          } else {
            return JSON.parse(sessionStorage.getItem(item));
          }

          $log.info('sessionStorage.getItem(item): ' + sessionStorage.getItem(item));

          return sessionStorage.getItem(item);
        },

        SetStorage: function(item, value) {
          if (item === 'token' || item === 'username') {
            return sessionStorage.setItem(item, value);
          } else {
            return sessionStorage.setItem(item, JSON.stringify(value));
          }

        },

        RemoveStorage: function(item) {
          return sessionStorage.removeItem(item);
        },

        ClearStorage: function() {
          Object.keys(sessionStorage)
          // .filter(function(k) { return /foo/.test(k); })
          .forEach(function(i) {
            console.log('Clearing item: ' + i);
            sessionStorage.removeItem(i);
          });
        }

      }

    }
  ]);
});