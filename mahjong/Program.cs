using Mahjong;
using Mahjong.Model;
using System;
using System.Collections.Generic;
using static Mahjong.Enums;

namespace mahjong
{
    class Program
    {
        static void Main(string[] args)
        {
            CHandParser oHandParser = new CHandParser();
            CTilesManager oTilesManager = new CTilesManager();
            CBlockParser oBlockParser = new CBlockParser();
            CShantenEvaluator shantenEvaluator = new CShantenEvaluator();
            CBlockSorter oBlockSorter = new CBlockSorter();
            CYakuEvaluator oYakuEvaluator = new CYakuEvaluator();

            //Build the hand

            //Console.WriteLine("Enter a mahjong hand to parse:");
            //String sMahjongHand = "111133p445566s9m"; //quad tile to see if it'll trip
            //String sMahjongHand = "44555666777888p"; //lots of block combinations
            String sMahjongHand = "34555666777888p"; //lots of block combinations, finished hand

            List<String> sCalledBlocks = new List<String>(){ 
                //"888p" 
            };

            Hand oHand = oHandParser.ParseHand(sMahjongHand);
            oTilesManager.SortTiles(oHand.Tiles);

            Console.WriteLine("Mahjong hand to evaluate: ");
            printList(oHand.Tiles);
            Console.WriteLine();

            if (sCalledBlocks.Count > 0)
            {
                foreach(String sCalledBlock in sCalledBlocks)
                {
                    oHand.LockedBlocks.Add(oBlockParser.ParseBlock(sCalledBlock, true));
                }
                
                Console.WriteLine("Called groups: ");
                foreach(Block oBlock in oHand.LockedBlocks)
                {
                    printList(oBlock.Tiles);
                }
                Console.WriteLine();
            }

            oHand.WinTile = new Tile("8p");
            Console.WriteLine("Win tile: " + oHand.WinTile.ToString());
            Console.WriteLine();

            //End Build the hand---------------------------------------------------------------------------------------------

            int shanten = shantenEvaluator.EvaluateShanten(oHand);
            Console.WriteLine("The shanten is " + shanten);
            Console.WriteLine();

            
            Console.WriteLine("The hand can be configured in the following ways:");
            List<List<Block>> oBlockCombinations = oBlockSorter.GetBlockCombinations(oHand);
            foreach (List<Block> oBlockCombination in oBlockCombinations)
            {
                printBlockCombination(oBlockCombination);
                printYakuList(oYakuEvaluator.EvaluateYakusFromSingleBlockCombination(oHand, oBlockCombination));
                Console.WriteLine();
            }

        }

        public static void printList(List<Tile> pTileList)
        {
            for (int i = 0; i < pTileList.Count; i++)
            {
                Console.Write(pTileList[i].ToString() + " ");
            }
            Console.WriteLine();
        }

        public static void printBlockCombination(List<Block> pBlockList)
        {
            for (int i = 0; i < pBlockList.Count; i++)
            {
                Block oBlock = pBlockList[i];
                for (int j = 0; j < oBlock.Tiles.Count; j++)
                {
                    Console.Write(oBlock.Tiles[j].ToString());
                }
                Console.Write(", ");
            }
            Console.WriteLine();
        }

        public static void printYakuList(List<Yaku> pYakuList)
        {
            for (int i = 0; i < pYakuList.Count; i++)
            {
                Console.Write(pYakuList[i].ToString().ToUpper() + " ");
            }
            Console.WriteLine();
        }

    }
}
