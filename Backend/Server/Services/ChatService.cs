using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    internal class ChatService
    {
        private readonly Repository _repo;

        public ChatService(Repository repo)
        {
            _repo = repo;
        }

        public User HandleDisconnect(string connectionId)
        {
            return _repo.RemoveUser(connectionId);
        }

        public IDictionary<string, User> GetUsers()
        {
            return _repo.GetUsers();
        }

        public void AddUser(string username, string connectionId)
        {
            _repo.AddUser(username, connectionId);
        }

        internal User GetUser(string connectionId)
        {
            var user = _repo.GetUser(connectionId);
            if (user == null)
                throw new Exception();
            return user;
        }
    }
}
