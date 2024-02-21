﻿using System;
using System.Collections.Generic;
using Mahjong;
using Mahjong.Model;

namespace mahjong
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter a mahjong hand to parse:");
            //String sMahjongHand = "111133p445566s9m";
            //String sMahjongHand = "44455566677888p";
            String sMahjongHand = "44445566777888p";

            CHandParser oHandParser = new CHandParser();
            Hand oHand = oHandParser.ParseHand(sMahjongHand);
            oHand.SortTiles();

            Console.WriteLine("Mahjong hand to evaluate: ");
            printList(oHand.Tiles);
            Console.WriteLine();

            CShantenEvaluator shantenEvaluator = new CShantenEvaluator();
            int shanten = shantenEvaluator.EvaluateShanten(oHand);
            Console.WriteLine("The shanten is " + shanten);
            Console.WriteLine();

            CBlockSorter oBlockSorter = new CBlockSorter();
            Console.WriteLine("The hand can be configured in the following ways:");
            List<List<Block>> oBlockCombinations = oBlockSorter.GetBlockCombinations(oHand);
            foreach (List<Block> oBlockCombination in oBlockCombinations)
            {
                for (int i = 0; i < oBlockCombination.Count; i++)
                {
                    Block oBlock = oBlockCombination[i];
                    for (int j = 0; j < oBlock.Tiles.Count; j++)
                    {
                        Console.Write(oBlock.Tiles[j].ToString());
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

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
