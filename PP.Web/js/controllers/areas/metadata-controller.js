define(['../module', 'angular', 'ngTable'], function(controllers, ng) {
    'use strict';
    controllers.controller('MetadataController', ['$scope', '$rootScope', 'AlertFactory', '$filter', '$log', '$location', '$anchorScroll',
        function($scope, $rootScope, alerts, $filter, $log, $location, $anchorScroll) {

            $scope.$watch('project.Members', function() {
                if ($rootScope.project.Members !== undefined) {

                    $scope.Members = $rootScope.project.Members;
                    $scope.Roles = [];
                    $scope.RoleEmails = [];

                    var roles = [];

                    $rootScope.project.Members.forEach(function(member) {
                        for (var i = 0; i < member.MemberRoles.length; i++) {
                            $scope.seen = false;
                            for (var j = 0; j != roles.length; ++j) {
                                if (roles[j].Id == member.MemberRoles[i].Id) {
                                    $scope.seen = true;
                                }
                            }
                            if (!$scope.seen) {
                                roles.push(member.MemberRoles[i]);
                            }
                        }
                    });

                    $scope.Roles = roles;
                    $scope.Roles.sort(dynamicSort("Name"));
                };
            }, true);

            $scope.mailToRole = function(role) {
                var emails = [];
                var membersWithSelectedRole = $filter('memberRoleFilter')($scope.Members, {
                    id: role.Id
                });

                angular.forEach(membersWithSelectedRole, function(member) {
                    emails.push(member.Email);
                    var email = emails.join(', ');
                    $scope.link = "mailto:" + escape(email) + "?subject=" + role.Name + "&body=" + 'Skriv ditt ärende här.';
                });

                window.location.href = $scope.link;
            }

            function unique(data, key) {
                var result = [];
                for (var i = 0; i < data.length; i++) {
                    var value = data[i][key];
                    if (result.indexOf(value) == -1) {
                        result.push(value);
                    }
                }
                return result;
            }

            function dynamicSort(property) {
                var sortOrder = 1;
                if (property[0] === "-") {
                    sortOrder = -1;
                    property = property.substr(1);
                }
                return function(a, b) {
                    var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
                    return result * sortOrder;
                }
            }

            var projectarea = $filter('filter')($rootScope.Areas, {
                Id: $rootScope.project.AreaId
            }, true);

            if (projectarea.length > 0) {
                $scope.projectAreaName = projectarea[0].Name;
            }

            $('.sidebarScrollContent').height($(window).height() - 350);

        }
    ]);
});
