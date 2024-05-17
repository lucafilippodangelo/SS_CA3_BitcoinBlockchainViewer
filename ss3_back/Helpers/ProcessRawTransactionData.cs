using Newtonsoft.Json.Linq;
using System.Text;

namespace ss3_back.Helpers
{
    public class ProcessRawTransactionData
    {
        /// <summary>
        /// LD this method parses raw transaction data and extracts: magic number, command, payload length, 
        /// payload(first 20 bytes for performance reasons) and Checksum. Details:
        /// magic number: it's fixed 4-byte value used to identify the Bitcoin network. I'm getting rid of "-" and converting to little-endian format.
        /// command:12-byte ASCII string specifying the type of message being sent ("version", "tx").
        /// payload length: this is a 4-byte unsigned integer indicating the size of the payload in bytes. I'm returning to Front end an INT representing the number of bytes
        /// checksum: 4 bytes to provide a checksum of the payload to ensure data integrity.
        /// payload: after parsing I'm returning the first 
        /// </summary>
        /// <param name="rawTransactionData">byte array containing raw transaction data.</param>
        /// <returns>JObject containing parsed transaction data.</returns>
        public static JObject ProcessRawTransactionDataMethod(byte[] rawTransactionData)
        {
            Dictionary<string, object> transactionData = new Dictionary<string, object>();

            using (MemoryStream ms = new MemoryStream(rawTransactionData))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    //LD Magic Number
                    byte[] magicNumberBytes = reader.ReadBytes(4);
                    //string magicNumberNotReversed = " 0x" + BitConverter.ToString(magicNumberBytes).Replace("-", string.Empty);
                    Array.Reverse(magicNumberBytes);
                    //uint magicNumber = BitConverter.ToUInt32(magicNumberBytes, 0);
                    string magicNumberLittleEndian = " 0x" + BitConverter.ToString(magicNumberBytes).Replace("-", string.Empty);
                    transactionData.Add("Magic Number: ", magicNumberLittleEndian);

                    //LD Command
                    byte[] commandBytes = reader.ReadBytes(12);
                    StringBuilder commandBuilder = new StringBuilder();
                    //string commandTemp = Encoding.ASCII.GetString(commandBytes).Trim('\0');
                    string command = BitConverter.ToString(commandBytes);
                    transactionData.Add("Command(bytes separated by hyphens): ", command);

                    //LD Length of Payload (think is better returning an int with the number of bytes, more readable than the size in bytes)
                    uint payloadLength = reader.ReadUInt32();
                    transactionData.Add("Payload Length: ", payloadLength+" bytes");

                    //LD Checksum
                    byte[] checksumBytes = reader.ReadBytes(4);
                    transactionData.Add("Checksum(bytes separated by hyphens)", BitConverter.ToString(checksumBytes));

                    //LD Payload 
                    int bytesToRead = (int)Math.Min(payloadLength, 20); 
                    byte[] payload = reader.ReadBytes(bytesToRead);
                    transactionData.Add("Payload (bytes separated by hyphens, first 20 bytes displayed): ", BitConverter.ToString(payload));

                    //LD Payload Full(commented, we interactiong with kilobytes.)
                    /*
                    int bytesRemaining = (int)payloadLength;
                    while (bytesRemaining > 0)
                    {
                        int chunkSize = Math.Min(bytesRemaining, 4096); //4096 bytes per time
                        byte[] payloadChunk = reader.ReadBytes(chunkSize);
                        Console.WriteLine("LD TEST Payload Chunk: " + BitConverter.ToString(payloadChunk));
                        bytesRemaining -= chunkSize;
                    }
                    */

                }
            }

            return JObject.FromObject(transactionData);
        }

    }
}
