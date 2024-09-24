using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using tcpip;

namespace utility
{
    public class Converting
    {
        public static byte[] PacketToByteArray(Packet packet)
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, packet);

                return memoryStream.ToArray();
            }
        }


        public static Packet ByteArrayToPacket(byte[] byteArray)
        {
            using (var memoryStream = new MemoryStream(byteArray))
            {
                var formatter = new BinaryFormatter();

                return (Packet)formatter.Deserialize(memoryStream);
            }
        }

    }
}
