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
    public class ChatHub : Hub
    {
        //private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                try
                {
                    //Add Logged User
                    var userName = httpContext.Request.Query["user"].ToString();
                    //var UserAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString();
                    var connId = Context.ConnectionId.ToString();
                    ConnectionMapping<string>.Add(userName, connId);
                    
                }
                catch (Exception) { }
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                //Remove Logged User
                var username = httpContext.Request.Query["user"];
                ConnectionMapping<string>.Remove(username, Context.ConnectionId);

            }

            //return base.OnDisconnectedAsync(exception);
        }
    }
}
