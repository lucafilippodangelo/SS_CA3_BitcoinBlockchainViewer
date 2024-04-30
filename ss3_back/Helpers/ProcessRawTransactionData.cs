using System.Text;

namespace ss3_back.Helpers
{
    public class ProcessRawTransactionData
    {
        public static void ProcessRawTransactionDataMethod(byte[] rawTransactionData)
        {
            using (MemoryStream ms = new MemoryStream(rawTransactionData))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    //LD LEFT?
                    /*
                        The basic gist is all messages sent over the network use a specific datagram format, consisting of:
                        • The magic number 0xD9B4BEF9, encoded in little endian format, i.e. as 0xF9BEB4D9
                        • 12 bytes, indicating the type or (command) of transaction being send, with zeros as padding
                        at the end if the type is less than 12 bytes long
                        • The length of the payload in bytes, encoded as a 32-bit value
                        • A checksum, the first four bytes of the double SHA256 hash of the payload (the double
                        SHA256 of a value is the SHA256 of the SHA256 of this value!)
                        • The payload itself
                     */

                    //LD Magic Number?
                    byte[] magicNumberBytes = reader.ReadBytes(4);
                    Array.Reverse(magicNumberBytes); // Convert to little-endian
                    uint magicNumber = BitConverter.ToUInt32(magicNumberBytes, 0);
                    Console.WriteLine("LD magic something: " + magicNumber);

                    //LD command
                    byte[] commandBytes = reader.ReadBytes(12);
                    StringBuilder commandBuilder = new StringBuilder();
                    Console.WriteLine("LD Exadecimal " + BitConverter.ToString(commandBytes)); //hexadecimal format

                    //LD Length of Payload
                    uint payloadLength = reader.ReadUInt32();
                    Console.WriteLine("LD Payload Length: " + payloadLength);

                    //LD Checksum
                    byte[] checksumBytes = reader.ReadBytes(4);
                    Console.WriteLine("LD Checksum: " + BitConverter.ToString(checksumBytes));

                    //LD Payload - TOO BIG GETTING STUCK
                    //int bytesRemaining = (int)payloadLength;
                    //while (bytesRemaining > 0)
                    //{
                    //    int chunkSize = Math.Min(bytesRemaining, 4096); //4096 bytes per time
                    //    byte[] payloadChunk = reader.ReadBytes(chunkSize);
                    //    Console.WriteLine("LD Payload Chunk: " + BitConverter.ToString(payloadChunk));
                    //    bytesRemaining -= chunkSize;
                    //}


                }
            }
        }
    }
}
