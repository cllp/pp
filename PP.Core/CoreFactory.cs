using PP.Core.Cache;
using PP.Core.Interfaces;
using PP.Core.Log;
using PP.Core.Repository;

namespace PP.Core
{
    public static class CoreFactory
    {
        public static IAppRepository AppRepository 
        { 
            get
            {
                //return new AppRepositoryMock();
                return new AppRepository();
            }
        }

        public static IReportRepository ReportRepository
        {
            get
            {
                //return new AppRepositoryMock();
                return new ReportRepository();
            }
        }  
        

        public static IProjectRepository ProjectRepository
        {
            get
            {
                return new ProjectRepository();
                // return new ContentRepository();
            }
        }

        public static ICommentRepository CommentRepository
        {
            get
            {
                return new CommentRepository();
            }
        } 

        /// <summary>
        /// internal because caching is not to be used outside core
        /// </summary>
        internal static ICache Cache
        {
            get
            {
                return new InMemoryCache();
            }
        }

        public static ILog Log
        {
            get
            {
                return new DatabaseLog();
                //return new FileLog();
            }
        }
    }
}
