using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Data
{
    internal class Repository
    {
        private readonly IDictionary<string, User> _users;

        public Repository(IDictionary<string, User> users = null)
        {
            _users = users ?? new Dictionary<string, User>();
        }

        public IDictionary<string, User> GetUsers()
        {
            return _users;
        }

        public User RemoveUser(string connectionId)
        {
            var user = GetUser(connectionId);

            if (user != null)
            {
                _users.Remove(user.Username);
                return user;
            }

            return null;
        }

        internal User GetUser(string connectionId)
        {
            return _users
                .Select(u => u.Value)
                .FirstOrDefault(u => u.ConnectionId.Equals(connectionId));
        }

        public void AddUser(string username, string connectionId)
        {
            if (_users.ContainsKey(username))
                throw new Exception("User already exists");

            _users.Add(username, new User(username, connectionId));
        }
    }
}
