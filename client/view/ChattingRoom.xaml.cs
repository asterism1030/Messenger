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
using tcpip;

namespace Client.view
{
    public partial class ChattingRoom : Window
    {
        private ChattingRoomViewModel viewmodel;

        public ChattingRoom(ChatRoomModel chatRoomModel, string nickName)
        {
            InitializeComponent();

            TcpIp.Instance.PacketReceived += OnPacketReceived;

            viewmodel = new ChattingRoomViewModel(chatRoomModel, nickName);

            lv_chathistory.ItemsSource = viewmodel.ChatHistory;
            this.Title = chatRoomModel.chatRoomInfo.Name;
        }


        private void tb_message_KeyDown(object sender, KeyEventArgs e)
        {
            if(tb_message.Text == "" || tb_message.Text == null)
            {
                return;
            }


            ChatIdModel chat = new ChatIdModel();
            chat.id = viewmodel.ChatRoomInfo.Id;
            chat.chatterName = viewmodel.UserNickName;
            chat.content = tb_message.Text;


            ChatModel chatModel = new ChatModel();
            chatModel.chatterName = chat.chatterName;
            chatModel.content = chat.content;

            viewmodel.ChatHistory.Add(chatModel);


            TcpIp.Instance.SendPacket(Command.CLIENT.SEND_MESSAGE, chat);


            tb_message.Text = "";
        }


        private void OnPacketReceived(Packet packet)
        {
            if (packet.Command == (int)Command.SERVER.SEND_MESSAGE)
            {
                ChatModel chat = (ChatModel)packet.Data;

                Dispatcher.Invoke(() => {
                    viewmodel.ChatHistory.Add(chat);
                });
            }
        }
    }
}
