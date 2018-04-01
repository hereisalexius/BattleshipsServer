using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient
{
    public class ClientPlayer
    {
        //Сокет клиента
        private Socket clientSocket;
        private bool wasStarted = false;
        private bool isClosed = false;

        public ClientPlayer()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        //Попытка подключиться, возвращает удальсь подключиться?
        public bool TryToConnect() 
        {
            bool result = true;
            try
            {
                clientSocket.Connect(IPAddress.Loopback, 100);
                wasStarted = true;
            }
            catch (SocketException) 
            {
                result = false;
            }
            return result;
        }

        //Отправить сообщение на сервер
        public void Send(params byte[] data)
        {
            if (IsSocketConnected()) 
            {
                clientSocket.Send(data);
            }
        }

        //получить актуальное сообщение с сервера
        public byte[] RecieveLast() 
        {
            if (IsSocketConnected()) 
            {
                byte[] recBuff = new byte[1024];
                int rec = clientSocket.Receive(recBuff);
                byte[] data = new byte[rec];
                Array.Copy(recBuff, data, rec);
                return data;
            }
            return new byte[] { 1 };
        }

        //был ли сокет запущен
        public bool WasStarted() 
        {
            return wasStarted;
        }

        //закрыть сокеты(для добавления к событию закрытия формы)
        public void CloseSocket() 
        {
            if (WasStarted())
            {
                //Закрываем соединение
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                wasStarted = false;
            }
        }

        public bool IsSocketConnected()
        {
            bool part1 = clientSocket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (clientSocket.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
    }
}
