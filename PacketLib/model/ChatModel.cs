using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLib.model
{
    [Serializable]
    public class ChatModel
    {
        public string chatterName { get; set; } // 대상 아이디
        public string content { get; set; } // 대화

    }

    [Serializable]
    public class ChatIdModel
    {
        public int id { get; set; } // 채팅방 아이디
        public string chatterName { get; set; } // 대상 아이디
        public string content { get; set; } // 대화

    }
}
