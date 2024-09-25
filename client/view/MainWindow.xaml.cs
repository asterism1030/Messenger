using Client.view;
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
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewmodel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            TcpIp.Instance.PacketReceived += OnPacketReceived;

            lv_chatrooms.ItemsSource = viewmodel.ChatRoomList;

            // 초기 필요한 데이터 요청
            TcpIp.Instance.SendPacket(Command.CLIENT.REQUEST_CHATROOM_LIST);
        }

        




        


        private void btn_create_chat_Click(object sender, RoutedEventArgs e)
        {

        }


        private void lv_chatrooms_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;

            if(listView.SelectedItem == null)
            {
                return;
            }

            // 채팅방 입장 요청
            ChatRoomListItemModel chatRoom = (ChatRoomListItemModel)listView.SelectedItem;

            TcpIp.Instance.SendPacket(Command.CLIENT.REQUEST_CHATROOM_ENTER, chatRoom.Id);
        }


        private void OnPacketReceived(Packet packet)
        {
            if (packet.Command == (int)Command.SERVER.SEND_CHATROOM_LIST)
            {
                Dispatcher.Invoke(() => {
                    viewmodel.UpdateChatRoomList((List<ChatRoomListItemModel>)packet.Data);
                });
            }
            else if(packet.Command == (int)Command.SERVER.ACCEPT_CHATROOM_ENTER)
            {

                Dispatcher.Invoke(() => {
                    ChattingRoom chattingRoom = new ChattingRoom();
                    chattingRoom.Show();
                });
                
                
            }
        }
    }
}
