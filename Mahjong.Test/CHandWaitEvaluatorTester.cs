using Mahjong.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Mahjong.Test
{
    [TestClass]
    public class CHandWaitEvaluatorTester
    {

        private CHandWaitEvaluator _SUT;
        private CHandParser _HandParser;

        [TestInitialize]
        public void SetUp()
        {
            _SUT = new CHandWaitEvaluator();
            _HandParser = new CHandParser();
        }

        [TestMethod]
        public void EvaluateWaits_HandIs24455667p33678s_Returns3p()
        {
            Hand oHand = _HandParser.ParseHand("24455667p33678s");

            List<Tile> oActualResult = _SUT.EvaluateWaits(oHand);
            List<Tile> oExpectedResult = new List<Tile>() { new Tile("3p") };
            foreach (Tile oTile in oActualResult) {
                Assert.IsTrue(oActualResult.Contains(oTile));
            }

        }

        [TestMethod]
        public void EvaluateWaits_HandIs33456789p23456s_Returns147s()
        {
            Hand oHand = _HandParser.ParseHand("33456789p23456s");

            List<Tile> oActualResult = _SUT.EvaluateWaits(oHand);
            List<Tile> oExpectedResult = new List<Tile>() { new Tile("1s"), new Tile("4s"), new Tile("7s") };
            foreach (Tile oTile in oActualResult)
            {
                Assert.IsTrue(oActualResult.Contains(oTile));
            }

        }

    }
}
