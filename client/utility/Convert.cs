using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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

                formatter.Binder = new PreMergeToMergedDeserializationBinder();
                object obj = formatter.Deserialize(memoryStream);

                memoryStream.Close();

                return (Packet)obj;
            }
        }

    }



    sealed class PreMergeToMergedDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = null;

            String exeAssembly = "Server";
            //String exeAssembly = Assembly.GetExecutingAssembly().FullName;


            typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, exeAssembly));

            return typeToDeserialize;
        }
    }
}
