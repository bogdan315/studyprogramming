using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.IO;

namespace ClientThreadConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is the client");

            Socket master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);

            master.Connect(ipEnd);

            string sendData = "";

            do 
            {
               // Console.Write("Data to send: ");
               // sendData = Console.ReadLine();
               // master.Send(System.Text.Encoding.UTF8.GetBytes(sendData));

                master.Send(CarToByte(new DTOs.Car(1998, "BMW 750i")));

                // Get piggyback data
                byte[] pbd = new byte[4];
                master.Receive(pbd);
                Console.WriteLine("Our piggyback data: " + System.Text.Encoding.UTF8.GetString(pbd));
            } while (sendData.Length > 0);

            master.Close();
        }

        static byte[] CarToByte(DTOs.Car car)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, car);
            return ms.ToArray();
        }
    }
}
