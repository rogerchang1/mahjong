using Mahjong.Model;
using System;
using System.Collections.Generic;

namespace Mahjong
{
    public class CYakuEvaluator
    {
        private CBlockSorter _BlockSorter;
        private CShantenEvaluator _ShantenEvaluator;

        public CYakuEvaluator()
        {
            _ShantenEvaluator = new CShantenEvaluator();
            _BlockSorter = new CBlockSorter();
        }

        public List<List<String>> EvaluateYaku(Hand poHand)
        {
            List<List<String>> oYakuCombinations = new List<List<String>>();

            if (_ShantenEvaluator.EvaluateShanten(poHand) != -1)
            {
                return oYakuCombinations;
            }

            List<List<Block>> oBlockCombinations = _BlockSorter.GetBlockCombinations(poHand);

            foreach (List<Block> oBlockCombination in oBlockCombinations)
            {
                oYakuCombinations.Add(EvaluateYakusFromSingleBlockCombination(poHand, oBlockCombination));
            }

            return oYakuCombinations;
        }

        public List<String> EvaluateYakusFromSingleBlockCombination(Hand poHand, List<Block> poBlockConfiguration)
        {
            List<String> oYakuCombination = new List<String>();

            if (IsTanyao(poBlockConfiguration))
            {
                oYakuCombination.Add("Tanyao");
            }


            return oYakuCombination;
        }

        public Boolean IsPinfu(Hand poHand, List<Block> poBlockCombination)
        {
            if(poHand.LockedBlocks.Count > 0)
            {
                return false;
            }
            
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
    }
}
