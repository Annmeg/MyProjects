using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading;

namespace GameEngine.Models
{
	public class board
	{
		row[] rows;       //Array of all the rows on the board
		int blackPending;   //black pending pieces
		int whitePending;   //white pending pieces
		int blackTotal;     //Keeps track of the amount of blacks that have made it off the board.
		int whiteTotal;
		bool blackHome;
		bool whiteHome;
		public board()
		{
			rows = new row[24];
			blackPending = 0;
			whitePending = 0;
			blackTotal = 0;
			whiteTotal = 0;
			blackHome = false;
			whiteHome = false;
			for (int i = 0; i < 24; i++)
			{
				rows[i] = new row();
				rows[i].setRow(i);
			}
		}
		public int getBlackTotal()
		{
			return blackTotal;
		}
		public int getWhiteTotal()
		{
			return whiteTotal;
		}
		public void reset()
        {
			blackPending = 0;
			whitePending = 0;
			blackTotal = 0;
			whiteTotal = 0;
			blackHome = false;
			whiteHome = false;
			for (int i = 0; i < 24; i++)
			{
				rows[i] = new row();
				rows[i].setRow(i);
			}
		}

		public bool isMovePossible(Color cur, int n)
		{
			for (int i = 0; i < 24; i++)
			{
				if (cur == Color.Aqua)
				{
					if (blackHome)
						return true;
					if (rows[i].getNumOfBlacks() > 0)
					{
						int moved = i + n;
						if (moved < 24 && rows[moved].getNumOfWhites() <= 1)
						{
							return true;
						}
					}
				}
				else// current piece is white
				{

					if (whiteHome)
						return true;
					if (rows[i].getNumOfWhites() > 0)
					{
						int moved = i - n;
						if (moved >= 0 && rows[moved].getNumOfBlacks() <= 1)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool checkStuckWhite(int x, int y)
		{
			if (rows[24 - x].getNumOfBlacks() > 1 && rows[24 - y].getNumOfBlacks() > 1) //Based on the dice rolls, (x and y), checks the possible moves from the bar to check if a move is possible.
			{
				return true;
			}
			else
				return false;
		}

		public bool checkStuckBlack(int x, int y)
		{
			if (rows[-1 + x].getNumOfWhites() > 1 && rows[-1 + y].getNumOfWhites() > 1)
			{
				return true;
			}
			else
				return false;
		}
		public bool checkPendingWhite()
		{
			if (whitePending > 0)
			{
				// must move piece from the bar if possible
				return true;
			}
			else
				return false;
		}

		public bool checkPendingBlack()
		{
			if (blackPending > 0)
			{
				//must move piece from the bar if possible
				return true;
			}
			else
				return false;
		}
		public int movePieceWhitePending(int choice)
		{
			int moved = choice - 1;
			if (rows[moved].getNumOfBlacks() > 1)
			{
				//Cannot move on row containing two or more opponents pieces;
				return 1;
			}
			else if (rows[moved].getNumOfBlacks() == 1)
			{
				whitePending--;
				rows[moved].decrementBlack();
				rows[moved].incrementWhite();
				blackPending++;
				return 0;
			}
			else
			{
				whitePending--;
				rows[moved].incrementWhite();
				return 0;
			}
		}
		public	int movePieceWhite(int x, int y)
		{
			int index = x;
			int moved = y;
			if (rows[index].getNumOfWhites() == 0)
			{
				//do not have any pieces to move in that row
				return 1;
			}
			if (moved <= -1)
			{
				int homeSum = 0;
				int i;
				for (i = 0; i < 6; i++)
				{
					homeSum = homeSum + rows[i].getNumOfWhites();
				}
				homeSum = homeSum + whiteTotal;
				if (homeSum == 15)
				{
					whiteHome = true;
					whiteTotal++;
					if (rows[index].getNumOfWhites() == 0)
					{
						while (--index >= 0)
						{
							if (rows[index].getNumOfWhites() > 0)
								rows[index].decrementWhite();
						}
					}
					else
						rows[index].decrementWhite();
					return 0;
				}
				else
				{
					// All pieces need to be on the home board to start bearing off.
					return 1;
				}
			}
			else if (rows[moved].getNumOfBlacks() > 1)
			{
				//Cannot move on row containing two or more opponents pieces
				return 1;
			}
			else if (rows[moved].getNumOfBlacks() == 1)
			{
				rows[index].decrementWhite();
				rows[moved].decrementBlack();
				rows[moved].incrementWhite();
				blackPending++;
				return 0;
			}
			else
			{
				rows[index].decrementWhite();
				rows[moved].incrementWhite();
				return 0;
			}
		}
		public int movePieceBlackPending(int choice)
		{
			int moved = choice - 1;
			if (rows[moved].getNumOfWhites() > 1)
			{
				//Cannot move on row containing two or more opponents pieces
				return 1;
			}
			else if (rows[moved].getNumOfWhites() == 1)
			{
				blackPending--;

				rows[moved].decrementWhite();
				rows[moved].incrementBlack();
				whitePending++;
				return 0;
			}
			else
			{
				blackPending--;
				rows[moved].incrementBlack();
				return 0;
			}
		}
		public int movePieceBlack(int x, int y)
		{
			int index = x;
			int moved = y;
			if (rows[index].getNumOfBlacks() == 0)
			{
				// do not have any pieces to move in that row;
				return 1;
			}

			if (moved >= 24)
			{
				int homeSum = 0;
				int i;
				for (i = 18; i < 24; i++)
				{
					homeSum = homeSum + rows[i].getNumOfBlacks();
				}

				homeSum = homeSum + blackTotal;
				if (homeSum == 15)
				{
					blackHome = true;
					blackTotal++;
                    if (rows[index].getNumOfBlacks() == 0)
                    {
                        while (++index < 24)
                        {
							if(rows[index].getNumOfBlacks()>0)
								rows[index].decrementBlack();
						}
                    }
                    else 
						rows[index].decrementBlack();
					return 0;
				}
				else
				{
					return 1;
				}
			}
			else if (rows[moved].getNumOfWhites() > 1)
			{
				// Cannot move on row containing two or more opponents pieces
				return 1;
			}
			else if (rows[moved].getNumOfWhites() == 1)
			{
				rows[index].decrementBlack();
				rows[moved].decrementWhite();
				rows[moved].incrementBlack();
				whitePending++;
				return 0;
			}
			else
			{
				rows[index].decrementBlack();
				rows[moved].incrementBlack();
				return 0;
			}
		}
		public	bool checkWhiteWin()
		{
			return whiteTotal == 15;
		}
		public	bool checkBlackWin()
		{
			return blackTotal == 15;
		}
		public int getWhites(int i, int j)
        {
			return rows[i*12+j].getNumOfWhites();
        }
		public int getBlacks(int i, int j)
		{
			return rows[i*12+j].getNumOfBlacks();
		}
		public int getBlackPending()
        {
			return blackPending;
        }
		public int getWhitePending()
		{
			return whitePending;
		}
	}
}
