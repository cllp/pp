define(['./module'], function(controllers) {
    'use strict';
    controllers.controller('VersionlistController', ['$scope', '$rootScope', '$window', '$filter', '$timeout', 'ContentFactory', 'ApplicationFactory', '$location', '$routeParams', 'ApplicationService', 'AlertFactory',
        function($scope, $rootScope, $window, $filter, $timeout, contentFactory, appfac, $location, $routeParams, appsvc, AlertFactory) {

            var VersionList = function(id) {
                contentFactory.GetVersionList(id)
                    .success(function(projectVersions) {

                        $('.sidebarScrollContent').height($(window).height() - 350);
                        $scope.versions = projectVersions;

                        $timeout(function() {
                            $(".nano").nanoScroller();
                        }, 0);
                    })
                    .error(function(error) {
                        $scope.versions = [];
                        AlertFactory.add('danger', 'Hämtning av projektversioner misslyckades.');
                    });
            };

            $scope.apiurl = appsvc.apiroot();

            //only get list of project verions if id in url is not undefined
            if (typeof($routeParams.id) !== 'undefined') {
                VersionList($routeParams.id);
            }

            $scope.redirectVersion = function(versionId) {
                if (versionId != null) {

                    contentFactory.GetVersion(versionId)
                        .success(function(projectVersion) {
                            $rootScope.version = projectVersion;
                            populateDropDowns();
                        })
                        .error(function(error) {
                            AlertFactory.add('danger', 'Hämtning av projektversion misslyckades.');
                        });
                }
            };

            $scope.publish = function() {
                loadVersionData();
                $scope.json = angular.toJson($scope.newVersion);
                if ($scope.json != null) {
                    publishVersion($scope.json);
                }
            };


            $scope.newVersion = {
                Id: '',
                ProjectId: '',
                PhaseId: '',
                PublishedDate: '',
                PublishedBy: '',
                PublishedByName: ''
            }

            function loadVersionData() {
                $scope.newVersion.Id = '';
                $scope.newVersion.ProjectId = $rootScope.project.Id;
                $scope.newVersion.PhaseId = $rootScope.project.PhaseId;
                $scope.newVersion.PublishedDate = new Date();
                $scope.newVersion.PublishedBy = $rootScope.loggedInUser.Id;
                $scope.newVersion.PublishedByName = $rootScope.loggedInUser.Name
            };

            function publishVersion(data) {
                contentFactory.PublishVersion(data)
                    .success(function(id) {
                        $rootScope.versionId = id;
                        $scope.newVersion.Comment = '';
                        VersionList($routeParams.id);
                    })
                    .error(function(error) {
                        AlertFactory.add('danger', 'Publicering av projekt misslyckades.');
                    });
            }

            // read only attributes projectidea
            function populateDropDowns() {
                if ($rootScope.version.Data.AreaId > 0) {
                    var projectarea = $rootScope.Areas[$rootScope.version.Data.AreaId];
                    if (projectarea !== null && projectarea !== undefined) {
                        $rootScope.projectAreaName = projectarea.Name;
                    }
                }
                if ($rootScope.version.Data.OrganizationId > 0) {
                    var organization = $filter('filter')($rootScope.Organizations, {
                        Id: $rootScope.version.Data.OrganizationId
                    }, true);
                    if (organization.length > 0) {
                        $rootScope.OrganizationName = organization[0].Name;
                    }
                }
            }

            //read only attributes risks
            //Risk analysis table
            $scope.consequence = [{
                name: "Låg",
                Id: 1,
                value: 1
            }, {
                name: "Mellan",
                Id: 2,
                value: 2
            }, {
                name: "Hög",
                Id: 3,
                value: 3
            }];
            $scope.probability = [{
                name: "Låg",
                Id: 1,
                value: 1
            }, {
                name: "Mellan",
                Id: 2,
                value: 2
            }, {
                name: "Hög",
                Id: 3,
                value: 3
            }];

            $scope.getProbability = function(item) {
                var probability = $filter('filter')($scope.probability, {
                    Id: item.ProbabilityId
                }, true);
                if (probability.length == 0) {
                    return "";
                }
                return probability[0].name;
            };

            $scope.getConsequence = function(item) {
                var consequence = $filter('filter')($scope.consequence, {
                    Id: item.ConsequenceId
                }, true);
                if (consequence.length == 0) {
                    return "";
                }
                return consequence[0].name;
            };
        }
    ]);
});
