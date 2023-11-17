namespace ProjectFF_Devops_Sec_2023
{
    partial class Form1
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
            this.LoginPanel = new System.Windows.Forms.Panel();
            this.NNlabel1 = new System.Windows.Forms.Label();
            this.UsernameTXTBOX = new System.Windows.Forms.TextBox();
            this.LoginBtn = new System.Windows.Forms.Button();
            this.LoginPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoginPanel
            // 
            this.LoginPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(166)))), ((int)(((byte)(91)))));
            this.LoginPanel.Controls.Add(this.LoginBtn);
            this.LoginPanel.Controls.Add(this.UsernameTXTBOX);
            this.LoginPanel.Controls.Add(this.NNlabel1);
            this.LoginPanel.Location = new System.Drawing.Point(494, 233);
            this.LoginPanel.Name = "LoginPanel";
            this.LoginPanel.Size = new System.Drawing.Size(352, 151);
            this.LoginPanel.TabIndex = 0;
            // 
            // NNlabel1
            // 
            this.NNlabel1.AutoSize = true;
            this.NNlabel1.Font = new System.Drawing.Font("Showcard Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NNlabel1.Location = new System.Drawing.Point(15, 0);
            this.NNlabel1.Name = "NNlabel1";
            this.NNlabel1.Size = new System.Drawing.Size(326, 40);
            this.NNlabel1.TabIndex = 0;
            this.NNlabel1.Text = "Geef een username";
            // 
            // UsernameTXTBOX
            // 
            this.UsernameTXTBOX.Font = new System.Drawing.Font("Showcard Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameTXTBOX.Location = new System.Drawing.Point(22, 43);
            this.UsernameTXTBOX.Name = "UsernameTXTBOX";
            this.UsernameTXTBOX.Size = new System.Drawing.Size(309, 47);
            this.UsernameTXTBOX.TabIndex = 1;
            this.UsernameTXTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LoginBtn
            // 
            this.LoginBtn.BackColor = System.Drawing.Color.White;
            this.LoginBtn.Font = new System.Drawing.Font("Showcard Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginBtn.Location = new System.Drawing.Point(22, 96);
            this.LoginBtn.Name = "LoginBtn";
            this.LoginBtn.Size = new System.Drawing.Size(309, 44);
            this.LoginBtn.TabIndex = 2;
            this.LoginBtn.Text = "START GAME";
            this.LoginBtn.UseVisualStyleBackColor = false;
            this.LoginBtn.Click += new System.EventHandler(this.LoginBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(66)))));
            this.ClientSize = new System.Drawing.Size(1482, 819);
            this.Controls.Add(this.LoginPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.LoginPanel.ResumeLayout(false);
            this.LoginPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LoginPanel;
        private System.Windows.Forms.TextBox UsernameTXTBOX;
        private System.Windows.Forms.Label NNlabel1;
        private System.Windows.Forms.Button LoginBtn;
    }
}

