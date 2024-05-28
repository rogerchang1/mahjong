using Mahjong.Model;
using System;

namespace Mahjong
{
    public class Enums
    {
        public enum Mentsu
        {
            Unknown,
            /// <summary>
            /// Sequence
            /// </summary>
            Shuntsu,
            /// <summary>
            /// Triplet
            /// </summary>
            Koutsu,
            /// <summary>
            /// Quad
            /// </summary>
            Kantsu,
            /// <summary>
            /// Pair
            /// </summary>
            Jantou
        }

        public enum Suit
        {
            Unknown,
            Pinzu,
            Souzu,
            Manzu,
            Honor
        }

        public static Suit TileSuitToEnum(Tile poTile)
        {
            switch (poTile.suit)
            {
                case "p":
                    return Suit.Pinzu;
                case "s":
                    return Suit.Souzu;
                case "m":
                    return Suit.Manzu;
                case "z":
                    return Suit.Honor;
                default:
                    return Suit.Unknown;
            }
        }

        public static String SuitEnumToString(Suit peSuit)
        {
            switch (peSuit)
            {
                case Suit.Pinzu:
                    return "p";
                case Suit.Souzu:
                    return "s";
                case Suit.Manzu:
                    return "m";
                case Suit.Honor:
                    return "z";
                default:
                    return "";
            }
        }

        public enum Wind
        {
            Unknown,
            East,
            South,
            West,
            North
        }

        public static Wind WindStringToEnum(String psWind)
        {
            switch (psWind.ToLower().Trim())
            {
                case "east":
                    return Wind.East;
                case "south":
                    return Wind.South;
                case "west":
                    return Wind.West;
                case "north":
                    return Wind.North;
                default:
                    return Wind.Unknown;
            }
        }

        public static Wind WindTileToEnum(Tile poTile)
        {
            if (poTile.suit != "z")
            {
                return Wind.Unknown;
            }
            switch (poTile.num)
            {
                case 1:
                    return Wind.East;
                case 2:
                    return Wind.South;
                case 3:
                    return Wind.West;
                case 4:
                    return Wind.North;
                default:
                    return Wind.Unknown;
            }
        }

        public static Tile WindEnumToTile(Wind peWind)
        {
            switch (peWind)
            {
                case Wind.East:
                    return new Tile("1z");
                case Wind.South:
                    return new Tile("2z");
                case Wind.West:
                    return new Tile("3z");
                case Wind.North:
                    return new Tile("4z");
                default:
                    return null;
            }
        }

        public enum Dragon
        {
            Unknown,
            Haku,
            Hatsu,
            Chun
        }

        public static Dragon DragonTileToEnum(Tile poTile)
        {
            if(poTile.suit != "z")
            {
                return Dragon.Unknown;
            }
            switch (poTile.num)
            {
                case 5:
                    return Dragon.Haku;
                case 6:
                    return Dragon.Hatsu;
                case 7:
                    return Dragon.Chun;
                default:
                    return Dragon.Unknown;
            }
        }

        public static Tile DragonEnumToTile(Dragon peDragon)
        {
            switch (peDragon)
            {
                case Dragon.Haku:
                    return new Tile("5z");
                case Dragon.Hatsu:
                    return new Tile("6z");
                case Dragon.Chun:
                    return new Tile("7z");
                default:
                    return null;
            }
        }

        public enum Agari
        {
            Unknown,
            Tsumo,
            Ron
        }

        public static Agari AgariStringToEnum(String psAgari)
        {
            switch (psAgari.ToLower().Trim())
            {
                case "tsumo":
                    return Agari.Tsumo;
                case "ron":
                    return Agari.Ron;
                default:
                    return Agari.Unknown;
            }
        }

        public enum Yaku
        {
            Riichi,
            Ippatsu,
            Haitei,
            Houtei,
            Rinshan,
            Chankan,
            DoubleRiichi,
            Pinfu,
            Tanyao,
            Tsumo,
            YakuhaiHaku,
            YakuhaiHatsu,
            YakuhaiChun,
            YakuhaiTon,
            YakuhaiNan,
            YakuhaiSha,
            YakuhaiPei,
            Iipeikou,
            Ittsuu,
            SanshokuDoujun,
            Chanta,
            Junchan,
            Chiitoi,
            Toitoi,
            Sanankou,
            Sankantsu,
            SanshoukuDoukou,
            Honroutou,
            Shousangen,
            Honitsu,
            Chinitsu,
            Ryanpeikou,
            KazoeYakuman,
            KokushiMusou,
            Suuankou,
            Daisangen,
            Shousuushii,
            Daisuushii,
            Tsuuiisou,
            Chinroutou,
            Ryuuiisou,
            ChuurenPoutou,
            Suukantsu,
            Tenhou,
            Chiihou,
            NagashiMangan
        }
    }
}
