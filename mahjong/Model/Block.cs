using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong.Model
{
    public class Block
    {
        public List<Tile> Tiles = new List<Tile>();
        public Boolean isOpen = false;
        public Boolean isTriplet = false;
        public Boolean isSequence = false;
        public Boolean isPair = false;
        public Boolean isKan = false;
    }
}
