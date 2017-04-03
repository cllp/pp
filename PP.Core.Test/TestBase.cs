using PP.Core.Context;
using PP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Tests
{
    public class TestBase
    {
        private static string username = "axel.olsson@stretch.se";
        //private static string username = "claes-philip.staiger@stretch.se";
        private static string password = "yammer";
        public static IProjectRepository projectRepository = CoreFactory.ProjectRepository;

        public TestBase()
        {
            InitUser();
        }

        private void InitUser()
        {
            //validate and create an test identity context
            var user = CoreFactory.AppRepository.Validate(username, password);
            IdentityContext.Initialize(new TestIdentity(user));
        }
    }
}
