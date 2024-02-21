using Mahjong.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong
{
    public class CShantenEvaluator
    {
        public CShantenEvaluator()
        {

        }

        public int EvaluateShanten(Hand poHand)
        {

            poHand.SortTiles();
            int shantenNormal = 8 - EvaluateShantenNormal(poHand.Clone(), 4);
            int shantenChiitoi = 6 - EvaluateShantenForChiitoi(poHand.Clone(), 7);
            return Math.Min(shantenNormal, shantenChiitoi);
        }

        public int EvaluateShantenNormal(Hand poHand, int nBlocksLeft)
        {
            if(poHand.Tiles.Count <= 1 || nBlocksLeft <= 0)
            {
                return 0;
            }

            bool bHasPair = false;
            int max = 0;

            for(int i = 0; i < poHand.Tiles.Count; i++)
            {

                
                if (IsPairDetected(poHand, i))
                {
                    bHasPair = true;
                    Hand tempHand = poHand.Clone();
                    max = Math.Max(max, 1 + EvaluateShantenNormalHelper(RemovePairAtIndex(tempHand, i), nBlocksLeft));
                }
            }
            if (!bHasPair)
            {
                max = Math.Max(max, EvaluateShantenNormalHelper(poHand, nBlocksLeft));
            }
            return max;
        }

        public int EvaluateShantenNormalHelper(Hand poHand, int nBlocksLeft)
        {
            if (poHand.Tiles.Count <= 1 || nBlocksLeft <= 0)
            {
                return 0;
            }
            int max = 0;
            int i = 0;
            while(i < poHand.Tiles.Count)
            {
                if (IsSequenceDetected(poHand, i) && nBlocksLeft > 0)
                {
                    max += 2;
                    nBlocksLeft--;
                    RemoveSequenceAtIndex(poHand, i);
                    i--;
                } else if (IsTripletDetected(poHand, poHand.Tiles[i]) && nBlocksLeft > 0)
                {
                    max += 2;
                    nBlocksLeft--;
                    RemoveTripletAtIndex(poHand, i);
                    i--;
                    //max = Math.Max(max, 2 + EvaluateShantenNormal(RemoveSequenceAtIndex(poHand.Clone(), i), nBlocksLeft - 1));
                }
                i++;
            }
            i = 0;
            while (i < poHand.Tiles.Count)
            {
                if (IsPartialSequenceDetected(poHand, i) && nBlocksLeft > 0)
                {
                    max ++;
                    nBlocksLeft--;
                    RemovePartialSequenceAtIndex(poHand, i);
                    i--;
                }
                else if (IsPairDetected(poHand, i) && nBlocksLeft > 0)
                {
                    max ++;
                    nBlocksLeft--;
                    RemovePairAtIndex(poHand, i);
                    i--;
                }
                i++;
            }
            return max;
        }

        public int EvaluateShantenForChiitoi(Hand poHand, int nBlocksLeft)
        {
            if (poHand.Tiles.Count <= 1 || nBlocksLeft <= 0)
            {
                return 0;
            }

            int max = 0;

            for (int i = 0; i < poHand.Tiles.Count; i++)
            {
                if (IsPairDetected(poHand, i))
                {
                    poHand.RemoveAllTilesOf(poHand.Tiles[i]);
                    max = 1 + EvaluateShantenForChiitoi(poHand, nBlocksLeft - 1);
                }
            }
            return max;
        }

        private Boolean IsSequenceDetected(Hand poSortedHand, int index)
        {
            if(index > poSortedHand.Tiles.Count)
            {
                return false;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            if(currentTile.suit == "z")
            {
                return false;
            }
            Tile seqTile2 = poSortedHand.GetNextSequentialTileInTheSameSuit(currentTile);
            Tile seqTile3 = poSortedHand.GetNextSequentialTileInTheSameSuit(seqTile2);
            if (seqTile2 == null || seqTile3 == null)
            {
                return false;
            }
            if (currentTile.num + 1 == seqTile2.num && currentTile.num + 2 == seqTile3.num)
            {
                return true;
            }
            return false;
        }

        private Boolean IsTripletDetected(Hand poSortedHand, Tile tile)
        {
            return poSortedHand.CountNumberOfTilesOf(tile) >= 3;
        }

        private Boolean IsPartialSequenceDetected(Hand poSortedHand, int index)
        {
            if (index > poSortedHand.Tiles.Count)
            {
                return false;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            if (currentTile.suit == "z")
            {
                return false;
            }
            Tile seqTile2 = poSortedHand.GetNextSequentialTileInTheSameSuit(currentTile);
            if (seqTile2 == null)
            {
                return false;
            }
            if (seqTile2.num - currentTile.num <= 2)
            {
                return true;
            }
            return false;
        }

        private Boolean IsPairDetected(Hand poSortedHand, int index)
        {
            if (index > poSortedHand.Tiles.Count)
            {
                return false;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            return poSortedHand.CountNumberOfTilesOf(currentTile) >= 2;
        }

        private Hand RemoveSequenceAtIndex(Hand poSortedHand, int index)
        {
            if (index > poSortedHand.Tiles.Count)
            {
                return poSortedHand;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            Tile seqTile2 = poSortedHand.GetNextSequentialTileInTheSameSuit(currentTile);
            Tile seqTile3 = poSortedHand.GetNextSequentialTileInTheSameSuit(seqTile2);
            poSortedHand.RemoveSingleTileOf(currentTile);
            poSortedHand.RemoveSingleTileOf(seqTile2);
            poSortedHand.RemoveSingleTileOf(seqTile3);
            return poSortedHand;
        }

        private Hand RemovePartialSequenceAtIndex(Hand poSortedHand, int index)
        {
            if (index > poSortedHand.Tiles.Count)
            {
                return poSortedHand;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            Tile seqTile2 = poSortedHand.GetNextSequentialTileInTheSameSuit(currentTile);
            poSortedHand.RemoveSingleTileOf(currentTile);
            poSortedHand.RemoveSingleTileOf(seqTile2);
            return poSortedHand;
        }

        private Hand RemoveTripletAtIndex(Hand poSortedHand, int index)
        {
            if (index > poSortedHand.Tiles.Count)
            {
                return poSortedHand;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            poSortedHand.RemoveSingleTileOf(currentTile);
            poSortedHand.RemoveSingleTileOf(currentTile);
            poSortedHand.RemoveSingleTileOf(currentTile);
            return poSortedHand;
        }

        private Hand RemovePairAtIndex(Hand poSortedHand, int index)
        {
            if (index > poSortedHand.Tiles.Count)
            {
                return poSortedHand;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            poSortedHand.RemoveSingleTileOf(currentTile);
            poSortedHand.RemoveSingleTileOf(currentTile);
            return poSortedHand;
        }
    }
}
