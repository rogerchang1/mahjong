using System;
using System.Collections.Generic;
using System.Text;

using Mahjong.Model;

namespace Mahjong
{
    public class CHandParser
    {

        public CHandParser()
        {
        }

        public Hand parseHand(string psHand)
        {
            String currentSuit = "";
            Hand oHand = new Hand();
            for (int i = psHand.Length - 1; i >= 0; i--)
            {

                if (Char.IsDigit(psHand[i]))
                {
                    oHand.Tiles.Insert(0, new Tile(psHand[i] - '0', currentSuit));
                }
                else
                {
                    currentSuit = psHand[i].ToString().ToLower();
                }
            }
            return oHand;
        }
    }
}
