using Newtonsoft.Json;
using NBitcoin;
using System;

namespace ss3_back.Helpers
{
    public class CreateBlockData
    {
        public static string GenerateJson(DateTimeOffset timestamp, Transaction[] transactions, uint nonce, double difficulty, bool hashVerification, uint256 hash)
        {
            var blockData = new
            {
                Timestamp = timestamp.ToString("dd/MM/yyyy HH:mm:ss") + "(UTC - Block Timestamp)",
                Transactions = transactions.Select(tx =>
                {
                    Console.WriteLine(" ***** LD transaction details :"); //hexadecimal format
                    ProcessRawTransactionData.ProcessRawTransactionDataMethod(tx.ToBytes());

                    //LD Calculate total value for the transaction
                    decimal totalValue = tx.Outputs.Sum(output => output.Value.ToDecimal(MoneyUnit.BTC));

                    return new
                    {
                        TransactionId = tx.GetHash().ToString(),
                        TotalValue = totalValue.ToString() //LD Include total value in the transaction
                    };
                }).ToList(),
                Nonce = nonce.ToString(),
                Difficulty = difficulty.ToString(),
                HashVerification = hashVerification.ToString(),
                Hash = hash.ToString()
            };

            return JsonConvert.SerializeObject(blockData);
        }
    }
}
