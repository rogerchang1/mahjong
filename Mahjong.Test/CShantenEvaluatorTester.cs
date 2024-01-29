using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mahjong;
using Mahjong.Model;
using System;

namespace Mahjong.Test
{
    [TestClass]
    public class CShantenEvaluatorTester
    {

        private CShantenEvaluator _SUT;
        
        [DataTestMethod]
        [DataRow("111133p445566s9m",1)]
        [DataRow("46s11222333p789m", 0)]
        [DataRow("46s11222333p112z", 1)]
        [DataRow("11123p2378s2378m", 2)]
        public void EvaluateShanten_ReceiveValidAHandStringArgument_GivesMinimumShanten(String psMahjongHand, int pnExpectedResult)
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.parseHand(psMahjongHand);
            oHand.SortTiles();

            CShantenEvaluator shantenEvaluator = new CShantenEvaluator();
            int shanten = shantenEvaluator.EvaluateShanten(oHand);
            Assert.AreEqual(pnExpectedResult, shanten);
        }
    }
}
