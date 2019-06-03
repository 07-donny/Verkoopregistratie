using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Verkoopregistratie_Donny
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\VerkoopDB.mdf;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadData()
        {
            con.Open();

            string filter = textBox1.Text;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Verkoop WHERE Klant LIKE @Filter", con);
            cmd.Parameters.AddWithValue("@Filter", $"%{filter}%");

            SqlDataReader reader = cmd.ExecuteReader();

            List<VerkoopClass> verkoopList = new List<VerkoopClass>();

            while (reader.Read())
            {
                VerkoopClass v = new VerkoopClass();
                v.Id = (int)reader["Id"];
                v.Product = (string)reader["Product"];
                v.Klant = (string)reader["Klant"];
                v.Prijs = (Decimal)reader["Prijs"];
                v.Datum = (DateTime)reader["Datum"];

                verkoopList.Add(v);
            }
            con.Close();

            dataGridView1.DataSource = verkoopList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            
                if (txtProduct.Text != "" && txtKlant.Text != "" && txtDatum.Text != "" && txtPrijs.Text != "")
                {
                    Decimal Prijs = Convert.ToDecimal(txtPrijs.Text);
                    if (Prijs > 0.00m && Prijs <= 999.99m)
                        {
                            SqlCommand cmd = new SqlCommand("INSERT INTO Verkoop (Product, Klant, Prijs, Datum) VALUES(@Product, @Klant, @Prijs, @Datum);", con);
                            cmd.Parameters.AddWithValue("Product", txtProduct.Text);
                            cmd.Parameters.AddWithValue("Klant", txtKlant.Text);

                            //Zorgt dat datum als dd/mm/yyyy ingetypt kan worden.
                            DateTime myDateTime = Convert.ToDateTime(txtDatum.Text);
                            string sqlFormattedDate = myDateTime.ToString("dd-MM-yyyy");
                             cmd.Parameters.AddWithValue("Datum", sqlFormattedDate);

                            //Zorgt dat prijs een decimaal getal blijft en een komma krijgt als ie niet is ingevoerd.
                            string prijs = String.Format("{0:C}", txtPrijs.Text);
                            decimal decprijs = Convert.ToDecimal(prijs);
                            cmd.Parameters.AddWithValue("Prijs", decprijs);

                            con.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Verkoop toegevoegd!");
                            con.Close();

                            txtProduct.Text = null;
                            txtKlant.Text = null;
                            txtPrijs.Text = null;
                            txtDatum.Text = null;

                            LoadData();
                         }
                            else
                            {
                                MessageBox.Show("Voer een correcte prijs in.");
                            }
                    
                            }
                                else
                                {
                                    MessageBox.Show("Voer alle gegevens in.");
                                }
        }

        private void DeleteRecord(int recordID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Verkoop WHERE Id=" + recordID, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Deze verkoop verwijderen?", "Weet u het zeker?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int recordID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
                DeleteRecord(recordID);
                LoadData();
            }
                
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadData();
            //Ik heb de filter functie al in de LoadData methode verwerkt.
        }
    }
}
