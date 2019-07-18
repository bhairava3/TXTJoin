using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace TXTJoin
{
    public partial class Form1 : Form
    {
        HashSet<string> resultCollection = new HashSet<string>();

        long allRows = 0;
        long dublicateRows = 0;
        long resultRows = 0;

        public Form1()
        {
            InitializeComponent();
        }      

        private void SelectFiles(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                Multiselect = true,
                Title = "Выбор файлов"
            };

            od.ShowDialog();

            if (od.FileName == String.Empty)
            {
                return;
            }

            foreach (string file in od.FileNames)
            {                
                FileToCollection(file);                
            }

            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Maximum = resultCollection.Count;

            int i = 1;
            foreach (String row in resultCollection)
            {
                PBar(i);
                i++;
            }

            String result = string.Join(Environment.NewLine, resultCollection);
            richTextBox1.Text = result;

            dublicateRows = allRows - resultCollection.Count;
            resultRows = resultCollection.Count;

            toolStripStatusLabel1.Text = "Всего строк: " + allRows.ToString();
            toolStripStatusLabel2.Text = "Дублей: " + dublicateRows.ToString();
            toolStripStatusLabel3.Text = "Уникальных: " + resultRows.ToString();

            ShowFields();
        }

        private void ClearSelected(object sender, EventArgs e)
        {
            resultCollection.Clear();
            richTextBox1.Text = "";
            toolStripStatusLabel1.Text = "Всего строк: 0";
            toolStripStatusLabel2.Text = "Дублей: 0";
            toolStripStatusLabel3.Text = "Уникальных: 0";

            toolStripProgressBar1.Maximum = 0;
            toolStripProgressBar1.Value = 0;

            HideFields();
        }

        private void SaveFile(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                Title = "Сохранение результата",
                FileName = "Result"
            };

            if (sd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sd.FileName);

                sw.WriteLine(richTextBox1.Text);

                sw.Close();

                richTextBox1.Text = richTextBox1.Text + "Готово!";
            }
        }
               
        void ShowFields()
        {
            button3.Visible = true;
            button2.Enabled = true;
            button4.Visible = true;
            сохранитьРезультатToolStripMenuItem.Visible = true;
            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel1.Visible = true;
            toolStripStatusLabel2.Visible = true;
            toolStripStatusLabel3.Visible = true;            
        }

        void HideFields()
        {
            button3.Visible = false;
            button2.Enabled = false;
            button4.Visible = false;
            сохранитьРезультатToolStripMenuItem.Visible = false;
            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Visible = false;
            toolStripStatusLabel2.Visible = false;
            toolStripStatusLabel3.Visible = false;
        }

        void FileToCollection(String filepath)
        {
            try
            {
                string s;

                StreamReader f = new StreamReader(filepath);
                while (!f.EndOfStream)
                {
                    s = f.ReadLine();

                    resultCollection.Add(s);                    

                    allRows++;
                }
                f.Close();                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        async void PBar(int val)
        {
            toolStripProgressBar1.Value = val;
            await Task.Delay(1);
        }

        private void CopyClipboard(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.Text);
        }
    }
}
