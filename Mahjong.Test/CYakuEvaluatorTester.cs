using Mahjong.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Mahjong.Test
{
    [TestClass]
    public class CYakuEvaluatorTester
    {

        private CYakuEvaluator _SUT;
        private CHandParser _HandParser;
        private CBlockParser _BlockParser;

        [TestInitialize]
        public void SetUp()
        {
            _SUT = new CYakuEvaluator();
            _HandParser = new CHandParser();
            _BlockParser = new CBlockParser();
        }

        [TestMethod]
        public void IsTanyao_HandIsInTenpaiAndIsTanyao_ReturnTrue()
        {
            List<Block> oBlockCombination = new List<Block>() {
                _BlockParser.ParseBlock("456p"),
                _BlockParser.ParseBlock("233s"),
                _BlockParser.ParseBlock("456s"),
                _BlockParser.ParseBlock("678m"),
                _BlockParser.ParseBlock("33s")
            };

            Assert.IsTrue(_SUT.IsTanyao(oBlockCombination));
        }

        [TestMethod]
        public void IsTanyao_HandIsInTenpaiAndIsNotTanyao_ReturnFalse()
        {
            Block oBlock1 = _BlockParser.ParseBlock("456p");
            Block oBlock2 = _BlockParser.ParseBlock("233s");
            Block oBlock3 = _BlockParser.ParseBlock("456s");
            Block oBlock4 = _BlockParser.ParseBlock("678m");
            Block oBlock5 = _BlockParser.ParseBlock("33z");

            List<Block> oBlockCombination = new List<Block>();
            oBlockCombination.Add(oBlock1);
            oBlockCombination.Add(oBlock2);
            oBlockCombination.Add(oBlock3);
            oBlockCombination.Add(oBlock4);
            oBlockCombination.Add(oBlock5);

            Assert.IsFalse(_SUT.IsTanyao(oBlockCombination));
        }

        [TestMethod]
        public void IsRyuuiisou_HandIsIsRyuuiisou_ReturnTrue()
        {
            Hand oHand = _HandParser.ParseHand("22334466888s66z");
            Assert.IsTrue(_SUT.IsRyuuiisou(oHand, new List<Block>()));
        }

        [TestMethod]
        public void IsRyuuiisou_HandIsIsNotRyuuiisou_ReturnFalse()
        {
            Hand oHand = _HandParser.ParseHand("23344566888s66z");
            Assert.IsFalse(_SUT.IsRyuuiisou(oHand, new List<Block>()));
        }
    }
}
