using Microsoft.VisualStudio.TestTools.UnitTesting;
using PP.APi.Tests;
using PP.Core.Context;

namespace PP.Api.Tests
{
    [TestClass]
    public class TestBase
    {

      

        public TestBase()
        {
            IdentityContext.Initialize(new Identity());
        }
    }
}
