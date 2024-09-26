using messenger.model;
using messenger.utility;
using PacketLib;
using PacketLib.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using utility;
using static messenger.utility.Command;
using static Server.utility.EnumDefinitions;

namespace tcpip
{
    public class TcpIp 
    {
        private static TcpListener server;

        
        // 클라이언트
        private static List<TcpClient> clients = new List<TcpClient>();



        //// 데이터
        // 채팅방 리스트
        private static List<ChatRoomListItemModel> chatroomsList = new List<ChatRoomListItemModel>();
        // 채팅방 (대화 이력, 정보 등)
        private static Dictionary<int, ChatRoomModel> chatroomDic = new Dictionary<int, ChatRoomModel>();
        




        public static void Start()
        {
            server = new TcpListener(IPAddress.Any, IpPort.SERVER_PORT);
            server.Start();
            Console.WriteLine("서버 시작...");

            while (true)
            {
                // waiting to connect client
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("클라이언트 연결");

                clients.Add(client);

                // thread
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }
        }



        private static void HandleClient(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[51200];

            while (true)
            {

                try
                {
                    // 연결 상태 확인
                    if (client.Client.Poll(1000, SelectMode.SelectRead))
                    {
                        if (client.Client.Available == 0)
                        {
                            break;
                        }
                    }


                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    // byte arry to packet
                    Packet receivedPacket = Converting.ByteArrayToPacket(buffer.Take(buffer.Length).ToArray());

                    // handel packet
                    CLIENT_STATE client_state = HandleCommand(receivedPacket, client, stream);

                    if(client_state == CLIENT_STATE.EXIT)
                    {
                        break;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(MethodBase.GetCurrentMethod() + " : " + ex.Message);
                    break;
                }
            }

            Console.WriteLine("클라이언트 연결 끊김");

            clients.Remove(client);
            client.Close();
        }


        private static void BroadcastMessage(Packet packet, TcpClient sender, List<TcpClient> targetClients)
        {
            byte[] responseData = Converting.PacketToByteArray(packet);

            foreach (TcpClient client in targetClients)
            {
                if (client == sender)
                {
                    continue;
                }

                NetworkStream stream = client.GetStream();
                stream.Write(responseData, 0, responseData.Length);
            }
        }



        private static CLIENT_STATE HandleCommand(Packet receivedPacket, TcpClient client, NetworkStream stream)
        {
            CLIENT_STATE client_state = CLIENT_STATE.REQUEST;


            switch(receivedPacket.Command)
            {
                case (int)Command.CLIENT.REQUEST_CHATROOM_LIST:         // 클라 - 채팅방 리스트 요청
                    {
                        var responsePacket = new Packet
                        {
                            Command = (int)Command.SERVER.SEND_CHATROOM_LIST,
                            Data = chatroomsList
                        };

                        byte[] responseData = Converting.PacketToByteArray(responsePacket);
                        stream.Write(responseData, 0, responseData.Length);

                        break;
                    }
                case (int)Command.CLIENT.REQUEST_CHATROOM_ENTER:        // 클라 - 채팅방 입장 요청
                    {
                        int id = (int)receivedPacket.Data;

                        var responsePacket = new Packet
                        {
                            Command = (int)Command.SERVER.ACCEPT_CHATROOM_ENTER,
                            Data = chatroomDic[id]
                        };

                        byte[] responseData = Converting.PacketToByteArray(responsePacket);
                        stream.Write(responseData, 0, responseData.Length);

                        break;
                    }
                case (int)Command.CLIENT.SEND_MESSAGE:                  // 클라 - 채팅방 메시지 전송
                    {
                        ChatIdModel chatIdModel = (ChatIdModel)receivedPacket.Data;

                        // adding data
                        ChatModel chat = new ChatModel();
                        chat.chatterName = chatIdModel.chatterName;
                        chat.content = chatIdModel.content;

                        chatroomDic[chatIdModel.id].chatHistory.Add(chat);


                        var responsePacket = new Packet
                        {
                            Command = (int)Command.SERVER.SEND_MESSAGE,
                            Data = chatIdModel
                        };


                        BroadcastMessage(responsePacket, null, clients);

                        break;
                    }
                case (int)Command.CLIENT.REQUEST_CHATROOM_CREATE:       // 클라 - 채팅방 생성 요청
                    {
                        ChatRoomModel chatRoomModel = (ChatRoomModel)receivedPacket.Data;
                        chatRoomModel.chatRoomInfo.Id = chatroomDic.Keys.Count;

                        chatroomDic.Add(chatroomDic.Keys.Count, chatRoomModel);

                        var responsePacket = new Packet
                        {
                            Command = (int)Command.SERVER.ACCEPT_CHATROOM_CREATE,
                            Data = chatRoomModel
                        };


                        byte[] responseData = Converting.PacketToByteArray(responsePacket);
                        stream.Write(responseData, 0, responseData.Length);

                        // 채팅방 목록 업데이트 및 브로드캐스트
                        chatroomsList.Add(chatRoomModel.chatRoomInfo);

                        var sendPacket = new Packet
                        {
                            Command = (int)Command.SERVER.SEND_CHATROOM_LIST,
                            Data = chatroomsList
                        };


                        byte[] sendData = Converting.PacketToByteArray(sendPacket);
                        BroadcastMessage(sendPacket, null, clients);

                        break;
                    }
                case (int)Command.CLIENT.EXIT_APP:
                    {
                        client_state = CLIENT_STATE.EXIT;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }


            return client_state;
        }




    }

}
