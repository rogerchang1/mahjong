using Mahjong.Model;
using System;
using System.Collections.Generic;
using static Mahjong.Enums;

namespace Mahjong
{
    public class CYakuEvaluator
    {
        private CBlockSorter _BlockSorter;
        private CBlockParser _BlockParser;
        private CShantenEvaluator _ShantenEvaluator;
        private CTilesManager _TilesManager;

        public CYakuEvaluator()
        {
            _ShantenEvaluator = new CShantenEvaluator();
            _BlockSorter = new CBlockSorter();
            _BlockParser = new CBlockParser();
            _TilesManager = new CTilesManager();
        }

        public List<List<Yaku>> EvaluateYaku(Hand poHand)
        {
            List<List<Yaku>> oYakuCombinations = new List<List<Yaku>>();

            if (_ShantenEvaluator.EvaluateShanten(poHand.Tiles) != -1)
            {
                return oYakuCombinations;
            }

            List<List<Block>> oBlockCombinations = _BlockSorter.GetBlockCombinations(poHand.Tiles);

            foreach (List<Block> oBlockCombination in oBlockCombinations)
            {
                oYakuCombinations.Add(EvaluateYakusFromSingleBlockCombination(poHand, oBlockCombination));
            }

            return oYakuCombinations;
        }

        public List<Yaku> EvaluateYakusFromSingleBlockCombination(Hand poHand, List<Block> poBlockConfiguration)
        {
            List<Yaku> oYakuCombination = new List<Yaku>();
            //Order of Yakus matter, for instance, Daisangen > Shousangen, Rynpeikou > Iipeikou, Junchan > Chanta
            if (IsPinfu(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Pinfu);
            }
            if (IsTanyao(poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Tanyao);
            }
            if (IsIipeikou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Iipeikou);
            }
            if (IsSanankou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Sanankou);
            }
            if (IsChinitsu(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Chinitsu);
            }

            return oYakuCombination;
        }

        #region YakuList
        public Boolean IsPinfu(Hand poHand, List<Block> poBlockCombination)
        {
            if (!IsHandClosed(poHand))
            {
                return false;
            }

            Boolean bWinTileIsInRyanmen = false;

            foreach (Block oBlock in poBlockCombination)
            {
                switch (oBlock.Type)
                {
                    case Mentsu.Koutsu:
                        return false;
                    case Mentsu.Kantsu:
                        return false;
                    case Mentsu.Jantou:
                        if (_TilesManager.IsDragonTile(oBlock.Tiles[0]))
                        {
                            return false;
                        }
                        if (_TilesManager.IsWindTile(oBlock.Tiles[0]) && (WindTileToEnum(oBlock.Tiles[0]) == poHand.RoundWind || WindTileToEnum(oBlock.Tiles[0]) == poHand.SeatWind))
                        {
                            return false;
                        }
                        break;
                    case Mentsu.Shuntsu:
                        if (bWinTileIsInRyanmen == false && (oBlock.Tiles[0].CompareTo(poHand.WinTile) == 0 || oBlock.Tiles[2].CompareTo(poHand.WinTile) == 0))
                        {
                            bWinTileIsInRyanmen = true;
                        }
                        break;
                    default:
                        return false;
                }
            }
            if (bWinTileIsInRyanmen)
            {
                return true;
            }
            return false;
        }
        
        public Boolean IsTanyao(List<Block> poBlockCombination)
        {
            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                Block oBlock = poBlockCombination[i];
                for (int j = 0; j < oBlock.Tiles.Count; j++)
                {
                    if (oBlock.Tiles[j].num == 1 || oBlock.Tiles[j].num == 9 || oBlock.Tiles[j].suit == "z")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Boolean IsIipeikou(Hand poHand, List<Block> poBlockCombination)
        {
            if (!IsHandClosed(poHand))
            {
                return false;
            }

            Boolean bIipeikouFound = false;

            for(int i = 0; i < poBlockCombination.Count; i++)
            {
                if(poBlockCombination[i].Type != Mentsu.Shuntsu)
                {
                    continue;
                }
                for(int j = i + 1; j < poBlockCombination.Count; j++)
                {
                    if (poBlockCombination[j].Type != Mentsu.Shuntsu)
                    {
                        continue;
                    }
                    if(poBlockCombination[i].Tiles[0].CompareTo(poBlockCombination[j].Tiles[0]) == 0)
                    {
                        if(bIipeikouFound == true)
                        {
                            return false;
                        }
                        bIipeikouFound = true;
                    }
                }
                if (bIipeikouFound)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsSanankou(Hand poHand, List<Block> poBlockCombination)
        {
            int count = 0;
            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                if (poBlockCombination[i].Type == Mentsu.Koutsu && poBlockCombination[i].IsOpen == false)
                {
                    count++;
                }
            }
            return count == 3;
        }

        public Boolean IsChinitsu(Hand poHand, List<Block> poBlockCombination)
        {
            List<Suit> oSuitList = _TilesManager.GetSuitsFromTileList(poHand.Tiles);
            if(oSuitList.Count == 1 && oSuitList[0] != Suit.Honor)
            {
                return true;
            }
            return false;
        }
        #endregion

        private Boolean IsHandClosed(Hand poHand)
        {
            foreach(Block oBlock in poHand.LockedBlocks)
            {
                if(oBlock.Type != Mentsu.Kantsu)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
