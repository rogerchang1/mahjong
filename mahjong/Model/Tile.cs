using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong.Model
{
    public class Tile
    {
        public int num;
        public string suit;

        public Tile(int pNum, string pSuit)
        {
            num = pNum;
            suit = pSuit;
        }

        public Tile(string psTileAbrreviation)
        {
            num = psTileAbrreviation[0] - '0';
            suit = psTileAbrreviation[1].ToString();
        }

        public int CompareValue
        {
            get
            {
                switch (this.suit)
                {
                    case "p":
                        return 1 * num;
                    case "s":
                        return 10 * num;
                    case "m":
                        return 100 * num;
                    case "z":
                        return 1000 * num;
                    default:
                        return 0;
                }
            }
        }

        public int CompareTo(Tile other)
        {
            return this.CompareValue - other.CompareValue;
        }

        public override string ToString()
        {
            return this == null ? "" : this.num + this.suit;
        }
    }
}
