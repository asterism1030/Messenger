using Client.view;
using messenger.model;
using messenger.utility;
using messenger.viewmodel;
using PacketLib;
using PacketLib.model;
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

            // event
            TcpIp.Instance.PacketReceived += OnPacketReceived;

            // data
            lv_chatrooms.ItemsSource = viewmodel.ChatRoomList;

            // tcp/ip packet
            TcpIp.Instance.SendPacket(Command.CLIENT.REQUEST_CHATROOM_LIST);
        }


        private void btn_create_chat_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(tb_chat_name.Text) || string.IsNullOrEmpty(tb_nickname.Text))
            {
                return;
            }

            // tcp/ip packet
            ChatRoomModel chatRoomModel = new ChatRoomModel();

            ChatRoomListItemModel chatRoomInfo = new ChatRoomListItemModel();
            chatRoomInfo.Name = tb_chat_name.Text;
            chatRoomInfo.Creater = tb_nickname.Text;
            chatRoomInfo.Cnt = 1;

            List<string> chatters = new List<string>();
            chatters.Add(tb_chat_name.Text);

            chatRoomModel.chatRoomInfo = chatRoomInfo;
            chatRoomModel.chatters = chatters;

            TcpIp.Instance.SendPacket(Command.CLIENT.REQUEST_CHATROOM_CREATE, chatRoomModel);


            // ui
            tb_chat_name.Text = "";
        }


        private void lv_chatrooms_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;

            if(listView.SelectedItem == null)
            {
                return;
            }

            
            // tcp/ip packet
            ChatRoomListItemModel chatRoom = (ChatRoomListItemModel)listView.SelectedItem;
            TcpIp.Instance.SendPacket(Command.CLIENT.REQUEST_CHATROOM_ENTER, chatRoom.Id);      // 채팅방 입장 요청
        }


        private void OnPacketReceived(Packet packet)
        {
            switch(packet.Command)
            {
                case (int)(Command.SERVER.SEND_CHATROOM_LIST):
                    {
                        Dispatcher.Invoke(() => {
                            viewmodel.UpdateChatRoomList((List<ChatRoomListItemModel>)packet.Data);
                        });

                        break;
                    }
                case (int)(Command.SERVER.ACCEPT_CHATROOM_ENTER):
                    {
                        ChatRoomModel chatRoomModel = (ChatRoomModel)packet.Data;

                        Dispatcher.Invoke(() => {
                            ChattingRoom chattingRoom = new ChattingRoom(chatRoomModel, tb_nickname.Text);
                            chattingRoom.Show();
                        });

                        break;
                    }
                case (int)(Command.SERVER.ACCEPT_CHATROOM_CREATE):
                    {
                        ChatRoomModel chatRoomModel = (ChatRoomModel)packet.Data;

                        Dispatcher.Invoke(() => {
                            ChattingRoom chattingRoom = new ChattingRoom(chatRoomModel, tb_nickname.Text);
                            chattingRoom.Show();
                        });

                        break;
                    }
                default:
                    break;

            }
        
        }
    }
}
