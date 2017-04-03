define(['./module'], function(controllers) {
  'use strict';
  controllers.controller('AllprojectsController', ['$scope', '$rootScope', 'ContentFactory', 'ApplicationFactory', 'AlertFactory', '$location', '$anchorScroll',
    function($scope, $rootScope, contentFactory, appfac, AlertFactory, $location, $anchorScroll) {

      $scope.loadProjects = false;

      function goToProject(name) {
        $location.hash(name);
        $anchorScroll();
      };

      $scope.selectedProject = function selectedProject(item) {
        var currentProjectId = $rootScope.project.Id;
        if (currentProjectId == null) {
          return false;
        }
        if (item.Id == $rootScope.project.Id) {
          goToProject(item.Id);
          return true
        } else {
          return false;
        }
      }

      $scope.orderByField = 'Name';
      $scope.reverseSort = false;
      $scope.AllProjectssearchText = '';

      $scope.MyProjectsOrderByField = 'Name';
      $scope.MyProjectsReverseSort = false;
      $scope.MyProjectsSearchText = '';

      $scope.MyFavouritesOrderByField = 'Name';
      $scope.MyFavouritesReverseSort = false;
      $scope.MyFavouritesSearchText = '';

      $scope.ArchivedOrderByField = 'Name';
      $scope.ArchivedReverseSort = false;
      $scope.ArchivedSearchText = '';

      $scope.myFilter = function(item) {
        //select search field based on active tab
        var search = '';
        if ($scope.activeTab == 0) {
          search = $scope.MyFavouritesSearchText;
        }
        if ($scope.activeTab == 1) {
          search = $scope.MyProjectsSearchText;
        }
        if ($scope.activeTab == 2) {
          search = $scope.AllProjectssearchText;
        }
        if ($scope.activeTab == 3) {
          search = $scope.ArchivedSearchText;
        }
        if (search.length === 0) {
          return true;
        }

        var objectPropertyMatch = new Array();
        var allwords = search.split(' ');
        for (var property in item) {
          for (var i = 0, limit = allwords.length; i < limit; i++) {
            if (property === 'Name' || property === 'Area' || property === 'Phase' || property === 'Municipality' || property === 'County') {
              if (item[property].toString().toLowerCase().indexOf(allwords[i].toLowerCase()) !== -1) { //if word match property = true
                objectPropertyMatch[property] = true;
              } else {
                objectPropertyMatch[property] = false;
              }
            }
          }
        }

        var propertiesmatch = 0;
        for (var property in objectPropertyMatch) {
          var val = objectPropertyMatch[property];
          if (val === true) {
            propertiesmatch++;
          }
        }
        if (propertiesmatch >= allwords.length) {
          return true;
        }
        return false;
      }

      function toggleProjectFavorite(item, boolean) {
        contentFactory.ToggleProjectFavorite(item.Id, boolean)
          .success(function() {
            item.Favorite = boolean;
          })
          .error(function(error) {
            AlertFactory.add('danger', 'Ändring av favoritstatus misslyckades.');
          });
      }

      $scope.isFavourite = function(item) {
        return (item.Favorite == true);
      };

      $scope.isMember = function(item) {
        return (item.Member == true);
      };


      $scope.toggleFavourite = function(selectedRowIndex, item) {

        var status = false;
        if (!item.Favorite) {
          status = true;
        } else {
          status = false;
        }
        toggleProjectFavorite(item, status);

        $scope.$apply;
      };


      $scope.removeFromFavourites = function(selectedRowIndex, item) {
        toggleProjectFavorite(item, false);
      };

      $scope.searchText = '';

      $scope.redirect = function(projectId) {
        if (projectId != null) {
          if (projectId !== $rootScope.project.Id) {
            $rootScope.project = {
              Name: 'Laddar...'
            };
          }

          $location.path('/content/' + projectId);
          $rootScope.getproject(projectId);
        }
      }
      $scope.currentProject = function(item) {
        var currentProjectId = $rootScope.project.Id;
        if (currentProjectId == null) {
          return false;
        }
        if (item.Id == $rootScope.project.Id) {
          return true;
        } else {
          return false;
        }
      }

    }
  ]);

});