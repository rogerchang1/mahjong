using Mahjong.Exceptions;
using Mahjong.Model;
using System;
using System.Collections.Generic;
using static Mahjong.Enums;

namespace Mahjong
{
    public class CBlockSorter
    {
        private CTilesManager _TilesManager;
        private CBlockParser _BlockParser;
        public CBlockSorter()
        {
            _TilesManager = new CTilesManager();
            _BlockParser = new CBlockParser();
        }

        //TODO:
        //1. Kan
        //2. Open Hands
        //3. Kokushi Musou
        public List<List<Block>> GetBlockCombinations(List<Tile> pTilesList)
        {
            List<List<Block>> oBlockCombinations = new List<List<Block>>();
            CShantenEvaluator oShantenEvaluator = new CShantenEvaluator();
            CTilesManager _TilesManager = new CTilesManager();
            if (oShantenEvaluator.EvaluateShanten(pTilesList) != -1)
            {
                throw new BlockSortException();
            }

            List<Tile> poTilesList = _TilesManager.Clone(pTilesList); //Eventually should rename poTilesList to oTempTilesList or something to remove the open blocks from it before processin.
            //Should remove open blocks from the hand before processing it over here or something.

            //poTilesList.SortTiles();
            _TilesManager.SortTiles(poTilesList);

            List<Block> oPossiblePairs = GetListOfPossiblePairBlocks(poTilesList);

            foreach (Block oPair in oPossiblePairs)
            {
                List<Tile> oTempTilesList = _TilesManager.Clone(poTilesList);

                _TilesManager.RemoveNumInstancesTileOf(oTempTilesList, oPair.Tiles[0], 2);

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
            List<List<Block>> oBlockCombinations = new List<List<Block>>();

            _TilesManager.SortTiles(poTilesList);

            //Recursive stopping point
            if (poTilesList.Count == 3)
            {
                int nNumberOfSameTiles = _TilesManager.CountNumberOfTilesOf(poTilesList, poTilesList[0]);
                Boolean bIsStartOfAValidRun = _TilesManager.CanBeStartOfARun(poTilesList, poTilesList[0]);

                if (nNumberOfSameTiles != 3 && !bIsStartOfAValidRun)
                {
                    return null;
                }

                Block oBlock = new Block();
                oBlock.Tiles = poTilesList;
                oBlock.Type = bIsStartOfAValidRun ? Mentsu.Shuntsu : Mentsu.Koutsu;

                List<Block> oBlockCombination = new List<Block>();
                oBlockCombination.Insert(0, oBlock);

                oBlockCombinations.Insert(0, oBlockCombination);
                return oBlockCombinations;
            }

            Tile o1stTile = poTilesList[0];

            //Recursion with triplet
            if (_TilesManager.CountNumberOfTilesOf(poTilesList, o1stTile) == 3)
            {
                List<Tile> oTempTilesList = _TilesManager.Clone(poTilesList);

                Block oBlock = _BlockParser.ParseBlock($"{o1stTile.ToString()}{o1stTile.ToString()}{o1stTile.ToString()}", Mentsu.Koutsu);

                _TilesManager.RemoveNumInstancesTileOf(oTempTilesList, o1stTile, 3);

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

            //Recursion with sequence
            if (_TilesManager.CanBeStartOfARun(poTilesList, o1stTile))
            {
                List<Tile> oTempTilesList = _TilesManager.Clone(poTilesList);

                Tile o2ndTile = _TilesManager.GetNextIncreasingTileInTheSameSuit(poTilesList, o1stTile);
                Tile o3rdTile = _TilesManager.GetNextIncreasingTileInTheSameSuit(poTilesList, o2ndTile);

                Block oBlock = _BlockParser.ParseBlock($"{o1stTile.ToString()}{o2ndTile.ToString()}{o3rdTile.ToString()}", Mentsu.Shuntsu);
                _TilesManager.RemoveNumInstancesTileOf(oTempTilesList, o1stTile, 1);
                _TilesManager.RemoveNumInstancesTileOf(oTempTilesList, o2ndTile, 1);
                _TilesManager.RemoveNumInstancesTileOf(oTempTilesList, o3rdTile, 1);

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

            Tile oCurrentTile = null;
            for (int i = 0; i < poTilesList.Count; i++)
            {
                if (oCurrentTile != null && oCurrentTile.CompareTo(poTilesList[i]) == 0)
                {
                    continue;
                }

                oCurrentTile = poTilesList[i];

                if (_TilesManager.CountNumberOfTilesOf(poTilesList, oCurrentTile) >= 2)
                {
                    Block oBlock = _BlockParser.ParseBlock($"{oCurrentTile.ToString()}{oCurrentTile.ToString()}", Mentsu.Jantou);
                    oBlockList.Add(oBlock);
                }
            }
            return oBlockList;
        }
    }
}
