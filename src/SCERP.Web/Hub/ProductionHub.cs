using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SCERP.Web.Hub
{
    [HubName("productionHub")]
    public class ProductionHub : Microsoft.AspNet.SignalR.Hub
    {
        public static void NotifyDailySewingProduction()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ProductionHub>();

            // the update client method will update the connected client about any recent changes in the server data
            context.Clients.All.updatedClients();
        }
    }
}