using System.ComponentModel;
namespace PP.Core.Model.Enum
{
    public enum Phase
    {
        /// <summary>
        /// Idé
        /// </summary>
        /// 
        [Description("Idé")]
        Idea = 1,
        /// <summary>
        /// Planering
        /// </summary>
         [Description("Planering")]
        Planning = 2,
        /// <summary>
        /// Genomförande
        /// </summary>
        [Description("Genomförande")]
        Implementation = 3,
        /// <summary>
        /// Uppföljning
        /// </summary>
        [Description("Uppföljning")]
        Monitoring = 4,

          /// <summary>
        /// Arkiverad
        /// </summary>
        [Description("Arkiverad")]
        Archived = 5
    }
}