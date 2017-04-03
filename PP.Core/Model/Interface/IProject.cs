using PP.Core.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model
{
    public interface IProject
    {
        int Id { get; set; }
        string Name { get; set; }
        ProjectArea ProjectArea { get; set; }
        Organization Organization { get; set; }
        int PhaseId { get; set; }
        int TypeId { get; set; }
        bool Favorite { get; set; }
        bool Member { get; set; }
        bool Active { get; set; }
        Phase Phase { get; }
        ProjectType ProjectType  { get; }
        IEnumerable<ProjectPermission> Permissions { get; set; }
    }
}
