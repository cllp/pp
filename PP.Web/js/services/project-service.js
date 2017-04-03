define(['./module'], function (services) {
    'use strict';
    services.service('ProjectService', ['ContentFactory', function (contentfac) {
        this.project = {
            name: "",
            area: 0
        };

        this.getProjectname = function() {
            return this.project.name;
        };

        this.setproject = function (content) {
            this.project.name = content.Name,
            this.project.area = content.Area;
        };

        this.loadproject = function(id) {
            this.setproject = contentfac.GetProject(id);
            return this.project;
        };

        this.getpermission = function (section, project) {

            return 'write';
        };

        //TODO add member edit to this function
        this.getmodules = function (readonly, version) {
            
            var modules = [];
            var path = '';

            if(readonly) {
                path = 'templates/readcontent/'
            } else {
                path = 'templates/content/'
            }

                var modules = [{
                        Id: 'projectidea',
                        Url: path + 'projectidea.html?' + version,
                        index: 0,
                        Controller: 'ProjectideaController'
                    }, {
                        Id: 'finance',
                        Url: path + 'finance.html?' + version,
                        index: 1,
                        Controller: 'FinanceController'
                    }, {
                        Id: 'participants',
                        Url: path + 'members.html?' + version,
                        index: 1,
                        Controller: 'MembersController'
                    }, {
                        Id: 'projectplan',
                        Url: path + 'plan.html?' + version,
                        index: 1,
                        Controller: 'PlanController'
                    }, {
                        Id: 'goals',
                        Url: path + 'goal.html?' + version,
                        index: 1,
                        Controller: 'GoalController'
                    }, {
                        Id: 'activities',
                        Url: path + 'activity.html?' + version,
                        index: 1,
                        Controller: 'ActivityController'
                     }, {
                        Id: 'followup',
                        Url: path + 'followup.html?' + version,
                        index: 1,
                        Controller: 'FollowupController'
                    }, {
                        Id: 'debriefing',
                        Url: path + 'debriefing.html?' + version,
                        index: 1,
                        Controller: 'DebriefingController'
                    }];

                return modules;
        };

    }]);
});
