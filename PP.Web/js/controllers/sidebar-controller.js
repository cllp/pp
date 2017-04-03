define(['./module'], function(controllers) {
    'use strict';
    controllers.controller('SidebarController', ['$scope', '$rootScope', '$timeout', '$filter', '$routeParams', 'ContentFactory', 'ApplicationFactory', 'AlertFactory', '$location',
        function($scope, $rootScope, $timeout, $filter, $routeParams, ContentFactory, appfac, AlertFactory, $location) {
            $scope.previousTab = {};
            $scope.activeTab = {};
            $scope.nextTab = {};
            $scope.activeIndex = 0; // set default tab index

            $scope.tabs = [{
                title: 'Projektinformation',
                template: 'Templates/content/sidebar/metadata.html?' + $rootScope.appVersion,
                index: 0,
                controller: 'MetadataController'
            }, {
                title: 'Publicerat',
                template: 'Templates/content/sidebar/versions_list.html?' + $rootScope.appVersion,
                index: 1
            }];

            /**
             * Get the comment types for the specified id. If the request
             * is successful we create a tab in the sidebar with the title
             * of the returned commentType, a generic comment template and
             * the returned commentType index.
             */
            if ($routeParams.id !== undefined || $routeParams !== '') {
                ContentFactory.GetCommentTypes($routeParams.id)
                    .success(function(commentTypes) {
                        angular.forEach(commentTypes, function(commentType) {
                            $scope.tabs.push({
                                title: commentType.Name,
                                template: 'Templates/content/sidebar/comments.html?' + $rootScope.appVersion,
                                index: 1 + commentType.Index,
                                commentable: true,
                                typeId: commentType.TypeId
                            });
                        });
                    })
                    .error(function(error) {
                        console.error(error);
                    });
            }

            $('#sidebar').css({
                'right': '' + ($(window).width() / 12 - 60) + 'px'
            });

            /**
             * Simple function in scope that triggers
             * when the user changes tabs.
             * @param  {Object} tab
             */
            $scope.changeTab = function(tab) {
                $scope.activeIndex = tab.index;
                SetTabIndex();

                $rootScope.currentType = {
                    TypeId: $scope.activeTab.typeId,
                    Name: $scope.activeTab.title,
                    Index: $scope.activeTab.index - 1
                };

            };

            /**
             * Sets the current tab index on input.
             */
            function SetTabIndex() {
                var previousIndex = 0;
                var nextIndex = 0;

                //if active index = 2
                if ($scope.activeIndex == $scope.tabs.length - 1) {
                    previousIndex = $scope.activeIndex - 1;
                } else if ($scope.activeIndex == 0) {
                    previousIndex = $scope.tabs.length - 1;
                    nextIndex++;
                } else {
                    previousIndex = $scope.activeIndex - 1;
                    nextIndex = $scope.activeIndex + 1;
                }

                $scope.previousTab = $scope.tabs[previousIndex];
                $scope.activeTab = $scope.tabs[$scope.activeIndex];
                $scope.nextTab = $scope.tabs[nextIndex];

                $(".nano").nanoScroller();

                $timeout(function() {
                    $(".nano").nanoScroller();
                }, 500);
                return;
            }

            //call
            SetTabIndex();
        }
    ]);

});
