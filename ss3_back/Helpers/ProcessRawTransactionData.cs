using Newtonsoft.Json;
using System.Text;

namespace ss3_back.Helpers
{
    public class ProcessRawTransactionData
    {
        public static string ProcessRawTransactionDataMethod(byte[] rawTransactionData)
    {
        Dictionary<string, object> transactionData = new Dictionary<string, object>();

        using (MemoryStream ms = new MemoryStream(rawTransactionData))
        {
            using (BinaryReader reader = new BinaryReader(ms))
            {
                //LD Magic Number
                byte[] magicNumberBytes = reader.ReadBytes(4);
                Array.Reverse(magicNumberBytes);
                uint magicNumber = BitConverter.ToUInt32(magicNumberBytes, 0);
                transactionData.Add("MagicNumber", magicNumber);

                //LD Command
                byte[] commandBytes = reader.ReadBytes(12);
                //string command = Encoding.ASCII.GetString(commandBytes).Trim('\0');
                
                //LD Command (another approach) Hexadecimal
                StringBuilder commandBuilder = new StringBuilder();
                string command = BitConverter.ToString(commandBytes);
                transactionData.Add("Command", command);

                //LD Length of Payload
                uint payloadLength = reader.ReadUInt32();
                transactionData.Add("PayloadLength", payloadLength);

                //LD Checksum
                byte[] checksumBytes = reader.ReadBytes(4);
                transactionData.Add("Checksum", BitConverter.ToString(checksumBytes));

                //LD Payload
                // int bytesRemaining = (int)payloadLength;
                // while (bytesRemaining > 0)
                // {
                //     int chunkSize = Math.Min(bytesRemaining, 4096); //4096 bytes per time
                //     byte[] payloadChunk = reader.ReadBytes(chunkSize);
                //     Console.WriteLine("LD Payload Chunk: " + BitConverter.ToString(payloadChunk));
                //     bytesRemaining -= chunkSize;
                // }
            }
        }

        string json = JsonConvert.SerializeObject(transactionData, Formatting.Indented);
        return json;
    }
    }
}
