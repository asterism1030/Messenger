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
            // tcp/ip
            TcpIp.Instance.Start();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // tcp/ip
            TcpIp.Instance.SendPacket(Command.CLIENT.EXIT_APP);
        }

    }
}