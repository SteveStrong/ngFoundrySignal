
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ngFoundrySignal
{
    public class ShapeHub : Hub
    {
        public Task SayHello()
        {
            return Clients.All.SendAsync("hello from shapehub");
        }

        public Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            return Clients.All.SendAsync("Receive", message);
        }


        private static readonly ConcurrentDictionary<string, object> _connections = new ConcurrentDictionary<string, object>();

        public Task ClientCount()
        {
            var result = Clients.All.SendAsync("clientCount", _connections.Count, "groupCount", this.GroupCount.Count);
             return result;
        }

        #region Connection Methods
        public override Task OnConnectedAsync()
        {
            _connections.TryAdd(Context.ConnectionId, null);
            var result = Clients.All.SendAsync("clientCountChanged", _connections.Count, "connected", this.GroupCount.Count);
            base.OnConnectedAsync();
            return result;
        }



        public override Task OnDisconnectedAsync(System.Exception stopCalled)
        {
            _connections.TryRemove(Context.ConnectionId, out object value);
            var result = Clients.All.SendAsync("clientCountChanged", _connections.Count, "disconnected", this.GroupCount.Count);
            base.OnDisconnectedAsync(stopCalled);
            return result;
        }

        #endregion


        //http://www.asp.net/signalr/overview/signalr-20/hubs-api/hubs-api-guide-server#groupsfromhub

        ConcurrentDictionary<string, int> GroupCount = new ConcurrentDictionary<string, int>();

        public Task JoinSessionGroup(string sessionKey)
        {
            var count = GroupCount.GetOrAdd(sessionKey, 0);
            GroupCount.TryUpdate(sessionKey, count + 1, count);
            return Groups.AddToGroupAsync(Context.ConnectionId, sessionKey);
        }

        public Task LeaveSessionGroup(string sessionKey)
        {
            var count = GroupCount.GetOrAdd(sessionKey, 0);
            GroupCount.TryUpdate(sessionKey, count - 1, count);
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionKey);
        }

        public int SessionCount(string sessionKey)
        {
            GroupCount.TryGetValue(sessionKey, out int count);
            return count;
        }

        public void AuthorSessionCount(string sessionKey, string userId)
        {
           //should only callback if this session key has other players
           var total = SessionCount(sessionKey);
           Clients.Caller.SendAsync("playerSessionCount", sessionKey, userId, total);
        }


        //public void AuthorPayloadKnowtify(string sessionKey, string userId, string payload)
        //{
        //    Clients.OthersInGroup(sessionKey).payloadKnowtify(sessionKey, userId, payload);
        //}

        //public void AuthorPayloadAdded(string sessionKey, string userId, string payload)
        //{
        //    Clients.OthersInGroup(sessionKey).payloadAdded(sessionKey, userId, payload);
        //}

        //public void AuthorPayloadDeleted(string sessionKey, string userId, string payload)
        //{
        //    Clients.OthersInGroup(sessionKey).payloadDeleted(sessionKey, userId, payload);
        //}

        //public void AuthorChangedModel(string sessionKey, string userId, string payload)
        //{
        //    Clients.OthersInGroup(sessionKey).updateModel(sessionKey, userId, payload);
        //}

        //public void AuthorReparentModelTo(string sessionKey, string uniqueID, string oldParentID, string newParentID, string location)
        //{
        //    Clients.OthersInGroup(sessionKey).parentModelTo(sessionKey, uniqueID, oldParentID, newParentID, location);
        //}

        //public void AuthorMovedShapeTo(string sessionKey, string uniqueID, double pinX, double pinY, double angle)
        //{
        //    Clients.OthersInGroup(sessionKey).repositionShapeTo(sessionKey, uniqueID, pinX, pinY, angle);
        //}

        //methods to syncrinize workspaces

        //public async Task PlayerCreateSession(string sessionKey, string userId, string payload)
        //{
        //    await JoinSessionGroup(sessionKey);
        //    Clients.OthersInGroup(sessionKey).authorReceiveJoinSessionFromPlayer(sessionKey, userId, payload);
        //    Clients.Caller.confirmCreateSession(sessionKey, userId);
        //}

        //public async Task PlayerJoinSession(string sessionKey, string userId, string payload)
        //{
        //    await JoinSessionGroup(sessionKey);
        //    Clients.OthersInGroup(sessionKey).authorReceiveJoinSessionFromPlayer(sessionKey, userId, payload);
        //    Clients.Caller.confirmJoinSession(sessionKey, userId);
        //}


        //public void PlayerExitSession(string sessionKey, string userId, string payload)
        //{
        //    Clients.OthersInGroup(sessionKey).receiveExitSessionFromPlayer(sessionKey, userId, payload);
        //    LeaveSessionGroup(sessionKey);
        //    Clients.Caller.confirmExitSession(sessionKey, userId);
        //}

        //public void AuthorResyncSession(string sessionKey, string userId)
        //{
        //    //should only callback if this session key has other players
        //    var total = SessionCount(sessionKey);
        //    Clients.Caller.confirmResyncSession(sessionKey, userId, total);
        //}

        //public void AuthorSendJoinSessionModelToPlayers(string sessionKey, string userId, string payload)
        //{
        //    Clients.OthersInGroup(sessionKey).playerReceiveJoinSessionModel(sessionKey, userId, payload);
        //}


        public void SendPing(string sessionKey, string sender, string senderId)
        {
            Clients.All.SendAsync("receivePing", sessionKey, sender, senderId);
        }

        public void SendMessage(string sessionKey, string sender, string senderId, string textMessage)
        {
            Clients.All.SendAsync("receiveMessage", sessionKey, sender, senderId);
        }

        public void AuthorInvite(string sessionKey, string author, string authorId, string player, string playerId)
        {
           Clients.Others.SendAsync("receiveInvitation", sessionKey, player, playerId, author, authorId);
        }

        //public void PlayerRSVP(string sessionKey, string player, string playerId, string author, string authorId, string payload)
        //{
        //    Clients.Others.receiveRSVP(sessionKey, player, playerId, author, authorId, payload);
        //}

        public void AuthorRequestModelFromPlayer(string sessionKey, string author, string authorId)
        {
           Clients.OthersInGroup(sessionKey).SendAsync("sendModelToAuthor", sessionKey, author, authorId);
        }

        //public void PlayerSendModelToAuthor(string sessionKey, string player, string playerId, string payload)
        //{
        //    Clients.OthersInGroup(sessionKey).authorReceiveModelFromPlayer(sessionKey, player, playerId, payload);
        //}

        //public void AuthorSendModelToPlayers(string sessionKey, string author, string authorId, string payload)
        //{
        //    Clients.OthersInGroup(sessionKey).playersReceiveSynchronizedModelFromAuthor(sessionKey, author, authorId, payload);
        //}

        //public void PlayerInviteSelf(string userId, string payload)
        //{
        //    Clients.Others.selfReceiveInviteFromPlayer(userId, payload);
        //}

        //public void SelfSendModelToPlayers(string sessionKey, string userId, string payload)
        //{
        //    Clients.OthersInGroup(sessionKey).playerReceiveSynchronizedModelSelf(sessionKey, userId, payload);
        //}

        //public void AuthorClearSession(string sessionKey, string author, string authorId, string payload)
        //{
        //    Clients.OthersInGroup(sessionKey).playersReceiveClearSessionFromAuthor(sessionKey, author, authorId, payload);
        //}



    }
}
