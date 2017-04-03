/**
 * attach services to this module
 * if you get 'unknown {x}Provider' errors from angular, be sure they are
 * properly referenced in one of the module dependencies in the array.
 * below, you can see we bring in our services and constants modules 
 * which avails each service of, for example, the `config` constants object.
 **/
define([
    './auth-interceptor',
    './upload-factory',
    './data-factory',
    './application-factory',
    './account-factory',
    './alert-factory',
    './content-factory'
], function () { });

