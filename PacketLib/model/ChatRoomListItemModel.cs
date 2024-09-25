using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace messenger.model
{
    [Serializable]
    public class ChatRoomListItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Creater { get; set; }
        public int Cnt { get; set; }
    }
}
