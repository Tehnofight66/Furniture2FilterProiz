using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Furniture2
{
    public partial class MainForm : Form
    {
        string connectionDB = "Host = localhost; Port = 5432; Username = postgres; Password = postgres; Database = db26furniture";
        public string filter = "";
        public string sort = "";
        public MainForm(string role, string fio)
        {
            InitializeComponent();
            label1.Text = role;
            label2.Text = fio;
            SetProduct();
            if (role == "Гость" || role == "Авторизированный клиент") { panel2.Visible = false; }
        }

        public void SetProduct()
        {
            flowLayoutPanel1.Controls.Clear();
            NpgsqlConnection con = new NpgsqlConnection(connectionDB);
            con.Open();
            string sql = $@"SELECT name_tovar, price, postavshik.postavshik_pk, proizvoditel.proizvoditel_pk, category.category_pk, discount, quantity_sclad, opisanie, picture,
    round (price * (100-discount) / 100,2) as total_price
	FROM public.tovar
	join public.postavshik ON postavshik.id = tovar.postavshik_fk
	join public.proizvoditel ON proizvoditel.id = tovar.proizvoditel_fk
	join public.category ON category.id = tovar.category_fk
	        where (name_tovar ilike '%{textBox1.Text}%'
			or proizvoditel.proizvoditel_pk ilike '%{textBox1.Text}%'
			or category.category_pk ilike '%{textBox1.Text}%'
			or opisanie ilike '%{textBox1.Text}%') and ('%{filter}%' is null or postavshik.postavshik_pk ilike '%{filter}%')
			{sort}";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var tovar = new TovarControl();
                tovar.NameTovar = reader.GetString(0);
                tovar.Price = reader.GetDouble(1);
                tovar.Postavshik = reader.GetString(2);
                tovar.Proizvoditel = reader.GetString(3);
                tovar.Category = reader.GetString(4);
                tovar.Discount = reader.GetInt32(5);
                tovar.QuantitySclad = reader.GetInt32(6);
                tovar.Opisanie = reader.GetString(7);
                tovar.Picture = reader.IsDBNull(8) ? "picture.png" : reader.GetString(8);
                tovar.TotalPrice = reader.GetDouble(9);
                tovar.SetLabels();
                flowLayoutPanel1.Controls.Add(tovar);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetProduct();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SetProduct();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            sort = "order by price ASC";
            SetProduct();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            sort = "order by price DESC";
            SetProduct();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox1.Clear();
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                sort = "";
                filter = "";
                sort = "order by price ASC";
                SetProduct();
            }
            else
            {
                filter = comboBox1.Text;
                SetProduct();
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AutForm autform = new AutForm();
            autform.ShowDialog();
            this.Close();
        }
    }
}
