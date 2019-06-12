using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace ServerThreadConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
            listenerSocket.Bind(ipEnd);

            int clientNo = 1;

            while (true)
            {
                listenerSocket.Listen(0);
                Socket clientSocket = listenerSocket.Accept();

                //Thread
                Thread clientThread;
                clientThread = new Thread(() => ClientConnection(clientSocket, clientNo));
                clientThread.Start();
                clientNo++;
            }           
        }

        private static void ClientConnection(Socket clientSocket, int clNr)
        {
            byte[] Buffer = new byte[clientSocket.SendBufferSize];

            int readByte;
            do
            {
                // Receive
                readByte = clientSocket.Receive(Buffer);

                // Do stuff
            
                byte[] rData = new byte[readByte];
                Array.Copy(Buffer, rData, readByte);
                // Console.WriteLine("We got: (" + clNr.ToString() + ")" + System.Text.Encoding.UTF8.GetString(rData));


                DTOs.Car c = ByteToCar(rData);
                Console.WriteLine(c.Make + " " + c.Year.ToString());

                // Piggyback data
                clientSocket.Send(new byte[4] { 65, 66, 67, 78 });

            } while (readByte > 0);

            Console.WriteLine("Client disconnected");
            Console.ReadKey();
        }

        private static DTOs.Car ByteToCar(byte[] inData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ms.Write(inData, 0, inData.Length);
            ms.Seek(0, SeekOrigin.Begin);
            object o = (object)bf.Deserialize(ms);
            return (DTOs.Car)o;
        }
    }
}
