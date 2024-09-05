using System;

namespace Mahjong.Model
{
    public class Tile : IEquatable<Tile>
    {
        public int num;
        public string suit;

        public Tile(int pNum, string pSuit)
        {
            num = pNum;
            suit = pSuit;
        }

        public Tile(Tile poTile)
        {
            num = poTile.num;
            suit = poTile.suit; 
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Tile objAsTile = obj as Tile;
            if (objAsTile == null) return false;
            else return Equals(objAsTile);
        }

        public bool Equals(Tile other)
        {
            if (other == null) return false;
            return this.num == other.num && this.suit == other.suit;
        }

        public Tile(int compareValue)
        {
            if (compareValue % 1000 >= 1)
            {
                suit = "z";
                num = compareValue - 1000;
            }
            else if (compareValue % 100 >= 1)
            {
                suit = "m";
                num = compareValue - 100;
            }
            else if (compareValue % 10 >= 1)
            {
                suit = "s";
                num = compareValue - 10;
            }
            else
            {
                suit = "p";
                num = compareValue;
            }
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
            if(other == null)
            {
                return int.MaxValue;
            }
            return this.CompareValue - other.CompareValue;
        }

        public override string ToString()
        {
            return this == null ? "" : this.num + this.suit;
        }
    }
}
