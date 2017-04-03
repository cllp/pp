using PP.Core.Helpers;
using PP.Core.Model.Enum;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PP.Core.Model
{
    public class ProjectView : IProject
    {
        public bool HasPublishedVersion { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public ProjectArea ProjectArea { get; set; }
        public Program Program { get; set; }
        public Organization Organization { get; set; }
        public int PhaseId { get; set; }
        public int TypeId { get; set; }
        public bool Favorite { get; set; }
        public bool Member { get; set; }
        public bool Active { get; set; }       

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

        //[XmlIgnore]
        //public bool ShowInListDueToWritePermissionAndVersion 
        //{
        //    get
        //    {
        //        var hasWritePermission = Permissions.Where(p => p.Section.Equals(PermissionSection.Project)).FirstOrDefault().Permission == Permission.Write;

        //        if (!hasWritePermission && !HasPublishedVersion)
        //            return false;
        //        else
        //            return true;
        //    }
        //}
      
    }
}
