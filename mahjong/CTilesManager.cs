﻿using Mahjong.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahjong
{
    public class CTilesManager
    {

        public CTilesManager()
        {

        }

        public void SortTiles(List<Tile> poTilesList)
        {
            poTilesList.Sort(delegate (Tile t1, Tile t2) { return t1.CompareTo(t2); });
        }

        /// <summary>
        /// Best used in conjunction with SortTiles() for finding sequential tiles.
        /// </summary>
        /// <param name="poTilesList"></param>
        /// <param name="poTile"></param>
        /// <returns></returns>
        public int FindFirstIndexOfTile(List<Tile> poTilesList, Tile poTile)
        {
            for (int i = 0; i < poTilesList.Count; i++)
            {
                if (poTile.CompareTo(poTilesList[i]) == 0)
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
        /// <param name="poTilesList"></param>
        /// <param name="poTile"></param>
        /// <returns></returns>
        public Tile GetNextIncreasingTileInTheSameSuit(List<Tile> poTilesList, Tile poTile)
        {
            if (poTile == null)
            {
                return null;
            }

            SortTiles(poTilesList);

            int index = FindFirstIndexOfTile(poTilesList, poTile);
            if (index == -1)
            {
                return null;
            }
            for (int i = index + 1; i < poTilesList.Count; i++)
            {
                if (poTilesList[i].num != poTilesList[index].num && poTilesList[i].suit == poTilesList[index].suit)
                {
                    return poTilesList[i];
                }
            }
            return null;
        }


        public int CountNumberOfTilesOf(List<Tile> poTilesList, Tile poTile)
        {
            if (poTile == null)
            {
                return 0;
            }

            int count = 0;

            for (int i = 0; i < poTilesList.Count; i++)
            {
                if (poTilesList[i].CompareTo(poTile) == 0)
                {
                    count++;
                }
            }

            return count;
        }

        public Boolean CanBeStartOfARun(List<Tile> poTilesList, Tile poTile)
        {
            if (poTile == null)
            {
                return false;
            }

            if (poTile.suit == "z")
            {
                return false;
            }

            SortTiles(poTilesList);

            int index = FindFirstIndexOfTile(poTilesList, poTile);

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

            Tile o2ndTile = GetNextIncreasingTileInTheSameSuit(poTilesList, poTile);

            if (o2ndTile == null)
            {
                return false;
            }

            if (o2ndTile.CompareTo(poTile) / divisor != 1)
            {
                return false;
            }

            Tile o3rdTile = GetNextIncreasingTileInTheSameSuit(poTilesList, o2ndTile);

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

        public void RemoveSingleTileOf(List<Tile> poTilesList, Tile poTileToRemove)
        {
            RemoveNumInstancesTileOf(poTilesList, poTileToRemove, 1);
        }

        public void RemoveNumInstancesTileOf(List<Tile> poTilesList, Tile poTileToRemove, int nNumTimesToRemove)
        {
            int count = 0;
            for (int i = 0; i < poTilesList.Count; i++)
            {
                if (poTileToRemove.CompareTo(poTilesList[i]) == 0)
                {
                    poTilesList.RemoveAt(i);
                    i--;
                    count++;
                    if (count >= nNumTimesToRemove)
                    {
                        break;
                    }
                }
            }
        }

        public void RemoveAllTilesOf(List<Tile> poTilesList, Tile poTileToRemove)
        {
            for (int i = 0; i < poTilesList.Count; i++)
            {
                if (poTileToRemove.CompareTo(poTilesList[i]) == 0)
                {
                    poTilesList.RemoveAt(i);
                    i--;
                }
            }
        }

        public List<Tile> Clone(List<Tile> poTilesList)
        {
            List<Tile> oHand = new List<Tile>();
            oHand = poTilesList.ToList();
            return oHand;
        }
    }
}