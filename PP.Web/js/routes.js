/// <reference path="../Templates/version.html" />
/**
 * Defines the main routes in the application.
 * The routes you see here will be anchors '#/' unless specifically configured otherwise.
 */

define(['./app'], function(app) {
    'use strict';
    return app.config(['$routeProvider',
        function($routeProvider) {
            $routeProvider.when('/dashboard', {
                templateUrl: 'templates/pages/dashboard.html',
                controller: 'DashboardController'
            });
            $routeProvider.when('/administration', {
                templateUrl: 'templates/Administration/administrationDashboard.html',
                controller: 'AdministratorController'
            });
            $routeProvider.when('/settings', {
                templateUrl: 'templates/pages/settings.html',
                controller: 'SettingsController'
            });
            $routeProvider.when('/create', {
                templateUrl: 'templates/pages/create.html',
                controller: 'CreateController'
            });
            $routeProvider.when('/content/:id', {
                //$routeProvider.when('/content', {
                templateUrl: 'templates/content.html',
                controller: 'ContentController',
            }),
            $routeProvider.when('/version/:id', {
                //$routeProvider.when('/content', {
                templateUrl: 'templates/version.html',
                controller: 'ReadviewController',
            }),
            $routeProvider.when('/error/', {
                //$routeProvider.when('/content', {
                templateUrl: 'templates/errorpage.html'

            })
            $routeProvider.otherwise({
                redirectTo: '/dashboard'
            });
        }
    ]);
});