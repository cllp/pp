define(['../module', 'angular', 'ngTable'], function(controllers, ng) {
  'use strict';
  controllers.controller('ProjectideaController', ['$scope', '$rootScope', 'ngTableParams', 'AlertFactory', 'ContentFactory', '$filter', '$log', '$location', '$anchorScroll',
    function($scope, $rootScope, ngTableParams, alerts, ContentFactory, $filter, $log, $location, $anchorScroll) {

      $scope.$watch('project.PublishDate', function() {
        if ($rootScope.project.PublishDate !== undefined && $rootScope.project.PublishDate.substring( 0, 10 ) !== '0001-01-01') {
          $scope.publishDate = $filter('date')($rootScope.project.PublishDate, 'yyyy-MM-dd HH:mm');
        } else {
          $scope.publishDate = 'Ej publicerad';
        }
      });

      $scope.$watch(function() {
        return $rootScope.project.Program;
      }, function() {
        if ($rootScope.project.Program !== undefined) {
          $scope.programName = $rootScope.project.Program.Name;
        }
      }, true);

      // read only attributes
      $scope.$watch(function() {
        return $rootScope.project.AreaId;
      }, function() {
        if ($rootScope.project.AreaId !== undefined && $rootScope.readOnly == true) {
          var projectArea = $filter('filter')($rootScope.Areas, {
            Id: $rootScope.project.AreaId
          }, true);
          if (projectArea.length > 0) {
            $scope.projectAreaName = projectArea[0].Name;
          }
        } else {
          $scope.availableProjectAreas = $rootScope.Areas;
        }
      });

      var phase = $filter('filter')($rootScope.Phases, {
        Id: $rootScope.project.PhaseId
      }, true);
      if (phase.length > 0) {
        $scope.Phase = phase[0].Name;
      }

      var organization = $filter('filter')($rootScope.Organizations, {
        Id: $rootScope.project.OrganizationId
      }, true);
      if (organization.length > 0) {
        $scope.OrganizationName = organization[0].Name;
      }

      //DatePicker options
      $scope.datePicker = {};
      $scope.datePicker.today = function() {
        $scope.datePicker.dt = $rootScope.project.StartDate;
      };
      $scope.datePicker.today();

      $scope.datePicker.showWeeks = true;
      $scope.datePicker.toggleWeeks = function() {
        $scope.datePicker.showWeeks = !$scope.datePicker.showWeeks;
      };

      $scope.datePicker.clear = function() {
        $scope.datePicker.dt = null;
      };

      $scope.datePicker.toggleMin = function() {
        $scope.datePicker.minDate = ($scope.datePicker.minDate) ? null : new Date();
      };
      $scope.datePicker.toggleMin();

      $scope.datePicker.open = function($event) {
        $event.datePicker.preventDefault();
        $event.datePicker.stopPropagation();

        $scope.datePicker.opened = true;
      };

      $scope.datePicker.dateOptions = {
        'year-format': "'yy'",
        'starting-day': 1
      };

      $scope.datePicker.formats = ['yyyy-MM-dd', 'yyyy/MM/dd', 'shortDate'];
      $scope.datePicker.format = $scope.datePicker.formats[0];
      $scope.datePicker.dateType = 'string';

    }
  ]);
});