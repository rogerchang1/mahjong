using Mahjong.Model;
using System;
using static Mahjong.Enums;

namespace Mahjong
{
    public class CBlockParser
    {

        public CBlockParser()
        {
        }

        public Block ParseBlock(string psBlock, Mentsu peType = Mentsu.Unknown, Boolean pbIsOpen = false)
        {
            String currentSuit = "";
            Block oBlock = new Block();
            for (int i = psBlock.Length - 1; i >= 0; i--)
            {

                if (Char.IsDigit(psBlock[i]))
                {
                    oBlock.Tiles.Insert(0, new Tile(psBlock[i] - '0', currentSuit));
                }
                else
                {
                    currentSuit = psBlock[i].ToString().ToLower();
                }
            }

            oBlock.Type = peType;
            oBlock.IsOpen = pbIsOpen;

            return oBlock;
        }

        public String ToString(Block poBlock)
        {

            //poHand.SortTiles();

            String sBlock = "";

            for (int i = 0; i < poBlock.Tiles.Count; i++)
            {

                sBlock += poBlock.Tiles[i].ToString();
            }
            return sBlock;
        }
    }
}
