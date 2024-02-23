using Mahjong.Model;
using System;
using System.Collections.Generic;

namespace Mahjong
{
    public class CShantenEvaluator
    {

        private CTilesManager _TilesManager;

        //TODO:
        //1. Kan
        //2. Open Hands
        //3. Kokushi Musou
        public CShantenEvaluator()
        {
            _TilesManager = new CTilesManager();
        }

        public int EvaluateShanten(List<Tile> poTilesList)
        {

            _TilesManager.SortTiles(poTilesList);
            int shantenNormal = 8 - EvaluateShantenNormal(_TilesManager.Clone(poTilesList), 4);
            int shantenChiitoi = 6 - EvaluateShantenForChiitoi(_TilesManager.Clone(poTilesList), 7);
            return Math.Min(shantenNormal, shantenChiitoi);
        }

        public int EvaluateShantenNormal(List<Tile> poTilesList, int nBlocksLeft)
        {
            if (poTilesList.Count <= 1 || nBlocksLeft <= 0)
            {
                return 0;
            }

            bool bHasPair = false;
            int max = 0;

            for (int i = 0; i < poTilesList.Count; i++)
            {
                if (IsPairDetected(poTilesList, i))
                {
                    bHasPair = true;
                    List<Tile> tempHand = _TilesManager.Clone(poTilesList);
                    max = Math.Max(max, 1 + EvaluateShantenNormalHelper(RemovePairAtIndex(tempHand, i), nBlocksLeft));
                }
            }
            if (!bHasPair)
            {
                max = Math.Max(max, EvaluateShantenNormalHelper(poTilesList, nBlocksLeft));
            }
            return max;
        }

        private int EvaluateShantenNormalHelper(List<Tile> poTilesList, int nBlocksLeft)
        {
            if (poTilesList.Count <= 1 || nBlocksLeft <= 0)
            {
                return 0;
            }
            int max = 0;
            int i = 0;
            while (i < poTilesList.Count)
            {
                if (IsSequenceDetected(poTilesList, i) && nBlocksLeft > 0)
                {
                    max += 2;
                    nBlocksLeft--;
                    RemoveSequenceAtIndex(poTilesList, i);
                    i--;
                }
                else if (IsTripletDetected(poTilesList, poTilesList[i]) && nBlocksLeft > 0)
                {
                    max += 2;
                    nBlocksLeft--;
                    RemoveTripletAtIndex(poTilesList, i);
                    i--;
                    //max = Math.Max(max, 2 + EvaluateShantenNormal(RemoveSequenceAtIndex(poTilesList.Clone(), i), nBlocksLeft - 1));
                }
                i++;
            }
            i = 0;
            while (i < poTilesList.Count)
            {
                if (IsPartialSequenceDetected(poTilesList, i) && nBlocksLeft > 0)
                {
                    max++;
                    nBlocksLeft--;
                    RemovePartialSequenceAtIndex(poTilesList, i);
                    i--;
                }
                else if (IsPairDetected(poTilesList, i) && nBlocksLeft > 0)
                {
                    max++;
                    nBlocksLeft--;
                    RemovePairAtIndex(poTilesList, i);
                    i--;
                }
                i++;
            }
            return max;
        }

        public int EvaluateShantenForChiitoi(List<Tile> poTilesList, int nBlocksLeft)
        {
            if (poTilesList.Count <= 1 || nBlocksLeft <= 0)
            {
                return 0;
            }

            int max = 0;

            for (int i = 0; i < poTilesList.Count; i++)
            {
                if (IsPairDetected(poTilesList, i))
                {
                    _TilesManager.RemoveAllTilesOf(poTilesList, poTilesList[i]);
                    max = 1 + EvaluateShantenForChiitoi(poTilesList, nBlocksLeft - 1);
                }
            }
            return max;
        }

        #region GroupDetection
        private Boolean IsSequenceDetected(List<Tile> poSortedTilesList, int index)
        {
            if (index > poSortedTilesList.Count)
            {
                return false;
            }
            Tile currentTile = poSortedTilesList[index];
            if (currentTile.suit == "z")
            {
                return false;
            }
            Tile seqTile2 = _TilesManager.GetNextIncreasingTileInTheSameSuit(poSortedTilesList, currentTile);
            Tile seqTile3 = _TilesManager.GetNextIncreasingTileInTheSameSuit(poSortedTilesList, seqTile2);
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

        private Boolean IsTripletDetected(List<Tile> poSortedTilesList, Tile tile)
        {
            return _TilesManager.CountNumberOfTilesOf(poSortedTilesList, tile) >= 3;
        }

        private Boolean IsPartialSequenceDetected(List<Tile> poSortedTilesList, int index)
        {
            if (index > poSortedTilesList.Count)
            {
                return false;
            }
            Tile currentTile = poSortedTilesList[index];
            if (currentTile.suit == "z")
            {
                return false;
            }
            Tile seqTile2 = _TilesManager.GetNextIncreasingTileInTheSameSuit(poSortedTilesList, currentTile);
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

        private Boolean IsPairDetected(List<Tile> poSortedTilesList, int index)
        {
            if (index > poSortedTilesList.Count)
            {
                return false;
            }
            Tile currentTile = poSortedTilesList[index];
            return _TilesManager.CountNumberOfTilesOf(poSortedTilesList, currentTile) >= 2;
        }
        #endregion

        #region GroupRemoval
        private List<Tile> RemoveSequenceAtIndex(List<Tile> poSortedTilesList, int index)
        {
            if (index > poSortedTilesList.Count)
            {
                return poSortedTilesList;
            }
            Tile currentTile = poSortedTilesList[index];
            Tile seqTile2 = _TilesManager.GetNextIncreasingTileInTheSameSuit(poSortedTilesList, currentTile);
            Tile seqTile3 = _TilesManager.GetNextIncreasingTileInTheSameSuit(poSortedTilesList, seqTile2);
            _TilesManager.RemoveSingleTileOf(poSortedTilesList, currentTile);
            _TilesManager.RemoveSingleTileOf(poSortedTilesList, seqTile2);
            _TilesManager.RemoveSingleTileOf(poSortedTilesList, seqTile3);
            return poSortedTilesList;
        }

        private List<Tile> RemovePartialSequenceAtIndex(List<Tile> poSortedTilesList, int index)
        {
            if (index > poSortedTilesList.Count)
            {
                return poSortedTilesList;
            }
            Tile currentTile = poSortedTilesList[index];
            Tile seqTile2 = _TilesManager.GetNextIncreasingTileInTheSameSuit(poSortedTilesList, currentTile);
            _TilesManager.RemoveSingleTileOf(poSortedTilesList, currentTile);
            _TilesManager.RemoveSingleTileOf(poSortedTilesList, seqTile2);
            return poSortedTilesList;
        }

        private List<Tile> RemoveTripletAtIndex(List<Tile> poSortedTilesList, int index)
        {
            if (index > poSortedTilesList.Count)
            {
                return poSortedTilesList;
            }
            Tile currentTile = poSortedTilesList[index];
            _TilesManager.RemoveSingleTileOf(poSortedTilesList, currentTile);
            _TilesManager.RemoveSingleTileOf(poSortedTilesList, currentTile);
            _TilesManager.RemoveSingleTileOf(poSortedTilesList, currentTile);
            return poSortedTilesList;
        }

        private List<Tile> RemovePairAtIndex(List<Tile> poSortedTilesList, int index)
        {
            if (index > poSortedTilesList.Count)
            {
                return poSortedTilesList;
            }
            Tile currentTile = poSortedTilesList[index];
            _TilesManager.RemoveSingleTileOf(poSortedTilesList, currentTile);
            _TilesManager.RemoveSingleTileOf(poSortedTilesList, currentTile);
            return poSortedTilesList;
        }
        #endregion
    }
}
