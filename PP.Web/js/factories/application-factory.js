define(['./module'], function (factories) {
    'use strict';
    factories.factory('ApplicationFactory', ['$http', 'ApplicationService', function ($http, appsvc) {

        return {
            GetVersion: function () {

                return $http.get(appsvc.apiroot() + 'application/version');
            },

            GetPDF: function (projectId, versionId) {

                return $http.get(appsvc.apiroot() + 'application/pdf/ + ' + projectId + '/' + versionId);
            },

            GetSetting: function (name) {

                return $http.get(appsvc.apiroot() + 'application/setting/' + name);
            },

            GetSettings: function (type) {

                return $http.get(appsvc.apiroot() + 'application/settings/' + type);
            },

            CreatePdfDraft: function (projectId) {
                return $http.get(appsvc.apiroot() + 'project/create/pdf/' + projectId);
            }
        };

    }]);
});
