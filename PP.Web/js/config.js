/**
 * Defines the main config of the application
 */

define(['./app'], function(app) {
  'use strict';
  return app.config(['$logProvider', '$httpProvider', '$scrollspyProvider', '$translateProvider', '$locationProvider',
    function($logProvider, $httpProvider, $scrollspyProvider, $translateProvider, $locationProvider) {

      $locationProvider.html5Mode(false);

      //register auth interceptor
      //setup bearer token for authentication
      $httpProvider.interceptors.push('AuthInterceptor');

      //disable debug logging
      $logProvider.debugEnabled(false);

      //enable cors in $http calls
      $httpProvider.defaults.useXDomain = true;
      delete $httpProvider.defaults.headers.common['X-Requested-With'];

      //angular.extend($scrollspyProvider.defaults, {
      //    animation: 'am-fade-and-slide-top'
      //});

      // Load resource file
      $translateProvider.useStaticFilesLoader({
        prefix: '/js/resources/resource-',
        suffix: '.json'
      });
      // Set language key
      $translateProvider.preferredLanguage('sv');

    }
  ]);
});