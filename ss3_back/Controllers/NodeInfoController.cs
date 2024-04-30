using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NBitcoin;
using NBitcoin.Crypto;
using NBitcoin.DataEncoders;
using NBitcoin.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ss3_back.Helpers;
using ss3_back.SignalR;
using System;
using System.Net;
using System.Numerics;
using System.Text;
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

                //LD SIMULATION SENDING A BLOCK EVENT *****************************************************
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
                                                        TransactionId = "740ca1eb216960deb30fcfd5e81722b77fda91471313accf5f27aa145536b747",
                                                        TotalValue = "0.00093022",
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
                //LD END SIMULATION SENDING A BLOCK EVENT *************************************************


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
                                   
                                    //LD the below should request raw transaction data FOR EACH INCOMING TRANSACTION
                                    //node.SendMessage(new GetDataPayload(item));

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

                            //LD Transactions
                            Block block = blockPayload.Object;
                            var transactions = block.Transactions;

                            var jsonData = CreateBlockData.GenerateJson(timestamp, transactions.ToArray(), nonce, difficulty, hashVerification, blockHash);

                            //LD Send block
                            await _hubContext.Clients.All.SendAsync("ReceiveBlockEvent", jsonData);

                        }
                        //else if (e.Message.Payload is TxPayload txPayload)
                        //{

                        //    NBitcoin.Transaction transaction = txPayload.Object;
                        //    byte[] rawTransactionData = transaction.ToBytes();

                        //    //LD TEST, TRY TO PROCESS RAW DATA FOR EACH TRANSACTION?
                        //    string transactionJson = ProcessRawTransactionData.ProcessRawTransactionDataMethod(rawTransactionData);

                        //}
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