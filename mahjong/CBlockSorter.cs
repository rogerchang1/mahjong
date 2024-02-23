using Mahjong.Exceptions;
using Mahjong.Model;
using System;
using System.Collections.Generic;

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
        public List<List<Block>> GetBlockCombinations(List<Tile> pTilesList)
        {
            List<List<Block>> oBlockCombinations = new List<List<Block>>();
            List<Block> oBlockCombination = new List<Block>();
            CShantenEvaluator oShantenEvaluator = new CShantenEvaluator();
            CTilesManager oHandManager = new CTilesManager();
            if (oShantenEvaluator.EvaluateShanten(pTilesList) != -1)
            {
                throw new BlockSortException();
            }

            List<Tile> poTilesList = oHandManager.Clone(pTilesList); //Eventually should rename poTilesList to oTempTilesList or something to remove the open blocks from it before processin.
            //Should remove open blocks from the hand before processing it over here or something.

            //poTilesList.SortTiles();
            oHandManager.SortTiles(poTilesList);

            List<Block> oPossiblePairs = GetListOfPossiblePairBlocks(poTilesList);

            foreach (Block oPair in oPossiblePairs)
            {
                List<Tile> oTempTilesList = oHandManager.Clone(poTilesList);

                oHandManager.RemoveNumInstancesTileOf(oTempTilesList, oPair.Tiles[0], 2);

                List<List<Block>> oSubBlockCombinations = GetBlockCombinationsWithNoPair(oTempTilesList);
                if (oSubBlockCombinations != null)
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

        public List<List<Block>> GetBlockCombinationsWithNoPair(List<Tile> poTilesList)
        {
            if (poTilesList.Count < 3)
            {
                return null;
            }
            CTilesManager oHandManager = new CTilesManager();
            List<List<Block>> oBlockCombinations = new List<List<Block>>();

            oHandManager.SortTiles(poTilesList);

            if (poTilesList.Count == 3 || poTilesList.Count == 4)
            {

                int nNumberOfSameTiles = oHandManager.CountNumberOfTilesOf(poTilesList, poTilesList[0]);
                Boolean bIsStartOfAValidRun = oHandManager.CanBeStartOfARun(poTilesList, poTilesList[0]);
                if (nNumberOfSameTiles == 3 || nNumberOfSameTiles == 4 || bIsStartOfAValidRun)
                {
                    List<Block> oBlockCombination = new List<Block>();
                    Block oBlock = new Block();
                    for (int i = 0; i < poTilesList.Count; i++)
                    {
                        oBlock.Tiles.Add(poTilesList[i]);
                    }
                    oBlockCombination.Insert(0, oBlock);
                    oBlockCombinations.Insert(0, oBlockCombination);
                    return oBlockCombinations;
                }
                else
                {
                    return null;
                }
            }

            Tile o1stTile = poTilesList[0];

            if (oHandManager.CountNumberOfTilesOf(poTilesList, o1stTile) == 3)
            {
                List<Tile> oTempTilesList = oHandManager.Clone(poTilesList);

                Block oBlock = new Block();
                oBlock.Tiles.Add(o1stTile);
                oBlock.Tiles.Add(o1stTile);
                oBlock.Tiles.Add(o1stTile);
                oBlock.Type = Enums.Mentsu.Koutsu;

                oHandManager.RemoveNumInstancesTileOf(oTempTilesList, o1stTile, 3);

                List<List<Block>> oSubBlockCombinations = GetBlockCombinationsWithNoPair(oTempTilesList);
                if (oSubBlockCombinations != null)
                {
                    foreach (List<Block> oListBlock in oSubBlockCombinations)
                    {
                        oListBlock.Insert(0, oBlock);
                        oBlockCombinations.Insert(0, oListBlock);
                    }

                }
            }
            if (oHandManager.CanBeStartOfARun(poTilesList, o1stTile))
            {
                List<Tile> oTempTilesList = oHandManager.Clone(poTilesList);

                Tile o2ndTile = oHandManager.GetNextIncreasingTileInTheSameSuit(poTilesList, o1stTile);
                Tile o3rdTile = oHandManager.GetNextIncreasingTileInTheSameSuit(poTilesList, o2ndTile);

                Block oBlock = new Block();
                oBlock.Tiles.Add(o1stTile);
                oBlock.Tiles.Add(o2ndTile);
                oBlock.Tiles.Add(o3rdTile);
                oBlock.Type = Enums.Mentsu.Shuntsu;
                oHandManager.RemoveNumInstancesTileOf(oTempTilesList, o1stTile, 1);
                oHandManager.RemoveNumInstancesTileOf(oTempTilesList, o2ndTile, 1);
                oHandManager.RemoveNumInstancesTileOf(oTempTilesList, o3rdTile, 1);

                List<List<Block>> oSubBlockCombinations = GetBlockCombinationsWithNoPair(oTempTilesList);
                if (oSubBlockCombinations != null)
                {
                    foreach (List<Block> oListBlock in oSubBlockCombinations)
                    {
                        oListBlock.Insert(0, oBlock);
                        oBlockCombinations.Insert(0, oListBlock);
                    }
                }
            }

            if (oBlockCombinations.Count == 0)
            {
                return null;
            }

            return oBlockCombinations;
        }

        public List<Block> GetListOfPossiblePairBlocks(List<Tile> poTilesList)
        {
            List<Block> oBlockList = new List<Block>();
            CTilesManager oHandManager = new CTilesManager();

            Tile oCurrentTile = null;
            for (int i = 0; i < poTilesList.Count; i++)
            {
                if (oCurrentTile != null && oCurrentTile.CompareTo(poTilesList[i]) == 0)
                {
                    continue;
                }

                oCurrentTile = poTilesList[i];

                if (oHandManager.CountNumberOfTilesOf(poTilesList, oCurrentTile) >= 2)
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
