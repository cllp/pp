define(['./module'], function(controllers) {
    'use strict';
    controllers.controller('CommentController', ['$scope', '$rootScope', '$window', '$filter', '$timeout', '$log', 'ContentFactory', 'ApplicationFactory', 'AlertFactory', '$location', '$routeParams',
        function($scope, $rootScope, $window, $filter, $timeout, $log, ContentFactory, appfac, AlertFactory, $location, $routeParams) {

            $scope.comment = {};
            $scope.currentArea = {};
            $scope.currentType = {};
            $scope.comments = [];
            $scope.currentComments = [];
            $scope.currentAreaComments = [];

            $('.sidebarScrollContent').height($(window).height() - 350);
            $scope.projectId = $routeParams.id;

            /**
             * Init function that fetches the different comment types,
             * and comment areas.
             */
            function Init() {
                $scope.$watch(function() {
                    return $rootScope.currentType;
                }, function() {
                    if ($rootScope.currentType !== undefined) {
                        $scope.currentType = $rootScope.currentType;
                        showType($scope.currentType);
                    }
                }, true);

                /**
                 * Function call to API via content factory.
                 * Gets the comment types for a specified project id.
                 * @param {Number} $scope.projectId The project id for the current selected project.
                 */
                ContentFactory.GetCommentTypes($scope.projectId)
                    .success(function(data) {
                        $scope.types = data;
                    })
                    .error(function(error) {
                        $scope.types = [];
                        AlertFactory.add('danger', 'Hämtning av kommentarstyper misslyckades.');
                    });

                /**
                 * Function call to API via content factory.
                 * Gets the available comment areas.
                 * If successful show the first area by default.
                 * Then get the comments to the current projectId.
                 */
                ContentFactory.GetCommentAreas()
                    .success(function(data) {
                        $scope.areas = data;
                        $scope.areas.forEach(function(area) {
                            area.UnRead = false;
                        });
                    }).then(function() {
                        GetComments($scope.projectId);
                    }, function(error) {
                        $scope.areas = [];
                        AlertFactory.add('danger', 'Hämtning av kommentarsområden misslyckades.');
                    });

            };

            /**
             * Function that gets the comments for the specified project id.
             * @param {Number} id
             */
            function GetComments(projectId) {
                /**
                 * Function call to API via content factory.
                 * If successful, set comments object array, and then populate
                 * (and sort by date) array for which areas that has comments
                 * for filtering in template.
                 */
                ContentFactory.GetComments(projectId)
                    .success(function(data) {
                        if ($scope.areas[0] !== null && $scope.currentType !== undefined) {
                            $scope.comments = data;
                            showArea($scope.areas[0]);
                        }
                    })
                    .error(function(error) {
                        $scope.comments = [];
                        $log.error('danger', 'Hämtning av kommentarer misslyckades.');
                    });

            };

            /**
             * Function that gets all the areas with unread comments for the
             * specified project id.
             * @param {Number} projectId
             */
            function GetUnreadProjectComments(projectId) {
                /**
                 * Function call to API via content factory.
                 * If successful, loop through data and sum the
                 * number of unread comments for each area to the
                 * global totalUnreadComments variable.
                 * @param {Number} projectId the id of the current
                 * selected project
                 */
                ContentFactory.GetUnreadProjectComments(projectId)
                    .success(function(data) {
                        var totalUnreadComments = 0;

                        $scope.areasWithUnreadComments = $filter('filter')(data, {
                            TypeId: $scope.currentType.TypeId,
                        }, true);

                        $scope.areasWithUnreadComments.forEach(function(area) {
                            totalUnreadComments += area.UnRead;
                            if ($scope.areas) {
                                $scope.areas.forEach(function(a) {
                                    if (a.AreaId == area.AreaId && area.UnRead > 0) {
                                        console.log('Has unread comments');
                                        a.UnRead = true;
                                    }
                                });
                            }
                        });

                        $rootScope.TotalUnreadProjectComments = totalUnreadComments;
                    })
                    .error(function(error) {
                        AlertFactory.add('danger', 'Hämtning av olästa projektkommentarer misslyckades.');
                    });

            };

            /**
             * Creates a comment based off user created comment object.
             * @param {Object} [comment] a comment object consisting of
             * the current logged in users info as well as the area the
             * user was commenting in.
             */
            $scope.CreateComment = function(comment, e) {
                /* If the user presses the return key and the comment
                 *  isn't empty, we create the comment.
                 */
                if (e.keyCode == 13 && comment.Text !== undefined) {
                    // Build the comment data object

                    comment.ProjectCommentAreaId = $scope.currentArea.AreaId;
                    comment.ProjectCommentTypeId = $scope.currentType.TypeId;
                    comment.ProjectId = $scope.projectId;
                    comment.Writer = $rootScope.loggedInUser.Name;
                    comment.AreaId = $scope.currentArea.AreaId;
                    comment.Date = new Date();
                    comment.isFromCurrentUser = true;

                    /**
                     * Function call to API via content factory to create
                     * a new project comment.
                     * @param {Object} [comment] the comment object containing
                     * the comment data.
                     */
                    ContentFactory.CreateProjectComment(comment)
                        .success(function(data) {
                            // Add the new comment to the comments in scope.
                            $scope.comments.push(data);
                            // A small timeout before displaying success
                            $timeout(function() {
                                $log.info('Kommentar tillagd.');
                                $(".nano").nanoScroller();
                            }, 100);
                            // Now show the current commented area (with the new comment)
                            showArea($scope.currentArea);

                        })
                        .error(function(error) {
                            AlertFactory.add('danger', 'Skapa kommentar misslyckades.');
                        });

                    $scope.comment = {};

                    $scope.isCommentFromCurrentUser = function isCommentFromCurrentUser(comment) {
                        if (comment.Writer.Id = $rootScope.loggedInUser.Id) {
                            comment.isFromCurrentUser = true;
                        } else {
                            comment.isFromCurrentUser = false;
                        }
                    }
                }
            }

            $scope.nextArea = function() {
                if ($scope.currentArea.AreaId == $scope.areas.length) {
                    showArea(findAreaById(1));
                } else {
                    showArea(findAreaById($scope.currentArea.AreaId + 1));
                }
                $scope.markAsRead();
            };

            $scope.previousArea = function() {
                if ($scope.currentArea.AreaId == 1)
                    showArea(findAreaById($scope.areas.length));
                else
                    showArea(findAreaById($scope.currentArea.AreaId - 1));
                $scope.markAsRead();
            };

            $scope.changeArea = function(area) {
                showArea(area);
                $scope.markAsRead();
            };

            $scope.changeType = function(type) {
                showType(type);
            };

            $scope.markAsRead = function() {
                var data = {
                    ProjectId: $scope.projectId,
                    ProjectCommentTypeId: $scope.currentType.TypeId,
                    ProjectCommentAreaId: $scope.currentArea.AreaId
                };

                ContentFactory.UpdateProjectCommentActivity(data)
                    .success(function() {
                        $log.log('Project comment activity updated');
                    })
                    .error(function(error) {
                        AlertFactory.add('danger', 'Uppdatering av projektkommentars aktivitet misslyckades.');
                    });
            };

            function showArea(area) {
                if ($scope.currentComments[0] !== null && $scope.currentAreaComments[0] !== null) {
                    GetUnreadProjectComments($scope.projectId);
                    // scroll to the bottom to show the latest added comment
                    $timeout(function() {
                        $(".nano").nanoScroller().nanoScroller({
                            scroll: 'bottom'
                        });
                    }, 500);

                    //set the current area
                    $scope.currentArea = area;

                    //get all comments for area
                    var currentAreaComments = objectFindByKey($scope.comments, 'AreaId', area.AreaId);
                    var currentTypeComments = objectFindByKey(currentAreaComments, 'TypeId', $scope.currentType.TypeId);

                    $scope.currentAreaComments = currentAreaComments;
                    $scope.currentComments = currentTypeComments;
                }
            };

            function showType(type) {
                showArea($scope.currentArea);
            };

            //find area by id
            function findAreaById(id) {
                for (var i = 0; i < $scope.areas.length; i++) {
                    if ($scope.areas[i].AreaId === id) {
                        return $scope.areas[i];
                    }
                }
                throw "Couldn't find object with id: " + id;
            }

            //find type by index
            function findTypeByIndex(index) {
                for (var i = 0; i < $scope.types.length; i++) {
                    if ($scope.types[i].Index === index) {
                        return $scope.types[i];
                    }
                }
                throw "Couldn't find type with index: " + index;
            }

            //find objects with key
            function objectFindByKey(array, key, value) {
                var newarray = [];

                for (var i = 0; i < array.length; i++) {

                    if (array[i][key] === value) {
                        newarray.push(array[i]);
                    }
                }

                return newarray;
            }

            // sort on key values
            function keysrt(key, desc) {
                return function(a, b) {
                    return desc ? ~~(a[key] < b[key]) : ~~(a[key] > b[key]);
                }
            }

            Init();

        }
    ]);
});
