using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace signServer
{
    class Program
    {
        private static Socket serverSocket;
        static void Main(string[] args)
        {
            IPAddress iP = IPAddress.Parse("127.0.0.1");
            IPEndPoint point = new IPEndPoint(iP, 3692);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(point);
            serverSocket.Listen(10);
            Thread thread = new Thread(ListenClientConnect);
            thread.Start();
        }

        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Thread thread = new Thread(RecieveMessage);
                thread.Start(clientSocket);
            }
        }
        private static void RecieveMessage(object socket)
        {
            Socket clientSocket = (Socket)socket;
            byte[] bytesMessage = new byte[1000];
            int recieveNumber = clientSocket.Receive(bytesMessage);
            string message = Encoding.UTF8.GetString(bytesMessage, 0, recieveNumber);
            User user = JsonConvert.DeserializeObject(message) as User;
            if (user != null)
            {

                using (SqlConnection con = new SqlConnection(""))
                {
                    string strCmd = "insert into user_list values(" + user.Account + "," + user.Name + "," + user.Password + ")";
                    using (SqlCommand cmd = new SqlCommand(strCmd, con))
                    {
                        con.Open();
                        int r = cmd.ExecuteNonQuery();
                        if (r > 0)
                        {
                            Console.WriteLine("a user signed");
                        }
                        else
                        {
                            Console.WriteLine("sign failed");
                        }
                    }
                }
            }
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
