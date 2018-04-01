using BattleshipsClient.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient.Game
{
    //Представление корабля
    public class Ship
    {
        //список клеток из которых состоит корабль
        private List<Cell> shipCells;

        public Ship(List<Cell> shipCells)
        {
            this.shipCells = shipCells;
            InitShipState();
        }

        //при создании обьекта, помечаем клетки как корабль        
        private void InitShipState()
        {
            foreach (Cell cell in shipCells)
            {
                cell.SetState(Cell.State.Ship);
            }
        }

        public List<Cell> GetShipCells()
        {
            return shipCells;
        }
        //Проверка - является ли клетка частью корабля 
        public bool IsPartOfShip(Cell cell)
        {
            return shipCells.Contains(cell);
        }

        //затопить корабль
        public bool SinkShip()
        {
            if (!IsAlive())
            {
                foreach (Cell cell in shipCells)
                {
                    cell.SetState(Cell.State.SinkShip);
                }
                return true;
            }
            return false;
        }

        //проверить жив ли корабль
        public bool IsAlive()
        {
            bool result = false;
            foreach (Cell cell in shipCells)
            {
                if (cell.CellState == Cell.State.Ship)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
