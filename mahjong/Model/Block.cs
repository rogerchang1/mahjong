using System;
using System.Collections.Generic;
using static Mahjong.Enums;

namespace Mahjong.Model
{
    public class Block
    {
        public List<Tile> Tiles = new List<Tile>();
        public Mentsu Type = Mentsu.Unknown;
        public Boolean IsOpen = false;
        public KanType KanType = KanType.None;
    }
}
