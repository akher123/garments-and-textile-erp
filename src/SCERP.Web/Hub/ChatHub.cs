using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Web.Hub
{
    [Microsoft.AspNet.SignalR.Hubs.HubName("chathub")]
    public class ChatHub: Microsoft.AspNet.SignalR.Hub
    {
        public static void NotificationMessage()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            // the update client method will update the connected client about any recent changes in the server data
            context.Clients.All.updatedClients();
        }
    }
}