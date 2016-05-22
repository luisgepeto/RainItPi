using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RainIt.Domain.Repository;

namespace RainIt.Business.Tests
{
    public class TestDbSetGenerator
    {
        public static TestRoleDbSet GetTestRoleDbSet()
        {
            return new TestRoleDbSet()
            {
                new Role()
                {
                    RoleId = 1,
                    Name = "customer",
                    Description = "customer"
                },
                new Role()
                {
                    RoleId = 2,
                    Name = "admin",
                    Description = "admin"
                }
            };
        }
        public static TestUserDbSet GetTestUserDbSet()
        {
            return new TestUserDbSet()
            {
                new User()
                {
                    UserId = 1,
                    Username = "UnitTestUser",
                    Email = "UnitTestUser@email.com",
                    UserInfo = GetTestUserInfoDbSet().FirstOrDefault(ui => ui.UserId==1),
                    Patterns = GetTestPatternDbSet().Where(p => p.UserId ==1).ToList(),
                    Routines = GetTestRoutineDbSet().Where(r => r.UserId == 1).ToList(),
                    Password = GetTestPasswordDbSet().SingleOrDefault(p=> p.UserId ==1),
                    Role = GetTestRoleDbSet().FirstOrDefault()
                },
                new User()
                {
                    UserId = 2,
                    Username = "OtherUser",
                    Email = "OtherUser@email.com",
                    UserInfo = GetTestUserInfoDbSet().FirstOrDefault(ui => ui.UserId==2),
                    Patterns = GetTestPatternDbSet().Where(p => p.UserId ==2).ToList(),
                    Routines = GetTestRoutineDbSet().Where(r => r.UserId == 2).ToList(),
                    Password = GetTestPasswordDbSet().SingleOrDefault(p=> p.UserId ==2),
                    Role = GetTestRoleDbSet().FirstOrDefault(r => r.Name=="admin")
                }
            };
        }

        public static TestPasswordDbSet GetTestPasswordDbSet()
        {
            return new TestPasswordDbSet()
            {
                new Password()
                {
                    Salt = "salt",
                    Hash = GetHashFrom("salt" + "password"),
                    UserId = 1
                },
                new Password()
                {
                    Salt = "peper",
                    Hash = GetHashFrom("pepper" + "password"),
                    UserId = 2
                }
            };
        }

        private static string GetHashFrom(string concatenatedPass)
        {
            HashAlgorithm hashAlg = new SHA256CryptoServiceProvider(); 
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(concatenatedPass);
            byte[] bytHash = hashAlg.ComputeHash(bytValue);
            string base64 = Convert.ToBase64String(bytHash);
            return base64;
        }

        public static TestUserInfoDbSet GetTestUserInfoDbSet()
        {
            return new TestUserInfoDbSet()
            {
                new UserInfo()
                {
                    BirthDate = new DateTime(1991, 07, 26),
                    FirstName = "UnitTestUserFirstName",
                    LastName = "UnitTestUserLastName",
                    Gender = "male",
                    UserId = 1
                },
                new UserInfo()
                {
                    BirthDate = new DateTime(1990, 06, 05),
                    FirstName = "OtherUserFirstName",
                    LastName = "OtherUserLastName",
                    Gender = "female",
                    UserId = 2
                }
            };
        }

        public static TestPatternDbSet GetTestPatternDbSet()
        {
            return new TestPatternDbSet()
            {
                new Pattern()
                {
                    PatternId = 1,
                    BytesFileSize = 1000,
                    Description = "some sample pattern",
                    FileType = "jpg",
                    Height = 600,
                    Width = 400,
                    Name = "samplepattern1",
                    Path = "samplepattern1path",
                    UserId = 1
                },
                new Pattern()
                {
                    PatternId = 2,
                    BytesFileSize = 2000,
                    Description = "some other pattern",
                    FileType = "png",
                    Height = 800,
                    Width = 1000,
                    Name = "samplepattern2",
                    Path = "samplepattern2path",
                    UserId = 1
                }
            };
        }

        public static TestRoutineDbSet GetTestRoutineDbSet()
        {
            return new TestRoutineDbSet()
            {
                new Routine()
                {
                    RoutineId = 1,
                    Description = "some sample routine",
                    Name = "sampleroutine1",
                    //Patterns = GetTestPatternDbSet().Where(p => p.UserId == 1).ToList(),
                    UserId = 1,
                }
            };
        }
    }
}
