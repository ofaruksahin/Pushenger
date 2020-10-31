using Microsoft.AspNetCore.SignalR;
using Pushenger.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Pushenger.Api.Hubs
{
    /// <summary>
    /// Bildirim sistemine abone olmak için kullanılır
    /// </summary>
    public class SubscriptionHub : Hub
    {
        IUnitOfWork unitOfWork;       

        public SubscriptionHub(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public override Task OnConnectedAsync()
        {         
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
