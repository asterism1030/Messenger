using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLib
{
    [Serializable]
    public class Packet
    {
        public int Command { get; set; }
        public object Data { get; set; }


    }
}
