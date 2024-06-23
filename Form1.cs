using MySqlConnector;
using System;
using System.Data;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;

namespace testDB
{
	public partial class Form1 : Form
	{
		Database DataBS = new Database();
		MySqlCommand command;
		MySqlDataAdapter dataadapter;
		DataTable datatable;
		private string selectedTableName;

		Form2 F2check = new Form2();
		Form3 F3login = new Form3();
		Form4 F4insert = new Form4();
		public Form1()
		{
			InitializeComponent();
			StartPosition = FormStartPosition.CenterScreen;
			DataBS.Connect();
			tablesUpdate();
			comboBox5.Items.AddRange(new string[] { "По возрастанию", "По убыванию" });
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			auth();
			DataBS.cn.Open();
			this.WindowState = FormWindowState.Maximized;
			DataBS.cn.Close();
		}
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //вывод таблиц после выбора из выпадающего списка
		{
			selectedTableName = comboBox1.SelectedItem.ToString();
			string query = "SELECT * FROM " + selectedTableName;
			using (MySqlCommand cmd = new MySqlCommand(query, DataBS.cn))
			{
				cmd.CommandText = query;
				MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
				DataSet dataSet = new DataSet();
				adapter.Fill(dataSet, selectedTableName);
				dataGridView1.DataSource = dataSet.Tables[selectedTableName];
			}
			LoadData();
			int namecount = dataGridView1.ColumnCount;
			string res = "";
			for (int i = 0; i < namecount; i++)
			{
				string rowname = dataGridView1.Columns[i].Name;
				res += rowname + ";";
			} //заполнение выпадающих списков названиями столбцов для поиска и сортировки
			res = res.Remove(res.Length - 1);
			comboBox2.Items.Clear();
			comboBox2.Items.AddRange(res.Split(';'));
			comboBox2.Text = null;
			comboBox4.Items.Clear();
			comboBox4.Items.AddRange(res.Split(';'));
			comboBox4.Text = null;
		}
		public void tablesUpdate() //добавление названий таблиц в выпадающий список
		{
			comboBox1.Items.Clear();
			dataGridView1.DataSource = null;
			string tables = " ";
			command = new MySqlCommand("SHOW TABLES", DataBS.cn);
			dataadapter = new MySqlDataAdapter(command);
			datatable = new DataTable();
			dataadapter.Fill(datatable);
			dataGridView3.DataSource = datatable.DefaultView;
			int rowcount = dataGridView3.RowCount;
			if (F3login.DialogResult == DialogResult.Cancel)
			{
				this.Close();
			}
			for (int i = 0; i < rowcount - 1; i++)
			{
				string rowname = dataGridView3.Rows[i].Cells[0].Value.ToString();
				tables += rowname + ";";
			}
			comboBox1.Items.Clear(); //очистка комбобокса для правильного заполнения
			tables = tables.Remove(tables.Length - 1);
			tables = tables.Remove(0, 1);
			comboBox1.Items.AddRange(tables.Split(';')); //заносим список таблиц в комбобокс для выбора
			comboBox1.Text = comboBox1.Items[0].ToString();
		}
		private void LoadData() //вывод выбранной таблицы
		{
			try
			{
				DataBS.cn.Close();
				DataBS.cn.Open();
				command = new MySqlCommand($"SELECT * FROM {comboBox1.Text}", DataBS.cn);
				command.ExecuteNonQuery();
				datatable = new DataTable();
				dataadapter = new MySqlDataAdapter(command);
				dataadapter.Fill(datatable);
				dataGridView1.DataSource = datatable.DefaultView;
				DataBS.cn.Close();
			}
			catch (Exception ex) { MessageBox.Show(ex.Message); }
		}
		private void auth() //аутентификация и проверка уровня доступа учетной записи
		{
			F3login.ShowDialog();
			if (F3login.DialogResult == DialogResult.Cancel)
			{
				this.Close();
			}
			else
			{
				label3.Text = Form3.userName;

				tablesUpdate();
				/*if (Form3.accesslevel == 1)
				{
					tablesUpdate();
					if (comboBox1.Text == "" || comboBox1.Text == " ")
					{
						this.dataGridView1.Rows.Clear();
					}
					else
					{
						LoadData();
					}
					dataGridView1.ReadOnly = false;
					dataGridView1.AllowUserToAddRows = true;
					dataGridView1.AllowUserToDeleteRows = true;
					button1.Enabled = true;
					button2.Enabled = true;
					button3.Enabled = true;
					button4.Enabled = true;
					F4insert.EnableButton();
				}
				else if (Form3.accesslevel == 2)
				{
					tablesUpdate();
					if (comboBox1.Text == "" || comboBox1.Text == " ")
					{
						this.dataGridView1.DataSource = null;
					}
					else
					{
						LoadData();
					}
					button1.Enabled = true;
					button2.Enabled = true;
					button3.Enabled = false;
					button4.Enabled = true;
					dataGridView1.ReadOnly = false;
					dataGridView1.AllowUserToAddRows = true;
					F4insert.DisableButton();
					comboBox1.Items.Remove("users");
				}
				else if (Form3.accesslevel == 3)
				{
					tablesUpdate();
					if (comboBox1.Text == "" || comboBox1.Text == " ")
					{
						this.dataGridView1.DataSource = null;
					}
					else
					{
						LoadData();
					}
					button1.Enabled = false;
					button2.Enabled = false;
					button3.Enabled = false;
					button4.Enabled = true;
					dataGridView1.ReadOnly = true;
					dataGridView1.AllowUserToAddRows = false; // Запретить добавление новых строк
					dataGridView1.AllowUserToDeleteRows = false; // Запретить удаление строк
					comboBox1.Items.Remove("users");
				}
				else
				{
					MessageBox.Show("Неправильно определен уровень доступа");
					auth();
				}*/
			}
		}
		private void button1_Click(object sender, EventArgs e) //добавление нового столбца в таблицу
		{
			if (comboBox1.Text != "")
			{
				try
				{
					F4insert.tableName = comboBox1.Text;
					F4insert.ShowDialog();
					LoadData();
				}
				catch
				{
					button1_Click(sender, e);
				}
			}
			else
			{
				MessageBox.Show("Таблица не выбрана");
			}
		}
		private void button2_Click(object sender, EventArgs e) //добавление пустой строки в таблицу
		{
			try
			{
				DataBS.cn.Close();
				DataBS.cn.Open();
				DataBS.command = new MySqlCommand($"INSERT INTO {comboBox1.Text}() VALUES()", DataBS.cn);
				DataBS.command.ExecuteNonQuery();
				DataBS.cn.Close();
				LoadData();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}
		private void textBox2_TextChanged(object sender, EventArgs e)
		{ }
		private void button3_Click(object sender, EventArgs e) //удаление полностью выделенной строки из таблицы
		{
			if (dataGridView1.CurrentRow.Selected == false)
			{
				MessageBox.Show("Необходимо выделить всю строку");
			}
			else
			{
				F2check.ShowDialog(); //защита от дурака
				if (F2check.DialogResult == DialogResult.OK)
				{
					try
					{
						if (dataGridView1.CurrentRow.Selected)
						{
							int rowIndex = dataGridView1.CurrentRow.Index;
							DataBS.cn.Open();
							DataBS.command = new MySqlCommand($"DELETE FROM {comboBox1.Text} WHERE id = {Int32.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString())}", DataBS.cn);
							DataBS.command.ExecuteNonQuery();
							DataBS.cn.Close();

							dataGridView1.Rows.RemoveAt(rowIndex);
						}
					}
					catch
					{
						MessageBox.Show("Ошибка, повторите попытку удаления");
						button3_Click(sender, e);
					}
				}
			}
		}
		private void button4_Click(object sender, EventArgs e) //обновление таблицы
		{
			try
			{
				DataBS.cn.Close();
				DataBS.cn.Open();
				DataBS.cn.Close();
				LoadData();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) //заполнение таблицы данными при изменении значений в какой-либо ячейке
		{
			try
			{
				DataBS.cn.Close();
				DataBS.cn.Open();
				string quote = "";
				if (dataGridView1.CurrentCell.ValueType.ToString() != "System.Int32")
				{
					quote = "'";
				}
				DataBS.command = new MySqlCommand($"UPDATE {comboBox1.Text} " +
					$"SET {dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].Name} " +
					$"= {quote}{dataGridView1.CurrentCell.Value}{quote} " +
					$"WHERE id = {Int32.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString())}", DataBS.cn);
				DataBS.command.ExecuteNonQuery();
				LoadData();
			}
			catch
			{
				MessageBox.Show("Неверный тип данных");
			}
		}
		private void textBox1_TextChanged(object sender, EventArgs e)
		{ }
		private void button6_Click(object sender, EventArgs e) //смена учетной записи
		{
			auth();
		}
		private void button7_Click(object sender, EventArgs e) //поиск элементов
		{
			if (comboBox2.Text == null || comboBox2.Text == "")
			{
				MessageBox.Show("Столбец для поиска не выбран");
			}
			else
			{
				if (textBox1.Text == "")
				{
					LoadData();
				}
				else
				{
					DataBS.cn.Close();
					DataBS.cn.Open();
					command = new MySqlCommand($"SELECT * FROM {comboBox1.Text} WHERE {comboBox2.Text} LIKE '%{textBox1.Text}%'", DataBS.cn);
					command.ExecuteNonQuery();
					datatable = new DataTable();
					dataadapter = new MySqlDataAdapter(command);
					dataadapter.Fill(datatable);
					dataGridView1.DataSource = datatable.DefaultView;
					DataBS.cn.Close();
				}
				DataBS.cn.Close();
			}
		}
		private void button8_Click(object sender, EventArgs e) //сортировка выбранного столбца
		{
			if (comboBox4.Text == null || comboBox4.Text == "")
			{
				MessageBox.Show("Столбец для сортировки не выбран");
			}
			else if (comboBox5.Text == null || comboBox5.Text == "")
			{
				MessageBox.Show("Тип сортировки не выбран");
			}
			else if (comboBox5.Text == "По возрастанию")
			{
				try
				{
					DataBS.cn.Open();
					command = new MySqlCommand($"SELECT * FROM {comboBox1.Text} ORDER BY {comboBox4.Text} ASC", DataBS.cn);
					command.ExecuteNonQuery();
					datatable = new DataTable();
					dataadapter = new MySqlDataAdapter(command);
					dataadapter.Fill(datatable);
					dataGridView1.DataSource = datatable.DefaultView;
					DataBS.cn.Close();
				}
				catch
				{
					button8_Click(sender, e);
				}
			}
			else
			{
				try
				{
					DataBS.cn.Open();
					command = new MySqlCommand($"SELECT * FROM {comboBox1.Text} ORDER BY {comboBox4.Text} DESC", DataBS.cn);
					command.ExecuteNonQuery();
					datatable = new DataTable();
					dataadapter = new MySqlDataAdapter(command);
					dataadapter.Fill(datatable);
					dataGridView1.DataSource = datatable.DefaultView;
					DataBS.cn.Close();
				}
				catch
				{
					button8_Click(sender, e);
				}
			}
		}
		private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			MessageBox.Show("Неверный тип данных");
		}

		private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}