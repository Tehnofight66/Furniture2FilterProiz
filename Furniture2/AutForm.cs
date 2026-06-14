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
    public partial class AutForm : Form
    {
        string connectDB = "Host = localhost; Port = 5432; Username = postgres; Password = postgres; Database = db26furniture";
        public AutForm()
        {
            InitializeComponent();
        }

        public void Autorizacia()
        {
            NpgsqlConnection con = new NpgsqlConnection(connectDB);
            con.Open();
            string sql = $@"SELECT roles.roles_pk, fio_pk, login, password
	            FROM public.users
	            join public.roles ON roles.id = users.roles_fk
	            where login = '{textBox2.Text}' and password = '{textBox1.Text}'";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                MessageBox.Show($"Добро пожаловать {reader.GetString(1)}", $"Информация ваша Роль: {reader.GetString(0)}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                var main = new MainForm(reader.GetString(1), reader.GetString(0));
                main.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Ошибка! Неверный логин либо пароль!", $"Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Autorizacia();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var main = new MainForm("Гость", "");
            main.ShowDialog();
            this.Close();
        }
    }
}
