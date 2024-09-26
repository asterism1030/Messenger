using Client.viewmodel;
using messenger.model;
using messenger.utility;
using PacketLib;
using PacketLib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using tcpip;

namespace Client.view
{
    public partial class ChattingRoom : Window
    {
        private ChattingRoomViewModel viewmodel;

        public ChattingRoom(ChatRoomModel chatRoomModel, string nickName)
        {
            InitializeComponent();
            
            // event
            TcpIp.Instance.PacketReceived += OnPacketReceived;

            // data
            viewmodel = new ChattingRoomViewModel(chatRoomModel, nickName);

            lv_chathistory.ItemsSource = viewmodel.ChatHistory;

            // ui
            if(lv_chathistory.Items.Count > 0)
            {
                lv_chathistory.ScrollIntoView(lv_chathistory.Items[lv_chathistory.Items.Count - 1]);
            }
            
            this.Title = chatRoomModel.chatRoomInfo.Name;
        }


        private void tb_message_KeyDown(object sender, KeyEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tb_message.Text) || e.Key != System.Windows.Input.Key.Enter)
            {
                return;
            }

            // tcp/ip packet
            ChatIdModel chat = new ChatIdModel();
            chat.id = viewmodel.ChatRoomInfo.Id;
            chat.chatterName = viewmodel.UserNickName;
            chat.content = tb_message.Text;


            ChatModel chatModel = new ChatModel();
            chatModel.chatterName = chat.chatterName;
            chatModel.content = chat.content;

            TcpIp.Instance.SendPacket(Command.CLIENT.SEND_MESSAGE, chat);


            // ui
            tb_message.Text = "";
        }

        private void lv_chathistory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;

            if (listView.SelectedItem == null)
            {
                return;
            }

            // ui
            ChatModel chatModel = (ChatModel)listView.SelectedItem;
            Clipboard.SetText(chatModel.content);

            MessageBox.Show("복사 되었습니다.");
        }


        private void OnPacketReceived(Packet packet)
        {
            if (packet.Command == (int)Command.SERVER.SEND_MESSAGE)
            {
                ChatModel chat = (ChatModel)packet.Data;

                Dispatcher.Invoke(() => {
                    viewmodel.ChatHistory.Add(chat);

                    lv_chathistory.ScrollIntoView(lv_chathistory.Items[lv_chathistory.Items.Count - 1]);
                });

            }
        }

        
    }
}
