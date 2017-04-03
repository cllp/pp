using PP.Core.Helpers;
using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace PP.Api.Models
{
    public class ProjectModel
    {
        private List<MemberModel> _members = new List<MemberModel>();

        private List<GoalModel> _goals = new List<GoalModel>();
        private List<ActivityModel> _activityModel = new List<ActivityModel>();
        private List<FollowUpModel> _followUpList = new List<FollowUpModel>();
        private List<RiskModel> _risks = new List<RiskModel>();
        private List<ProjectPermissionModel> _projectPermission = new List<ProjectPermissionModel>();
        public int Id { get; set; }
        public string Name { get; set; }
        public int AreaId { get; set; }
        public int OrganizationId { get; set; }
        public int PhaseId { get; set; }
        public string Phase { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTime StartDate { get; set; }
        public string YammerGroup { get; set; }
        public string Description { get; set; }
        public int FundingEstimate { get; set; }
        public int FundingActual { get; set; }
        public int FundingStimulus { get; set; }
        public int FundingExternal { get; set; }
        public string IntroductionBackground { get; set; }
        public string IntroductionDefinition { get; set; }
        public string IntroductionComments { get; set; }
        public string DescriptionExtent { get; set; }
        public string DescriptionDelimitation { get; set; }
        public string DescriptionManagement { get; set; }
        public string DescriptionEvaluation { get; set; }
        public string ImplementationDescription { get; set; }
        public string CommunicationInterestAnalysis { get; set; }
        public string CommunicationPlan { get; set; }
        public string CommunicationDefinition { get; set; }
        public DateTime PublishDate { get; set; }
        public string Debriefing { get; set; }

        public List<MemberModel> Members
        {
            get { return _members; }
            set { _members = value; }
        }

        public List<RiskModel> Risks
        {
            get { return _risks; }
            set { _risks = value; }
        }

        public List<GoalModel> Goals
        {
            get { return _goals; }
            set { _goals = value; }
        }

        public List<FollowUpModel> FollowUp
        {
            get { return _followUpList; }
            set { _followUpList = value; }
        }

        public List<ActivityModel> Activity
        {
            get { return _activityModel; }
            set { _activityModel = value; }
        }

        public List<ProjectPermissionModel> ProjectPermissions
        {
            get { return _projectPermission; }
            set { _projectPermission = value; }
        }

        public Program Program { get; set; }

        public string Draft
        {
            get 
            {
                string name = Id.ToString() + ".0" + ".pdf";
                string fullpath = EnvironmentHelper.PdfDir + name;
                if (File.Exists(fullpath))
                    return "" + name;

                return string.Empty;
            } 
        }
    }
}