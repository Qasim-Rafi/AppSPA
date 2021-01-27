using CoreWebApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Hubs
{
    public class MessageNotificationHub : Hub
    {       
        //private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        //public async Task SendMessage(string message)
        //{
        //    //Receive Message
        //    List<string> ReceiverConnectionids = _connections.GetConnections("").ToList<string>();
          
        //}
        //public override async Task OnConnectedAsync()
        //{
        //    var httpContext = Context.GetHttpContext();
        //    if (httpContext != null)
        //    {
        //        try
        //        {
        //            //Add Logged User
        //            var userName = httpContext.Request.Query["user"].ToString();
        //            //var UserAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString();
        //            var connId = Context.ConnectionId.ToString();
        //            _connections.Add(userName, connId);

        //            //Update Client
        //            await Clients.All.SendAsync("UpdateOnlineUserList", _connections.ToJson());
        //        }
        //        catch (Exception) { }
        //    }
        //}

        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    var httpContext = Context.GetHttpContext();
        //    if (httpContext != null)
        //    {
        //        //Remove Logged User
        //        var username = httpContext.Request.Query["user"];
        //        _connections.Remove(username, Context.ConnectionId);

        //        //Update Client
        //        await Clients.All.SendAsync("UpdateOnlineUserList", _connections.ToJson());
        //    }

        //    //return base.OnDisconnectedAsync(exception);
        //}
    }
}
