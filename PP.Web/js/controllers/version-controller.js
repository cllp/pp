define(['./module'], function(controllers) {
    'use strict';
    controllers.controller('VersionController', ['$scope', '$rootScope', '$window', '$filter', 'ContentFactory', 'ApplicationFactory', 'ApplicationService', '$location',
        function($scope, $rootScope, $window, $filter, contentFactory, appfac, appsvc, $location) {

            $scope.$watch('version.Data', function() {
                if ($rootScope.version.Data.Members !== undefined) {
                    $scope.ProjectLeaders = ($filter('filter')($rootScope.version.Data.Members, 'Projektledare'));
                    $scope.ProjectOwners = ($filter('filter')($rootScope.version.Data.Members, 'Projektägare'));
                }
            });

            $scope.apiurl = appsvc.apiroot();

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

            // to be collected from database
            $scope.$watch('finaceRatios', function() {
                if ($rootScope.finaceRatios !== undefined) {
                    $scope.internalTimeCost = $filter('filter')($rootScope.finaceRatios, {
                        Name: 'financeInternalCost'
                    })[0].Value; // 0.5;
                    $scope.externalTimeCost = $filter('filter')($rootScope.finaceRatios, {
                        Name: 'financeExternalCost'
                    })[0].Value; // 1;
                }
            }, true);

            //activity calculations
            $scope.$watch('version.Data.Activity', function() {
                var totalInternal = 0;
                var totalExernal = 0;
                var totalMiscCost = 0;
                var total = 0; //this is the total calculation for all fields
                var totalExernalInternalTime = 0;

                angular.forEach($rootScope.version.Data.Activity, function(value, index) {

                    if (value != null && value != 'undefined') {
                        var internalTime = parseInt(value.InternalHours);
                        var externalTime = parseInt(value.ExternalHours);
                        var cost = parseInt(value.Cost);

                        if (!isNaN(internalTime) && !isNaN(externalTime) && !isNaN(cost)) {
                            var calculatedInternalCost = internalTime * $scope.internalTimeCost;
                            var calculatedExternalCost = externalTime * $scope.externalTimeCost;
                            total = total + (calculatedInternalCost + calculatedExternalCost + cost);
                            $rootScope.version.Data.Activity.SummaryTotal = total;
                            $rootScope.rootscopeFunding.activityEstimate = total;
                        }

                        if (!isNaN(internalTime)) {
                            totalInternal += internalTime;
                            $rootScope.version.Data.Activity.SummaryInternalHours = totalInternal;
                        }
                        if (!isNaN(externalTime)) {
                            totalExernal += externalTime;
                            $rootScope.version.Data.Activity.SummaryExternalHours = totalExernal;
                        }
                        if (!isNaN(cost)) {
                            totalMiscCost += cost;
                            $rootScope.version.Data.Activity.SummaryCost = totalMiscCost;
                        }


                        //sum of each row
                        var rowInternalTime = internalTime;
                        var rowExternalTime = externalTime;
                        var rowCost = cost;

                        if (!isNaN(rowInternalTime) && !isNaN(rowExternalTime) && !isNaN(rowCost)) {
                            var rowcalculatedInternalCost = rowInternalTime * $scope.internalTimeCost;
                            var rowcalculatedExternalCost = rowExternalTime * $scope.externalTimeCost;
                            var rowTotal = (rowcalculatedInternalCost + rowcalculatedExternalCost + rowCost);
                            $rootScope.version.Data.Activity[index].SummaryRow = rowTotal;
                            //  $scope.activity.row[index].result = rowTotal;
                        }
                    }
                });
            }, true);


            //Followup calculations
            $scope.$watch('version.Data.FollowUp', function(value, index) {
                var totalInternal = 0;
                var totalExernal = 0;
                var totalmisc = 0;
                var total = 0; //this is the total calculation for all fields
                //$scope.followup.row
                angular.forEach($rootScope.version.Data.FollowUp, function(value, index) {

                    if (value != null && value != 'undefined') {
                        var internalTime = parseInt(value.InternalHours);
                        var externalTime = parseInt(value.ExternalHours);
                        var OtherCosts = parseInt(value.OtherCosts); // to be changed to various hours
                        var activityTotal = parseInt(value.ActivityTotal);
                        var rowInternalTime = internalTime;
                        var rowExternalTime = externalTime;
                        var rowMisc = OtherCosts;
                        var rowTotal = 0;

                        if (!isNaN(rowInternalTime) && !isNaN(rowExternalTime) && !isNaN(rowMisc)) {
                            var rowcalculatedInternalCost = parseInt(rowInternalTime * $scope.internalTimeCost);
                            var rowcalculatedExternalCost = parseInt(rowExternalTime * $scope.externalTimeCost);
                            var rowTotal = (rowcalculatedInternalCost + rowcalculatedExternalCost + rowMisc);
                            $rootScope.version.Data.FollowUp[index].RowTotalCost = rowTotal;
                            if (!isNaN(activityTotal)) {
                                $rootScope.version.Data.FollowUp[index].RowActivityTotal = activityTotal - rowTotal;
                            } else {
                                $rootScope.version.Data.FollowUp[index].RowActivityTotal = '';
                            }
                        }
                    }
                });
            }, true);



        }
    ]);
});
