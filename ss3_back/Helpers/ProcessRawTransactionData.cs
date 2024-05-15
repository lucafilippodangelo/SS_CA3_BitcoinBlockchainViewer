using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ss3_back.Helpers
{
    public class ProcessRawTransactionData
    {
        public static JObject ProcessRawTransactionDataMethod(byte[] rawTransactionData)
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
                    
                    StringBuilder commandBuilder = new StringBuilder();
                    string command = BitConverter.ToString(commandBytes);


                    string command2 = Encoding.ASCII.GetString(commandBytes).Trim('\0');

                    transactionData.Add("Command", command);

                    //LD Length of Payload
                    uint payloadLength = reader.ReadUInt32();
                    transactionData.Add("PayloadLength", payloadLength);

                    //LD Checksum
                    byte[] checksumBytes = reader.ReadBytes(4);
                    transactionData.Add("Checksum", BitConverter.ToString(checksumBytes));

                    //LD Payload (commented, believe we interactiong with kilobytes. Too slow to process)
                    /*
                    int bytesRemaining = (int)payloadLength;
                    while (bytesRemaining > 0)
                    {
                        int chunkSize = Math.Min(bytesRemaining, 4096); //4096 bytes per time
                        byte[] payloadChunk = reader.ReadBytes(chunkSize);
                        Console.WriteLine("LD Payload Chunk: " + BitConverter.ToString(payloadChunk));
                        bytesRemaining -= chunkSize;
                    }
                    */
                }
            }

            return JObject.FromObject(transactionData);
        }

    }
}
