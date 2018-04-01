using BattleshipsClient.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient.Game.Editor
{
    //Установщик событий для кнопок редактора
    public class EditorCellEventHandler
    {
        //ссылка "итератор" свойств корабля редактора
        private EditorShipIterator refToShipIterator;
        //ссылка на поле редактора
        private Field refToField;


        public EditorCellEventHandler(Field refToField,EditorShipIterator refToShipIterator)
        {
            this.refToField = refToField;
            this.refToShipIterator = refToShipIterator;
            InitEvents();
        }


        //Инициализация событий всех кнопок(через лямбды)
        private void InitEvents()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Cell c = refToField.GetCellAt(i, j);
                    //курсор над кнопкой
                    c.MouseEnter += (object sender, EventArgs e) => { 
                       OnSelect(c);
                    };
                    //курсор сдвинулся с кнопки
                    c.MouseLeave += (object sender, EventArgs e) => { 
                       OnDeselect(c);
                    };
                    //обработка события нажатия
                    c.Click += (object sender, EventArgs e) => { 
                      OnAction(c);   
                    };

                }
            }
        }

        //Можно ли отрисовать корабль через клетку
        private bool IsClearPath(Cell c)
        {
            bool isClear = true;
            foreach (Cell btn in GetRelativeCells(c))
            {

                if (btn.BackColor == Color.Gray || IsTouchOther(c) || !refToShipIterator.HasNext())
                {
                    isClear = false;
                    break;
                }
            }

            return isClear;
        }

        //прикасается ли выбраная клетка(под курсором), к уже расположеномукораблю
        private bool IsTouchOther(Cell c)
        {
            bool result = false;
          
            foreach (Cell btn in GetRelativeCells(c))
            {
                int x = btn.LocationX;
                int y = btn.LocationY;

                /*
                 * [6][7][8]
                 * [5][X][1]
                 * [4][3][2]
                 */

                //[1]
                if (x < 9 && refToField.Cells[x + 1, y].BackColor == Color.Gray)
                {
                    return true;
                }

                //[2]
                if (x < 9 && y < 9 && refToField.Cells[x + 1, y + 1].BackColor == Color.Gray)
                {
                    return true;
                }

                //[3]
                if (y < 9 && refToField.Cells[x, y + 1].BackColor == Color.Gray)
                {
                    return true;
                }

                //[4]
                if (x > 0 && y < 9 && refToField.Cells[x - 1, y + 1].BackColor == Color.Gray)
                {
                    return true;
                }

                //[5]
                if (x > 0 && refToField.Cells[x - 1, y].BackColor == Color.Gray)
                {
                    return true;
                }

                //[6]
                if (x > 0 && y > 0 && refToField.Cells[x - 1, y - 1].BackColor == Color.Gray)
                {
                    return true;
                }

                //[7]
                if (y > 0 && refToField.Cells[x, y - 1].BackColor == Color.Gray)
                {
                    return true;
                }

                //[8]
                if (x < 9 && y > 0 && refToField.Cells[x + 1, y - 1].BackColor == Color.Gray)
                {
                    return true;
                }


            }

            return result;
        }

        //перекраска клеток потенциального коабля в зелёный
        //при наведении курсороа
        private void OnSelect(Cell c)
        {
            if (refToShipIterator.HasNext() && IsClearPath(c) && !refToShipIterator.HasLast())
            {
                foreach (Cell btn in GetRelativeCells(c))
                {
                    btn.BackColor = Color.Green;
                }
            }



        }
        //перекраска обратно, при уходе курсора 
        private void OnDeselect(Cell c)
        {
            if (refToShipIterator.HasNext() && !refToShipIterator.HasLast())
            {
                foreach (Cell btn in GetRelativeCells(c))
                {
                    if (btn.BackColor != Color.Gray)
                    {

                        btn.BackColor = Color.LightCyan;
                    }
                }
            }
        }

        //нажатие на кнопку
        private void OnAction(Cell c)
        {
           
            //Если корабль на потходящем месте, и итератор ещё не пуст
            if (IsClearPath(c) && refToShipIterator.HasNext() &&!refToShipIterator.HasLast())
            {
                List<Cell> cellsOfNewShip = new List<Cell>();
                foreach (Cell btn in GetRelativeCells(c))
                {
                    btn.SetState(Cell.State.Ship);
                    cellsOfNewShip.Add(btn); 
                }
                
                //помещаем корабль на поле
                refToField.Fleet.Add(new Ship(cellsOfNewShip));
              
                //итерируем следующие свойства корабля
                refToShipIterator.NextShipProps();

            }
        }


        //Узнаём относительные кнопки для отрисовки, чтобы не вылазить за границы поля 
        private List<Cell> GetRelativeCells(Cell c)
        {
            List<Cell> cells = new List<Cell>();

            //перебираем по текущей длине корабля(из итератора) 
            for (int i = 0; i < refToShipIterator.GetCurrentShipProps().Size; i++)
            {
                //горизонтальный поворот
                if (refToShipIterator.GetCurrentShipProps().IsHorisontal)
                {
                    if (c.LocationX <= 5)
                    {   
                        //если область на первой половине, отрисовуем в сторонувторой итд
                        
                        cells.Add(refToField.Cells[c.LocationX + i, c.LocationY]);
                    }
                    else
                    {
                        cells.Add(refToField.Cells[c.LocationX - i, c.LocationY]);
                    }
                }
                //Вертикальный поворот
                else
                {
                    if (c.LocationY <= 5)
                    {
                        cells.Add(refToField.Cells[c.LocationX, c.LocationY + i]);
                    }
                    else
                    {
                        cells.Add(refToField.Cells[c.LocationX, c.LocationY - i]);
                    }
                }

            }

            return cells;
        }
    }
    }
