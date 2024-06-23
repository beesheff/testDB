using System;
using System.Windows.Forms;

namespace testDB
{
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
			StartPosition = FormStartPosition.CenterScreen;
		}

		private void Form2_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
	}
}
