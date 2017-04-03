using System.Security.Cryptography;
using PP.Core.Context;
using PP.Core.Model;
using PP.Core.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Logic
{
    public static class PermissionLogic
    {
        public static List<ProjectPermission> GetPermissions(IEnumerable<ProjectRoleView> projectRoles, IProject project, UserView currentUser, bool anyAvaliableRoles)
        {
            List<ProjectPermission> permissions = new List<ProjectPermission>();
            var projectRolePermission = GetProjectRolePermission(projectRoles, currentUser.Role);

            foreach (var sec in Enum.GetValues(typeof(PermissionSection)).Cast<PermissionSection>())
            {
                permissions.Add(GetPermission(sec, currentUser, project, projectRolePermission, anyAvaliableRoles));
            }

            return permissions;
        }

        /// <summary>
        /// Contains all permission logic
        /// </summary>
        /// <param name="section"></param>
        /// <param name="user"></param>
        /// <param name="project"></param>
        /// <param name="projectRolePermission"></param>
        /// <returns></returns>
        private static ProjectPermission GetPermission(PermissionSection section, UserView user, IProject project, Permission projectRolePermission, bool anyAvaliableRoles)
        {
            ProjectPermission p = new ProjectPermission();
            p.Section = section;
            p.Permission = Permission.None; //set default

            switch (section)
            {
                case PermissionSection.CountyCoordinatorComments:
                    {
                        switch (user.Role)
                        {
                            case Role.SuperUser:
                                {
                                    //superuser has always write permission
                                    p.Permission = Permission.Write;
                                    break;
                                }
                            case Role.OrganizationAdmin:
                                {
                                    //check if my organization is same as project
                                    //if true -> default is write
                                    if (user.Organization.Equals(project.Organization.Name))
                                        p.Permission = Permission.Write;
                                    else
                                    {
                                        // if project is not my organization I should not have read or write permissions to CountyCoordinatorComments
                                        p.Permission = Permission.None;
                                    }

                                    break;
                                }
                            case Role.CountyAdmin:
                                {
                                    //check if my county is same as project county //??? how do I know that
                                    if (user.County.Equals(project.Organization.County))
                                        p.Permission = Permission.Write;
                                    else
                                    {
                                        // if project is not my county I should not have read or write permissions to CountyCoordinatorComments
                                        p.Permission = Permission.None;
                                    }

                                    break;
                                }
                            case Role.Default:
                                {
                                    //default role has no rights to CountyCoordinatorComments
                                    p.Permission = Permission.None;
                                    break;
                                }
                        }

                        break;
                    }
                case PermissionSection.MemberComments:
                    {
                        //if I am a member, the permission shall be the same as the projectMemberPermission
                        p.Permission = projectRolePermission;
                        break;
                    }
                case PermissionSection.MemberEdit:
                    {
                        if (anyAvaliableRoles)
                        {
                            p.Permission = Permission.Write;
                        }

                        break;
                    }
                case PermissionSection.Project:
                    {
                        //the project permission IS the projectMemberPermission
                        p.Permission = projectRolePermission;
                        break;
                    }
                case PermissionSection.Stimulus:
                    {
                        if (project.ProjectType == ProjectType.Internal)
                            p.Permission = Permission.None;
                        else
                        {
                            if (user.Role == Role.CountyAdmin)
                                p.Permission = Permission.Write;
                            else
                                p.Permission = Permission.Read;
                        }
                        break;
                    }
            }

            return p;
        }

        /// <summary>
        /// Get the max permission from project roles (0 = no display | 1 = read | 2 = write)
        /// </summary>
        /// <param name="projectRoles"></param>
        /// <returns></returns>
        private static Permission GetProjectRolePermission(IEnumerable<ProjectRoleView> projectRoles, Role currentUserRole)
        {
            if (currentUserRole == Role.CountyAdmin || currentUserRole == Role.OrganizationAdmin || currentUserRole == Role.SuperUser)
            {
                return Permission.Write;
            }
            else
            {
                if (projectRoles.Any())
                {
                    return (Permission) projectRoles.Max(i => i.PermissionId);
                }
                else
                {
                    return Permission.Read;
                }
            }   
        }

        public static IEnumerable<ProjectView> FilterProjectList(this IEnumerable<ProjectView> projects)
        {
            UserView user = IdentityContext.Current.User;
            const string orgStateInternal = "Internal";



            

            // Filter out projects with write access or a publiced version.
            IEnumerable<ProjectView> accessibleProjects =
                Enumerable.Where(projects.Select(p => new
                {
                    p,
                    projectPermission =
                        p.Permissions.FirstOrDefault(pm => pm.Section.Equals(PermissionSection.Project))
                }), @t => @t.projectPermission != null &&
                          ((@t.projectPermission.Permission.Equals(Permission.Read) && @t.p.HasPublishedVersion) ||
                           @t.projectPermission.Permission.Equals(Permission.Write)))
                    .Select(@t => @t.p);


            // Filter out projects on thier type.
            IEnumerable<ProjectView> filteredList = from p in accessibleProjects
                               // ProgramType.Project is members only
                              where (p.Member)
                                  ||
                                   // ProgramType.Municipality or ProgramType.County or Member
                                    ((p.Program.TypeId == (int)ProgramType.Municipality && p.Organization.Name == user.Organization) 
                                    || (p.Program.TypeId == (int)ProgramType.County && p.Organization.County == user.County))
                                  // Everyone can se national projects
                                  || (p.Program.TypeId == (int)ProgramType.National && user.OrganizationState == orgStateInternal)
                              select p;

            return filteredList;
        }
    }
}
