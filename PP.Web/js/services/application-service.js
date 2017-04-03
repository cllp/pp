define(['./module'], function(services) {
    'use strict';
    services.service('ApplicationService', [
        function() {

            this.apiroot = function() {
                return appConfig.apiUrl;
            };

            this.pdfDraftUri = function(projectId) {
                return appConfig.apiUrl + 'pdf/' + projectId + '.0.pdf';
            };

            this.getpermission = function(project, section) {

                var result = 0;

                project.ProjectPermissions.forEach(function(permission) {
                    if (permission.Permission === 2 && permission.Section === section) {
                        result = 2;
                    }
                    if (permission.Permission === 1 && permission.Section === section) {
                        result = 1;
                    }
                    if (permission.Permission === 0 && permission.Section === section) {
                        result = 0;
                    }
                });
                return result;
            };
        }
    ]);
});
