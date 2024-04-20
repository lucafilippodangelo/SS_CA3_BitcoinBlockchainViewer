using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NBitcoin;
using NBitcoin.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ss3_back.Helpers;
using ss3_back.SignalR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                ///*
                string jsonData = @"{
                       ""Timestamp"":""2024-04-20T17:40:14.0000000+00:00"",
                       ""Transactions"":[
                          {
                             ""TransactionId"":""45950b1628c2dc34a2e00c31cb96ab5fcf15ffed40364aaf972ba71cc865bf5b"",
                             ""TotalValue"":""10.28637578""
                          },
                          {
                             ""TransactionId"":""04b0a83476f9a452b98873af179e22912537b075a118653b12bd88c8579b1e8f"",
                             ""TotalValue"":""0.0024609""
                          },
                          {
                             ""TransactionId"":""2360fdf9eac828cc93e678609f7343c1969739bfa4837636f4e71957a0c51f8a"",
                             ""TotalValue"":""0.00060756""
                          },
                          {
                             ""TransactionId"":""c8fd9166901ccc41bd963236d1ea35b9381db3331e2e70bb26915ddc6414fa8a"",
                             ""TotalValue"":""0.06042177""
                          },
                          {
                             ""TransactionId"":""c5dbdf8b10652b414454db5b4fabe3bed95af66d8edb928d20d8760b89e158b9"",
                             ""TotalValue"":""0.49264513""
                          },
                          {
                             ""TransactionId"":""03b2a75952ab9ff14f26f9c52d92880b3bd08aff72b128fb3f6d1f8c124312ff"",
                             ""TotalValue"":""0.00167801""
                          },
                          {
                             ""TransactionId"":""2360fdf9eac828cc93e678609f7343c1969739bfa4837636f4e71957a0c51f8a"",
                             ""TotalValue"":""0.00060756""
                          },
                          {
                             ""TransactionId"":""c8fd9166901ccc41bd963236d1ea35b9381db3331e2e70bb26915ddc6414fa8a"",
                             ""TotalValue"":""0.06042177""
                          },
                          {
                             ""TransactionId"":""c5dbdf8b10652b414454db5b4fabe3bed95af66d8edb928d20d8760b89e158b9"",
                             ""TotalValue"":""0.49264513""
                          },
                          {
                             ""TransactionId"":""03b2a75952ab9ff14f26f9c52d92880b3bd08aff72b128fb3f6d1f8c124312ff"",
                             ""TotalValue"":""0.00167801""
                          },
                          {
                             ""TransactionId"":""2360fdf9eac828cc93e678609f7343c1969739bfa4837636f4e71957a0c51f8a"",
                             ""TotalValue"":""0.00060756""
                          },
                          {
                             ""TransactionId"":""c8fd9166901ccc41bd963236d1ea35b9381db3331e2e70bb26915ddc6414fa8a"",
                             ""TotalValue"":""0.06042177""
                          },
                          {
                             ""TransactionId"":""c5dbdf8b10652b414454db5b4fabe3bed95af66d8edb928d20d8760b89e158b9"",
                             ""TotalValue"":""0.49264513""
                          },
                          {
                             ""TransactionId"":""03b2a75952ab9ff14f26f9c52d92880b3bd08aff72b128fb3f6d1f8c124312ff"",
                             ""TotalValue"":""0.00167801""
                          }
                       ],
                       ""Nonce"":""1699485074"",
                       ""Difficulty"":""86388558925171.02"",
                       ""HashVerification"":""True""
                    }";

                // Converting the JSON string to a JObject
                JObject json = JObject.Parse(jsonData);

                // Serialize the JObject to a string
                string jsonString = JsonConvert.SerializeObject(json);

                // Send the serialized JSON string through SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveBlockEvent", jsonString);

                return Ok("Node info request stopped");
                //*/


                //LD DYNAMIC IP
                //IPAddress[] addresses = Dns.GetHostAddresses("seed.bitcoin.sipa.be");
                //Random random = new Random();
                //IPAddress selectedAddress = addresses[random.Next(addresses.Length)];
                //Console.WriteLine($" 000 IP {selectedAddress}");

                /*
                //LD STATIC IP
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

                                //LD if transaction send
                                if (item.Type.HasFlag(InventoryType.MSG_TX))
                                {
                                    await _hubContext.Clients.All.SendAsync("ReceiveTransactionEvent", eventType.ToString(), item.Hash.ToString());
                                }
                                

                                //LD if block request block data
                                if (item.Type.HasFlag(InventoryType.MSG_BLOCK))
                                {
                                    node.SendMessage(new GetDataPayload(item));
                                }

                            }
                        }
                        else if (e.Message.Payload is BlockPayload blockPayload)
                        {

                            BlockHeader header = blockPayload.Object.Header;

                            DateTimeOffset timestamp = header.BlockTime;
                            Console.WriteLine($"Block Timestamp: {timestamp}");

                            uint nonce = header.Nonce;
                            Console.WriteLine($"Nonce: {nonce}");

                            double difficulty = header.Bits.Difficulty;
                            Console.WriteLine($"Difficulty: {difficulty}");

                            uint256 computedHash = header.GetHash();
                            uint256 blockHash = blockPayload.Object.GetHash();
                            bool hashVerification = computedHash == blockHash;
                            Console.WriteLine(hashVerification ? "Hash verified" : "Hash not verified");

                            // LD Transactions
                            Block block = blockPayload.Object;
                            var transactions = block.Transactions;

                            var jsonData = CreateBlockData.GenerateJson(timestamp, transactions.ToArray(), nonce, difficulty, hashVerification);

                            //LD Send block
                            await _hubContext.Clients.All.SendAsync("ReceiveBlockEvent", jsonData);

                        }

                        else
                        {
                            Console.WriteLine($"LD OTHER Received message: {e.Message.Payload}");

                            var receivedMessage = e.Message.Payload.GetType().Name;
                            Console.WriteLine($"LD OTHER Received message type: {receivedMessage}");
                        }
                    };

                    node.VersionHandshake();

                    await Task.Delay(Timeout.Infinite, cancellationToken);

                    return Ok("Node info request stopped");
                }
                */



            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}