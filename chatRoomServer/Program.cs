using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace chatRoomServer
{
    class Program
    {
        static Socket serverSocket;
        static List<Socket> clients = new List<Socket>();
        static void Main(string[] args)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress iP = IPAddress.Parse("127.0.0.1");
            IPEndPoint point = new IPEndPoint(iP, 3693);
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
                clients.Add(clientSocket);
                Thread thread = new Thread(RecieveMessage);
                thread.Start(clientSocket);
            }
        }

        private static void RecieveMessage(object socket)
        {
            Socket clientSocket = (Socket)socket;
            byte[] bytesMessage = new byte[1024];
            while (true)
            {
                try
                {
                    int recieveNumber = clientSocket.Receive(bytesMessage);
                    string message = Encoding.UTF8.GetString(bytesMessage, 0, recieveNumber);
                    foreach (var client in clients)
                    {
                        client.Send(Encoding.UTF8.GetBytes(message));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    clients.Remove(clientSocket);
                }
            }
        }
    }
}
