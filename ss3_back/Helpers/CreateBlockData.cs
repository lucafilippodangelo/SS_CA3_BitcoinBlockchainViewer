using Newtonsoft.Json;
using NBitcoin;
using System;

namespace ss3_back.Helpers
{
    public class CreateBlockData
    {
        public static string GenerateJson(DateTimeOffset timestamp, Transaction[] transactions, uint nonce, double difficulty, bool hashVerification)
        {
            var blockData = new
            {
                Timestamp = timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz"),
                Transactions = transactions.Select(tx =>
                {
                    // Calculate total value for the transaction
                    decimal totalValue = tx.Outputs.Sum(output => output.Value.ToDecimal(MoneyUnit.BTC));

                    return new
                    {
                        TransactionId = tx.GetHash().ToString(),
                        TotalValue = totalValue.ToString() // Include total value in the transaction
                    };
                }).ToList(),
                Nonce = nonce.ToString(),
                Difficulty = difficulty.ToString(),
                HashVerification = hashVerification.ToString()
            };

            return JsonConvert.SerializeObject(blockData);
        }
    }
}

//using NBitcoin;
//using Newtonsoft.Json;

//namespace ss3_back.Helpers
//{
//    public class CreateBlockData
//    {
//        public static string GenerateJson(DateTimeOffset timestamp, Transaction[] transactions, uint nonce, double difficulty, bool hashVerification)
//        {
//            var blockData = new
//            {
//                Timestamp = timestamp,
//                Transactions = transactions.Select(tx =>
//                {
//                    var outputs = tx.Outputs.Select(output =>
//                    {
//                        string address;
//                        var destination = output.ScriptPubKey.GetDestination();

//                        if (destination != null)
//                        {
//                            // If scriptPubKey corresponds to a standard address format
//                            address = destination.GetAddress(Network.Main).ToString();
//                        }
//                        else
//                        {
//                            // Handle other types of scriptPubKeys here
//                            address = "Non-standard script";
//                        }

//                        return new
//                        {
//                            Address = address,
//                            Value = output.Value.ToDecimal(MoneyUnit.BTC).ToString()
//                        };
//                    }).ToList();

//                    return new
//                    {
//                        TransactionId = tx.GetHash().ToString(),
//                        TotalValue = tx.TotalOut.ToDecimal(MoneyUnit.BTC).ToString(),
//                        Outputs = outputs
//                    };
//                }).ToList(),
//                Nonce = nonce.ToString(),
//                Difficulty = difficulty.ToString(),
//                HashVerification = hashVerification.ToString()
//            };

//            return JsonConvert.SerializeObject(blockData);
//        }
//    }
//}
