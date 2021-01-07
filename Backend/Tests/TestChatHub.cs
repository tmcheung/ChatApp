using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Server.Data;
using Server.Hubs;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.TestClasses;
using Xunit;

namespace Tests
{
    public class TestChatHub
    {
        [Fact]
        public void Users_Should_Be_Notified_When_User_Joins()
        {
            //Arrange
            var repo = new Repository();
            var service = new ChatService(repo);
            var hub = new ChatHub(service);

            var contextMock = new Mock<HubCallerContext>();
            contextMock.SetupGet(x => x.ConnectionId).Returns("testconn");

            var groupMock = new Mock<IGroupManager>();
            groupMock.Setup(x => x.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), default));

            var clientProxyMock = new ClientProxy();
            var clientMock = new Mock<IHubCallerClients>();
            clientMock.SetupGet(x => x.All).Returns(clientProxyMock);

            hub.Context = contextMock.Object;
            hub.Clients = clientMock.Object;
            hub.Groups = groupMock.Object;

            //Act
            var success = hub.JoinChat("test").Result;

            //Assert
            success.Should().BeTrue();
            clientProxyMock.SendCoreAsyncInvokedCount.Should().Be(1);
        }

        [Fact]
        public void Users_Should_Not_Be_Notified_When_User_Fails_To_Join()
        {
            //Arrange
            var existingUser = new User("test", "123");
            var userDict = new Dictionary<string, User>()
            {
                { existingUser.Username, existingUser }
            };
            var repo = new Repository(userDict);
            var service = new ChatService(repo);
            var hub = new ChatHub(service);

            var contextMock = new Mock<HubCallerContext>();
            contextMock.SetupGet(x => x.ConnectionId).Returns("testconn");

            var groupMock = new Mock<IGroupManager>();
            groupMock.Setup(x => x.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), default));

            var clientProxyMock = new ClientProxy();
            var clientMock = new Mock<IHubCallerClients>();
            clientMock.SetupGet(x => x.All).Returns(clientProxyMock);

            hub.Context = contextMock.Object;
            hub.Clients = clientMock.Object;
            hub.Groups = groupMock.Object;

            //Act
            var success = hub.JoinChat("test").Result;

            //Assert
            success.Should().BeFalse();
            clientProxyMock.SendCoreAsyncInvokedCount.Should().Be(0);
        }
    }
}
