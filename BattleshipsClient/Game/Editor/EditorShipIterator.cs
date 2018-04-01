using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient.Game.Editor
{

    //Итереатор свойтсв корабля
    public class EditorShipIterator
    {
        //массив длин кораблей
        private static int[] Ships = { 1, 1, 1, 1, 1, 2, 2, 2, 3, 3, 4 };
        
        //гранийи индексации для перебора массива
        private static int MinIndex = 0;
        private static int MaxIndex = 10;

        //свойтсво отвечающееза поворот корабля
        private bool isHorizontal = true;
        public bool IsHorizontal
        {
            get { return isHorizontal; }
            set { isHorizontal = value; }
        }
        //текущийиндекитератора
        private int currentIndex = 9;
        
        // Контейнер для текущий свойств
        private NewShipProperties currentShipProps;

        public EditorShipIterator()
        {
            Reset();
        }

        //Генерация новых свойств
        public NewShipProperties NextShipProps()
        {   
            //декремент индекса
            currentIndex--;
            if (HasNext())
            {
                //если ещё можна, создаем новыесвойства
                currentShipProps = new NewShipProperties(Ships[currentIndex], isHorizontal);
            }
            else
            {
                currentShipProps = null;
            }

            return currentShipProps;

        }

        //Возврат текущихсвойств
        public NewShipProperties GetCurrentShipProps()
        {
            return new NewShipProperties(Ships[currentIndex], isHorizontal); ;
        }

        //Есть ли ещё свойства/дошли доконца массива длин
        public bool HasNext()
        {
            return currentIndex >= MinIndex;
        }

        //Содержит последний набор свойств
        public bool HasLast()
        {
            return currentIndex == MinIndex;
        }

        //повернуть корабль
        public void RotateShip()
        {
            IsHorizontal = !IsHorizontal;
            currentShipProps.IsHorisontal = IsHorizontal;
        }

        //перезапустить итератор
        public void Reset()
        {
            currentIndex = MaxIndex;
            currentShipProps = new NewShipProperties(Ships[currentIndex], isHorizontal);
        }
    }
}
