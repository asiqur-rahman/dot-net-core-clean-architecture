using Microsoft.AspNetCore.SignalR;
using Project.Core.Entities.Common.User;

namespace Project.App.Hubs
{
    public class SignalRHub : Hub<IStreamingHub>
    {
        private readonly List<SignalRUser> _Users;
        private readonly List<SignalRUserCall> _SignalRUserCalls;
        private readonly List<SignalRUserOffer> _SignalRUserOffers;
        private readonly SignalRHubService _signalRHubService;

        public SignalRHub(List<SignalRUser> users, List<SignalRUserCall> SignalRUserCalls, List<SignalRUserOffer> SignalRUserOffers, SignalRHubService signalRHubService)
        {
            _Users = users;
            _SignalRUserCalls = SignalRUserCalls;
            _SignalRUserOffers = SignalRUserOffers;
            _signalRHubService = signalRHubService;
        }

        public async Task Join(string username)
        {
            // Add the new user
            _Users.Add(new SignalRUser
            {
                Username = username,
                ConnectionId = Context.ConnectionId
            });

            // Send down the new list to all clients
            await SendUserListUpdate();
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.Identity.Name;
            _signalRHubService.AddMapping(userId, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.Identity.Name;
            // Hang up any calls the user is in
            await HangUp(); // Gets the user from "Context" which is available in the whole hub

            // Remove the user
            _Users.RemoveAll(u => u.ConnectionId == Context.ConnectionId);
            _signalRHubService.RemoveMapping(userId,Context.ConnectionId);

            // Send down the new user list to all clients
            await SendUserListUpdate();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task CallUser(SignalRUser targetConnectionId)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var targetUser = _Users.SingleOrDefault(u => u.ConnectionId == targetConnectionId.ConnectionId);

            // Make sure the person we are trying to call is still here
            if (targetUser == null)
            {
                // If not, let the caller know
                await Clients.Caller.CallDeclined(targetConnectionId, "The user you called has left.");
                return;
            }

            // And that they aren't already in a call
            if (GetSignalRUserCall(targetUser.ConnectionId) != null)
            {
                await Clients.Caller.CallDeclined(targetConnectionId, string.Format("{0} is already in a call.", targetUser.Username));
                return;
            }

            // They are here, so tell them someone wants to talk
            await Clients.Client(targetConnectionId.ConnectionId).IncomingCall(callingUser);

            // Create an offer
            _SignalRUserOffers.Add(new SignalRUserOffer
            {
                Caller = callingUser,
                Callee = targetUser
            });
        }

        public async Task AnswerCall(bool acceptCall, SignalRUser targetConnectionId)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var targetUser = _Users.SingleOrDefault(u => u.ConnectionId == targetConnectionId.ConnectionId);

            // This can only happen if the server-side came down and clients were cleared, while the user
            // still held their browser session.
            if (callingUser == null)
            {
                return;
            }

            // Make sure the original caller has not left the page yet
            if (targetUser == null)
            {
                await Clients.Caller.CallEnded(targetConnectionId, "The other user in your call has left.");
                return;
            }

            // Send a decline message if the callee said no
            if (acceptCall == false)
            {
                await Clients.Client(targetConnectionId.ConnectionId).CallDeclined(callingUser, string.Format("{0} did not accept your call.", callingUser.Username));
                return;
            }

            // Make sure there is still an active offer.  If there isn't, then the other use hung up before the Callee answered.
            var offerCount = _SignalRUserOffers.RemoveAll(c => c.Callee.ConnectionId == callingUser.ConnectionId
                                                  && c.Caller.ConnectionId == targetUser.ConnectionId);
            if (offerCount < 1)
            {
                await Clients.Caller.CallEnded(targetConnectionId, string.Format("{0} has already hung up.", targetUser.Username));
                return;
            }

            // And finally... make sure the user hasn't accepted another call already
            if (GetSignalRUserCall(targetUser.ConnectionId) != null)
            {
                // And that they aren't already in a call
                await Clients.Caller.CallDeclined(targetConnectionId, string.Format("{0} chose to accept someone elses call instead of yours :(", targetUser.Username));
                return;
            }

            // Remove all the other offers for the call initiator, in case they have multiple calls out
            _SignalRUserOffers.RemoveAll(c => c.Caller.ConnectionId == targetUser.ConnectionId);

            // Create a new call to match these folks up
            _SignalRUserCalls.Add(new SignalRUserCall
            {
                Users = new List<SignalRUser> { callingUser, targetUser }
            });

            // Tell the original caller that the call was accepted
            await Clients.Client(targetConnectionId.ConnectionId).CallAccepted(callingUser);

            // Update the user list, since thes two are now in a call
            await SendUserListUpdate();
        }

        public async Task HangUp()
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);

            if (callingUser == null)
            {
                return;
            }

            var currentCall = GetSignalRUserCall(callingUser.ConnectionId);

            // Send a hang up message to each user in the call, if there is one
            if (currentCall != null)
            {
                foreach (var user in currentCall.Users.Where(u => u.ConnectionId != callingUser.ConnectionId))
                {
                    await Clients.Client(user.ConnectionId).CallEnded(callingUser, string.Format("{0} has hung up.", callingUser.Username));
                }

                // Remove the call from the list if there is only one (or none) person left.  This should
                // always trigger now, but will be useful when we implement conferencing.
                currentCall.Users.RemoveAll(u => u.ConnectionId == callingUser.ConnectionId);
                if (currentCall.Users.Count < 2)
                {
                    _SignalRUserCalls.Remove(currentCall);
                }
            }

            // Remove all offers initiating from the caller
            _SignalRUserOffers.RemoveAll(c => c.Caller.ConnectionId == callingUser.ConnectionId);

            await SendUserListUpdate();
        }

        // WebRTC Signal Handler
        public async Task SendSignal(string signal, string targetConnectionId)
        {
            var callingUser = _Users.SingleOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var targetUser = _Users.SingleOrDefault(u => u.ConnectionId == targetConnectionId);

            // Make sure both users are valid
            if (callingUser == null || targetUser == null)
            {
                return;
            }

            // Make sure that the person sending the signal is in a call
            var SignalRUserCall = GetSignalRUserCall(callingUser.ConnectionId);

            // ...and that the target is the one they are in a call with
            if (SignalRUserCall != null && SignalRUserCall.Users.Exists(u => u.ConnectionId == targetUser.ConnectionId))
            {
                // These folks are in a call together, let's let em talk WebRTC
                await Clients.Client(targetConnectionId).ReceiveSignal(callingUser, signal);
            }
        }

        #region Private Helpers

        private async Task SendUserListUpdate()
        {
            _Users.ForEach(u => u.InCall = (GetSignalRUserCall(u.ConnectionId) != null));
            await Clients.All.UpdateUserList(_Users);
        }

        private SignalRUserCall GetSignalRUserCall(string connectionId)
        {
            var matchingCall =
                _SignalRUserCalls.SingleOrDefault(uc => uc.Users.SingleOrDefault(u => u.ConnectionId == connectionId) != null);
            return matchingCall;
        }

        #endregion

    }
    public interface IStreamingHub
    {
        Task UpdateUserList(List<SignalRUser> userList);
        Task CallAccepted(SignalRUser acceptingUser);
        Task CallDeclined(SignalRUser decliningUser, string reason);
        Task IncomingCall(SignalRUser callingUser);
        Task ReceiveSignal(SignalRUser signalingUser, string signal);
        Task CallEnded(SignalRUser signalingUser, string signal);
    }

    public class SignalRUserOffer
    {
        public SignalRUser Caller { get; set; }
        public SignalRUser Callee { get; set; }
    }

    public class SignalRUser
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }
        public bool InCall { get; set; }
    }

    public class SignalRUserCall
    {
        public List<SignalRUser> Users { get; set; }
    }

    public class SignalRHubService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _userConnections = new Dictionary<string, Dictionary<string, string>>();

        private readonly IHubContext<SignalRHub> _hubContext;
        public SignalRHubService(IHubContext<SignalRHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void AddMapping(string userId, string connectionId)
        {
            if (!_userConnections.TryGetValue(userId, out var connections))
            {
                connections = new Dictionary<string, string>();
                _userConnections[userId] = connections;
            }

            connections[connectionId] = connectionId;
        }

        public void RemoveMapping(string userId, string connectionId)
        {
            if (_userConnections.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);

                // Remove the user entry if there are no connections left
                if (connections.Count == 0)
                {
                    _userConnections.Remove(userId);
                }
            }
        }

        public bool IsUserConnected(string userId)
        {
            return _userConnections.ContainsKey(userId);
        }

        public IEnumerable<string> GetConnectionIds(string userId)
        {
            return _userConnections.TryGetValue(userId, out var connections)
                ? connections.Values
                : Enumerable.Empty<string>();
        }

        public async Task InvokeHubMethod(string methodName, params object[] args)
        {
            if (_hubContext != null)
            {
                await _hubContext.Clients.All.SendAsync(methodName, args);
            }
        }

        public async Task InvokeHubMethod(string userId, string methodName, params object[] args)
        {
            var connectionId = GetConnectionIds(userId);

            if (connectionId != null)
            {
                await _hubContext.Clients.Clients(connectionId).SendAsync(methodName, args);
            }
        }
    }

}
