using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
namespace ApIForms
{
    public partial class Form1 : Form
    {
        HttpClient client;
        public Form1()
        {
            InitializeComponent();
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:55821/api/");
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            HttpResponseMessage res = client.GetAsync("Students").Result;
            HttpResponseMessage mes = client.GetAsync("Departments").Result;
            if (res.IsSuccessStatusCode && mes.IsSuccessStatusCode)
            {
                List<StudentData> stds = res.Content.ReadAsAsync<List<StudentData>>().Result;
                dataGridView1.DataSource = stds.Select(a => new { a.Id, a.Name, a.Age, a.Email, a.Password, a.Department.DeptName }).ToList();

                List<Department> dpts = mes.Content.ReadAsAsync<List<Department>>().Result;
                comboBox1.DisplayMember = "DeptName";
                comboBox1.ValueMember = "Id";
                comboBox1.DataSource = dpts.ToList();
            }
            else
            {
                MessageBox.Show("Check your Internet Connection");
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!isDataValid())
            {
                MessageBox.Show("Plz Enter Yuor Data");
            }
            else
            {
                StudentData std = new StudentData()
                {
                    Name = textBox1.Text,
                    Age = (int)numericUpDown1.Value,
                    Email = textBox3.Text,
                    Password = textBox4.Text,
                    DeptId = (int)comboBox1.SelectedValue

                };

                HttpResponseMessage mes = client.PostAsJsonAsync("students", std).Result;
                if (mes.IsSuccessStatusCode)
                {
                    Form1_Load(null, null);
                    textBox1.Text = textBox3.Text = textBox4.Text = "";
                    numericUpDown1.Value = 0;
                    MessageBox.Show("Success Operation");
                }
                else
                {
                    MessageBox.Show("cant not Add Data");
                }
            }


        }

        private bool isDataValid()
        {
            if (
                textBox1.Text == "" ||
                numericUpDown1.Value <= 18 ||
                textBox3.Text == "" ||
                textBox4.Text == "" ||
                comboBox1.SelectedItem == null

                )
            {
                return false;
            }
            return true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex > 0)
            {
                this.dataGridView1.CurrentCell.Selected = false;
            }
            else
            {
                textBox1.Text = dataGridView1.SelectedCells[1].Value.ToString();
                numericUpDown1.Value = (int)dataGridView1.SelectedCells[2].Value;
                textBox3.Text = dataGridView1.SelectedCells[3].Value.ToString();
                textBox4.Text = dataGridView1.SelectedCells[4].Value.ToString();

                comboBox1.Text = string.Empty;
                comboBox1.Text = dataGridView1.SelectedCells[5].Value.ToString();

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!isDataValid())
            {
                MessageBox.Show("Plz Enter Yuor Data");
            }
            else
            {
                int id = (int)dataGridView1.SelectedCells[0].Value;
                StudentData std = new StudentData()
                {
                    Id = id,
                    Name = textBox1.Text,
                    Age = (int)numericUpDown1.Value,
                    Email = textBox3.Text,
                    Password = textBox4.Text,
                    DeptId = (int)comboBox1.SelectedValue

                };
                HttpResponseMessage mes = client.PutAsJsonAsync("students/" + id, std).Result;
                if (mes.IsSuccessStatusCode)
                {
                    Form1_Load(null, null);
                    textBox1.Text = textBox3.Text = textBox4.Text = "";
                    numericUpDown1.Value = 0;
                    MessageBox.Show("Success Operation");
                }
                else
                {
                    MessageBox.Show("cant not Add Data");
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id = 0;

            if (!(dataGridView1.SelectedRows.Count > 0))
            {
                MessageBox.Show("Selected plz");
                return;
            }
            id= (int)dataGridView1.SelectedCells[0].Value;
            HttpResponseMessage mes = client.DeleteAsync("students/" + id).Result;
            if (mes.IsSuccessStatusCode)
            {
                Form1_Load(null, null);
                textBox1.Text = textBox3.Text = textBox4.Text = "";
                numericUpDown1.Value = 0;
                MessageBox.Show("Success Operation");
            }
            else
            {
                MessageBox.Show("cant not Add Data");
            }
        }
    }
}
