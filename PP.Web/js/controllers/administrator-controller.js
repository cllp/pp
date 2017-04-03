define(['./module'], function(controllers) {
  'use strict';
  controllers.controller('AdministratorController', ['$log', '$scope', '$rootScope', 'ContentFactory', 'ApplicationFactory', 'ApplicationService', 'AlertFactory', '$location', '$anchorScroll', '$filter',
    function($log, $scope, $rootScope, contentFactory, appfac, appsvc, AlertFactory, $location, $anchorScroll, $filter) {

      $scope.tabs = [{
        title: 'Projekt',
        template: 'Templates/administration/projectAdministration.html'
      }, {
        title: 'Alla Användare',
        template: 'Templates/administration/AdministrationAllUsers.html'
      }, {
        title: 'Projektsamordnare',
        template: 'Templates/administration/AdministrationProjectCoordinators.html'
      }, {
        title: 'Programägare',
        template: 'Templates/administration/AdministrationProgramOwners.html'
      }, {
        title: 'Raderade projekt',
        template: 'Templates/administration/AdministrationDeletedProjects.html'
      }
      /*, {
        title: 'Rapporter',
        template: 'Templates/administration/reports.html'
      }*/
      ];

      $scope.activeTab = 0;

      $scope.removedUserAdministrationRoleList = [];
      $scope.userAdministrationRoleList = [];
      $scope.Levels = [{
        Id: '1',
        Name: 'Län'
      }, {
        Id: '2',
        Name: 'Nationellt'
      }];

      $scope.AdminRoles = $filter("filter")($rootScope.ProjectRoles, {
        Name: 'Programägare'
      });
      $scope.AdminRoles.push($filter("filter")($rootScope.ProjectRoles, {
        Name: 'Projektsamordnare'
      })[0]);

      $scope.userRoleList = []

      $scope.addRow = function() {
        $scope.userAdministrationRoleList.push({
          UserId: $scope.selectedUser.Id,
          ProjectRoleId: '',
          ProgramTypeId: '',
          OrganizationId: '',
          Saved: false
        });
        // Create ok.
      };

      $scope.deleteRow = function deleteRow(selectedRow) {

        var confirmed = confirm("Bekräfta borttagning av rad.");
        var selectedItem = $scope.userAdministrationRoleList[selectedRow];
        if (selectedItem.Saved) {
          if (confirmed) {
            $scope.userAdministrationRoleList.splice(selectedRow, 1);
            $scope.removedUserAdministrationRoleList.push(selectedItem);
          }
        }
      };

      //-------------------------------------------------------
      $scope.AdministratorProjectsField = 'Name';
      $scope.AdministratorProjectsReverseSort = false;

      $scope.memberSortField = 'Name';
      $scope.memberReverseSort = false;

      $scope.ProjectCoordinatorsField = 'Name';
      $scope.ProjectCoordinatorsReverseSort = false;

      $scope.ProgramOwnersField = 'Name';
      $scope.ProgramOwnersReverseSort = false;

      $scope.editUser = function(selectedRow, item) {
        GetUserAdministrationProgramRoles(item.Id);
        GetProgramTypes();

        $scope.includeUserEdit = true;
        if (item.OrganizationId !== undefined) {
          var organization = $filter('filter')($rootScope.Organizations, {
            Id: item.OrganizationId
          }, true);
          if (organization.length > 0) {
            $scope.userOrganization = organization[0].Name;
          }
        }

        $scope.Programs = [];
        $scope.AllPrograms = $rootScope.Programs;

        for (var area in $rootScope.Areas) {
          $scope.Programs.push($rootScope.Areas[area].Program);
        }

        $scope.selectedUser = {
          County: item.County,
          Domain: item.Domain,
          Email: item.Email,
          Id: item.Id,
          Name: item.Name,
          Organization: item.Organization,
          OrganizationId: item.OrganizationId,
          OrganizationState: item.OrganizationState,
          RoleId: item.RoleId,
        };
        $scope.modalSelectedRow = selectedRow;

      }

      $scope.saveUser = function(action) {
        if (action === 'save') {
          var user = $filter('filter')($rootScope.userListAdministration, {
            Id: $scope.selectedUser.Id
          }, true);
          $rootScope.userListAdministration[$rootScope.userListAdministration.indexOf(user[0])] = $scope.selectedUser;

          $scope.saveRow($scope.modalSelectedRow, 'update');
        }
        $scope.includeUserEdit = false;
      };


      $scope.saveRow = function saveRow(selectedRow, type) {
        angular.forEach($scope.removedUserAdministrationRoleList, function(obj) {
          RemoveRole(obj);
        });

        angular.forEach($scope.userAdministrationRoleList, function(obj) {
          if (!obj.Saved) {
            AddRole(obj);
          }
        });

        contentFactory.UpdateUser($scope.selectedUser)
          .success(function(bool) {
            if (bool) {
              GetUsersAdministration();
            }

          })
          .error(function(error) {
            AlertFactory.add('danger', 'Uppdatering av uppföljning misslyckades.');
          });

      };

      $scope.AdministratorProjectsSearchText = '';
      $scope.MembersSearchText = '';
      $scope.ProjectCoordinatorsSearchText = '';
      $scope.ProgramOwnersSearchText = '';
      $scope.DeletedProjectsSearchText = '';

      $scope.projectFilter = function(item) {

        var propertyCount = 0;
        var search = '';

        //select search field based on active tab
        if ($scope.activeTab == 0) {
          search = $scope.AdministratorProjectsSearchText;
        }
        if ($scope.activeTab == 4) {
          search = $scope.DeletedProjectsSearchText;
        }
        if (search === undefined) {
          return true;
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

      $scope.restore = function(projectId) {
        console.log(projectId);
        //$rootScope.projectView.
       
        var found = $filter('filter')($rootScope.projectView, {Id: projectId}, true);
        if (found.length) {
          
          AlertFactory.add('success', 'Projekt ' + found[0].Name + ' återskapades');
          contentFactory.RestoreProject(projectId);
          found[0].Active = true;
        } 
        else {
           alert('not found');
        }
      };

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
    

      /*
      $scope.memberFilter = function(item) {
        var propertyCount = 0;
        var search = '';
        
        if ($scope.activeTab == 1) {
          search = $scope.MembersSearchText;
        }
        if ($scope.activeTab == 2) {
          search = $scope.ProjectCoordinatorsSearchText;
        }
        if ($scope.activeTab == 3) {
          search = $scope.ProgramOwnersSearchText;
        }
        if (search.length === 0) {
          return true;
        }

        var objectPropertyMatch = new Array();
        var allwords = search.split(' ');
        for (var property in item) {
          for (var i = 0, limit = allwords.length; i < limit; i++) {
            if ((property === 'Name' || property === 'Email' || property === 'Organization' || property === 'County') && objectPropertyMatch[property] !== true) {
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
*/
      function GetUserAdministrationProgramRoles(id) {
        if ($rootScope.token !== undefined) {

          contentFactory.GetUserAdministrationProgramRoles(id)
            .success(function(roles) {
              $scope.userAdministrationRoleList = roles;
            })
            .error(function(error) {
              $rootScope.status = 'Hämtning av projektlista misslyckades: ' + error.message;
            });
        }
      }

      function GetProgramTypes() {
        if ($rootScope.token !== undefined) {
          if ($scope.programTypes !== null || $scope.programTypes !== undefined) {
            contentFactory.GetProgramTypes()
              .success(function(programTypes) {
                $scope.programTypes = programTypes;
              })
              .error(function(error) {
                $rootScope.status = 'Hämtning av projektlista misslyckades: ' + error.message;
              });
          }
        }
      }

      function AddRole(obj) {
        contentFactory.CreateAdministrationProgramRole(obj)
          .success(function(bool) {
            $log.log('Administration Program Role successfully added');
          })
          .error(function(error) {
            AlertFactory.add('danger', 'Skapande av roll misslyckades.');
            //  $rootScope.status = 'Det misslyckades att uppdatera uppföljning ' + error.message;
          });
      }

      function RemoveRole(selectedItem) {
        contentFactory.DeleteAdministrationProgramRole(selectedItem)
          .success(function() {

          })
          .error(function(error) {
            AlertFactory.add('danger', 'Misslyckades att ta bort projekmedlem.');
            //  $rootScope.status = 'Det misslyckades att tabort projekmedlem: ' + error.message;
          });
      }

      $scope.load = function() {
        var selectedProgram = $filter('filter')($rootScope.Programs, {
          Id: this.item.ProgramTypeId
        });

        var level = selectedProgram[0].TypeId;
        this.item.ProgramTypeId = level;
      }

      var init = function () {

        console.log('init');

      };

      init();

      $rootScope.$watch('token', function() {
        //Watch for when token is set. Then init data.
        if ($rootScope.token !== undefined) {
          /*
          contentFactory.GetCountyProjectList()
            .success(function(projects) {
              $rootScope.countyProjectView = projects;
            })
            .error(function(error) {
              $rootScope.status = 'Hämtning av projektlista misslyckades: ' + error.message;
            });
*/

          contentFactory.GetProjectList()
            .success(function(projects) {
              console.log('get project list');
              $rootScope.projectView = projects;
            })
            .error(function(error) {
              $rootScope.status = 'Hämtning av projektlista misslyckades: ' + error.message;
            });

          contentFactory.GetCounties()
            .success(function(counties) {
              console.log('get counties');
              $rootScope.countyList = counties;
            })
            .error(function(error) {
              $rootScope.status = 'Hämtning av Län misslyckades: ' + error.message;
            });

          GetUsersAdministration();

          contentFactory.GetUsersAdministrationProjectCoordinators()
            .success(function(users) {
              console.log('get GetUsersAdministrationProjectCoordinators');
              $scope.userListProjectCoordinators = users;
            })
            .error(function(error) {
              $rootScope.status = 'Hämtning av användare misslyckades: ' + error.message;
            });

          contentFactory.GetUsersAdministrationProgramOwners()
            .success(function(users) {
               console.log('get userListProgramOwners');
              $scope.userListProgramOwners = users;
            })
            .error(function(error) {
              $rootScope.status = 'Hämtning av användare misslyckades: ' + error.message;
            });

          contentFactory.GetSystemRoles()
            .success(function(roles) {
              console.log('get GetSystemRoles');
              $rootScope.systemroles = roles;
            })
            .error(function(error) {
              $rootScope.status = 'Hämtning av systemroller misslyckades: ' + error.message;
            });

          // contentFactory.GetDeletedProjectList()
          //   .success(function(deletedProjects) {
          //     console.log('get deleted projects');
          //     $rootScope.projectView = deletedProjects;
          //   }).error(function(error) {
          //     $log.error(error);
          //   });

        }
      });

     

      $scope.deletedProjectListFilter = function (item) { 

        var search = $scope.DeletedProjectsSearchText;
        console.log('search: ' + search);
        
        if(item.Active === false && item.Name.indexOf(search) !=-1)
          return true;
        else
          return false;

        //return item.Active === false; 
      };

      String.prototype.contains = function(it) { 
          return this.indexOf(it) != -1; 
        };



      function GetUsersAdministration() {
        contentFactory.GetUsersAdministration()
          .success(function(users) {
            $rootScope.userListAdministration = users;
            $scope.originalList = users;
          })
          .error(function(error) {
            $rootScope.status = 'Hämtning av användare misslyckades: ' + error.message;
          });
      }

    }
  ]);
});