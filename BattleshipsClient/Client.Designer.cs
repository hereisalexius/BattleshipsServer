namespace BattleshipsClient
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.clearButton = new System.Windows.Forms.Button();
            this.rotateButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.oponentField = new BattleshipsClient.Controls.Field();
            this.playerFeld = new BattleshipsClient.Controls.Field();
            this.SuspendLayout();
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(13, 13);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(98, 23);
            this.clearButton.TabIndex = 2;
            this.clearButton.Text = "Очистить";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // rotateButton
            // 
            this.rotateButton.Location = new System.Drawing.Point(117, 13);
            this.rotateButton.Name = "rotateButton";
            this.rotateButton.Size = new System.Drawing.Size(95, 23);
            this.rotateButton.TabIndex = 3;
            this.rotateButton.Text = "Повернуть";
            this.rotateButton.UseVisualStyleBackColor = true;
            this.rotateButton.Click += new System.EventHandler(this.rotateButton_Click);
            // 
            // startButton
            // 
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F);
            this.startButton.Location = new System.Drawing.Point(13, 254);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(419, 50);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Начать";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(229, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Поле опонента";
            // 
            // oponentField
            // 
            this.oponentField.Enabled = false;
            this.oponentField.Location = new System.Drawing.Point(232, 48);
            this.oponentField.Name = "oponentField";
            this.oponentField.Size = new System.Drawing.Size(200, 200);
            this.oponentField.TabIndex = 1;
            // 
            // playerFeld
            // 
            this.playerFeld.Location = new System.Drawing.Point(12, 48);
            this.playerFeld.Name = "playerFeld";
            this.playerFeld.Size = new System.Drawing.Size(200, 200);
            this.playerFeld.TabIndex = 0;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 317);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.rotateButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.oponentField);
            this.Controls.Add(this.playerFeld);
            this.Name = "Client";
            this.Text = "Battleships - Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.Field playerFeld;
        private Controls.Field oponentField;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button rotateButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label1;
    }
}

