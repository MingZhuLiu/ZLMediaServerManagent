using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using ZLMediaServerManagent.Commons;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Hubs
{
    public class MessageHub : Hub
    {

        public async Task BrowserReceiveMessageAsync(string msg, MessageType messageType)
        {
            var clientId = Context?.User?.FindFirst(ClaimTypes.Sid)?.Value;
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                if (GloableCache.OnlineClients.ContainsKey(clientId) && !string.IsNullOrWhiteSpace(GloableCache.OnlineClients[clientId].SignarlRId))
                {
                    await Clients.Client(GloableCache.OnlineClients[clientId].SignarlRId).SendAsync("BrowserReceiveLoaderMessage", msg, messageType);
                }
            }
        }

        public async Task CleanCookieAndExit(string signalRId)
        {
            await Clients.Client(signalRId).SendAsync("CleanCookieAndExit");
        }


        public override async Task OnConnectedAsync()
        {
            //将连接上来的消息服务与浏览器客户端做绑定
            var clientId = Context?.User?.FindFirst(ClaimTypes.Sid)?.Value;
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                if (GloableCache.OnlineClients.ContainsKey(clientId))
                {
                    //cookie冲突，之前的强制清除下线
                    if (!string.IsNullOrWhiteSpace(GloableCache.OnlineClients[clientId].SignarlRId))
                    {
                        await CleanCookieAndExit(GloableCache.OnlineClients[clientId].SignarlRId);
                    }
                    GloableCache.OnlineClients[clientId].SignarlRId = Context.ConnectionId;
                }
                else
                {
                    GloableCache.OnlineClients.Add(clientId, new Models.ViewDto.OnlineClientTokenInfo() { ClientId = clientId, SignarlRId = Context.ConnectionId });
                }
            }
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            //断开连接取消绑定
            var item = GloableCache.OnlineClients.Values.Where(p => Context.ConnectionId == p.SignarlRId).FirstOrDefault();
            if (item != null)
            {
                item.SignarlRId = null;
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}