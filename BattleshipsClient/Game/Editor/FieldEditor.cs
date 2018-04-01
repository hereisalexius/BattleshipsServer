using BattleshipsClient.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient.Game.Editor
{

    //Редактор поля( набор методов) - вызывается из формы
    public class FieldEditor
    {
        //Редактируемое поле
        private Field field;
        //Итератор свойств
        private EditorShipIterator shipIterator;
        //Установщиксобытий для кнопок при редактировании
        private EditorCellEventHandler cellEvents;


   
        public FieldEditor(Field field)
        {
            this.field = field;
            shipIterator = new EditorShipIterator();
            Clear();
            cellEvents = new EditorCellEventHandler(field, shipIterator);
        }

        //Завершён липроцесс создания?
        public bool IsReady() 
        {
            return shipIterator.HasLast();
        }
        //поворот корабля
        public void RotateShip()
        {
            shipIterator.RotateShip();
        }
        //Очистить поле, обновить параметры редактирования
        public void Clear()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    field.GetCellAt(i, j).SetState(Cell.State.Normal);
                }
            }
            shipIterator.Reset();
            field.Fleet = new List<Ship>();
        }


    }
}
