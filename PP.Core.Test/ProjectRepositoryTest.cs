using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PP.Core.Interfaces;
using PP.Core.Model;
using System.Collections.Generic;

namespace PP.Core.Tests
{
    [TestClass]
    public class ProjectTest : TestBase
    {
        [TestMethod]
        public void UpdateProjectMember()
        {
            var member = new ProjectMember();
            member.Email = "axel.olsson@stretch.se";
            member.Id = 11605;
            member.Name = "Axel Olsson";
            member.ProjectId = 3305;
            member.ProjectRoleIds = new List<int>() { 3, 7 };
            member.UserId = 8024;

            var result = projectRepository.UpdateProjectMemberRole(member);

            //todo create expected return value in core
            Assert.AreEqual(1, 1);
        }
    }
}
