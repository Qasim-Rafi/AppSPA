using CoreWebApi.Data;
using CoreWebApi.Helpers;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Hubs
{
    public class WebRtcHub : Hub
    {
        //private readonly DataContext _context;
        //public WebRtcHub(DataContext context)
        //{
        //    _context = context;
        //}
        private static readonly List<Room> RoomsThatAreFull = new List<Room>();
        private static readonly List<Room> RoomsThatAreActive = new List<Room>();
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public RtcIceServer[] GetIceServers()
        {
            // Perhaps Ice server management.

            return new RtcIceServer[] { new RtcIceServer() { Username = "", Credential = "" } };
        }

        public async Task Join(string userName, string roomName)
        {
            var user = RTCUser.Get(userName, Context.ConnectionId);
            var room = Room.Get(roomName);

            if (user.CurrentRoom != null)
            {
                room.Users.Remove(user);
                await SendUserListUpdate(Clients.Others, room, false);
            }

            user.CurrentRoom = room;
            room.Users.Add(user);

            await SendUserListUpdate(Clients.Caller, room, true);
            await SendUserListUpdate(Clients.Others, room, false);
            if (!RoomsThatAreActive.Any(x => x.Name == room.Name))
            {
                RoomsThatAreActive.Add(room);
            }
            if (room.Users.Count() == 2)
            {
                RoomsThatAreFull.Add(room);
            }
        }
        public async Task CheckRoomIsFull(string roomName)
        {
            if (RoomsThatAreFull.Select(m => m.Name).Contains(roomName))
                await Clients.Client(Context.ConnectionId).SendAsync("CheckRoomIsFull", true);
            else
                await Clients.Client(Context.ConnectionId).SendAsync("CheckRoomIsFull", false);
        }
        public async Task CheckRoomIsActive(string roomName)
        {
            if (RoomsThatAreActive.Select(m => m.Name).Contains(roomName))
                await Clients.Client(Context.ConnectionId).SendAsync("CheckRoomIsActive", true);
            else
                await Clients.Client(Context.ConnectionId).SendAsync("CheckRoomIsActive", false);
        }
        public async Task SendCallSignalToUser(int userId, string userName, string roomName, int senderUserId)
        {
            await Clients.Others.SendAsync("ReceiveCallSignalFromUser", userId, userName, roomName, senderUserId);
        }
        public async Task SendCallSignalToGroup(string userIds, string userName, string roomName, int senderUserId, int groupId, string receiverNames, string groupName)
        {
            await Clients.Others.SendAsync("ReceiveCallSignalFromGroup", userIds, userName, roomName, senderUserId, groupId, receiverNames, groupName);
        }
        //public async Task CheckUserExistInGroup(int groupId, int userId)
        //{
        //    var exist = _context.ChatGroups.Any(m => m.Id == groupId && m.UserIds.Contains(userId.ToString()));
        //    await Clients.Caller.SendAsync("UserExistInGroup", exist);
        //}
        public async Task CheckRoomUsers(string roomName)
        {
            var room = Room.Get(roomName);
            var userCount = room.Users.Count();
            await Clients.All.SendAsync("RoomUsersCount", userCount);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await HangUp();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task HangUp()
        {
            var callingUser = RTCUser.Get(Context.ConnectionId);

            if (callingUser == null)
            {
                return;
            }

            if (callingUser.CurrentRoom != null)
            {
                callingUser.CurrentRoom.Users.Remove(callingUser);
                await SendUserListUpdate(Clients.Others, callingUser.CurrentRoom, false);
            }
            if (callingUser.CurrentRoom.Users.Count() == 0)
            {
                RoomsThatAreFull.Remove(callingUser.CurrentRoom);
            }
            if (callingUser.CurrentRoom.Users.Count() == 0)
            {
                RoomsThatAreActive.Remove(callingUser.CurrentRoom);
                Room.Remove(callingUser.CurrentRoom);
            }
            if (RoomsThatAreActive.Count() > 0)
            {
                var toRemove = RoomsThatAreActive.Where(m => m.Name == callingUser.CurrentRoom.Name).Select(m => m.Users).FirstOrDefault();
                if (toRemove != null)
                {
                    toRemove.Remove(callingUser);
                }
            }
            RTCUser.Remove(callingUser);
        }

        // WebRTC Signal Handler
        public async Task SendSignal(string signal, string targetConnectionId)
        {
            var callingUser = RTCUser.Get(Context.ConnectionId);
            var targetUser = RTCUser.Get(targetConnectionId);

            // Make sure both users are valid
            if (callingUser == null || targetUser == null)
            {
                return;
            }

            // These folks are in a call together, let's let em talk WebRTC
            await Clients.Client(targetConnectionId).SendAsync("receiveSignal", callingUser, signal);
        }

        private async Task SendUserListUpdate(IClientProxy to, Room room, bool callTo)
        {
            await to.SendAsync(callTo ? "callToUserList" : "updateUserList", room.Name, room.Users);
        }
    }
}
