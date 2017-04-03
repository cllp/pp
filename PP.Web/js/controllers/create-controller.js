define(['./module'], function(controllers) {
  'use strict';
  controllers.controller('CreateController', ['$scope', '$rootScope', '$routeParams', '$location', '$filter', 'ContentFactory', 'AlertFactory', 'ProjectService', 'AccountFactory',
    function($scope, $rootScope, $routeParams, $location, $filter, contentFactory, alertFactory, projectService, accfac) {

      //TODO change to account factory
      $rootScope.Areas = accfac.GetStorage('areas');
      $rootScope.Organizations = accfac.GetStorage('organizations');

      $scope.newProject = {
        Name: '',
        OrganizationId: null,
        AreaId: null,
        ProgramOwner: null,
        ProgramId: null,
        ProjectCoordinator: null,
        CreatedById: $rootScope.loggedInUser.Id,
        CreatedBy: $rootScope.loggedInUser.DisplayName,
        StartDate: new Date(),
        CreatedDate: '',
        Description: ''
      };

      $scope.newProject.OrganizationId = $rootScope.loggedInUser.OrganizationId;

      $scope.requiredRoles = {
        RequireProgramOwner: '',
        RequireProjectCoordinator: ''
      };

      $scope.projectCoordinators = [{
        Id: '',
        Name: ''
      }];

      $scope.programOwners = [{
        Id: '',
        Name: ''
      }];

      $scope.$watch('newProject', function() {
        // var projectarea = $filter('filter')($scope.Areas, { Id: $scope.newProject.AreaId }, true);
        if ($scope.newProject.ProgramId !== null && $scope.newProject.OrganizationId !== null && $scope.newProject.ProgramId !== 6) {
          //TODO: Dynamic roleId
          // TEMP FIX MM: Program Id changed to program type id. 
          getprogramowners(1, 1, $scope.newProject.OrganizationId);
          //TODO: Dynamic roleId
          // TEMP FIX MM: Program Id changed to program type id. 
          getprojectcoordinators(1, 6, $scope.newProject.OrganizationId);
        }
      }, true);

      // Detta är den nya
      $scope.$watch('newProject.ProgramId', function() {
        var projectareas = $filter('filter')($scope.Areas, {
          // OrganizationId: $scope.newProject.OrganizationId,
          ProgramId: $scope.newProject.ProgramId
        }, true);

        $scope.selectedAreas = projectareas;
        if (projectareas.length > 0) {
          var program = $filter('filter')($scope.Programs, {
            Id: $scope.newProject.ProgramId
          }, true);

          if (program !== undefined && program.id !== 6) {
            $scope.requiredRoles.RequireProgramOwner = program[0].RequireProjectCoordinator;
            $scope.requiredRoles.RequireProjectCoordinator = program[0].RequireProgramOwner;
          }

        }
      });

      function getprogramowners(programTypeId, projectRoleId, organizationId) {

        contentFactory.GetProgramAdministrators(programTypeId, projectRoleId, organizationId)
          .success(function(adminList) {
            $scope.programOwners = adminList;
          })
          .error(function(error) {
            alertFactory.add('danger', 'Hämtning av programägare misslyckades.');
          });
      }

      function getprojectcoordinators(programTypeId, projectRoleId, organizationId) {

        contentFactory.GetProgramAdministrators(programTypeId, projectRoleId, organizationId)
          .success(function(adminList) {
            $scope.projectCoordinators = adminList;
          })
          .error(function(error) {
            alertFactory.add('danger', 'Hämtning av projektsamordnare misslyckades.');
          });
      }

      $scope.programFilter = function(program) {
        return program.OrganizationId === null || program.OrganizationId == $rootScope.loggedInUser.OrganizationId
      }

      $scope.projectAreaFilter = function(area) {
      return area.OrganizationId === null || area.OrganizationId == $rootScope.loggedInUser.OrganizationId
      }

      //DatePicker options
      $scope.datePicker = {};
      $scope.datePicker.today = function() {
        $scope.datePicker.dt = $scope.newProject.StartDate;
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
      $scope.submitted = false;

      $scope.createProject = function() {
        $scope.submitted = true;
        $scope.isFormValid = Form.$invalid;

        if ($scope.Form.projectName.$valid && $scope.Form.projectArea.$valid && $scope.Form.programOwner.$valid && $scope.Form.projectCoordinator.$valid) {
          $scope.json = angular.toJson($scope.newProject);

          if ($scope.json != null) {
            createProject($scope.json);
          }
        }
      };

      function createProject(data) {
        contentFactory.CreateProject(data)
          .success(function(id) {
            $location.path('/content/' + id);
          })
          .error(function(error) {
            alertFactory.add('danger', 'Det misslyckades att skapa projektet.');
          });
      }
    }
  ]);
});