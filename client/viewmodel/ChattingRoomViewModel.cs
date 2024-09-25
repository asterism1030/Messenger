using messenger.model;
using PacketLib.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.viewmodel
{
    public class ChattingRoomViewModel : INotifyPropertyChanged
    {
        private ChatRoomListItemModel chatRoomInfo;
        private List<string> chatters;

        private ObservableCollection<ChatModel> chatHistory = new ObservableCollection<ChatModel>();
        public ObservableCollection<ChatModel> ChatHistory
        {
            get { return chatHistory; }
            set
            {
                chatHistory = value;
                OnPropertyChanged(nameof(chatHistory));
            }
        }


        public ChattingRoomViewModel(ChatRoomModel chatRoomModel)
        {
            if(chatRoomModel == null)
            {
                return;
            }

            chatRoomInfo = chatRoomModel.chatRoomInfo;
            chatters = chatRoomModel.chatters;

            foreach(var chat in chatRoomModel.chatHistory)
            {
                chatHistory.Add(chat);
            }
        }









        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
