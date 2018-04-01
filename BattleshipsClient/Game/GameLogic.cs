using BattleshipsClient.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleshipsClient.Game
{

    //Игровая логика, привязанак компонентам формы
    public class GameLogic
    {
        //Ссылка на сокет клиента
        private ClientPlayer client;
        //Ссылка на поле игрока
        private Field playerField;
        //Ссылка на поле опонента
        private Field oponentField;


        private bool hasMove = false;
        private bool isWaiting = false;
        private bool isNeedToReceive = true;

        private Button startButton;

        public GameLogic(ClientPlayer client, Field playerField, Field oponentField, Button startButton)
        {
            this.client = client;
            this.playerField = playerField;
            this.oponentField = oponentField;
            this.startButton = startButton;

        }

        //Сеттер разрешения на ход
        public void SetMove(bool hasMove) 
        {
            this.hasMove = hasMove;
        }

        //Запустить поток ожидания сообщений от второго игрока
        public void Join() 
        {
            new Thread(new ThreadStart(Wait)).Start();
        }

        //Удар по опоненту в Cell c
        public void HitOponent(Cell c)
        {
            //Если не ждёт
            if (!isWaiting && isNeedToReceive)
            {
                //предварительная синхронизация
                //Join();
                //client.RecieveLast();
                //удар по полю
                CMessage msg = new CMessage(CMessage.Command.Hit, (byte)c.LocationX, (byte)c.LocationY);
                client.Send(CMessage.Serialize(msg));
                //синхронизация
                Join();
            }
            
        }

        //Метод ожидания сообщений от противника, запускается через поток
        private void Wait()
        {
            if (isNeedToReceive)
            {
                //ставим в режим ожидания
                isWaiting = true;
                oponentField.Enabled = false;

                //принимаем сообщение от противника
                CMessage msg = CMessage.Deserialize(client.RecieveLast());

                switch (msg.CommandCode)
                {
                    case CMessage.Command.Hit:
                        AcceptHit(msg.Data[0], msg.Data[1]);
                        break;
                    case CMessage.Command.HitShip:
                        oponentField.GetCellAt(msg.Data[0], msg.Data[1]).SetState(Cell.State.HitShip);
                        oponentField.GetCellAt(msg.Data[0], msg.Data[1]).Enabled = false;
                        MessageBox.Show("Ранил!");
                        hasMove = true;
                        break;
                    case CMessage.Command.SinkShip:
                        oponentField.GetCellAt(msg.Data[0], msg.Data[1]).SetState(Cell.State.SinkShip);
                        oponentField.GetCellAt(msg.Data[0], msg.Data[1]).Enabled = false;
                        MessageBox.Show("Потопил!");
                        RepaintOponentShip(msg.Data[0], msg.Data[1]);
                        hasMove = true;
                        break;
                    case CMessage.Command.HitWater:
                        oponentField.GetCellAt(msg.Data[0], msg.Data[1]).SetState(Cell.State.HitWater);
                        oponentField.GetCellAt(msg.Data[0], msg.Data[1]).Enabled = false;
                        MessageBox.Show("Промах!");
                        hasMove = false;
                        break;
                    case CMessage.Command.Lose:
                        oponentField.GetCellAt(msg.Data[0], msg.Data[1]).SetState(Cell.State.SinkShip);
                        oponentField.GetCellAt(msg.Data[0], msg.Data[1]).Enabled = false;
                        RepaintOponentShip(msg.Data[0], msg.Data[1]);
                        MessageBox.Show("Победа!");
                        startButton.Text = "Вы победили.";
                        isNeedToReceive = false;
                        oponentField.Enabled = false;
                        break;
                    case CMessage.Command.Exit:
                        MessageBox.Show("Соединение прервано!");
                        startButton.Text = "Соединение прервано!";
                        isNeedToReceive = false;
                        oponentField.Enabled = false;
                        break;
                }

                

                if (!hasMove && isNeedToReceive)
                {
                    //Если противник опять ходит/ опять ждём(рекурсивно)
                    startButton.Text = "Ход противника...";
                    Wait();
                }
                else if (isNeedToReceive)
                {
                    startButton.Text = "Ваш ход.";
                    oponentField.Enabled = true;
                }

                //выходим из режима ожидания
                isWaiting = false;
            }
 
        }

       
        /************************************/


        //метод принятия удара по полю,
        private void AcceptHit(int x, int y) 
        {
            //Удар по полю
            Field.HitResult hr = playerField.Hit(x, y);
            
            switch(hr)
            {
                    //ОТправляем отчёт об ударах
                case Field.HitResult.Water:
                    client.Send(CMessage.Serialize(new CMessage(CMessage.Command.HitWater, (byte)x, (byte)y)));
                    hasMove = true;
                    break;
                case Field.HitResult.Hit:
                    client.Send(CMessage.Serialize(new CMessage(CMessage.Command.HitShip, (byte)x, (byte)y)));
                    hasMove = false;
                    break;
                case Field.HitResult.Sink:
                   
                    //Если после действий противника все карабли затонули
                    if (!playerField.IsFleetAlive())
                    {
                        //MessageBox.Show("Лузер.");
                        startButton.Text = "Вы проиграли.";
                        //Шлём сообщение о проиграше
                        CMessage resp = new CMessage(CMessage.Command.Lose, (byte)x, (byte)y);
                        client.Send(CMessage.Serialize(resp));
                        isNeedToReceive = false;
                        //hasMove = true;
                    }
                    else 
                    {
                        client.Send(CMessage.Serialize(new CMessage(CMessage.Command.SinkShip, (byte)x, (byte)y)));
                       
                    }
                    hasMove = false;
                    break;
            }
        }

        
        //перерисовка жёлтых клеток в красный, если корабль затонул
        private void RepaintOponentShip(int x, int y)
        {
            Cell button = oponentField.GetCellAt(x,y);
            button.SetState(Cell.State.SinkShip);

            if (x < 9 && oponentField.GetCellAt(x + 1, y).BackColor == Color.Yellow)
            {
                RepaintOponentShip(x + 1, y);
            }

            if (y < 9 && oponentField.GetCellAt(x, y + 1).BackColor == Color.Yellow)
            {
                RepaintOponentShip(x, y + 1);
            }

            if (x > 0 && oponentField.GetCellAt(x - 1, y).BackColor == Color.Yellow)
            {
                RepaintOponentShip(x - 1, y);
            }

            if (y > 0 && oponentField.GetCellAt(x, y - 1).BackColor == Color.Yellow)
            {
                RepaintOponentShip(x, y - 1);
            }


        }

    }
}
