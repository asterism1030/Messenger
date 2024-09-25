using PacketLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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


        // TODO) 에러 수정
        public static byte[] StreamToByteArry(NetworkStream stream)
        {
            if (stream == null)
            {
                return null;
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                /*
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytesRead);
                }
                */

                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }


    }





    sealed class PreMergeToMergedDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = null;

            //String exeAssembly = Assembly.GetExecutingAssembly().FullName;
            String exeAssembly = "PacketLib";


            typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, exeAssembly));

            return typeToDeserialize;
        }
    }
}
