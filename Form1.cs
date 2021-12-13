using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;



namespace CSVRedactorWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.SortCompare += new DataGridViewSortCompareEventHandler(
            this.dataGridView1_SortCompare);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            comboBox1.SelectedIndex = 1;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
                label2.Text = $"Стр {dataGridView1.CurrentCell.RowIndex + 1 } Стлб {dataGridView1.CurrentCell.ColumnIndex + 1}";
            else label2.Text = "Стр 0 Стлб 0";
        }
        string currentDeliment = ",";
        public string newColumnName = "";
        string Path;
        string encodingString  =  "utf-8";
        ObservableCollection<ObservableCollection<dynamic>> users = new ObservableCollection<ObservableCollection<dynamic>>();

        private void dataGridView1_SortCompare(object sender,
        DataGridViewSortCompareEventArgs e)
        {

            // Try to sort based on the cells in the current column.
            if (e.CellValue1 == null) {
                e.SortResult = -1;
            }
            else if (e.CellValue2 == null)
            {
                e.SortResult = 1;
            }

            else if (double.TryParse(e.CellValue1.ToString().Replace(".", ","), out double number1) && double.TryParse(e.CellValue2.ToString().Replace(".", ","), out double number2))
            {
                e.SortResult = number1.CompareTo(number2);
            }
            else
            {
                e.SortResult = System.String.Compare(
                    e.CellValue1.ToString(), e.CellValue2.ToString()); // descending sort
            }
            e.Handled = true;
        }

        private void save()
        {
            if (Path == null) 
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "Document"; // Default file name
                dlg.DefaultExt = ".csv"; // Default file extension
                dlg.Filter = "csv file (.csv)|*.csv| txt files (*.txt)|*.txt"; // Filter files by extension

                // Show save file dialog box


                // Process save file dialog box results
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // Save document
                    Path = dlg.FileName;
                }
            };
            using (var reader = new StreamWriter(Path,false,Encoding.GetEncoding(encodingString)))
            {
                string text = "";
                if (dataGridView1.Columns.Count == 0) return;
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    text += column.Name + currentDeliment;
                }

                text = text.Remove(text.Length - 1);
                reader.WriteLine(text);
                if (dataGridView1.Rows.Count == 0) return;
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    text = "";
                    dynamic some = dataGridView1.Rows[i];
                    try
                    {
                        for (int j = 0; j < some.Cells.Count; j++)
                        {
                            text += some.Cells[j].Value + currentDeliment;
                        }
                        text = text.Remove(text.Length - 1);
                        reader.WriteLine(text);
                    }
                    catch (Exception exept)
                    {
                        Console.WriteLine(exept);
                    }
                }
                reader.Close();
            }
        }
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            encodingString = comboBox1.SelectedItem.ToString();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV FILE (*.csv)|*.csv| txt files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                Path = openFileDialog.FileName;

            if (Path == null)
            {
                MessageBox.Show("Пусть к CSV файлу не указан", "Ok");
                return;
            }
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            users.Clear();
            using (var reader = new StreamReader(Path, Encoding.GetEncoding(encodingString)))
            {

                int first = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (first == 0) {
                        var val1 = line.Split(";");
                        var val2 = line.Split(",");
                        if (val1.Length > val2.Length)
                        {
                            currentDeliment = ";";
                        }
                        else currentDeliment = ",";
                    }
                    var values = line.Split(currentDeliment);
                    

                    if (first == 0)
                    {              
                        for (int i = 0; i < values.Length; i++)
                        {
                            var col = new DataGridViewTextBoxColumn();

                            col.Name = values[i];

                            dataGridView1.Columns.Add(col);
                        }
                        first++;
                    }
                    else
                    {
                        dataGridView1.Rows.Add(values);
                    }
                }
                reader.Close();
            }

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "csv file (.csv)|*.csv| txt files (*.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box


            // Process save file dialog box results
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // Save document
                Path = dlg.FileName;
                save();
            }
        }

        private void добавитьКолонкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            form2.Show();
            this.Enabled = false;
        }

        private void удалитьКолонкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell == null) return;
                int rowIndex = dataGridView1.CurrentCell.ColumnIndex;
                dataGridView1.Columns.RemoveAt(rowIndex);
            }
            catch
            {
                return;
            }
        }

    }
}
