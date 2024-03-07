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
            CJSONHandLoader oJSONHandLoader = new CJSONHandLoader();
            CScoreEvaluator oScoreEvaluator = new CScoreEvaluator();
            //Load the hand(s)
            //Hand oHand = oJSONHandLoader.CreateHandFromJSONFile("R:/roger/coding/mahjong/mahjong/SampleHands/hand1.json");
            List<Hand> oHandList = oJSONHandLoader.CreateHandsFromJSONFile("R:/roger/coding/mahjong/mahjong/SampleHands/hand1.json");

            foreach(Hand oHand in oHandList){
                printHandInformation(oHand);

                //Evaluate the hand
                int shanten = shantenEvaluator.EvaluateShanten(oHand);
                if(shanten == -1)
                {
                    Score oScore = oScoreEvaluator.EvaluateScore(oHand);
                    printScore(oScore);
                }
                else
                {
                    Console.WriteLine("The shanten is " + shanten);
                }

                //List<List<Block>> oBlockCombinations = oBlockSorter.GetBlockCombinations(oHand);
                //foreach (List<Block> oBlockCombination in oBlockCombinations)
                //{
                //    printBlockCombination(oBlockCombination);
                //    printYakuList(oYakuEvaluator.EvaluateYakusFromSingleBlockCombination(oHand, oBlockCombination));
                //    Score oScore = oScoreEvaluator.EvaluteScoreFromABlockCombination(oHand, oBlockCombination);
                //    printScore(oScore);
                //    Console.WriteLine();
                //}
                Console.WriteLine("//-------------------------------------------------------------------------------------------");
            }
        }
        #region PrintFunctions
        public static void printTileList(List<Tile> pTileList)
        {
            for (int i = 0; i < pTileList.Count; i++)
            {
                Console.Write(pTileList[i].ToString() + " ");
            }
            Console.WriteLine();
        }

        public static void printLockedBlocks(Hand poHand)
        {
            if(poHand.LockedBlocks.Count > 0)
            {
                Console.WriteLine("Locked groups: ");
                foreach (Block oBlock in poHand.LockedBlocks)
                {
                    printTileList(oBlock.Tiles);
                }
                Console.WriteLine();
            }
        }

        public static void printHandInformation(Hand poHand)
        {
            Console.WriteLine("Mahjong hand to evaluate: ");
            printTileList(poHand.Tiles);
            Console.WriteLine();

            printLockedBlocks(poHand);

            Console.WriteLine("Additional Hand Information:");
            Console.WriteLine("Win tile: " + poHand.WinTile.ToString() + ", Win type: " + poHand.Agari.ToString());
            Console.WriteLine("Round Wind: " + poHand.RoundWind.ToString() + ", Seat Wind: " + poHand.SeatWind.ToString());
            Console.WriteLine("Riichi?: " + poHand.IsRiichi + ", Double Riichi?: " + poHand.IsDoubleRiichi);
            Console.WriteLine("Ippatsu?: " + poHand.IsIppatsu + ", Rinshan Kaihou?: " + poHand.IsRinshan);
            Console.WriteLine("Chankan?: " + poHand.IsChankan + ", Haitei?: " + poHand.IsHaitei + ", Houtei?: " + poHand.IsHoutei);
            Console.WriteLine("Dora: " + poHand.DoraCount + ", AkaDora: " + poHand.AkaDoraCount + ", UraDora: " + poHand.UraDoraCount);
            Console.WriteLine();
        }

        public static void printBlockCombination(List<Block> pBlockList)
        {
            Console.WriteLine("The hand can be configured in the following ways:");
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

        public static void printScore(Score poScore)
        {
            printYakuList(poScore.YakuList);
            Console.Write("Score: " + poScore.Han + " han " + poScore.Fu + " fu, ");
            if (poScore.SinglePayment != 0)
            {
                Console.WriteLine(poScore.SinglePayment);
            }
            else
            {
                if (poScore.AllPayment["Dealer"] == 0)
                {
                    Console.WriteLine(poScore.AllPayment["Regular"] + " ALL");
                }
                else
                {
                    Console.WriteLine(poScore.AllPayment["Regular"] + "-" + poScore.AllPayment["Dealer"]);
                }

            }
        }
        #endregion
    }
}
