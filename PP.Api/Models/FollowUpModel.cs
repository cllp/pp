using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class FollowUpModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public DateTime Date { get; set; }
        public int InternalHours { get; set; }
        public int ExternalHours { get; set; }
        public int OtherCosts { get; set; }
        public string Notes { get; set; }
        public bool IsOpen { get; set; }
        public int RowTotalCost { get; set; }
        public int? ActivityTotal { get; set; }
        public bool isSaved { get; set; }
        public bool Canceled { get; set; }
    }
}