using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleServerSocket
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Server");
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse("192.168.1.103"), 8888);
            listenerSocket.Bind(iPEnd);
            int clientID = 1;
            while (true)
            {

                listenerSocket.Listen(0);
                Socket clientSocket = listenerSocket.Accept();

                Thread clientThread = new Thread(() => ClientConnection(clientSocket,clientID));
                clientThread.Start();
                clientID++;
            }

        }

        private static void ClientConnection(Socket clientSocket,int clientID)
        {
            byte[] Buffer = new byte[clientSocket.SendBufferSize];
            int readByte=1;
            do
            {
                try
                {
                    readByte = clientSocket.Receive(Buffer);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Connection aborted. " + e.ToString());
                }

                Console.WriteLine("Number of characters in message: ");
                Console.WriteLine(readByte.ToString());
                byte[] readData = new byte[readByte];
                Array.Copy(Buffer, readData, readByte);
                Console.WriteLine("From client numer " + clientID + "We got: " + System.Text.Encoding.UTF8.GetString(readData));

                try
                {
                    clientSocket.Send(new byte[2] { 79, 75 });// O K
                }
                catch (Exception e)
                {
                    Console.WriteLine("Connection aborted. " + e.ToString());
                }


            } while (readByte > 0);


            Console.WriteLine("Client disonnected");
            Console.ReadKey();
        }
    }
}
