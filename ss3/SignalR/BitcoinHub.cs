using Microsoft.AspNetCore.SignalR;

namespace ss3.SignalR
{
    public class BitcoinHub : Hub
    {
        public async Task SendBitcoinEvent(string eventType, string hash)
        {
            await Clients.All.SendAsync("ReceiveBitcoinEvent", eventType, hash); //LD send events to connected clients. Will need to pull that info from react
        }
    }
}
