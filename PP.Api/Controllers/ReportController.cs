using PP.Api.Helpers;
using PP.Api.Models;
using PP.Core;
using PP.Core.Interfaces;
using PP.Core.Model;
using PP.Core.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PP.Api.Controllers
{
    [RoutePrefix("report")]
    public class ReportController : BaseController
    {
        public IReportRepository _repository = CoreFactory.ReportRepository;

        [HttpGet]
        [Route("get/{key}")]
        [AllowAnonymous]
        public ReportModel GetReport(string key)
        {
            Report report = new Report();
            var reportData = reportRepository.GetReport(key, out report);
           
            var fileName = ExcelGenerator.GenerateExcel(reportData, key);
            
            return new ReportModel()
            { 
                Key = key,
                Name = report.Name,
                Id = report.Id,
                Description = report.Description,
                Procedure = report.Procedure,
                FileName = fileName,
                Type = "Excel"
            };
        }

        [HttpGet]
        [Route("get")]
        [AllowAnonymous]
        public IEnumerable<Report> GetReports()
        {
            return reportRepository.GetReports();
        }

        //private static string api_password = "1921X";
        //private const string NotAuthorizedMessage = "You are not authorized to view reports with given email '{0}' and password '{1}'.";

        //// // GET api/report
        ////// public ProjectOverview Get(string email, string password)
        //// public HttpResponseMessage Get(string email, string password)
        //// {
        ////     //return new List<ProjectOverviewReport>();
        ////     var report = _repository.GetProjectOverviewReport(email);

        ////     //forcing to send back response in Xml format
        ////     HttpResponseMessage resp = Request.CreateResponse<ProjectOverview>(HttpStatusCode.OK, value: report,
        ////         formatter: Configuration.Formatters.XmlFormatter);

        ////     return resp;
        //// }

        //// GET api/report
        //public ProjectOverviewReport GetOverviewReport(string email, string password)
        //{
        //    if (!password.Equals(api_password))
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, String.Format(NotAuthorizedMessage, email, password)));

        //    return _repository.GetProjectOverviewReport(email);
        //}

        //// GET api/report
        //public ProjectStatusReport GetStatusReport(string email, string password)
        //{
        //    if (!password.Equals(api_password))
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, String.Format(NotAuthorizedMessage, email, password)));

        //    return _repository.GetProjectStatusReport(email);
        //}

        //// GET api/report
        //public ProjectFinanceReport GetFinanceReport(string email, string password)
        //{
        //    if (!password.Equals(api_password))
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, String.Format(NotAuthorizedMessage, email, password)));

        //    return _repository.GetProjectFinanceReport(email);
        //}

        //// GET api/report
        //public ProjectMemberReport GetMemberReport(string email, string password)
        //{
        //    if (!password.Equals(api_password))
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, String.Format(NotAuthorizedMessage, email, password)));

        //    return _repository.GetProjectMemberReport(email);
        //}

        //// GET api/report
        //public ProjectReport GetProjectReport(string email, string password)
        //{
        //    if (!password.Equals(api_password))
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, String.Format(NotAuthorizedMessage, email, password)));

        //    return _repository.GetProjectReport(email);
        //}

    }
}
