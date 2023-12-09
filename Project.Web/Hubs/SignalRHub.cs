using Microsoft.AspNetCore.SignalR;

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
            // Get user ID from your authentication mechanism
            var userId = Context.User.Identity.Name;

            // Add mapping when connection is established
            _userMappingService.AddMapping(userId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToUser(string userId, string message)
        {
            var connectionId = _userMappingService.GetConnectionId(userId);

            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
        }
    }


    public class SignalRUserMappingService
    {
        private readonly Dictionary<string, string> _userConnections = new Dictionary<string, string>();

        public void AddMapping(string userId, string connectionId)
        {
            _userConnections[userId] = connectionId;
        }

        public string GetConnectionId(string userId)
        {
            return _userConnections.TryGetValue(userId, out var connectionId) ? connectionId : null;
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

        public async Task InvokeHubMethod(string methodName, params object[] args)
        {
            if (_hubContext != null)
            {
                await _hubContext.Clients.All.SendAsync(methodName, args);
            }
        }

        public async Task InvokeHubMethod(string userId, string methodName, params object[] args)
        {
            var connectionId = _userMappingService.GetConnectionId(userId);

            if (connectionId != null)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync(methodName, args);
            }
        }
    }

}
