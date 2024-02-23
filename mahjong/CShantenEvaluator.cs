using Mahjong.Model;
using System;

namespace Mahjong
{
    public class CShantenEvaluator
    {

        private CHandManager _HandManager;

        //TODO:
        //1. Kan
        //2. Open Hands
        //3. Kokushi Musou
        public CShantenEvaluator()
        {
            _HandManager = new CHandManager();
        }

        public int EvaluateShanten(Hand poHand)
        {

            _HandManager.SortTiles(poHand);
            int shantenNormal = 8 - EvaluateShantenNormal(_HandManager.Clone(poHand), 4);
            int shantenChiitoi = 6 - EvaluateShantenForChiitoi(_HandManager.Clone(poHand), 7);
            return Math.Min(shantenNormal, shantenChiitoi);
        }

        public int EvaluateShantenNormal(Hand poHand, int nBlocksLeft)
        {
            if (poHand.Tiles.Count <= 1 || nBlocksLeft <= 0)
            {
                return 0;
            }

            CHandManager oHandManager = new CHandManager();
            bool bHasPair = false;
            int max = 0;

            for (int i = 0; i < poHand.Tiles.Count; i++)
            {
                if (IsPairDetected(poHand, i))
                {
                    bHasPair = true;
                    Hand tempHand = oHandManager.Clone(poHand);
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
            while (i < poHand.Tiles.Count)
            {
                if (IsSequenceDetected(poHand, i) && nBlocksLeft > 0)
                {
                    max += 2;
                    nBlocksLeft--;
                    RemoveSequenceAtIndex(poHand, i);
                    i--;
                }
                else if (IsTripletDetected(poHand, poHand.Tiles[i]) && nBlocksLeft > 0)
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
                    max++;
                    nBlocksLeft--;
                    RemovePartialSequenceAtIndex(poHand, i);
                    i--;
                }
                else if (IsPairDetected(poHand, i) && nBlocksLeft > 0)
                {
                    max++;
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
                    _HandManager.RemoveAllTilesOf(poHand, poHand.Tiles[i]);
                    max = 1 + EvaluateShantenForChiitoi(poHand, nBlocksLeft - 1);
                }
            }
            return max;
        }

        private Boolean IsSequenceDetected(Hand poSortedHand, int index)
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
            Tile seqTile2 = _HandManager.GetNextIncreasingTileInTheSameSuit(poSortedHand, currentTile);
            Tile seqTile3 = _HandManager.GetNextIncreasingTileInTheSameSuit(poSortedHand, seqTile2);
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
            return _HandManager.CountNumberOfTilesOf(poSortedHand, tile) >= 3;
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
            Tile seqTile2 = _HandManager.GetNextIncreasingTileInTheSameSuit(poSortedHand, currentTile);
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
            return _HandManager.CountNumberOfTilesOf(poSortedHand, currentTile) >= 2;
        }

        private Hand RemoveSequenceAtIndex(Hand poSortedHand, int index)
        {
            if (index > poSortedHand.Tiles.Count)
            {
                return poSortedHand;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            Tile seqTile2 = _HandManager.GetNextIncreasingTileInTheSameSuit(poSortedHand, currentTile);
            Tile seqTile3 = _HandManager.GetNextIncreasingTileInTheSameSuit(poSortedHand, seqTile2);
            _HandManager.RemoveSingleTileOf(poSortedHand, currentTile);
            _HandManager.RemoveSingleTileOf(poSortedHand, seqTile2);
            _HandManager.RemoveSingleTileOf(poSortedHand, seqTile3);
            return poSortedHand;
        }

        private Hand RemovePartialSequenceAtIndex(Hand poSortedHand, int index)
        {
            if (index > poSortedHand.Tiles.Count)
            {
                return poSortedHand;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            Tile seqTile2 = _HandManager.GetNextIncreasingTileInTheSameSuit(poSortedHand, currentTile);
            _HandManager.RemoveSingleTileOf(poSortedHand, currentTile);
            _HandManager.RemoveSingleTileOf(poSortedHand, seqTile2);
            return poSortedHand;
        }

        private Hand RemoveTripletAtIndex(Hand poSortedHand, int index)
        {
            if (index > poSortedHand.Tiles.Count)
            {
                return poSortedHand;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            _HandManager.RemoveSingleTileOf(poSortedHand, currentTile);
            _HandManager.RemoveSingleTileOf(poSortedHand, currentTile);
            _HandManager.RemoveSingleTileOf(poSortedHand, currentTile);
            return poSortedHand;
        }

        private Hand RemovePairAtIndex(Hand poSortedHand, int index)
        {
            if (index > poSortedHand.Tiles.Count)
            {
                return poSortedHand;
            }
            Tile currentTile = poSortedHand.Tiles[index];
            _HandManager.RemoveSingleTileOf(poSortedHand, currentTile);
            _HandManager.RemoveSingleTileOf(poSortedHand, currentTile);
            return poSortedHand;
        }
    }
}
