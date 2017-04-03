define(['../module', 'angular', 'ngTable'], function(controllers, ng) {
  'use strict';
  controllers.controller('FinanceController', ['$scope', '$rootScope', 'ngTableParams', 'DataFactory', 'AlertFactory', '$filter', '$log', '$location', '$anchorScroll', '$timeout',
    function($scope, $rootScope, ngTableParams, datafac, alerts, $filter, $log, $location, $anchorScroll, $timeout) {

      checkStimulusPermission();
      $scope.getFundingEstimate = function() {
        var fundingEstimate = parseInt($rootScope.rootscopeFunding.activityEstimate);
        if (!isNaN(fundingEstimate)) {
          $rootScope.project.FundingEstimate = $rootScope.rootscopeFunding.activityEstimate;

          //data collected variable existing to change input-changed css class. This is a special case.
          $scope.dataCollected = true;
          $scope.saveData('FundingEstimate', $rootScope.project.FundingEstimate);
          $timeout(function() {
            $scope.dataCollected = false;
          }, 3500);
        }
      };

      $scope.tooltip = {
        "title": "Hämta summa från aktiviteter.",
        "checked": false
      };

      // Funding calculations
      $scope.$watch('[project.FundingEstimate, project.FundingActual, project.FundingStimulus, project.FundingExternal]', function() {

        // Load data
        $rootScope.rootscopeFunding.actual = parseInt($rootScope.project.FundingActual);
        $rootScope.rootscopeFunding.stimulus = parseInt($rootScope.project.FundingStimulus);
        $rootScope.rootscopeFunding.external = parseInt($rootScope.project.FundingExternal);
        $rootScope.rootscopeFunding.estimate = parseInt($rootScope.project.FundingEstimate);

        var fundingEstimate = parseInt($rootScope.rootscopeFunding.estimate);
        var fundingActual = $rootScope.rootscopeFunding.actual;
        var fundingStimulus = $rootScope.rootscopeFunding.stimulus;
        var fundingExternal = $rootScope.rootscopeFunding.external;
        var totalFunding = fundingActual + fundingStimulus + fundingExternal;

        if (!isNaN(totalFunding) && !isNaN(fundingEstimate)) {
          var result = 0;
          if (fundingEstimate > totalFunding) {
            result = fundingEstimate - totalFunding;
          }
          $rootScope.rootscopeFunding.unfinanced = result;
        }
      }, true);

      $scope.$watch('project.Members', function() {
        checkStimulusPermission();
      }, true);


      function checkStimulusPermission() {
        if ($rootScope.project.Members !== undefined) {
          var stimulusAllowedUsers = [];
          $rootScope.project.Members.forEach(function(member) {
            if(member.isSaved !== false) {
            member.MemberRoles.forEach(function(memberRole) {
              if (memberRole.Id == 1 || memberRole.Id == 2) {
                stimulusAllowedUsers.push(member);
              }
            });
            }
          });

          $rootScope.allowStimulusUpdate = false;

          for (var i = 0; i < stimulusAllowedUsers.length; i++) {
            if (stimulusAllowedUsers[i].UserId === $rootScope.loggedInUser.Id) {
              $rootScope.allowStimulusUpdate = true;
            } else {
              $rootScope.allowStimulusUpdate = false;
            }
          }
        }
      }
    }
  ]);

});