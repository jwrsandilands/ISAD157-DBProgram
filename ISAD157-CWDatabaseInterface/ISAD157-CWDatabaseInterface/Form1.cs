using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ISAD157_CWDatabaseInterface
{
    public partial class FBAdminDataviewer : Form
    {
        //### APPLICATION START ###

        //Setup App
        public FBAdminDataviewer()
        {
            InitializeComponent();
            setup_combo_box2();
            ShowAll();
        }

        //Detect when the Table Search Combobox is changed
        private void searchComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchComboBox2.Items.Clear();
            if (searchComboBox1.Text == "Users")
            {
                setup_combo_box2();
            }
            else if (searchComboBox1.Text == "Universities")
            {
                searchComboBox2.Text = "User ID";
                searchComboBox2.Items.Add("User ID");
                searchComboBox2.Items.Add("University");
                searchComboBox2.Items.Add("Attending Dates");
            }
            else if (searchComboBox1.Text == "Workplaces")
            {
                searchComboBox2.Text = "User ID";
                searchComboBox2.Items.Add("User ID");
                searchComboBox2.Items.Add("Workplace");
                searchComboBox2.Items.Add("Attending Dates");
            }
            else if (searchComboBox1.Text == "Friendships")
            {
                searchComboBox2.Text = "User ID";
                searchComboBox2.Items.Add("User ID");
                searchComboBox2.Items.Add("Friend ID");
            }
            else if (searchComboBox1.Text == "Messages")
            {
                searchComboBox2.Text = "Sender ID";
                searchComboBox2.Items.Add("Sender ID");
                searchComboBox2.Items.Add("Reciever ID");
                searchComboBox2.Items.Add("Date");
                searchComboBox2.Items.Add("Message Content");
            }
        }

        //This is used to pull the information in Column combobox as it is on startup
        private void setup_combo_box2()
        {
            searchComboBox2.Text = "User ID";
            searchComboBox2.Items.Add("User ID");
            searchComboBox2.Items.Add("User First Name");
            searchComboBox2.Items.Add("User Second Name");
            searchComboBox2.Items.Add("User Gender");
            searchComboBox2.Items.Add("User Current Relationship Status");
            searchComboBox2.Items.Add("User Hometown");
            searchComboBox2.Items.Add("User Current Residence");
        }


        //### RETURN TABLE DATA ###

        //When a search is complete, hit the go button to return data
        private void GoBtn_Click(object sender, EventArgs e)
        {
            //establish connection
            string connectionString = Connect();
            string userquery;
            string searchText = searchbarTextBox.Text;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //Perform search and find results
                string tableChoice = searchComboBox1.Text;
                string columnChoice = searchComboBox2.Text;

                if (searchText == "")
                {
                    ShowAll();
                }
                else if (tableChoice == "Users")
                {
                    if (columnChoice == "User ID")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender = " + searchText;
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Please input a valid integer from 1-5000.", "Invalid User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else if (columnChoice == "User First Name")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.first_name LIKE '%" + searchText + "%'";
                            QueryStore.sort[0] = userquery;
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id  IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.first_name LIKE '%" + searchText + "%')";
                            QueryStore.sort[1] = uniquery;
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.first_name LIKE '%" + searchText + "%')";
                            QueryStore.sort[2] = workquery;
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.first_name LIKE '%" + searchText + "%')";

                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.first_name LIKE '%" + searchText + "%')";

                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen name was not found. To solve this, please input a valid string of characters, or First Name.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "User Second Name")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.last_name LIKE '%" + searchText + "%'";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id  IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.last_name LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.last_name LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.last_name LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.last_name LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen name was not found. To solve this, please input a valid string of characters, or Second Name.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "User Gender")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.gender = '" + searchText + "'";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id  IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.gender = '" + searchText + "')";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.gender = '" + searchText + "')";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.gender = '" + searchText + "')";
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.gender = '" + searchText + "')";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen gender was not found. To solve this, please input a valid string of characters, or gender.", "Invalid gender", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "User Current Relationship Status")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.relationship LIKE '%" + searchText + "%'";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id  IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.relationship LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.relationship LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.relationship LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.relationship LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen relationship status was not found. To solve this, please input a valid string of characters, or relationship status.", "Invalid Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "User Hometown")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.hometown LIKE '%" + searchText + "%'";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id  IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.hometown LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.hometown LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.hometown LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.hometown LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen Hometown was not found. To solve this, Please input a valid string of characters, or hometown.", "Invalid hometown name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "User Current Residence")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.residence LIKE '%" + searchText + "%'";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id  IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.residence LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.residence LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.residence LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender IN (SELECT isad157_jsandilands.users.user_id FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.residence LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen city of residence was not found. To solve this, please input a valid string of characters, or residence.", "Invalid city of residence", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please input a valid column name.", "Invalid Column", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (tableChoice == "Universities")
                {
                    if (columnChoice == "User ID")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender = " + searchText;
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Please input a valid integer from 1-5000.", "Invalid User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "University")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id IN (SELECT isad157_jsandilands.users_education.user_id FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.education LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.education LIKE '%" + searchText + "%'";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id IN (SELECT isad157_jsandilands.users_education.user_id FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.education LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.users_education.user_id FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.education LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender IN (SELECT isad157_jsandilands.users_education.user_id FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.education LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen university was not found. To solve this, please input a valid string of characters, or university.", "Invalid University Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "Attending Dates")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id IN (SELECT isad157_jsandilands.users_education.user_id FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.timeofuni LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.timeofuni LIKE '%" + searchText + "%'";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id IN (SELECT isad157_jsandilands.users_education.user_id FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.timeofuni LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.users_education.user_id FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.timeofuni LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender IN (SELECT isad157_jsandilands.users_education.user_id FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.timeofuni LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen attending date was not found. To solve this, please input a valid string of characters, or date. This will be in the format 'DD MM YYYY' OR 'DD MM YYYY - DD MM YYYY'", "Invalid University Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please input a valid column name.", "Invalid Column", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (tableChoice == "Workplaces")
                {
                    if (columnChoice == "User ID")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender = " + searchText;
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Please input a valid integer from 1-5000.", "Invalid User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "Workplace")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id IN (SELECT isad157_jsandilands.users_workplaces.user_id FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.workplace LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id IN (SELECT isad157_jsandilands.users_workplaces.user_id FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.workplace LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.workplace LIKE '%" + searchText + "%'";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.users_workplaces.user_id FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.workplace LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender IN (SELECT isad157_jsandilands.users_workplaces.user_id FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.workplace LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen workplace was not found. To solve this, lease input a valid integer string of characters, or workplace.", "Invalid Workplace Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "Attending Dates")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id IN (SELECT isad157_jsandilands.users_workplaces.user_id FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.timeofwork LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id IN (SELECT isad157_jsandilands.users_workplaces.user_id FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.timeofwork LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.timeofwork LIKE '%" + searchText + "%'";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.users_workplaces.user_id FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.timeofwork LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender IN (SELECT isad157_jsandilands.users_workplaces.user_id FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.timeofwork LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen attending date was not found. To solve this, please input a valid string of characters, or date. This will be in the format 'DD MM YYYY' OR 'DD MM YYYY - DD MM YYYY'", "Invalid Workplace Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please input a valid column name.", "Invalid Column", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (tableChoice == "Friendships")
                {
                    if (columnChoice == "User ID")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender = " + searchText;
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Please input a valid integer from 1-5000.", "Invalid User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "Friend ID")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.friend_id = " + searchText;
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender = " + searchText;
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Please input a valid integer from 1-5000.", "Invalid Friend ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please input a valid column name.", "Invalid Column", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (tableChoice == "Messages")
                {
                    if (columnChoice == "Sender ID")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_sender = " + searchText;
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Please input a valid integer from 1-5000.", "Invalid User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "Reciever ID")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id = " + searchText;
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.user_id_reciever = " + searchText;
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Please input a valid integer from 1-5000.", "Invalid User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "Date")
                    {
                        try
                        {
                            string[] dates = searchText.Split(',');
                            string searchText1 = dates[0];
                            string searchText2 = dates[1];


                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id IN (SELECT isad157_jsandilands.messages.user_id_sender FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.date_time >= '" + searchText1 + " 12:00:00 AM' AND isad157_jsandilands.messages.date_time <= '" + searchText2 + " 12:00:00 AM')";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id IN (SELECT isad157_jsandilands.messages.user_id_sender FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.date_time >= '" + searchText1 + " 12:00:00 AM' AND isad157_jsandilands.messages.date_time <= '" + searchText2 + " 12:00:00 AM')";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id IN (SELECT isad157_jsandilands.messages.user_id_sender FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.date_time >= '" + searchText1 + " 12:00:00 AM' AND isad157_jsandilands.messages.date_time <= '" + searchText2 + " 12:00:00 AM')";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.messages.user_id_sender FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.date_time >= '" + searchText1 + " 12:00:00 AM' AND isad157_jsandilands.messages.date_time <= '" + searchText2 + " 12:00:00 AM')";
                            connection.Open();
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.date_time >= '" + searchText1 + " 12:00:00 AM' AND isad157_jsandilands.messages.date_time <= '" + searchText2 + " 12:00:00 AM'";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Please input a valid set of dates. This can be any date input within the database's range. The search format is as follows 'YYYY-MM-DD,YYYY-MM-DD'. Please do not include spaces in this format.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (columnChoice == "Message Content")
                    {
                        try
                        {
                            userquery = "SELECT * FROM isad157_jsandilands.users WHERE isad157_jsandilands.users.user_id IN (SELECT isad157_jsandilands.messages.user_id_sender FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.msg_text LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                            DataTable userDataTable = new DataTable();
                            usersqlDA.Fill(userDataTable);
                            usersDataGrid.DataSource = userDataTable;
                            connection.Close();

                            string uniquery = "SELECT * FROM isad157_jsandilands.users_education WHERE isad157_jsandilands.users_education.user_id IN (SELECT isad157_jsandilands.messages.user_id_sender FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.msg_text LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                            MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                            DataTable uniDataTable = new DataTable();
                            unisqlDA.Fill(uniDataTable);
                            universitiesDataGrid.DataSource = uniDataTable;
                            connection.Close();

                            string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE isad157_jsandilands.users_workplaces.user_id IN (SELECT isad157_jsandilands.messages.user_id_sender FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.msg_text LIKE '%" + searchText + "%')";
                            connection.Open();
                            MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                            MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                            DataTable workDataTable = new DataTable();
                            worksqlDA.Fill(workDataTable);
                            workDataGrid.DataSource = workDataTable;
                            connection.Close();

                            string friendquery = "SELECT * FROM isad157_jsandilands.friendships WHERE isad157_jsandilands.friendships.user_id IN (SELECT isad157_jsandilands.messages.user_id_sender FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.msg_text LIKE '%" + searchText + "%')";
                            MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                            MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                            DataTable friendDataTable = new DataTable();
                            friendsqlDA.Fill(friendDataTable);
                            friendsDataGrid.DataSource = friendDataTable;
                            connection.Close();

                            string messagesquery = "SELECT * FROM isad157_jsandilands.messages WHERE isad157_jsandilands.messages.msg_text LIKE '%" + searchText + "%'";
                            connection.Open();
                            MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                            MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                            DataTable messagesDataTable = new DataTable();
                            messagessqlDA.Fill(messagesDataTable);
                            messageDataGrid.DataSource = messagesDataTable;
                            connection.Close();

                            //store queries for sort

                            QueryStore.sort[0] = userquery;
                            QueryStore.sort[1] = uniquery;
                            QueryStore.sort[2] = workquery;
                            QueryStore.sort[3] = friendquery;
                            QueryStore.sort[4] = messagesquery;
                        }
                        catch
                        {
                            MessageBox.Show("Your chosen text was not found. To solve this, lease input a valid integer string of characters, or text.", "Invalid Message Text", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please input a valid column name.", "Invalid Column", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please input a valid table name.", "Invalid Table", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //button that refers to the show all data action below
        private void DisplayButton_Click(object sender, EventArgs e)
        {
            //fill content
            {
                //fill 1
                {
                ////////////////////////////////////
                //  //TEMPORARY CONTENT FILLER (For User Status)
                //  
                //  //establish connection
                //  string connectionString = Connect();
                //  using (MySqlConnection connection = new MySqlConnection(connectionString))
                //  {
                //      int useridtotal = 5000;
                //      int randomGen;
                //      string status;
                //  
                //      for (int userid = 1; userid <= useridtotal; userid++)
                //      {
                //      randomGen = RandomNumber(1, 5);
                //  
                //      if (randomGen == 1)
                //      {
                //          status = "single";
                //      }
                //      else if (randomGen == 2)
                //      {
                //          status = "engaged";
                //      }
                //      else if (randomGen == 3)
                //      {
                //          status = "married";
                //      }
                //      else if (randomGen == 4)
                //      {
                //      status = "its complicated";
                //      }
                //      else
                //      {
                //          status = "oops";
                //      }
                //  
                //  string query = "UPDATE isad157_jsandilands.users SET relationship = '" + status + "' WHERE isad157_jsandilands.users.user_id = " + userid;
                //  connection.Open();
                //  MySqlCommand usercmd = new MySqlCommand(query, connection);
                //  MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                //  DataTable userDataTable = new DataTable();
                //  usersqlDA.Fill(userDataTable);
                //  usersDataGrid.DataSource = userDataTable;
                //  connection.Close();
                ////////////////////////////////////
            }
                //fill 2
                {
                ////////////////////////////////////
                //  TEMPORARY CONTENT FILLER VOL 2 (For Workplaces and Education)
                //  string temprecords;
                //  string query;
                //
                //  establish connection
                //  string connectionString = Connect();
                //  using (MySqlConnection connection = new MySqlConnection(connectionString))
                //  {
                //      int useridtotal = 5000;
                //      string randomGen = "";
                //      int maxRowNumber;
                //
                //
                //      for (int userid = 1; userid <= useridtotal; userid++)
                //      {
                //          query = "SELECT * FROM isad157_jsandilands.users_workplaces WHERE user_id = " + userid;
                //          connection.Open();
                //          MySqlCommand usercmd = new MySqlCommand(query, connection);
                //          MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                //          DataTable userDataTable = new DataTable();
                //          usersqlDA.Fill(userDataTable);
                //          connection.Close();
                //
                //          if(userDataTable.Rows[0].ItemArray[1] != "")
                //          {
                //              maxRowNumber = userDataTable.Rows.Count;
                //              for (int row = 0; row <= maxRowNumber - 1; row++ )
                //              {
                //                  temprecords = userDataTable.Rows[row].ItemArray[1].ToString();
                //  
                //                  int dayOne = RandomNumber(1, 31);
                //                  int monthOne = RandomNumber(1, 12);
                //                  int yearOne = RandomNumber(2015, 2019);
                //
                //                  int dayTwo = RandomNumber(1, 31);
                //                  int monthTwo = RandomNumber(monthOne, 13);
                //                  int yearTwo = RandomNumber(2015, 2021);
                //
                //                  if (yearTwo == 2020)
                //                  {
                //                      randomGen = Convert.ToString(dayOne) + " " + Convert.ToString(monthOne) + " " + Convert.ToString(yearOne) + " - Present";
                //                  }
                //                  else
                //                  {
                //                      randomGen = Convert.ToString(dayOne) + " " + Convert.ToString(monthOne) + " " + Convert.ToString(yearOne) + " - " + Convert.ToString(dayTwo) + " " + Convert.ToString(monthTwo) + " " + Convert.ToString(yearTwo);
                //                  }
                //  
                //                  query = "UPDATE isad157_jsandilands.users_workplaces SET timeofwork = '" + randomGen + "' WHERE isad157_jsandilands.users_workplaces.user_id = " + userid + " AND isad157_jsandilands.users_workplaces.workplace = '" + temprecords+ "'";
                //                  connection.Open();
                //                  usercmd = new MySqlCommand(query, connection);
                //                  usersqlDA = new MySqlDataAdapter(usercmd);
                //                  DataTable tempStoreTable = new DataTable();
                //                  usersqlDA.Fill(tempStoreTable);
                //                  connection.Close();
                //              }
                //          }
                //      }
                //  }
                //  
                ////////////////////////////////////
            }
            }

            ShowAll();
        }


        //Temporary random number generator for extra info columns
        public int RandomNumber(int min, int max)
        {
            Random Random = new Random();
            return Random.Next(min, max);
        }

        //action that shows all data
        private void ShowAll()
        {

            //establish connection
            string connectionString = Connect();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {            
                try
                {
                    string userquery = "SELECT * FROM isad157_jsandilands.users";
                    connection.Open();
                    MySqlCommand usercmd = new MySqlCommand(userquery, connection);
                    MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                    DataTable userDataTable = new DataTable();
                    usersqlDA.Fill(userDataTable);
                    usersDataGrid.DataSource = userDataTable;
                    connection.Close();

                    string uniquery = "SELECT * FROM isad157_jsandilands.users_education";
                    connection.Open();
                    MySqlCommand unicmd = new MySqlCommand(uniquery, connection);
                    MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                    DataTable uniDataTable = new DataTable();
                    unisqlDA.Fill(uniDataTable);
                    universitiesDataGrid.DataSource = uniDataTable;
                    connection.Close();

                    string workquery = "SELECT * FROM isad157_jsandilands.users_workplaces";
                    connection.Open();
                    MySqlCommand workcmd = new MySqlCommand(workquery, connection);
                    MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                    DataTable workDataTable = new DataTable();
                    worksqlDA.Fill(workDataTable);
                    workDataGrid.DataSource = workDataTable;
                    connection.Close();

                    string friendquery = "SELECT * FROM isad157_jsandilands.friendships";
                    connection.Open();
                    MySqlCommand friendcmd = new MySqlCommand(friendquery, connection);
                    MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                    DataTable friendDataTable = new DataTable();
                    friendsqlDA.Fill(friendDataTable);
                    friendsDataGrid.DataSource = friendDataTable;
                    connection.Close();

                    string messagesquery = "SELECT * FROM isad157_jsandilands.messages";
                    connection.Open();
                    MySqlCommand messagescmd = new MySqlCommand(messagesquery, connection);
                    MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                    DataTable messagesDataTable = new DataTable();
                    messagessqlDA.Fill(messagesDataTable);
                    messageDataGrid.DataSource = messagesDataTable;
                    connection.Close();

                    //store queries for sort

                    QueryStore.sort[0] = userquery;
                    QueryStore.sort[1] = uniquery;
                    QueryStore.sort[2] = workquery;
                    QueryStore.sort[3] = friendquery;
                    QueryStore.sort[4] = messagesquery;
                }
                catch
                {
                    MessageBox.Show("Please check your connection settings!", "Connection Not Established!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        //connection string compiler code
        private string Connect()
        {
            string connectionString = "SERVER=" + DBConnect.SERVER + ";" + "DATABASE=" + DBConnect.DATABASE_NAME + ";" + "UID=" + DBConnect.USER_NAME + ";" + "PASSWORD=" + DBConnect.PASSWORD + ";" + "SslMode=" + DBConnect.SslMode + ";";
            return connectionString;
        }


        //### SORTING CODE ###
        
        //Used Only To stop the program from crashing for now, old code reference.
        private void UDIDSortCheckBox_CheckedChanged(object sender, EventArgs e)
        {                
        //    //establish connection
        //    string connectionString = Connect();
        //
        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        if (UDIDSortCheckBox.Checked)
        //        {
        //
        //            string query = QueryStore.sort[0] + " ORDER BY isad157_jsandilands.users.user_id";
        //            QueryStore.sorted[0] = query;
        //
        //            connection.Open();
        //            MySqlCommand usercmd = new MySqlCommand(query, connection);
        //            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
        //            DataTable userDataTable = new DataTable();
        //            usersqlDA.Fill(userDataTable);
        //            usersDataGrid.DataSource = userDataTable;
        //            connection.Close();
        //        }
        //        else
        //        {
        //            string query = QueryStore.sort[0];
        //
        //            connection.Open();
        //            MySqlCommand usercmd = new MySqlCommand(query, connection);
        //            MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
        //            DataTable userDataTable = new DataTable();
        //            usersqlDA.Fill(userDataTable);
        //            usersDataGrid.DataSource = userDataTable;
        //            connection.Close();
        //        }
        //    }
        }

        //User Data Sort Code
        private void userSortBtn_Click(object sender, EventArgs e)
        {
            //Ready Strings for Sort
            string query = QueryStore.sort[0] + " ORDER BY ";
            bool oneORmore = false;
            bool errorRecieved = false;

            //establish connection
            string connectionString = Connect();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //check ticks
                if (UDIDSortCheckBox.Checked)
                {
                    oneORmore = true;
                    query = query + "isad157_jsandilands.users.user_id";

                    //check ascending or descending
                    if (UDsortComboBox1.Text == "Ascending")
                    {
                        query = query + " ASC";
                    }
                    else if (UDsortComboBox1.Text == "Descending")
                    {
                        query = query + " DESC";
                    }
                    else
                    {
                        errorRecieved = true;
                        oneORmore = false;
                    }
                }
                if(UDFNSortCheckBox.Checked)
                {
                    if(oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.users.first_name";

                        //check ascending or descending
                        if (UDsortComboBox2.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox2.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.users.first_name";

                        //check ascending or descending
                        if (UDsortComboBox2.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox2.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }
                if (UDLNSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.users.last_name";

                        //check ascending or descending
                        if (UDsortComboBox3.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox3.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.users.last_name";

                        //check ascending or descending
                        if (UDsortComboBox3.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox3.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }
                if (UDGdSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.users.gender";

                        //check ascending or descending
                        if (UDsortComboBox4.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox4.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.users.gender";

                        //check ascending or descending
                        if (UDsortComboBox4.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox4.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }
                if (UDRSSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.users.relationship";

                        //check ascending or descending
                        if (UDsortComboBox7.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox7.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.users.relationship";

                        //check ascending or descending
                        if (UDsortComboBox7.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox7.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }
                if (UDHTSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.users.hometown";

                        //check ascending or descending
                        if (UDsortComboBox5.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox5.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.users.hometown";

                        //check ascending or descending
                        if (UDsortComboBox5.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox5.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }
                if (UDCRSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.users.residence";

                        //check ascending or descending
                        if (UDsortComboBox6.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox6.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        query = query + "isad157_jsandilands.users.residence";

                        //check ascending or descending
                        if (UDsortComboBox6.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (UDsortComboBox6.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }

                if(errorRecieved == true)
                {
                    //allow user knowledge of the error.
                    MessageBox.Show("Please select one of the two options from the drop down!", "Invalid Order.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //cancel sort
                    query = QueryStore.sort[0];
                }

                if (!UDIDSortCheckBox.Checked && !UDFNSortCheckBox.Checked && !UDLNSortCheckBox.Checked && !UDGdSortCheckBox.Checked && !UDRSSortCheckBox.Checked && !UDHTSortCheckBox.Checked && !UDCRSortCheckBox.Checked == true)
                {
                    //cancel sort
                    query = QueryStore.sort[0];
                }

                //connect
                connection.Open();
                MySqlCommand usercmd = new MySqlCommand(query, connection);
                MySqlDataAdapter usersqlDA = new MySqlDataAdapter(usercmd);
                DataTable userDataTable = new DataTable();
                usersqlDA.Fill(userDataTable);
                usersDataGrid.DataSource = userDataTable;
                connection.Close();
            }
        }

        //Uni Data Sort Code
        private void uniSortBtn_Click(object sender, EventArgs e)
        {
            //Ready Strings for Sort
            string query = QueryStore.sort[1] + " ORDER BY ";
            bool oneORmore = false;
            bool errorRecieved = false;

            //establish connection
            string connectionString = Connect();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //Check Ticks
                if(EDIDSortCheckBox.Checked)
                {
                    oneORmore = true;
                    query = query + "isad157_jsandilands.users_education.user_id";

                    //check ascending or descending
                    if (EDsortComboBox1.Text == "Asc")
                    {
                        query = query + " ASC";
                    }
                    else if (EDsortComboBox1.Text == "Desc")
                    {
                        query = query + " DESC";
                    }
                    else
                    {
                        errorRecieved = true;
                        oneORmore = false;
                    }
                }
                if(EDUNSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.users_education.education";

                        //check ascending or descending
                        if (EDsortComboBox2.Text == "Asc")
                        {
                            query = query + " ASC";
                        }
                        else if (EDsortComboBox2.Text == "Desc")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.users_education.education";

                        //check ascending or descending
                        if (EDsortComboBox2.Text == "Asc")
                        {
                            query = query + " ASC";
                        }
                        else if (EDsortComboBox2.Text == "Desc")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }
                if (EDADSortCheckBox.Checked)
                {
                    {
                        if (oneORmore == true)
                        {
                            query = query + ", isad157_jsandilands.users_education.timeofuni";

                            //check ascending or descending
                            if (EDsortComboBox3.Text == "Asc")
                            {
                                query = query + " ASC";
                            }
                            else if (EDsortComboBox3.Text == "Desc")
                            {
                                query = query + " DESC";
                            }
                            else
                            {
                                errorRecieved = true;
                            }
                        }
                        else
                        {
                            oneORmore = true;
                            query = query + "isad157_jsandilands.users_education.timeofuni";

                            //check ascending or descending
                            if (EDsortComboBox3.Text == "Asc")
                            {
                                query = query + " ASC";
                            }
                            else if (EDsortComboBox3.Text == "Desc")
                            {
                                query = query + " DESC";
                            }
                            else
                            {
                                errorRecieved = true;
                                oneORmore = false;
                            }
                        }
                    }
                }

                if (errorRecieved == true)
                {
                    //allow user knowledge of the error.
                    MessageBox.Show("Please select one of the two options from the drop down!", "Invalid Order.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //cancel sort
                    query = QueryStore.sort[1];
                }

                if (!EDIDSortCheckBox.Checked && !EDUNSortCheckBox.Checked && !EDADSortCheckBox.Checked == true)
                {
                    //cancel sort
                    query = QueryStore.sort[1];
                }

                //connect
                connection.Open();
                MySqlCommand unicmd = new MySqlCommand(query, connection);
                MySqlDataAdapter unisqlDA = new MySqlDataAdapter(unicmd);
                DataTable uniDataTable = new DataTable();
                unisqlDA.Fill(uniDataTable);
                universitiesDataGrid.DataSource = uniDataTable;
                connection.Close();
            }
        }

        //Work Data Sort Code
        private void workSortBtn_Click(object sender, EventArgs e)
        {
            //Ready Strings for Sort
            string query = QueryStore.sort[2] + " ORDER BY ";
            bool oneORmore = false;
            bool errorRecieved = false;

            //establish connection
            string connectionString = Connect();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //Check Ticks
                if (WDIDSortCheckBox.Checked)
                {
                    oneORmore = true;
                    query = query + "isad157_jsandilands.users_workplaces.user_id";

                    //check ascending or descending
                    if (WDsortComboBox1.Text == "Asc")
                    {
                        query = query + " ASC";
                    }
                    else if (WDsortComboBox1.Text == "Desc")
                    {
                        query = query + " DESC";
                    }
                    else
                    {
                        errorRecieved = true;
                        oneORmore = false;
                    }
                }
                if (WDWPSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.users_workplaces.workplace";

                        //check ascending or descending
                        if (WDsortComboBox2.Text == "Asc")
                        {
                            query = query + " ASC";
                        }
                        else if (WDsortComboBox2.Text == "Desc")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.users_workplaces.workplace";

                        //check ascending or descending
                        if (WDsortComboBox2.Text == "Asc")
                        {
                            query = query + " ASC";
                        }
                        else if (WDsortComboBox2.Text == "Desc")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }
                if (WDADSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.users_workplaces.timeofwork";

                        //check ascending or descending
                        if (WDsortComboBox3.Text == "Asc")
                        {
                            query = query + " ASC";
                        }
                        else if (WDsortComboBox3.Text == "Desc")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.users_workplaces.timeofwork";

                        //check ascending or descending
                        if (WDsortComboBox3.Text == "Asc")
                        {
                            query = query + " ASC";
                        }
                        else if (WDsortComboBox3.Text == "Desc")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }

                if (errorRecieved == true)
                {
                    //allow user knowledge of the error.
                    MessageBox.Show("Please select one of the two options from the drop down!", "Invalid Order.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //cancel sort
                    query = QueryStore.sort[2];
                }

                if (!WDIDSortCheckBox.Checked && !WDWPSortCheckBox.Checked && !WDADSortCheckBox.Checked == true)
                {
                    //cancel sort
                    query = QueryStore.sort[2];
                }

                //connect
                connection.Open();
                MySqlCommand workcmd = new MySqlCommand(query, connection);
                MySqlDataAdapter worksqlDA = new MySqlDataAdapter(workcmd);
                DataTable workDataTable = new DataTable();
                worksqlDA.Fill(workDataTable);
                workDataGrid.DataSource = workDataTable;
                connection.Close();
            }
        }

        //Friend Data Sort Code
        private void friendSortBtn_Click(object sender, EventArgs e)
        {
            //Ready Strings for Sort
            string query = QueryStore.sort[3] + " ORDER BY ";
            bool oneORmore = false;
            bool errorRecieved = false;

            //establish connection
            string connectionString = Connect();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //Check Ticks
                if (FDIDSortCheckBox.Checked)
                {
                    oneORmore = true;
                    query = query + "isad157_jsandilands.friendships.user_id";

                    //check ascending or descending
                    if (FDsortComboBox1.Text == "Asc")
                    {
                        query = query + " ASC";
                    }
                    else if (FDsortComboBox1.Text == "Desc")
                    {
                        query = query + " DESC";
                    }
                    else
                    {
                        errorRecieved = true;
                        oneORmore = false;
                    }
                }
                if (FDFDSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.friendships.friend_id";

                        //check ascending or descending
                        if (FDsortComboBox2.Text == "Asc")
                        {
                            query = query + " ASC";
                        }
                        else if (WDsortComboBox2.Text == "Desc")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.friendships.friend_id";

                        //check ascending or descending
                        if (FDsortComboBox2.Text == "Asc")
                        {
                            query = query + " ASC";
                        }
                        else if (FDsortComboBox2.Text == "Desc")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }

                if (errorRecieved == true)
                {
                    //allow user knowledge of the error.
                    MessageBox.Show("Please select one of the two options from the drop down!", "Invalid Order.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //cancel sort
                    query = QueryStore.sort[3];
                }

                if (!FDIDSortCheckBox.Checked && !FDFDSortCheckBox.Checked == true)
                {
                    //cancel sort
                    query = QueryStore.sort[3];
                }

                //connect
                connection.Open();
                MySqlCommand friendcmd = new MySqlCommand(query, connection);
                MySqlDataAdapter friendsqlDA = new MySqlDataAdapter(friendcmd);
                DataTable friendDataTable = new DataTable();
                friendsqlDA.Fill(friendDataTable);
                friendsDataGrid.DataSource = friendDataTable;
                connection.Close();
            }
        }

        //Message Data Sort Code
        private void messageSortBtn_Click(object sender, EventArgs e)
        {
            //Ready Strings for Sort
            string query = QueryStore.sort[4] + " ORDER BY ";
            bool oneORmore = false;
            bool errorRecieved = false;

            //establish connection
            string connectionString = Connect();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //check ticks
                if (MDSDSortCheckBox.Checked)
                {
                    oneORmore = true;
                    query = query + "isad157_jsandilands.messages.user_id_sender";

                    //check ascending or descending
                    if (MDsortComboBox1.Text == "Ascending")
                    {
                        query = query + " ASC";
                    }
                    else if (MDsortComboBox1.Text == "Descending")
                    {
                        query = query + " DESC";
                    }
                    else
                    {
                        errorRecieved = true;
                        oneORmore = false;
                    }
                }
                if (MDRDSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.messages.user_id_reciever";

                        //check ascending or descending
                        if (MDsortComboBox2.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (MDsortComboBox2.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.messages.user_id_reciever";

                        //check ascending or descending
                        if (MDsortComboBox2.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (MDsortComboBox2.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }
                if (MDDTSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.messages.date_time";

                        //check ascending or descending
                        if (MDsortComboBox3.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (MDsortComboBox3.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        oneORmore = true;
                        query = query + "isad157_jsandilands.messages.date_time";

                        //check ascending or descending
                        if (MDsortComboBox3.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (MDsortComboBox3.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }
                if (MDMCSortCheckBox.Checked)
                {
                    if (oneORmore == true)
                    {
                        query = query + ", isad157_jsandilands.messages.msg_text";

                        //check ascending or descending
                        if (MDsortComboBox4.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (MDsortComboBox4.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                        }
                    }
                    else
                    {
                        query = query + "isad157_jsandilands.messages.msg_text";

                        //check ascending or descending
                        if (MDsortComboBox4.Text == "Ascending")
                        {
                            query = query + " ASC";
                        }
                        else if (MDsortComboBox4.Text == "Descending")
                        {
                            query = query + " DESC";
                        }
                        else
                        {
                            errorRecieved = true;
                            oneORmore = false;
                        }
                    }
                }

                if (errorRecieved == true)
                {
                    //allow user knowledge of the error.
                    MessageBox.Show("Please select one of the two options from the drop down!", "Invalid Order.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //cancel sort
                    query = QueryStore.sort[4];
                }

                if (!MDSDSortCheckBox.Checked && !MDRDSortCheckBox.Checked && !MDDTSortCheckBox.Checked && !MDMCSortCheckBox.Checked == true)
                {
                    //cancel sort
                    query = QueryStore.sort[4];
                }

                //connect
                connection.Open();
                MySqlCommand messagescmd = new MySqlCommand(query, connection);
                MySqlDataAdapter messagessqlDA = new MySqlDataAdapter(messagescmd);
                DataTable messagesDataTable = new DataTable();
                messagessqlDA.Fill(messagesDataTable);
                messageDataGrid.DataSource = messagesDataTable;
                connection.Close();
            }
        }

        private void FDsortComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FDsortComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
