using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	public partial class Admin : Form
	{
		string ordb = "Data Source = orcl ;  User Id = scott ; Password = tiger ;";
		OracleConnection conn;
		OracleDataAdapter adabter;
		OracleDataAdapter adabter2;
		OracleDataAdapter adabter3;
		DataSet ds;
		DataSet ds2;
		DataSet ds3;
		public Admin()
		{
			InitializeComponent();
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void tabPage1_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			string searchText = textBox1.Text.Trim();
			if(searchText != "")
			{
				string query = "SELECT * FROM product WHERE Name = :searchText ";
				OracleCommand command = new OracleCommand(query, conn);
				command.Parameters.Add(":searchText", OracleDbType.Varchar2).Value = searchText;

				OracleDataAdapter adapter = new OracleDataAdapter(command);
				DataSet searchResultDataSet = new DataSet();
				adapter.Fill(searchResultDataSet);

				if (searchResultDataSet.Tables.Count > 0 && searchResultDataSet.Tables[0].Rows.Count > 0)
				{
					dataGridView1.DataSource = searchResultDataSet.Tables[0];
				}
				else
				{
					dataGridView1.DataSource = null;
					MessageBox.Show("No results found.");
				}
			}
			else
			{
				string cmd = $"select * from product";


				adabter = new OracleDataAdapter(cmd, ordb);
				ds = new DataSet();
				adabter.Fill(ds);
				dataGridView1.DataSource = ds.Tables[0];
			}

			

		}

		private void Admin_Load(object sender, EventArgs e)
		{
			conn = new OracleConnection(ordb);
			conn.Open();


			string cmd = $"select * from product";

			
			adabter = new OracleDataAdapter(cmd, ordb);
			
			ds = new DataSet();
			adabter.Fill(ds);
			dataGridView1.DataSource = ds.Tables[0];

			string cmd2 = $"select * from category";
			adabter2 = new OracleDataAdapter(cmd2, ordb);
			ds2 = new DataSet();
			adabter2.Fill(ds2);
			dataGridView2.DataSource = ds2.Tables[0];

			button6_Click(sender ,e);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			OracleCommandBuilder builder = new OracleCommandBuilder(adabter);
			adabter.Update(ds.Tables[0]);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			string searchText = textBox2.Text.Trim();
			if (searchText != "")
			{
				string query = "SELECT * FROM category WHERE Name = :searchText ";
				OracleCommand command = new OracleCommand(query, conn);
				command.Parameters.Add(":searchText", OracleDbType.Varchar2).Value = searchText;

				OracleDataAdapter adapter = new OracleDataAdapter(command);
				DataSet searchResultDataSet = new DataSet();
				adapter.Fill(searchResultDataSet);

				if (searchResultDataSet.Tables.Count > 0 && searchResultDataSet.Tables[0].Rows.Count > 0)
				{
					dataGridView2.DataSource = searchResultDataSet.Tables[0];
				}
				else
				{
					dataGridView2.DataSource = null;
					MessageBox.Show("No results found.");
				}
			}
			else
			{
				string cmd2 = $"select * from category";
				adabter2 = new OracleDataAdapter(cmd2, ordb);
				ds2 = new DataSet();
				adabter2.Fill(ds2);
				dataGridView2.DataSource = ds2.Tables[0];
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			OracleCommandBuilder builder = new OracleCommandBuilder(adabter2);
			adabter2.Update(ds2.Tables[0]);
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void button6_Click(object sender, EventArgs e)
		{
			
				string cmd = $"SELECT c.Id AS OrderId, p.Name ,p.list_price , p.Sale_Price, c.Status FROM product p  INNER JOIN Orders c ON p.id = c.ProductId ";

				adabter3 = new OracleDataAdapter(cmd, ordb);

				ds3 = new DataSet();
				adabter3.Fill(ds3);
				dataGridView3.DataSource = ds3.Tables[0];
			
		}

		private void button5_Click(object sender, EventArgs e)
		{
			string OrderId = dataGridView3.SelectedRows[0].Cells["OrderId"].Value.ToString();


			OracleCommand cmd = new OracleCommand();

			cmd.Connection = conn;
			cmd.CommandType = CommandType.Text;


			cmd.CommandText = $"update  orders set Status = 'Done' where id = {OrderId} ";
			cmd.ExecuteNonQuery();
			MessageBox.Show($"Success");
			button6_Click(sender , e);
		}
	}
}
