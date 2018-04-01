using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServer
{
    public class ServerPlayer
    {
        //Cсылка на буфер севера
        private byte[] serverBuffer;
        //Cсылка на сокет клиента
        private Socket clientSocket;

        private byte[] lastReceived;


        public ServerPlayer(byte[] serverBuffer, Socket clientSocket) 
        {
            this.serverBuffer = serverBuffer;
            this.clientSocket = clientSocket;
            StartReceiving();
        }

        //Запуск приема сообщений от клиента
        private void StartReceiving() 
        {
            if (IsSocketConnected()) 
            {
                clientSocket.BeginReceive(serverBuffer, 0, serverBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
            }
                
        }

        public void Send(params byte[] data)
        {
            if (IsSocketConnected())
            {
                //Открываем передачу(закрывает метод SendCallback)
                clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), clientSocket);
            }

        }


        //Глобальный доступ к последнему сообщению
        public byte[] ReceiveLast()
        {
            byte[] data = lastReceived;
            lastReceived = null;
            return data;
        }

        //Рекурсивный, асинхронный приём сообщений отклиента
        private void ReceiveCallback(IAsyncResult AR)
        {
            if (IsSocketConnected())
            {
                //Получаем ссылку на сокет
                clientSocket = (Socket)AR.AsyncState;
                //Завершаем приём/ узнаем размер полученой информации
                int received = clientSocket.EndReceive(AR);
                //создаем локальный буфер для сообщения
                byte[] dataBuf = new byte[received];
                //копируем сообщение из буфера сервера
                Array.Copy(serverBuffer, dataBuf, received);
                //десериализируем сообщение, и сохраняем
                lastReceived = dataBuf;
                if (lastReceived[0] == 1) { Send(1); }
                //открываем новый приём данных(рекурсивно,  new AsyncCallback(ReceiveCallback))
                if (IsSocketConnected())
                {
                    clientSocket.BeginReceive(serverBuffer, 0, serverBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
                }
            }
        }

        //Закрываем асинхронную передачу
        private void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
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

        public void CloseSocket()
        {
                Send(1);
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
        }
    }
}
