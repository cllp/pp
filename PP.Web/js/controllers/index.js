/** attach controllers to this module
 * if you get 'unknown {x}Provider' errors from angular, be sure they are
 * properly referenced in one of the module dependencies in the array.
 * below, you can see we bring in our services and constants modules
 * which avails each controller of, for example, the `config` constants object.
 **/
define([
    './content-controller',
    './areas/projectidea-controller',
    './areas/finance-controller',
    './areas/members-controller',
    './areas/plan-controller',
    './areas/goal-controller',
    './areas/activity-controller',
    './areas/followup-controller',
    './areas/debriefing-controller',
    './areas/metadata-controller',
    './header-controller',
    './dashboard-controller',
    './create-controller',
    './allprojects-controller',
    './versionlist-controller',
    './comment-controller',
    './sidebar-controller',
    './version-controller',
    './administrator-controller'
    //'./administration/administrator-projects-controller',
    //'./administration/administrator-deletedprojects-controller',
    //'./administration/administrator-allusers-controller',
    //'./administration/administrator-coordinators-controller',
    //'./administration/administrator-programowners-controller'
    
], function() {});
