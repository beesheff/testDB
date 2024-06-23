using MySqlConnector;
using System;
using System.Windows.Forms;

namespace testDB
{
	public partial class Form4 : Form
	{
		Database DB = new Database();
		public string tableName;
		Form2 f2 = new Form2();
		public Form4()
		{
			InitializeComponent();
			StartPosition = FormStartPosition.CenterScreen;
		}
		private void Form4_Load(object sender, EventArgs e) //вывод названия текущей таблицы для добавления столбца
		{
			DB.Connect();
			label1.Text = "";
			label1.Text = $"Таблица: {tableName}";
		}
		private void button1_Click(object sender, EventArgs e) //добавление столбца
		{
			DB.cn.Open();
			string colName = textBox1.Text;
			string dataType = textBox2.Text;
			try
			{
				MySqlCommand command = new MySqlCommand($"ALTER TABLE {tableName} ADD COLUMN {colName} {dataType}", DB.cn);
				command.ExecuteNonQuery();
				MessageBox.Show("Столбец успешно добавлен");
				DB.Connect();
			}
			catch
			{
				MessageBox.Show("Ошибка заполнения!");
			}
		}
		private void button2_Click(object sender, EventArgs e) //закрытие формы
		{
			textBox1.Clear();
			textBox2.Clear();
			Close();
		}
		private void button3_Click(object sender, EventArgs e) //удаление колонки по названию
		{
			DB.cn.Open();
			string colName = textBox1.Text;
			textBox2.Clear();
			f2.ShowDialog();
			if (f2.DialogResult == DialogResult.OK)
			{
				try
				{
					MySqlCommand command = new MySqlCommand($"ALTER TABLE {tableName} DROP COLUMN {colName}", DB.cn);
					command.ExecuteNonQuery();
					MessageBox.Show("Столбец успешно удален");
					DB.Connect();
				}
				catch
				{
					MessageBox.Show("Ошибка заполнения!");
				}
			}
		}
		public void DisableButton() //отключение кнопки удаления для низкоуровневых пользователей
		{
			button3.Enabled = false;
		}
		public void EnableButton() //включение кнопки удаления
		{
			button3.Enabled = true;
		}
	}
}