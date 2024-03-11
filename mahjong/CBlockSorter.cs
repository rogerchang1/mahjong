using Mahjong.Exceptions;
using Mahjong.Model;
using System;
using System.Collections.Generic;
using static Mahjong.Enums;

namespace Mahjong
{
    /// <summary>
    /// This doesn't take into account of chiitoi and kokushi as they are special and they can only have a specific block combination
    /// </summary>
    public class CBlockSorter
    {
        private CTilesManager _TilesManager;
        private CBlockParser _BlockParser;
        public CBlockSorter()
        {
            _TilesManager = new CTilesManager();
            _BlockParser = new CBlockParser();
        }


        public List<List<Block>> GetBlockCombinations(Hand poHand)
        {
            CShantenEvaluator oShantenEvaluator = new CShantenEvaluator();
            if (oShantenEvaluator.EvaluateShanten(poHand.Tiles) != -1)
            {
                throw new BlockSortException();
            }

            List<Tile> oTileList = _TilesManager.Clone(poHand.Tiles);

            List<Block> oLockedBlockList = new List<Block>();

            //Remove the tiles from the locked blocks from the hand so we can just figure out the remaining tiles for the possible combinations
            for (int i = 0; i < poHand.LockedBlocks.Count; i++)
            {
                Block oBlock = poHand.LockedBlocks[i];
                for (int j = 0; j < oBlock.Tiles.Count; j++)
                {
                    _TilesManager.RemoveSingleTileOf(oTileList, oBlock.Tiles[j]);
                }
                oLockedBlockList.Add(oBlock);
            }
            List<List<Block>> oBlockCombinations = GetBlockCombinationsFromTileList(oTileList);

            if(oLockedBlockList.Count > 0)
            {
                foreach(List<Block> oBlockCombination in oBlockCombinations)
                {
                    foreach (Block oLockedBlock in oLockedBlockList)
                    {
                        oBlockCombination.Add(oLockedBlock);
                    }
                }
            }
            return oBlockCombinations;
        }

        //TODO sigh rename this function better to differentiate. This one doesn't take into account of the locked blocks.
        public List<List<Block>> GetBlockCombinationsFromTileList(List<Tile> poTilesList)
        {
            List<List<Block>> oBlockCombinations = new List<List<Block>>();            

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
