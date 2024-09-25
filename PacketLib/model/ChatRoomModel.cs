using messenger.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLib.model
{
    [Serializable]
    public class ChatRoomModel
    {
        public ChatRoomListItemModel chatRoomInfo { get; set; }

        public List<string> chatters { get; set; }
        public List<ChatModel> chatHistory { get; set; }

    }
}
