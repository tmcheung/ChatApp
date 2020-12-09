using FluentAssertions;
using Server.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class TestRepository
    {
        [Fact]
        public void Should_Return_Users()
        {
            //Arrange
            var users = new Dictionary<string, User>
            {
                { "", new User("test", "test") }
            };
            var repo = new Repository(users);

            //Act
            var u = repo.GetUsers();

            //Assert
            u.Should().HaveCount(1);
        }

        [Fact]
        public void Should_Return_Empty_UserDict()
        {
            //Arrange
            var repo = new Repository();

            //Act
            var u = repo.GetUsers();

            //Assert
            u.Should().HaveCount(0);
        }


        [Fact]
        public void Should_Return_Existing_User()
        {
            //Arrange
            var users = new Dictionary<string, User>
            {
                { "", new User("test", "testconn") }
            };
            var repo = new Repository(users);

            //Act
            var u = repo.GetUser("testconn");

            //Assert
            u.Should().NotBeNull();
        }

        [Fact]
        public void Should_Return_Null_When_No_User()
        {
            //Arrange
            var users = new Dictionary<string, User>
            {
                { "", new User("test", "testconn") }
            };
            var repo = new Repository(users);

            //Act
            var u = repo.GetUser("wrongconn");

            //Assert
            u.Should().BeNull();
        }
    }
}
