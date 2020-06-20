using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DataGridView
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openDialog = new OpenFileDialog();
        private SaveFileDialog saveDialog = new SaveFileDialog();
        private bool changed = false;

        public Form1()
        {
            InitializeComponent();

            openDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;

            saveDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveDialog.AddExtension = true;
            saveDialog.FilterIndex = 1;
            saveDialog.RestoreDirectory = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                DialogResult result = MessageBox.Show("Save changes?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:
                        {
                            SaveAs();
                            Open();
                            break;
                        }
                    case DialogResult.No:
                        {
                            Open();
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            return;
                        }
                }        
            }
            else Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                DialogResult result = MessageBox.Show("Save changes?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:
                        {
                            SaveAs();
                            New();
                            break;
                        }
                    case DialogResult.No:
                        {
                            New();
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            return;
                        }
                }
            }
            else New();
        }
        private void New()
        {
            if (changed)
            {
                DialogResult result2 = MessageBox.Show("Save changes?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result2)
                {
                    case DialogResult.Yes:
                        {
                            SaveAs();
                            break;
                        }
                    case DialogResult.No:
                        {
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            return;
                        }
                }
            }

            New select = new New();
            DialogResult result = select.ShowDialog();

            switch(result)
            {
                case DialogResult.OK:
                    {
                        dataSet1.Clear();
                        dataSet1.Tables.Clear();
                        dataSet1.Tables.Add(new DataTable(select.textBox2.Text.Trim(new char[] { ' ' })));
                        foreach (string str in select.textBox1.Text.Split(','))

                            dataSet1.Tables[0].Columns.Add(str.Trim(new char[] { ' ' }), typeof(string));

                        dataGridView1.DataSource = dataSet1.Tables[0];
                        dataGridView1.Update();
                        dataGridView1.Refresh();

                        break;
                    }
                case DialogResult.Cancel:
                    return;
            }
            
        }

        private void Open()
        {
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                dataSet1.Clear();
                dataSet1.Tables.Clear();
                // Code to write the stream goes here.
                dataSet1.ReadXml(openDialog.FileName);
                dataGridView1.DataSource = dataSet1.Tables[0];
                dataGridView1.Update();
                dataGridView1.Refresh();
            }
        }

        private void SaveAs()
        {
            Stream stream;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveDialog.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    stream.Close();
                    dataSet1.WriteXml(saveDialog.FileName);
                    changed = false;
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            changed = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changed)
            {
                DialogResult result = MessageBox.Show("Save changes?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:
                        {
                            SaveAs();
                            break;
                        }
                    case DialogResult.No:
                        {
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            e.Cancel = true;
                            break;
                        }
                }
            }
            else return;
        }

    }
}
