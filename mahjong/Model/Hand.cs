using System;
using System.Collections.Generic;
using static Mahjong.Enums;

namespace Mahjong.Model
{
    public class Hand
    {
        public List<Tile> Tiles = new List<Tile>();
        public List<Block> LockedBlocks = new List<Block>();
        public Tile WinTile = null;
        public Agari Agari = Agari.Unknown;
        public Wind SeatWind = Wind.Unknown;
        public Wind RoundWind = Wind.Unknown;
        public Boolean IsRiichi = false;
        public Boolean IsDoubleRiichi = false;
        public int DoraCount = 0;
    }
}
