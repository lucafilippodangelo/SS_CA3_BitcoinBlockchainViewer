using NBitcoin;
using Newtonsoft.Json;
using ss3_back.Helpers;
using System;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace ss3_back.UnitTests
{
    public class CreateBlockDataTests
    {
        private Transaction[] CreateTransactions()
        {
            var transactions = new List<Transaction>();

            // Create dummy transactions with specific properties
            var tx1 = Transaction.Create(Network.Main);
            tx1.Outputs.Add(new TxOut(Money.Coins(0.5678m), new Key().PubKey.Hash));
            tx1.Outputs.Add(new TxOut(Money.Coins(0.6789m), new Key().PubKey.Hash));
            // Set transaction ID
            //tx1.Inputs.Add(new TxIn(new OutPoint(new uint256("a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1"), 0)));
            transactions.Add(tx1);

            var tx2 = Transaction.Create(Network.Main);
            tx2.Outputs.Add(new TxOut(Money.Coins(1.2345m), new Key().PubKey.Hash));
            tx2.Outputs.Add(new TxOut(Money.Coins(1.1111m), new Key().PubKey.Hash));
            // Set transaction ID
            //tx2.Inputs.Add(new TxIn(new OutPoint(new uint256("b1c2d3e4f5g6b7c8d9e0f1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0"), 0)));
            transactions.Add(tx2);

            return transactions.ToArray();
        }

        [Fact]
        public void GenerateJson_ReturnsValidJson()
        {
            DateTimeOffset timestamp = new DateTimeOffset(2024, 4, 20, 10, 36, 22, TimeSpan.Zero);
            var transactions = CreateTransactions();
            uint nonce = 123;
            double difficulty = 1.0;
            bool hashVerification = true;

            var expectedJson = @"{
                              ""Timestamp"": ""2024-04-20T10:36:22+00:00"",
                              ""Transactions"": [
                                {
                                  ""TransactionId"": ""a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2e3f4a5b6c7d8e9f0a1"",
                                  ""TotalValue"": ""1.2345"",
                                  ""Outputs"": [
                                    {
                                      ""Address"": ""dummy_address_1"",
                                      ""Value"": ""0.5678""
                                    },
                                    {
                                      ""Address"": ""dummy_address_2"",
                                      ""Value"": ""0.6789""
                                    }
                                  ]
                                },
                                {
                                  ""TransactionId"": ""b1c2d3e4f5g6b7c8d9e0f1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0"",
                                  ""TotalValue"": ""2.3456"",
                                  ""Outputs"": [
                                    {
                                      ""Address"": ""dummy_address_3"",
                                      ""Value"": ""1.2345""
                                    },
                                    {
                                      ""Address"": ""dummy_address_4"",
                                      ""Value"": ""1.1111""
                                    }
                                  ]
                                }
                              ],
                              ""Nonce"": ""123"",
                              ""Difficulty"": ""1.0"",
                              ""HashVerification"": ""True""
                            }";

            byte[] randomBytes = RandomUtils.GetBytes(32);
            uint256 randomUint256 = new uint256(randomBytes);


            var jsonData = CreateBlockData.GenerateJsonString(timestamp, transactions, nonce, difficulty, hashVerification, randomUint256);


            Assert.NotNull(jsonData);
            Assert.Equal(expectedJson, jsonData);
        }
    }

}


