using System;
using System.Collections.Generic;
using System.Text;

namespace Matchmaking.jeu
{
    class Case
    {
		public TypeCase stateCase;

		public Case()
		{
			this.stateCase = TypeCase.VIDE;
		}

		public void setCase(TypeCase case1)
		{
			this.stateCase = case1;
		}


		public TypeCase getCase()
		{
			return this.stateCase;
		}

		public override String ToString()
		{
			if (this.stateCase == TypeCase.VIDE)
			{
				return " ";
			}
			else if (this.stateCase == TypeCase.J1)
			{
				return "1";
			}
			else
			{
				return "2";
			}
		}
	}
}
