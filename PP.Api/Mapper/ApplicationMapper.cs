using PP.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PP.Core;
using PP.Core.Interfaces;
using PP.Core.Model;
using PP.Core.Helpers;
using PP.Core.Model.Enum;
using UpdateSection = PP.Core.Model.Enum.UpdateSection;
using PP.Core.Context;
using PP.Api.Helpers;

namespace PP.Api.Mapper
{
    public static class ApplicationMapper
    {

        public static Project MapCreateProjectContent(CreateProjectModel createProject)
        {

            Project project = new Project();
            project.Name = createProject.Name;
            project.OrganizationId = createProject.OrganizationId;
            project.ProjectAreaId = createProject.AreaId;
            project.ProgramOwnerId = createProject.ProgramOwner;
            project.ProjectCoordinatorId = createProject.ProjectCoordinator;
            project.CreatedById = createProject.CreatedById;
            project.Description = createProject.Description;
            project.StartDate = createProject.StartDate.GetMyUtcTime();
            return project;
        }

        public static IEnumerable<UserViewModel> MapUserViewList(IEnumerable<UserView> userList)
        {
            List<UserViewModel> userViewList = new List<UserViewModel>();
            foreach (var userView in userList)
            {
                var uiModel = MapUserView(userView);
                userViewList.Add(uiModel);
            }
            return userViewList;
        }

        public static UserViewModel MapUserView(UserView userView)
        {
            UserViewModel userViewModel = new UserViewModel();


            if (string.IsNullOrEmpty(userView.Name))
                userView.Name = "";

            userViewModel.Id = userView.Id;
            userViewModel.Name = string.IsNullOrEmpty(userView.Name.Trim()) ? userView.Email.Trim() : userView.Name.Trim();
            userViewModel.Organization = string.IsNullOrEmpty(userView.Organization) ? "Extern" : userView.Organization;
            userViewModel.OrganizationId = userView.OrganizationId;
            userViewModel.RoleId = userView.RoleId;
            userViewModel.County = userView.County;
            userViewModel.Domain = userView.Domain;
            userViewModel.Email = string.IsNullOrEmpty(userView.Email.Trim()) ? "Epost saknas" : userView.Email.Trim();
            userViewModel.OrganizationState = userView.OrganizationState;
            return ObjectTextTrim.TrimStringProperties(userViewModel);
        }

        public static User MapUserUpdate(UserViewModel userView)
        {
            User user = new User();
            user.Id = userView.Id;
            user.Name = userView.Name;
            user.RoleId = userView.RoleId;
            user.Email = userView.Email;
            return user;
        }


        public static IEnumerable<ProjectViewModel> MapProjectContent(IEnumerable<ProjectView> list)
        {
            var uiModelList = new List<ProjectViewModel>();

            foreach (var p in list)
            {
                var uiModel = MapProjectViewContent(p);

                uiModelList.Add(uiModel);
            }
            return uiModelList.AsEnumerable();
        }

        public static ProjectViewModel MapProjectViewContent(Core.Model.ProjectView p)
        {
            ProjectViewModel uiModel = new ProjectViewModel();

            uiModel.Id = p.Id;
            uiModel.Name = p.Name;
            uiModel.Phase = EnumHelper.GetEnumDescription((Phase)p.PhaseId);
            uiModel.Area = p.ProjectArea.Name;
            uiModel.Favorite = p.Favorite;
            uiModel.Member = p.Member;
            uiModel.Municipality = p.Organization.Name;
            uiModel.County = p.Organization.County;
            uiModel.Active = p.Active;
            return ObjectTextTrim.TrimStringProperties(uiModel);
        }

        public static Core.Model.Update MapProjectUpdate(UpdateModel uiUpdateModel)
        {
            var update = new Core.Model.Update();
            update.ProjectId = uiUpdateModel.ProjectId;
            update.TableName = GetTableNameFromSection(uiUpdateModel.SectionName);
            update.ColumnName = uiUpdateModel.FieldName;
            update.Value = uiUpdateModel.FieldValue;
            return update;
        }


        public static Core.Model.ProjectVersion MapProjectVersion(ProjectVersionModel projectVersionModel)
        {
            ProjectVersion projectVersion = new Core.Model.ProjectVersion();
            projectVersion.Comment = projectVersionModel.Comment;
            projectVersion.PublishedBy = projectVersionModel.PublishedBy.Value;
            projectVersion.PublishedDate = projectVersionModel.PublishedDate.Value.GetMyUtcTime();
            projectVersion.ProjectId = projectVersionModel.ProjectId;
            projectVersion.PhaseId = projectVersionModel.PhaseId;

            return projectVersion;
        }

        public static IEnumerable<SelectOption> MapPhaseSelectOptions()
        {

            IEnumerable<Phase> phaseIds = Enum.GetValues(typeof(Phase)).Cast<Phase>();
            List<SelectOption> phases = new List<SelectOption>();

            foreach (int value in phaseIds)
            {
                SelectOption option = new SelectOption();
                option.Name = EnumHelper.GetEnumDescription((Phase)value);
                option.Id = value;
                phases.Add(option);
            }

            return phases;
        }

        public static IEnumerable<ProjectVersionModel> MapProjectVersionContent(IEnumerable<Core.Model.ProjectVersion> versions)
        {
            List<ProjectVersionModel> uiVersions = new List<ProjectVersionModel>();
            foreach (ProjectVersion version in versions)
            {
                uiVersions.Add(MapProjectVersion(version));
            }
            return uiVersions;
        }

        public static ProjectVersionModel MapProjectVersion(ProjectVersion version)
        {
            var uiModel = new ProjectVersionModel();

            uiModel.ProjectId = version.ProjectId;
            uiModel.Id = version.Id;
            uiModel.Comment = version.Comment;
            uiModel.PublishedDate = version.PublishedDate.GetMyLocalTime();
            uiModel.PublishedBy = version.PublishedBy;
            uiModel.PublishedByName = version.PublishedByName;
            uiModel.Phase = EnumHelper.GetEnumDescription((Phase)version.PhaseId);

            ISerializer xmlSerializer = new SerializeXml();
            Project project = xmlSerializer.Deserialize<Project>(version.ProjectData);
            ProjectModel projectcontent = null;
            //Mapp to UI model 
            if (project != null)
            {
                projectcontent = ApplicationMapper.MapProjectContent(project);
            }
            uiModel.Data = projectcontent;
            uiModel.PublishedByName = version.PublishedByName;

            return ObjectTextTrim.TrimStringProperties(uiModel);
        }

        public static ProjectModel MapProjectContent(Core.Model.Project p)
        {
            var uiModel = new ProjectModel();
            uiModel.Id = p.Id;
            uiModel.Name = p.Name;
            uiModel.AreaId = p.ProjectAreaId;
            uiModel.OrganizationId = p.OrganizationId;
            uiModel.PhaseId = p.PhaseId;
            uiModel.Phase = EnumHelper.GetEnumDescription((Phase)p.PhaseId);
            uiModel.Program = p.Program;
            uiModel.YammerGroup = p.YammerGroup;

            if (p.StartDate.HasValue)
                uiModel.StartDate = p.StartDate.Value.GetMyLocalTime();

            uiModel.CreatedById = p.CreatedById;
            uiModel.CreatedBy = p.User.Name;
            uiModel.Description = p.Description;
            uiModel.FundingEstimate = p.FundingEstimate;
            uiModel.FundingActual = p.FundingActual;
            uiModel.FundingExternal = p.FundingExternal;
            uiModel.FundingStimulus = p.FundingStimulus;
            uiModel.IntroductionBackground = p.PlanIntroductionBackground;
            uiModel.IntroductionDefinition = p.PlanIntroductionDefinition;
            uiModel.IntroductionComments = p.PlanIntroductionComments;
            uiModel.DescriptionExtent = p.PlanDescriptionExtent;
            uiModel.DescriptionDelimitation = p.PlanDescriptionDelimitation;
            uiModel.DescriptionManagement = p.PlanDescriptionManagement;
            uiModel.DescriptionEvaluation = p.PlanDescriptionEvaluation;
            uiModel.ImplementationDescription = p.PlanImplementationDescription;
            uiModel.CommunicationInterestAnalysis = p.PlanCommunicationInterestAnalysis;
            uiModel.CommunicationDefinition = p.PlanCommunicationDefinition;
            uiModel.CommunicationPlan = p.PlanCommunicationPlan;
            uiModel.PublishDate = p.PublishedDate.GetMyLocalTime();
            uiModel.Debriefing = p.Debriefing;

            uiModel.Members = new List<MemberModel>();
            foreach (ProjectMemberView member in p.ProjectMembers)
            {
                MemberModel memberModel = new MemberModel();
                memberModel.Id = member.ProjectMemberId;
                memberModel.UserId = member.UserId;
                memberModel.ProjectId = p.Id;
                memberModel.Name = member.UserName;
                memberModel.Email = member.UserEmail;
                memberModel.Municipality = member.Organization;
                memberModel.isSaved = true;
                memberModel.MemberRoles = new List<RoleModel>();
                foreach (ProjectRoleView roleView in member.ProjectRoles)
                {
                    memberModel.MemberRoles.Add(new RoleModel { Id = roleView.ProjectRoleId, Name = roleView.ProjectRoleName, ProjectRoleGroupName = roleView.ProjectRoleGroupName });
                }
                memberModel = ObjectTextTrim.TrimStringProperties(memberModel);
                uiModel.Members.Add(memberModel);
            }

            uiModel.Risks = (from risk in p.ProjectRisks
                             select new RiskModel
                             {
                                 ProjectId = risk.ProjectId,
                                 Id = risk.Id,
                                 Name = risk.Name,
                                 ConsequenceId = risk.ConsequenceId,
                                 ProbabilityId = risk.ProbabilityId,
                                 Effect = risk.Effect
                             }).ToList();

            uiModel.Goals = (from goal in p.ProjectGoals
                             select new GoalModel
                             {
                                 ProjectId = goal.ProjectId,
                                 Id = goal.Id,
                                 Type = goal.Type,
                                 Achieved = goal.Achieved,
                                 MesaureMethod = goal.MesaureMethod,
                                 GoalDefinition = goal.GoalDefinition
                             }).ToList();

            uiModel.Activity = (from activity in p.ProjectActivities
                                select new ActivityModel
                                {
                                    ProjectId = activity.ProjectId,
                                    Id = activity.Id,
                                    Name = activity.Name,
                                    StatusId = activity.Status,
                                    Notes = activity.Notes,
                                    InternalHours = activity.InternalHours,
                                    ExternalHours = activity.ExternalHours,
                                    Cost = activity.Cost,
                                    SummaryRow = 0,
                                    SummaryCost = 0,
                                    SummaryInternalHours = 0,
                                    SummaryExternalHours = 0,
                                    SummaryTotal = 0,

                                }).ToList();

            if (p.Permissions != null)
            {
                uiModel.ProjectPermissions = (from permission in p.Permissions
                                              select new ProjectPermissionModel
                                              {
                                                  Permission = permission.Permission,
                                                  Section = Enum.GetName(typeof(PermissionSection), (int)permission.Section)
                                              }).ToList();
            }
            uiModel.FollowUp = (from followUp in p.ProjectFollowUps
                                select new FollowUpModel
                                {
                                    ProjectId = followUp.ProjectId,
                                    Id = followUp.Id,
                                    Date = followUp.Date.HasValue ? followUp.Date.Value.GetMyLocalTime() : DateTimeHelper.GetMinDateTime(),
                                    Notes = followUp.Notes,
                                    InternalHours = followUp.InternalHours,
                                    ExternalHours = followUp.ExternalHours,
                                    OtherCosts = followUp.OtherCosts,
                                    ActivityTotal = followUp.ActivityTotal,
                                    Canceled = followUp.Canceled,
                                    IsOpen = false,
                                    isSaved = true,
                                    RowTotalCost = 0
                                }).ToList();


            return ObjectTextTrim.TrimStringProperties(uiModel);
        }

        public static Core.Model.ProjectMember MapProjectMember(MemberModel memberModel)
        {
            if (memberModel.ProjectRoleIds == null)
            {
                memberModel.ProjectRoleIds = new List<int>();

                foreach (var role in memberModel.MemberRoles)
                    memberModel.ProjectRoleIds.Add(role.Id);
            }

            var member = new Core.Model.ProjectMember
            {
                Id = memberModel.Id,
                ProjectId = memberModel.ProjectId,
                UserId = memberModel.UserId,
                Email = memberModel.Email,
                Name = memberModel.Name,
                ProjectRoleIds = memberModel.ProjectRoleIds
            };
            return member;
        }

        public static List<ProjectMemberRoleResultsModel> MapProjectMemberResults(IEnumerable<UpdateProjectRoleResult> results)
        {
            List<ProjectMemberRoleResultsModel> models = new List<ProjectMemberRoleResultsModel>();

            foreach (var item in results)
            {
                var model = new ProjectMemberRoleResultsModel();
                model.Status = item.Status;
                model.ProjectMemberRole = new RoleModel()
                {
                    Id = item.Id,
                    Name = item.Name
                    //ProjectRoleGroupName = item.
                };

                models.Add(model);
            }

            return models;
        }

        public static List<MemberModel> MapProjectMemberList(List<ProjectMember> memberList)
        {
            return memberList.Select(MapProjectMember).ToList();
        }

        public static MemberModel MapProjectMember(ProjectMember member)
        {
            var memberModel = new MemberModel
            {
                Id = member.Id,
                ProjectId = member.ProjectId,
                UserId = member.UserId,
                Email = member.Email,
                Name = member.Name,
                ProjectRoleIds = member.ProjectRoleIds
            };
            return ObjectTextTrim.TrimStringProperties(memberModel);
        }

        public static ProjectRisk MapProjectRisk(RiskModel riskModel)
        {

            var risk = new ProjectRisk
            {
                Id = riskModel.Id,
                ProjectId = riskModel.ProjectId,
                Name = riskModel.Name,
                ProbabilityId = riskModel.ProbabilityId,
                ConsequenceId = riskModel.ConsequenceId
            };
            //risk.Effect = riskModel.Effect;
            return risk;

        }

        public static ProjectGoal MapProjectGoal(GoalModel goalModel)
        {
            var goal = new ProjectGoal
            {
                Id = goalModel.Id,
                ProjectId = goalModel.ProjectId,
                GoalDefinition = goalModel.GoalDefinition,
                MesaureMethod = goalModel.MesaureMethod,
                Achieved = goalModel.Achieved,
                Type = goalModel.Type
            };
            return goal;
        }

        public static ProjectActivity MapProjectActivity(ActivityModel activityModel)
        {
            var activity = new ProjectActivity
            {
                Id = activityModel.Id,
                ProjectId = activityModel.ProjectId,
                Name = activityModel.Name,
                Notes = activityModel.Notes,
                Status = activityModel.StatusId,
                InternalHours = activityModel.InternalHours,
                ExternalHours = activityModel.ExternalHours,
                Cost = activityModel.Cost
            };
            return activity;
        }

        public static ProjectFollowUp MapProjectFollowUp(FollowUpModel followUpModel)
        {
            var followup = new ProjectFollowUp
            {
                ProjectId = followUpModel.ProjectId,
                Id = followUpModel.Id,
                Date = followUpModel.Date.GetMyLocalTime(),
                InternalHours = followUpModel.InternalHours,
                ExternalHours = followUpModel.ExternalHours,
                OtherCosts = followUpModel.OtherCosts,
                ActivityTotal = followUpModel.ActivityTotal,
                Notes = followUpModel.Notes,
                Canceled = followUpModel.Canceled
            };
            return followup;
        }

        private static string GetTableNameFromSection(string sectionName)
        {
            string tableName;
            try
            {
                var updateSection = (UpdateSection)Enum.Parse(typeof(UpdateSection), sectionName);
                tableName = EnumHelper.GetEnumDescription(updateSection);
            }
            catch (Exception ex)
            {
                ILog log = CoreFactory.Log;
                log.Error("Faild to to translate section name to table", ex);
                throw;
            }
            return tableName;
        }

        public static MemberModel MapProjectMemberView(ProjectMemberView pmv)
        {
            var memberModel = new MemberModel
            {
                Id = pmv.ProjectMemberId,
                UserId = pmv.UserId,
                Email = pmv.UserEmail,
                Municipality = pmv.County,
                Name = pmv.UserName,
                ProjectId = pmv.ProjectId,
                Domain = pmv.Domain,
                Organization = pmv.Organization
            };
            memberModel.MemberRoles = new List<RoleModel>();
            foreach (ProjectRoleView roleView in pmv.ProjectRoles)
            {
                memberModel.MemberRoles.Add(MapProjectRoleView(roleView));
            }
            return ObjectTextTrim.TrimStringProperties(memberModel);

        }

        public static RoleModel MapProjectRoleView(ProjectRoleView role)
        {
            var roleModel = new RoleModel
            {
                Id = role.ProjectRoleId,
                Name = role.ProjectRoleName,
            };
            return ObjectTextTrim.TrimStringProperties(roleModel);
        }

        public static IEnumerable<RoleModel> MapProjectRoles(IEnumerable<ProjectRole> projectRoles)
        {
            return projectRoles.Select(projectRole => new RoleModel { Id = projectRole.Id, Name = projectRole.Name }).ToList();
        }

        public static IEnumerable<AdminUserModel> MapAdminUserList(IEnumerable<UserView> adminList)
        {
            var adminUserList = new List<AdminUserModel>();
            foreach (UserView userView in adminList)
            {
                var adminUser = new AdminUserModel { Id = userView.Id, Name = userView.Name };
                adminUser = ObjectTextTrim.TrimStringProperties(adminUser);
                adminUserList.Add(adminUser);
            }
            return adminUserList;
        }

    }
}