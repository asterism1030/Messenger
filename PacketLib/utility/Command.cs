using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace messenger.utility
{
    public static class Command
    {
        public enum TYPE {
            REQUEST_CHATROOM_LIST,
            REQUEST_CHATROOM_ENTER,
            
            SEND_MESSAGE,
            SEND_CREATEROOM,
        }

    }
}
