define(['./module'], function(filters) {
    'use strict';

    return filters.filter('memberRoleFilter', function() {
        return function(members, role) {
            var filtered = [];
            members.forEach(function(member) {
                if (member.isSaved) {
                    member.MemberRoles.forEach(function(memberRole) {
                        if (memberRole.Id == role.id) {
                            filtered.push(member);
                        }
                    });
                }
            });
            return filtered;
        };
    });
});
