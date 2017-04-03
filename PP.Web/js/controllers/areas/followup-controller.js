define(['../module', 'angular', 'ngTable'], function(controllers, ng) {
  'use strict';
  controllers.controller('FollowupController', ['$scope', '$rootScope', 'ngTableParams', 'ContentFactory', 'AlertFactory', 'ApplicationFactory', '$filter', '$log', '$location', '$anchorScroll', '$timeout',
    function($scope, $rootScope, ngTableParams, contentFactory, AlertFactory, appfac, $filter, $log, $location, $anchorScroll, $timeout) {

      // table add row
      $scope.addRow = function() {
        $rootScope.project.FollowUp.push({
          Date: new Date(),
          InternalHours: 0,
          ExternalHours: 0,
          OtherCosts: 0,
          IsOpen: false,
          Notes: '',
          RowTotalCost: 0,
          ActivityTotal: $rootScope.project.Activity.SummaryTotal,
          isSaved: false
        });
      };

      $scope.openModal = function(selectedRow, item) {
        $scope.topDistance = $(document).scrollTop() + 200;
        $scope.modalCommentField = '';
        $scope.modalSelectedRow = selectedRow;
      };

      $scope.closeModal = function(comment) {

        var selectedItem = $rootScope.project.FollowUp[$scope.modalSelectedRow];
        selectedItem.Notes += '\r\n\r\nMakulerad: ' + $filter('date')(new Date(), "yyyy-MM-dd") + ' \r\n' + comment;
        $scope.modalCommentField = '';
        $scope.saveRow($scope.modalSelectedRow, 'remove');
      };

      $scope.saveRow = function saveRow(selectedRow, type) {

        var selectedItem = $rootScope.project.FollowUp[selectedRow];
        $scope.tempRow = selectedRow;

        if (type === 'remove') {
          selectedItem.Canceled = true;
        }

        var followUpModel = {
          Name: selectedItem.Name,
          ProjectId: $rootScope.project.Id,
          Date: selectedItem.Date,
          InternalHours: selectedItem.InternalHours,
          Id: selectedItem.Id,
          ExternalHours: selectedItem.ExternalHours,
          OtherCosts: selectedItem.OtherCosts,
          Notes: selectedItem.Notes,
          ActivityTotal: selectedItem.ActivityTotal,
          Canceled: selectedItem.Canceled
        };

        // Check if this is a create or update
        if (followUpModel.Id != null || followUpModel.Id > 0) {
          contentFactory.UpdateFollowUp(followUpModel)
            .success(function() {
              // Update ok.
              $scope.saveFade('followupRow', 'table', selectedRow);
              $scope.enableSave(false, selectedItem);

              savedItemStatusDelay(selectedRow);
            })
            .error(function(error) {
              AlertFactory.add('danger', 'Uppdatering av uppföljning misslyckades.');
            });

        } else {

          contentFactory.CreateFollowUp(followUpModel)
            .success(function(id) {
              $rootScope.project.FollowUp[selectedRow].Id = id;
              $scope.saveFade('followupRow', 'table', selectedRow);
              $scope.enableSave(false, selectedItem);

              savedItemStatusDelay(selectedRow);

            })
            .error(function(error) {
              AlertFactory.add('danger', 'Skapande av uppföljning misslyckades.');
            });
        }
      };

      function savedItemStatusDelay(selectedRow) {
        $timeout(function() {
          $rootScope.project.FollowUp[selectedRow].isSaved = true;
        }, 3500);
      }

      // Table delete row
      $scope.deleteRow = function deleteRow(selectedRow) {
        var confirmed = confirm("Bekräfta borttagning av rad.");
        if (confirmed) {
          var selectedItem = $rootScope.project.FollowUp[selectedRow];
          contentFactory.DeleteFollowUp(selectedItem.Id)
            .success(function() {
              $rootScope.project.FollowUp.splice(selectedRow, 1);
            })
            .error(function(error) {
              AlertFactory.add('danger', 'Borttagning av uppföljning misslyckades.');
            });
        }
      };

      $scope.$watch('finaceRatios', function() {

      }, true);

      $scope.budgetSummary = {
        total: 0
      };

      $scope.$watch('project.FollowUp', function(value, index) {
        if ($rootScope.finaceRatios !== undefined) {
          $scope.internalTimeCost = $filter('filter')($rootScope.finaceRatios, {
            Name: 'financeInternalCost'
          })[0].Value; // 0.5;
          $scope.externalTimeCost = $filter('filter')($rootScope.finaceRatios, {
            Name: 'financeExternalCost'
          })[0].Value; // 1;
        }
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


        var totalInternal = 0;
        var totalExernal = 0;
        var totalmisc = 0;
        var total = 0; //this is the total calculation for all fields                
        //$scope.followup.row
        var totalSum = 0;
        var previousSum = 0;
        angular.forEach($rootScope.project.FollowUp, function(value, index) {

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
              var rowcalculatedInternalCost = parseInt(rowInternalTime * internalTimeCost);
              var rowcalculatedExternalCost = parseInt(rowExternalTime * externalTimeCost);
              var rowTotal = (rowcalculatedInternalCost + rowcalculatedExternalCost + rowMisc);

              $rootScope.project.FollowUp[index].RowTotalCost = rowTotal;
              if (!value.Canceled) {
                previousSum = totalSum;
                totalSum += rowTotal;
                $rootScope.project.FollowUp[index].RowActivityTotal = activityTotal - totalSum;
              } else {
                $rootScope.project.FollowUp[index].RowActivityTotal = activityTotal - (totalSum + rowTotal);
              }

            }
          }
        });
      }, true);

      appfac.GetSetting('HelpText').success(function(data, status) {
        $scope.helpTexts = data;
      }).error(function(data, status) {
        $log.error('Get helptexts failed.');
      });

    }
  ]);
});