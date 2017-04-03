define(['../module', 'angular', 'ngTable'], function (controllers, ng) {
    'use strict';
    controllers.controller('GoalController',
        ['$scope', '$rootScope', 'ngTableParams', 'ContentFactory', 'AlertFactory', '$filter', '$log', '$location', '$anchorScroll',
        function ($scope, $rootScope, ngTableParams, contentFactory, AlertFactory, $filter, $log, $location, $anchorScroll) {

            $scope.goalType = [{ name: "Projekt", id: 1 }, { name: "Effekt", id: 2 }];
            $scope.goalAchieved = [{ name: "Ja", id: 1 }, { name: "Nej", id: 2 }, { name: "Delvis", id: 3 }];
            $scope.getGoalType = function(item) {
                var goal = $filter('filter')($scope.goalType, { id: item.Type }, true);
                if (goal.length == 0) {
                    return "";
                }
                return goal[0].name;
            };
            $scope.getGoalAchieved = function(item) {
                var goal = $filter('filter')($scope.goalAchieved, { id: item.Achieved }, true);
                if (goal.length == 0) {
                    return "";
                }
                return goal[0].name;
            };

            // table add row
            $scope.addRow = function () {
                $rootScope.project.Goals.push({ Type: $scope.goalType, Name: '', GoalDefinition: '', MesaureMethod: '', Achieved: $scope.goalAchieved });
            };

            $scope.saveRow = function saveRow(selectedRow) {
                var selectedItem = $rootScope.project.Goals[selectedRow];
                var goalModel = { Type: selectedItem.Type, ProjectId: $rootScope.project.Id, Achieved: selectedItem.Achieved, GoalDefinition: selectedItem.GoalDefinition, Id: selectedItem.Id, MesaureMethod: selectedItem.MesaureMethod };
                // Check if this is a create or update
                if (goalModel.Id != null || goalModel.Id > 0) {
                    contentFactory.UpdateGoal(goalModel)
                        .success(function () {
                            // Update ok.
                            $scope.saveFade('goal', 'table', selectedRow);
                            $scope.enableSave(false, selectedItem);
                        })
                        .error(function (error) {
                            AlertFactory.add('danger', 'Det misslyckades att uppdatera projektmål.');
                          //  $rootScope.status = 'Det misslyckades att uppdatera projektmål ' + error.message;
                        });

                } else {

                    contentFactory.CreateGoal(goalModel)
                        .success(function (id) {
                            $rootScope.project.Goals[selectedRow].Id = id;
                            $scope.saveFade('goal', 'table', selectedRow);
                            $scope.enableSave(false, selectedItem);
                            // Create ok.
                        })
                        .error(function (error) {
                            AlertFactory.add('danger', 'Det misslyckades att lägga till projektmål.');
                            //$rootScope.status = 'Det misslyckades att lägga till projektmål: ' + error.message;
                        });
                }
            };
            // Table delete row
            $scope.deleteRow = function deleteRow(selectedRow) {
                var confirmed = confirm("Bekräfta borttagning av rad.");
                if (confirmed) {
                    var selectedItem = $rootScope.project.Goals[selectedRow];
                    contentFactory.DeleteGoal(selectedItem.Id)
                        .success(function () {
                            $rootScope.project.Goals.splice(selectedRow, 1);
                        })
                        .error(function (error) {
                            AlertFactory.add('danger', 'Det misslyckades att tabort projektmål.');
                            //  $rootScope.status = 'Det misslyckades att tabort projektmål: ' + error.message;
                        });
                };
            };
        }]);
});