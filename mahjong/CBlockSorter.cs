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

        //TODO:
        //1. Kan
        //2. Open Hands
        //3. Kokushi Musou
        public List<List<Block>> GetBlockCombinations(Hand pHand)
        {
            List<List<Block>> oBlockCombinations = new List<List<Block>>();
            List<Block> oBlockCombination = new List<Block>();
            CShantenEvaluator oShantenEvaluator = new CShantenEvaluator();
            CHandManager oHandManager = new CHandManager();
            if(oShantenEvaluator.EvaluateShanten(pHand) != -1)
            {
                throw new BlockSortException();
            }

            Hand poHand = oHandManager.Clone(pHand); //Eventually should rename poHand to oTempHand or something to remove the open blocks from it before processin.
            //Should remove open blocks from the hand before processing it over here or something.

            //poHand.SortTiles();
            oHandManager.SortTiles(poHand);

            List<Block> oPossiblePairs = GetListOfPossiblePairBlocks(poHand);

            foreach(Block oPair in oPossiblePairs)
            {
                Hand oTempHand = oHandManager.Clone(poHand);

                oHandManager.RemoveNumInstancesTileOf(oTempHand, oPair.Tiles[0], 2);

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
            CHandManager oHandManager = new CHandManager();
            List<List<Block>> oBlockCombinations = new List<List<Block>>();

            oHandManager.SortTiles(poHand);

            if (poHand.Tiles.Count == 3 || poHand.Tiles.Count == 4)
            {
                
                int nNumberOfSameTiles = oHandManager.CountNumberOfTilesOf(poHand, poHand.Tiles[0]);
                Boolean bIsStartOfAValidRun = oHandManager.CanBeStartOfARun(poHand, poHand.Tiles[0]);
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
           
            if(oHandManager.CountNumberOfTilesOf(poHand, o1stTile) == 3)
            {
                Hand oTempHand = oHandManager.Clone(poHand);
                    
                Block oBlock = new Block();
                oBlock.Tiles.Add(o1stTile);
                oBlock.Tiles.Add(o1stTile);
                oBlock.Tiles.Add(o1stTile);
                oBlock.Type = Enums.Mentsu.Koutsu;

                oHandManager.RemoveNumInstancesTileOf(oTempHand, o1stTile, 3);

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
            if (oHandManager.CanBeStartOfARun(poHand, o1stTile))
            {
                Hand oTempHand = oHandManager.Clone(poHand);

                Tile o2ndTile = oHandManager.GetNextIncreasingTileInTheSameSuit(poHand, o1stTile);
                Tile o3rdTile = oHandManager.GetNextIncreasingTileInTheSameSuit(poHand, o2ndTile);

                Block oBlock = new Block();
                oBlock.Tiles.Add(o1stTile);
                oBlock.Tiles.Add(o2ndTile);
                oBlock.Tiles.Add(o3rdTile);
                oBlock.Type = Enums.Mentsu.Shuntsu;
                oHandManager.RemoveNumInstancesTileOf(oTempHand, o1stTile, 1);
                oHandManager.RemoveNumInstancesTileOf(oTempHand, o2ndTile, 1);
                oHandManager.RemoveNumInstancesTileOf(oTempHand, o3rdTile, 1);

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

        public List<Block> GetListOfPossiblePairBlocks(Hand poHand)
        {
            List<Block> oBlockList = new List<Block>();
            CHandManager oHandManager = new CHandManager();

            Tile oCurrentTile = null;
            for(int i = 0; i < poHand.Tiles.Count; i++)
            {
                if(oCurrentTile != null && oCurrentTile.CompareTo(poHand.Tiles[i]) == 0)
                {
                    continue;  
                }

                oCurrentTile = poHand.Tiles[i];

                if (oHandManager.CountNumberOfTilesOf(poHand, oCurrentTile) >= 2)
                {
                    Block oBlock = new Block();
                    oBlock.Tiles.Add(oCurrentTile);
                    oBlock.Tiles.Add(oCurrentTile);
                    oBlockList.Add(oBlock);
                    oBlock.Type = Enums.Mentsu.Jantou;
                }
            }
            return oBlockList;
        }
    }
}
