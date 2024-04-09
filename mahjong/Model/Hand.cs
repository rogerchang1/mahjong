using System;
using System.Collections.Generic;
using static Mahjong.Enums;

namespace Mahjong.Model
{
    public class Hand
    {
        public List<Tile> Tiles = new List<Tile>();
        public List<Block> LockedBlocks = new List<Block>();
        public List<Tile> DiscardedTiles = new List<Tile>();
        public Tile WinTile = null;
        public Agari Agari = Agari.Unknown;
        public Wind SeatWind = Wind.Unknown;
        public Wind RoundWind = Wind.Unknown;
        public Boolean IsRiichi = false;
        public Boolean IsDoubleRiichi = false;
        public Boolean IsIppatsu = false;
        public Boolean IsRinshan = false;
        public Boolean IsChankan = false;
        public Boolean IsHaitei = false;
        public Boolean IsHoutei = false;
        public int DoraCount = 0;
        public int AkaDoraCount = 0;
        public int UraDoraCount = 0;
    }
}
