using Mahjong.Model;
using System;
using System.Collections.Generic;

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

        public List<Tile> ParseTileStringToTileList(string psTiles)
        {
            String currentSuit = "";
            List<Tile> oTileList = new List<Tile>();
            for (int i = psTiles.Length - 1; i >= 0; i--)
            {

                if (Char.IsDigit(psTiles[i]))
                {
                    oTileList.Insert(0, new Tile(psTiles[i] - '0', currentSuit));
                }
                else
                {
                    currentSuit = psTiles[i].ToString().ToLower();
                }
            }
            return oTileList;
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
