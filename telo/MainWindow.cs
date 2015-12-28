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

namespace telo
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region global variables
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True;");

        string arDwm ;
        
        #endregion global variables

        #region functions
        public void FillDropDownList(string query, System.Windows.Forms.ComboBox DropDownName, string id, string name)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(query, con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                DropDownName.ValueMember = id;
                DropDownName.DisplayMember = name;
                DropDownName.DataSource = dt;

                DataRow datarow = dt.NewRow();
                datarow[name] = "Επιλέξτε";
                datarow[id] = 0;
                dt.Rows.InsertAt(datarow, 0);

                DropDownName.SelectedIndex = 0;

                con.Close();
            }
        }
        
        public void FillRoomsDropDown(System.Windows.Forms.ComboBox DropDownName, string idSelectedValueCombo, string DisplayNameCombo, DateTime checkInDate, DateTime checkOutDte, string IdTyposDwmatiou,bool addOrEdit)
        {
            // true gia edit kratisi combo-dateTime  false  gia add kratisi combo-dateTime
            if (!addOrEdit)
            {
                con.Open();
                string ID = IdTyposDwmatiou;
                SqlDataReader reader;
                DataTable dt = new DataTable();

                using (SqlCommand sc = new SqlCommand(@"SELECT idDwmatiou,arithmosDwmatiou
                                                          FROM ROOMS
                                                         WHERE idTyposDwmatiou = " + ID + @" 
                                                           AND idDwmatiou
                                                        NOT IN (SELECT idDwmatiou FROM KRATISEIS WHERE (hmerominiaAfixis <= @checkinDate and hmerominiaAnaxwrisis > @checkinDate)
                                                            OR (hmerominiaAfixis<@checkoutte AND hmerominiaAnaxwrisis>=@checkoutte)
                                                            OR (hmerominiaAfixis>=@checkinDate and hmerominiaAfixis<=@checkoutte))", con))

                {

                    sc.Parameters.AddWithValue("@checkinDate", checkInDate);
                    sc.Parameters.AddWithValue("@checkoutte", checkOutDte);

                    reader = sc.ExecuteReader();

                    dt.Columns.Add(idSelectedValueCombo, typeof(string));
                    dt.Columns.Add(DisplayNameCombo, typeof(string));
                    dt.Load(reader);

                    DropDownName.ValueMember = idSelectedValueCombo;
                    DropDownName.DisplayMember = DisplayNameCombo;
                    DropDownName.DataSource = dt;

                    DataRow datarow = dt.NewRow();
                    datarow[DisplayNameCombo] = "Επιλέξτε";
                    datarow[idSelectedValueCombo] = 0;
                    dt.Rows.InsertAt(datarow, 0);

                    DropDownName.SelectedIndex = 0;

                    con.Close();
                }
            }
            else
            {
                con.Close();
                con.Open();
                string ID = IdTyposDwmatiou;
                SqlDataReader reader;
                DataTable dt = new DataTable();

                using (SqlCommand sc = new SqlCommand(@"select idDwmatiou,arithmosDwmatiou 
                                                          from ROOMS
                                                         where idTyposDwmatiou = '" + ID + @"' 
                                                           and idDwmatiou
                                                        NOT IN (select idDwmatiou from KRATISEIS 
                                                                 where (hmerominiaAfixis <= @checkinDate and hmerominiaAnaxwrisis > @checkinDate)
                                                                   OR (hmerominiaAfixis<@checkoutte AND hmerominiaAnaxwrisis>=@checkoutte)
                                                                   OR (hmerominiaAfixis>=@checkinDate and hmerominiaAfixis<=@checkoutte))
                                                            OR ((arithmosDwmatiou = '" + arDwm + "'  ) and (idTyposDwmatiou =  '" + ID + "'))", con))
               

                {

                    sc.Parameters.AddWithValue("@checkinDate", checkInDate);
                    sc.Parameters.AddWithValue("@checkoutte", checkOutDte);

                    reader = sc.ExecuteReader();

                    dt.Columns.Add(idSelectedValueCombo, typeof(string));
                    dt.Columns.Add(DisplayNameCombo, typeof(string));
                    dt.Load(reader);

                    DropDownName.ValueMember = idSelectedValueCombo;
                    DropDownName.DisplayMember = DisplayNameCombo;
                    DropDownName.DataSource = dt;

                    DataRow datarow = dt.NewRow();
                    datarow[DisplayNameCombo] = "Επιλέξτε";
                    datarow[idSelectedValueCombo] = 0;
                    dt.Rows.InsertAt(datarow, 0);

                    DropDownName.SelectedIndex = 0;

                    con.Close();
                }
            }
        }

        public void clearFields(System.Windows.Forms.GroupBox grbBox)
        {
            foreach (var TextBox in grbBox.Controls.OfType<TextBox>())
            {                
                TextBox.Text = string.Empty;             
            }
            foreach (var ComboBox in grbBox.Controls.OfType<ComboBox>())
            {
                ComboBox.SelectedIndex = 0;
            }
            foreach (var checkBox in grbBox.Controls.OfType<CheckBox>())
            {
                checkBox.Checked = false;
            }
            foreach(var dateTimePicker in grbBox.Controls.OfType<DateTimePicker>())
            {
                dateTimePicker.Format = DateTimePickerFormat.Custom;
                dateTimePicker.CustomFormat = "dd-MM-yyyy";
                dateTimePicker.Value = DateTime.Today;
            }
        }   
        public void lblKatastasi(int idTypouDwmatiou,System.Windows.Forms.Label lbl)
        {
            con.Open();
            string MyQuery = @"SELECT count(ROOMS.arithmosDwmatiou)
                                 FROM ROOMS
                                WHERE ROOMS.idTyposDwmatiou = '"+  idTypouDwmatiou + @"'
                                  AND ROOMS.idDwmatiou NOT IN(SELECT idDwmatiou FROM KRATISEIS)";
            SqlCommand cmd = new SqlCommand(MyQuery, con);
            lbl.Text = cmd.ExecuteScalar().ToString();
            con.Close();
        }

        public void dataGridViewRefresh(string query,System.Windows.Forms.DataGridView dgv)
        {
            
            using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgv.DataSource = dt;

                foreach (DataGridViewRow row in dataGridViewKatastasiDwmatiwn.Rows)
                {
                    if (row.Cells.Cast<DataGridViewCell>().Any(c => c.Value == null || string.IsNullOrWhiteSpace(c.Value.ToString())))
                    {

                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                    else row.DefaultCellStyle.BackColor = Color.Red;
                }
                con.Close();
            }
        }

        public bool IsEmpty(System.Windows.Forms.GroupBox grbBox)
        {
            int count = 0;
            foreach (Control tb in grbBox.Controls)
            {
                if (tb is TextBox)
                {
                    if (String.IsNullOrEmpty(tb.Text))
                    {
                        count += 1;
                    }
                }
            }

            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }               
        }

        public void txtValidationIsEmpty(System.Windows.Forms.TextBox tb)
        {
            if (String.IsNullOrEmpty(tb.Text))
            {
                MessageBox.Show("Υποχρεωτικό πεδίο!");
                tb.Focus();
            }
        }

        private void IsNumber(object sender, EventArgs e)
        {
            double Num;
            TextBox tb = (TextBox)sender;

            if (!String.IsNullOrEmpty(tb.Text.Trim()))
            {
                bool isNum = double.TryParse(tb.Text.Trim(), out Num);

                if (!isNum)
                {
                    MessageBox.Show("Λάθος Νούμερο!");
                    tb.Focus();
                }

            }
        }

        public void RowsColor()
        {
            
            //for(int i=0; i<dataGridViewKatastasiDwmatiwn.RowCount-1;i++)
            //{
            //    int val = Convert.ToInt32(dataGridViewKatastasiDwmatiwn.Rows[i].Cells[2].Value.ToString());

            //    if(val==0)
            //    {
            //        dataGridViewKatastasiDwmatiwn.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            //    }
            //    else
            //    {
            //        dataGridViewKatastasiDwmatiwn.Rows[i].DefaultCellStyle.BackColor = Color.Green;
            //    }
            //}
        }
        #endregion functions

        #region tab pelates

        //add customer
        private void btn_add_customer_Click(object sender, EventArgs e)
        {

            if (!IsEmpty(grpbox_add_customer) || comboBox_group_add_customer.SelectedIndex == 0) //Empty textboxes
            {
                MessageBox.Show("Δεν εχουν συμπληρωθει ολα τα πεδία!");
                return;
            }

            con.Open();

            SqlCommand myCommand = con.CreateCommand();
            SqlTransaction myTrans;

            // Start a local transaction
            using (myTrans = con.BeginTransaction("SampleTransaction"))
            {

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                myCommand.Connection = con;
                myCommand.Transaction = myTrans;

                // Create Command and Execute the Transactions
                try
                {

                    myCommand.CommandText = @"insert into CUSTOMERS (idGroup,epwnimo,onoma,taftotita,afm,tilefwno,xwra,poli,odos,arithmos)
                                              values ('" + comboBox_group_add_customer.SelectedValue + @"',
                                                     N'" + txtbox_lname_add_customer.Text + @"',
                                                     N'" + txtbox_fname_add_customer.Text + @"',
                                                     N'" + txtbox_taftotita_add_customer.Text + @"',
                                                      '" + txtbox_afm_add_customer.Text + @"',
                                                      '" + txtbox_tel_add_customer.Text + @"',
                                                     N'" + txtbox_country_add_customer.Text + @"',
                                                     N'" + txtbox_city_add_customer.Text + @"',
                                                     N'" + txtbox_odos_add_customer.Text + @"',
                                                      '" + txtbox_arithmosodou_add_customer.Text + "')";

                    myCommand.ExecuteNonQuery();
                    myTrans.Commit();

                    MessageBox.Show("ΠΡΟΣΤΕΘΗΚΕ ΕΠΙΤΥΧΩΣ");
                    //grid refresh
                    dataGridViewRefresh(@"select CUSTOMERS.idPElati AS 'ID ΠΕΛΑΤΗ',
                                                 [GROUP].perigrafi AS 'ΓΚΡΟΥΠ',
                                                 CUSTOMERS.epwnimo AS ΕΠΩΝΥΜΟ,
                                                 CUSTOMERS.onoma AS 'ΟΝΟΜΑ',
                                                 CUSTOMERS.taftotita AS 'ΤΑΥΤΟΤΗΤΑ', 
                                                 CUSTOMERS.afm AS 'ΑΦΜ',
                                                 CUSTOMERS.tilefwno AS 'ΤΗΛΕΦΩΝΟ',
                                                 CUSTOMERS.xwra AS 'ΧΩΡΑ',
                                                 CUSTOMERS.poli AS 'ΠΟΛΗ', 
                                                 CUSTOMERS.odos AS 'ΟΔΟΣ',
                                                 CUSTOMERS.arithmos AS 'ΑΡΙΘΜΟΣ' 
                                            from CUSTOMERS, [GROUP]
                                           where CUSTOMERS.idGroup=[GROUP].idGroup
                                        ORDER BY CUSTOMERS.epwnimo ASC ", dataGridView_pelates);
                }
                catch (Exception ex)
                {
                    try
                    {
                        myTrans.Rollback("SampleTransaction");
                    }
                    catch (SqlException eep)
                    {
                        if (myTrans.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                        " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void btn_search_customer_add_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM CUSTOMERS where CUSTOMERS.taftotita='" + txtbox_taftotita_add_customer.Text + "'", con);

            txtbox_arithmosodou_add_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[10].Value.ToString();

            txtbox_odos_add_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[9].Value.ToString();

            txtbox_city_add_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[8].Value.ToString();

            txtbox_country_add_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[7].Value.ToString();

            txtbox_lname_add_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[2].Value.ToString();

            txtbox_tel_add_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[6].Value.ToString();

            txtbox_fname_add_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[3].Value.ToString();

            txtbox_afm_add_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[5].Value.ToString();

            txtbox_taftotita_add_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[4].Value.ToString();

            comboBox_group_add_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[1].Value.ToString();

            con.Close();
        }

        private void btn_clear_customer_add_Click(object sender, EventArgs e)
        {                 
            clearFields(grpbox_add_customer);
        }

        //edit customer
        private void btn_edit_customer_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand myCommand = con.CreateCommand();
            SqlTransaction myTrans;

            // Start a local transaction
            using (myTrans = con.BeginTransaction("SampleTransaction"))
            {

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                myCommand.Connection = con;
                myCommand.Transaction = myTrans;

                // Create Command and Execute the Transactions
                try
                {
                    //(select idGroup from[GROUP] where[GROUP].perigrafi = N'" + comboBox_group_edit_customer.Text + "')
                    myCommand.CommandText = @"UPDATE CUSTOMERS
                                                 SET CUSTOMERS.idGroup='" + comboBox_group_edit_customer.SelectedValue + @"', 
                                                     CUSTOMERS.epwnimo =N'" + txtbox_lname_edit_customer.Text + @"', 
                                                     CUSTOMERS.onoma=N'" + txtbox_fname_edit_customer.Text + @"',   
                                                     CUSTOMERS.taftotita=N'" + txtbox_taftotita_edit_customer.Text + @"',
                                                     CUSTOMERS.afm='" + txtbox_afm_edit_customer.Text + @"',
                                                     CUSTOMERS.tilefwno='" + txtbox_tel_edit_customer.Text + @"',
                                                     CUSTOMERS.xwra=N'" + txtbox_country_edit_customer.Text + @"', 
                                                     CUSTOMERS.poli=N'" + txtbox_city_edit_customer.Text + @"',
                                                     CUSTOMERS.odos=N'" + txtbox_odos_edit_customer.Text + @"', 
                                                     CUSTOMERS.arithmos='" + txtbox_arithmosodou_edit_customer.Text + @"'
                                               WHERE CUSTOMERS.idPelati='" + txtbox_idPelati_edit_customer.Text + "'";
                    myCommand.ExecuteNonQuery();
                    myTrans.Commit();
                    MessageBox.Show("ΕΝΗΜΕΡΩΘΗΚΕ ΕΠΙΤΥΧΩΣ");
                    //grid refresh edit customer
                    dataGridViewRefresh(@"select CUSTOMERS.idPElati AS 'ID ΠΕΛΑΤΗ',
                                                 [GROUP].perigrafi AS 'ΓΚΡΟΥΠ',
                                                 CUSTOMERS.epwnimo AS ΕΠΩΝΥΜΟ,
                                                 CUSTOMERS.onoma AS 'ΟΝΟΜΑ',
                                                 CUSTOMERS.taftotita AS 'ΤΑΥΤΟΤΗΤΑ', 
                                                 CUSTOMERS.afm AS 'ΑΦΜ',
                                                 CUSTOMERS.tilefwno AS 'ΤΗΛΕΦΩΝΟ',
                                                 CUSTOMERS.xwra AS 'ΧΩΡΑ',
                                                 CUSTOMERS.poli AS 'ΠΟΛΗ', 
                                                 CUSTOMERS.odos AS 'ΟΔΟΣ',
                                                 CUSTOMERS.arithmos AS 'ΑΡΙΘΜΟΣ' 
                                            from CUSTOMERS, [GROUP]
                                           where CUSTOMERS.idGroup=[GROUP].idGroup
                                        ORDER BY CUSTOMERS.epwnimo ASC ", dataGridView_pelates);
}
                catch (Exception ex)
                {
                    try
                    {
                        myTrans.Rollback("SampleTransaction");
                    }
                    catch (SqlException eep)
                    {
                        if (myTrans.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                        " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void btn_search_customer_edit_Click(object sender, EventArgs e)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(@"select CUSTOMERS.idPElati AS 'ID ΠΕΛΑΤΗ',
                                                             [GROUP].perigrafi AS 'ΓΚΡΟΥΠ',
                                                             CUSTOMERS.epwnimo AS ΕΠΩΝΥΜΟ,
                                                             CUSTOMERS.onoma AS 'ΟΝΟΜΑ', 
                                                             CUSTOMERS.taftotita AS 'ΤΑΥΤΟΤΗΤΑ',
                                                             CUSTOMERS.afm AS 'ΑΦΜ',
                                                             CUSTOMERS.tilefwno AS 'ΤΗΛΕΦΩΝΟ',
                                                             CUSTOMERS.xwra AS 'ΧΩΡΑ',
                                                             CUSTOMERS.poli AS 'ΠΟΛΗ',
                                                             CUSTOMERS.odos AS 'ΟΔΟΣ',
                                                             CUSTOMERS.arithmos AS 'ΑΡΙΘΜΟΣ'
                                                        from CUSTOMERS,[GROUP]
                                                       where CUSTOMERS.idGroup=[GROUP].idGroup
                                                         and ((CUSTOMERS.taftotita= '" + txtbox_taftotita_edit_customer.Text + @"') 
                                                          or (idPelati= '" + txtbox_idPelati_edit_customer.Text + @"')) 
                                                       ORDER BY CUSTOMERS.epwnimo ASC", con);
            sda.Fill(dt);

            dataGridView_pelates.DataSource = dt;

            txtbox_arithmosodou_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[10].Value.ToString();

            txtbox_odos_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[9].Value.ToString();

            txtbox_city_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[8].Value.ToString();

            txtbox_country_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[7].Value.ToString();

            txtbox_lname_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[2].Value.ToString();

            txtbox_tel_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[6].Value.ToString();

            txtbox_fname_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[3].Value.ToString();

            txtbox_afm_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[5].Value.ToString();

            txtbox_taftotita_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[4].Value.ToString();

            txtbox_idPelati_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[0].Value.ToString();

            comboBox_group_edit_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[1].Value.ToString();
           
            con.Close();
        }

        private void btn_clear_customer_edit_Click(object sender, EventArgs e)
        {
            clearFields(grpbox_edit_customer);          
        }

        //delete customer
        private void btn_delete_customer_Click(object sender, EventArgs e)
        {


            con.Open();

            SqlCommand myCommand = con.CreateCommand();
            SqlTransaction myTrans;

            // Start a local transaction
            using (myTrans = con.BeginTransaction("SampleTransaction"))
            {

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                myCommand.Connection = con;
                myCommand.Transaction = myTrans;

                // Create Command and Execute the Transactions
                try
                {
                    myCommand.CommandText = @"DELETE FROM CUSTOMERS 
                                               WHERE idPelati='" + txtbox_idPelati_delete_customer.Text + "'";

                    myCommand.ExecuteNonQuery();
                    myTrans.Commit();
                    MessageBox.Show("ΔΙΑΓΡΑΦΗΚΕ ΕΠΙΤΥΧΩΣ");
                    //grid refresh
                    dataGridViewRefresh(@"select CUSTOMERS.idPElati AS 'ID ΠΕΛΑΤΗ',
                                                 [GROUP].perigrafi AS 'ΓΚΡΟΥΠ',
                                                 CUSTOMERS.epwnimo AS ΕΠΩΝΥΜΟ,
                                                 CUSTOMERS.onoma AS 'ΟΝΟΜΑ',
                                                 CUSTOMERS.taftotita AS 'ΤΑΥΤΟΤΗΤΑ', 
                                                 CUSTOMERS.afm AS 'ΑΦΜ',
                                                 CUSTOMERS.tilefwno AS 'ΤΗΛΕΦΩΝΟ',
                                                 CUSTOMERS.xwra AS 'ΧΩΡΑ',
                                                 CUSTOMERS.poli AS 'ΠΟΛΗ', 
                                                 CUSTOMERS.odos AS 'ΟΔΟΣ',
                                                 CUSTOMERS.arithmos AS 'ΑΡΙΘΜΟΣ' 
                                            from CUSTOMERS, [GROUP]
                                           where CUSTOMERS.idGroup=[GROUP].idGroup
                                        ORDER BY CUSTOMERS.epwnimo ASC ", dataGridView_pelates);
                }
                catch (Exception exep)
                {
                    try
                    {
                        myTrans.Rollback("SampleTransaction");
                    }
                    catch (SqlException ex)
                    {
                        if (myTrans.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                        " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                }
                finally
                {
                    con.Close();
                }
            }

        }

        private void btn_search_customer_delete_Click(object sender, EventArgs e)
        {
            con.Open();
            DataTable dt = new DataTable();

            SqlDataAdapter sda = new SqlDataAdapter(@" select CUSTOMERS.idPElati AS 'ID ΠΕΛΑΤΗ',
                                                              [GROUP].perigrafi AS 'ΓΚΡΟΥΠ',
                                                              CUSTOMERS.epwnimo AS ΕΠΩΝΥΜΟ,
                                                              CUSTOMERS.onoma AS 'ΟΝΟΜΑ',
                                                              CUSTOMERS.taftotita AS 'ΤΑΥΤΟΤΗΤΑ',
                                                              CUSTOMERS.afm AS 'ΑΦΜ',
                                                              CUSTOMERS.tilefwno AS 'ΤΗΛΕΦΩΝΟ',
                                                              CUSTOMERS.xwra AS 'ΧΩΡΑ',
                                                              CUSTOMERS.poli AS 'ΠΟΛΗ', 
                                                              CUSTOMERS.odos AS 'ΟΔΟΣ',
                                                              CUSTOMERS.arithmos AS 'ΑΡΙΘΜΟΣ'
                                                         from CUSTOMERS,[GROUP]
                                                        where CUSTOMERS.idGroup=[GROUP].idGroup
                                                          and ((CUSTOMERS.taftotita= '" + txtbox_taftotita_delete_customer.Text + @"')
                                                           or (idPelati= '" + txtbox_idPelati_delete_customer.Text + @"')) 
                                                     ORDER BY CUSTOMERS.epwnimo ASC", con);

            sda.Fill(dt);

            dataGridView_pelates.DataSource = dt;

            txtbox_arithmosodou_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[10].Value.ToString();

            txtbox_odos_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[9].Value.ToString();

            txtbox_city_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[8].Value.ToString();

            txtbox_country_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[7].Value.ToString();

            txtbox_lname_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[2].Value.ToString();

            txtbox_tel_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[6].Value.ToString();

            txtbox_fname_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[3].Value.ToString();

            txtbox_afm_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[5].Value.ToString();

            txtbox_taftotita_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[4].Value.ToString();

            txtbox_idPelati_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[0].Value.ToString();

            comboBox_group_delete_customer.Text = dataGridView_pelates.SelectedRows[0].Cells[1].Value.ToString();

            con.Close();
        }

        private void btn_clear_customer_delete_Click(object sender, EventArgs e)
        {
            clearFields(grpbox_delete_customer);
        }
        #endregion tab pelates

        #region tab kratiseis

        //add kratisi btn
        private void btn_add_kratisi_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand myCommand = con.CreateCommand();
            SqlTransaction myTrans;

            // Start a local transaction
            using (myTrans = con.BeginTransaction("SampleTransaction"))
            {

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                myCommand.Connection = con;
                myCommand.Transaction = myTrans;

                // Create Command and Execute the Transactions
                try
                {
                    // (select idPelati from CUSTOMERS where taftotita='" + txtbox_taftotitta_add_kratisi.Text + "'))";
                    myCommand.CommandText = @"insert into KRATISEIS(idExoflisiKratisis, idDWmatiou, idTyposKratisis, idGroup, hmerominiaAfixis, hmerominiaAnaxwrisis, idTroposPlirwmis, idPelati)
                                              values ('" + checkBox_exoflisiKratisis_add_kratisi.Checked + @"',
                                                      '" + comboBox_arithmosDwmatiou_add_kratisi.SelectedValue + @"',
                                                      '" + comboBox_typos_kratisis_add_kratisi.SelectedValue + @"',
                                                      '" + comboBox_group_add_kratisi.SelectedValue + @"',
                                                           @dateCheckIn,
                                                           @dateCheckOut, 
                                                      '" + comboBox_troposPlirwmis_add_kratisi.SelectedValue + @"',
                                                        (select idPelati from CUSTOMERS where taftotita= N'" + txtbox_taftotitta_add_kratisi.Text + "'))";

                    myCommand.Parameters.Add("@dateCheckIn", SqlDbType.Date).Value = dateTimePicker_checkIn_add_kratisi.Value.Date;
                    myCommand.Parameters.Add("@dateCheckOut", SqlDbType.Date).Value = dateTimePicker_checkout_add_kratisi.Value.Date;

                    myCommand.ExecuteNonQuery();

                    myTrans.Commit();

                    MessageBox.Show("ΠΡΟΣΤΕΘΗΚΕ ΕΠΙΤΥΧΩΣ");
                    //grid refresh
                    dataGridViewRefresh(@"SELECT KRATISEIS.idKratisis as'ID ΚΡΑΤΗΣΗΣ', 
                                                 CUSTOMERS.idPelati as 'ID ΠΕΛΑΤΗ',
                                                 CUSTOMERS.taftotita as 'ΤΑΥΤΟΤΗΤΑ ΠΕΛΑΤΗ',
                                                 CUSTOMERS.epwnimo as 'ΕΠΩΝΥΜΟ',
                                                 CUSTOMERS.onoma as 'ΟΝΟΜΑ',
                                                 ROOMS.arithmosDwmatiou as 'ΑΡΙΘΜΟΣ ΔΩΜΑΤΙΟΥ',
                                                 ROOMTYPE.perigrafi as 'ΤΥΠΟΣ ΔΩΜΑΤΙΟΥ',
                                                 [GROUP].perigrafi as 'ΓΡΟΥΠ',
                                                 KRATISEIS.hmerominiaAfixis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΦΙΞΗΣ',
                                                 KRATISEIS.hmerominiaAnaxwrisis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΝΑΧΩΡΗΣΗΣ',
                                                 TyposKratisis.perigrafi as 'ΤΥΠΟΣ ΚΡΑΤΗΣΗΣ',
                                                 tropoiPlirwmis.perigrafi as 'ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ',
                                                 exoflisiKratisis.perigrafi as 'ΕΞΟΦΛΗΣΗ ΚΡΑΤΗΣΗΣ'
                                            FROM KRATISEIS,ROOMS,ROOMTYPE,CUSTOMERS,[GROUP],tropoiPlirwmis,TyposKratisis, exoflisiKratisis
                                           WHERE KRATISEIS.idDwmatiou=ROOMS.idDwmatiou and
                                                 KRATISEIS.idExoflisiKratisis=exoflisiKratisis.IdExoflisiKratisis and
                                                 KRATISEIS.idGroup=[GROUP].idGroup and
                                                 KRATISEIS.idPelati=CUSTOMERS.idPelati and
                                                 KRATISEIS.idTroposPlirwmis=tropoiPlirwmis.idTroposPlirwmis and
                                                 KRATISEIS.idTyposKratisis=TyposKratisis.idTyposKratisis and
                                                 ROOMS.idTyposDwmatiou=ROOMTYPE.idTyposDwmatiou
                                        ORDER BY hmerominiaAfixis DESC ", dataGridView_kratisi);
                }
                catch (Exception ex)
                {
                    try
                    {
                        myTrans.Rollback("SampleTransaction");
                    }
                    catch (SqlException eep)
                    {
                        if (myTrans.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() + " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void btn_clear_add_kratisi_Click(object sender, EventArgs e)
        {           
            clearFields(grpbox_add_kratisi);
        }

        //add kratisi combo-dataTimePicker
        private void dateTimePicker_checkIn_add_kratisi_ValueChanged(object sender, EventArgs e)
        {
            FillRoomsDropDown(comboBox_arithmosDwmatiou_add_kratisi, "idDwmatiou", "arithmosDwmatiou", dateTimePicker_checkIn_add_kratisi.Value.Date, dateTimePicker_checkout_add_kratisi.Value.Date, comboBox_typosDwmatiou_add_kratisi.SelectedValue.ToString(),false);
        }

        private void dateTimePicker_checkout_add_kratisi_ValueChanged(object sender, EventArgs e)
        {
            FillRoomsDropDown(comboBox_arithmosDwmatiou_add_kratisi, "idDwmatiou", "arithmosDwmatiou", dateTimePicker_checkIn_add_kratisi.Value.Date, dateTimePicker_checkout_add_kratisi.Value.Date, comboBox_typosDwmatiou_add_kratisi.SelectedValue.ToString(),false);
        }

        private void comboBox_typosDwmatiou_add_kratisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillRoomsDropDown(comboBox_arithmosDwmatiou_add_kratisi, "idDwmatiou", "arithmosDwmatiou", dateTimePicker_checkIn_add_kratisi.Value.Date, dateTimePicker_checkout_add_kratisi.Value.Date, comboBox_typosDwmatiou_add_kratisi.SelectedValue.ToString(),false);
        }

        //edit kratisi btn
        private void btn_edit_kratisi_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand myCommand = con.CreateCommand();
            SqlTransaction myTrans;

            // Start a local transaction
            using (myTrans = con.BeginTransaction("SampleTransaction"))
            {

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                myCommand.Connection = con;
                myCommand.Transaction = myTrans;

                // Create Command and Execute the Transactions               
                try
                {
                    myCommand.CommandText = @"UPDATE KRATISEIS 
                                                 SET idDwmatiou='" + comboBox_arithmosDwmatiou_editKratisi.SelectedValue + @"', 
                                                     idTyposKratisis='" + comboBox_typosKratisis_editKratisi.SelectedValue + @"', 
                                                     idGroup='" + comboBox_group_edit_kratisi.SelectedValue + @"',
                                                     hmerominiaAfixis=@dateCheckIn1,
                                                     hmerominiaAnaxwrisis=@dateCheckOut1,
                                                     idTroposPlirwmis='" + comboBox_troposPlirwmis_editKratisi.SelectedIndex + @"',
                                                     idExoflisiKratisis='"+ ckbox_exoflisiKratisis_editKratisi.Checked + @"'
                                               where KRATISEIS.idKratisis= '"+ txtbox_idKratisis_editKratisi.Text + "'";

                    myCommand.Parameters.Add("@dateCheckIn1", SqlDbType.Date).Value = dateTimePicker_checkIn_editKratisi.Value.Date;
                    myCommand.Parameters.Add("@dateCheckOut1", SqlDbType.Date).Value = dateTimePicker_checkOut_editKratisi.Value.Date;

                    myCommand.ExecuteNonQuery();
                    myTrans.Commit();
                    MessageBox.Show("ΕΝΗΜΕΡΩΘΗΚΕ ΕΠΙΤΥΧΩΣ");
                    //grid refresh
                    dataGridViewRefresh(@"SELECT KRATISEIS.idKratisis as'ID ΚΡΑΤΗΣΗΣ', 
                                                 CUSTOMERS.idPelati as 'ID ΠΕΛΑΤΗ',
                                                 CUSTOMERS.taftotita as 'ΤΑΥΤΟΤΗΤΑ ΠΕΛΑΤΗ',
                                                 CUSTOMERS.epwnimo as 'ΕΠΩΝΥΜΟ',
                                                 CUSTOMERS.onoma as 'ΟΝΟΜΑ',
                                                 ROOMS.arithmosDwmatiou as 'ΑΡΙΘΜΟΣ ΔΩΜΑΤΙΟΥ',
                                                 ROOMTYPE.perigrafi as 'ΤΥΠΟΣ ΔΩΜΑΤΙΟΥ',
                                                 [GROUP].perigrafi as 'ΓΡΟΥΠ',
                                                 KRATISEIS.hmerominiaAfixis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΦΙΞΗΣ',
                                                 KRATISEIS.hmerominiaAnaxwrisis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΝΑΧΩΡΗΣΗΣ',
                                                 TyposKratisis.perigrafi as 'ΤΥΠΟΣ ΚΡΑΤΗΣΗΣ',
                                                 tropoiPlirwmis.perigrafi as 'ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ',
                                                 exoflisiKratisis.perigrafi as 'ΕΞΟΦΛΗΣΗ ΚΡΑΤΗΣΗΣ'
                                            FROM KRATISEIS,ROOMS,ROOMTYPE,CUSTOMERS,[GROUP],tropoiPlirwmis,TyposKratisis, exoflisiKratisis
                                           WHERE KRATISEIS.idDwmatiou=ROOMS.idDwmatiou and
                                                 KRATISEIS.idExoflisiKratisis=exoflisiKratisis.IdExoflisiKratisis and
                                                 KRATISEIS.idGroup=[GROUP].idGroup and
                                                 KRATISEIS.idPelati=CUSTOMERS.idPelati and
                                                 KRATISEIS.idTroposPlirwmis=tropoiPlirwmis.idTroposPlirwmis and
                                                 KRATISEIS.idTyposKratisis=TyposKratisis.idTyposKratisis and
                                                 ROOMS.idTyposDwmatiou=ROOMTYPE.idTyposDwmatiou
                                        ORDER BY hmerominiaAfixis DESC ", dataGridView_kratisi);
                }
                catch (Exception ex)
                {
                    try
                    {
                        myTrans.Rollback("SampleTransaction");
                    }
                    catch (SqlException eep)
                    {
                        if (myTrans.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                        " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void btn_search_editKratisi_Click(object sender, EventArgs e)
        {
            //(select idPelati from CUSTOMERS where taftotita='" + txtbox_taftotitaPelati_editKratisi.Text + "'))
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(@"select KRATISEIS.idKratisis as 'ID ΚΡΑΤΗΣΗΣ', 
                                                             CUSTOMERS.idPelati as 'ID ΠΕΛΑΤΗ',
                                                             CUSTOMERS.taftotita as 'ΤΑΥΤΟΤΗΤΑ ΠΕΛΑΤΗ',
                                                             CUSTOMERS.epwnimo as 'ΕΠΩΝΥΜΟ',
                                                             CUSTOMERS.onoma as 'ΟΝΟΜΑ',
                                                             ROOMS.arithmosDwmatiou as 'ΑΡΙΘΜΟΣ ΔΩΜΑΤΙΟΥ',
                                                             ROOMTYPE.perigrafi as 'ΤΥΠΟΣ ΔΩΜΑΤΙΟΥ',
                                                             [GROUP].perigrafi as 'ΓΡΟΥΠ',
                                                             KRATISEIS.hmerominiaAfixis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΦΙΞΗΣ',
                                                             KRATISEIS.hmerominiaAnaxwrisis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΝΑΧΩΡΗΣΗΣ',
                                                             TyposKratisis.perigrafi as 'ΤΥΠΟΣ ΚΡΑΤΗΣΗΣ',
                                                             tropoiPlirwmis.perigrafi as 'ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ',
                                                             exoflisiKratisis.perigrafi as 'ΕΞΟΦΛΗΣΗ ΚΡΑΤΗΣΗΣ',
                                                             KRATISEIS.idExoflisiKratisis as 'CHECKED STATUS'
                                                        from KRATISEIS,ROOMS,ROOMTYPE,CUSTOMERS,[GROUP],tropoiPlirwmis,TyposKratisis, exoflisiKratisis
                                                       where (KRATISEIS.idKratisis=(select idKratisis from KRATISEIS where idKratisis='" + txtbox_idKratisis_editKratisi.Text + @"')
                                                          or KRATISEIS.idPelati = (select idPelati from CUSTOMERS where idPelati='" + txtbox_idPelati_editKratisi.Text + @"')
                                                          or KRATISEIS.idPelati = (select idPelati from CUSTOMERS where taftotita = N'" + txtbox_taftotitaPelati_editKratisi.Text + @"'))
                                                         and KRATISEIS.idDwmatiou = ROOMS.idDwmatiou
                                                         and KRATISEIS.idExoflisiKratisis = exoflisiKratisis.IdExoflisiKratisis
                                                         and KRATISEIS.idGroup =[GROUP].idGroup and KRATISEIS.idPelati = CUSTOMERS.idPelati
                                                         and KRATISEIS.idTroposPlirwmis = tropoiPlirwmis.idTroposPlirwmis
                                                         and KRATISEIS.idTyposKratisis = TyposKratisis.idTyposKratisis
                                                         and ROOMS.idTyposDwmatiou = ROOMTYPE.idTyposDwmatiou", con);

           
            sda.Fill(dt);
            dataGridView_kratisi.DataSource = dt;
           

            txtbox_idKratisis_editKratisi.Text = dataGridView_kratisi.SelectedRows[0].Cells[0].Value.ToString();

            txtbox_idPelati_editKratisi.Text = dataGridView_kratisi.SelectedRows[0].Cells[1].Value.ToString();

            txtbox_taftotitaPelati_editKratisi.Text = dataGridView_kratisi.SelectedRows[0].Cells[2].Value.ToString();

            dateTimePicker_checkIn_editKratisi.Value = Convert.ToDateTime(dataGridView_kratisi.CurrentRow.Cells[8].Value);

            dateTimePicker_checkOut_editKratisi.Value = Convert.ToDateTime(dataGridView_kratisi.CurrentRow.Cells[9].Value);

            comboBox_typosDwmatiou_editKratisi.Text = dataGridView_kratisi.SelectedRows[0].Cells[6].Value.ToString();
           

            comboBox_typosKratisis_editKratisi.Text = dataGridView_kratisi.SelectedRows[0].Cells[10].Value.ToString();

            comboBox_arithmosDwmatiou_editKratisi.Text = dataGridView_kratisi.SelectedRows[0].Cells[5].Value.ToString();
            arDwm = comboBox_arithmosDwmatiou_editKratisi.Text;

           

            comboBox_troposPlirwmis_editKratisi.Text = dataGridView_kratisi.SelectedRows[0].Cells[11].Value.ToString();

            comboBox_group_edit_kratisi.Text = dataGridView_kratisi.SelectedRows[0].Cells[7].Value.ToString();

            //  ckbox_exoflisiKratisis_editKratisi.Checked =  Convert.ToBoolean(dataGridView_kratisi.SelectedRows[0].Cells[12].Value.ToString());

            ckbox_exoflisiKratisis_editKratisi.Checked = Convert.ToBoolean(dataGridView_kratisi.SelectedRows[0].Cells[13].Value);


            //dataGridView_kratisi(SelectedRows[0].Cells[12].Value;
            // Convert.ToBoolean(dataGridView_kratisi.CurrentRow.Cells[12].Value)

            //dataGridView_kratisi.SelectedRows[0].Cells[12].Selected.ToString();

            //trexei kai edw i sinartisi gia na vgazei aftomatos ta apotelesmata ston arithmo dwmatiou
            FillRoomsDropDown(comboBox_arithmosDwmatiou_editKratisi, "idDwmatiou", "arithmosDwmatiou", dateTimePicker_checkIn_editKratisi.Value.Date, dateTimePicker_checkOut_editKratisi.Value.Date, comboBox_typosDwmatiou_editKratisi.SelectedValue.ToString(), true);



            con.Close();
        }

        private void btn_clear_edit_kratisi_Click(object sender, EventArgs e)
        {
            clearFields(grpbox_edit_kratisi);
        }

        //edit kratisi combo-dateTimePicker
        private void dateTimePicker_checkIn_editKratisi_ValueChanged(object sender, EventArgs e)
        {
            FillRoomsDropDown(comboBox_arithmosDwmatiou_editKratisi, "idDwmatiou", "arithmosDwmatiou", dateTimePicker_checkIn_editKratisi.Value.Date, dateTimePicker_checkOut_editKratisi.Value.Date, comboBox_typosDwmatiou_editKratisi.SelectedValue.ToString(), true);
            
        }

        private void dateTimePicker_checkOut_editKratisi_ValueChanged(object sender, EventArgs e)
        {
            FillRoomsDropDown(comboBox_arithmosDwmatiou_editKratisi, "idDwmatiou", "arithmosDwmatiou", dateTimePicker_checkIn_editKratisi.Value.Date, dateTimePicker_checkOut_editKratisi.Value.Date, comboBox_typosDwmatiou_editKratisi.SelectedValue.ToString(), true);
           
        }

        private void comboBox_typosDwmatiou_editKratisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillRoomsDropDown(comboBox_arithmosDwmatiou_editKratisi, "idDwmatiou", "arithmosDwmatiou", dateTimePicker_checkIn_editKratisi.Value.Date, dateTimePicker_checkOut_editKratisi.Value.Date, comboBox_typosDwmatiou_editKratisi.SelectedValue.ToString(), true);
           
        }

        //delete kratisi btn
        private void btn_delete_kratisi_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand myCommand = con.CreateCommand();
            SqlTransaction myTrans;

            // Start a local transaction
            using (myTrans = con.BeginTransaction("SampleTransaction"))
            {

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                myCommand.Connection = con;
                myCommand.Transaction = myTrans;

                // Create Command and Execute the Transactions
                try
                {
                    myCommand.CommandText = "DELETE FROM KRATISEIS WHERE idKratisis='" + txtbox_idKratisis_deleteKratisi.Text + "'";
                    myCommand.ExecuteNonQuery();
                    myTrans.Commit();
                    MessageBox.Show("ΔΙΑΓΡΑΦΗΚΕ ΕΠΙΤΥΧΩΣ");
                    //grid refresh
                    dataGridViewRefresh(@"SELECT KRATISEIS.idKratisis as'ID ΚΡΑΤΗΣΗΣ', 
                                                 CUSTOMERS.idPelati as 'ID ΠΕΛΑΤΗ',
                                                 CUSTOMERS.taftotita as 'ΤΑΥΤΟΤΗΤΑ ΠΕΛΑΤΗ',
                                                 CUSTOMERS.epwnimo as 'ΕΠΩΝΥΜΟ',
                                                 CUSTOMERS.onoma as 'ΟΝΟΜΑ',
                                                 ROOMS.arithmosDwmatiou as 'ΑΡΙΘΜΟΣ ΔΩΜΑΤΙΟΥ',
                                                 ROOMTYPE.perigrafi as 'ΤΥΠΟΣ ΔΩΜΑΤΙΟΥ',
                                                 [GROUP].perigrafi as 'ΓΡΟΥΠ',
                                                 KRATISEIS.hmerominiaAfixis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΦΙΞΗΣ',
                                                 KRATISEIS.hmerominiaAnaxwrisis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΝΑΧΩΡΗΣΗΣ',
                                                 TyposKratisis.perigrafi as 'ΤΥΠΟΣ ΚΡΑΤΗΣΗΣ',
                                                 tropoiPlirwmis.perigrafi as 'ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ',
                                                 exoflisiKratisis.perigrafi as 'ΕΞΟΦΛΗΣΗ ΚΡΑΤΗΣΗΣ'
                                            FROM KRATISEIS,ROOMS,ROOMTYPE,CUSTOMERS,[GROUP],tropoiPlirwmis,TyposKratisis, exoflisiKratisis
                                           WHERE KRATISEIS.idDwmatiou=ROOMS.idDwmatiou and
                                                 KRATISEIS.idExoflisiKratisis=exoflisiKratisis.IdExoflisiKratisis and
                                                 KRATISEIS.idGroup=[GROUP].idGroup and
                                                 KRATISEIS.idPelati=CUSTOMERS.idPelati and
                                                 KRATISEIS.idTroposPlirwmis=tropoiPlirwmis.idTroposPlirwmis and
                                                 KRATISEIS.idTyposKratisis=TyposKratisis.idTyposKratisis and
                                                 ROOMS.idTyposDwmatiou=ROOMTYPE.idTyposDwmatiou
                                        ORDER BY hmerominiaAfixis DESC ", dataGridView_kratisi);
                }
                catch (Exception exep)
                {
                    try
                    {
                        myTrans.Rollback("SampleTransaction");
                    }
                    catch (SqlException ex)
                    {
                        if (myTrans.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                        " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void btn_search_deleteKratisi_Click(object sender, EventArgs e)
        {

            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(@"select KRATISEIS.idKratisis as'ID ΚΡΑΤΗΣΗΣ', 
                                                             CUSTOMERS.idPelati as 'ID ΠΕΛΑΤΗ',
                                                             CUSTOMERS.taftotita as 'ΤΑΥΤΟΤΗΤΑ ΠΕΛΑΤΗ',
                                                             CUSTOMERS.epwnimo as 'ΕΠΩΝΥΜΟ',
                                                             CUSTOMERS.onoma as 'ΟΝΟΜΑ',
                                                             ROOMS.arithmosDwmatiou as 'ΑΡΙΘΜΟΣ ΔΩΜΑΤΙΟΥ',
                                                             ROOMTYPE.perigrafi as 'ΤΥΠΟΣ ΔΩΜΑΤΙΟΥ',
                                                             [GROUP].perigrafi as 'ΓΡΟΥΠ',
                                                             KRATISEIS.hmerominiaAfixis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΦΙΞΗΣ',
                                                             KRATISEIS.hmerominiaAnaxwrisis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΝΑΧΩΡΗΣΗΣ',
                                                             TyposKratisis.perigrafi as 'ΤΥΠΟΣ ΚΡΑΤΗΣΗΣ',
                                                             tropoiPlirwmis.perigrafi as 'ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ',
                                                             exoflisiKratisis.perigrafi as 'ΕΞΟΦΛΗΣΗ ΚΡΑΤΗΣΗΣ'
                                                        from KRATISEIS,ROOMS,ROOMTYPE,CUSTOMERS,[GROUP],tropoiPlirwmis,TyposKratisis, exoflisiKratisis
                                                       where (KRATISEIS.idPelati=
                                                             (select idPelati from CUSTOMERS 
                                                               where taftotita='" + txtbox_taftotitaPelati_deleteKratisi.Text + @"')) 
                                                                  or (KRATISEIS.idKratisis='" + txtbox_idKratisis_deleteKratisi.Text + @"') 
                                                         and KRATISEIS.idDwmatiou=ROOMS.idDwmatiou
                                                         and KRATISEIS.idExoflisiKratisis=exoflisiKratisis.IdExoflisiKratisis
                                                         and KRATISEIS.idGroup=[GROUP].idGroup
                                                         and KRATISEIS.idPelati=CUSTOMERS.idPelati
                                                         and KRATISEIS.idTroposPlirwmis=tropoiPlirwmis.idTroposPlirwmis
                                                         and KRATISEIS.idTyposKratisis=TyposKratisis.idTyposKratisis 
                                                         and ROOMS.idTyposDwmatiou=ROOMTYPE.idTyposDwmatiou
                                                       ORDER BY hmerominiaAfixis DESC ", con);

            sda.Fill(dt);
            dataGridView_kratisi.DataSource = dt;

            txtbox_idKratisis_deleteKratisi.Text = dataGridView_kratisi.SelectedRows[0].Cells[0].Value.ToString();
            txtbox_taftotitaPelati_deleteKratisi.Text = dataGridView_kratisi.SelectedRows[0].Cells[2].Value.ToString();

            con.Close();
        }

        private void btn_clear_delete_kratisi_Click(object sender, EventArgs e)
        {
            clearFields(grpbox_delete_kratisi);
        }
        #endregion tab kratiseis

        #region katastasi dwmatiwn
        private void rBtnKatastasiDwmatiwnOlaChanged(object sender, EventArgs e)
        {
            dataGridViewRefresh(@"select r.arithmosDwmatiou,
                                         rt.perigrafi AS 'ΤΥΠΟΣ ΔΩΜΑΤΙΟΥ',
                                         tk.perigrafi AS 'ΤΥΠΟΣ ΚΡΑΤΗΣΗΣ',
                                         datediff(day,kr.hmerominiaAfixis,kr.hmerominiaAnaxwrisis) as 'ΑΡΙΘΜΟΣ ΔΙΑΝΥΚΤΕΡΕΥΣΕΩΝ'
                                    from ROOMS r 
                                    left join ROOMTYPE rt
                                      on r.idTyposDwmatiou = rt.idTyposDwmatiou
                                    left join KRATISEIS kr
                                      on kr.idDwmatiou = r.idDwmatiou
                                    left join TyposKratisis tk
                                      on tk.idTyposKratisis = kr.idTyposKratisis", dataGridViewKatastasiDwmatiwn);
        }
        private void rBtnKatastasiDwmatiwnAdia_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewRefresh(@"select r.arithmosDwmatiou,
                                         rt.perigrafi AS 'ΤΥΠΟΣ ΔΩΜΑΤΙΟΥ',
                                         tk.perigrafi AS 'ΤΥΠΟΣ ΚΡΑΤΗΣΗΣ',
                                         datediff(day,kr.hmerominiaAfixis,kr.hmerominiaAnaxwrisis) as 'ΑΡΙΘΜΟΣ ΔΙΑΝΥΚΤΕΡΕΥΣΕΩΝ'
                                    from ROOMS r 
                                    left join ROOMTYPE rt
                                      on r.idTyposDwmatiou = rt.idTyposDwmatiou
                                    left join KRATISEIS kr
                                      on kr.idDwmatiou = r.idDwmatiou
                                    left join TyposKratisis tk
                                      on tk.idTyposKratisis = kr.idTyposKratisis
                                  where  datediff(day,kr.hmerominiaAfixis, kr.hmerominiaAnaxwrisis) is null", dataGridViewKatastasiDwmatiwn);            
        }
        #endregion katastasi dwmatiwn

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //grid pelates refresh
            dataGridViewRefresh(@"select CUSTOMERS.idPElati AS 'ID ΠΕΛΑΤΗ',
                                         [GROUP].perigrafi AS 'ΓΚΡΟΥΠ',
                                         CUSTOMERS.epwnimo AS ΕΠΩΝΥΜΟ,
                                         CUSTOMERS.onoma AS 'ΟΝΟΜΑ',
                                         CUSTOMERS.taftotita AS 'ΤΑΥΤΟΤΗΤΑ', 
                                         CUSTOMERS.afm AS 'ΑΦΜ',
                                         CUSTOMERS.tilefwno AS 'ΤΗΛΕΦΩΝΟ',
                                         CUSTOMERS.xwra AS 'ΧΩΡΑ',
                                         CUSTOMERS.poli AS 'ΠΟΛΗ', 
                                         CUSTOMERS.odos AS 'ΟΔΟΣ',
                                         CUSTOMERS.arithmos AS 'ΑΡΙΘΜΟΣ' 
                                    from CUSTOMERS, [GROUP]
                                   where CUSTOMERS.idGroup=[GROUP].idGroup
                                ORDER BY CUSTOMERS.epwnimo ASC ", dataGridView_pelates);
            //grid kratiseis refresh
            dataGridViewRefresh(@"SELECT KRATISEIS.idKratisis as'ID ΚΡΑΤΗΣΗΣ', 
                                         CUSTOMERS.idPelati as 'ID ΠΕΛΑΤΗ',
                                         CUSTOMERS.taftotita as 'ΤΑΥΤΟΤΗΤΑ ΠΕΛΑΤΗ',
                                         CUSTOMERS.epwnimo as 'ΕΠΩΝΥΜΟ',
                                         CUSTOMERS.onoma as 'ΟΝΟΜΑ',
                                         ROOMS.arithmosDwmatiou as 'ΑΡΙΘΜΟΣ ΔΩΜΑΤΙΟΥ',
                                         ROOMTYPE.perigrafi as 'ΤΥΠΟΣ ΔΩΜΑΤΙΟΥ',
                                         [GROUP].perigrafi as 'ΓΡΟΥΠ',
                                         KRATISEIS.hmerominiaAfixis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΦΙΞΗΣ',
                                         KRATISEIS.hmerominiaAnaxwrisis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΝΑΧΩΡΗΣΗΣ',
                                         TyposKratisis.perigrafi as 'ΤΥΠΟΣ ΚΡΑΤΗΣΗΣ',
                                         tropoiPlirwmis.perigrafi as 'ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ',
                                         exoflisiKratisis.perigrafi as 'ΕΞΟΦΛΗΣΗ ΚΡΑΤΗΣΗΣ',
                                         KRATISEIS.idExoflisiKratisis as 'CHECKED STATUS'
                                    FROM KRATISEIS,ROOMS,ROOMTYPE,CUSTOMERS,[GROUP],tropoiPlirwmis,TyposKratisis, exoflisiKratisis
                                   WHERE KRATISEIS.idDwmatiou=ROOMS.idDwmatiou and
                                         KRATISEIS.idExoflisiKratisis=exoflisiKratisis.IdExoflisiKratisis and
                                         KRATISEIS.idGroup=[GROUP].idGroup and
                                         KRATISEIS.idPelati=CUSTOMERS.idPelati and
                                         KRATISEIS.idTroposPlirwmis=tropoiPlirwmis.idTroposPlirwmis and
                                         KRATISEIS.idTyposKratisis=TyposKratisis.idTyposKratisis and
                                         ROOMS.idTyposDwmatiou=ROOMTYPE.idTyposDwmatiou
                                ORDER BY hmerominiaAfixis DESC ", dataGridView_kratisi);
            //gridrefreesh katastasi dwmatiwn
            dataGridViewRefresh(@"select r.arithmosDwmatiou,
                                         rt.perigrafi AS 'ΤΥΠΟΣ ΔΩΜΑΤΙΟΥ',
                                         tk.perigrafi AS 'ΤΥΠΟΣ ΚΡΑΤΗΣΗΣ',
                                         datediff(day,kr.hmerominiaAfixis,kr.hmerominiaAnaxwrisis) as 'ΑΡΙΘΜΟΣ ΔΙΑΝΥΚΤΕΡΕΥΣΕΩΝ'
                                    from ROOMS r 
                                    left join ROOMTYPE rt
                                      on r.idTyposDwmatiou = rt.idTyposDwmatiou
                                    left join KRATISEIS kr
                                      on kr.idDwmatiou = r.idDwmatiou
                                    left join TyposKratisis tk
                                      on tk.idTyposKratisis = kr.idTyposKratisis", dataGridViewKatastasiDwmatiwn);
            //tupos dwmatiou
            FillDropDownList("select idTyposDwmatiou,perigrafi from ROOMTYPE order by idTyposDwmatiou", comboBox_typosDwmatiou_add_kratisi, "idTyposDwmatiou", "perigrafi");
            FillDropDownList("select idTyposDwmatiou,perigrafi from ROOMTYPE order by idTyposDwmatiou", comboBox_typosDwmatiou_editKratisi, "idTyposDwmatiou", "perigrafi");

            //typos krathshs
            FillDropDownList("select idTyposKratisis,perigrafi from TyposKratisis order by idTyposKratisis", comboBox_typos_kratisis_add_kratisi, "idTyposKratisis", "perigrafi");
            FillDropDownList("select idTyposKratisis,perigrafi from TyposKratisis order by idTyposKratisis", comboBox_typosKratisis_editKratisi, "idTyposKratisis", "perigrafi");

            //tropos plirwmis
            FillDropDownList("select idTroposPlirwmis,perigrafi from tropoiPlirwmis order by idTroposPlirwmis", comboBox_troposPlirwmis_add_kratisi, "idTroposPlirwmis", "perigrafi");
            FillDropDownList("select idTroposPlirwmis,perigrafi from tropoiPlirwmis order by idTroposPlirwmis", comboBox_troposPlirwmis_editKratisi, "idTroposPlirwmis", "perigrafi");

            //Group
            FillDropDownList("select idGroup,perigrafi from [GROUP] order by idGroup", comboBox_group_add_kratisi, "idGroup", "perigrafi");
            FillDropDownList("select idGroup,perigrafi from [GROUP] order by idGroup", comboBox_group_edit_kratisi, "idGroup", "perigrafi");
            FillDropDownList("select idGroup,perigrafi from [GROUP] order by idGroup", comboBox_group_add_customer, "idGroup", "perigrafi");
            FillDropDownList("select idGroup,perigrafi from [GROUP] order by idGroup", comboBox_group_edit_customer, "idGroup", "perigrafi");
            FillDropDownList("select idGroup,perigrafi from [GROUP] order by idGroup", comboBox_group_delete_customer, "idGroup", "perigrafi");

           
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void tabControlKatastasiDwmatiwn_Selected(object sender, TabControlEventArgs e)
        {
            //arithmos adeiwn fardiklina
            lblKatastasi(1, lblKatastasiDwmatiwnFardiklinaArithmos);
            //arithmos adeiwn diklina
            lblKatastasi(2, lblKatastasiDwmatiwnDiklinaArithmos);
            //arithmos adeiwn monoklina
            lblKatastasi(3, lblKatastasiDwmatiwnMonoklinaArithmos);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.dataGridViewKatastasiDwmatiwn.Width, this.dataGridViewKatastasiDwmatiwn.Height);
            dataGridViewKatastasiDwmatiwn.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridViewKatastasiDwmatiwn.Width, this.dataGridViewKatastasiDwmatiwn.Height));
            e.Graphics.DrawImage(bm, 0, 0);
        }

        private void btnPrintKatastasiDwmatiwn_Click(object sender, EventArgs e)
        {
            printDocumentKatastasiDwmatiwn.Print();
        }
    }
}

