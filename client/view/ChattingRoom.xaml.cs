﻿using Client.viewmodel;
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

namespace Client.view
{
    public partial class ChattingRoom : Window
    {
        private ChattingRoomViewModel viewmodel;

        public ChattingRoom(ChatRoomModel chatRoomModel)
        {
            InitializeComponent();

            viewmodel = new ChattingRoomViewModel(chatRoomModel);

            lv_chathistory.ItemsSource = viewmodel.ChatHistory;
        }


    }
}
