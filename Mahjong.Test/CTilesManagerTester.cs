using Mahjong.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahjong.Test
{
    [TestClass]
    public class CTilesManagerTester
    {

        private CTilesManager _SUT;

        [TestInitialize]
        public void SetUp()
        {
            _SUT = new CTilesManager();
        }

        #region SortTiles
        [TestMethod]
        public void SortTiles_TileListArgumentIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _SUT.SortTiles(null));
        }

        [DataTestMethod]
        [DataRow("1p4s5m0z3z6s1p")]
        [DataRow("5s4m8p2z1p4s5m0z3z6s1p")]
        public void SortTiles_TilesAreUnSorted_SortsTheTiles(String psMahjongHand)
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);

            _SUT.SortTiles(oTilesList);
            for (int i = 0; i < oTilesList.Count - 1; i++)
            {
                Assert.IsTrue(oTilesList[i].CompareTo(oTilesList[i + 1]) <= 0);
            }
        }
        #endregion

        #region FindFirstIndexOfTile
        [TestMethod]
        public void FindFirstIndexOfTile_TileListArgumentIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _SUT.FindFirstIndexOfTile(null,null));
        }

        [DataTestMethod]
        [DataRow("1p4s5m0z3z6s1p", "4s", 1)]
        [DataRow("5s4m8p2z1p4s5m0z3z6s1p", "1p", 4)]
        public void FindFirstIndexOfTile_TileExistsInHand_ReturnsFirstIndexOfTile(String psMahjongHand, String psTile, int pnExpectedResult)
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);
            Tile oTile = new Tile(psTile);
            Assert.AreEqual(pnExpectedResult, _SUT.FindFirstIndexOfTile(oTilesList, oTile));
        }
        #endregion

        #region GetNextIncreasingTileInTheSameSuit
        [TestMethod]
        public void GetNextIncreasingTileInTheSameSuit_TileListArgumentIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _SUT.GetNextIncreasingTileInTheSameSuit(null, null));
        }

        [DataTestMethod]
        [DataRow("1278s", "1s", "2s")]
        [DataRow("1278s", "2s", "7s")]
        public void GetNextIncreasingTileInTheSameSuit_TileExistsInHand_ReturnsNextIncreasingTileInSameSuit(String psMahjongHand, String psTile, String psExpectedResult)
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);
            Tile oTile = new Tile(psTile);
            Tile oExpectedTile = new Tile(psExpectedResult);
            Assert.AreEqual(oExpectedTile.CompareValue, _SUT.GetNextIncreasingTileInTheSameSuit(oTilesList, oTile).CompareValue);
        }

        [TestMethod]
        public void GetNextIncreasingTileInTheSameSuit_TileDoesNotExistInHand_ReturnsNull()
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList("123s");
            Tile oTile = new Tile("4s");

            Assert.IsNull(_SUT.GetNextIncreasingTileInTheSameSuit(oTilesList, oTile));
        }

        [TestMethod]
        public void GetNextIncreasingTileInTheSameSuit_NextIncreasingTileDoesNotExistInHand_ReturnsNull()
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList("123s");
            Tile oTile = new Tile("3s");

            Assert.IsNull(_SUT.GetNextIncreasingTileInTheSameSuit(oTilesList, oTile));
        }

        #endregion

        #region ContainsTileOf
        [TestMethod]
        public void ContainsTileOf_TileListArgumentIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _SUT.ContainsTileOf(null, null));
        }

        [TestMethod]
        public void ContainsTileOf_ValidHandAndTileIsNotInHand_ReturnsFalse()
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList("111456789s");
            Tile oTile = new Tile("1z");
            Assert.IsFalse(_SUT.ContainsTileOf(oTilesList, oTile));
        }

        [TestMethod]
        public void ContainsTileOf_ValidHandAndTileIsInHand_ReturnsTrue()
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList("111456789s");
            Tile oTile = new Tile("1s");
            Assert.IsTrue(_SUT.ContainsTileOf(oTilesList, oTile));
        }

        [TestMethod]
        public void ContainsTileOf_ValidHandAndTileIsNull_ReturnsFalse()
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList("111456789s");
            Assert.IsFalse(_SUT.ContainsTileOf(oTilesList, null));
        }
        #endregion

        #region CountNumberOfTilesOf
        [TestMethod]
        public void CountNumberOfTilesOf_TileListArgumentIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _SUT.CountNumberOfTilesOf(null, ""));
        }

        [DataTestMethod]
        [DataRow("111456789s", "1z", 0)]
        [DataRow("111456789s", "", 0)]
        [DataRow("111456789s", "9s", 1)]
        [DataRow("111456789s", "1s", 3)]
        public void CountNumberOfTilesOf_ValidHandAndTileIsNotInHand_ReturnsFalse(String psMahjongHand, String psTile, int pnExpectedResult)
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);
            Assert.AreEqual(pnExpectedResult,_SUT.CountNumberOfTilesOf(oTilesList, psTile));
        }
        #endregion

        #region CanBeStartOfARun
        [TestMethod]
        public void CanBeStartOfARun_TileListArgumentIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _SUT.CanBeStartOfARun(null, null));
        }

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
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);
            Tile oTile = new Tile(psTile);
            Assert.IsTrue(_SUT.CanBeStartOfARun(oTilesList, oTile));
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
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);
            Tile oTile = new Tile(psTile);
            Assert.IsFalse(_SUT.CanBeStartOfARun(oTilesList, oTile));
        }
        #endregion

        #region RemoveSingleTileOf
        [TestMethod]
        public void RemoveSingleTileOf_TileListArgumentIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _SUT.RemoveSingleTileOf(null, null));
        }

        [DataTestMethod]
        [DataRow("123456789s", "1s", 0)]
        [DataRow("113456789s", "1s", 1)]
        [DataRow("111456789s", "1s", 2)]
        public void RemoveSingleTileOf_ValidHandAndTile_RemoveTileFromHand(String psMahjongHand, String psTile, int pnExpectedCountOfTileAfterRemoval)
        {
            CHandParser oHandParser = new CHandParser();
            List<Tile> oTilesList = oHandParser.ParseTileStringToTileList(psMahjongHand);
            Tile oTile = new Tile(psTile);
            _SUT.RemoveSingleTileOf(oTilesList, oTile);

            Assert.AreEqual(pnExpectedCountOfTileAfterRemoval, oTilesList.Where(s => s != null && s.CompareTo(oTile) == 0).Count());
        }
        #endregion

    }
}
