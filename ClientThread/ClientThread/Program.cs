using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace ClientThread
{
    class Program
    {
        const int PORT = 5006;
        const string ADDRESS = "127.0.0.1";

        static void Main(string[] args)
        {
            TcpClient client = null;
            try
            {
                while (true)
                {
                    client = new TcpClient(ADDRESS, PORT);
                    Console.Write("Введите сообщение: ");
                    string message = Console.ReadLine();
                    NetworkStream stream = client.GetStream();

                    // отправляем сообщение
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(message);
                    writer.Flush();

                    BinaryReader reader = new BinaryReader(stream);
                    message = reader.ReadString();
                    Console.WriteLine("Получен ответ: " + message);


                    reader.Close();
                    writer.Close();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (client != null)
                    client.Close();
            }
        }
    }
}
