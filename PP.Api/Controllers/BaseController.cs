using PP.Api.Filters;
using PP.Core;
using PP.Core.Helpers;
using PP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PP.Api.Controllers
{
    [ApiExceptionFilter]
    //[EnableCors(origins: "http://myclient.azurewebsites.net", headers: "*", methods: "*")]
    //[EnableCors(origins: Constants.APICrossDomainOrigins, headers: "*", methods: "*")]
    public class BaseController : ApiController
    {
        public IAppRepository appRepository = CoreFactory.AppRepository;
        public IProjectRepository projectRepository = CoreFactory.ProjectRepository;
        public ICommentRepository commentRepository = CoreFactory.CommentRepository;
        public IReportRepository reportRepository = CoreFactory.ReportRepository;
        public ILog log = CoreFactory.Log;
    }
}