using Mahjong.Model;
using System;

namespace Mahjong
{
    public class CHandParser
    {

        public CHandParser()
        {
        }

        public Hand ParseHand(string psHand)
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

        public String ToString(Hand poHand)
        {

            //poHand.SortTiles();

            String sHand = "";

            for (int i = 0; i < poHand.Tiles.Count; i++)
            {

                sHand += poHand.Tiles[i].ToString();
            }
            return sHand;
        }
    }
}
