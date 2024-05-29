using Mahjong.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Mahjong.Test
{
    [TestClass]
    public class CShantenEvaluatorTester
    {

        private CShantenEvaluator _SUT;
        private CHandParser _HandParser;
        private CBlockParser _BlockParser;

        [TestInitialize]
        public void SetUp()
        {
            _SUT = new CShantenEvaluator();
            _HandParser = new CHandParser();
            _BlockParser = new CBlockParser();
        }

        [DataTestMethod]
        [DataRow("555s555678p222z66z", -1)]
        [DataRow("555s567888p222z66z", -1)]
        [DataRow("555s566667p222z66z", -1)]
        [DataRow("147p258s369m1234z", 6)] //chiitoi will never make shanten go above 6
        [DataRow("147p258s369m1134z", 5)]
        [DataRow("147p258s369m1555z", 5)]
        [DataRow("147p258s123789m6z", 4)]
        [DataRow("147p258s1345789m", 4)]
        [DataRow("145p258s1345789m", 3)]
        [DataRow("3466788p259s689m5z", 3)]
        [DataRow("11123p2378s2378m", 2)]
        [DataRow("111133p445566s9m", 1)]
        [DataRow("46s11222333p112z", 1)]
        [DataRow("46s11222333p789m", 0)]
        [DataRow("24567p33355678s", 0)] //24567 pattern
        [DataRow("2455567p333678s", 0)] //24567 pattern
        [DataRow("24556677p33678s", 0)] //24567 pattern
        [DataRow("24455667p33678s", 0)] //24567 pattern
        [DataRow("45679p33355678s", 0)]
        [DataRow("456s11222333p789m", -1)]
        [DataRow("111333p445566s99m", -1)]
        [DataRow("115588m147p2588s", 2)] //chiitoi
        [DataRow("112233m668899p4z", 0)] //chiitoi
        [DataRow("19p19s15m1234567z", 1)] //kokushi
        [DataRow("19p19s19m1234567z", 0)] //kokushi
        [DataRow("19p19s1m12344567z", 0)] //kokushi
        public void EvaluateShanten_ReceiveValidATileListArgument_GivesMinimumShanten(String psMahjongHand, int pnExpectedResult)
        {
            List<Tile> oTilesList = _HandParser.ParseTileStringToTileList(psMahjongHand);

            int shanten = _SUT.EvaluateShanten(oTilesList);
            Assert.AreEqual(pnExpectedResult, shanten);
        }

        [DataTestMethod]
        [DataRow("123p458s1345789m","789m", 1)]
        public void EvaluateShanten_ReceiveValidAHandArgumentWith1LockedBlocks_GivesMinimumShanten(String psMahjongHand, String psLockedBlock, int pnExpectedResult)
        {
            Hand oHand = new Hand();
            oHand.Tiles = _HandParser.ParseTileStringToTileList(psMahjongHand);
            oHand.LockedBlocks.Add(_BlockParser.ParseBlock(psLockedBlock));

            int shanten = _SUT.EvaluateShanten(oHand);
            Assert.AreEqual(pnExpectedResult, shanten);
        }
    }
}
