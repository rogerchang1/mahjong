using System;
using System.Collections.Generic;
using static Mahjong.Enums;

namespace Mahjong.Model
{
    public class Score
    {
        public int Han = 0;
        public int Fu = 0;
        public int SinglePayment = 0;
        public Dictionary<string, int> AllPayment = new Dictionary<string, int>() { { "Regular", 0 }, {"Dealer", 0 }};
        public List<Yaku> YakuList = new List<Yaku>();
    }
}
