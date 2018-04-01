using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleshipsServer
{

    //Игровая комната
    public class Room
    {
        private ServerPlayer player1;
        private ServerPlayer player2;

        //Поток синхронизации
        private Thread syncThread;

        private bool isClosed = false;

        private int roomId;


        public Room(int roomId ,ServerPlayer player1) 
        {
            this.roomId = roomId;
            this.player1 = player1;
            syncThread = new Thread(new ThreadStart(Sync));
        }

        //Пуста ли комната
        public bool IsEmpty() 
        {
            return !player1.IsSocketConnected();
        }

        public bool IsClosed() 
        {
            return isClosed;
        }

        //В режиме ожидания?
        public bool IsWaiting()
        {
            return player1 != null && player2 == null;
        }

        //Метод для подключения второго игрока
        public void Join(ServerPlayer player2) 
        {
            this.player2 = player2;
          
            StartGame();
        }

        //Начать игровой поток
        private void StartGame()
        {
            //определяем кто ходит первым
            SetMoves();
            //запускаем синхронизацию
            syncThread.Start();
 
        }

        private void Sync() 
        {
            while (player1.IsSocketConnected() || player2.IsSocketConnected())
            {
                byte[] data1 = player1.ReceiveLast();
                byte[] data2 = player2.ReceiveLast();
                

                if (data1 != null)
                {
                    Console.WriteLine("[room " + roomId + "][player 1]:" + CMessage.Deserialize(data1).CommandCode.ToString() + " " + data1[0]);
                    player2.Send(data1);
                }

                if (data2 != null)
                {
                    Console.WriteLine("[room " + roomId + "][player 2]:" + CMessage.Deserialize(data2).CommandCode.ToString() + " " + data2[0]);
                    player1.Send(data2);
                }
            }
            //CloseRoom();
        }

        private void SetMoves()
        {
            
            if (RandomMove())
            {
                player1.Send(10);
                player2.Send(0);
            }
            else 
            {
                player1.Send(0);
                player2.Send(10);
            }
        }

        //Генератор bool значений
        private bool RandomMove()
        {
            Random rnd = new Random();
            int number = rnd.Next(10);
            if (number > 5)
            {
                return true;
            }

            return false;
        }

        //Закрыть комнату
        public void CloseRoom() 
        {
            //Sendshit pls
            isClosed = true; ;
            player1.Send(1);
            if (player2 != null) 
            {
                player2.Send(1);
                
            }
            
        }
    }
}
