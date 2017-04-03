using System.Web.Http;
using PP.Api.Mapper;
using PP.Api.Models;
using PP.Core;
using PP.Core.Interfaces;
using PP.Core.Model;
using PP.Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using PP.Core.Model.Enum;
using System;
using System.Net;
using System.Web.Http.Cors;
using PP.Api.Helpers;
using System.IO;
using System.Net.Http;

namespace PP.Api.Controllers
{
    [RoutePrefix("project")]
    public class ProjectController : BaseController
    {

        [HttpPost]
        [Route("users/search")]
        [AllowAnonymous]
        //[EnableCors("ori, ori2", "headers,...", "GET")]
        public IEnumerable<UserViewModel> SearchUsers(SearchUserModel search)
        {
            // Mapp to backend model 
            IEnumerable<UserView> users = projectRepository.SearchUsers(search.Text);
            IEnumerable<UserViewModel> models = null;

            if (users != null)
            {
                models = ApplicationMapper.MapUserViewList(users);
            }
            return models;
        }

        [HttpGet]
        [Route("all")]
        [AllowAnonymous]
        //[EnableCors("ori, ori2", "headers,...", "GET")]
        public IEnumerable<ProjectViewModel> GetProjectList()
        {
            // Mapp to backend model 
            IEnumerable<ProjectView> projectViewList = projectRepository.GetProjects(ActiveState.All);
            IEnumerable<ProjectViewModel> content = null;

            if (projectViewList != null)
            {

                content = ApplicationMapper.MapProjectContent(projectViewList);
            }
            return content;
        }

        [HttpGet]
        [Route("all/{active}")]
        [AllowAnonymous]
        //[EnableCors("ori, ori2", "headers,...", "GET")]
        public IEnumerable<ProjectViewModel> GetProjectList(bool active)
        {
            ActiveState state;

            if (active)
                state = ActiveState.Active;
            else
                state = ActiveState.InActive;

            // Mapp to backend model 
            IEnumerable<ProjectView> projectViewList = projectRepository.GetProjects(state);
            IEnumerable<ProjectViewModel> content = null;

            if (projectViewList != null)
            {

                content = ApplicationMapper.MapProjectContent(projectViewList);
            }
            return content;
        }

        [HttpGet]
        [Route("all/county")]
        [AllowAnonymous]
        public IEnumerable<ProjectViewModel> GetAdministratorProjectList()
        {
            // Mapp to backend model 
            IEnumerable<ProjectView> projectViewList = projectRepository.GetCountyProjects();
            IEnumerable<ProjectViewModel> content = null;

            if (projectViewList != null)
            {

                content = ApplicationMapper.MapProjectContent(projectViewList);
            }
            return content;
        }

        [HttpGet]
        [Route("inactive")]
        [AllowAnonymous]
        public IEnumerable<ProjectViewModel> GetInActiveProjectList()
        {
            // Mapp to backend model
            IEnumerable<ProjectView> projectViewList = projectRepository.GetInActiveProjects();
            IEnumerable<ProjectViewModel> content = null;

            if (projectViewList != null)
            {
                content = ApplicationMapper.MapProjectContent(projectViewList);
            }
            return content;
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public ProjectModel GetProject(int id)
        {
            ProjectModel content = null;
            // Get project from DB
            Core.Model.Project project = projectRepository.GetProject(id);


            //Mapp to UI model 
            if (project != null)
            {
                content = ApplicationMapper.MapProjectContent(project);
            }
            return content;
        }

        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public int CreateProject(CreateProjectModel projectContent)
        {
            Project project = new Project();

            if (projectContent != null)
            {
                project = ApplicationMapper.MapCreateProjectContent(projectContent);
            }

            return projectRepository.CreateProject(project);
        }

        [HttpPost]
        [Route("update")]
        [AllowAnonymous]
        public void UpdateProject(UpdateModel updateModel)
        {
            var update = new Update();
            if (updateModel != null)
            {
                update = ApplicationMapper.MapProjectUpdate(updateModel);
            }

            projectRepository.UpdateProject(update);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [AllowAnonymous]
        public void DeleteProject(int id)
        {
            // Delete P.Risk
            projectRepository.DeleteProject(id);
        }

        [HttpDelete]
        [Route("restore/{id}")]
        [AllowAnonymous]
        public void RestoreProject(int id)
        {
            // Delete P.Risk
            projectRepository.RestoreProject(id);
        }

        [HttpPost]
        [Route("create/member")]
        [AllowAnonymous]
        public MemberModel CreateProjectMember(MemberModel memberModel)
        {
            // Mapp to backend model 
            var member = new ProjectMember();
            if (memberModel != null)
            {
                member = ApplicationMapper.MapProjectMember(memberModel);
            }
            // Create P.Member
            bool memberExists;
            bool userExists;
            ProjectMemberView pmv = projectRepository.CreateProjectMember(member, out memberExists, out userExists);

            //If the member is a new member
            if (!userExists)
            {
                if (!string.IsNullOrEmpty(pmv.UserEmail))
                {
                    ITextReplace replace = new TextReplace();
                    //replace.Add("%name%", pmv.UserName);
                    replace.Add("%username%", pmv.UserEmail.Trim());
                    string body = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "NewUser.html"));
                    // TODO replace with setting
                    MailService smtp = new MailService("http://communicationservice.woxion.com/MailService.asmx");
                    bool result = smtp.Send("Projektplaneraren", "info@projektplaneraren.se", pmv.UserEmail.Trim(), null, "Projektplaneraren", body);
                }
                else
                {
                    throw new Exception("Could not send new member email. Email is null or empty.");
                }
            }

            MemberModel createMemberModel = ApplicationMapper.MapProjectMemberView(pmv);
            createMemberModel.memberExists = memberExists;
            createMemberModel.MemberRoles = memberModel.MemberRoles;
            return createMemberModel;
        }

        [HttpPost]
        [Route("update/member")]
        [AllowAnonymous]
        public IEnumerable<ProjectMemberRoleResultsModel> UpdateProjectMember(MemberModel memberModel)
        {
            // Mapp to backend model 
            var member = new ProjectMember();
            if (member != null)
            {
                member = ApplicationMapper.MapProjectMember(memberModel);
                IEnumerable<UpdateProjectRoleResult> results = projectRepository.UpdateProjectMemberRole(member);
                return ApplicationMapper.MapProjectMemberResults(results);
            }
            // Create P.Risk
            // projectRepository.UpdateProjectRisk(risk);
            return null;
        }

        [HttpDelete]
        [Route("delete/member/{id}")]
        [AllowAnonymous]
        public void DeleteProjectMember(int id)
        {
            // Delete P.Followup
            projectRepository.DeleteProjectMember(id);
        }

        [HttpPost]
        [Route("create/risk")]
        [AllowAnonymous]
        public int CreateProjectRisk(RiskModel riskModel)
        {
            // Mapp to backend model 
            var risk = new ProjectRisk();
            if (riskModel != null)
            {
                risk = ApplicationMapper.MapProjectRisk(riskModel);
            }
            // Create P.Risk
            return projectRepository.CreateProjectRisk(risk);
        }

        [HttpPost]
        [Route("update/risk")]
        [AllowAnonymous]
        public void UpdateProjectRisk(RiskModel riskModel)
        {
            // Mapp to backend model 
            var risk = new ProjectRisk();
            if (riskModel != null)
            {
                risk = ApplicationMapper.MapProjectRisk(riskModel);
            }
            // Create P.Risk
            projectRepository.UpdateProjectRisk(risk);
        }

        [HttpDelete]
        [Route("delete/risk/{id}")]
        [AllowAnonymous]
        public void DeleteProjectRisk(int id)
        {
            // Delete P.Risk
            projectRepository.DeleteProjectRisk(id);
        }

        [HttpPost]
        [Route("create/goal")]
        [AllowAnonymous]
        public int CreateProjectGoal(GoalModel goalModel)
        {
            // Mapp to backend model 
            var goal = new ProjectGoal();
            if (goalModel != null)
            {
                goal = ApplicationMapper.MapProjectGoal(goalModel);
            }
            // Create P.Goal
            return projectRepository.CreateProjectGoal(goal);
        }

        [HttpPost]
        [Route("update/goal")]
        [AllowAnonymous]
        public void UpdateProjectGoal(GoalModel goalModel)
        {
            // Mapp to backend model 
            var goal = new ProjectGoal();
            if (goalModel != null)
            {
                goal = ApplicationMapper.MapProjectGoal(goalModel);
            }
            // Create P.Goal
            projectRepository.UpdateProjectGoal(goal);
        }

        [HttpDelete]
        [Route("delete/goal/{id}")]
        [AllowAnonymous]
        public void DeleteProjectGoal(int id)
        {
            // Delete P.Goal
            projectRepository.DeleteProjectGoal(id);
        }

        [HttpPost]
        [Route("create/activity")]
        [AllowAnonymous]
        public int CreateProjectActivity(ActivityModel activityModel)
        {
            // Mapp to backend model 
            var activity = new ProjectActivity();
            if (activityModel != null)
            {
                activity = ApplicationMapper.MapProjectActivity(activityModel);
            }
            // Create P.Goal
            return projectRepository.CreateProjectActivity(activity);
        }

        [HttpPost]
        [Route("update/activity")]
        [AllowAnonymous]
        public void UpdateProjectActivity(ActivityModel goalModel)
        {
            // Mapp to backend model 
            var activity = new ProjectActivity();
            if (goalModel != null)
            {
                activity = ApplicationMapper.MapProjectActivity(goalModel);
            }
            // Create P.Activity
            projectRepository.UpdateProjectActivity(activity);
        }

        [HttpDelete]
        [Route("delete/activity/{id}")]
        [AllowAnonymous]
        public void DeleteProjectActivity(int id)
        {
            // Delete P.Activity
            projectRepository.DeleteProjectActivity(id);
        }

        [HttpPost]
        [Route("create/followup")]
        [AllowAnonymous]
        public int CreateProjectFollowUp(FollowUpModel followUpModel)
        {
            // Mapp to backend model 
            var followUp = new ProjectFollowUp();
            if (followUpModel != null)
            {
                followUp = ApplicationMapper.MapProjectFollowUp(followUpModel);
            }
            // Create P.followup
            return projectRepository.CreateProjectFollowUp(followUp);
        }

        [HttpPost]
        [Route("update/followup")]
        [AllowAnonymous]
        public void UpdateProjectGoal(FollowUpModel followUpModel)
        {
            // Mapp to backend model 
            var followUp = new ProjectFollowUp();
            if (followUpModel != null)
            {
                followUp = ApplicationMapper.MapProjectFollowUp(followUpModel);
            }
            // Create P.FollowUp
            projectRepository.UpdateProjectFollowUp(followUp);
        }

        [HttpDelete]
        [Route("delete/followup/{id}")]
        [AllowAnonymous]
        public void DeleteProjectFollowUp(int id)
        {
            // Delete P.Followup
            projectRepository.DeleteProjectFollowUp(id);
        }

        [HttpPost]
        [Route("create/version")]
        [AllowAnonymous]
        public int CreateVersion(ProjectVersionModel projectVersion)
        {
            var version = new ProjectVersion();
            if (projectVersion != null)
            {
                version = ApplicationMapper.MapProjectVersion(projectVersion);
            }

            var project = new Project();
            if (projectVersion != null)
                project = projectRepository.GetProject(projectVersion.ProjectId);

            ISerializer xmlSerializer = new SerializeXml();
            xmlSerializer.Serialize(project);
            version.ProjectData = xmlSerializer.Serialize(project);

            int versionId = projectRepository.CreateVersion(version);

            //generate PDF
            PdfGenerator.GeneratePdf(project, projectVersion.PublishedByName, versionId, false, projectRepository.GetRoles());

            return versionId;
        }

        [HttpGet]
        [Route("create/pdf")]
        [AllowAnonymous]
        public void CreatePdf()
        {
            var projectVersions = CoreFactory.ProjectRepository.GetProjectVersions();

            foreach (var version in projectVersions)
            {
                var project = new SerializeXml().Deserialize<Project>(version.ProjectData);
                PdfGenerator.GeneratePdf(project, version.PublishedByName, version.Id, false, projectRepository.GetRoles());
            }
        }

        [HttpGet]
        [Route("create/pdf/{projectId}")]
        [AllowAnonymous]
        public HttpResponseMessage CreatePdf(int projectId)
        {
            //add a draft thus this is not a published pdf
            bool draft = true;
            //there is no version, only projectId
            int versionId = 0;
            var project = CoreFactory.ProjectRepository.GetProject(projectId);

            var creator = string.Empty;
            if (project.CreatedBy != null)
                if (!string.IsNullOrEmpty(project.CreatedBy.Name))
                    creator = project.CreatedBy.Name;

            PdfGenerator.GeneratePdf(project, creator, versionId, draft, projectRepository.GetRoles());

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<object>(new
                {
                    VersionId = versionId,
                    ProjectId = project.Id
                }, Configuration.Formatters.JsonFormatter)
            };

        }

        //[HttpGet]
        //[Route("create/pdf/{projectVersionId}/{draft}")]
        //[AllowAnonymous]
        //public HttpResponseMessage CreatePdf(int projectVersionId, bool draft)
        //{
        //    var version = CoreFactory.ProjectRepository.GetVersion(projectVersionId);

        //    var project = new SerializeXml().Deserialize<Project>(version.ProjectData);
        //    PdfGenerator.GeneratePdf(project, version.PublishedByName, version.Id, draft);

        //    return new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ObjectContent<object>(new
        //        {
        //            VersionId = version.Id,
        //            ProjectId = version.ProjectId
        //        }, Configuration.Formatters.JsonFormatter)
        //    };

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("versionList/{id}")]
        [AllowAnonymous]
        public IEnumerable<ProjectVersionModel> GetProjectVersionList(int id)
        {
            IEnumerable<ProjectVersionModel> content = null;

            // Get project from DB
            IEnumerable<ProjectVersion> projectVersions = projectRepository.GetVersionList(id);

            //Mapp to UI model 
            if (projectVersions != null)
            {
                content = ApplicationMapper.MapProjectVersionContent(projectVersions);
            }
            return content;
        }

        [HttpGet]
        [Route("version/{id}")]
        [AllowAnonymous]
        public ProjectVersionModel GetProjectVersion(int id)
        {
            ProjectVersionModel versionModel = null;
            ProjectVersion projectVersion = projectRepository.GetVersion(id);
            if (projectVersion != null)
            {
                versionModel = ApplicationMapper.MapProjectVersion(projectVersion);
            }
            return versionModel;
        }

        [HttpGet]
        [Route("version/latest/{id}")]
        [AllowAnonymous]
        public ProjectVersionModel GetLatestProjectVersion(int id)
        {
            ProjectVersionModel versionModel = null;
            ProjectVersion projectVersion = projectRepository.GetLatestVersion(id);
            if (projectVersion != null)
            {
                versionModel = ApplicationMapper.MapProjectVersion(projectVersion);
            }
            else
            {
                versionModel = new ProjectVersionModel();
            }
            return versionModel;
        }

        [HttpGet]
        [Route("phases")]
        public IEnumerable<SelectOption> GetProjectPhases()
        {
            return ApplicationMapper.MapPhaseSelectOptions();
        }

        [HttpGet]
        [Route("areas")]
        public IEnumerable<ProjectArea> GetProjectAreas()
        {
            return projectRepository.GetProjectAreas();
        }

        [HttpGet]
        [Route("areas/{active}")]
        public IEnumerable<ProjectArea> GetProjectAreas(bool active)
        {
            return projectRepository.GetProjectAreas(active);
        }

        [HttpGet]
        [Route("programs")]
        public IEnumerable<Program> GetPrograms()
        {
            return projectRepository.GetPrograms();
        }

        [HttpGet]
        [Route("organizations")]
        [AllowAnonymous]
        public IEnumerable<Organization> GetOrganizations()
        {
            return projectRepository.GetOrganizations().OrderBy(i => i.Name);
        }

        [HttpGet]
        [Route("roles")]
        [AllowAnonymous]
        public IEnumerable<RoleModel> GetRoles()
        {
            IEnumerable<ProjectRole> roleList = projectRepository.GetRoles();
            if (roleList != null)
            {
                return ApplicationMapper.MapProjectRoles(roleList);
            }
            return null;
        }

        [HttpGet]
        [Route("roles/avaliable/{userId}/{projectId}")]
        [AllowAnonymous]
        public IEnumerable<RoleModel> GetAvaliableRoles(int userId, int projectId)
        {
            IEnumerable<ProjectRole> roleList = projectRepository.GetAvaliableRoles(userId, projectId);
            if (roleList != null)
            {
                return ApplicationMapper.MapProjectRoles(roleList);
            }
            return null;
        }

        [HttpGet]
        [Route("programadmins/{programtypeid}/{projectroleid}/{organizationId}")]
        [AllowAnonymous]
        public IEnumerable<AdminUserModel> GetProgramAdministrators(int programTypeId, int projectRoleId, int organizationId)
        {
            IEnumerable<UserView> adminList = projectRepository.GetProgramAdministrators(programTypeId, projectRoleId, organizationId);
            if (adminList != null)
            {
                return ApplicationMapper.MapAdminUserList(adminList);
            }
            return null;
        }

        [HttpGet]
        [Route("programtypes")]
        [AllowAnonymous]
        public IEnumerable<ProgramTypeModel> GetProgramTypes()
        {
            return projectRepository.GetProgramTypes();
        }


        [HttpGet]
        [Route("favorite/{projectid}/{favorite}")]
        [AllowAnonymous]
        public void ToggleFavorite(int projectId, bool favorite)
        {
            projectRepository.ToggleFavorite(projectId, favorite);
        }

        [HttpPost]
        [Route("helptext")]
        public string GetHelpTextSection(UrlPassthrough value)
        {
            //using System.Net;
            string htmlCode = string.Empty;
            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString(value.Url);
            }
            return htmlCode;
        }

        [HttpGet]
        [Route("administration/users")]
        public IEnumerable<UserViewModel> GetUserListAdministration()
        {
            IEnumerable<UserView> userList = projectRepository.GetUserListAdministration();
            if (userList != null)
            {
                return ApplicationMapper.MapUserViewList(userList);
            }
            return null;
        }
        [HttpGet]
        [Route("administration/projectoordinators")]
        public IEnumerable<UserViewModel> GetProjectCoordinators()
        {
            IEnumerable<UserView> userList = projectRepository.GetProjectCoordinators();
            if (userList != null)
            {
                return ApplicationMapper.MapUserViewList(userList);
            }
            return null;
        }

        [HttpGet]
        [Route("administration/programcoowners")]
        public IEnumerable<UserViewModel> GetProgramOwners()
        {
            IEnumerable<UserView> userList = projectRepository.GetProgramOwners();
            if (userList != null)
            {
                return ApplicationMapper.MapUserViewList(userList);
            }
            return null;
        }

        [HttpGet]
        [Route("systemroles")]
        public IEnumerable<SelectOption> GetSystemRoles()
        {
            List<SelectOption> roles = new List<SelectOption>();
            var enumValues = Enum.GetValues(typeof(Role));


            foreach (var value in enumValues)
            {
                SelectOption option = new SelectOption();
                int itemvalue = (int)value;
                string description = EnumHelper.GetEnumDescription((Role)itemvalue);
                option.Id = itemvalue;
                option.Value = description;
                option.Name = description;
                roles.Add(option);
            }

            return roles;
        }

        [HttpPost]
        [Route("update/user")]
        public bool UpdateUser(UserViewModel user)
        {
            User coreUser = ApplicationMapper.MapUserUpdate(user);
            return projectRepository.UpdateUser(coreUser);
        }

        [HttpPost]
        [Route("administration/createprogramrole")]
        [AllowAnonymous]
        public void CreateAdministrationProgramRole(Api.Models.ProgramRoleAdministration obj)
        {
            projectRepository.CreateProgramRoleAdministrator(
                obj.UserId,
                obj.ProgramTypeId,
                obj.ProjectRoleId,
                obj.organizationId);
        }

        [HttpPost]
        [Route("administration/deleteprogramrole")]
        [AllowAnonymous]
        public void DeleteAdministrationProgramRole(Api.Models.ProgramRoleAdministration obj)
        {
            projectRepository.DeleteProgramRoleAdministrator(
                obj.UserId,
                obj.ProgramTypeId,
                obj.ProjectRoleId);
        }

        [HttpGet]
        [Route("administration/userprogramrole/{userId}")]
        [AllowAnonymous]
        public IEnumerable<Core.Model.ProgramRoleAdministration> GetUserProgramRoleAdministration(int userId)
        {
            var result = projectRepository.GetUserProgramRoleAdministration(userId);
            return result;
        }

        [HttpGet]
        [Route("administration/getcounties/")]
        [AllowAnonymous]
        public IEnumerable<Core.Model.Organization> GetCounties()
        {
            return projectRepository.GetOrganizations().DistinctBy(p => p.County);
        }

        /*ProjectComments*/

        [HttpGet]
        [Route("comments/get/{projectId}")]
        [AllowAnonymous]
        public IEnumerable<ProjectCommentViewModel> GetProjectComments(int projectId)
        {
            var models = new List<ProjectCommentViewModel>();
            var coreModels = commentRepository.GetComments(projectId);

            foreach (var coreModel in coreModels)
            {
                models.Add(ProjectCommentMapper.MapProjectComment(coreModel));
            }

            //order by date descending
            return models.OrderByDescending(c => c.Date).AsEnumerable();
        }

        [HttpGet]
        [Route("comments/areas/")]
        [AllowAnonymous]
        public IEnumerable<ProjectCommentAreaModel> GetProjectCommentAreas()
        {
            var models = new List<ProjectCommentAreaModel>();
            var coreModels = commentRepository.GetCommentAreas();

            int index = 1;

            foreach (var coreModel in coreModels)
            {
                models.Add(ProjectCommentMapper.MapProjectCommentArea(coreModel, index));
                index++;
            }

            return models;
        }

        [HttpGet]
        [Route("comments/types/{projectId}")]
        [AllowAnonymous]
        public IEnumerable<ProjectCommentTypeModel> GetProjectCommentTypes(int projectId)
        {
            var coreModels = commentRepository.GetCommentTypes(projectId);
            return ProjectCommentMapper.MapProjectCommentTypeModels(coreModels);
        }

        [HttpPost]
        [Route("comment/create/")]
        [AllowAnonymous]
        public ProjectCommentViewModel CreateProjectComment(ProjectCommentModel comment)
        {
            //map the incoming model to to a coreModel for insert
            var coreModel = ProjectCommentMapper.MapProjectCommentModel(comment);

            //insert the core model and return a core view
            var projectCommentView = commentRepository.CreateComment(coreModel);

            //map the core view to a model view
            var projectCommentViewModel = ProjectCommentMapper.MapProjectComment(projectCommentView);

            //return the model view
            return projectCommentViewModel;
        }

        [HttpPost]
        [Route("comment/edit/")]
        [AllowAnonymous]
        public void EditComment(ProjectCommentModel comment)
        {
            commentRepository.EditComment(comment.Id, comment.Text);
        }

        //[HttpPost]
        //[Route("comment/create/")]
        //[AllowAnonymous]
        //public ProjectCommentViewModel CreateComment(ProjectCommentModel comment)
        //{
        //    var projectCommentView = commentRepository.CreateComment(ProjectCommentMapper.MapProjectCommentModel(comment));

        //    var commentViewModel = ProjectCommentMapper.MapProjectComment()


        //}

        [HttpGet]
        [Route("comments/unread/{projectId}")]
        [AllowAnonymous]
        public IEnumerable<UnreadProjectComment> GetUnreadProjectComment(int projectId)
        {
            return commentRepository.GetUnreadProjectComments(projectId);
        }

        [HttpPost, HttpPut]
        [Route("comments/updateactivity/")]
        [AllowAnonymous]
        public void EditComment(UpdateProjectCommentActivityModel updateActivityModel)
        {
            commentRepository.UpdateProjectCommentActivity(updateActivityModel.ProjectId, updateActivityModel.ProjectCommentAreaId, updateActivityModel.ProjectCommentTypeId);
        }

        [HttpGet]
        [Route("templates/{key}")]
        [AllowAnonymous]
        public Template GetTemplate(string key)
        {
            return projectRepository.GetTemplate(key);
        }
    }
}