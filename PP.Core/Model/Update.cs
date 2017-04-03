using System;
using PP.Core.Model.Enum;

namespace PP.Core.Model
{
    public class Update
    {
        public int ProjectId { get; set; }

        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public object Value { get; set; }

    }
}