using Matchmaking.jeu;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
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
        private string couleur = "ROUGE";

        public PartieEnCours(Client firstClient, Client secondClient)
        {
            this.firstClient = firstClient;
            this.secondClient = secondClient;

            this.plateau = new Plateau();
            this.jeuEnCours = true;
            
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");


            while (this.jeuEnCours)
            {
                if (this.plateau.Gagne() == 0)
                {
                    // savoir qui joue et qui attend
                    Client clientJoue = this.getPlayingClient(); 
                    Client clientAttente = this.getOpponentClient(clientJoue);

                    //envoie des données en fonction de si le joueur attend ou joue
                    JObject jsonObjectClientJoue = new JObject();
                    jsonObjectClientJoue.Add("jeSuis", clientJoue.ToString());
                    jsonObjectClientJoue.Add("quiJoue", clientJoue.ToString());
                    jsonObjectClientJoue.Add("plateau", this.plateau.ToString());
                    jsonObjectClientJoue.Add("message",  "C'est à vous de jouer !");
                    jsonObjectClientJoue.Add("couleur", this.getCouleur());

                    this.toggleCouleur();

                    JObject jsonObjectClientAttente = new JObject();
                    jsonObjectClientAttente.Add("jeSuis", clientAttente.ToString());
                    jsonObjectClientAttente.Add("quiJoue", clientJoue.ToString());
                    jsonObjectClientAttente.Add("plateau", this.plateau.ToString());
                    jsonObjectClientAttente.Add("message", $"En attente de {clientJoue}...");
                    jsonObjectClientAttente.Add("couleur", this.getCouleur());


                    this.Send(clientJoue.getWorkSocket(), jsonObjectClientJoue.ToString(Formatting.None) + "\n");
                    this.Send(clientAttente.getWorkSocket(), jsonObjectClientAttente.ToString(Formatting.None) + "\n");

                    // récupérer colonne joué.
                    try
                    {
                        clientJoue.appendStringData(clientJoue.getWorkSocket().Receive(clientJoue.getBuffer()));
                        int reponse = int.Parse(clientJoue.getStringData());
                        this.plateau.Joue(reponse);

                    }
                    catch (Exception)
                    {
                        this.SendError();
                        this.jeuEnCours = false;
                    }
                    
                    this.plateau.changeJoueur();
                    clientJoue.clearStringData();
                    clientAttente.clearStringData();

                }
                else
                {
                    this.toggleCouleur();
                    this.SendResultat();
                    this.jeuEnCours = false;
                }
            }
            Serveur.nbrPartie--;
            Console.WriteLine($"Nombre de parties en cours : {Serveur.nbrPartie}");

            //on coupe la connexion
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

        public Client getPlayingClient()
        {
            return plateau.getQuiJoue() == 1 ? this.firstClient : this.secondClient;
        }

        public Client getOpponentClient(Client client)
        {
            return client == this.getFisrtClient() ? this.getSecondClient() : this.getFisrtClient();
        }

        public string getCouleur()
        {
            return this.couleur;
        }

        public void toggleCouleur()
        {
            this.couleur = this.couleur == "ROUGE" ? "JAUNE" : "ROUGE";
        }


        private void Send(Socket socketClient, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.  
            socketClient.Send(byteData);
        }

        private void SendResultat()
        {
            String resultat = (this.plateau.Gagne() == 1 ? $"{this.firstClient.ToString().Replace("\n", String.Empty)} a gagné !" : $"{this.secondClient.ToString().Replace("\n", String.Empty)} a gagné !") + $" ({this.getCouleur()})\n";


            JObject obj = new JObject();
            obj.Add("message", resultat);
            obj.Add("plateau", this.plateau.ToString());

            // Convert the string data to byte data using UTF8 encoding.  

            byte[] byteData = Encoding.UTF8.GetBytes(obj.ToString(Formatting.None));
            //Sending the data to the remote device.  
            this.firstClient.getWorkSocket().Send(byteData);
            this.secondClient.getWorkSocket().Send(byteData);

        }

        private void SendError()
        {
            JObject obj = new JObject();
            obj.Add("message", "Un joueur s'est déconnecté. Veuillez relancer une partie.");
            obj.Add("plateau", this.plateau.ToString());
            byte[] byteData = Encoding.UTF8.GetBytes(obj.ToString(Formatting.None));
            // sending the data to the remote device.  
            try
            {
                this.firstClient.getWorkSocket().Send(byteData);
            }
            catch (Exception)
            {

                Console.WriteLine("j1 déconnecté");
            }

            try
            {
                this.secondClient.getWorkSocket().Send(byteData);
            }
            catch (Exception)
            {

                Console.WriteLine("j2 déconnecté");
            }

        }

    }
}
