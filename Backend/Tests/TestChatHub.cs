using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Server.Data;
using Server.Hubs;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Text;
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

            var clientProxyMock = new Mock<IClientProxy>();
            clientProxyMock.Setup(x => x.SendAsync(It.Is<string>(s => s.Equals("ReceiveMembershipChange")), It.IsAny<object>(), default))
                .Verifiable();
            var clientMock = new Mock<IHubCallerClients>();
            clientMock.SetupGet(x => x.All).Returns(clientProxyMock.Object);

            hub.Context = contextMock.Object;
            hub.Clients = clientMock.Object;
            hub.Groups = groupMock.Object;

            //Act
            var success = hub.JoinChat("test").Result;

            //Assert
            success.Should().BeTrue();
            clientProxyMock.Verify();
        }
    }
}
