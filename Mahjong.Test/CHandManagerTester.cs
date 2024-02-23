using Mahjong.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Mahjong.Test
{
    [TestClass]
    public class CHandManagerTester
    {

        private CHandManager _SUT;

        [TestInitialize]
        public void SetUp()
        {
            _SUT = new CHandManager();
        }

        #region SortTiles
        [DataTestMethod]
        [DataRow("1p4s5m0z3z6s1p")]
        [DataRow("5s4m8p2z1p4s5m0z3z6s1p")]
        public void SortTiles_TilesAreUnSorted_SortsTheTiles(String psMahjongHand)
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(psMahjongHand);

            _SUT.SortTiles(oHand);
            for (int i = 0; i < oHand.Tiles.Count - 1; i++)
            {
                Assert.IsTrue(oHand.Tiles[i].CompareTo(oHand.Tiles[i + 1]) <= 0);
            }
        }
        #endregion

        #region FindFirstIndexOfTile
        [DataTestMethod]
        [DataRow("1p4s5m0z3z6s1p", "4s", 1)]
        [DataRow("5s4m8p2z1p4s5m0z3z6s1p", "1p", 4)]
        public void FindFirstIndexOfTile_TileExistsInHand_ReturnsFirstIndexOfTile(String psMahjongHand, String psTile, int pnExpectedResult)
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(psMahjongHand);
            Tile oTile = new Tile(psTile);
            Assert.AreEqual(pnExpectedResult, _SUT.FindFirstIndexOfTile(oHand, oTile));
        }
        #endregion

        #region GetNextIncreasingTileInTheSameSuit
        [DataTestMethod]
        [DataRow("1278s", "1s", "2s")]
        [DataRow("1278s", "2s", "7s")]
        public void GetNextIncreasingTileInTheSameSuit_TileExistsInHand_ReturnsNextIncreasingTileInSameSuit(String psMahjongHand, String psTile, String psExpectedResult)
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(psMahjongHand);
            Tile oTile = new Tile(psTile);
            Tile oExpectedTile = new Tile(psExpectedResult);
            Assert.AreEqual(oExpectedTile.CompareValue, _SUT.GetNextIncreasingTileInTheSameSuit(oHand, oTile).CompareValue);
        }

        [TestMethod]
        public void GetNextIncreasingTileInTheSameSuit_TileDoesNotExistInHand_ReturnsNull()
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand("123s");
            Tile oTile = new Tile("4s");

            Assert.IsNull(_SUT.GetNextIncreasingTileInTheSameSuit(oHand, oTile));
        }

        [TestMethod]
        public void GetNextIncreasingTileInTheSameSuit_NextIncreasingTileDoesNotExistInHand_ReturnsNull()
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand("123s");
            Tile oTile = new Tile("3s");

            Assert.IsNull(_SUT.GetNextIncreasingTileInTheSameSuit(oHand, oTile));
        }

        #endregion

        #region CanBeStartOfARun
        [DataTestMethod]
        [DataRow("123456789s", "1s")]
        [DataRow("123456789s", "2s")]
        [DataRow("123456789s", "3s")]
        [DataRow("123456789p", "4p")]
        [DataRow("123456789p", "5p")]
        [DataRow("123456789p", "6p")]
        [DataRow("123456789m", "7m")]
        public void CanBeStartOfARun_ValidHandAndTileCanBeStartOfARun_ReturnsTrue(String psMahjongHand, String psTile)
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(psMahjongHand);
            Tile oTile = new Tile(psTile);
            Assert.IsTrue(_SUT.CanBeStartOfARun(oHand, oTile));
        }

        [DataTestMethod]
        [DataRow("111456789s", "1s")]
        [DataRow("126789s", "2m")]
        [DataRow("123456789p111z", "1z")]
        [DataRow("123456789p", "8p")]
        [DataRow("123456789m", "9m")]
        public void CanBeStartOfARun_ValidHandAndTileCanNotBeStartOfARun_ReturnsFalse(String psMahjongHand, String psTile)
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(psMahjongHand);
            Tile oTile = new Tile(psTile);
            Assert.IsFalse(_SUT.CanBeStartOfARun(oHand, oTile));
        }
        #endregion

        #region RemoveSingleTileOf
        [DataTestMethod]
        [DataRow("123456789s", "1s", 0)]
        [DataRow("113456789s", "1s", 1)]
        [DataRow("111456789s", "1s", 2)]
        public void RemoveSingleTileOf_ValidHandAndTile_RemoveTileFromHand(String psMahjongHand, String psTile, int pnExpectedCountOfTileAfterRemoval)
        {
            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(psMahjongHand);
            Tile oTile = new Tile(psTile);
            _SUT.RemoveSingleTileOf(oHand, oTile);

            Assert.AreEqual(pnExpectedCountOfTileAfterRemoval, oHand.Tiles.Where(s => s != null && s.CompareTo(oTile) == 0).Count());
        }
        #endregion

    }
}
