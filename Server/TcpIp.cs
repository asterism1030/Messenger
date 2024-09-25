using messenger.model;
using messenger.utility;
using PacketLib;
using PacketLib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using utility;
using static messenger.utility.Command;

namespace tcpip
{
    public class TcpIp 
    {

        // 클라이언트
        private static List<TcpClient> clients = new List<TcpClient>();

        // TODO) 사용
        private static Dictionary<string, TcpClient> clientsDic = new Dictionary<string, TcpClient>(); // 닉네임,클라이언트
        
        private static TcpListener server;


        //// 데이터
        // 채팅방 리스트
        private static List<ChatRoomListItemModel> chatroomsList = new List<ChatRoomListItemModel>();
        // 채팅방 (대화 이력, 정보 등)
        private static Dictionary<int, ChatRoomModel> chatroomDic = new Dictionary<int, ChatRoomModel>();
        




        public static void Start()
        {
            server = new TcpListener(IPAddress.Any, IpPort.SERVER_PORT);
            server.Start();
            Console.WriteLine("서버가 시작되었습니다...");

            while (true)
            {
                // 클라이언트 연결 대기
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("클라이언트가 연결되었습니다.");

                // 클라이언트를 목록에 추가
                clients.Add(client);

                // 클라이언트를 처리하는 스레드 시작
                Thread clientThread = new Thread(new ParameterizedThreadStart(ProcessClient));
                clientThread.Start(client);
            }
        }



        private static void ProcessClient(object clientObj)
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
                            Console.WriteLine("클라이언트 연결 끊김");

                            clients.Remove(client);
                            client.Close();

                            break;
                        }
                    }


                    //byte[] buffer = Converting.StreamToByteArry(stream);

                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    // 바이트 배열을 패킷으로 변환
                    Packet receivedPacket = Converting.ByteArrayToPacket(buffer.Take(buffer.Length).ToArray());


                    // Command - 패킷 처리
                    if (receivedPacket.Command == (int)Command.CLIENT.REQUEST_CHATROOM_LIST)  // 클라 - 채팅방 리스트 요청
                    {
                        var responsePacket = new Packet
                        {
                            Command = (int)Command.SERVER.SEND_CHATROOM_LIST,
                            Data = chatroomsList
                        };


                        byte[] responseData = Converting.PacketToByteArray(responsePacket);
                        stream.Write(responseData, 0, responseData.Length);
                    }

                    else if (receivedPacket.Command == (int)(Command.CLIENT.REQUEST_CHATROOM_ENTER)) // 클라 - 채팅방 입장 요청
                    {
                        // TODO) 닉네임 추가하여 수정
                        int id = (int)receivedPacket.Data;

                        var responsePacket = new Packet
                        {
                            Command = (int)Command.SERVER.ACCEPT_CHATROOM_ENTER,
                            Data = chatroomDic[id]
                        };


                        byte[] responseData = Converting.PacketToByteArray(responsePacket);
                        stream.Write(responseData, 0, responseData.Length);
                    }

                    else if (receivedPacket.Command == (int)(Command.CLIENT.SEND_MESSAGE)) // 클라 - 채팅방 메시지 전송
                    {
                        ChatIdModel chatIdModel = (ChatIdModel)receivedPacket.Data;

                        ChatModel chat = new ChatModel();
                        chat.chatterName = chatIdModel.chatterName;
                        chat.content = chatIdModel.content;


                        chatroomDic[chatIdModel.id].chatHistory.Add(chat);


                        var responsePacket = new Packet
                        {
                            Command = (int)Command.SERVER.SEND_MESSAGE,
                            Data = chat
                        };

                        

                        // TODO) 채팅방 인원들에게만 broadcast
                        List<TcpClient> targetClients = new List<TcpClient>();

                        ChatRoomModel chatters = chatroomDic[chatIdModel.id];


                        BroadcastMessage(responsePacket, null, clients);
                        //BroadcastMessage(responsePacket, client, clients);
                        ///////////////////////////////////////////

                    }

                    else if (receivedPacket.Command == (int)(Command.CLIENT.REQUEST_CHATROOM_CREATE)) // 클라 - 채팅방 생성 요청
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
                    }



                    else
                    {
                        // 메시지를 모든 클라이언트에 브로드캐스트
                        //BroadcastMessage(receivedPacket, client);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("TcpIp ProcessClient : " + ex.Message);
                    break;
                }
            }

            clients.Remove(client);
            client.Close();
        }


        private static void BroadcastMessage(Packet packet, TcpClient sender, List<TcpClient> targetClients)
        {
            byte[] responseData = Converting.PacketToByteArray(packet);

            // 모든 클라이언트에게 메시지 전송
            foreach (TcpClient client in targetClients)
            {
                if (client != sender) // 보낸 클라이언트는 제외
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(responseData, 0, responseData.Length);
                }
            }
        }
    }






}
