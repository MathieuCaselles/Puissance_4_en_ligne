using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Matchmaking
{
    class PartieEnCours
    {
        private Client firstClient;
        private Client secondClient;
        private int monTest = 0;

        public PartieEnCours(Client firstClient, Client secondClient)
        {
            this.firstClient = firstClient;
            this.secondClient = secondClient;

            firstClient.getWorkSocket().BeginReceive(firstClient.getBuffer(), 0, Client.BufferSize, 0,
                new AsyncCallback(ReadCallback), firstClient);

            secondClient.getWorkSocket().BeginReceive(secondClient.getBuffer(), 0, Client.BufferSize, 0,
                new AsyncCallback(ReadCallback), secondClient);
        }

        ~PartieEnCours()
        {
            Console.WriteLine("Objet détruit !");
        }

        public Client getFisrtClient()
        {
            return this.firstClient;
        }



        public Client getSecondClient()
        {
            return this.secondClient;
        }

        public Client getOpponentClient(Client client)
        {
            return client == this.getFisrtClient() ? this.getSecondClient() : this.getFisrtClient();
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the socketClient socket  
            // from the asynchronous state object.  
            Client client = (Client)ar.AsyncState;
            Socket socketClient = client.getWorkSocket();

            // Read data from the client socket.
            int bytesRead = socketClient.EndReceive(ar);

            

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                client.appendStringData(bytesRead);

                Console.WriteLine("client actuel : " + client);
 
                // Check for end-of-file tag. If it is not there, read
                // more data.  
                content = client.getStringData();
                if (content.IndexOf("stop") > -1)
                {
                    // All the data has been read from the
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : \n {1}",
                        content.Length, content);
                    // Echo the data back to the client.  

                    this.monTest++;
                    if(this.monTest < 2)
                    {
                        Console.Write("En Attente d'une réponse des 2 joueurs... \n");
                        while (this.monTest < 2)
                        {
                            Thread.Sleep(1000);
                        }
                    }

                    Client opponent = this.getOpponentClient(client);
                    Send(opponent.getWorkSocket(), content);
                }
                else
                {
                    // Not all data received. Get more.  
                    socketClient.BeginReceive(client.getBuffer(), 0, Client.BufferSize, 0,
                    new AsyncCallback(ReadCallback), client);
                }
            }
        }

        private void Send(Socket socketClient, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.  
            socketClient.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), socketClient);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket socketClient = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = socketClient.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                socketClient.Shutdown(SocketShutdown.Both);
                socketClient.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }




    }
}
