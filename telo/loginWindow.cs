using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;

namespace telo
{
    public partial class loginWindow : Form
    {
        int idTyposXristi;

        public loginWindow()
        {
            InitializeComponent();
           
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True;");
        private void btn_exit_Click(object sender, EventArgs e)
        {
            
            Application.Exit();
        }

        private void btn_lgn_Click(object sender, EventArgs e)
        {

            
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from USERS where  USERS.username= '" + txtbox_username.Text + "' and USERS.password = '" + txtbox_password.Text + "' ", con);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            int count = 0;
            while (dr.Read())
            {
                idTyposXristi = dr.GetOrdinal("idTyposXristi");
                count += 1;
            }
            if (count == 1)
            {

                
                this.Hide();
                MainWindow mW = new MainWindow();
                mW.Show();

            }
      
            else
                MessageBox.Show("ΛΑΘΟΣ ΟΝΟΜΑ ΧΡΗΣΤΗ Ή ΚΩΔΙΚΟΣ !");
            con.Close();
            
        }

        private void txtbox_username_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = btn_lgn;
        }

        private void txtbox_password_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = btn_lgn;
        }
    }
}
