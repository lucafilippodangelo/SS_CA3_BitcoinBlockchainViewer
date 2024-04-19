﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NBitcoin;
using NBitcoin.Protocol;
using ss3_back.SignalR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetNodeInfo(CancellationToken cancellationToken)
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

                    node.MessageReceived += async (sender, e) =>
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
                                await _hubContext.Clients.All.SendAsync("ReceiveBitcoinEvent", eventType.ToString(), item.Hash.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine($"LD OTHER Received message: {e.Message.Payload}");

                            var receivedMessage = e.Message.Payload.GetType().Name;
                            Console.WriteLine($"LD OTHER Received message type: {receivedMessage}");
                        }
                    };

                    node.VersionHandshake();

                    // Wait until cancellation is requested
                    await Task.Delay(Timeout.Infinite, cancellationToken);

                    return Ok("Node info request stopped");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}