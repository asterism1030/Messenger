using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace tcpip
{
    [Serializable]
    public class Packet
    {
        public int Command { get; set; }
        public object Data { get; set; }

        
    }

}
