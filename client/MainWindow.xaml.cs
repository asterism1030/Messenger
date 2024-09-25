using messenger.model;
using messenger.utility;
using messenger.viewmodel;
using PacketLib;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tcpip;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatRoomListViewModel viewmodel = new ChatRoomListViewModel();

        public MainWindow()
        {
            InitializeComponent();

            TcpIp.Instance.PacketReceived += OnPacketReceived;

            lv_chatrooms.ItemsSource = viewmodel.ChatRoomList;
        }

        private void btn_create_chat_Click(object sender, RoutedEventArgs e)
        {
            // Test
            TcpIp.Instance.SendPacket(Command.TYPE.REQUEST_CHATROOM_LIST);
        }




        private void OnPacketReceived(Packet packet)
        {
            if (packet.Command == (int)Command.TYPE.REQUEST_CHATROOM_LIST)
            {
                Dispatcher.Invoke(() => {
                    viewmodel.UpdateChatRoomList((List<ChatRoomListItemModel>)packet.Data);
                });
            }
        }
    }
}
