using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleshipsClient.Controls
{

    //Класс - клетка(поля)
    public class Cell : Button
    {
        //Стандартный размер кнопки 
        public static int DefaultButtonSize = 20;

        //Позиция на поле (X,Y) 
        public int LocationX { get; set; }
        public int LocationY { get; set; }

        //Состояние поля(через энумератор)
        private State cellStatate;
        public State CellState { get { return cellStatate; } }

        public Cell(int x, int y) 
        {
            LocationX = x;
            LocationY = y;
            cellStatate = State.Normal;
            InitButtonParams();
        }

        //Инициализация нужныйх параметров кнопки для отображения
        private void InitButtonParams()
        {
            this.Height = DefaultButtonSize;
            this.Width = DefaultButtonSize;
            this.BackColor = Color.LightCyan;
            this.ForeColor = Color.Blue;
            this.Location = new Point(LocationX * DefaultButtonSize, LocationY * DefaultButtonSize);
            this.Text = " ";
            this.Name = "cell[" + LocationX + "," + LocationY + "]";
            this.Font = new Font("Georgia", 16);
        }

        //Сеттер смены состояния кнопки(при смене меняется цвет)
        public void SetState(State state) 
        {
            cellStatate = state;
            switch (state) 
            {
                case State.Normal:
                    BackColor = Color.LightCyan;
                    break;
                case State.HitWater:
                    BackColor = Color.Blue;
                    break;
                case State.HitShip:
                    BackColor = Color.Yellow;
                    break;
                case State.SinkShip:
                    BackColor = Color.Red;
                    break;
                case State.Ship:
                    BackColor = Color.Gray;
                    break;
            }
        }

        //Сеттер смены состояния кнопки через byte
        public void SetState(byte state)
        {
            switch (state)
            {
                case 1:
                    SetState(State.HitWater);
                    break;
                case 2:
                    SetState(State.HitShip);
                    break;
                case 3:
                    SetState(State.SinkShip);
                    break;
                case 4:
                    SetState(State.Ship);
                    break;
                default:
                    SetState(State.Normal);
                    break;
            }
        }

        //Энумеатор состояний
        public enum State : byte 
        { 
            Normal = 0,     //нетронуто - вода
            HitWater = 1,   //удар по аоде
            HitShip = 2,    //удар по кораблю
            SinkShip = 3,   //потонувший корабль
            Ship = 4        //нетронуто - корабль
        }
    }
}
