using PP.Core.Cache;
using PP.Core.Context;
using PP.Core.Interfaces;
using PP.Core.Model;

namespace PP.Core.Repository
{
    public class RepositoryBase
    {
        public ICache cache = CoreFactory.Cache;
        public ILog log = CoreFactory.Log;
        //public User CurrentUser = IdentityContext.Current.User;
    }
}
