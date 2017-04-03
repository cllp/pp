define(['./module'], function(controllers) {
    'use strict';
    controllers.controller('DashboardController', ['$scope', '$rootScope', '$timeout', '$filter', 'ContentFactory', 'ApplicationFactory', '$location', 'ApplicationService',
        function($scope, $rootScope, $timeout, $filter, confac, appfac, $location, appsvc) {

            $scope.tabs = [{
                    title: 'Favoriter',
                    template: 'Templates/dashboard/my_favourites.html'
                }, {
                    title: 'Mina Projekt',
                    template: 'Templates/dashboard/my_projects.html'
                }, {
                    title: 'Alla Projekt',
                    template: 'Templates/dashboard/all_projects.html'
                }, {
                    title: 'Arkiv',
                    template: 'Templates/dashboard/archived.html'
                }, {
                    title: 'Rapporter',
                    template: 'Templates/dashboard/reports.html'
                }];

            $scope.activeTab = 0;

            //execute method if token is changed and not undefined
            $rootScope.$watch('token', function() {
                //Watch for when token is set. Then init data.
                if ($rootScope.token !== undefined) {
                    getprojectViewList();
                }
            });

            //on dashboard load, exeute method if token is set.
            if ($rootScope.token !== undefined) {
                getprojectViewList();
                getReports();
            }


            function getprojectViewList() {
                confac.GetProjectList()
                    .success(function(projects) {
                        $rootScope.projectView = projects;
                    })
                    .error(function(error) {
                        $rootScope.status = 'Hämtning av projektlista misslyckades: ' + error.message;
                    });
            };

            function getReports() {
                confac.GetReports()
                    .success(function(reports) {
                        $scope.reports = reports;
                        console.log(JSON.stringify($scope.reports));
                    })
                    .error(function(error) {
                        $rootScope.status = 'Hämtning av rapporter misslyckades: ' + error.message;
                    });
            };

            $scope.getReport = function (name) {
                console.log(name);

                confac.GetReport(name)
                    .success(function(reportModel) {
                      
                      reportModel.Url = appsvc.apiroot() + 'report/' + reportModel.FileName;
                      $scope.currentReport = reportModel;
                      console.log('Report: ' + JSON.stringify(reportModel));

                    })
                    .error(function(error) {
                      $rootScope.status = 'Hämtning av rapport misslyckades: ' + error.message;
                });
          }

        }
    ]);

});