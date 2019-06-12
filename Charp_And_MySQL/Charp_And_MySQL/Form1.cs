using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charp_And_MySQL
{
    public partial class Form1 : Form
    {

        MySqlConnection connection = null;
        DataTable table = null;
        MySqlDataAdapter adapter = null;
        MySqlCommand command = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            viewDataSourceDB();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            tbID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            tbFName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            tbLName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            tbEmail.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            tbAge.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //MessageBox.Show(this, "CLOSE() Connection: ");
            closeConnection();
            //if(connection.State == ConnectionState.Open)
            //{
            //    MessageBox.Show(this, "CLOSE() Connection: ");
            //    connection.Close();
            //}
        }

        public void viewDataSourceDB()
        {
            string selectQuery = "SELECT * FROM users";
            connection = new MySqlConnection("datasource=localhost;port=3306; Initial Catalog='emailbd';username=root;password='5964458321'");

            //Если соединение с базой не установленно то нужно его установить openConnection();
            if (connection.State == ConnectionState.Closed)
            {
                openConnection();

                //Если соединение с базой установленно то выводим данные на DataGridView1
                if (connection.State == ConnectionState.Open)
                {

                    // MessageBox.Show(this, "OPEN DATABASE() Connection: ");
                    table = new DataTable();
                    adapter = new MySqlDataAdapter(selectQuery, connection);
                    adapter.Fill(table);


                    dataGridView1.DataSource = table;
                }
                else
                {
                    // MessageBox.Show(this, "ERROR Connection: ");
                }

            }
            else
            {
                //Если соединение уже установленно то закрываем его это гарантия только одного соединения(SINGLETON)
                closeConnection();
            }
            //---------------------------------------------------
        }

        public void openConnection()
        {
            //Если соединение с базой не установленно то открываем его
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            else {
                //Иначе соединение уже было установленно то нужно его закрыть.
                connection.Close(); 
            }
        }

        public void closeConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                // MessageBox.Show(this, "CLOSE() Connection: ");
                connection.Close();
            }
        }

        public void executeMyQuery(string query)
        {
            try
            {
               //Если соединение с базой установленно то выполняем SQL запрос
                if (connection.State == ConnectionState.Closed)
                {
                    // MessageBox.Show("Если соединение с базой закрыто то мы открываем его" + query);
                    openConnection();
                    executeMyQuery(query);
                } 
                else
                {
                    // MessageBox.Show(this, "executeMyQuery(): "+query);
                    command = new MySqlCommand(query, connection);

                    if (command.ExecuteNonQuery() == 1)
                    {
                        // MessageBox.Show("Query Executed Обновляем данные viewDataSourceDB()");
                        viewDataSourceDB();
                    }
                    else
                    {
                        // MessageBox.Show("Query Not Executed");
                    }

                  } 
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            } 
            finally
            {
                closeConnection();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string insertQuery = "INSERT INTO users(firstname,lastname,email,age) VALUES('"+tbFName.Text+"','"+tbLName.Text+"','"+tbEmail.Text+"','"+tbAge.Text+"');";
            // MessageBox.Show(insertQuery);
            executeMyQuery(insertQuery);
        }


        private void btnInsert_Click(object sender, EventArgs e)
        {
            string insertQuery = "INSERT INTO users(firstname,lastname,email,age) VALUES('" + tbFName.Text + "','" + tbLName.Text + "','" + tbEmail.Text + "','" + tbAge.Text + "');";
            // MessageBox.Show(insertQuery);
            executeMyQuery(insertQuery);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string updateQuery = "UPDATE users SET firstname='" + tbFName.Text + "',lastname='" + tbLName.Text + "',email='" + tbEmail.Text + "',age='" + tbAge.Text + "' WHERE id ='" + tbID.Text + "';";
            // MessageBox.Show(updateQuery);
            executeMyQuery(updateQuery);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string deleteQuery = "DELETE FROM users WHERE id = '" + tbID.Text + "';";
            // MessageBox.Show(deleteQuery);
            executeMyQuery(deleteQuery);
        }

    }
}
