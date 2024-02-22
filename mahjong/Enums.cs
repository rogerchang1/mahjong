using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
