using System.Collections.Generic;

namespace Mahjong.Model
{
    public class Hand
    {
        public List<Tile> Tiles = new List<Tile>();
        public List<Block> LockedBlocks = new List<Block>();
        public Tile WinningTile = null;
    }
}
