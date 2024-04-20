using NBitcoin;
using Newtonsoft.Json;
using ss3_back.Helpers;
using Xunit;

namespace ss3_back.UnitTests
{
    public class CreateBlockDataTests
    {
        private Transaction[] CreateTransactions()
        {

            var transactions = new List<Transaction>();

            var tx1 = Transaction.Create(Network.Main);
            transactions.Add(tx1);

            var tx2 = Transaction.Create(Network.Main);
            transactions.Add(tx2);

            return transactions.ToArray();
        }

        [Fact]
        public void GenerateJson_ReturnsValidJson()
        {
            DateTimeOffset timestamp = DateTimeOffset.UtcNow;
            var transactions = CreateTransactions();
            uint nonce = 123;
            double difficulty = 1.0;
            bool hashVerification = true;

            var jsonData = CreateBlockData.GenerateJson(timestamp, transactions, nonce, difficulty, hashVerification);

            Assert.NotNull(jsonData);

        }

    }


}


