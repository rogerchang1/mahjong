using System;
using System.Collections.Generic;
using System.Text;
using static Mahjong.Enums;

namespace Mahjong.Model
{
    public class Table
    {
        public List<Tile> Wall = new List<Tile>();
        public Wind RoundWind;
        public int Honba1000 = 0;
        public int Honba300 = 0;
        public List<int> DoraIndicator = new List<int>();
    }
}
