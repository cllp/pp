using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class ActivityModel
    {
        public int Id { get; set; }
        public int StatusId { get; set; }        
        public int InternalHours { get; set; }
        public int ExternalHours { get; set; }
        public int Cost { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int SummaryInternalHours { get; set; }
        public int SummaryExternalHours { get; set; }
        public int SummaryCost { get; set; }
        public int SummaryRow { get; set; }
        public int SummaryTotal { get; set; }
    }
}