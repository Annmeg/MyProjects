using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.Models
{
public	class row
	{
		int numOfBlack;     //Stores number of black pieces on a row.
		int numOfWhite;     //Stores number of white pieces on a row.
		char[] list;       //array to store pieces or blank spaces.

		public row()
		{
			list = new char[6];
			numOfBlack = 0;
			numOfWhite = 0;
		}
		public int getNumOfBlacks()
		{
			return numOfBlack;
		}

		public int getNumOfWhites()
		{
			return numOfWhite;
		}

		public void incrementWhite()
		{
			numOfWhite++;
		}

		public void incrementBlack()
		{
			numOfBlack++;
		}

		public void decrementWhite()
		{
			numOfWhite--;
		}

		public void decrementBlack()
		{
			numOfBlack--;
		}

		public void setRow(int x)
		{
			switch (x)
			{
				case 0:
					numOfBlack = 2;
					break;
				case 1:
				case 2:
				case 3:
				case 4:
				case 6:
				case 8:
				case 9:
				case 10:
					break;
				case 5:
					numOfWhite = 5;
					break;
				case 7:
					numOfWhite = 3;
					break;
				case 11:
					numOfBlack = 5;
					break;
				case 12:
					numOfWhite = 5;
					break;
				case 13:
				case 14:
				case 15:
				case 17:
				case 20:
				case 19:
				case 21:
				case 22:
					break;
				case 16:
					numOfBlack = 3;
					break;
				case 18:
					numOfBlack = 5;
					break;
				case 23:
					numOfWhite = 2;
					break;
				case 24:
					break;
				}
		}
	}
}
