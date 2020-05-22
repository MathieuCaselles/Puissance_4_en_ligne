using Matchmaking.jeu;
using System;
using System.Net.Sockets;
using System.Text;

namespace Matchmaking
{
    class PartieEnCours
    {
        private Client firstClient;
        private Client secondClient;
        private Plateau plateau;
        private Boolean jeuEnCours;

        public PartieEnCours(Client firstClient, Client secondClient)
        {
            this.firstClient = firstClient;
            this.secondClient = secondClient;

            this.plateau = new Plateau();
            this.jeuEnCours = true;

            while (jeuEnCours)
            {
                if (this.plateau.Gagne() == 0)
                {
                    Client clientJoue = this.getPlayingClient(); 
                    Client clientAttente = this.getOpponentClient(clientJoue);

                    this.Send(clientJoue.getWorkSocket(), "\nChoisissez une colonne entre 0 et 6\n");
                    this.Send(clientAttente.getWorkSocket(), $"\nAttendez la fin du tour de {clientJoue}\n");

                    // récupérer colonne joué.
                    clientJoue.appendStringData(clientJoue.getWorkSocket().Receive(clientJoue.getBuffer()));

                    int reponse = int.Parse(clientJoue.getStringData());

                    this.Send(clientAttente.getWorkSocket(), $"\nVous avez joué dans la colonne {reponse}");
                    this.Send(clientAttente.getWorkSocket(), $"\nle joueur {clientJoue} à joué dans la colonne {reponse}");

                    //							System.out.print("Joueur "+ serveur.getP().getQuiJoue() +", quelle colonne ? ");
                    this.plateau.Joue(reponse);
                    this.Send(clientJoue.getWorkSocket(), this.plateau.ToString());
                    this.Send(clientAttente.getWorkSocket(), this.plateau.ToString());

                    this.plateau.changeJoueur();
                    clientJoue.clearStringData();
                    clientAttente.clearStringData();

                }
                else
                {
                    this.SendResultat();
                    jeuEnCours = false;
                }
            }
            Serveur.nbrPartie--;
            Console.WriteLine($"Nombre de parties en cours : {Serveur.nbrPartie}");
            this.firstClient.getWorkSocket().Shutdown(SocketShutdown.Both);
            this.firstClient.getWorkSocket().Close();
            this.secondClient.getWorkSocket().Shutdown(SocketShutdown.Both);
            this.secondClient.getWorkSocket().Close();
        }

        public Client getFisrtClient()
        {
            return this.firstClient;
        }

        public Client getSecondClient()
        {
            return this.secondClient;
        }

        public Socket getSocketFisrtClient()
        {
            return this.firstClient.getWorkSocket();
        }

        public Socket getSocketSecondClient()
        {
            return this.secondClient.getWorkSocket();
        }

        public Client getPlayingClient()
        {
            return plateau.getQuiJoue() == 1 ? this.firstClient : this.secondClient;
        }

        public Client getOpponentClient(Client client)
        {
            return client == this.getFisrtClient() ? this.getSecondClient() : this.getFisrtClient();
        }


        private void Send(Socket socketClient, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.  
            socketClient.Send(byteData);
        }

        private async void SendResultat()
        {
            String resultat = this.plateau.Gagne() == 1 ? $"\n{this.firstClient} a gagné\n" : $"\n{this.secondClient} a gagné\n";

            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.UTF8.GetBytes(resultat);

            // Begin sending the data to the remote device.  
            this.firstClient.getWorkSocket().Send(byteData);
            this.secondClient.getWorkSocket().Send(byteData);

        }

    }
}
