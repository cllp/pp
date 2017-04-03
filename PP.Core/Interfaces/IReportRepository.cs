using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Interfaces
{
    public interface IReportRepository
    {
        //IEnumerable<object> GetReport(int id);
        DataTable GetReport(string key, out Report report);
        IEnumerable<Report> GetReports();

    }
}
