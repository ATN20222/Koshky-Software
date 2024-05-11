using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp1
{
	public partial class Register : Form
	{


		string ordb = "Data Source = orcl ;  User Id = scott ; Password = tiger ;";
		OracleConnection conn;
		public Register()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string name = textBox4.Text.Trim();
			string email = textBox1.Text.Trim();
			string password = textBox2.Text;
			string confirmPassword = textBox3.Text;
			int id = GetMaxUserID();

			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
			{
				MessageBox.Show("All fields are required.");
				return;
			}


			if (password != confirmPassword)
			{
				MessageBox.Show("Password and Confirm Password do not match.");
				return;
			}



			conn = new OracleConnection(ordb);
			conn.Open();

			string query = "INSERT INTO users (Id , name, email, password ,Role) VALUES ( :id , :name, :email, :password , 0)";

			OracleCommand command = new OracleCommand(query, conn);
			command.Parameters.Add(":id", OracleDbType.Varchar2).Value = id;
			command.Parameters.Add(":name", OracleDbType.Varchar2).Value = name;
			command.Parameters.Add(":email", OracleDbType.Varchar2).Value = email;
			command.Parameters.Add(":password", OracleDbType.Varchar2).Value = password;

			int rowsInserted = command.ExecuteNonQuery();

			if (rowsInserted > 0)
			{
				MessageBox.Show("Registration successful.");
				this.Hide();
				Form1 form = new Form1();
				form.Show();
			}
			else
			{
				MessageBox.Show("Registration failed.");
			}

			conn.Close();
		}



		private int GetMaxUserID()
		{
			int maxId = 0;

			using (OracleConnection conn = new OracleConnection(ordb))
			{
				conn.Open();

				
				OracleCommand command = new OracleCommand("GetMaxUserID", conn);
				command.CommandType = CommandType.StoredProcedure;

				
				OracleParameter maxIdParam = new OracleParameter("UID", OracleDbType.Int32);
				maxIdParam.Direction = ParameterDirection.Output;
				command.Parameters.Add(maxIdParam);

				
				command.ExecuteNonQuery();

				
				OracleDecimal oracleDecimal = (OracleDecimal)maxIdParam.Value;
				if (!oracleDecimal.IsNull)
				{
					maxId = oracleDecimal.ToInt32();
				}

				conn.Close();
			}

			return maxId + 1;
		}



		private void Register_Load(object sender, EventArgs e)
		{

		}
	}



}