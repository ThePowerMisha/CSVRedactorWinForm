using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSVRedactorWinForm
{
    public partial class Form2 : Form
    {
        Form1 FirstForm;
        public bool isadmin = false;
        public Form2(Form1 _form)
        {
            InitializeComponent();
            FirstForm = _form;
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatStyle = FlatStyle.Flat;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите данные");
            }
            else 
            {
                FirstForm.newColumnName = textBox1.Text;
                FirstForm.Enabled = true;
                var col = new DataGridViewTextBoxColumn();

                col.Name = FirstForm.newColumnName;

                FirstForm.dataGridView1.Columns.Add(col);
                this.Close();
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FirstForm.newColumnName = "";
            FirstForm.Enabled = true;
            this.Close();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //сохранить всё
        
        FirstForm.Enabled = true;
            
        }
    }
}
