namespace telo
{
    partial class loginWindow
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
            this.btn_exit = new System.Windows.Forms.Button();
            this.btn_lgn = new System.Windows.Forms.Button();
            this.lbl_password = new System.Windows.Forms.Label();
            this.lbl_username = new System.Windows.Forms.Label();
            this.txtbox_password = new System.Windows.Forms.TextBox();
            this.txtbox_username = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(385, 204);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(75, 23);
            this.btn_exit.TabIndex = 11;
            this.btn_exit.Text = "Exit";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_lgn
            // 
            this.btn_lgn.Location = new System.Drawing.Point(276, 204);
            this.btn_lgn.Name = "btn_lgn";
            this.btn_lgn.Size = new System.Drawing.Size(75, 23);
            this.btn_lgn.TabIndex = 10;
            this.btn_lgn.Text = "Login";
            this.btn_lgn.UseVisualStyleBackColor = true;
            this.btn_lgn.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_password
            // 
            this.lbl_password.AutoSize = true;
            this.lbl_password.Location = new System.Drawing.Point(172, 144);
            this.lbl_password.Name = "lbl_password";
            this.lbl_password.Size = new System.Drawing.Size(53, 13);
            this.lbl_password.TabIndex = 9;
            this.lbl_password.Text = "Password";
            // 
            // lbl_username
            // 
            this.lbl_username.AutoSize = true;
            this.lbl_username.Location = new System.Drawing.Point(172, 100);
            this.lbl_username.Name = "lbl_username";
            this.lbl_username.Size = new System.Drawing.Size(55, 13);
            this.lbl_username.TabIndex = 8;
            this.lbl_username.Text = "Username";
            // 
            // txtbox_password
            // 
            this.txtbox_password.Location = new System.Drawing.Point(276, 137);
            this.txtbox_password.Name = "txtbox_password";
            this.txtbox_password.PasswordChar = '*';
            this.txtbox_password.Size = new System.Drawing.Size(184, 20);
            this.txtbox_password.TabIndex = 7;
            this.txtbox_password.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // txtbox_username
            // 
            this.txtbox_username.Location = new System.Drawing.Point(276, 97);
            this.txtbox_username.Name = "txtbox_username";
            this.txtbox_username.Size = new System.Drawing.Size(184, 20);
            this.txtbox_username.TabIndex = 6;
            this.txtbox_username.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // loginWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 320);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btn_lgn);
            this.Controls.Add(this.lbl_password);
            this.Controls.Add(this.lbl_username);
            this.Controls.Add(this.txtbox_password);
            this.Controls.Add(this.txtbox_username);
            this.Name = "loginWindow";
            this.Text = "LOGIN WINDOW";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Button btn_lgn;
        private System.Windows.Forms.Label lbl_password;
        private System.Windows.Forms.Label lbl_username;
        private System.Windows.Forms.TextBox txtbox_password;
        private System.Windows.Forms.TextBox txtbox_username;
    }
}

