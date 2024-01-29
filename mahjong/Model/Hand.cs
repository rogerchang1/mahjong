using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mahjong.Model
{
    public class Hand
    {
        public List<Tile> Tiles = new List<Tile>();

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
        public Tile GetNextSequentialTile(Tile poTile)
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
