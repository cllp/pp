using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PP.Core.Exceptions;
using PP.Core.Helpers;
using PP.Core.Interfaces;
using PP.Core.Logic;
using PP.Core.Model;
using System.Data;
using PP.Core.Context;
using PP.Core.Model.Enum;

namespace PP.Core.Repository
{
    public class ProjectRepository : RepositoryBase, IProjectRepository
    {
        public IEnumerable<Project> SearchProjects(string search)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                search = search.Replace("%", "[%]").Replace("[", "[[]").Replace("]", "[]]");
                search = "%" + search + "%";

                var par = new Dapper.DynamicParameters();
                par.Add("@Search", search);

                return conn.Query<Project>("SELECT * FROM [Project] WHERE Name LIKE @Search", par);
            }
        }

        public IEnumerable<UserView> SearchUsers(string search)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                //search = search.Replace("%", "[%]").Replace("[", "[[]").Replace("]", "[]]");
                //search = "%" + search + "%";

                var par = new Dapper.DynamicParameters();
                par.Add("@Search", search);
                par.Add("@ProjectId", IdentityContext.CurrentProjectId);

                return conn.Query<UserView>("SearchUsers", par, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<ProjectView> GetProjects(ActiveState activeState)
        {
            return GetProjects(IdentityContext.Current.User, activeState);
        }

        public IEnumerable<ProjectView> GetProjects(UserView user, ActiveState activeState)
        {
            //TODO CLLP, Handle if Active is null, return all projects
            int active = EnumHelper.EnumToInt(activeState);

            if (user != null)
            {
                using (SqlConnection conn = SqlHelper.GetOpenConnection())
                {
                    IEnumerable<ProjectView> projects = conn.Query<ProjectView, ProjectArea, Program, Organization, ProjectView>("GetProjects", (p, pa, prog, o) =>
                    {
                        p.ProjectArea = pa;
                        p.Program = prog;
                        p.Organization = o;
                        return p;
                    }, new { UserId = user.Id, ActiveState = active }, commandType: CommandType.StoredProcedure);

                    var query = @"SELECT prv.* FROM [ProjectRoleView] prv WHERE prv.UserId = @UserId";

                    var projectRoles = conn.Query<ProjectRoleView>(query, new { UserId = user.Id });

                    //add the project permissions
                    foreach (var project in projects)
                    {
                        var rolesForProject = projectRoles.Where(p => p.ProjectId.Equals(project.Id));
                        project.Permissions = PermissionLogic.GetPermissions(rolesForProject, project, user, false);

                        //var permission = project.Permissions.Where(p => p.Section.Equals(PermissionSection.Project)).FirstOrDefault();
                    }

                    return projects.FilterProjectList();
                    //return projects.Where(p => p.ShowInListDueToWritePermissionAndVersion.Equals(true));
                }
            }
            else
            {
                //throw new Exception("User is null, can not get projects");
                log.Error("User is null, can not get projects");
                return null;
            }
        }

        public IEnumerable<ProjectView> GetCountyProjects()
        {
            var user = IdentityContext.Current.User;

            if (user != null)
            {
                using (SqlConnection conn = SqlHelper.GetOpenConnection())
                {
                    IEnumerable<ProjectView> projects = conn.Query<ProjectView, ProjectArea, Program, Organization, ProjectView>("GetProjects", (p, pa, prog, o) =>
                    {
                        p.ProjectArea = pa;
                        p.Program = prog;
                        p.Organization = o;
                        return p;
                    }, new { UserId = user.Id, ActiveState = ActiveState.All }, commandType: CommandType.StoredProcedure);

                    IEnumerable<ProjectView> filteredProjects;
                    if (user.Role == Role.CountyAdmin)
                    {
                        filteredProjects = from p in projects
                                           where p.Organization.County.Equals(user.County)
                                           select p;
                        return filteredProjects;
                    }
                    else if (user.Role == Role.OrganizationAdmin)
                    {
                        filteredProjects = from p in projects
                                           where p.Organization.Name.Equals(user.Organization)
                                           select p;
                        return filteredProjects;
                    }
                    else if (user.Role == Role.SuperUser)
                    {
                        return projects;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                //throw new Exception("User is null, can not get projects");
                log.Error("User is null, can not get projects");
                return null;
            }
        }

        public IEnumerable<ProjectView> GetInActiveProjects()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                IEnumerable<ProjectView> projects = conn.Query<ProjectView, ProjectArea, Program, Organization, ProjectView>("GetInActiveProjects", (p, pa, prog, o) =>
                {
                    p.ProjectArea = pa;
                    p.Program = prog;
                    p.Organization = o;
                    return p;
                }, commandType: CommandType.StoredProcedure);

                return projects;
            }
        }

        public Project GetProjectByName(string name)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                par.Add("@Name", name);

                return conn.Query<Project>("SELECT * FROM [Project] WHERE Name = @Name", par).FirstOrDefault();
            }
        }

        public Project GetProject(int id)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                using (var multi = conn.QueryMultiple("GetProject", new { ProjectId = id }, commandType: CommandType.StoredProcedure))
                {
                    //var project = multi.Read<Project>().FirstOrDefault();  

                    var project = multi.Read<Project, Organization, UserView, Program, Project>((p, o, uv, program) =>
                    {
                        p.Organization = o;
                        p.User = uv;
                        p.Program = program;
                        return p;
                    }).FirstOrDefault();

                    var projectActivities = multi.Read<ProjectActivity>();
                    var projectFollowUps = multi.Read<ProjectFollowUp>();
                    var projectGoals = multi.Read<ProjectGoal>();
                    var projectMembers = multi.Read<ProjectMemberView>();
                    var projectRoles = multi.Read<ProjectRoleView>();
                    var projectRisks = multi.Read<ProjectRisk>();

                    if (project == null) return null;

                    if (projectActivities.Any())
                        project.ProjectActivities = projectActivities.ToList();

                    if (projectFollowUps.Any())
                        project.ProjectFollowUps = projectFollowUps.ToList();

                    if (projectGoals.Any())
                        project.ProjectGoals = projectGoals.ToList();

                    var projectMemberViews = projectMembers as List<ProjectMemberView> ?? projectMembers.ToList();
                    if (projectMemberViews.Any())
                        project.ProjectMembers = projectMemberViews;

                    // add member roles
                    var projectRoleView = projectRoles as List<ProjectRoleView> ?? projectRoles.ToList();
                    foreach (var member in project.ProjectMembers)
                    {
                        ProjectMemberView memberCopy = member;
                        foreach (var roleView in projectRoleView.Where(roleView => memberCopy.UserId == roleView.UserId))
                        {
                            member.ProjectRoles.Add(roleView);
                        }
                    }

                    if (projectRisks.Any())
                        project.ProjectRisks = projectRisks.ToList();

                    var query = @"SELECT prv.* FROM [ProjectRoleView] prv WHERE prv.ProjectId = @ProjectId AND prv.UserId = @UserId";

                    var user = IdentityContext.Current.User;

                    IEnumerable<ProjectRoleView> currentUserRoles = conn.Query<ProjectRoleView>(query, new { ProjectId = id, UserId = user.Id });

                    //add the project permissions
                    if (project != null)
                    {
                        IProjectRepository projectRepository = CoreFactory.ProjectRepository;
                        IEnumerable<ProjectRole> avaliableRoles = projectRepository.GetAvaliableRoles(user.Id, project.Id);
                        project.Permissions = PermissionLogic.GetPermissions(currentUserRoles, project, user, avaliableRoles.Any());
                    }

                    //set the current project to identity context
                    IdentityContext.CurrentProjectId = project.Id;

                    return project;
                }
            }
        }

        public Project GetProjectForPublish(int id)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                using (var multi = conn.QueryMultiple("GetProject", new { ProjectId = id }, commandType: CommandType.StoredProcedure))
                {
                    //var project = multi.Read<Project>().FirstOrDefault();  

                    var project = multi.Read<Project, Organization, UserView, Program, Project>((p, o, uv, program) =>
                    {
                        p.Organization = o;
                        p.User = uv;
                        p.Program = program;
                        return p;
                    }).FirstOrDefault();

                    var projectActivities = multi.Read<ProjectActivity>();
                    var projectFollowUps = multi.Read<ProjectFollowUp>();
                    var projectGoals = multi.Read<ProjectGoal>();
                    var projectMembers = multi.Read<ProjectMemberView>();
                    var projectRoles = multi.Read<ProjectRoleView>();
                    var projectRisks = multi.Read<ProjectRisk>();

                    if (project == null) return null;

                    if (projectActivities.Any())
                        project.ProjectActivities = projectActivities.ToList();

                    if (projectFollowUps.Any())
                        project.ProjectFollowUps = projectFollowUps.ToList();

                    if (projectGoals.Any())
                        project.ProjectGoals = projectGoals.ToList();

                    var projectMemberViews = projectMembers as List<ProjectMemberView> ?? projectMembers.ToList();
                    if (projectMemberViews.Any())
                        project.ProjectMembers = projectMemberViews;

                    // add member roles
                    var projectRoleView = projectRoles as List<ProjectRoleView> ?? projectRoles.ToList();
                    foreach (var member in project.ProjectMembers)
                    {
                        ProjectMemberView memberCopy = member;
                        foreach (var roleView in projectRoleView.Where(roleView => memberCopy.UserId == roleView.UserId))
                        {
                            member.ProjectRoles.Add(roleView);
                        }
                    }

                    if (projectRisks.Any())
                        project.ProjectRisks = projectRisks.ToList();

                    return project;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public int CreateProject(Project project)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                par.Add("@OrganizationId", project.OrganizationId);
                par.Add("@Name", project.Name);
                par.Add("@Description", project.Description);
                par.Add("@StartDate", project.StartDate);
                par.Add("@CreatedById", project.CreatedById);
                par.Add("@ProjectAreaId", project.ProjectAreaId);
                par.Add("@ProgramOwnerId", project.ProgramOwnerId);
                par.Add("@ProjectCoordinatorId", project.ProjectCoordinatorId);

                int result = conn.Query<int>("CreateProject", par, commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (result < 1)
                    throw new RepositoryException("Projektet sparades inte, ett projekt med samma namn finns redan", project.Name);

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        public void UpdateProject(Update update)
        {
            var validation = string.Empty;

            if (ValidateUpdate(update, out validation))
            {
                const string PROJECTID = "@ProjectId";
                const string UPDATEVALUE = "@UpdateValue";

                using (SqlConnection conn = SqlHelper.GetOpenConnection())
                {
                    var par = new Dapper.DynamicParameters();
                    par.Add(PROJECTID, update.ProjectId);
                    par.Add(UPDATEVALUE, update.Value);
                    conn.Execute(string.Format("UPDATE {0} SET {1} = {2} WHERE Id = {3}", update.TableName, update.ColumnName, UPDATEVALUE, PROJECTID), par);
                }
            }
            else
            {
                throw new DuplicateNameException("Error while updating project: " + validation);
            }
        }

        private bool ValidateUpdate(Update update, out string validation)
        {
            var valid = true;
            validation = "Validated Successfully";

            //validate the project name
            if (update.TableName.ToLower().Equals("project") && update.ColumnName.ToLower().Equals("name"))
            {
                using (SqlConnection conn = SqlHelper.GetOpenConnection())
                {
                    var par = new Dapper.DynamicParameters();
                    par.Add("@ProjectId", update.ProjectId);
                    par.Add("@ProjectName", update.Value);
                    valid = conn.Query<bool>("ValidateProjectName", par, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (valid == false)
                        validation = "There is already a project with same name and organization";
                }
            }

            return valid;
        }

        public void DeleteProject(int projectId)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                connection.Execute("UPDATE Project SET [Active] = 0 WHERE Id = @ProjectId", new { ProjectId = projectId });
            }
        }

        public void RestoreProject(int projectId)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                connection.Execute("UPDATE Project SET [Active] = 1 WHERE Id = @ProjectId", new { ProjectId = projectId });
            }
        }

        public ProjectMemberView CreateProjectMember(ProjectMember member, out bool memberExists, out bool userExists)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                // Get user          

                var p = new Dapper.DynamicParameters();
                p.Add("@ProjectId", member.ProjectId);
                p.Add("@Name", member.Name);
                p.Add("@Email", member.Email);
                p.Add("@ProjectRoles", string.Join<int>(",", member.ProjectRoleIds));
                //p.Add("@MemberExists", dbType: DbType.Boolean, direction: ParameterDirection.ReturnValue);
                //p.Add("@UserExists", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                //var result = connection.Query<ProjectMemberView, User, ProjectMemberView>("dbo.[CreateProjectMember]", (pmv, u) =>
                // {
                //     pmv.UserId = u.Id;
                //     return pmv;
                // }, p, commandType: CommandType.StoredProcedure).FirstOrDefault();


                var multi = connection.QueryMultiple("[CreateProjectMember]", p, commandType: CommandType.StoredProcedure);

                var memberViews = multi.Read<ProjectMemberView>().FirstOrDefault();

                memberExists = Convert.ToBoolean(multi.Read<bool>().FirstOrDefault());
                userExists = Convert.ToBoolean(multi.Read<bool>().FirstOrDefault());

                //memberExists = Convert.ToBoolean(p.Get<bool>("@MemberExists"));
                //userExists = false;//Convert.ToBoolean(p.Get<bool>("@UserExists"));

                //var userExistsRes = multi.Read<int>().FirstOrDefault();
                //userExists = Convert.ToBoolean(userExistsRes);

                return memberViews;
            }
        }

        public IEnumerable<UpdateProjectRoleResult> UpdateProjectMemberRole(ProjectMember member)
        {
            var p = new Dapper.DynamicParameters();
            p.Add("@CurrentUserId", IdentityContext.Current.User.Id);
            p.Add("@MemberUserId", member.UserId);
            p.Add("@MemberId", member.Id);
            p.Add("@ProjectId", member.ProjectId);
            p.Add("@RoleIdList", string.Join<int>(",", member.ProjectRoleIds));

            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var results = conn.Query<UpdateProjectRoleResult>("UpdateProjectRoles", p, commandType: CommandType.StoredProcedure);
                return results;
            };

            //BESKRIVNING
            //1. Vill kolla befintliga roller på användaren på ett visst projekt.
            //2. Current user. kanske inte har rättigheter att lägga till alla roller för användaren.
            //3. Den berörda databas tabellen gäller endast ProjectMemberRole

            //ÅTGÄRD
            //1. Hämta en lista på alla roller som current user får lägga till eller ta bort.
            //2. Jämför listan med de rollerna som finns på ProjektMember objektet från gränsnsittet
            //3. Ta bort alla poster som current user har rättighet till och som  i ProjektMember objektet från gränsnsittet.
            //4. Skapa nya poster för alla roller som current user har rättighet till och som finns i ProjektMember objektet från gränsnsittet.
            //5. Skicka tillbaka en lista för alla de roller som current user inte har rättighet till som finns i ProjektMember objektet från gränsnsittet.

            //på detta sätt så kan användare ta bort och lägga till endast projektmedlemmar som den har rättigheter till.

            //get all editable roleid´s for current user

            //var avaliableRoles = GetAvaliableRolesIds(member.ProjectId);

            ////filter out roles chosen by current user and available for edit
            //var filteredRoles = avaliableRoles.Select(f => f).Intersect(member.ProjectRoleIds).ToList();

            ////all role id´s from input where not exists in editable roles for current user
            //var removedRolesId = (from a in member.ProjectRoleIds
            //                      where !(from roleId in filteredRoles
            //                              select roleId).Contains(a)
            //                      select a);

            ////joins list of all roles from database 
            //List<ProjectRole> removedRoles = (from list1Item in GetRoles().ToList()
            //                                  join list2Item in removedRolesId on list1Item.Id equals list2Item
            //                                  select list1Item).ToList();

            ////get previous role for member
            //var previousRoles = GetProjectRolesByUserId(member.UserId, member.ProjectId);

            //foreach (var role in previousRoles)
            //{
            //    //add the previous role to filtered role
            //    filteredRoles.Add(role.Id);
            //}

            //using (SqlConnection conn = SqlHelper.GetOpenConnection())
            //{
            //    //removes previous roles
            //    var removePreviousRolesQuery = @"DELETE FROM ProjectMemberRole where ProjectMemberId = @MemberId";
            //    var result = conn.Execute(removePreviousRolesQuery, new { MemberId = member.Id });

            //    foreach (var roleId in filteredRoles.Distinct())
            //    {
            //        var addRoleQuery = @"INSERT INTO [ProjectMemberRole] ([ProjectMemberId],[ProjectRoleId]) VALUES (@MemberId, @RoleId)";
            //        var tempavaliableUserRoles = conn.Execute(addRoleQuery, new { MemberId = member.Id, RoleId = roleId });
            //    }
            //};

            //if (removedRoles.Count() > 0)
            //{
            //    return removedRoles.Where(f => !previousRoles.Select(y => y.Id).Contains(f.Id));
            //}

            //return new List<ProjectRole>();
        }

        public void DeleteProjectMember(int id)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                // Get user 
                var p = new Dapper.DynamicParameters();
                p.Add("@ProjectMemberId", id);
                connection.Execute("[DeleteProjectMember]", p, commandType: CommandType.StoredProcedure);
            }
        }

        public int CreateProjectRisk(ProjectRisk risk)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                //var projectQuery = BuildInsertQuery(risk);
                //int projectId = connection.Query<int>(projectQuery, risk).Single();
                int projectId = connection.Insert(risk);
                return projectId;
            }
        }

        public void UpdateProjectRisk(ProjectRisk risk)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                connection.Update(risk);

                //var query = BuildUpdateQuery(risk);
                //connection.Execute(query, risk);
            }
        }

        public void DeleteProjectRisk(int id)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                par.Add("@Id", id);
                conn.Execute("DELETE ProjectRisk WHERE Id = @Id", par);
            }
        }

        public int CreateProjectGoal(ProjectGoal goal)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                int projectId = connection.Insert(goal);
                return projectId;
            }
        }

        public void UpdateProjectGoal(ProjectGoal goal)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                connection.Update(goal);
            }
        }

        public void DeleteProjectGoal(int id)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                par.Add("@Id", id);
                conn.Execute("DELETE ProjectGoal WHERE Id = @Id", par);
            }
        }

        public int CreateProjectActivity(ProjectActivity activity)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                int projectId = connection.Insert(activity);
                return projectId;
            }
        }

        public void UpdateProjectActivity(ProjectActivity activity)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                connection.Update(activity);
            }
        }

        public void DeleteProjectActivity(int id)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                par.Add("@Id", id);
                conn.Execute("DELETE ProjectActivity WHERE Id = @Id", par);
            }
        }

        public int CreateProjectFollowUp(ProjectFollowUp followUp)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                int projectId = connection.Insert(followUp);
                return projectId;
            }
        }

        public void UpdateProjectFollowUp(ProjectFollowUp followUp)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                connection.Update(followUp);
            }
        }

        public void DeleteProjectFollowUp(int id)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                par.Add("@Id", id);
                conn.Execute("DELETE ProjectFollowUp WHERE Id = @Id", par);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="projectVersion"></param>
        /// <returns></returns>
        public int CreateVersion(ProjectVersion projectVersion)
        {
            using (SqlConnection connection = SqlHelper.GetOpenConnection())
            {
                var query = @"
					SELECT 
					CAST (
						CASE WHEN EXISTS (
							SELECT Id FROM 
							ProjectVersion 
							where Version like @Version and Name = @Name) 
								THEN 1 ELSE 0 END AS BIT
						)";

                var projectVersionQuery = @"
									INSERT INTO ProjectVersion ([ProjectId],[PhaseId],[Comment],[ProjectData],[PublishedDate],[PublishedBy])
									VALUES (@ProjectId,@PhaseId,@Comment,@ProjectData,@PublishedDate,@PublishedBy);
									SELECT CAST(SCOPE_IDENTITY() AS INT);";

                int versionDatabaseId = connection.Query<int>(projectVersionQuery, new
                {
                    ProjectId = projectVersion.ProjectId,
                    PhaseId = projectVersion.PhaseId,
                    Comment = projectVersion.Comment,
                    ProjectData = projectVersion.ProjectData,
                    PublishedDate = projectVersion.PublishedDate,
                    PublishedBy = projectVersion.PublishedBy
                }).Single();

                //project.ProjectMembers.Add(CreateMember(projectId, currentUserId));
                projectVersion.Id = versionDatabaseId;
                //transaction.Commit();
                return versionDatabaseId;
            }
        }

        public IEnumerable<ProjectVersion> GetVersionList(int projectId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var query = @"SELECT p.*, uv.Name as PublishedByName FROM [ProjectVersion] p INNER JOIN [UserView] uv ON p.PublishedBy = uv.Id where p.ProjectId = @ProjectId order by p.PublishedDate desc";
                IEnumerable<ProjectVersion> projectVersions = conn.Query<ProjectVersion>(query, new { ProjectId = projectId });
                return projectVersions;
            }
        }

        public ProjectVersion GetVersion(int versionId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var query = @"SELECT p.*, u.Name as PublishedByName FROM [ProjectVersion] p, [User] u WHERE p.PublishedBy = u.Id and p.Id = @Id";

                ProjectVersion projectVersions = conn.Query<ProjectVersion>(query, new { Id = versionId }).FirstOrDefault();
                return projectVersions;
            }
        }

        public ProjectVersion GetLatestVersion(int projectId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var query = @"SELECT TOP 1 * FROM [ProjectVersion] WHERE ProjectId = @ProjectId order by PublishedDate desc";
                ProjectVersion projectVersions = conn.Query<ProjectVersion>(query, new { ProjectId = projectId }).FirstOrDefault();
                return projectVersions;
            }
        }

        public IEnumerable<ProjectArea> GetProjectAreas()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var query = @"SELECT pa.*, p.* FROM ProjectArea pa INNER JOIN Program p ON pa.ProgramID = p.Id";

                return conn.Query<ProjectArea, Program, ProjectArea>(query, (pa, p) =>
                {
                    pa.Program = p;
                    return pa;
                }, commandType: CommandType.Text);
            }
        }

        public IEnumerable<ProjectArea> GetProjectAreas(bool active)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var query = @"SELECT pa.*, p.* FROM ProjectArea pa INNER JOIN Program p ON pa.ProgramID = p.Id WHERE pa.Active = @Active";

                return conn.Query<ProjectArea, Program, ProjectArea>(query, (pa, p) =>
                {
                    pa.Program = p;
                    return pa;
                }, new { Active = active }, commandType: CommandType.Text);
            }
        }

        public IEnumerable<Program> GetPrograms()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<Program>(@"SELECT  * FROM Program WHERE Active = 1");
            }
        }

        public IEnumerable<Organization> GetOrganizations()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<Organization>(@"SELECT * FROM Organization");
            }
        }

        public IEnumerable<ProjectRole> GetRoles()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<ProjectRole>(@"SELECT * FROM ProjectRole");
            }
        }

        public IEnumerable<ProjectRole> GetProjectRolesByUserId(int userId, int projectId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var query = @"select pr.* from ProjectRole pr
							inner join ProjectMemberRole pmr on pmr.ProjectRoleId = pr.Id
							inner join ProjectMember pm on pm.Id = pmr.ProjectMemberId
							where pm.UserId = @UserId and pm.ProjectId = @ProjectId";
                return conn.Query<ProjectRole>(query, new { UserId = userId, ProjectId = projectId }).ToList();
            }
        }

        public IEnumerable<int> GetAvaliableRolesIds(int projectId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var query = @"
					SELECT
					pra.RoleId
					FROM ProjectMember pm
					INNER JOIN ProjectMemberRole pmr ON pmr.ProjectMemberId = pm.Id
					INNER JOIN ProjectRole pr ON pmr.ProjectRoleId = pr.Id
					INNER JOIN ProjectRoleGroup prg ON pr.ProjectRoleGroupId = prg.Id
					INNER JOIN ProjectRoleAdministration pra ON pra.RoleGroupId = prg.Id
					WHERE pm.UserId = @CurrentUserId AND pm.ProjectId = @ProjectId";

                return conn.Query<int>(query, new { CurrentUserId = IdentityContext.Current.User.Id, ProjectId = projectId });
            };
        }

        public IEnumerable<ProjectRole> GetAvaliableRoles(int userId, int projectId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                if (IdentityContext.Current.User.Role == Role.SuperUser)
                {
                    var query = @"SELECT * FROM ProjectRole";
                    return conn.Query<ProjectRole>(query);
                }
                else
                {
                    var query = @"
						SELECT * FROM ProjectRole WHERE Id IN (
							SELECT
							pra.RoleId
							FROM ProjectMember pm
							INNER JOIN ProjectMemberRole pmr ON pmr.ProjectMemberId = pm.Id
							INNER JOIN ProjectRole pr ON pmr.ProjectRoleId = pr.Id
							INNER JOIN ProjectRoleGroup prg ON pr.ProjectRoleGroupId = prg.Id
							INNER JOIN ProjectRoleAdministration pra ON pra.RoleGroupId = prg.Id
							WHERE pm.UserId = @UserId AND pm.ProjectId = @ProjectId)";

                    return conn.Query<ProjectRole>(query, new { UserId = userId, ProjectId = projectId });
                }
            };
        }

        public void ToggleFavorite(int projectId, bool favorite)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                par.Add("@ProjectId", projectId);
                par.Add("@UserId", IdentityContext.Current.User.Id);

                if (favorite)
                    conn.Execute(@"INSERT INTO ProjectFavorite (ProjectId, UserId) VALUES (@ProjectId, @UserId)", par);
                else
                    conn.Execute(@"DELETE FROM ProjectFavorite WHERE ProjectId = @ProjectId AND UserId = @UserId", par);
            }
        }

        public IEnumerable<UserView> GetProgramAdministrators(int programTypeId, int projectRoleId, int organizationId)
        {

            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                par.Add("@ProgramTypeId", programTypeId);
                par.Add("@ProjectRoleId", projectRoleId);
                par.Add("@organizationId ", organizationId);
                var query = @"";
                if (programTypeId > 1)
                {
                    query = @"SELECT uv.* FROM UserView uv
							INNER JOIN  ProgramRoleAdministration pra
							ON uv.Id = pra.UserId
							WHERE pra.ProgramTypeId = @ProgramTypeId 
							AND pra.ProjectRoleId = @ProjectRoleId
							AND pra.OrganizationId = @organizationId ";
                }
                else
                {
                    query = @"SELECT uv.* FROM UserView uv
							INNER JOIN  ProgramRoleAdministration pra
							ON uv.Id = pra.UserId
							WHERE pra.ProgramTypeId = @ProgramTypeId 
							AND pra.ProjectRoleId = @ProjectRoleId";
                }
                return conn.Query<UserView>(query, par);
            }
        }

        public IEnumerable<ProgramTypeModel> GetProgramTypes()
        {
            var types = new List<ProgramTypeModel>();

            foreach (var type in System.Enum.GetValues(typeof(ProgramType)).Cast<ProgramType>().AsEnumerable<ProgramType>())
            {
                types.Add(new ProgramTypeModel()
                {
                    Id = EnumHelper.EnumToInt(type),
                    Name = type.ToString(),
                    Description = EnumHelper.GetEnumDescription(type)
                });
            }

            return types;
        }

        public IEnumerable<UserView> GetUserListAdministration()
        {

            //Filter på område: om user är länssamordnare eller kommunsamordnare.
            //Ex kommunsamordnare skall lista alla användare i sin kommun
            //Ex länssamordnare skall lista alla användare i sitt län
            //Ex superuser skall se alla användare
            //Ex om man är vanlig användare skall inget listas
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                IEnumerable<UserView> userList = conn.Query<UserView>(@"SELECT * FROM [UserView]");
                return SortUserListByCurrentUserSystemRole(userList);
            }
        }

        public IEnumerable<UserView> GetProjectCoordinators()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                IEnumerable<UserView> userList = conn.Query<UserView>(@"select distinct uv.*
											from [UserView] uv
											inner join ProgramRoleAdministration pra on pra.UserId = uv.Id
											inner join ProjectRole pr on pr.Id = pra.ProjectRoleId
											where pr.Id = 6");
                return SortUserListByCurrentUserSystemRole(userList);

            }
        }

        public IEnumerable<UserView> GetProgramOwners()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                IEnumerable<UserView> userList = conn.Query<UserView>(@"select distinct uv.*
											from [UserView] uv
											inner join ProgramRoleAdministration pra on pra.UserId = uv.Id
											inner join ProjectRole pr on pr.Id = pra.ProjectRoleId
											where pr.Id = 1");
                return SortUserListByCurrentUserSystemRole(userList);
            }
        }


        public IEnumerable<ProgramRoleAdministration> GetUserProgramRoleAdministration(int userId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<ProgramRoleAdministration>(@"SELECT * FROM [ProgramRoleAdministration] WHERE UserId = @UserId", new { userId });
            }
        }

        private IEnumerable<UserView> SortUserListByCurrentUserSystemRole(IEnumerable<UserView> inputList)
        {
            var user = IdentityContext.Current.User;
            IEnumerable<UserView> filteredUsers;
            if (user.Role == Role.CountyAdmin)
            {
                filteredUsers = from p in inputList
                                where p.County.Equals(user.County)
                                select p;
                return filteredUsers;
            }
            else if (user.Role == Role.OrganizationAdmin)
            {
                filteredUsers = from p in inputList
                                where p.Organization.Equals(user.Organization)
                                select p;
                return filteredUsers;
            }
            else if (user.Role == Role.SuperUser)
            {
                return inputList;
            }
            else
            {
                return null;
            }
        }



        public bool UpdateUser(User user)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                string updateQuery = @"UPDATE [dbo].[User]
											SET [Name] = @Name
											,[Email] = @Email                                          
											,[RoleId] = @RoleId                                          
											WHERE Id = @UserId";
                int result = conn.Execute(updateQuery, new { Name = user.Name, Email = user.Email, RoleId = user.RoleId, UserId = user.Id });
                if (result == -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }




        }

        public IEnumerable<Role> GetSystemRoles()
        {

            var enumValues = Enum.GetValues(typeof(Role));


            foreach (var value in enumValues)
            {
                int itemvalue = (int)value;
                EnumHelper.GetEnumDescription((Role)itemvalue);
            }
            return new List<Role>();
        }


        public void CreateProgramRoleAdministrator(int userId, int programTypeId, int projectRoleId, int? organizationId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var query = string.Empty;
                var par = new Dapper.DynamicParameters();
                par.Add("@UserId", userId);
                par.Add("@ProgramTypeId", programTypeId);
                par.Add("@ProjectRoleId", projectRoleId);

                if (organizationId.HasValue)
                {
                    par.Add("@OrganizationId", organizationId.Value);
                    query = @"INSERT INTO ProgramRoleAdministration (UserId, ProgramTypeId, ProjectRoleId, OrganizationId) VALUES (@UserId, @ProgramTypeId, @ProjectRoleId, @OrganizationId)";
                }
                else
                    query = @"INSERT INTO ProgramRoleAdministration (UserId, ProgramTypeId, ProjectRoleId) VALUES (@UserId, @ProgramTypeId, @ProjectRoleId)";

                conn.Execute(query, par);
            }
        }

        public IEnumerable<int> GetUnpublishedProjects()
        {
            var query = @"
						SELECT 
						p.Id
						FROM Project p
						INNER JOIN ProjectArea pa ON p.ProjectAreaId = pa.Id
						INNER JOIN Program pgm ON pa.ProgramId = pgm.Id
						LEFT JOIN ProjectVersion pv ON pv.ProjectId = p.Id
						WHERE p.Active = 1 AND pv.Id IS NULL AND pgm.Id = 3--eHälsoprojekt 2013";

            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<int>(query);
            }
        }

        public void InsertMappedProjectMemberRoles(int projectMemberId, int projectRoleId)
        {
            string query = @"IF NOT EXISTS (SELECT * FROM ProjectMemberRole WHERE ProjectmemberId = @ProjectmemberId AND ProjectRoleId = @ProjectRoleId)
							 BEGIN
								INSERT INTO ProjectMemberRole (ProjectmemberId, ProjectRoleId) VALUES (@ProjectmemberId, @ProjectRoleId)
							 END";

            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                conn.Execute(query, new { ProjectmemberId = projectMemberId, ProjectRoleId = projectRoleId });
            }
        }

        public void DeleteProgramRoleAdministrator(int userId, int programTypeId, int projectRoleId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                par.Add("@UserId", userId);
                par.Add("@ProgramTypeId", programTypeId);
                par.Add("@ProjectRoleId", projectRoleId);

                var query = @"DELETE FROM ProgramRoleAdministration 
							  WHERE 
							  UserId = @UserId AND 
							  ProgramTypeId = @ProgramTypeId AND 
							  ProjectRoleId = @ProjectRoleId";

                conn.Execute(query, par);
            }
        }

        public IEnumerable<ProjectVersion> GetProjectVersions()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<ProjectVersion>("SELECT * FROM ProjectVersion");
            }
        }

        public IEnumerable<int> GetProjectIds()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<int>("SELECT Id FROM Project WHERE Active = 1");
            }
        }

        public void UpdateProjectFollowUpActivityTotal(int projectFollowUpId, decimal total)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                par.Add("@ActivityTotal", total);
                par.Add("@Id", projectFollowUpId);
                conn.Execute("UPDATE ProjectFollowUp SET ActivityTotal = @ActivityTotal WHERE Id = @Id", par);
            }
        }

        public Template GetTemplate(string key)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new Dapper.DynamicParameters();
                return conn.Query<Template>("SELECT * FROM Template WHERE [Key] = @Key", new { Key = key }).FirstOrDefault();
            }
        }
    }
}
