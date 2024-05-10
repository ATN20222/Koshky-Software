using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace WindowsFormsApp1
{

	public partial class Form1 : Form
	{
		string ordb = "Data Source = orcl ;  User Id = scott ; Password = tiger ;";
		OracleConnection conn;
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			conn = new OracleConnection(ordb);
			conn.Open();
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			string email = textBox1.Text.Trim();
			string password = textBox2.Text.Trim();

			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
			{
				MessageBox.Show("Please enter username and password.");
				return;
			}

			OracleCommand cmd   = new OracleCommand();	
			cmd.Connection = conn;
			cmd.CommandType = CommandType.Text;
			cmd.Parameters.Add("Email", email);
			cmd.Parameters.Add("Password", password);
			cmd.CommandText = " SELECT Id, role FROM users WHERE email = :email AND password = :password";
			try
			{
				OracleDataReader oracleDataReader = cmd.ExecuteReader();
				if (oracleDataReader.Read())
				{
					string role = oracleDataReader["role"].ToString();
					//MessageBox.Show($"{oracleDataReader["Id"]}");
					int userId = Convert.ToInt32(oracleDataReader["Id"]);
					if (role == "1")
					{
						this.Hide();
						Admin admin = new Admin();
						admin.Show();
					}else if (role == "0")
					{
						this.Hide();


						Koshky koshky = new Koshky(userId);
						koshky.Show();
					}
						
				}
				else
				{
					MessageBox.Show("Invalid username or password.");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred: " + ex.Message);


			}




		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Hide();
			Register register = new Register();	
			register.Show();
		}



	}
}
