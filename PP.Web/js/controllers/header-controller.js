define(['./module'], function(controllers) {
    'use strict';
    controllers.controller('HeaderController', [
        '$scope', '$rootScope', '$log', '$location', 'ApplicationFactory', 'ApplicationService', 'AccountFactory', 'ContentFactory', 'AlertFactory', '$window', '$timeout',
        function($scope, $rootScope, $log, $location, appfac, appsvc, accfac, confac, AlertFactory, $window, $timeout) {

            $scope.currentPath = $location.path();
            $scope.version = "12345";


            //check if there is a token
            var token = accfac.GetStorage('token');

            //if token is null
            if (token === null) {

                //Token not found
                $log.info('Token not found, authenticating.')

                //todo check if username or password is null - can be forms or yammer
                $log.info('username: ' + accfac.GetStorage('username'));
                $log.info('password: ' + accfac.GetStorage('password'));

                //authenticate
                accfac.Authenticate(accfac.GetStorage('username'), accfac.GetStorage('password'))
                    .success(function(data) {

                        //authentication successful, set token in storage
                        accfac.SetStorage('token', data.access_token);
                        $rootScope.token = data.access_token;
                        $log.info('token: ' + accfac.GetStorage('token'));
                        $rootScope.$broadcast('tokenRetrieve', {
                            token: $rootScope.token
                        });

                        //get the profile from api
                        accfac.GetProfile().success(function(data, status) {
                            //set the logged in user in scope
                            $scope.loggedInUser = JSON.parse(data.User);
                            $rootScope.loggedInUser = $scope.loggedInUser;
                            accfac.SetStorage('profile', data);
                            //set authenticated in rootscope
                            $rootScope.isAuthenticated = true;

                        }).error(function(data, status) {
                            $log.error('Get profile failed. Token: ' + accfac.GetStorage('token'));
                            //$scope.signin();
                            console.log('GetProfile Error: status ' + status + '. Data: ' + JSON.stringify(data));
                        });

                    })
                    .error(function() {
                        //could not authenticate. remove token if any
                        $log.error('Authentication failed');
                        console.log('GetProfile Error: status ' + status + '. Data: ' + JSON.stringify(data));
                    });

            } else {
                //token found
                $log.info('token found: ' + accfac.GetStorage('token'));
                $rootScope.token = accfac.GetStorage('token');
                //token is found, getting user
                accfac.GetProfile().success(function(data, status) {
                    //set the logged in user in scope
                    $rootScope.loggedInUser = JSON.parse(data.User);
                    //set authenticated in rootscope
                    $rootScope.isAuthenticated = true
                }).error(function(data, status) {
                    $log.error('Get profile failed. Token: ' + accfac.GetStorage('token'));
                    //$scope.signin();
                    console.log('GetProfile Error: status ' + status + '. Data: ' + JSON.stringify(data));
                });
            }

            $scope.redirectToPage = function(page) {
                console.log('redirectToPage: ' + page);
                $location.path(page);
            }
            $scope.redirect = function() {
                if ($rootScope.project.Id !== undefined) {
                    $location.path('/content/' + $rootScope.project.Id);
                }
            }

            $scope.createPdfDraft = function(projectId) {

                appfac.CreatePdfDraft(projectId)
                    .success(function(result) {

                        // + appsvc.pdfDraftUri(projectId)
                        AlertFactory.add('success', 'PDF utkast skapades');

                        console.log('pdf fullpath: ' + appsvc.pdfDraftUri(projectId));
                        $window.open(appsvc.pdfDraftUri(projectId), "_self");
                    })
                    .error(function(error) {
                        AlertFactory.add('danger', 'Skapa pdf misslyckades');
                    });
            };

            $scope.deleteProject = function(projectId) {

                //console.log('deleting project...');

                confac.DeleteProject(projectId)
                    .success(function(result) {
                        AlertFactory.add('success', $rootScope.project.Name + ' togs bort');
                        location.href = '/app.html';
                    })
                    .error(function(error) {
                        AlertFactory.add('danger', 'Det gick inte att ta bort projektet');
                    });
            };

            $scope.signout = function() {
                accfac.ClearStorage();
                $rootScope.loggedInUser = {};
                $rootScope.signedout = true;

                $log.info('User signed out');

                setTimeout(function() {
                    location.href = 'signedout.html';
                }, 1000);
            };

            $scope.signin = function() {
                accfac.ClearStorage();
                $rootScope.loggedInUser = {};
                $rootScope.signedout = true;

                $log.info('User needs to signin');

                location.href = '/';
            };

            $scope.pushAlert = function(message) {
                alerts.add('success', message);
            };

            $scope.closeAlert = function(index) {
                $rootScope.alerts.splice(index, 1);
            };

            $scope.modalPosition = function() {

                var el = document.querySelector("#newMemberButton");
                var top = el.getBoundingClientRect().top;
                var jquerytop = (jQuery("#newMemberButton").offset().top - 100) + 'px';
                var position = "position: absolute; top: " + jquerytop + "; left: 30%;";
                return position;
            };

            appfac.GetVersion().success(function(data, status) {
                $scope.version = JSON.parse(data);
            }).error(function(data, status) {
                $scope.version = 'n/a';
                $log.error("Get version failed");
            });
        }
    ]);
});
