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
            string strIp = "	172.19.205.120";
            IPAddress iP = IPAddress.Parse(strIp.Trim());
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
            Console.WriteLine(message);
            User user = JsonConvert.DeserializeObject<User>(message);
            Console.WriteLine(user.Account);
            if (user != null)
            {

                using (SqlConnection con = new SqlConnection(@"Data Source =iZunzhqtbslzqoZ\SQLEXPRESS; database=userList; user id=sa; pwd=Evicce123;"))
                {
                    string strCmd = "insert into user_list values('" + user.Account + "','" + user.Name + "','" + user.Password + "')";
                    using (SqlCommand cmd = new SqlCommand(strCmd, con))
                    {
                        try
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
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
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
