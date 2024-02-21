using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mahjong;
using Mahjong.Model;
using System;
using System.Collections.Generic;

namespace Mahjong.Test
{
    [TestClass]
    public class CBlockSorterTester
    {

        private CBlockSorter _SUT;

        [TestInitialize]
        public void SetUp()
        {
            _SUT = new CBlockSorter();
        }

        [DataTestMethod]
        [DataRow("456s12333456p789m", -1)]
        public void GetBlockCombinations(String psMahjongHand, int pnExpectedResult)
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(psMahjongHand);
        }

        [TestMethod]
        public void GetListOfPossiblePairs_HandHasOnePair_ReturnsListWithOneBlock()
        {
            String psMahjongHand = "456s12333456p789m";
            
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(psMahjongHand);
            List<Block> oActualResult = _SUT.GetListOfPossiblePairs(oHand);

            CBlockParser oBlockParser = new CBlockParser();

            for(int i = 0; i < oActualResult.Count; i++)
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
            Hand oHand = oHandParser.ParseHand(psMahjongHand);
            List<Block> oActualResult = _SUT.GetListOfPossiblePairs(oHand);

            CBlockParser oBlockParser = new CBlockParser();

            for (int i = 0; i < oActualResult.Count; i++)
            {
                String sActualBlockResult = oBlockParser.ToString(oActualResult[i]);
                Assert.AreEqual("3p3p", sActualBlockResult);
                Assert.AreEqual("4s4s", sActualBlockResult);
            }

        }

        [TestMethod]
        public void GetBlockCombinationsWithNoPair()
        {
            //String psMahjongHand = "444567s123456p";
            String psMahjongHand = "444555666s123p";

            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(psMahjongHand);
            List<List<Block>> oActualResult = _SUT.GetBlockCombinationsWithNoPair(oHand);

            CBlockParser oBlockParser = new CBlockParser();

            for (int i = 0; i < oActualResult.Count; i++)
            {
                //String sActualBlockResult = oBlockParser.ToString(oActualResult[i]);
                
            }

        }
    }
}
