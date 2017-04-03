using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using PP.Core.Helpers;
using PP.Core.Model.Enum;

namespace PP.Core.Model
{
    public partial class Project : IProject
    {
        private DateTime _startDate = DateTimeHelper.GetMinDateTime();

        private List<ProjectMemberView> projectMembers = new List<ProjectMemberView>();

        private List<ProjectGoal> projectGoals = new List<ProjectGoal>();

        private List<ProjectActivity> projectActivities = new List<ProjectActivity>();

        private List<ProjectFollowUp> projectFollowUps = new List<ProjectFollowUp>();

        private List<ProjectRisk> projectRisks = new List<ProjectRisk>();

        private List<ErrorCode> errors = new List<ErrorCode>();

        public string YammerGroup { get; set; }

        [XmlIgnore]
        public DateTime PublishedDate { get; set; }
        
        public UserView User { get; set; }

        public Program Program { get; set; }
        
        public List<ProjectMemberView> ProjectMembers
        {
            get { return projectMembers; }
            set { projectMembers = value; }
        }

        [XmlIgnore]
        public User CreatedBy { get; set; }

        [XmlIgnore]
        public decimal TotalCost
        {
            get
            {
                decimal costs = 0;
                if (this.projectActivities != null && this.projectActivities.Any())
                {
                    costs = projectActivities.Aggregate(costs, (current, a) => current + a.Cost);
                }

                return costs;
            }
        }

        [XmlIgnore]
        public int TotalInternalHours
        {
            get
            {
                int h = 0;
                if (this.projectActivities != null && this.projectActivities.Any())
                {
                    foreach (var a in projectActivities)
                    {
                        h += a.InternalHours;
                    }
                }

                return h;
            }
        }

        [XmlIgnore]
        public int TotalExternalHours
        {
            get
            {
                int h = 0;
                if (this.projectActivities != null && this.projectActivities.Any())
                {
                    foreach (var a in projectActivities)
                    {
                        h += a.ExternalHours;
                    }
                }

                return h;
            }
        }


        [XmlIgnore]
        public Organization Organization { get; set; }

        public ProjectArea ProjectArea { get; set; }


        public List<ProjectGoal> ProjectGoals
        {
            get { return projectGoals; }
            set { projectGoals = value; }
        }

        public List<ProjectActivity> ProjectActivities
        {
            get { return projectActivities; }
            set { projectActivities = value; }
        }


        public List<ProjectFollowUp> ProjectFollowUps
        {
            get { return projectFollowUps; }
            set { projectFollowUps = value; }
        }

        public List<ProjectRisk> ProjectRisks
        {
            get { return projectRisks; }
            set { projectRisks = value; }
        }

        [XmlIgnore]
        public List<ErrorCode> Errors
        {
            get { return errors; }
            set { errors = value; }
        }

        [XmlIgnore]
        public Phase Phase
        {
            get
            {
                return this.PhaseId.ToEnum<int, Phase>();
            }
        }

        [XmlIgnore]
        public ProjectType ProjectType
        {
            get
            {
                return this.TypeId.ToEnum<int, ProjectType>();
            }
        }

        [XmlIgnore]
        public IEnumerable<ProjectPermission> Permissions { get; set; }

        public bool Favorite { get; set; }
        public bool Member { get; set; }
        public int ProgramOwnerId { get; set; }
        public int ProjectCoordinatorId { get; set; }
    }
}
