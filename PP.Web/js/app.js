/**
 * loads sub modules and wraps them up into the main module
 * this should be used for top-level module definitions only
 */
define([
    'angular',  
    'angular-route',
    'angular-animate',
    './controllers/index',
    './directives/index',
    './filters/index',
    './services/index',
    './factories/index',
    './providers/index',
    'angular-sanitize',
    //External modules
    'chosenDirective',
    'jqueryChosen',
    'bootstrapjs',
    'angularStrap',
    'ui-bootstrap',
    //'angucompleteDirective',
    'ngTable',
    //'ngQuickDate',
    'angularFileUpload',
    'ngStorage',
    'respond',
    'angular-translate',
    'angular-translate-loader-static-files',
    'nanoscroll'


], function (angular) {
    'use strict';

    return angular.module('app', [
        'app.controllers',
        'app.directives',
        'app.filters',
        'app.services',
        'app.factories',
        'app.providers',
        'ngRoute',
        'ngSanitize',

         //External
        'localytics.directives',
        'mgcrea.ngStrap.modal',
        'mgcrea.ngStrap.tab',
        'mgcrea.ngStrap.tooltip',
        'mgcrea.ngStrap.helpers.dateParser',
        'mgcrea.ngStrap.datepicker',
        'mgcrea.ngStrap.navbar',
        'mgcrea.ngStrap.scrollspy',
        'mgcrea.ngStrap.affix',
        'mgcrea.ngStrap.helpers.dimensions',
        'ui.bootstrap.alert',
        'ui.bootstrap.datepicker',
        'ui.bootstrap.tpls',
        'ngTable',
        'angularFileUpload',
        'ngStorage',
        'pascalprecht.translate'
    ]);
});
