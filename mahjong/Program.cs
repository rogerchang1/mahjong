using System;
using System.Collections.Generic;
using Mahjong;
using Mahjong.Model;

namespace mahjong
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a mahjong hand to parse:");
            String sMahjongHand = "111133p445566s9m";
            Console.WriteLine("Mahjong hand to evaluate: ");

            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.parseHand(sMahjongHand);
            oHand.SortTiles();
            printList(oHand.Tiles);
            CShantenEvaluator shantenEvaluator = new CShantenEvaluator();
            int shanten = shantenEvaluator.EvaluateShanten(oHand);
            Console.WriteLine("The shanten is " + shanten);
        }

        public static void printList(List<Tile> pTileList)
        {
            for (int i = 0; i < pTileList.Count; i++)
            {
                Console.Write(pTileList[i].ToString()+" ");
            }
            Console.WriteLine();
        }
    }
}
