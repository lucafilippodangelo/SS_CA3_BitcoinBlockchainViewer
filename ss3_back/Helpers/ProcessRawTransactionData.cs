using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ss3_back.Helpers
{
    public class ProcessRawTransactionData
    {
        /// <summary>
        /// MagicNumber:
        ///The MagicNumber is a fixed 4 - byte(32 - bit) value used to identify the Bitcoin network.For the main network, this value 
        ///is 0xD9B4BEF9.It helps ensure that nodes are communicating on the correct network.It is measured in bytes.
        ///Command:
        ///The Command field is a 12 - byte(96 - bit) ASCII string that specifies the type of message being sent(e.g., "version", 
        ///"verack", "tx", "block").It is null - padded if the command is less than 12 bytes.It is measured in bytes.
        ///PayloadLength:
        ///The PayloadLength is a 4 - byte(32 - bit) unsigned integer that specifies the size of the payload(the actual data) 
        ///in bytes.This indicates how many bytes of data follow the header.It is measured in bytes.
        ///Checksum:
        ///The Checksum is a 4 - byte(32 - bit) field that provides a checksum of the payload to ensure data integrity.It is calculated 
        ///as the first 4 bytes of the double SHA - 256 hash of the payload.It is measured in bytes.
        /// </summary>
        /// <param name="rawTransactionData"></param>
        /// <returns></returns>
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

                    //LD Length of Payload
                    uint payloadLength = reader.ReadUInt32();
                    transactionData.Add("Payload Length: ", payloadLength+" bytes");

                    //LD Checksum
                    byte[] checksumBytes = reader.ReadBytes(4);
                    transactionData.Add("Checksum(bytes separated by hyphens)", BitConverter.ToString(checksumBytes));

                    //LD Payload 
                    int bytesToRead = (int)Math.Min(payloadLength, 20); 
                    byte[] payload = reader.ReadBytes(bytesToRead);
                    transactionData.Add("Payload (bytes separated by hyphens, first 20 bytes displayed): ", BitConverter.ToString(payload));

                    //LD Payload |Full(commented, we interactiong with kilobytes.)
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
