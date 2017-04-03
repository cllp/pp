using System;
using System.ComponentModel.DataAnnotations;

namespace PP.Core.Model
{
    public partial class ProjectCommentArea
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class __RefactorLog
    {
        public System.Guid OperationKey { get; set; }
    }

    public partial class Log
    {
        [Key]
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }
        public string Type { get; set; }
    }

    public partial class Organization
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string County { get; set; }
        public string Domain { get; set; }
    }

    public partial class Program
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public bool RequireProgramOwner { get; set; }
        public bool RequireProjectCoordinator { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public int? OrganizationId { get; set; }
    }

    public partial class ProgramRoleAdministration
    {
        public int UserId { get; set; }
        public int ProgramTypeId { get; set; }
        public int ProjectRoleId { get; set; }
        public int? OrganizationId { get; set; }
    }

    public partial class ProjectComment
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public int ProjectCommentAreaId { get; set; }
        public int ProjectCommentTypeId { get; set; }
        public System.DateTime Date { get; set; }
        public string Text { get; set; }
    }

    public partial class Project
    {
        public int FundingEstimate { get; set; }
        public int FundingActual { get; set; }
        public int FundingStimulus { get; set; }
        public int FundingExternal { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime LastUpdate { get; set; }
        public bool Active { get; set; }
        public int ProjectAreaId { get; set; }
        public decimal InternalCostPerHour { get; set; }
        public decimal ExternalCostPerHour { get; set; }
        public int PhaseId { get; set; }
        [Key]
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int TypeId { get; set; }
        public System.DateTime? StartDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PlanIntroductionBackground { get; set; }
        public string PlanIntroductionDefinition { get; set; }
        public string PlanIntroductionComments { get; set; }
        public string PlanDescriptionExtent { get; set; }
        public string PlanDescriptionDelimitation { get; set; }
        public string PlanDescriptionManagement { get; set; }
        public string PlanDescriptionEvaluation { get; set; }
        public string PlanImplementationDescription { get; set; }
        public string PlanCommunicationDefinition { get; set; }
        public string PlanCommunicationInterestAnalysis { get; set; }
        public string PlanCommunicationPlan { get; set; }
        public string Debriefing { get; set; }
    }

    public partial class ProjectActivity
    {
        [Key]
        public int Id { get; set; }
        public int Status { get; set; }
        public int InternalHours { get; set; }
        public int ExternalHours { get; set; }
        public int Cost { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public System.DateTime? Created { get; set; }
    }

    public partial class ProjectArea
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProgramId { get; set; }
        public bool Active { get; set; }
        public int? OrganizationId { get; set; }
    }

    public partial class Template
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Section { get; set; }
        public string Format { get; set; }
        public string Data { get; set; }
    }

    public partial class ProjectCommentType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class ProjectFavorite
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }

    public partial class ProjectFollowUp
    {
        public int InternalHours { get; set; }
        public int ExternalHours { get; set; }
        public int OtherCosts { get; set; }
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public bool Canceled { get; set; }
        public System.DateTime? Date { get; set; }
        public string Notes { get; set; }
        public int? ActivityTotal { get; set; }
    }

    public partial class ProjectGoal
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int Type { get; set; }
        public int Achieved { get; set; }
        public string GoalDefinition { get; set; }
        public string MesaureMethod { get; set; }
    }

    public partial class ProjectMember
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }

    public partial class ProjectCommentActivity
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public int ProjectCommentAreaId { get; set; }
        public int ProjectCommentTypeId { get; set; }
        public int LastReadCount { get; set; }
        public System.DateTime LastReadDate { get; set; }
    }

    public partial class ProjectMemberRole
    {
        public int ProjectMemberId { get; set; }
        public int ProjectRoleId { get; set; }
    }

    public partial class ProjectRisk
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ConsequenceId { get; set; }
        public int ProbabilityId { get; set; }
        public string Name { get; set; }
    }

    public partial class ProjectRole
    {
        [Key]
        public int Id { get; set; }
        public int PermissionId { get; set; }
        public int SortOrder { get; set; }
        public int ProjectRoleGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public partial class ProjectRoleAdministration
    {
        public int RoleId { get; set; }
        public int RoleGroupId { get; set; }
    }

    public partial class ProjectRoleGroup
    {
        [Key]
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public int ProjectCommentTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public partial class ProjectVersion
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int PhaseId { get; set; }
        public System.DateTime PublishedDate { get; set; }
        public int PublishedBy { get; set; }
        public string ProjectData { get; set; }
        public string Comment { get; set; }
    }

    public partial class Settings
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }

    public partial class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public int? RoleId { get; set; }
        public System.Guid? ChangePasswordRequest { get; set; }
        public System.DateTime? ChangePasswordDate { get; set; }
    }

    public partial class Report
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Procedure { get; set; }
        public string Description { get; set; }
    }

    public partial class ReportPermission
    {
        public int ReportId { get; set; }
        public int ProjectRoleGroupId { get; set; }
    }

    public partial class ProjectCommentTypePermission
    {
        public int ProjectCommentTypeId { get; set; }
        public int ProjectRoleGroupId { get; set; }
    }

    public partial class ProjectCommentView
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public int AreaId { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string Writer { get; set; }
        public string Area { get; set; }
        public string Text { get; set; }
    }

    public partial class NewId
    {
        public System.Guid? new_id { get; set; }
    }

    public partial class ProjectMemberView
    {
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int ProjectMemberId { get; set; }
        public string OrganizationState { get; set; }
        public string Organization { get; set; }
        public int? OrganizationId { get; set; }
        public string County { get; set; }
        public string Domain { get; set; }
        public int? RoleId { get; set; }
        public byte[] Password { get; set; }
    }

    public partial class ProjectRoleView
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int ProjectRoleId { get; set; }
        public int PermissionId { get; set; }
        public int ProjectRoleGroupId { get; set; }
        public string ProjectRoleGroupName { get; set; }
        public string UserEmail { get; set; }
        public string ProjectRoleName { get; set; }
        public string ProjectRoleDescription { get; set; }
    }

    public partial class UserView
    {
        public int Id { get; set; }
        public string OrganizationState { get; set; }
        public string Organization { get; set; }
        public int? OrganizationId { get; set; }
        public string County { get; set; }
        public string Domain { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public System.Guid? ChangePasswordRequest { get; set; }
    }

}
