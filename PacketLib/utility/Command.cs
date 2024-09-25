using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace messenger.utility
{
    public static class Command
    {
        public enum CLIENT {
            REQUEST_CHATROOM_LIST,
            REQUEST_CHATROOM_ENTER,
            REQUEST_CREATEROOM,
            
            SEND_MESSAGE,
        }

        public enum SERVER
        {
            SEND_CHATROOM_LIST,

            ACCEPT_CHATROOM_ENTER,
            ACCEPT_CREATEROOM,

            SEND_MESSAGE,
        }
    }
}
