/**
 * configure RequireJS
 * prefer named modules to long paths, especially for version mgt
 * or 3rd party libraries
 */
require.config({

    //disable caching

    //live mode
    //urlArgs: "bust=v2",

    //dev mode
    urlArgs: "bust=" + (new Date()).getTime(),

    paths: {
        'angular': '../lib/angular/angular',
        'angular-sanitize': '../lib/angular-sanitize/angular-sanitize.min',
        'angular-route': '../lib/angular-route/angular-route',
        'angular-animate': '../lib/angular-animate/angular-animate.min',
        'domReady': '../lib/requirejs-domready/domReady',
        'angularStrap': '../lib/angular-strap/angular-strap.min',
        'ui-bootstrap': '../lib/ui-bootstrap/ui-bootstrap-tpls-0.10.0',
        'jquery': '../lib/jquery/jquery.min',
        'bootstrapjs': '../lib/bootstrap/bootstrap.min',
        'jqueryChosen': '../lib/chosen/chosen.jquery.min',
        'angularFileUploadShim': '../lib/directives/angular-file-upload-shim',
        'chosenDirective': '../lib/directives/chosen',
        //'angucompleteDirective': '../lib/directives/angucomplete',
        'ngTable': '../lib/directives/ng-table.src',
        //'ngQuickDate': '../lib/directives/ng-quick-date',
        'angularFileUpload': '../lib/directives/angular-file-upload',
        'ngStorage': '../lib/ngStorage/ngStorage.min',
        'respond': '../lib/respond/respond.min',
        'angular-translate': '../lib/angular-translate/angular-translate.min',
        'angular-translate-loader-static-files': '../lib/angular-translate/angular-translate-loader-static-files.min',
        'nanoscroll': '../lib/nanoscroller/jquery.nanoscroller.min',
        //'monospaced.elastic': 'directives/elastic'        
    },

    /**
     * for libs that either do not support AMD out of the box, or
     * require some fine tuning to dependency mgt'
     */
    shim: {
        //'angular': {
        //    deps: ['angularFileUploadShim'],
        //    exports: 'angular'
        //},
        //'angular-route': {
        //    deps: ['angular']
        //},
        //'ngTable': ['angular'],
        'angular': { deps: ['angularFileUploadShim', 'jquery'], 'exports': 'angular' },
        'angular-sanitize': ['angular'],
        'angular-route': ['angular'],
        'angular-animate': ['angular'],
        //'angularResource': ['angular'],
        'chosenDirective': ['angular'],
        //'angucompleteDirective': ['angular'],
        'ngTable': ['angular'],
        //'ngQuickDate': ['angular'],
        'angularFileUpload': ['angular'],
        'angularStrap': ['angular'],
        'ui-bootstrap': ['angular'],
        'ngStorage': ['angular'],
        'angular-translate': ['angular'],
        'angular-translate-loader-static-files': ['angular-translate'],
        'jqueryChosen': ['jquery'],
        'nanoscroll':['jquery'],
        'bootstrapjs': ['jquery']//,
        //'monospaced.elastic': ['angular']
        },
    //priority: ['angularFileUploadShim', 'angular'],

    deps: [
        // kick start application... see bootstrap.js
        './bootstrap'
    ]
});
