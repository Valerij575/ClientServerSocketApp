using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            string host = ConfigurationManager.AppSettings["Host"];

            try
            {
                bool isContinue = true;
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(iPEndPoint);

                while (isContinue)
                {
                    
                    Console.WriteLine("Enter message");
                    
                    string message = Console.ReadLine();
                    if (message.Equals("Exit"))
                    {
                        isContinue = false;
                        break;
                    }
                        

                        byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);

                    data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                    Console.WriteLine("ответ сервера: " + builder.ToString());
                }
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }
    }
}
