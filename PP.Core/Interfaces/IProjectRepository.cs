using System.Collections.Generic;
using PP.Core.Model;
using PP.Core.Model.Enum;

namespace PP.Core.Interfaces
{
    public interface IProjectRepository
    {

        IEnumerable<Project> SearchProjects(string search);
        IEnumerable<UserView> SearchUsers(string search);
        IEnumerable<ProjectView> GetProjects(ActiveState activeState);
        IEnumerable<ProjectView> GetProjects(UserView user, ActiveState activeState);
        IEnumerable<ProjectView> GetCountyProjects();
        IEnumerable<ProjectView> GetInActiveProjects();
        Project GetProject(int id);

        IEnumerable<int> GetProjectIds();
        int CreateProject(Project project);
        void UpdateProject(Update update);
        int CreateVersion(ProjectVersion version);
        IEnumerable<ProjectVersion> GetVersionList(int id);
        ProjectVersion GetVersion(int id);
        ProjectVersion GetLatestVersion(int id);
        int CreateProjectRisk(ProjectRisk risk);
        void DeleteProjectRisk(int riskId);
        void DeleteProject(int projectId);
        void RestoreProject(int projectId);
        void UpdateProjectRisk(ProjectRisk risk);
        int CreateProjectGoal(ProjectGoal goal);
        void UpdateProjectGoal(ProjectGoal goal);
        void DeleteProjectGoal(int id);
        int CreateProjectActivity(ProjectActivity activity);
        void UpdateProjectActivity(ProjectActivity activity);
        void DeleteProjectActivity(int id);
        int CreateProjectFollowUp(ProjectFollowUp followUp);
        void UpdateProjectFollowUp(ProjectFollowUp followUp);
        void DeleteProjectFollowUp(int id);
        IEnumerable<ProjectArea> GetProjectAreas();
        IEnumerable<ProjectArea> GetProjectAreas(bool active);
        IEnumerable<Program> GetPrograms();
        IEnumerable<Organization> GetOrganizations();
        void ToggleFavorite(int projectId, bool favorite);
        ProjectMemberView CreateProjectMember(ProjectMember member, out bool memberExists, out bool userExists);
        //IEnumerable<ProjectRole> UpdateProjectMemberRole(ProjectMember member);
        IEnumerable<UpdateProjectRoleResult> UpdateProjectMemberRole(ProjectMember member);
        void DeleteProjectMember(int id);
        IEnumerable<ProjectRole> GetRoles();
        IEnumerable<UserView> GetProgramAdministrators(int programTypeId, int projectRoleId, int organizationId);

        IEnumerable<ProgramRoleAdministration> GetUserProgramRoleAdministration(int userId);
        IEnumerable<ProgramTypeModel> GetProgramTypes();

        void CreateProgramRoleAdministrator(int userId, int programTypeId, int projectRoleId, int? organizationId);

        void DeleteProgramRoleAdministrator(int userId, int programTypeId, int projectRoleId);

        IEnumerable<int> GetAvaliableRolesIds(int projectId);
        IEnumerable<ProjectRole> GetAvaliableRoles(int userId, int projectId);
        IEnumerable<UserView> GetUserListAdministration();
        IEnumerable<UserView> GetProjectCoordinators();
        IEnumerable<UserView> GetProgramOwners();
        IEnumerable<Role> GetSystemRoles();
        bool UpdateUser(User user);

        IEnumerable<int> GetUnpublishedProjects();

        Project GetProjectForPublish(int id);

        void InsertMappedProjectMemberRoles(int projectMemberId, int projectRoleId);

        IEnumerable<ProjectVersion> GetProjectVersions();

        void UpdateProjectFollowUpActivityTotal(int projectFollowUpId, decimal total);

        Template GetTemplate(string key);
    }
}
 