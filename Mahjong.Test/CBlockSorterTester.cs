using Mahjong.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Mahjong.Test
{
    [TestClass]
    public class CBlockSorterTester
    {

        private CBlockSorter _SUT;
        private CHandParser _HandParser;
        private CBlockParser _BlockParser;

        [TestInitialize]
        public void SetUp()
        {
            _SUT = new CBlockSorter();
            _HandParser = new CHandParser();
            _BlockParser = new CBlockParser();
        }

        #region GetBlockCombinationsWithHand
        [DataTestMethod]
        [DataRow("123p456s11345789m", "789m")]
        public void GetBlockCombinations_ValidHandWith1LockedBlock_ReturnsAListOfBlockCombinations(String psMahjongHand, String psLockedBlock)
        {
            Hand oHand = new Hand();
            oHand.Tiles = _HandParser.ParseTileStringToTileList(psMahjongHand);
            oHand.LockedBlocks.Add(_BlockParser.ParseBlock(psLockedBlock));

            List<List<Block>> oActualResults = _SUT.GetBlockCombinations(oHand);
            Block oBlock1 = _BlockParser.ParseBlock("123p");
            Block oBlock2 = _BlockParser.ParseBlock("456s");
            Block oBlock3 = _BlockParser.ParseBlock("345m");
            Block oBlock4 = _BlockParser.ParseBlock("11m");
            Block oBlock5 = _BlockParser.ParseBlock("789m");

            List<Block> oExpectedBlockCombination = new List<Block>();
            oExpectedBlockCombination.Add(oBlock1);
            oExpectedBlockCombination.Add(oBlock2);
            oExpectedBlockCombination.Add(oBlock3);
            oExpectedBlockCombination.Add(oBlock4);
            oExpectedBlockCombination.Add(oBlock5);

            List<List<Block>> oExpectedBlockCombinations = new List<List<Block>>();
            oExpectedBlockCombinations.Add(oExpectedBlockCombination);

            Assert.AreEqual(oExpectedBlockCombinations.Count, oActualResults.Count);

            for (int i = 0; i < oActualResults.Count; i++)
            {
                List<Block> oActualResult = oActualResults[i];
                Assert.AreEqual(oExpectedBlockCombinations[i].Count, oActualResult.Count);
                for (int j = 0; j < oActualResult.Count; j++)
                {
                    Block oActualBlock = oActualResult[j];
                    Block oExpectedBlock = oExpectedBlockCombinations[i][j];
                    for (int k = 0; k < oActualBlock.Tiles.Count; k++)
                    {
                        Assert.AreEqual(oExpectedBlock.Tiles[k].CompareValue, oActualBlock.Tiles[k].CompareValue);
                    }
                }
            }
        }
        #endregion

        #region GetBlockCombinationsTileList
        [DataTestMethod]
        [DataRow("456p12333456s789m")]
        public void GetBlockCombinations_ValidHand_ReturnsAListOfBlockCombinations(String psMahjongHand)
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);
            List<List<Block>> oActualResults = _SUT.GetBlockCombinationsFromTileList(oTilesList);


            Block oBlock1 = _BlockParser.ParseBlock("456p");
            Block oBlock2 = _BlockParser.ParseBlock("123s");
            Block oBlock3 = _BlockParser.ParseBlock("456s");
            Block oBlock4 = _BlockParser.ParseBlock("789m");
            Block oBlock5 = _BlockParser.ParseBlock("33s");

            List<Block> oExpectedBlockCombination = new List<Block>();
            oExpectedBlockCombination.Add(oBlock1);
            oExpectedBlockCombination.Add(oBlock2);
            oExpectedBlockCombination.Add(oBlock3);
            oExpectedBlockCombination.Add(oBlock4);
            oExpectedBlockCombination.Add(oBlock5);

            List<List<Block>> oExpectedBlockCombinations = new List<List<Block>>();
            oExpectedBlockCombinations.Add(oExpectedBlockCombination);

            Assert.AreEqual(oExpectedBlockCombinations.Count, oActualResults.Count);

            for (int i = 0; i < oActualResults.Count; i++)
            {
                List<Block> oActualResult = oActualResults[i];
                Assert.AreEqual(oExpectedBlockCombinations[i].Count, oActualResult.Count);
                for (int j = 0; j < oActualResult.Count; j++)
                {
                    Block oActualBlock = oActualResult[j];
                    Block oExpectedBlock = oExpectedBlockCombinations[i][j];
                    for (int k = 0; k < oActualBlock.Tiles.Count; k++)
                    {
                        Assert.AreEqual(oExpectedBlock.Tiles[k].CompareValue, oActualBlock.Tiles[k].CompareValue);
                    }
                }
            }

        }
        #endregion

        #region GetListOfPossiblePairs
        [TestMethod]
        public void GetListOfPossiblePairs_HandHasOnePair_ReturnsListWithOneBlock()
        {
            String psMahjongHand = "456s12333456p789m";

            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);
            List<Block> oActualResult = _SUT.GetListOfPossiblePairBlocks(oTilesList);

            CBlockParser oBlockParser = new CBlockParser();

            for (int i = 0; i < oActualResult.Count; i++)
            {
                String sActualBlockResult = oBlockParser.ToString(oActualResult[i]);
                Assert.AreEqual("3p3p", sActualBlockResult);
            }

        }

        [TestMethod]
        public void GetListOfPossiblePairs_HandHasTwoPairs_ReturnsListWithTwoBlocks()
        {
            String psMahjongHand = "444567s12333456p";

            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);
            List<Block> oActualResult = _SUT.GetListOfPossiblePairBlocks(oTilesList);

            CBlockParser oBlockParser = new CBlockParser();

            Assert.AreEqual("4s4s", oBlockParser.ToString(oActualResult[0]));
            Assert.AreEqual("3p3p", oBlockParser.ToString(oActualResult[1]));
        }
        #endregion


        #region GetBlockCombinationsWithNoPair
        [TestMethod]
        public void GetBlockCombinationsWithNoPair_ValidHandWithNoPair_ReturnsBlockCombinations()
        {
            String psMahjongHand = "456p123456s789m";

            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);
            List<List<Block>> oActualResults = _SUT.GetBlockCombinationsWithNoPair(oTilesList);

            Block oBlock1 = _BlockParser.ParseBlock("456p");
            Block oBlock2 = _BlockParser.ParseBlock("123s");
            Block oBlock3 = _BlockParser.ParseBlock("456s");
            Block oBlock4 = _BlockParser.ParseBlock("789m");

            List<Block> oExpectedBlockCombination = new List<Block>();
            oExpectedBlockCombination.Add(oBlock1);
            oExpectedBlockCombination.Add(oBlock2);
            oExpectedBlockCombination.Add(oBlock3);
            oExpectedBlockCombination.Add(oBlock4);

            List<List<Block>> oExpectedBlockCombinations = new List<List<Block>>();
            oExpectedBlockCombinations.Add(oExpectedBlockCombination);

            Assert.AreEqual(oExpectedBlockCombinations.Count, oActualResults.Count);

            for (int i = 0; i < oActualResults.Count; i++)
            {
                List<Block> oActualResult = oActualResults[i];
                Assert.AreEqual(oExpectedBlockCombinations[i].Count, oActualResult.Count);
                for (int j = 0; j < oActualResult.Count; j++)
                {
                    Block oActualBlock = oActualResult[j];
                    Block oExpectedBlock = oExpectedBlockCombinations[i][j];
                    for (int k = 0; k < oActualBlock.Tiles.Count; k++)
                    {
                        Assert.AreEqual(oExpectedBlock.Tiles[k].CompareValue, oActualBlock.Tiles[k].CompareValue);
                    }
                }
            }

        }
        #endregion

    }
}
