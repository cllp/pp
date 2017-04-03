using System.ComponentModel;

namespace PP.Core.Model.Enum
{
    public enum UpdateSection
    {
        /// <summary>
        /// Project
        /// </summary>
        [Description("Project")]
        Project = 1,
        /// <summary>
        /// Member
        /// </summary>
        [Description("ProjectMember")]
        Member = 2,
        /// <summary>
        /// Risk
        /// </summary>
        [Description("ProjectRisk")]
        Risk = 3,
        /// <summary>
        /// Aktivitet
        /// </summary>
        [Description("ProjectActivity")]
        Activity = 4,
        /// <summary>
        /// Uppföljning
        /// </summary>
        [Description("ProjectFollowup")]
        Followup = 5
    }
}