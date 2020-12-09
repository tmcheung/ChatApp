using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data
{
    public class User
    {
        public User(string username, string connectionId)
        {
            Username = username;
            ConnectionId = connectionId;
        }

        public string Username { get; private set; }
        public string ConnectionId { get; private set; }
    }
}
