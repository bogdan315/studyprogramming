using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ServerThread
{
    class Program
    {
        const int PORT = 5006; // порт для прослушивания подключений
        static TcpListener listener;

        static void Main(string[] args)
        {
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), PORT);
                listener.Start();
                Console.WriteLine("Ожидание подключений...");

                //while (true)
                //{
                    TcpClient client = listener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    BinaryReader reader = new BinaryReader(stream);
                    // считываем строку из потока
                    string message = reader.ReadString();
                    Console.WriteLine("Получено: " + message);

                    // отправляем ответ
                    BinaryWriter writer = new BinaryWriter(stream);
                    message = message.ToUpper();
                    Console.WriteLine("Отправлено: " + message);
                    writer.Write(message);
                    writer.Flush();

                    writer.Close();
                    reader.Close();
                    stream.Close();
                    client.Close();
              //  }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }
    }
}
