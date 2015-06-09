using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RainIt.Interfaces.Repository;

namespace RainIt.Business.Tests
{
    [TestClass]
    public class ManagerTests
    {
        protected IRainItContext RainItContext;
        protected IAzureCloudContext AzureCloudContext;

         [TestInitialize]
        public void Setup()
        {
            RainItContext = new TestRainItContext()
            {
                UserSet = TestDbSetGenerator.GetTestUserDbSet(),
                UserInfoSet = TestDbSetGenerator.GetTestUserInfoDbSet(),
                PatternSet = TestDbSetGenerator.GetTestPatternDbSet(),
                RoutineSet = TestDbSetGenerator.GetTestRoutineDbSet(),
                PasswordSet = TestDbSetGenerator.GetTestPasswordDbSet(),
                RoleSet = TestDbSetGenerator.GetTestRoleDbSet()
            };
            AzureCloudContext = new TestAzureCloudContext();

            ConfigurationManager.AppSettings["MaxPatternPixelHeight"] = "200";
            ConfigurationManager.AppSettings["MaxPatternPixelWidth"] = "200";
            ConfigurationManager.AppSettings["MaxPatternByteCount"] = "1000";
        }
    }
}
