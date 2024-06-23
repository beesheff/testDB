using MySqlConnector;
using System;
using System.Data;
using System.Windows.Forms;

namespace testDB
{
	public partial class Form3 : Form
	{
		Database DB = new Database();
		public string loginUser;
		public string passwordUser;
		//public static int accesslevel;
		public static string accesslevel;
		public static string userName;
		public Form3()
		{
			InitializeComponent();
			StartPosition = FormStartPosition.CenterScreen;
			DB.Connect();
			DB.cn.Open();
		}
		private void Form3_Load(object sender, EventArgs e)
		{
			textBox1.MaxLength = 20; //устанавливаем максимально вводимую длину логина и пароля
			textBox2.MaxLength = 20;
		}
		private void label2_Click(object sender, EventArgs e)
		{ }
		private void textBox1_TextChanged(object sender, EventArgs e)
		{ }
		private void textBox2_TextChanged(object sender, EventArgs e)
		{ }
		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				/*MySqlCommand command = new MySqlCommand($"SELECT Login, Password, access_level FROM users " +
					$"WHERE (Login = '{textBox1.Text}') AND (Password = '{textBox2.Text}')", DB.cn);*/
				MySqlCommand command = new MySqlCommand($"SELECT login, password FROM admins " +
					$"WHERE (login = '{textBox1.Text}') AND (password = '{textBox2.Text}')", DB.cn);
				command.ExecuteNonQuery();
				DataTable table = new DataTable();
				MySqlDataAdapter adapter = new MySqlDataAdapter(command);
				adapter.Fill(table);
				dataGridView1.DataSource = table.DefaultView;

				if (table.Rows.Count > 0)
				{
					loginUser = table.Rows[0]["login"].ToString();
					passwordUser = table.Rows[0]["password"].ToString();

					if (textBox1.Text == loginUser && textBox2.Text == passwordUser)
					{
						MessageBox.Show($"Добро пожаловать, {loginUser}");
						userName = textBox1.Text;
						//accesslevel = Convert.ToInt32(table.Rows[0]["access_level"].ToString());
						//accesslevel = table.Rows[0]["user_type"].ToString();
						textBox1.Text = null;
						textBox2.Text = null;
						DialogResult = DialogResult.OK;
					}
					else
					{
						MessageBox.Show("Неверный логин или пароль");
					}
				}
				else
				{
					MessageBox.Show("Неверный логин или пароль");
				}
			}
			catch
			{
				MessageBox.Show("Неверный логин или пароль");
			}
		}
		private void checkBox1_CheckedChanged(object sender, EventArgs e) //скрытый ввод пароля и его показ по нажатию на кнопку(checkbox)
		{
			if (textBox2.PasswordChar == '*')
			{
				textBox2.PasswordChar = '\0';
			}
			else
			{
				textBox2.PasswordChar = '*';
			}
		}
		private void button2_Click(object sender, EventArgs e) //закрытие формы входа, а также основной формы, если была открыта ранее
		{
			this.Close();
		}
	}
}
