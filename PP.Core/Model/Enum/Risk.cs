using System.ComponentModel;

namespace PP.Core.Model.Enum
{
    public enum Risk
    {
        /// <summary>
        /// Låg
        /// </summary>
        /// 
        [Description("Låg")]
        Idea = 1,
        /// <summary>
        /// Medel
        /// </summary>
        [Description("Medel")]
        Planning = 2,
        /// <summary>
        /// Hög
        /// </summary>
        [Description("Hög")]
        Implementation = 3,



    }
}