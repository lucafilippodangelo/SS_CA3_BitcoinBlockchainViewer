using NBitcoin;
using Newtonsoft.Json;

namespace ss3_back.Helpers
{
    public class CreateBlockData
    {
        public static string GenerateJson(DateTimeOffset timestamp, Transaction[] transactions, uint nonce, double difficulty, bool hashVerification)
        {
            var blockData = new
            {
                Timestamp = timestamp,
                Transactions = transactions.Select(tx =>
                {
                    var outputs = tx.Outputs.Select(output => new
                    {
                        Address = output.ScriptPubKey.GetDestinationAddress(Network.Main),
                        Value = output.Value.ToDecimal(MoneyUnit.BTC)
                    }).ToList();

                    return new
                    {
                        TransactionId = tx.GetHash().ToString(), //LD forcing conversion to string
                        TotalValue = tx.TotalOut.ToDecimal(MoneyUnit.BTC),
                        Outputs = outputs
                    };
                }).ToList(),
                Nonce = nonce,
                Difficulty = difficulty,
                HashVerification = hashVerification
            };

            return JsonConvert.SerializeObject(blockData);
        }
    }
}
