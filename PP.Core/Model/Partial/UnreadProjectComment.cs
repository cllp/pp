using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Model
{
    public class UnreadProjectComment
    {
        public int AreaId { get; set; }
        public string Area { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public int UnRead { get; set; }
    }
}
