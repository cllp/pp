define(['../module', 'angular', 'ngTable'], function (controllers, ng) {
    'use strict';
    controllers.controller('PlanController',
        ['$scope', '$rootScope', 'ngTableParams', 'ContentFactory', 'AlertFactory', '$filter', '$log', '$location', '$anchorScroll',
        function ($scope, $rootScope, ngTableParams, contentFactory, AlertFactory, $filter, $log, $location, $anchorScroll) {
            
            //Risk analysis table
            $scope.consequence = [{ name: "Låg", id: 1, value:1 }, { name: "Mellan", id: 2, value: 2 }, { name: "Hög", id: 3, value:3 }];
           // $scope.selectedConsequence = $scope.consequence[0];
            $scope.probability = [{ name: "Låg", id: 1, value: 1 }, { name: "Mellan", id: 2, value: 2 }, { name: "Hög", id: 3, value: 3 }];
           // $scope.selectedProbability = $scope.probability[0];            


            $scope.$watch('project.Risks', function(newValue) {

                angular.forEach($rootScope.project.Risks, function(value, index) {
                    var consequence = parseInt(newValue[index].ConsequenceId);
                    var probability = parseInt(newValue[index].ProbabilityId);

                    var result = 0;
                    if (consequence == 1 && probability == 1) {
                        result = 1;
                    }
                    if (consequence == 1 && probability != 1) {
                        result = probability;
                    }
                    if (probability == 1 && consequence != 1) {
                        result = consequence;
                    }
                    if (probability != 1 && consequence != 1) {
                        result = probability * consequence;
                    }
                    if (isNaN(consequence) || isNaN(probability)) {
                        result = 0;
                    }
                    $rootScope.project.Risks[index].Effect = result;
                });
            }, true);            

                
            // table add row
            $scope.addRow = function () {
                $rootScope.project.Risks.push({ Name: '', ConsequenceId: 0, ProbabilityId: 0, Effect: 0 });
            };

            $scope.saveRow = function saveRow (selectedRow) {
                var selectedRisk = $rootScope.project.Risks[selectedRow];
                var riskModel = {Id: selectedRisk.Id, Name: selectedRisk.Name, ProjectId: $rootScope.project.Id, ConsequenceId: selectedRisk.ConsequenceId, ProbabilityId: selectedRisk.ProbabilityId,Effect: selectedRisk.Effect };
                if (riskModel.Id != null || riskModel.Id > 0)
                {
                    contentFactory.UpdateRisk(riskModel)
                        .success(function () {
                            $scope.saveFade('risk', 'table', selectedRow);

                            // Update ok.
                            // Notify for update color removal.
                            $scope.enableSave(false, selectedRisk);
                        })
                        .error(function (error) {
                            AlertFactory.add('danger', 'Det misslyckades att uppdatera projektrisk.');
                           // $rootScope.status = 'Det misslyckades att uppdatera projektrisk';
                        });

                } else {
                    
                contentFactory.CreateRisk(riskModel)
                    .success(function (id) {
                        $scope.saveFade('risk', 'table', selectedRow);
                        $rootScope.project.Risks[selectedRow].Id = id;
                        $scope.enableSave(false, selectedRisk);
                       
                        // Create ok.
                    })
                    .error(function (error) {
                        AlertFactory.add('danger', 'Det misslyckades att lägga till projektrisk.');
                       // $rootScope.status = 'Det misslyckades att lägga till projektrisk';
                    });
                }

            };
            // Table delete row
            $scope.deleteRow = function deleteRow(selectedRow) {
                var confirmed = confirm("Bekräfta borttagning av rad.");
                if (confirmed) {
                    var selectedRisk = $rootScope.project.Risks[selectedRow];
                    contentFactory.DeleteRisk(selectedRisk.Id)
                        .success(function () {
                            $rootScope.project.Risks.splice(selectedRow, 1);
                        })
                        .error(function (error) {
                            AlertFactory.add('danger', 'Det misslyckades att tabort projektrisk.');
                            //  $rootScope.status = 'Det misslyckades att tabort projektrisk';
                        });
                }

            };
             // end risk analysis table

            //read only attributes
            $scope.getProbability = function(item) {
                var probability = $filter('filter')($scope.probability, { id: item.ProbabilityId }, true);
                if (probability.length == 0) {
                    return "";
                }
                return probability[0].name;
            };

            $scope.getConsequence = function(item) {
                var consequence = $filter('filter')($scope.consequence, { id: item.ConsequenceId }, true);
                if (consequence.length == 0) {
                    return "";
                }
                return consequence[0].name;
            };
        }]);
});
