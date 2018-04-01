using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BattleshipsClient.Game;

namespace BattleshipsClient.Controls
{
    //Класс игрового поля
    public partial class Field : UserControl
    {
        //Матрица клеток/кнопок
        private Cell[,] cells;
        public Cell[,] Cells { get { return cells; } }

        //Список всех кораблей на поле
        public List<Ship> Fleet{ get; set;}
       
        public Field()
        {
            InitializeComponent();
            InitCells();
        }

        //Добавление кноапокна поле
        private void InitCells()
        {
            cells = new Cell[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    cells[i, j] = new Cell(i, j);
                    Controls.Add(cells[i, j]);
                }
            }
        }

        //Геттер клетки по координатам
        public Cell GetCellAt(int x, int y) 
        {
            return cells[x, y];
        }

        //Удар/выстрел по клетке
        //Сменяет нетронутое состояние
        //принимает - координаты int x, int y
        //возвращает - результат через энумератор HitResult
        public HitResult Hit(int x, int y)
        {
            Cell cellToHit = GetCellAt(x, y);
           
            HitResult result = HitResult.Null;
                switch (cellToHit.CellState)
                {
                    case Cell.State.Normal: cellToHit.SetState(Cell.State.HitWater);
                        result = HitResult.Water;
                        break;
                    case Cell.State.Ship: 
                        cellToHit.SetState(Cell.State.HitShip);
                        if (GetShipByCell(GetCellAt(x, y)).SinkShip())
                        {
                            result = HitResult.Sink;
                        }
                        else 
                        {
                            result = HitResult.Hit;
                        }
                        break;     
                }
                return result;
        }

        //Энумератор результатов удара
        public enum HitResult 
        { Null,Water,Hit,Sink }

        //Метод определяет - корабль(остальные клетки) по одной клетке 
        private Ship GetShipByCell(Cell cell)
        {
            Ship result = null;
            foreach (Ship ship in Fleet)
            {
                if (ship.IsPartOfShip(cell))
                {
                    result = ship;
                    break;
                }

            }
            return result;
        }

        //все корабли целы?
        public bool IsFleetAlive()
        {
            bool result = false;
            foreach (Ship ship in Fleet)
            {
                if (ship.IsAlive())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        
    }
}
