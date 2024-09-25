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


            
        }

    }
}