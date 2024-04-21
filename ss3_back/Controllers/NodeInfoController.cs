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
                ///*
                ///
                DateTimeOffset now = DateTimeOffset.UtcNow;
                string formattedDateTime = now.ToString("dd MMMM yyyy, h\\:mm\\:ss tt zzz") + " " + now.ToString("fffffffzzz").Substring(0, 7) + "+01:00";

                BigInteger blockHash = GenerateRandomUInt256BlockHash();
                string blockHashString = "0x" + blockHash.ToString("X");
                Random random = new Random();
                int randomNumber = random.Next(2500, 12001);

                List<Transaction> transactions = GenerateRandomTransactions(randomNumber);

                dynamic jsonData = new
                {
                    Timestamp = DateTimeOffset.UtcNow.ToString("dd MMMM yyyy, h\\:mm\\:ss tt zzz") + " " + now.ToString("fffffffzzz").Substring(0, 7) + "+01:00",
                    Transactions = transactions,
                    Nonce = "1699485074",
                    Difficulty = "86388558925171.02",
                    HashVerification = true,
                    Hash = blockHashString,
                };

                string jsonString = JsonConvert.SerializeObject(jsonData, Formatting.Indented);


                //string jsonData = @"{
                //       ""Timestamp"":""" + formattedDateTime + @""",
                //       ""Transactions"":[
                //          {
                //             ""TransactionId"":""45950b1628c2dc34a2e00c31cb96ab5fcf15ffed40364aaf972ba71cc865bf5b"",
                //             ""TotalValue"":""10.28637578""
                //          },
                //          {
                //             ""TransactionId"":""04b0a83476f9a452b98873af179e22912537b075a118653b12bd88c8579b1e8f"",
                //             ""TotalValue"":""0.0024609""
                //          },
                //          {
                //             ""TransactionId"":""2360fdf9eac828cc93e678609f7343c1969739bfa4837636f4e71957a0c51f8a"",
                //             ""TotalValue"":""0.00060756""
                //          },
                //          {
                //             ""TransactionId"":""04b0a83476f9a452b98873af179e22912537b075a118653b12bd88c8579b1e8f"",
                //             ""TotalValue"":""0.0024609""
                //          }
                //       ],
                //       ""Nonce"":""1699485074"",
                //       ""Difficulty"":""86388558925171.02"",
                //       ""HashVerification"":""True"",
                //       ""Hash"":""" + blockHashString + @"""
                //    }";

                //// Converting the JSON string to a JObject
                //JObject json = JObject.Parse(jsonData);

                //// Serialize the JObject to a string
                //string jsonString = JsonConvert.SerializeObject(json);

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