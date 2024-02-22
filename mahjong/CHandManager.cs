using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mahjong.Model;

namespace Mahjong
{
    public class CHandManager
    {

        public CHandManager()
        {
            
        }

        public void SortTiles(Hand poHand)
        {
            poHand.Tiles.Sort(delegate (Tile t1, Tile t2) { return t1.CompareTo(t2); });
        }

        /// <summary>
        /// Best used in conjunction with SortTiles() for finding sequential tiles.
        /// </summary>
        /// <param name="poHand"></param>
        /// <param name="poTile"></param>
        /// <returns></returns>
        public int FindFirstIndexOfTile(Hand poHand, Tile poTile)
        {
            for (int i = 0; i < poHand.Tiles.Count; i++)
            {
                if (poTile.CompareTo(poHand.Tiles[i]) == 0)
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
        /// <param name="poHand"></param>
        /// <param name="poTile"></param>
        /// <returns></returns>
        public Tile GetNextIncreasingTileInTheSameSuit(Hand poHand, Tile poTile)
        {
            if (poTile == null)
            {
                return null;
            }

            SortTiles(poHand);

            int index = FindFirstIndexOfTile(poHand, poTile);
            if (index == -1)
            {
                return null;
            }
            for (int i = index + 1; i < poHand.Tiles.Count; i++)
            {
                if (poHand.Tiles[i].num != poHand.Tiles[index].num && poHand.Tiles[i].suit == poHand.Tiles[index].suit)
                {
                    return poHand.Tiles[i];
                }
            }
            return null;
        }

        
        public int CountNumberOfTilesOf(Hand poHand, Tile poTile)
        {
            if (poTile == null)
            {
                return 0;
            }

            int count = 0;

            for (int i = 0; i < poHand.Tiles.Count; i++)
            {
                if (poHand.Tiles[i].CompareTo(poTile) == 0)
                {
                    count++;
                }
            }

            return count;
        }

        public Boolean CanBeStartOfARun(Hand poHand, Tile poTile)
        {
            if (poTile == null)
            {
                return false;
            }

            if(poTile.suit == "z")
            {
                return false;
            }

            SortTiles(poHand);

            int index = FindFirstIndexOfTile(poHand, poTile);

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

            Tile o2ndTile = GetNextIncreasingTileInTheSameSuit(poHand, poTile);

            if (o2ndTile == null)
            {
                return false;
            }

            if (o2ndTile.CompareTo(poTile) / divisor != 1)
            {
                return false;
            }

            Tile o3rdTile = GetNextIncreasingTileInTheSameSuit(poHand, o2ndTile);

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

        public void RemoveSingleTileOf(Hand poHand, Tile poTileToRemove)
        {
            RemoveNumInstancesTileOf(poHand, poTileToRemove, 1);
        }

        public void RemoveNumInstancesTileOf(Hand poHand, Tile poTileToRemove, int nNumTimesToRemove)
        {
            int count = 0;
            for (int i = 0; i < poHand.Tiles.Count; i++)
            {
                if (poTileToRemove.CompareTo(poHand.Tiles[i]) == 0)
                {
                    poHand.Tiles.RemoveAt(i);
                    i--;
                    count++;
                    if (count >= nNumTimesToRemove)
                    {
                        break;
                    }
                }
            }
        }

        public void RemoveAllTilesOf(Hand poHand, Tile poTileToRemove)
        {
            for (int i = 0; i < poHand.Tiles.Count; i++)
            {
                if (poTileToRemove.CompareTo(poHand.Tiles[i]) == 0)
                {
                    poHand.Tiles.RemoveAt(i);
                    i--;
                }
            }
        }

        public Hand Clone(Hand poHand)
        {
            Hand oHand = new Hand();
            oHand.Tiles = poHand.Tiles.ToList();
            return oHand;
        }
    }
}
