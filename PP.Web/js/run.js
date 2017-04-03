/**
 * Initializes the application
 */

define(['./app'], function(app) {
    'use strict';
    return app.run([
        '$location', '$log', '$http', '$timeout', '$route', '$routeParams', 'authentication', '$rootScope', '$anchorScroll', 'AccountFactory', 'ContentFactory', 'ApplicationFactory', 'ContentFactory', 'ApplicationService',
        function($location, $log, $http, $timeout, $route, $routeParams, authentication, $rootScope, $anchorScroll, accfac, confac, appfac, contentFactory, appsvc) {

            $rootScope.$watch('token', function() {
                //Watch for when token is set. Then init data.
                if ($rootScope.token !== undefined) {
                    init();
                }
            });

            $rootScope.showToolbar = function(type, item) {
                $rootScope.currentSideBar;
                $rootScope.toolbar = item;
                if ($rootScope.collapsed || $rootScope.currentSideBar != type) {
                    $rootScope.sidebartemplate = sideBarTemplateLogic(type);
                    $rootScope.currentSideBar = type;
                    $rootScope.collapsed = false
                } else {
                    $rootScope.collapsed = true;
                }
            };

            //decide which template to use in toolbar
            function sideBarTemplateLogic(type) {
                if (type == 'showVersions') {
                    $rootScope.publishVersion = false;
                    $rootScope.showVersions = true;
                    return "templates/versions_list.html";
                }
                if (type == 'publishVersion') {
                    $rootScope.publishVersion = true;
                    $rootScope.showVersions = false;
                    return "templates/versions_list.html";
                }
                if (type == 'showToolbar') {
                    return "templates/toolbar.html";
                }

            }

            /*
             * Global variables to be used by several controllers
             *
             */
            // Project content
            $rootScope.project = {};

            //used to display or hide textbox in finance-controller, finance table if stimulus is authorized
            $rootScope.rootscopeIsStimulusAuthorized = true;

            //used for display and calculations in fincnace-controller, funding table
            $rootScope.rootscopeFunding = {
                activityEstimate: 0,
                estimate: '',
                actual: '',
                stimulus: '',
                external: '',
                unfinanced: ''
            };

            $rootScope.loggedInUser = {};


            $rootScope.version = {
                Name: '',
                Version: '',
                Data: {}
            };

            function init() {
                getprojectPhases();
                getprojectAreas();
                getPrograms();
                getOrganizations();
                getProjectRoles();
                getApplicationSettings();
                getApplicationVersion();
                $rootScope.readOnly = false;
                //  getprojectViewList(); // collected in dashboard controller
                //get data that requires token.
            };


            function getprojectPhases() {
                var phaseStorage = accfac.GetStorage('phases');
                if (phaseStorage === null) {
                    confac.GetPhases()
                        .success(function(phases) {
                            $rootScope.Phases = phases;

                            accfac.SetStorage('phases', phases);
                        })
                        .error(function(error) {
                            $rootScope.status = 'Hämtning av projektfaser misslyckades: ' + error.message;
                        });
                } else {
                    $rootScope.Phases = phaseStorage;
                }

            }

            function getPrograms() {

                var programStorage = accfac.GetStorage('programs');
                if (programStorage === null) {
                    confac.GetPrograms()
                        .success(function(programs) {
                            $rootScope.Programs = programs;
                            accfac.SetStorage('programs', programs);
                        })
                        .error(function(error) {
                            $rootScope.status = 'Hämtning av program misslyckades: ' + error.message;
                        });
                } else {
                    $rootScope.Programs = programStorage;
                }
            }

            function getprojectAreas() {
                var areaStorage = accfac.GetStorage('areas');
                if (areaStorage === null) {
                    confac.GetAreas()
                        .success(function(areas) {
                            $rootScope.Areas = areas;
                            accfac.SetStorage('areas', areas);
                        })
                        .error(function(error) {
                            $rootScope.status = 'Hämtning av projektområden misslyckades: ' + error.message;
                        });
                } else {
                    $rootScope.Areas = areaStorage;
                }
            }

            function getOrganizations() {
                var organizationStorage = accfac.GetStorage('organizations');
                if (organizationStorage === null) {
                    confac.GetOrganizations()
                        .success(function(organizations) {
                            $rootScope.Organizations = organizations;
                            accfac.SetStorage('organizations', organizations);
                        })
                        .error(function(error) {
                            $rootScope.status = 'Hämtning av projektfaser misslyckades: ' + error.message;
                        });
                } else {
                    $rootScope.Organizations = organizationStorage;
                }

            }

            function getProjectRoles() {
                var roles = accfac.GetStorage('roles');
                if (roles === null) {
                    confac.GetProjectRoles()
                        .success(function(roles) {
                            $rootScope.ProjectRoles = roles;
                            accfac.SetStorage('roles', roles);
                        })
                        .error(function(error) {
                            $rootScope.status = 'Hämtning av projektroller misslyckades: ' + error.message;
                        });
                } else {
                    $rootScope.ProjectRoles = roles;
                }
            }


            function getApplicationSettings() {
                // Get short help texts
                appfac.GetSettings('HelpText').success(function(data, status) {
                    $rootScope.helpTexts = data;
                }).error(function(data, status) {
                    $log.error('Get helptexts failed.');
                });
                // Get finance convert ratios
                appfac.GetSettings('FinanceRatio').success(function(data, status) {
                    $rootScope.finaceRatios = data;
                }).error(function(data, status) {
                    $log.error('Get helptexts failed.');
                });
            }

            $rootScope.getproject = function getproject(projectId) {
                contentFactory.GetProject(projectId)
                    .success(function(project) {
                        $rootScope.tmpProject = project;
                        getPermissions(project);
                    })
                    .error(function(error) {
                        AlertFactory.add('danger', 'Hämtning av projektdetaljer misslyckades.');
                    });
            }

            function getPermissions(project) {
                var permission = appsvc.getpermission($rootScope.tmpProject, 'Project');
                if (permission < 2) {
                    $rootScope.readOnly = true;
                    getLatestVersion();
                } else {
                    $rootScope.readOnly = false;
                    $rootScope.project = project;
                }
                if (project.Phase === 'Arkiverad') {
                    $rootScope.readOnly = true;
                }
            }

            function getLatestVersion() {
                var projectId = $rootScope.tmpProject.Id;
                if (projectId != null) {
                    contentFactory.GetLatestVersion(projectId)
                        .success(function(projectVersion) {
                            if (projectVersion.Data !== null && projectVersion.Data !== 'undefined') {
                                $rootScope.project = projectVersion.Data;
                                $rootScope.project.PublishDate = projectVersion.PublishedDate;
                            } else {
                                $rootScope.ErrorInfo = {
                                    projectManagers: ($filter('filter')($scope.tmpProject.Members, 'Projektledare')),
                                    projectName: $scope.tmpProject.Name
                                }
                                $location.path('/error/');
                            }
                        })
                        .error(function(error) {
                            AlertFactory.add('danger', 'Hämtning av projektversion misslyckades.');
                        });
                }
            };

            function getApplicationVersion() {


                contentFactory.GetApplicationVersion()
                        .success(function(version) {
                            var cleanString = version.replace(/[|&;$%@"<>()+,.]/g, "");
                           $rootScope.appVersion = cleanString;
                           console.log(cleanString);
                        })
                        .error(function(error) {
                            AlertFactory.add('danger', 'Hämtning av applikationsversion misslyckades.');
                        });
               
            };



        }
    ]);
});
