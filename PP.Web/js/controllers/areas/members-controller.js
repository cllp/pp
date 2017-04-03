define(['../module', 'angular', 'ngTable'], function(controllers, ng) {
    'use strict';
    controllers.controller('MembersController', ['$scope', '$rootScope', 'ngTableParams', 'ContentFactory', 'AlertFactory', '$filter', '$log', '$location', '$anchorScroll',
        function($scope, $rootScope, ngTableParams, contentFactory, AlertFactory, $filter, $log, $location, $anchorScroll) {

            /**
             * Function for adding new rows to the members area.
             * This is executed when the link to add a new member is clicked.
             */
            $scope.addRow = function() {
                var roles = null;
                var userId = $rootScope.loggedInUser.Id;
                var projectId = $rootScope.project.Id;

                contentFactory.GetAvaliableProjectRoles(userId, projectId)
                    .success(function(data) {
                        roles = data;
                        $scope.avaliableRoles = data;
                        var selectedRole = null;
                        if (roles.length < 1) {
                            selectedRole = null;
                        } else {
                            selectedRole = ($filter('filter')(roles, 'Projektmedlem'));
                        }

                        $rootScope.project.Members.push({
                            Name: '',
                            Email: '',
                            MemberRoles: roles[roles.indexOf(selectedRole[0])],
                            Municipality: '',
                            isSaved: false
                        });
                        // Create ok.
                    })
                    .error(function(error) {
                        $rootScope.status = 'Det misslyckades att lägga till projektmedlem: ' + error.message;
                    });
            };

            /**
             * Function for saving a row in the members area.
             * This is executed when the link to save a member row is clicked.
             * @param  {Number} selectedRow [An integer that represents the
             * index of the currently clicked row].
             */
            $scope.saveRow = function saveRow(selectedRow) {
                var selectedItem = $rootScope.project.Members[selectedRow];
                var memberRoles = [];
                var roleIds = [];

                if (selectedItem.isSaved == false) {

                    if (Array.isArray(selectedItem.MemberRoles)) {
                        if (selectedItem.MemberRoles !== null) {
                            for (var i = 0; i < selectedItem.MemberRoles.length; i++) {
                                roleIds[i] = selectedItem.MemberRoles[i].Id;
                            }
                        }
                    } else {
                        roleIds[0] = selectedItem.MemberRoles.Id;
                        memberRoles.push(selectedItem.MemberRoles);
                    }

                    var memberModel = {
                        Id: '',
                        UserId: '',
                        ProjectId: $rootScope.project.Id,
                        MemberRoles: memberRoles,
                        Municipality: selectedItem.Municipality,
                        Name: selectedItem.Name,
                        ProjectRoleIds: roleIds,
                        Email: selectedItem.Email
                    };

                    console.log(memberModel);

                    contentFactory.CreateMember(memberModel)
                        .success(function(memberView) {
                            // If the request was successful the member is now saved.
                            $rootScope.project.Members[selectedRow].isSaved = true;

                            // If the memberView has the member remove the row.
                            if (memberView.memberExists == true) {
                                AlertFactory.add('danger', 'Deltagare finns redan tillagd i projektet');
                                $rootScope.project.Members.splice(selectedRow, 1);
                                // If not set the remaining properties in scope.
                            } else {
                                // If the memberView does not have the member
                                // make success animation on the selected row and set
                                // the other values in scope.
                                console.log(memberView);
                                console.log('MEMBER: ' + JSON.stringify(memberView));

                                var roleIds = [];
                                // As long as the selected member row has roles.
                                if ($rootScope.project.Members[selectedRow].MemberRoles !== null) {
                                    // Loop through the roles.
                                    for (var i = 0; i < $rootScope.project.Members[selectedRow].MemberRoles.length; i++) {
                                        // And push the value into the roleIds array.
                                        roleIds[i] = $rootScope.project.Members[selectedRow].MemberRoles[i].Id;
                                    }
                                }

                                $scope.saveFade('member', 'table', selectedRow);
                                $rootScope.project.Members[selectedRow].Id = memberView.Id;
                                $rootScope.project.Members[selectedRow].Name = memberView.Name;
                                $rootScope.project.Members[selectedRow].Municipality = memberView.Organization;
                                $rootScope.project.Members[selectedRow].UserId = memberView.UserId;
                                $rootScope.project.Members[selectedRow].ProjectId = memberView.ProjectId;
                                $rootScope.project.Members[selectedRow].ProjectRoleIds = roleIds;
                            }
                        })
                        .error(function(error) {
                            AlertFactory.add('danger', 'Det misslyckades att lägga till projektmedlem.');
                        });
                } else {
                    /**
                     * If the selected item is saved and the memberModel
                     * we update the selected row with any new information.
                     */
                    $scope.updateRow(selectedRow);
                }

            };

            /**
             * Function to update a specific member in the project.
             * This is called when the update link is clicked in
             * the list of members.
             * @param  {Number} selectedRow [An integer that represents the
             * index of the currently clicked row]
             */
            $scope.updateRow = function updateRow(selectedRow) {
                var selectedItem = $rootScope.project.Members[selectedRow];
                if (selectedItem.Email !== "") {
                    // Array to hold the roleIds.
                    var roleIds = [];
                    // As long as the selected member row has roles.
                    if (selectedItem.MemberRoles !== null) {
                        // Loop through the roles.
                        for (var i = 0; i < selectedItem.MemberRoles.length; i++) {
                            // And push the value into the roleIds array.
                            roleIds[i] = selectedItem.MemberRoles[i].Id;
                        }
                    }

                    // Fetch the current values from the selected item and populate memberModel
                    var memberModel = {
                        Id: selectedItem.Id,
                        UserId: selectedItem.UserId,
                        ProjectId: selectedItem.ProjectId,
                        Name: selectedItem.Name,
                        ProjectRoleIds: roleIds,
                        Municipality: selectedItem.Municipality,
                        Email: selectedItem.Email
                    };

                    console.log(JSON.stringify(memberModel));

                    // Perform call to API with memberModel as parameter
                    contentFactory.UpdateMember(memberModel)
                        .success(function(response) {
                            if (response[0]) {
                                var response = response[0];
                                if (response.Status == 'Denied') {
                                    var errormessage = 'Du har inte rättighet att lägga till följande roller: ';
                                    angular.forEach(response, function(value, key) {
                                        errormessage += value.Name + ', ';
                                    });
                                    AlertFactory.add('danger', errormessage);
                                }
                                // Added new role.
                                if (response.Status == 'Created') {
                                    // Filter in the project members to see if there is already a member added in scope with
                                    // the same role.
                                    var filtered = $filter('filter')($rootScope.project.Members[selectedRow].MemberRoles, {
                                        Id: response.ProjectMemberRole.Id
                                    });

                                    if (filtered.length == 0) {
                                        $rootScope.project.Members[selectedRow].MemberRoles.push(response.ProjectMemberRole);
                                        $rootScope.project.Members[selectedRow].isSaved = true;
                                    }
                                }
                                // Removed existing role
                                if (response.Status == 'Deleted') {
                                    $rootScope.removedRoles = response.ProjectMemberRole;
                                }
                                $scope.saveFade('member', 'table', selectedRow);
                            }
                        })
                        .error(function(error) {
                            AlertFactory.add('danger', 'Misslyckades att uppdatera projektmedlem');
                        });
                } else {
                    AlertFactory.add('danger', 'Du kan inte spara en medlem utan epost.');
                }
            };

            // Table delete row
            $scope.deleteRow = function deleteRow(selectedRow) {
                var confirmed = confirm("Bekräfta borttagning av rad.");
                var selectedItem = $rootScope.project.Members[selectedRow];
                if (selectedItem.Email !== '') {
                    if (confirmed) {
                        contentFactory.DeleteMember(selectedItem.Id)
                            .success(function() {
                                $rootScope.project.Members.splice(selectedRow, 1);
                            })
                            .error(function(error) {
                                AlertFactory.add('danger', 'Det misslyckades att tabort projekmedlem.');
                            });
                    }
                } else { // This should never happen…
                    console.log('No email, delete the row');
                    console.log(selectedItem);
                    $rootScope.project.Members.splice(selectedRow, 1);
                }
            };
        }
    ]);
});
