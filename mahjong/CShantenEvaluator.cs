using Mahjong.Model;
using System;
using System.Collections.Generic;

namespace Mahjong
{
    public class CShantenEvaluator
    {

        private CTilesManager _TilesManager;
        private CHandParser _HandParser;

        //TODO:
        //1. Kan
        //2. Open Hands
        //3. Kokushi Musou
        public CShantenEvaluator()
        {
            _TilesManager = new CTilesManager();
            _HandParser = new CHandParser();
        }

        public int EvaluateShanten(Hand poHand)
        {
            List<Tile> oTileList = _TilesManager.Clone(poHand.Tiles);

            int nNumLockedBlocks = 0;

            for(int i = 0; i < poHand.LockedBlocks.Count; i++)
            {
                Block oBlock = poHand.LockedBlocks[i];
                for(int j = 0; j < oBlock.Tiles.Count; j++)
                {
                    _TilesManager.RemoveSingleTileOf(oTileList,oBlock.Tiles[j]);
                }
                nNumLockedBlocks++;
            }
            return EvaluateShanten(oTileList, 4- nNumLockedBlocks);
        }

        /// <summary>
        /// For evaluating a normal closed hand. Start at 8 and deduct 1 if it's a pair or partial group, deduct 2 if it's a full group. 
        /// For evaluating a chiitoi closed hand. Start at 6 and deduct 1 if it's a pair. 
        /// For evaluating a kokushi closed hand. Start at 13 and deduct 1 if it's in the kokushi tile list. Deduct only once again if a pair with any of kokushi tiles is found.
        /// nBlocksLeft = 4 implies there are no locked blocks, aka there is no closed kan or open calls.
        /// </summary>
        /// <param name="oTempTilesList"></param>
        /// <param name="nBlocksLeft"></param>
        /// <returns></returns>
        public int EvaluateShanten(List<Tile> poTilesList, int nBlocksLeft = 4)
        {
            List<Tile> oTempTilesList =_TilesManager.Clone(poTilesList);
            _TilesManager.SortTiles(oTempTilesList);

            if(nBlocksLeft < 4 && nBlocksLeft > 0)
            {
                return (nBlocksLeft * 2) - EvaluateShantenNormal(_TilesManager.Clone(oTempTilesList), nBlocksLeft);
            }

            int shantenNormal = (nBlocksLeft * 2) - EvaluateShantenNormal(_TilesManager.Clone(oTempTilesList), nBlocksLeft);
            int shantenChiitoi = EvaluateShantenForChiitoi(oTempTilesList);
            int shantenKokushi = EvaluateShantenForKokushiMusou(_TilesManager.Clone(oTempTilesList));
            return Math.Min(shantenNormal, Math.Min(shantenChiitoi, shantenKokushi));
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
                    List<Tile> tempHand = _TilesManager.Clone(poTilesList);
                    max = Math.Max(max, 1 + EvaluateShantenNormalHelper(RemovePairAtIndex(tempHand, i), nBlocksLeft));
                }
            }
            //Evaluate in the scenario that there's no pair
            max = Math.Max(max, EvaluateShantenNormalHelper(poTilesList, nBlocksLeft));
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
                
                if (IsTripletDetected(poTilesList, poTilesList[i]) && nBlocksLeft > 0)
                {
                    max += 2;
                    nBlocksLeft--;
                    RemoveTripletAtIndex(poTilesList, i);
                    i--;
                    //max = Math.Max(max, 2 + EvaluateShantenNormal(RemoveSequenceAtIndex(poTilesList.Clone(), i), nBlocksLeft - 1));
                }else if (IsSequenceDetected(poTilesList, i) && nBlocksLeft > 0)
                {
                    //edgecase for a 24567 pattern. we'll want to remove 567 instead of 456 to prioritize the partial shape of 24.
                    //what about a 2455567 pattern or 244567 pattern?
                    if(IsThereA24567Pattern(poTilesList,i))
                    {
                        Tile nextTile = _TilesManager.GetNextIncreasingTileInTheSameSuit(poTilesList, poTilesList[i]);
                        int nextTileIndex = _TilesManager.FindFirstIndexOfTile(poTilesList, nextTile);
                        RemoveSequenceAtIndex(poTilesList, nextTileIndex);
                    }
                    else
                    {
                        RemoveSequenceAtIndex(poTilesList, i);
                    }

                    max += 2;
                    nBlocksLeft--;
                    i--;
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

        //edgecase for a 24567 pattern. we'll want to remove 567 instead of 456 to prioritize the partial shape of 24.
        //Because we are going in ascending order when evaluating shanten, we might spot 456 and remove it, leaving 27 which won't amount to anything.
        //This function is assuming index would be the 4 in a 24567 pattern.
        //This is a suboptimal solution, might need to do dynamic programming refactor later in the future.
        private bool IsThereA24567Pattern(List<Tile> poTilesList, int pnIndex)
        {
            Tile prevTile = _TilesManager.GetPreviousDecreasingTileInTheSameSuit(poTilesList, poTilesList[pnIndex]);
            if(prevTile == null || prevTile.num != poTilesList[pnIndex].num - 2)
            {
                return false;
            }

            Tile nextTile = _TilesManager.GetNextIncreasingTileInTheSameSuit(poTilesList, poTilesList[pnIndex]);
            if(nextTile == null || nextTile.num != poTilesList[pnIndex].num + 1)
            {
                return false;
            }
            Tile nextTile2 = _TilesManager.GetNextIncreasingTileInTheSameSuit(poTilesList, nextTile);
            if (nextTile2 == null || nextTile2.num != nextTile.num + 1)
            {
                return false;
            }
            int nextTileIndex = _TilesManager.FindFirstIndexOfTile(poTilesList,nextTile);
            int nextTile2Index = _TilesManager.FindFirstIndexOfTile(poTilesList, nextTile2);
            if (IsSequenceDetected(poTilesList, nextTileIndex) && !IsSequenceDetected(poTilesList, nextTile2Index))
            {
                return true;
            }

            return false;
        }

        public int EvaluateShantenForChiitoi(List<Tile> poTilesList)
        {
            return 6 - EvaluateShantenForChiitoiHelper(_TilesManager.Clone(poTilesList), 7);
        }

        private int EvaluateShantenForChiitoiHelper(List<Tile> poTilesList, int nBlocksLeft)
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
                    max = 1 + EvaluateShantenForChiitoiHelper(poTilesList, nBlocksLeft - 1);
                }
            }
            return max;
        }

        public int EvaluateShantenForKokushiMusou(List<Tile> poTilesList)
        {
            if (poTilesList.Count == 0)
            {
                return 0;
            }

            int max = 0;
            Boolean bPairFound = false;

            List<Tile> oKokushiTileList = _HandParser.ParseTileStringToTileList("19p19s19m1234567z");

            for(int i = 0; i < oKokushiTileList.Count; i++)
            {
                int nNumTiles = _TilesManager.CountNumberOfTilesOf(poTilesList, oKokushiTileList[i]);
                if(nNumTiles > 0)
                {
                    max++;
                }
                if(bPairFound == false && nNumTiles > 1)
                {
                    max++;
                    bPairFound = true;
                }
            }
            return 13 - max;
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
