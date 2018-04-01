using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServer
{
    public class Server
    {
        //Стандартный размер буфера
        public static int DefaultBufferSize = 1024;

        //Буфер сервера
        private byte[] serverBuffer;
        //Сокет сервера
        private Socket serverSocket;
        //Список игровых комнат
        private List<Room> playRooms;

        private int roomCounter = 0;

        public Server()
        {
            InitServer();
        }

        //Инициализация сервера
        private void InitServer()
        {
            Console.WriteLine("Initialize server...");
            serverBuffer = new byte[DefaultBufferSize];
            Console.WriteLine("[init]buffer was set.");
            //Создаем ТСР сокет для сервера 
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("[init]socket was set.");
            playRooms = new List<Room>();
            Console.WriteLine("[init]play rooms - container list was set.");
        }

        //Запуск приёма клиентов
        public void StartServer()
        {
            Console.WriteLine("Starting server...");
            //биндим к свободному IP
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
            serverSocket.Listen(1);
            //Начинаем приём сокетов
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            Console.WriteLine("Starting accept sockets...");
        }

        //Прием сокетов
        private void AcceptCallback(IAsyncResult AR)
        {
            //ПРиняли сокет
            Socket socket = serverSocket.EndAccept(AR);
            ClearEmptyRooms();
            //Отправили в комнату
            JoinRoom(socket);
            Console.WriteLine("New socket connected.");
            //ПРинимаем новый сокет
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void JoinRoom(Socket client) 
        {

            bool hasFoundFreeRoom = false;
            foreach (Room room in playRooms) 
            {
                if (room.IsWaiting()) 
                {
                    hasFoundFreeRoom = true;
                    //Если нашли свободную комнату, добавляемся
                    room.Join(new ServerPlayer(serverBuffer,client));
                    break;
                }
            }

            if (!hasFoundFreeRoom) 
            {
                //Если не нашли свободную комнату, создаем новую
                playRooms.Add(new Room(roomCounter,new ServerPlayer(serverBuffer,client)));
                roomCounter++;
            }
        }

        private void ClearEmptyRooms() 
        {
            List<Room> roomsToRemove = new List<Room>();

            foreach(Room room in playRooms)
            {
                if(room.IsEmpty())
                {
                    roomsToRemove.Add(room);

                }
            }

            foreach(Room room in roomsToRemove)
            {
                    playRooms.Remove(room);
            }

            
        }

        public void ShutdownServer() 
        {
            Console.WriteLine("Server is going to shutdown...");
            foreach (Room room in playRooms)
            {
                room.CloseRoom();
            }
        }
        
    }
}
