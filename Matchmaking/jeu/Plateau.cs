using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaking.jeu
{
    class Plateau
    {

		public readonly static int tailleLigne = 6;
		public readonly static int tailleColonne = 7;
		public Case[][] tab = new Case[tailleLigne][];
		public int quiJoue = 1;


		public Plateau()
		{
			//initialiser les colonne
			for (int ligne = 0; ligne < tailleLigne; ligne++)
			{
				this.tab[ligne] = new Case[tailleColonne];
			}
			//initialiser les Case
			for (int ligne = 0; ligne < tailleLigne; ligne++)
			{
				for (int colonne = 0; colonne < tailleColonne; colonne++)
				{
					this.tab[ligne][colonne] = new Case();
				}
			}
		}

		public void Joue(int colonne)
		{
			int ligne = 0;
			
			while (this.tab[ligne][colonne].getCase() != TypeCase.VIDE)
			{
				ligne++;
			}
			this.tab[ligne][colonne].setCase(quiJoue == 1 ? TypeCase.J1 : TypeCase.J2);

		}

		public int Gagne()
		{
			// recherche dans le plateau les successions identiques horizontales
			for (int ligne = 0; ligne < tailleLigne; ligne++)
			{
				for (int colonne = 0; colonne < tailleColonne - 4; colonne++)
				{
					if (this.tab[ligne][colonne].getCase() != TypeCase.VIDE &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne][colonne + 1].getCase() &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne][colonne + 2].getCase() &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne][colonne + 3].getCase())
					{
						return (int) this.tab[ligne][colonne].getCase();
					}
				}
			}


			for (int ligne = 0; ligne < tailleLigne - 3; ligne++)
			{
				for (int colonne = 0; colonne < tailleColonne; colonne++)
				{
					if (this.tab[ligne][colonne].getCase() != TypeCase.VIDE &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne + 1][colonne].getCase() &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne + 2][colonne].getCase() &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne + 3][colonne].getCase())
					{
						return (int) this.tab[ligne][colonne].getCase();
					}
				}
			}

			for (int ligne = 0; ligne < tailleLigne - 3; ligne++)
			{
				for (int colonne = 0; colonne < tailleColonne - 4; colonne++)
				{
					if (this.tab[ligne][colonne].getCase() != TypeCase.VIDE &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne + 1][colonne + 1].getCase() &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne + 2][colonne + 2].getCase() &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne + 3][colonne + 3].getCase())
					{
						return (int) this.tab[ligne][colonne].getCase();
					}
				}
			}

			for (int ligne = 0; ligne < tailleLigne - 3; ligne++)
			{
				for (int colonne = tailleColonne - 4; colonne < tailleColonne; colonne++)
				{
					if (this.tab[ligne][colonne].getCase() != TypeCase.VIDE &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne + 1][colonne - 1].getCase() &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne + 2][colonne - 2].getCase() &&
							this.tab[ligne][colonne].getCase() == this.tab[ligne + 3][colonne - 3].getCase())
					{
						return (int) this.tab[ligne][colonne].getCase();
					}
				}
			}
			return 0;
		}

		public override String ToString()
		{
			String res = "";
			for (int ligne = tailleLigne - 1; ligne >= 0; ligne--)
			{
				String lignePlateau = "";
				for (int colonne = 0; colonne < tailleColonne; colonne++)
				{
					lignePlateau += this.tab[ligne][colonne];
				}
				res += lignePlateau + "\n";
			}
			return res;
		}

		public void changeJoueur()
		{
			quiJoue = (quiJoue == 1) ? 2 : 1;
		}

		public Case[][] getTab()
		{
			return this.tab;
		}

		public int getQuiJoue()
		{
			return this.quiJoue;
		}

		public int getQuiNeJouePas()
		{
			return this.quiJoue == 1 ? 2 : 1;
		}

		public static void main(String[] aaa)
		{

		}
	}
}
