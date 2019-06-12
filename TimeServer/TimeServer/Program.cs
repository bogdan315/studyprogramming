using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.Security;
using System.Threading;

namespace TimeServer
{
    class Program
    {
        private static byte[] _buffer = new byte[1024];
        private static List<Socket> _clientSockets = new List<Socket>();
        private static Socket _serverSocket= new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

        static void Main(string[] args)
        {
            //Console.WriteLine("Setting up Server...");
            //Console.ReadLine();
            Console.Title = "Server";
            SetupServer();
            Console.ReadLine();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up Server...");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any,100));
            _serverSocket.Listen(1);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(socket);
            Console.WriteLine("Client Connected");
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuf = new byte[received];
            Array.Copy(_buffer, dataBuf, received);
            string text = Encoding.ASCII.GetString(dataBuf);
            Console.WriteLine("Text received: "+text);

            string response = string.Empty;

            //Здесь мы приняли текст от клиента и проверяем что клиент запрашивает у нас время на сервере (мы являемся сервером)
            //поэтому отправляем клиенту время.

           /* if (text.ToLower() != "get time")
            {
                response = "Invalid Request";
            }
            else
            {
                response = DateTime.Now.ToLongTimeString();
            }*/

            switch (text.ToLower()) {
                case "get time": { 
                          response = text.ToLower().ToString()+":"+DateTime.Now.ToLongTimeString(); 
                } break;
                case "brat315": { response = text.ToLower().ToString()+":"+"Hello brat315"; } break;
                default: { response = text.ToLower().ToString()+":"+"default"; } break;
            }

            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }


        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }
    }
}
