using Newtonsoft.Json;
using NBitcoin;
using System;

namespace ss3_back.Helpers
{
    public class CreateBlockData
    {
        /// <summary>
        /// LD scope of this method is to generate a JSON string representing block data. Will add descriptions of what the code is doing inside the method.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="transactions"></param>
        /// <param name="nonce"></param>
        /// <param name="difficulty"></param>
        /// <param name="hashVerification"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static string GenerateJsonString(DateTimeOffset timestamp, Transaction[] transactions, uint nonce, double difficulty, bool hashVerification, uint256 hash)
        {
            //LD "blockData" is an anonymous object to hold the block data. (as the name says :D )
            var blockData = new
            {
                //LD formatting the timestamp as per CA3 request
                Timestamp = timestamp.ToString("dd/MM/yyyy HH:mm:ss") + " (UTC - Block Timestamp)",
                //LD processing each transaction in the block
                Transactions = transactions.Select(tx =>
                {
                    //LD Calculating total value of the transaction in bitcoins
                    decimal totalValue = tx.Outputs.Sum(output => output.Value.ToDecimal(MoneyUnit.BTC));
                    return new
                    {
                        TransactionId = tx.GetHash().ToString(),
                        TotalValue = totalValue.ToString(),
                        //LD needed this helper method to process raw transaction data into a readable format. Details in "ProcessRawTransactionData.cs"
                        TransactionRaw = ProcessRawTransactionData.ProcessRawTransactionDataMethod(tx.ToBytes())
                };
                }).ToList(),
                Nonce = nonce.ToString(),
                Difficulty = difficulty.ToString(),
                HashVerification = hashVerification.ToString(),
                Hash = hash.ToString()
            };
            //LD serializing the block data object to a formatted JSON string
            return JsonConvert.SerializeObject(blockData, Formatting.Indented);
        }
    }
}
