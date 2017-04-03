using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PP.Api.Models
{
    public class UpdateModel
    {
        public int ProjectId { get; set; }

        public string SectionName { get; set; }

        public string FieldName { get; set; }

        public object FieldValue { get; set; }

    }
}