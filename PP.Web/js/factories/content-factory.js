define(['./module'], function(factories) {
    'use strict';
    factories.factory('ContentFactory', ['$http', 'ApplicationService',
        function($http, appsvc) {
            return {

                GetProject: function(id) {
                    return $http.get(appsvc.apiroot() + 'project/' + id);
                },
                GetProjectList: function() {
                    return $http.get(appsvc.apiroot() + 'project/all/');
                },
                GetDeletedProjectList: function() {
                    return $http.get(appsvc.apiroot() + 'project/inactive');
                },
                GetCountyProjectList: function() {
                    return $http.get(appsvc.apiroot() + 'project/all/county');
                },
                CreateProject: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/create/', data);
                },
                UpdateProject: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/update/', data);
                },
                DeleteProject: function(id) {
                    return $http.delete(appsvc.apiroot() + 'project/delete/' + id);
                },
                RestoreProject: function(id) {
                    return $http.delete(appsvc.apiroot() + 'project/restore/' + id);
                },
                CreateRisk: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/create/risk', data);
                },
                UpdateRisk: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/update/risk', data);
                },
                DeleteRisk: function(id) {
                    return $http.delete(appsvc.apiroot() + 'project/delete/risk/' + id);
                },
                CreateGoal: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/create/goal', data);
                },
                UpdateGoal: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/update/goal', data);
                },
                DeleteGoal: function(id) {
                    return $http.delete(appsvc.apiroot() + 'project/delete/goal/' + id);
                },
                CreateActivity: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/create/activity', data);
                },
                UpdateActivity: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/update/activity', data);
                },
                DeleteActivity: function(id) {
                    return $http.delete(appsvc.apiroot() + 'project/delete/activity/' + id);
                },
                CreateFollowUp: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/create/followup', data);
                },
                UpdateFollowUp: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/update/followup', data);
                },
                DeleteFollowUp: function(id) {
                    return $http.delete(appsvc.apiroot() + 'project/delete/followup/' + id);
                },
                CreateMember: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/create/member', data);
                },
                UpdateMember: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/update/member', data);
                },
                DeleteMember: function(id) {
                    return $http.delete(appsvc.apiroot() + 'project/delete/member/' + id);
                },
                PublishVersion: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/create/version', data);
                },
                GetVersionList: function(id) {
                    return $http.get(appsvc.apiroot() + 'project/versionList/' + id);
                },
                GetVersion: function(id) {
                    return $http.get(appsvc.apiroot() + 'project/version/' + id);
                },
                GetLatestVersion: function(id) {
                    return $http.get(appsvc.apiroot() + 'project/version/latest/' + id);
                },
                GetPhases: function() {
                    return $http.get(appsvc.apiroot() + 'project/phases/');
                },
                GetAreas: function() {
                    return $http.get(appsvc.apiroot() + 'project/areas/');
                },
                GetActiveAreas: function() {
                    return $http.get(appsvc.apiroot() + 'project/areas/true');
                },
                GetInactiveAreas: function() {
                    return $http.get(appsvc.apiroot() + 'project/areas/false');
                },
                GetPrograms: function() {
                    return $http.get(appsvc.apiroot() + 'project/programs/');
                },
                GetOrganizations: function() {
                    return $http.get(appsvc.apiroot() + 'project/organizations/');
                },
                GetProjectRoles: function() {
                    return $http.get(appsvc.apiroot() + 'project/roles/');
                },
                GetAvaliableProjectRoles: function(userId, projectId) {
                    return $http.get(appsvc.apiroot() + 'project/roles/avaliable/' + userId + '/' + projectId);
                },
                GetProgramAdministrators: function(programTypeId, projectRoleId, organizationId) {
                    return $http.get(appsvc.apiroot() + 'project/programadmins/' + programTypeId + '/' + projectRoleId + '/' + organizationId);
                },
                ToggleProjectFavorite: function(projectId, isFavorite) {
                    return $http.get(appsvc.apiroot() + 'project/favorite/' + projectId + '/' + isFavorite);
                },
                GetSectionHelpText: function(key) {
                    return $http.get(appsvc.apiroot() + 'project/templates/' + key);
                },
                GetUsersAdministrationProgramOwners: function() {
                    return $http.get(appsvc.apiroot() + 'project/administration/programcoowners');
                },
                GetUsersAdministrationProjectCoordinators: function() {
                    return $http.get(appsvc.apiroot() + 'project/administration/projectoordinators');
                },
                GetUsersAdministration: function() {
                    return $http.get(appsvc.apiroot() + 'project/administration/users');
                },
                GetSystemRoles: function() {
                    return $http.get(appsvc.apiroot() + 'project/systemroles');
                },
                UpdateUser: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/update/user', data);
                },
                CreateAdministrationProgramRole: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/administration/createprogramrole', data);
                },
                DeleteAdministrationProgramRole: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/administration/deleteprogramrole', data);
                },
                GetUserAdministrationProgramRoles: function(id) {
                    return $http.get(appsvc.apiroot() + 'project/administration/userprogramrole/' + id);
                },
                GetProgramTypes: function() {
                    return $http.get(appsvc.apiroot() + 'project/programtypes');
                },
                GetCounties: function() {
                    return $http.get(appsvc.apiroot() + 'project/administration/getcounties/');
                },
                GetComments: function(id) {
                    return $http.get(appsvc.apiroot() + 'project/comments/get/' + id);
                },
                GetCommentAreas: function() {
                    return $http.get(appsvc.apiroot() + 'project/comments/areas/');
                },
                GetCommentTypes: function(projectId) {
                    return $http.get(appsvc.apiroot() + 'project/comments/types/' + projectId);
                },
                GetUnreadProjectComments: function(projectId) {
                    return $http.get(appsvc.apiroot() + 'project/comments/unread/' + projectId);
                },
                UpdateProjectCommentActivity: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/comments/updateactivity/', data);
                },
                CreateProjectComment: function(data) {
                    return $http.post(appsvc.apiroot() + 'project/comment/create/', data);
                },
                GetApplicationVersion: function() {
                    return $http.get(appsvc.apiroot() + 'application/version/');
                },
                GetReport: function(name) {
                    return $http.get(appsvc.apiroot() + 'report/get/' + name);
                },
                GetReports: function() {
                    return $http.get(appsvc.apiroot() + 'report/get/');
                },
                SearchUsers: function(search) {
                    return $http.post(appsvc.apiroot() + 'project/users/search', search);
                }
            };
        }
    ]);
});
