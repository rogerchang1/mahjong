using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mahjong.Model
{
    public class Hand
    {
        public List<Tile> Tiles = new List<Tile>();
        public List<Block> LockedBlocks = new List<Block>();
        public Tile WinningTile = null;

        public void SortTiles()
        {
            Tiles.Sort(delegate (Tile t1, Tile t2) { return t1.CompareTo(t2);});
        }

        //please sort hand first
        public int FindFirstIndexOfTile(Tile poTile)
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (poTile.CompareTo(Tiles[i]) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        //please sort hand first
        public Tile GetNextSequentialTileInTheSameSuit(Tile poTile)
        {
            if(poTile == null)
            {
                return null;
            }
            int index = FindFirstIndexOfTile(poTile);
            if(index == -1)
            {
                return null;
            }
            for (int i = index + 1; i < Tiles.Count; i++)
            {
                if (Tiles[i].num != Tiles[index].num && Tiles[i].suit == Tiles[index].suit)
                {
                    return Tiles[i];
                }
            }
            return null;
        }

        //please sort hand first
        public int CountNumberOfTilesOf(Tile poTile)
        {
            if (poTile == null)
            {
                return 0;
            }
            int index = FindFirstIndexOfTile(poTile);
            if (index == -1)
            {
                return 0;
            }
            int count = 1;
            for (int i = index + 1; i < Tiles.Count; i++)
            {
                if (Tiles[i].num == Tiles[index].num && Tiles[i].suit == Tiles[index].suit)
                {
                    count++;
                }
            }
            return count;
        }

        public Boolean CanBeStartOfARun(Tile poTile)
        {
            if (poTile == null)
            {
                return false;
            }

            SortTiles();

            int index = FindFirstIndexOfTile(poTile);
            
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

            Tile o2ndTile = GetNextSequentialTileInTheSameSuit(poTile);

            if(o2ndTile == null)
            {
                return false;
            }

            if(o2ndTile.CompareTo(poTile) / divisor != 1)
            {
                return false;
            }

            Tile o3rdTile = GetNextSequentialTileInTheSameSuit(o2ndTile);

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

        public void RemoveSingleTileOf(Tile poTileToRemove)
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                if(poTileToRemove.CompareTo(Tiles[i]) == 0)
                {
                    Tiles.RemoveAt(i);
                    break;
                }
            }
        }

        public void RemoveNumInstancesTileOf(Tile poTileToRemove, int nNumTimesToRemove)
        {
            int count = 0;
            for (int i = 0; i < Tiles.Count; i++)
            {
                
                if (poTileToRemove.CompareTo(Tiles[i]) == 0)
                {
                    Tiles.RemoveAt(i);
                    i--;
                    count++;
                    if (count >= nNumTimesToRemove)
                    {
                        break;
                    }
                }
            }
        }

        public void RemoveAllTilesOf(Tile poTileToRemove)
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (poTileToRemove.CompareTo(Tiles[i]) == 0)
                {
                    Tiles.RemoveAt(i);
                    i--;
                }
            }
        }

        public Hand Clone()
        {
            Hand oHand = new Hand();
            oHand.Tiles = Tiles.ToList();
            return oHand;
        }
    }
}
