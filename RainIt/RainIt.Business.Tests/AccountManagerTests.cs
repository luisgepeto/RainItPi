using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;
using Address = RainIt.Domain.DTO.Address;
using User = RainIt.Domain.DTO.User;
using UserInfo = RainIt.Domain.DTO.UserInfo;

namespace RainIt.Business.Tests
{
    [TestClass]
    public class AccountManagerTests : ManagerTests
    {
        [TestMethod]
        public void IsUsernameAvailable_IsNotAvailable()
        {
            //Arrange
            var manager = new AccountManager(RainItContext);
            
            //Act
            const string username = "UnitTestUser";
            var isAvailable = manager.IsUsernameAvailable(username);

            //Assert
            Assert.IsFalse(isAvailable, "the username is not available");
        }

        [TestMethod]
        public void IsUsernameAvailable_IsAvailable()
        {
            //Arrange
            var manager = new AccountManager(RainItContext);
            
            //Act
            const string username = "SampleUser";
            var isAvailable = manager.IsUsernameAvailable(username);

            //Assert
            Assert.IsTrue(isAvailable, "the username is available");
        }

        [TestMethod]
        public void IsEmailAvailable_IsNotAvailable()
        {
            //Arrange
            var manager = new AccountManager(RainItContext);
            
            //Act
            const string email = "UnitTestUser@email.com";
            var isAvailable = manager.IsEmailAvailable(email);

            //Assert
            Assert.IsFalse(isAvailable, "the username is not available");
        }

        [TestMethod]
        public void IsEmailAvailable_IsAvailable()
        {
            //Arrange
            var manager = new AccountManager(RainItContext);
            
            //Act
            const string email = "UnitTestUser2@email.com";
            var isAvailable = manager.IsEmailAvailable(email);

            //Assert
            Assert.IsTrue(isAvailable, "the username is available");
        }

        [TestMethod]
        public void Authenticate_ValidCredentials()
        {
            //Arrange
            var manager = new AccountManager(RainItContext);

            //Act
            var login = new Login()
            {
                Username = "UnitTestUser",
                Password = "password"
            };
            var canLogin = manager.Authenticate(login);

            //Assert
            Assert.IsTrue(!canLogin.IsError, "was not able to login");
        }

        [TestMethod]
        public void Authenticate_InValidCredentials()
        {
            //Arrange
            var manager = new AccountManager(RainItContext);

            //Act
            var login = new Login()
            {
                Username = "UnitTestUser",
                Password = "otherpassword"
            };
            var canLogin = manager.Authenticate(login);

            //Assert
            Assert.IsFalse(!canLogin.IsError, "was able to login");
        }

        [TestMethod]
        public void Authenticate_InvalidUsername()
        {
            //Arrange
            var manager = new AccountManager(RainItContext);

            //Act
            var login = new Login()
            {
                Username = "sampleuser",
                Password = "password"
            };
            var canLogin = manager.Authenticate(login);

            //Assert
            Assert.IsFalse(!canLogin.IsError, "was able to login");
        }

        [TestMethod]
        public void Register_InvalidUsername()
        {
             //Arrange
            var manager = new AccountManager(RainItContext);

            //Act
            var registration = new Registration()
            {
                User = new User()
                {
                    Username = "UnitTestUser",
                    Email = "UnitTestUser2@email.com",
                    Password = "password",
                    PasswordConfirmation = "password"
                }
            };
            var canRegister = manager.Register(registration);

            //Assert
            Assert.IsFalse(!canRegister.IsError, " the user should not have been registered");
        }

        [TestMethod]
        public void Register_InvalidEmail()
        {
             //Arrange
            var manager = new AccountManager(RainItContext);

            //Act
            var registration = new Registration()
            {
                User = new User()
                {
                    Username = "UnitTestUser2",
                    Email = "UnitTestUser@email.com",
                    Password = "password",
                    PasswordConfirmation = "password"
                }
            };
            var canRegister = manager.Register(registration);

            //Assert
            Assert.IsFalse(!canRegister.IsError, " the user should not have been registered");
        }

        [TestMethod]
        public void Register_NonMatchingPasswords()
        {
             //Arrange
            var manager = new AccountManager(RainItContext);

            //Act
            var registration = new Registration()
            {
                User = new User()
                {
                    Username = "UnitTestUser2",
                    Email = "UnitTestUser2@email.com",
                    Password = "password",
                    PasswordConfirmation = "password2"
                }
            };
            var canRegister = manager.Register(registration);

            //Assert
            Assert.IsFalse(!canRegister.IsError, " the user should not have been registered");
        }

        [TestMethod]
        public void Register_ValidRegistration()
        {
             //Arrange
            var manager = new AccountManager(RainItContext);

            //Act
            var registration = new Registration()
            {
                User = new User()
                {
                    Username = "UnitTestUser2",
                    Email = "UnitTestUser2@email.com",
                    Password = "password",
                    PasswordConfirmation = "password"
                },
                Address = new Address(),
                UserInfo = new UserInfo()
            };
            var canRegister = manager.Register(registration);
            var user = RainItContext.UserSet.SingleOrDefault(u => u.Username == "UnitTestUser2");

            //Assert
            Assert.IsNotNull(user, "the user should have been registered");
            Assert.AreEqual("UnitTestUser2", user.Username, "an incorrect user was retrieved");
            Assert.IsTrue(!canRegister.IsError, " the user should have been registered");
        }

        [TestMethod]
        public void GetRolesfor_ExistingUsername_MultipleRoles()
        {
             //Arrange
            var manager = new AccountManager(RainItContext);

            //Act
            const string username = "UnitTestUser";
            var roles = manager.GetRolesFor(username);

            //Assert
            Assert.IsNotNull(roles, "roles should not be null");
            Assert.AreEqual(2, roles.Count, "this user should have two roles");
            Assert.AreEqual("admin", roles.LastOrDefault(), "this customer should have the admin role");
            Assert.AreEqual("customer", roles.FirstOrDefault(), "this customer should have the customer role");
        }

         [TestMethod]
        public void GetRolesfor_ExistingUsername_SingleRole()
        {
             //Arrange
            var manager = new AccountManager(RainItContext);

            //Act
            const string username = "OtherUser";
            var roles = manager.GetRolesFor(username);

            //Assert
            Assert.IsNotNull(roles, "roles should not be null");
            Assert.AreEqual(1, roles.Count, "this user should have one role");
            Assert.AreEqual("admin", roles.FirstOrDefault(), "this customer should have the admin role");
        }

        [TestMethod]
        public void GetRolesfor_InvalidUsername()
        {
             //Arrange
            var manager = new AccountManager(RainItContext);

            //Act
            const string username = "SampleUser";
            List<string> roles = null;
            var canGetRoles = false;
            try
            {
                roles = manager.GetRolesFor(username);
                canGetRoles = true;
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex, " an exception should have been generated");
                Assert.IsNull(roles, "roles should be null");
            }
            Assert.IsFalse(canGetRoles, "roles should not have been retrieved");
        }
        

    }
}
