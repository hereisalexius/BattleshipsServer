using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient
{

    //Представление сообщений между игроками
    public class CMessage
    {
        public Command CommandCode{ get; set;}
        public byte[] Data { get; set; }

        public CMessage() 
        {
            CommandCode = Command.Null;
            Data = new byte[] { 0 };
        }

        public CMessage( Command command)
        {
            CommandCode = command;
            Data = new byte[] { 0 };
        }

        public CMessage(Command command, params byte[] data)
        {
            CommandCode = command;
            Data = data;
        }

        public static byte[] Serialize(CMessage msg)
        {
            byte[] data = new byte[msg.Data.Length + 2];
            data[0] = (byte)msg.CommandCode;
            Array.Copy(msg.Data, 0, data, 1, msg.Data.Length);
            return data;
        }

        public static CMessage Deserialize(byte[] data)
        {
            CMessage msg = new CMessage();
            msg.CommandCode = (Command)data[0];
            byte[] body = new byte[data.Length - 1];
            Array.Copy(data, 1, body, 0, data.Length - 1);
            msg.Data = body;
            return msg;
        }

        /********************Энумератор***********************/

        public enum Command : byte 
        { 
            Null = 0,
            Exit = 1,
            Move = 2,
            WaitMove = 3,
            Ready = 4,
            Hit = 5,
            HitWater = 6,
            HitShip = 7,
            SinkShip = 8,
            Win = 9,
            Lose = 10
        } 


    }
}
