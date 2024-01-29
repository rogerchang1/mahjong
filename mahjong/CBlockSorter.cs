using Mahjong.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong
{
    class CBlockSorter
    {
        public List<List<Block>> GetBlockCombinations(Hand poHand)
        {
            List<List<Block>> oBlockCombinations = new List<List<Block>>();

            return oBlockCombinations;
        }

        //public int EvaluateShantenNormalHelper(Hand poHand, int nBlocksLeft)
        //{
        //    if (poHand.Tiles.Count <= 1 || nBlocksLeft <= 0)
        //    {
        //        return 0;
        //    }
        //    int max = 0;
        //    int i = 0;
        //    while (i < poHand.Tiles.Count)
        //    {
        //        if (IsSequenceDetected(poHand, i) && nBlocksLeft > 0)
        //        {
        //            max += 2;
        //            nBlocksLeft--;
        //            RemoveSequenceAtIndex(poHand, i);
        //            i--;
        //        }
        //        else if (IsTripletDetected(poHand, poHand.Tiles[i]) && nBlocksLeft > 0)
        //        {
        //            max += 2;
        //            nBlocksLeft--;
        //            RemoveTripletAtIndex(poHand, i);
        //            i--;
        //        }
        //        i++;
        //    }
        //    i = 0;
        //    while (i < poHand.Tiles.Count)
        //    {
        //        if (IsPartialSequenceDetected(poHand, i) && nBlocksLeft > 0)
        //        {
        //            max++;
        //            nBlocksLeft--;
        //            RemovePartialSequenceAtIndex(poHand, i);
        //            i--;
        //        }
        //        else if (IsPairDetected(poHand, i) && nBlocksLeft > 0)
        //        {
        //            max++;
        //            nBlocksLeft--;
        //            RemovePairAtIndex(poHand, i);
        //            i--;
        //        }
        //        i++;
        //    }
        //    return max;
        //}
    }
}
