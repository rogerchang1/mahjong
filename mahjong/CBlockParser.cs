using Mahjong.Model;
using System;
using static Mahjong.Enums;

namespace Mahjong
{
    public class CBlockParser
    {
        private CTilesManager _TilesManager;
        public CBlockParser()
        {
            _TilesManager = new CTilesManager();
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

            if(oBlock.Type == Mentsu.Unknown)
            {
                oBlock.Type = EvaluateMentsuType(oBlock);
            }


            return oBlock;
        }

        public Mentsu EvaluateMentsuType(Block poBlock)
        {
            _TilesManager.SortTiles(poBlock.Tiles);
            if(poBlock.Tiles.Count == 4 && poBlock.Tiles[0].CompareTo(poBlock.Tiles[3]) == 0)
            {
                return Mentsu.Kantsu;
            }

            if (poBlock.Tiles.Count == 3)
            {
                if(poBlock.Tiles[0].CompareTo(poBlock.Tiles[2]) == 0)
                {
                    return Mentsu.Koutsu;
                }
                if (poBlock.Tiles[0].CompareTo(poBlock.Tiles[1]) == -1 && poBlock.Tiles[1].CompareTo(poBlock.Tiles[2]) == -1)
                {
                    return Mentsu.Shuntsu;
                }
            }

            if (poBlock.Tiles.Count == 2 && poBlock.Tiles[0].CompareTo(poBlock.Tiles[1]) == 0)
            {
                return Mentsu.Jantou;
            }

            return Mentsu.Unknown;
        }

        public Block ParseBlock(string psBlock, Boolean pbIsOpen)
        {
            return ParseBlock(psBlock, Mentsu.Unknown, pbIsOpen);
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
