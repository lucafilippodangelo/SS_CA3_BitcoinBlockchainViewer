using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using ss3.Models;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;

using NBitcoin;
using NBitcoin.Protocol;
using Microsoft.AspNetCore.SignalR;
using ss3.SignalR;

namespace ss3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<BitcoinHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, IHubContext<BitcoinHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {

            try
            {
                IPAddress selectedAddress = IPAddress.Parse("66.94.117.48"); //LD this is a working node
                Console.WriteLine($" 000 IP {selectedAddress}");

                var peerEndPoint = new IPEndPoint(selectedAddress, 8333); //LD bitcoin port is usually this one

                //LD connection to node
                using (var node = Node.Connect(Network.Main, peerEndPoint))
                {
                    //LD any changing state
                    node.StateChanged += (sender, state) =>
                    {
                        Console.WriteLine($"LD Node Sate: {state}");
                    };

                    //LD handling any received message
                    node.MessageReceived += (sender, e) =>
                    {
                        if (e.Message.Payload is InvPayload invPayload) //LD check if inv
                        {
                            foreach (InventoryVector item in invPayload.Inventory)//LD looping in vectors
                            {
                                string eventType = item.Type switch
                                {
                                    InventoryType.MSG_TX => "Transaction",
                                    InventoryType.MSG_BLOCK => "Block", //LD hash related to data block. That's what I need
                                    _ => "Unknown"
                                };

                                Console.WriteLine($" 001 Received {eventType} event: {item.Hash}"); //LD to be deleted
                                _hubContext.Clients.All.SendAsync("ReceiveBitcoinEvent", eventType, item.Hash); //LD will send updates to signalR

                            }
                        }
                        else
                        {
                            //LD any other message
                            Console.WriteLine($"LD OTHER Received message: {e.Message.Payload}");

                            var receivedMessage = e.Message.Payload.GetType().Name;
                            Console.WriteLine($"LD OTHER Received message type: {receivedMessage}");
                        }
                    };

                    //LD vesron handshake
                    node.VersionHandshake();

                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

            return StatusCode(500, "Internal Server Error");

            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}