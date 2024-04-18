using Microsoft.AspNetCore.Mvc;
using NBitcoin;
using NBitcoin.Protocol;
using System;
using System.Net;
using ss3.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace ss3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeInfoController : ControllerBase
    {
        private readonly IHubContext<BitcoinHub> _hubContext;

        public NodeInfoController(IHubContext<BitcoinHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("nodeinfo")]
        public IActionResult GetNodeInfo()
        {
            try
            {
                IPAddress selectedAddress = IPAddress.Parse("66.94.117.48");
                Console.WriteLine($"IP {selectedAddress}");

                var peerEndPoint = new IPEndPoint(selectedAddress, 8333);

                using (var node = Node.Connect(Network.Main, peerEndPoint))
                {
                    node.StateChanged += (sender, state) =>
                    {
                        Console.WriteLine($"Node State: {state}");
                    };

                    node.MessageReceived += (sender, e) =>
                    {
                        if (e.Message.Payload is InvPayload invPayload)
                        {
                            foreach (InventoryVector item in invPayload.Inventory)
                            {
                                string eventType = item.Type switch
                                {
                                    InventoryType.MSG_TX => "Transaction",
                                    InventoryType.MSG_BLOCK => "Block",
                                    _ => "Unknown"
                                };

                                Console.WriteLine($"Received {eventType} event: {item.Hash}");
                                _hubContext.Clients.All.SendAsync("ReceiveBitcoinEvent", eventType, item.Hash);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"OTHER Received message: {e.Message.Payload}");

                            var receivedMessage = e.Message.Payload.GetType().Name;
                            Console.WriteLine($"OTHER Received message type: {receivedMessage}");
                        }
                    };

                    node.VersionHandshake();

                    return Ok("Node info request successful");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}