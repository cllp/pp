using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class GoalModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int Type { get; set; }
        public int Achieved { get; set; }
        public string GoalDefinition { get; set; }
        public string MesaureMethod { get; set; }
    }
}