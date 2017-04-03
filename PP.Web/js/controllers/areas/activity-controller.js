define(['../module', 'angular', 'ngTable'], function (controllers, ng) {
    'use strict';
    controllers.controller('ActivityController',
        ['$scope', '$rootScope', 'ngTableParams', 'ContentFactory', 'AlertFactory', '$filter', '$log', '$location', '$anchorScroll',
        function ($scope, $rootScope, ngTableParams, contentFactory, alerts, $filter, $log, $location, $anchorScroll) {

            $scope.statusList = [{ Name: "Ej startat", Id: 1 }, { Name: "Pågående", Id: 2 }, { Name: "Avslutad", Id: 3 }];

            // table add row
            $scope.addRow = function () {
                $rootScope.project.Activity.push({ Name: '', StatusId: 1, InternalHours: 0, ExternalHours: 0, Cost: 0, Notes: '' });
            };

            $scope.saveRow = function saveRow(selectedRow) {
                var selectedItem = $rootScope.project.Activity[selectedRow];
                var activityModel = { Id: selectedItem.Id, Name: selectedItem.Name, ProjectId: $rootScope.project.Id, StatusId: selectedItem.StatusId, InternalHours: selectedItem.InternalHours, ExternalHours: selectedItem.ExternalHours, Cost: selectedItem.Cost, Notes: selectedItem.Notes };
                // Check if this is a create or update
                if (activityModel.Id != null || activityModel.Id > 0) {
                    contentFactory.UpdateActivity(activityModel)
                        .success(function () {
                            // Update ok.
                            $scope.saveFade('activity', 'table', selectedRow);
                            $scope.enableSave(false, selectedItem);
                        })
                        .error(function (error) {
                            //  $rootScope.status = 'Det misslyckades att uppdatera projektaktivitet ' + error.message;
                            alerts.add('danger', 'Det misslyckades att uppdatera projektaktivitet.');
                        });

                } else {

                    contentFactory.CreateActivity(activityModel)
                        .success(function (id) {
                            $rootScope.project.Activity[selectedRow].Id = id;
                            $scope.saveFade('activity', 'table', selectedRow);
                            $scope.enableSave(false, selectedItem);
                            // Create ok.
                        })
                        .error(function (error) {
                            //  $rootScope.status = 'Det misslyckades att lägga till projektaktivitet: ' + error.message;
                            alerts.add('danger', 'Det misslyckades att lägga till projektaktivitet.');
                        });
                }
            };
            // Table delete row
            $scope.deleteRow = function deleteRow(selectedRow) {
                var confirmed = confirm("Bekräfta borttagning av rad.");
                if (confirmed) {
                    var selectedItem = $rootScope.project.Activity[selectedRow];
                    contentFactory.DeleteActivity(selectedItem.Id)
                        .success(function () {
                            $rootScope.project.Activity.splice(selectedRow, 1);
                        })
                        .error(function (error) {
                            //  $rootScope.status = 'Det misslyckades att tabort projektaktivitet: ' + error.message;
                            alerts.add('danger', 'Det misslyckades att lägga till projektaktivitet.');
                        });
                }

            };

            // to be collected from database


                



            //watch on the activity table, updates the total funding.
            $scope.$watch('project.Activity', function () {
                if ($rootScope.finaceRatios !== undefined) {
                    $scope.internalTimeCost = parseFloat($filter('filter')($rootScope.finaceRatios, { Name: 'financeInternalCost' })[0].Value);// 0.5;
                    $scope.externalTimeCost = parseFloat($filter('filter')($rootScope.finaceRatios, { Name: 'financeExternalCost' })[0].Value); // 1;
                }

                var totalInternal = 0;
                var totalExernal = 0;
                var totalMiscCost = 0;
                var total = 0; //this is the total calculation for all fields
                var totalExernalInternalTime = 0;

                angular.forEach($rootScope.project.Activity, function (value, index) {


                    if (value != null && value != 'undefined') {
                        var internalTime = parseInt(value.InternalHours);
                        var externalTime = parseInt(value.ExternalHours);
                        var cost = parseInt(value.Cost);
                        var internalTimeCost = $scope.internalTimeCost;
                        var externalTimeCost = $scope.externalTimeCost;

                        // These two if-statements are just a safeguard for undefined values.
                        //In order to prevent null value to be printed to gui 
                        if (isNaN(internalTimeCost)) {
                            internalTimeCost = 0;
                        }
                        if (isNaN(externalTimeCost)) {
                            externalTimeCost = 0;
                        }


                        if (!isNaN(internalTime) && !isNaN(externalTime) && !isNaN(cost)) {
                            var calculatedInternalCost = internalTime * internalTimeCost;
                            var calculatedExternalCost = externalTime * externalTimeCost;
                            total = total + (calculatedInternalCost + calculatedExternalCost + cost);
                            $rootScope.project.Activity.SummaryTotal = total;
                            $rootScope.rootscopeFunding.activityEstimate = total;
                        }

                        if (!isNaN(internalTime)) {
                            totalInternal += internalTime;
                            $rootScope.project.Activity.SummaryInternalHours = totalInternal;
                        }
                        if (!isNaN(externalTime)) {
                            totalExernal += externalTime;
                            $rootScope.project.Activity.SummaryExternalHours = totalExernal;
                        }
                        if (!isNaN(cost)) {
                            totalMiscCost += cost;
                            $rootScope.project.Activity.SummaryCost = totalMiscCost;
                        }


                        //sum of each row
                        var rowInternalTime = internalTime;
                        var rowExternalTime = externalTime;
                        var rowCost = cost;

                        if (!isNaN(rowInternalTime) && !isNaN(rowExternalTime) && !isNaN(rowCost)) {
                            var rowcalculatedInternalCost = rowInternalTime * internalTimeCost;
                            var rowcalculatedExternalCost = rowExternalTime * externalTimeCost;
                            var rowTotal = (rowcalculatedInternalCost + rowcalculatedExternalCost + rowCost);
                            $rootScope.project.Activity[index].SummaryRow = rowTotal;
                            //  $scope.activity.row[index].result = rowTotal;                                                     
                        }
                    }
                });
            }, true);

            // read only

            $scope.getstatus = function (item) {
                var status = $filter('filter')($scope.statusList, { Id: item.StatusId }, true);
                if (status.length == 0) {
                    return "";
                }
                return status[0].Name;
            }

        }]);

});
