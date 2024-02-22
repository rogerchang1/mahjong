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

        [TestInitialize]
        public void SetUp()
        {
            _SUT = new CShantenEvaluator();
        }

        [DataTestMethod]
        [DataRow("147p258s369m1234z", 6)] //chiitoi will never make shanten go above 6
        [DataRow("147p258s369m1134z", 5)]
        [DataRow("147p258s369m1555z", 5)]
        [DataRow("147p258s123789m6z", 4)]
        [DataRow("147p258s1345789m", 4)]
        [DataRow("145p258s1345789m", 3)]
        [DataRow("11123p2378s2378m", 2)]
        [DataRow("111133p445566s9m", 1)]
        [DataRow("46s11222333p112z", 1)]
        [DataRow("46s11222333p789m", 0)]
        [DataRow("456s11222333p789m", -1)]
        [DataRow("111333p445566s99m", -1)]
        [DataRow("115588m147p2588s", 2)]
        [DataRow("112233m668899p4z", 0)]
        public void EvaluateShanten_ReceiveValidAHandStringArgument_GivesMinimumShanten(String psMahjongHand, int pnExpectedResult)
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(psMahjongHand);

            int shanten = _SUT.EvaluateShanten(oHand);
            Assert.AreEqual(pnExpectedResult, shanten);
        }
    }
}
