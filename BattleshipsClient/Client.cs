using BattleshipsClient.Controls;
using BattleshipsClient.Game.Editor;
using BattleshipsClient.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleshipsClient
{
    public partial class Client : Form
    {
        private ClientPlayer client;

        private FieldEditor editor;

        private bool isWaiting = true;
        private bool hasMove = false;
        private GameLogic game;

        private Thread waitingConnectionThread;

        public Client()
        {
            InitializeComponent();
            editor = new FieldEditor(playerFeld);
            client = new ClientPlayer();
            waitingConnectionThread = new Thread(new ThreadStart(WaitConnection));
            game = new GameLogic(client, playerFeld, oponentField, startButton);
            InitActions();
            FormClosing += CloseConnection;
           
        }



        private void startButton_Click(object sender, EventArgs e)
        {
            if(editor.IsReady())
            {

                if (client.TryToConnect())
                {
                    DisableEdition();
                    startButton.Text = "Waiting for opponent...";
                    waitingConnectionThread.Start();
                    
                }
                else { MessageBox.Show("Server not respond!"); }
            }
            else { MessageBox.Show("Your field not filled!"); } 
           
        }

        private void InitActions() 
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Cell cell = oponentField.GetCellAt(i, j);
                    cell.Click += (object sender, EventArgs e) => {
                  
                            game.HitOponent(cell);

                    };
                }
            }
        
        }

       
        


        //+++++Методы редактирования+++++/
        private void clearButton_Click(object sender, EventArgs e)
        {
            editor.Clear();
        }

        private void rotateButton_Click(object sender, EventArgs e)
        {
            editor.RotateShip();
        }

        private void DisableEdition() 
        {
            clearButton.Enabled = false;
            rotateButton.Enabled = false;
            playerFeld.Enabled = false;
            startButton.Enabled = false;
        }
        //-----Методы редактирования-----/

        private void WaitConnection() 
        {
            
            byte[] data = client.RecieveLast();
            if (data[0] == 1) { this.Close(); }
            else 
            {
                hasMove = data[0] == 10;
                isWaiting = false;
                if (hasMove)
                {
                    startButton.Text = "Your move.";
                    oponentField.Enabled = true;
                    game.SetMove(true);
                }
                else
                {
                    startButton.Text = "Opponents move...";
                    game.Join();

                }
            }
        }

        private void CloseConnection(object sender, EventArgs e) 
        {
            if (client.WasStarted()) 
            {
                client.Send(1);
                client.Send(1);
                client.RecieveLast();
                //while (game.IsReceiving()) ;
                client.CloseSocket();
            }
        }

    }
}
