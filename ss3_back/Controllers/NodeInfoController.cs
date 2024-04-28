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
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ss3.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class NodeInfoController : ControllerBase
    {
        static BigInteger GenerateRandomUInt256BlockHash()
        {
            Random rand = new Random();
            byte[] bytes = new byte[32];
            rand.NextBytes(bytes);
            BigInteger blockHash = new BigInteger(bytes);

            return blockHash;
        }

        class Transaction
        {
            public string TransactionId { get; set; }
            public double TotalValue { get; set; }
        }

        static List<Transaction> GenerateRandomTransactions(int count)
        {
            Random random = new Random();
            List<Transaction> transactions = new List<Transaction>();

            for (int i = 0; i < count; i++)
            {
                string transactionId = i + " - " + Guid.NewGuid().ToString("N");
                double totalValue = random.NextDouble() * 100; // Generate random total value

                Transaction transaction = new Transaction
                {
                    TransactionId = transactionId,
                    TotalValue = totalValue
                };

                transactions.Add(transaction);
            }

            return transactions;
        }






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
                //LD DYNAMIC IP
                //IPAddress[] addresses = Dns.GetHostAddresses("seed.bitcoin.sipa.be");
                //Random random = new Random();
                //IPAddress selectedAddress = addresses[random.Next(addresses.Length)];
                //Console.WriteLine($" 000 IP {selectedAddress}");

                ///*
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

                                //LD if transaction
                                if (item.Type.HasFlag(InventoryType.MSG_TX))
                                {
                                    await _hubContext.Clients.All.SendAsync("ReceiveTransactionEvent", eventType.ToString(), item.Hash.ToString());
                                }
                                
                                //LD if block 
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

                            var jsonData = CreateBlockData.GenerateJson(timestamp, transactions.ToArray(), nonce, difficulty, hashVerification, blockHash);

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
                //*/



            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}