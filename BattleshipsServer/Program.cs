﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Battleships - Server";
            Server server = new Server();
            server.StartServer();
            Console.ReadLine();
            server.ShutdownServer();

        }
    }
}
