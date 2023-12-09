using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Project.Web.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly SignalRUserMappingService _userMappingService;
        public SignalRHub(SignalRUserMappingService userMappingService)
        {
            _userMappingService = userMappingService;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.Identity.Name;
            _userMappingService.AddMapping(userId, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.Identity.Name; 
            _userMappingService.RemoveMapping(userId, Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToUser(string userId, string message)
        {
            var connectionId = _userMappingService.GetConnectionIds(userId);

            if (connectionId != null)
            {
                await Clients.Clients(connectionId).SendAsync("ReceiveMessage", message);
            }
        }
    }


    public class SignalRUserMappingService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _userConnections = new Dictionary<string, Dictionary<string, string>>();

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
    }

    public class SignalRHubService 
    {
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly SignalRUserMappingService _userMappingService;
        public SignalRHubService(IHubContext<SignalRHub> hubContext, SignalRUserMappingService userMappingService)
        {
            _hubContext = hubContext;
            _userMappingService = userMappingService;
        }
        public void RemoveMapping(string userId, string connectionId)
        {
            _userMappingService.RemoveMapping(userId, connectionId);
        }

        public bool IsUserConnected(string userId)
        {
            return _userMappingService.IsUserConnected(userId);
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
            var connectionId = _userMappingService.GetConnectionIds(userId);

            if (connectionId != null)
            {
                await _hubContext.Clients.Clients(connectionId).SendAsync(methodName, args);
            }
        }
    }

}
