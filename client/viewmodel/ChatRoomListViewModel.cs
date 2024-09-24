using messenger.model;
using messenger.utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcpip;

namespace messenger.viewmodel
{
    public class ChatRoomListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ChatRoomListItemModel> chatRoomList = new ObservableCollection<ChatRoomListItemModel>();
        public ObservableCollection<ChatRoomListItemModel> ChatRoomList
        {
            get { return chatRoomList; }
            set { 
                chatRoomList = value;
                OnPropertyChanged(nameof(ChatRoomList));
            }
        }

        public ChatRoomListViewModel() 
        {
            
        }




        public void UpdateChatRoomList(List<ChatRoomListItemModel> list)
        {
            ChatRoomList.Clear();
            foreach (var room in list)
            {
                ChatRoomList.Add(room);
            }
        }





        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
