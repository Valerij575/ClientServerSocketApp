using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientServerSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);

            if (args.Length > 0)
            {
                port = int.Parse(args[0]);
            }
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Bind(iPEndPoint);
                socket.Listen(10);
                Socket handler = socket.Accept();
                Console.WriteLine("Server was started. Waiting connections");

                while (true)
                {
                    
                    StringBuilder message = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[256];

                    do
                    {
                        bytes = handler.Receive(data);
                        message.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine($"{DateTime.Now.ToShortDateString()}: {message}");

                    // send ansver
                    string ansver = $"IP address {handler.LocalEndPoint}, Sum: {message}";
                    data = Encoding.Unicode.GetBytes(ansver);
                    handler.Send(data);
                   
                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
