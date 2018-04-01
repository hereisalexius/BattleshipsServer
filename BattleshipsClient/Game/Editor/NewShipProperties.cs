using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient.Game.Editor
{

    //Контейнер свойств, для итератора свойств и редактора
    public class NewShipProperties
    {
        private int size;
        public int Size 
        {
            get { return size; }
            set { size = value; }
        }

        private bool isHorizontal;
        public bool IsHorisontal 
        {
            get { return isHorizontal; }
            set { isHorizontal = value; }
        }

        public NewShipProperties(int size, bool isHorizontal) 
        {
            this.size = size;
            this.isHorizontal = isHorizontal;
        }
    }
}
