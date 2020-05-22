using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaking.jeu
{
    class Plateau
    {

		public readonly static int taillePlateau = 7;
		public Case[][] tab = new Case[taillePlateau][];
		public int quiJoue = 1;


		public Plateau()
		{
			//initialiser les colonne
			for (int ligne = 0; ligne < taillePlateau; ligne++)
			{
				this.tab[ligne] = new Case[taillePlateau];
			}
			//initialiser les Case
			for (int ligne = 0; ligne < taillePlateau; ligne++)
			{
				for (int colonne = 0; colonne < taillePlateau; colonne++)
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
			for (int ligne = 0; ligne < taillePlateau; ligne++)
			{
				for (int colonne = 0; colonne < taillePlateau - 4; colonne++)
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


			for (int ligne = 0; ligne < taillePlateau - 4; ligne++)
			{
				for (int colonne = 0; colonne < taillePlateau; colonne++)
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

			for (int ligne = 0; ligne < taillePlateau - 4; ligne++)
			{
				for (int colonne = 0; colonne < taillePlateau - 4; colonne++)
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

			for (int ligne = 0; ligne < taillePlateau - 4; ligne++)
			{
				for (int colonne = taillePlateau - 4; colonne < taillePlateau; colonne++)
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
			for (int ligne = taillePlateau - 1; ligne >= 0; ligne--)
			{
				String lignePlateau = "";
				for (int colonne = 0; colonne < taillePlateau; colonne++)
				{
					lignePlateau += this.tab[ligne][colonne];
				}
				res += lignePlateau + "\n";
			}
			res += "-------\n0123456";
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
