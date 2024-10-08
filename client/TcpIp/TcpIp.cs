﻿using messenger;
using messenger.designPattern;
using messenger.model;
using messenger.utility;
using PacketLib;
using PacketLib.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Documents;
using utility;

namespace tcpip
{
    public class TcpIp : Singleton<TcpIp>
    {
        public event Action<Packet> PacketReceived;


        private TcpClient client;
        private NetworkStream stream;

        public TcpClient Client { get; }
        public NetworkStream Stream { get; }


        protected TcpIp()
        {
            client = new TcpClient(IpPort.SERVER_IP, IpPort.SERVER_PORT);
            stream = client.GetStream();
        }

        public void Start()
        {
            Thread receiveThread = new Thread(() => ReceivePacket());
            receiveThread.SetApartmentState(ApartmentState.STA);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Debug.WriteLine("서버 연결");

            
        }


        public void Close()
        {
            stream.Close();
            client.Close();
        }


        public void SendPacket(Command.CLIENT command, Object data = null)
        {
            var packet = new Packet
            {
                Command = (int)command,
                Data = data
            };

            byte[] requestData = Converting.PacketToByteArray(packet);
            stream.Write(requestData, 0, requestData.Length);
        }


        public void ReceivePacket()
        {
            byte[] buffer = new byte[51200];

            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    
                    // 서버 연결 종료
                    if (bytesRead == 0) { 
                        break; 
                    }

                    Packet receivedPacket = Converting.ByteArrayToPacket(buffer.Take(bytesRead).ToArray());

                    PacketReceived?.Invoke(receivedPacket);


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(MethodBase.GetCurrentMethod() + " : " + ex.Message);
                    break;
                }
            }
        }
    }
}


