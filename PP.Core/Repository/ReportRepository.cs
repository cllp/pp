using PP.Core.Helpers;
using PP.Core.Interfaces;
using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PP.Core.Context;
using PP.Core.Exceptions;

namespace PP.Core.Repository
{
    public class ReportRepository : RepositoryBase, IReportRepository
    {

        public DataTable GetReport(string key, out Report report)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                report = conn.Query<Report>("SELECT * FROM [Report] WHERE [Key] = @Key", new { Key = key }).FirstOrDefault();

                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("[ExecuteProcedure]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Key", SqlDbType.NVarChar);
                    cmd.Parameters["@Key"].Value = key;

                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@UserId"].Value = IdentityContext.Current.User.Id;
                    
                    DataTable table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    return table;
                }

                //return conn.Query<object>("Report01", p, commandType: CommandType.StoredProcedure);
            };
        }

        public IEnumerable<Report> GetReports()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new DynamicParameters();
                par.Add("@UserId", IdentityContext.Current.User.Id);
                return conn.Query<Report>("[GetReports]", par, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
