using messenger.utility;
using System.Configuration;
using System.Data;
using System.Windows;
using tcpip;

namespace Client
{
    public partial class App : Application
    {
        public App()
        {
            TcpIp.Instance.Start();


            // 초기 필요한 데이터 요청
            TcpIp.Instance.SendPacket(Command.TYPE.REQUEST_CHATROOM_LIST);
        }

    }
}