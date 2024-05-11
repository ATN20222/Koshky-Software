using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
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

namespace WindowsFormsApp1
{
	public partial class Koshky : Form
	{
		int UserId = 0;
		string ordb = "Data Source = orcl ;  User Id = scott ; Password = tiger ;";
		OracleConnection conn;
		OracleDataAdapter adabter;
		OracleDataAdapter adabter2;
		OracleDataAdapter adabter3;
		OracleDataAdapter adabter4;
		OracleDataAdapter adabter5;
		DataSet ds;
		DataSet ds2;
		DataSet ds3;
		DataSet ds4;
		DataSet ds5;
		public Koshky(int id)
		{
			UserId = id;

			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string productId = dataGridView2.SelectedRows[0].Cells["ID"].Value.ToString();
			int userId = UserId;
			int cartId = GetMaxCartID();
			if (string.IsNullOrEmpty(productId))
			{
				MessageBox.Show("Please select a product.");
				return;
			}

			using (OracleConnection conn = new OracleConnection(ordb))
			{
				conn.Open();
				string query = "INSERT INTO cart (Id, UserId, ProductId) VALUES (:cartId,:userId, :productId)";
				OracleCommand command = new OracleCommand(query, conn);
				command.Parameters.Add(":cartId", OracleDbType.Int32).Value = cartId;
				command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
				command.Parameters.Add(":productId", OracleDbType.Varchar2).Value = productId;

				try
				{
					int rowsInserted = command.ExecuteNonQuery();
					if (rowsInserted > 0)
					{
						MessageBox.Show("Product added to cart successfully.");
					}
					else
					{
						MessageBox.Show("Failed to add product to cart.");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("An error occurred: " + ex.Message);
				}
			}
			button3_Click(sender , e);
		}
		private void Koshky_Load(object sender, EventArgs e)
		{
			conn = new OracleConnection(ordb);
			conn.Open();
			string cmd = $"SELECT p.* FROM product p INNER JOIN cart c ON p.id = c.ProductId WHERE c.UserId = {UserId}"; ;




			adabter = new OracleDataAdapter(cmd, ordb);

			ds = new DataSet();
			adabter.Fill(ds);
			dataGridView1.DataSource = ds.Tables[0];


			string cmd2 = $"select * from product";
			
			adabter2 = new OracleDataAdapter(cmd2, ordb);
			ds2 = new DataSet();
			adabter2.Fill(ds2);
			dataGridView2.DataSource = ds2.Tables[0];




			OracleCommand oracleCommand = new OracleCommand();
			oracleCommand.Connection = conn;
			oracleCommand.CommandType = CommandType.Text;
			oracleCommand.CommandText = "select Name, id from product ";

			OracleDataReader dr = oracleCommand.ExecuteReader();
			while (dr.Read())
			{
				comboBox1.Items.Add(dr.GetString(0));
				comboBox2.Items.Add(dr.GetString(0));	
				comboBox3.Items.Add(dr.GetString(0));
			}
				dr.Close();

			button7_Click(sender, e);








		}

		private void button4_Click(object sender, EventArgs e)
		{
			string searchText = textBox2.Text.Trim();
			if (searchText != "")
			{
				string query = "SELECT * FROM product WHERE Name = :searchText ";
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
				string cmd2 = $"select * from product";
				adabter2 = new OracleDataAdapter(cmd2, ordb);
				ds2 = new DataSet();
				adabter2.Fill(ds2);
				dataGridView2.DataSource = ds2.Tables[0];
			}
		}


		private int GetMaxCartID()
		{
			int maxId = 0;

			using (OracleConnection conn = new OracleConnection(ordb))
			{
				conn.Open();


				OracleCommand command = new OracleCommand("GetMaxCartID", conn);
				command.CommandType = CommandType.StoredProcedure;


				OracleParameter maxIdParam = new OracleParameter("MID", OracleDbType.Int32);
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

		private void button3_Click(object sender, EventArgs e)
		{
			conn = new OracleConnection(ordb);
			conn.Open();
			string cmd = $"SELECT p.* FROM product p INNER JOIN cart c ON p.id = c.ProductId WHERE c.UserId = {UserId}"; ;


			adabter = new OracleDataAdapter(cmd, ordb);

			ds = new DataSet();
			adabter.Fill(ds);
			dataGridView1.DataSource = ds.Tables[0];


			string cmd2 = $"select * from product";

			adabter2 = new OracleDataAdapter(cmd2, ordb);
			ds2 = new DataSet();
			adabter2.Fill(ds2);
			dataGridView2.DataSource = ds2.Tables[0];

		}

		private void button5_Click(object sender, EventArgs e)
		{
			decimal Rate = numericUpDown1.Value;
			ComboBox cmb = new ComboBox();

			OracleCommand oracleCommand = new OracleCommand();
			oracleCommand.Connection = conn;
			oracleCommand.CommandType = CommandType.Text;
			oracleCommand.CommandText = "select id , Name from product ";

			OracleDataReader dr = oracleCommand.ExecuteReader();
			while (dr.Read())
			{
				cmb.Items.Add(dr[0]);
			}
			dr.Close();

			cmb.SelectedIndex = comboBox1.SelectedIndex;
				
			OracleCommand cmd = new OracleCommand();

			cmd.Connection = conn;
			cmd.CommandType = CommandType.Text;


			cmd.CommandText = $"update  product set Rating = {Rate} where id = {cmb.SelectedItem } ";
			cmd.ExecuteNonQuery();
			MessageBox.Show($"Success");


		}

		private void tabPage4_Click(object sender, EventArgs e)
		{

		}

		private void button6_Click(object sender, EventArgs e)
		{
			ComboBox cmbIds = new ComboBox();

			OracleCommand oracleCommand = new OracleCommand();
			oracleCommand.Connection = conn;
			oracleCommand.CommandType = CommandType.Text;
			oracleCommand.CommandText = "select id , Name from product ";

			OracleDataReader dr = oracleCommand.ExecuteReader();
			while (dr.Read())
			{
				cmbIds.Items.Add(dr[0]);
			}
			dr.Close();

			cmbIds.SelectedIndex = comboBox2.SelectedIndex;
			string productId1  = cmbIds.Text;
			cmbIds.SelectedIndex = comboBox3.SelectedIndex;
			string productId2 = cmbIds.Text;


			string cmd1 = $"select *from product where id ={productId1} ";
			string cmd2 = $"select *from product where id ={productId2} ";

			adabter3 = new OracleDataAdapter(cmd1, ordb);

			ds3 = new DataSet();
			adabter3.Fill(ds3);
			dataGridView3.DataSource = ds3.Tables[0];


			

			adabter4 = new OracleDataAdapter(cmd2, ordb);
			ds4 = new DataSet();
			adabter4.Fill(ds4);
			dataGridView4.DataSource = ds4.Tables[0];



		}

		private void button2_Click(object sender, EventArgs e)
		{
			OracleCommand cmd = new OracleCommand();
			OracleCommand cmd2 = new OracleCommand();
			cmd.Connection = conn;
			cmd2.Connection = conn;

			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				if (row.IsNewRow) continue;

				int productId = Convert.ToInt32(row.Cells["ID"].Value);
				int orderId = GetMaxOrderID();

				string query = $"INSERT INTO Orders (Id, UserId, ProductId , Status) VALUES ({orderId}, {UserId}, {productId} , 'Pending')";
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = query;
				cmd.ExecuteNonQuery();

				string query2 = $"DELETE FROM cart WHERE UserId = {UserId}";
				cmd2.CommandType = CommandType.Text;
				cmd2.CommandText = query2;
				cmd2.ExecuteNonQuery(); 

				MessageBox.Show("Success");
			}

			button3_Click(sender , e);
			button7_Click(sender , e);

		}



		private int GetMaxOrderID()
		{
			int maxId = 0;

			using (conn = new OracleConnection(ordb))
			{
				conn.Open();

				string query = "SELECT MAX(Id) FROM orders";
				OracleCommand command = new OracleCommand(query, conn);

				object result = command.ExecuteScalar();
				if (result != null && result != DBNull.Value)
				{
					maxId = Convert.ToInt32(result);
				}

				conn.Close();
			}

			return maxId + 1;
		}

		private void button7_Click(object sender, EventArgs e)
		{
			string cmd = $"SELECT p.Name ,p.list_price , p.Sale_Price, c.Status FROM product p  INNER JOIN Orders c ON p.id = c.ProductId WHERE c.UserId = {UserId}";

			adabter5 = new OracleDataAdapter(cmd, ordb);

			ds5 = new DataSet();
			adabter5.Fill(ds5);
			dataGridView5.DataSource = ds5.Tables[0];
		}
	}
}
