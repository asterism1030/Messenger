using System.Configuration;
using System.Data;
using System.Windows;
using tcpip;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            TcpIp.Instance.Start();
        }

    }
}