using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NBitcoin;
using NBitcoin.Protocol;
using ss3_back.Helpers;
using ss3_back.SignalR;
using System.Net;

namespace ss3.Controllers
{
    /// <summary>
    /// LD the constructor is initialising the controller with a SignalR hub context.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NodeInfoController : ControllerBase
    {
        private readonly IHubContext<BitcoinHub> _hubContext; //LD initialising SignalR hub
        public NodeInfoController(IHubContext<BitcoinHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// LD "GetBlockchainInfo" is asynchronously handling node information retrieval and interaction with Bitcoin nodes. Details into the method.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("nodeinfo")]
        public async Task<IActionResult> GetBlockchainInfo(CancellationToken cancellationToken)
        {
            try
            {
                //used the below to simulate sending block data to clients. Commented out in final CA3 version
                //LD START SIMULATION SEND A BLOCK EVENT ***********************************************
                /*
                var blockData = new
                {
                    Timestamp = "30/04/2024 08:06:14(UTC - Block Timestamp)",
                    Transactions = new[]
                    {
                
                                                    new
                                                    {
                                                        TransactionId = "b5d096a2f62821311ecc110e5ba0483a202df5aa1da42b91e5885c2540ea4f31",
                                                        TotalValue = "0.62173947",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-60-3E-97-68-18-C1-F7-34-CE-14-F8",
                                                            PayloadLength = 2970077826,
                                                            Checksum = "67-65-B0-88"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    },
                                                    new
                                                    {
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b746",
                                                        TotalValue = "0.00093026",
                                                        TransactionRaw = new
                                                        {
                                                            MagicNumber = 16777216,
                                                            Command = "01-D4-21-3B-8A-78-B6-42-A4-62-0D-C7",
                                                            PayloadLength = 2893358498,
                                                            Checksum = "A7-0C-D3-2B"
                                                        }
                                                    }
                    },
                    Nonce = "2095658546",
                    Difficulty = "88104191118793.16",
                    HashVerification = "True",
                    Hash = "000000000000000000018815b37126242a916666dd4aa7f9d6cf540aca24417e"
                };

                string jsonData = JsonConvert.SerializeObject(blockData, Formatting.None);

                await _hubContext.Clients.All.SendAsync("ReceiveBlockEvent", jsonData);
                */
                //LD END SIMULATION SEND A BLOCK EVENT *************************************************

                //LD Dynamic IP resolution. Retrieves and logs IP addresses of a Bitcoin node seed.
                IPAddress[] addresses = Dns.GetHostAddresses("seed.bitcoin.sipa.be");
                Random random = new Random();
                IPAddress selectedAddressTemp = addresses[random.Next(addresses.Length)];
                Console.WriteLine($" 000 IP {selectedAddressTemp}");

                //LD Static IP assignment(dynamic IP resolution and assignment is not used because not all the addresses are working)
                IPAddress selectedAddress = IPAddress.Parse("66.94.117.48"); //81.83.45.130
                Console.WriteLine($"IP {selectedAddress}");

                //LD defining the peer endpoint
                var peerEndPoint = new IPEndPoint(selectedAddress, 8333);

                using (var node = Node.Connect(Network.Main, peerEndPoint))
                {
                    //LD after connection to bitcoin node, here I'm subscribing to any node state changes. Logging for monitoring events
                    node.StateChanged += (sender, state) =>
                    {
                        Console.WriteLine($"Node State: {state}");
                    };
                    //LD Subscribing to message received event
                    node.MessageReceived += async (sender, e) =>
                    {
                        // Handling inventory payload messages
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

                                //LD if transaction, send event to clients
                                if (item.Type.HasFlag(InventoryType.MSG_TX))
                                    {
                                        await _hubContext.Clients.All.SendAsync("ReceiveTransactionEvent", eventType.ToString(), item.Hash.ToString());
                                    }
                                //LD if block, request block data
                                if (item.Type.HasFlag(InventoryType.MSG_BLOCK))
                                {
                                    node.SendMessage(new GetDataPayload(item));
                                }
                            }
                        }
                        //LD Handling block payload header and transactions content as per CA3 request. 
                        else if (e.Message.Payload is BlockPayload blockPayload)
                        {
                            BlockHeader header = blockPayload.Object.Header;

                            DateTimeOffset timestamp = header.BlockTime;
                            Console.WriteLine($"Block Timestamp: {timestamp}");

                            uint nonce = header.Nonce;
                            Console.WriteLine($"Nonce: {nonce}");

                            double difficulty = header.Bits.Difficulty;
                            Console.WriteLine($"Difficulty: {difficulty}");

                            //LD validating that the hash matches the hash included in the block
                            uint256 computedHash = header.GetHash();
                            uint256 blockHash = blockPayload.Object.GetHash();
                            bool hashVerification = computedHash == blockHash;
                            Console.WriteLine(hashVerification ? "Hash verified" : "Hash not verified");

                            //LD handling transactions within the block
                            Block block = blockPayload.Object;
                            var transactions = block.Transactions;

                            //LD generating a Json string with block data. Details in "CReateBlockData.cs"
                            var jsonStringData = CreateBlockData.GenerateJsonString(timestamp, transactions.ToArray(), nonce, difficulty, hashVerification, blockHash);

                            //LD sending block event to clients
                            await _hubContext.Clients.All.SendAsync("ReceiveBlockEvent", jsonStringData);
                        }
                        else
                        {
                            Console.WriteLine($"LD OTHER Received message: {e.Message.Payload}");
                            var receivedMessage = e.Message.Payload.GetType().Name;
                            Console.WriteLine($"LD OTHER Received message type: {receivedMessage}");
                        }
                    };

                    node.VersionHandshake();

                    //LD this is key: logic is waiting indefinitely or until cancellation is requested(from API)
                    await Task.Delay(Timeout.Infinite, cancellationToken);

                    return Ok("Node info request stopped");
                }
            }
            catch (Exception ex)
            {
                //LD all the logic is in try catch, just doing a basic handling of exceptions and in case returning internal server error
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}