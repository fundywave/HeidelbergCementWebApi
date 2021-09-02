using HeidelbergCement.Data.Models;
using HeidelbergCement.Service.Interface;
using HeidelbergCement.Service.Service;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace HeidelbergCement.Tests.Service
{
    [TestFixture]
    public class UserServiceTests
    {
        Mock<IUserRepository<User>> _userRepositoryMock;
        IUserService<User> _userService;
        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository<User>>();
            _userService = new UserService(_userRepositoryMock.Object);
        }
        [TearDown]
        public void TearDown()
        {
            _userRepositoryMock.Reset();
        }
        [Test()]
        public void Should_Get_Users()
        {
            //arrange
            var users = new List<User>() {
                new User { ID = 1, Name = "user1", Password = "hello" },
                new User { ID = 2, Name = "user2", Password = "125" }
            };
                
            _userRepositoryMock.Setup(x => x.GetUsersAsync()).ReturnsAsync(users);

            //act
            var expectedUsers = _userService.GetUsers();

            //assert
            Assert.IsNotNull(expectedUsers);
            Assert.AreEqual(2, (expectedUsers.Result as List<User>).Count);
            _userRepositoryMock.Verify(x => x.GetUsersAsync(),Times.Once);

        }
    }
}
