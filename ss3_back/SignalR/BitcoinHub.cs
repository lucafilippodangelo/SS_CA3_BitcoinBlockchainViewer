using Microsoft.AspNetCore.SignalR;

namespace ss3_back.SignalR
{
    public class BitcoinHub : Hub
    {
        public async Task SendBitcoinEvent(string eventType, string hash)
        {
            //LD commented, for CA3 I do not need to handle incoming events from sunscribers. Not in scope.
            //try
            //{
            //    Console.WriteLine(" please work ");
            //    await Clients.All.SendAsync("ReceiveBitcoinEvent", eventType.ToString(), hash.ToString());
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //    throw; 
            //}
        }
    }
}