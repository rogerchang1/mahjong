using Mahjong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static Mahjong.Enums;

namespace Mahjong
{
    /// <summary>
    /// Utility class for managing a list of tiles
    /// Perform counting, removal, sorting, searching, cloning.
    /// </summary>
    public class CTilesManager
    {

        public CTilesManager()
        {

        }

        public void SortTiles(List<Tile> poTileList)
        {
            poTileList.Sort(delegate (Tile t1, Tile t2) { return t1.CompareTo(t2); });
        }

        /// <summary>
        /// Best used in conjunction with SortTiles() for finding sequential tiles.
        /// </summary>
        /// <param name="poTileList"></param>
        /// <param name="poTile"></param>
        /// <returns></returns>
        public int FindFirstIndexOfTile(List<Tile> poTileList, Tile poTile)
        {
            for (int i = 0; i < poTileList.Count; i++)
            {
                if (poTile.CompareTo(poTileList[i]) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the next tile in the hand of the same suit. 
        /// For example, GetNextIncreasingTileInTheSameSuit("1278s",1s") = 2s, GetNextIncreasingTileInTheSameSuit("1278s",2s") = 7s, GetNextIncreasingTileInTheSameSuit("1278s",8s") = null
        /// </summary>
        /// <param name="poTileList"></param>
        /// <param name="poTile"></param>
        /// <returns></returns>
        public Tile GetNextIncreasingTileInTheSameSuit(List<Tile> poTileList, Tile poTile)
        {
            if (poTile == null)
            {
                return null;
            }

            SortTiles(poTileList);

            int index = FindFirstIndexOfTile(poTileList, poTile);
            if (index == -1)
            {
                return null;
            }
            for (int i = index + 1; i < poTileList.Count; i++)
            {
                if (poTileList[i].num != poTileList[index].num && poTileList[i].suit == poTileList[index].suit)
                {
                    return poTileList[i];
                }
            }
            return null;
        }


        public int CountNumberOfTilesOf(List<Tile> poTileList, Tile poTile)
        {
            if (poTile == null)
            {
                return 0;
            }

            int count = 0;

            for (int i = 0; i < poTileList.Count; i++)
            {
                if (poTileList[i].CompareTo(poTile) == 0)
                {
                    count++;
                }
            }

            return count;
        }

        public int CountNumberOfTilesOf(List<Tile> poTileList, String psTile)
        {
            if (psTile == "")
            {
                return 0;
            }

            Tile oTile = new Tile(psTile);

            int count = 0;

            for (int i = 0; i < poTileList.Count; i++)
            {
                if (poTileList[i].CompareTo(oTile) == 0)
                {
                    count++;
                }
            }

            return count;
        }

        public Boolean CanBeStartOfARun(List<Tile> poTileList, Tile poTile)
        {
            if (poTile == null)
            {
                return false;
            }

            if (poTile.suit == "z")
            {
                return false;
            }

            SortTiles(poTileList);

            int index = FindFirstIndexOfTile(poTileList, poTile);

            if (index == -1)
            {
                return false;
            }

            int divisor = 1;

            switch (poTile.suit)
            {
                case "s":
                    divisor = 10;
                    break;
                case "m":
                    divisor = 100;
                    break;
                case "z":
                    divisor = 1000;
                    break;
                default:
                    break;
            }

            Tile o2ndTile = GetNextIncreasingTileInTheSameSuit(poTileList, poTile);

            if (o2ndTile == null)
            {
                return false;
            }

            if (o2ndTile.CompareTo(poTile) / divisor != 1)
            {
                return false;
            }

            Tile o3rdTile = GetNextIncreasingTileInTheSameSuit(poTileList, o2ndTile);

            if (o3rdTile == null)
            {
                return false;
            }

            if (o3rdTile.CompareTo(o2ndTile) / divisor != 1)
            {
                return false;
            }

            return true;

        }

        public void RemoveSingleTileOf(List<Tile> poTileList, Tile poTileToRemove)
        {
            RemoveNumInstancesTileOf(poTileList, poTileToRemove, 1);
        }

        public void RemoveNumInstancesTileOf(List<Tile> poTileList, Tile poTileToRemove, int nNumTimesToRemove)
        {
            int count = 0;
            for (int i = 0; i < poTileList.Count; i++)
            {
                if (poTileToRemove.CompareTo(poTileList[i]) == 0)
                {
                    poTileList.RemoveAt(i);
                    i--;
                    count++;
                    if (count >= nNumTimesToRemove)
                    {
                        break;
                    }
                }
            }
        }

        public void RemoveAllTilesOf(List<Tile> poTileList, Tile poTileToRemove)
        {
            for (int i = 0; i < poTileList.Count; i++)
            {
                if (poTileToRemove.CompareTo(poTileList[i]) == 0)
                {
                    poTileList.RemoveAt(i);
                    i--;
                }
            }
        }

        public List<Suit> GetSuitsFromTileList(List<Tile> poTileList)
        {
            List<Suit> oSuitList = new List<Suit>();
            for(int i = 0; i < poTileList.Count; i++)
            {
                Suit oSuit = TileSuitToEnum(poTileList[i]);
                if (!oSuitList.Contains(oSuit))
                {
                    oSuitList.Add(oSuit);
                }
            }
            return oSuitList;
        }

        public Boolean IsHonorTile(Tile poTile)
        {
            return poTile.suit == "z";
        }

        public Boolean IsWindTile(Tile poTile)
        {
            return (poTile.suit == "z" && poTile.num >= 1 && poTile.num <= 4);
        }

        public Boolean IsDragonTile(Tile poTile)
        {
            return (poTile.suit == "z" && poTile.num >= 5 && poTile.num <= 7);
        }

        public List<Tile> Clone(List<Tile> poTileList)
        {
            List<Tile> oHand = new List<Tile>();
            oHand = poTileList.ToList();
            return oHand;
        }
    }
}
