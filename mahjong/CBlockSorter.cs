using Mahjong.Exceptions;
using Mahjong.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong
{
    public class CBlockSorter
    {
        
        public CBlockSorter()
        {

        }
        
        public List<List<Block>> GetBlockCombinations(Hand poHand)
        {
            List<List<Block>> oBlockCombinations = new List<List<Block>>();
            List<Block> oBlockCombination = new List<Block>();
            CShantenEvaluator oShantenEvaluator = new CShantenEvaluator();
            if(oShantenEvaluator.EvaluateShanten(poHand) != -1)
            {
                throw new BlockSortException();
            }

            poHand.SortTiles();

            List<Block> oPossiblePairs = GetListOfPossiblePairs(poHand);

            foreach(Block oPair in oPossiblePairs)
            {
                Hand oTempHand = poHand.Clone();
                
                oTempHand.RemoveNumInstancesTileOf(oPair.Tiles[0], 2);

                List<List<Block>> oSubBlockCombinations = GetBlockCombinationsWithNoPair(oTempHand);
                if(oSubBlockCombinations != null)
                {
                    foreach (List<Block> oSubBlockCombination in oSubBlockCombinations)
                    {
                        oSubBlockCombination.Add(oPair);
                        oBlockCombinations.Add(oSubBlockCombination);
                    }
                }
            }
            return oBlockCombinations;
        }

        public List<List<Block>> GetBlockCombinationsWithNoPair(Hand poHand)
        {
            if(poHand.Tiles.Count < 3)
            {
                return null;
            }

            List<List<Block>> oBlockCombinations = new List<List<Block>>();

            poHand.SortTiles();

            if (poHand.Tiles.Count == 3 || poHand.Tiles.Count == 4)
            {
                
                int nNumberOfSameTiles = poHand.CountNumberOfTilesOf(poHand.Tiles[0]);
                Boolean bIsStartOfAValidRun = poHand.CanBeStartOfARun(poHand.Tiles[0]);
                if (nNumberOfSameTiles == 3 || nNumberOfSameTiles == 4 || bIsStartOfAValidRun)
                {
                    List<Block> oBlockCombination = new List<Block>();
                    Block oBlock = new Block();
                    for(int i = 0; i < poHand.Tiles.Count; i++)
                    {
                        oBlock.Tiles.Add(poHand.Tiles[i]);
                    }
                    oBlockCombination.Insert(0,oBlock);
                    oBlockCombinations.Insert(0,oBlockCombination);
                    return oBlockCombinations;
                }
                else
                {
                    return null;
                }
            }


            Tile o1stTile = poHand.Tiles[0];
           
            if(poHand.CountNumberOfTilesOf(o1stTile) == 3)
            {
                Hand oTempHand = poHand.Clone();
                    
                Block oBlock = new Block();
                oBlock.Tiles.Add(o1stTile);
                oBlock.Tiles.Add(o1stTile);
                oBlock.Tiles.Add(o1stTile);

                oTempHand.RemoveNumInstancesTileOf(o1stTile, 3);

                List<List<Block>> oSubBlockCombinations = GetBlockCombinationsWithNoPair(oTempHand);
                if (oSubBlockCombinations != null)
                {
                    foreach (List<Block> oListBlock in oSubBlockCombinations)
                    {
                        oListBlock.Insert(0,oBlock);
                        oBlockCombinations.Insert(0,oListBlock);
                    }

                }
                

            }
            if (poHand.CanBeStartOfARun(o1stTile))
            {
                Hand oTempHand = poHand.Clone();

                Tile o2ndTile = poHand.GetNextSequentialTileInTheSameSuit(o1stTile);
                Tile o3rdTile = poHand.GetNextSequentialTileInTheSameSuit(o2ndTile);

                Block oBlock = new Block();
                oBlock.Tiles.Add(o1stTile);
                oBlock.Tiles.Add(o2ndTile);
                oBlock.Tiles.Add(o3rdTile);

                oTempHand.RemoveNumInstancesTileOf(o1stTile, 1);
                oTempHand.RemoveNumInstancesTileOf(o2ndTile, 1);
                oTempHand.RemoveNumInstancesTileOf(o3rdTile, 1);

                List<List<Block>> oSubBlockCombinations = GetBlockCombinationsWithNoPair(oTempHand);
                if(oSubBlockCombinations != null)
                {
                    foreach(List<Block> oListBlock in oSubBlockCombinations)
                    {
                        oListBlock.Insert(0,oBlock);
                        oBlockCombinations.Insert(0,oListBlock);
                    }
                }
            }

            if(oBlockCombinations.Count == 0)
            {
                return null;
            }


            return oBlockCombinations;
        }

        private Block ExtractRunWithStartingTile(Hand poHand, Tile poStartingTile)
        {
            Block oBlock = new Block();
            Tile o2ndTile = new Tile(poStartingTile.CompareValue + 1);
            Tile o3rdTile = new Tile(poStartingTile.CompareValue + 2);

            if(poHand.FindFirstIndexOfTile(o2ndTile) == -1 || poHand.FindFirstIndexOfTile(o3rdTile) == -1)
            {
                return oBlock;
            }

            oBlock.Tiles.Add(poStartingTile);
            oBlock.Tiles.Add(o2ndTile);
            oBlock.Tiles.Add(o3rdTile);

            return oBlock;
        }

        private Block ExtractPairWithTile(Hand poHand, Tile poTile)
        {
            Block oBlock = new Block();



            return oBlock;
        }

        public List<Block> GetListOfPossiblePairs(Hand poHand)
        {
            List<Block> oBlockList = new List<Block>();

            Tile oCurrentTile = null;
            for(int i = 0; i < poHand.Tiles.Count; i++)
            {
                if(oCurrentTile != null && oCurrentTile.CompareTo(poHand.Tiles[i]) == 0)
                {
                    continue;  
                }

                oCurrentTile = poHand.Tiles[i];

                if (poHand.CountNumberOfTilesOf(oCurrentTile) >= 2)
                {
                    Block oBlock = new Block();
                    oBlock.Tiles.Add(oCurrentTile);
                    oBlock.Tiles.Add(oCurrentTile);
                    oBlockList.Add(oBlock);
                }

            }
            return oBlockList;
        }

        private Block ExtractTripletWithTile(Hand poHand, Tile poTile)
        {
            Block oBlock = new Block();



            return oBlock;
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
