using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class Room
    {
        private static readonly List<Room> Rooms = new List<Room>();

        public string Name { get; set; }
        public List<RTCUser> Users { get; set; } = new List<RTCUser>();

        public static int TotalUsers => Rooms.Sum(room => room.Users.Count);

        public static Room Get(string name)
        {
            lock (Rooms)
            {
                var current = Rooms.SingleOrDefault(r => r.Name == name);

                if (current == default(Room))
                {
                    current = new Room
                    {
                        Name = name
                    };
                    Rooms.Add(current);
                }

                return current;
            }
        }
    }
}
