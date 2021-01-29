using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class RTCUser
    {
        private static readonly List<RTCUser> Users = new List<RTCUser>();

        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        [JsonIgnore]
        public Room CurrentRoom { get; set; }

        public static void Remove(RTCUser user)
        {
            Users.Remove(user);
        }

        public static RTCUser Get(string connectionId)
        {
            return Users.SingleOrDefault(u => u.ConnectionId == connectionId);
        }

        public static RTCUser Get(string userName, string connectionId)
        {
            lock (Users)
            {
                var current = Users.SingleOrDefault(u => u.ConnectionId == connectionId);

                if (current == default(RTCUser))
                {
                    current = new RTCUser
                    {
                        UserName = userName,
                        ConnectionId = connectionId
                    };
                    Users.Add(current);
                }
                else
                {
                    current.UserName = userName;
                }

                return current;
            }
        }
    }
}
