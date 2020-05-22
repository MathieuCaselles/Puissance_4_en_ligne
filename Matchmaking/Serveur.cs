using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Matchmaking
{
    public class Serveur
    {

  

        public static void StartServer()
        {
            // Establish the local endpoint for the socket.  
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    Console.WriteLine("En attente de client 1...");
                    Socket firstClient = listener.Accept();
                    Client client1 = new Client(firstClient, "Client 1");

                    Console.WriteLine("En attente de client 2...");
                    Socket secondClient = listener.Accept();
                    Client client2 = new Client(secondClient, "Client 2");


                    new PartieEnCours(client1, client2);

                    Console.WriteLine("Partie lancée ! glhf !");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

       

    }
}
