define(['./module'], function(controllers) {
    'use strict';
    controllers.controller('ContentController', ['$scope', '$rootScope', '$location', '$filter', '$routeParams', '$exceptionHandler', 'ApplicationFactory', 'ContentFactory', 'ApplicationService', '$anchorScroll', '$timeout', 'AlertFactory', 'ApplicationFactory', '$modal', 'ProjectService',
        function($scope, $rootScope, $location, $filter, $routeParams, $exceptionHandler, accfac, contentFactory, appsvc, $anchorScroll, $timeout, AlertFactory, appfac, $modal, prjsvc) {

            var memberTemplate = '';

            /*if($rootScope.Memberedit == 2) {
                memberTemplate = 'templates/content/members.html?' + $rootScope.appVersion;
            }
            else {
                memberTemplate = 'templates/readcontent/members.html?' + $rootScope.appVersion;
            }*/

            $scope.getTemplateUrl = function (template) {
                var url = 'templates/' + template + '.html?' + $rootScope.appVersion;
                //console.log('returnning template url: ' + url);
                return url;
            };
            

            //console.log('modules: ' + JSON.stringify($scope.modules));

            /*$scope.modules = [{
                Id: 'projectidea',
                Url: 'templates/content/projectidea.html?' + $rootScope.appVersion,
                index: 0,
                Controller: 'ProjectideaController'
            }, {
                Id: 'finance',
                Url: 'templates/content/finance.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'FinanceController'
            }, {
                Id: 'participants',
                Url: memberTemplate,
                index: 1,
                Controller: 'MembersController'
            }, {
                Id: 'projectplan',
                Url: 'templates/content/plan.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'PlanController'
            }, {
                Id: 'goals',
                Url: 'templates/content/goal.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'GoalController'
            }, {
                Id: 'activities',
                Url: 'templates/content/activity.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'ActivityController'
             }, {
                Id: 'followup',
                Url: 'templates/content/followup.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'FollowupController'
            }, {
                Id: 'debriefing',
                Url: 'templates/content/debriefing.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'DebriefingController'
            }];

            $scope.readonlymodules = [{
                Id: 'projectidea',
                Url: 'templates/readcontent/projectidea.html?' + $rootScope.appVersion,
                index: 0,
                Controller: 'ProjectideaController'
            }, {
                Id: 'finance',
                Url: 'templates/readcontent/finance.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'FinanceController'
            }, {
                Id: 'participants',
                Url: memberTemplate,
                index: 1,
                Controller: 'MembersController'
            }, {
                Id: 'projectplan',
                Url: 'templates/readcontent/plan.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'PlanController'
            }, {
                Id: 'goals',
                Url: 'templates/readcontent/goal.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'GoalController'
            }, {
                Id: 'activities',
                Url: 'templates/readcontent/activity.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'ActivityController'
             }, {
                Id: 'followup',
                Url: 'templates/readcontent/followup.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'FollowupController'
            }, {
                Id: 'debriefing',
                Url: 'templates/readcontent/debriefing.html?' + $rootScope.appVersion,
                index: 1,
                Controller: 'DebriefingController'
            }];
            */

            $scope.currentWindowHeight = document.documentElement.clientHeight - 250;

            //TODO change to account factory
            $rootScope.Areas = JSON.parse(sessionStorage.getItem('areas')); // accfac.GetStorage('areas');
            $rootScope.Organizations = JSON.parse(sessionStorage.getItem('organizations')); //accfac.GetStorage('organizations');
            $rootScope.Phases = JSON.parse(sessionStorage.getItem('phases')); // accfac.GetStorage('phases');
            $rootScope.ProjectRoles = JSON.parse(sessionStorage.getItem('roles')); // accfac.GetStorage('roles');

            $scope.$watch('project.ProjectPermissions', function() {
                if ($rootScope.project.ProjectPermissions !== undefined) {
                    $rootScope.Memberedit = appsvc.getpermission($rootScope.project, 'MemberEdit');
                    $rootScope.StimulusEdit = appsvc.getpermission($rootScope.project, 'Stimulus');

                    console.log('read only: ' + $rootScope.readOnly);
                    $scope.modules = prjsvc.getmodules($rootScope.readOnly, $rootScope.appVersion);

                }
            }, true);

            if ($rootScope.collapsed == null) {
                $rootScope.collapsed = true;
            }

            $scope.toggleMenu = function() {
                if ($rootScope.collapsed)
                    $rootScope.collapsed = false;
                else
                    $rootScope.collapsed = true;
            };

            $scope.goToLocation = function goToLocation(id) {
                var old = $location.hash();
                $location.hash(id);
                $anchorScroll();
                //reset to old to keep any additional routing logic from kicking in
                $location.hash(old);
            };

            //check if we have a current project
            if ($rootScope.dirty == null)
                $rootScope.dirty = false;

            //only load the project from database if scope is null
            if ($rootScope.project == null) {

                //alert($routeParams.id);
                //$rootScope.project = getproject($routeParams.id);
                //set routescope current project from dashboard eller header controller
                $rootScope.project = $rootScope.getproject($routeParams.id);
                $rootScope.dirty = false;
            } else {
                //scope.project is not null, check if we should load a new based on current and the parameter
                if ($rootScope.project.Id != $routeParams.id) {
                    $rootScope.getproject($routeParams.id); //$rootScope.project =
                }
            }

            $scope.saveData = function saveData(field, value) {
                $scope.saveFade('', 'field', field)
                var updateModel = {
                    "ProjectId": $routeParams.id,
                    "SectionName": "Project",
                    "FieldName": field,
                    "FieldValue": value
                };
                contentFactory.UpdateProject(updateModel)
                    .success(function() {
                        //$rootScope.project = project;
                    })
                    .error(function(error) {
                        AlertFactory.add('danger', 'Updatering  av projektdetaljer misslyckades.');
                    });
            };

            $scope.enableSave = function(enable, item) {

                item.showSave = enable;
            };

            $scope.getContentIncludeSource = function (source) {
                //alert(source + '?' + appVersion);
                console.log(source + '?' + appVersion);
                 return source + '?' + appVersion;
            };

            $scope.saveFade = function(section, type, selectedElement) {

                if (type === 'table') {
                    selectedElement = "#" + section + selectedElement + " .input-changed";
                }
                if (type === 'field') {
                    selectedElement = "#" + selectedElement;
                }

                $timeout(function() {
                    angular.element(selectedElement).addClass("saving");
                }, 1000);

                $timeout(function() {
                    angular.element(selectedElement).addClass("saved");
                }, 3000);
                $timeout(function() {
                    angular.element(selectedElement).removeClass("saving");
                    angular.element(selectedElement).removeClass("saved");
                    angular.element(selectedElement).removeClass("input-changed");

                }, 3500);

            };

            // Sets the top distance when opening a helptext modal
            $scope.helpTextNumLimit = 150;

            $rootScope.$watch('helpTexts', function() {
                if ($scope.helpTexts !== undefined) {
                    $scope.helpTextPreambleProjectidea = $filter('filter')($rootScope.helpTexts, {
                        Name: 'helpTextProjectidea'
                    })[0].Value;
                    $scope.helpTextPreambleFinance = $filter('filter')($rootScope.helpTexts, {
                        Name: 'helpTextFinance'
                    })[0].Value;
                    $scope.helpTextPreambleMembers = $filter('filter')($rootScope.helpTexts, {
                        Name: 'helpTextMembers'
                    })[0].Value;
                    $scope.helpTextPreamblePlan = $filter('filter')($rootScope.helpTexts, {
                        Name: 'helpTextPlan'
                    })[0].Value;
                    $scope.helpTextPreambleGoal = $filter('filter')($rootScope.helpTexts, {
                        Name: 'helpTextGoal'
                    })[0].Value;
                    $scope.helpTextPreambleActivity = $filter('filter')($rootScope.helpTexts, {
                        Name: 'helpTextActivity'
                    })[0].Value;
                    $scope.helpTextPreambleFollowup = $filter('filter')($rootScope.helpTexts, {
                        Name: 'helpTextFollowup'
                    })[0].Value;
                    $scope.helpTextPreambleDebriefing = $filter('filter')($rootScope.helpTexts, {
                        Name: 'helpTextDebriefing'
                    })[0].Value;
                }
            }, true);

            $scope.showHelpText = function(section) {
                $scope.topDistance = $(document).scrollTop() + 200;
                contentFactory.GetSectionHelpText(section)
                    .success(function(result) {
                        $scope.helpText = result.Data;
                    })
                    .error(function(error) {
                        AlertFactory.add('danger', 'Hämtning av hjälptext misslyckades.');
                    });
            };

            //NEW MEMBER MODAL
            /*$scope.showModal = function () {

                memberModal.show();
            };*/

            /*var modalObj = {
                template:'./templates/content/newmember.html', 
                show:false, 
                container: 'body', 
                //'bs-modal': 'modal'
            };

            var newMemberModal = $modal(modalObj);

            $scope.showNewMember = function() {
                newMemberModal.$promise.then(newMemberModal.show);
            };
            */

             $scope.modalPosition = function() {

                var el = document.querySelector("#newMemberButton");
                var top = el.getBoundingClientRect().top;
                var jquerytop = (jQuery("#newMemberButton").offset().top - 100) + 'px';
                var position = "position: absolute; top: " + jquerytop + "; left: 30%;";
                return position;
            };

            $scope.search = {};
            $scope.newUser = false;
            $scope.memberSaved = false;
            $scope.memberValid = false;

            $scope.searchUsers = function() {

                if($scope.search.text.length > 0) {

                    $scope.selectedUser = null;
                    contentFactory.SearchUsers($scope.search)
                        .success(function(result) {
                            console.log(JSON.stringify(result));

                            if(result.length < 1){
                                $scope.users = [];
                            } else {

                                //TODO: filter users with the current memberlist
                                $scope.users = result;
                            }
                        })
                    .error(function(error) {
                        console.log(error);
                        AlertFactory.add('danger', 'Sökning av användare misslyckades.');
                    });
                } else {
                    $scope.users = [];
                }

                $scope.loadingUsers = false;
            };

           /*$scope.addMember = function () {
                //$scope.isEditMember = false;
                console.log();
            };*/

            $scope.selectUser = function(user) {

                //console.log('user: ' + JSON.stringify(user));
                $scope.selectedUser = user;
                $scope.selectedUser.MemberRoles = [];

                var defaultRole = $filter('filter')($rootScope.ProjectRoles, {Id: 7})[0];

                $scope.selectedUser.MemberRoles.push(defaultRole);
                //$scope.search.text = '';
                $scope.users = [];
                $scope.memberValid = true;

            };

            $scope.resetSearchMember = function(user) {

                //$scope.selectedUser.MemberRoles = [];
                $scope.selectedUser = null;
                $scope.newUser = false;
                $scope.memberSaved = false;
                $scope.isEditMember = false;
                $scope.memberValid = false;
                $scope.search.text = '';
            };

            $scope.editMember = function(member) {

                console.log('editMember: ' + JSON.stringify(member));
                $scope.selectedUser = member;
                $scope.selectedUser.memberExists = true;
                $scope.isEditMember = true;
                $scope.memberValid = true;
            };

            $scope.saveMember = function() {
                
                $scope.selectedUser.ProjectId = $rootScope.project.Id;

                if(!$scope.isEditMember) {
                    //create member

                   contentFactory.CreateMember($scope.selectedUser)
                            .success(function(result) {
                                //pushing the member

                                console.log('NEW MEMBER: ' + JSON.stringify(result));

                                console.log('pushing');
                                console.log(JSON.stringify($scope.selectedUser));
                                $scope.selectedUser.isSaved = true;
                                $scope.selectedUser.UserId = result.UserId;
                                $scope.selectedUser.Id = result.Id;

                                $rootScope.project.Members.push($scope.selectedUser);

                                AlertFactory.add('success', 'Användare ' + $scope.selectedUser.Name + ' sparades');
                                $scope.search.text = null;
                                $scope.users = [];
                                $scope.selectedUser = null;
                                $scope.newUser = false;
                                $scope.memberSaved = true;
                                $scope.isEditMember = false;
                            })
                        .error(function(error) {
                            console.log(error);
                            AlertFactory.add('danger', 'Det gick inte att spara medlem');
                        });

                } else {
                    //update member instead!!!
                    contentFactory.UpdateMember($scope.selectedUser)
                            .success(function(result) {
                                //dont do anything
                                AlertFactory.add('success', 'Användare ' + $scope.selectedUser.Name + ' sparades');
                                $scope.search.text = null;
                                $scope.users = [];
                                $scope.selectedUser = null;
                                $scope.newUser = false;
                                $scope.memberSaved = true;
                                $scope.isEditMember = false;
                            })
                        .error(function(error) {
                            console.log(error);
                            AlertFactory.add('danger', 'Det gick inte att spara medlem');
                        });
                }

                //close the modal
                //$('#memberModal').hide();
            };

            $scope.changeProjectRole = function() {
                console.log('changeProjectRole');
            };

            $scope.deleteMember = function(member) {
                console.log('delete member: ' + JSON.stringify(member));
            };

            $scope.createNewUser = function() {

                $scope.newUser = true;
                $scope.selectedUser = {};
                $scope.selectedUser.MemberRoles = [];
                var defaultRole = findProjectRole(7);
                $scope.selectedUser.MemberRoles.push(defaultRole);
            };

            function validateEmail(email) { 
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                return re.test(email);
            }; 

            function validateName(name) {                 
                return name.length > 3;
            };

            $scope.validateUser = function() {

                
                if(validateEmail($scope.selectedUser.Email)) {
                    
                    //email is valid
                    //get selected organizationid
                    //var orgId = $scope.selectedUser.OrganizationId;
                    var emailDomain = $scope.selectedUser.Email.replace(/.*@/, "");
                    var org = findOrganizationByDomain(emailDomain);

                    if(org != null) {
                        //console.log('emailDomain exists: ' + JSON.stringify(org));
                        $scope.selectedUser.OrganizationId = org.Id;
                        $scope.selectedUser.Organization = org.Name;
                        $scope.selectedUser.Domain = org.Domain;
                        $scope.selectedUser.Municipality = org.Name;

                    } else {
                        //console.log('emailDomain does not exists: ' + emailDomain);
                        $scope.selectedUser.Organization = null;
                    }

                    if(validateName($scope.selectedUser.Name)) {
                        $scope.memberValid = true;
                    } else {
                        $scope.memberValid = false;
                    }

                } else {
                    //email is not valid
                    //console.log('no valid email: ' + JSON.stringify($scope.selectedUser));
                    $scope.selectedUser.Organization = null;
                    $scope.memberValid = false;
                }                
            };

            var matchDomains = function () {

                var orgId = $scope.selectedUser.OrganizationId;
                var emailDomain = $scope.selectedUser.Email.replace(/.*@/, "");
                var organizationDomain = findOrganizationById(orgId).Domain;

                if(emailDomain == organizationDomain) {
                    return true;
                }

                return false;
            };

            var findOrganizationById = function (id) {

              var result  = $rootScope.Organizations.filter(function(o){return o.Id == id;} );
              return result? result[0] : null; // or undefined
            }

            var findOrganizationByDomain = function (domain) {
              var result  = $rootScope.Organizations.filter(function(o){return o.Domain == domain;} );
              return result? result[0] : null; // or undefined
            };

            var findProjectRole = function (id) {
              var result  = $rootScope.ProjectRoles.filter(function(o){return o.Id == id;} );
              return result? result[0] : null; // or undefined
            };
        }
    ]);
});
