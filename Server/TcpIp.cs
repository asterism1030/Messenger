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

namespace tcpip
{
    public class TcpIp 
    {
        private static List<TcpClient> clients = new List<TcpClient>();
        private static TcpListener server;


        //// 데이터
        // 채팅방 리스트
        private static List<ChatRoomListItemModel> chatroomsList = new List<ChatRoomListItemModel>();
        // 채팅방 (대화 이력, 정보 등)
        private static Dictionary<int, ChatRoomModel> chatroomDic = new Dictionary<int, ChatRoomModel>();





        public static void Start()
        {
            /////////////////////// test
            ChatRoomListItemModel test = new ChatRoomListItemModel();
            ChatRoomModel tt = new ChatRoomModel();

            test.Id = 0;
            test.Creater = "크롱";
            test.Cnt = 0;
            test.Name = "빠른 이직 기원";
            tt.chatRoomInfo = test;

            chatroomDic.Add(test.Id, tt);
            chatroomsList.Add(test);
            /////////////////////
            test.Id = 1;
            test.Creater = "루피";
            test.Cnt = 0;
            test.Name = "빠른 이직 기원";
            tt.chatRoomInfo = test;

            chatroomDic.Add(test.Id, tt);
            chatroomsList.Add(test);
            //////////////////////////////////

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
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    // 바이트 배열을 패킷으로 변환
                    Packet receivedPacket = Converting.ByteArrayToPacket(buffer.Take(bytesRead).ToArray());


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

                    if (receivedPacket.Command == (int)Command.CLIENT.REQUEST_CHATROOM_ENTER) // 클라 - 채팅방 입장 요청
                    {
                        int id = (int)receivedPacket.Data;

                        var responsePacket = new Packet
                        {
                            Command = (int)Command.SERVER.ACCEPT_CHATROOM_ENTER,
                            Data = chatroomDic[id]
                        };


                        byte[] responseData = Converting.PacketToByteArray(responsePacket);
                        stream.Write(responseData, 0, responseData.Length);
                    }



                    else
                    {
                        // 메시지를 모든 클라이언트에 브로드캐스트
                        BroadcastMessage(receivedPacket.Data.ToString(), client);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("TcpIp ProcessClient : " + ex.Message);
                    break;
                }
            }
        }


        private static void BroadcastMessage(string message, TcpClient sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);

            // 모든 클라이언트에게 메시지 전송
            foreach (TcpClient client in clients)
            {
                if (client != sender) // 보낸 클라이언트는 제외
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
            }
        }
    }






}
