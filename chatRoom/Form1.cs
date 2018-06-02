using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace chatRoom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JObject json = new JObject();
            User user = new User() { Account = txtAccount.Text, Name = txtName.Text, Password = txtPassword.Text };
            string userJson = JsonConvert.SerializeObject(user);
            send(userJson);
        }

        private void send(string text)
        {
            IPAddress iP = IPAddress.Parse("	101.132.163.49");
            IPEndPoint point = new IPEndPoint(iP, 3693);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(point);
            socket.Send(Encoding.UTF8.GetBytes(text));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    public class User
    {
        private string account;
        private string name;
        private string password;
        public string Password { get => password; set => password = value; }
        public string Name { get => name; set => name = value; }
        public string Account { get => account; set => account = value; }

    }

}