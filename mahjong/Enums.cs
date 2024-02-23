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

        public enum Wind
        {
            Unknown,
            East,
            South,
            West,
            North
        }

        public enum Agari
        {
            Unknown,
            Tsumo,
            Ron
        }
    }
}
