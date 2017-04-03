using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class RiskModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ConsequenceId { get; set; }
        public int ProbabilityId { get; set; }
        public string Name { get; set; }
        public int Effect { get; set; }
    }
}