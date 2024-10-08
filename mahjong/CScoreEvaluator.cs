﻿using Mahjong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static Mahjong.Enums;

namespace Mahjong
{
    public class CScoreEvaluator
    {
        private CBlockSorter _BlockSorter;
        private CBlockParser _BlockParser;
        private CShantenEvaluator _ShantenEvaluator;
        private CTilesManager _TilesManager;
        private CHandParser _HandParser;
        private CYakuEvaluator _YakuEvaluator;

        public static readonly Dictionary<Yaku, int> YAKUVALUE = new Dictionary<Yaku, int>()
        {
            { Yaku.Riichi, 1 },
            { Yaku.Ippatsu, 1 },
            { Yaku.Haitei, 1 },
            { Yaku.Houtei, 1 },
            { Yaku.Rinshan, 1 },
            { Yaku.Chankan, 1 },
            { Yaku.DoubleRiichi, 2 },
            { Yaku.Pinfu, 1 },
            { Yaku.Tanyao, 1 },
            { Yaku.Tsumo, 1 },
            { Yaku.YakuhaiHaku, 1 },
            { Yaku.YakuhaiHatsu, 1 },
            { Yaku.YakuhaiChun, 1 },
            { Yaku.YakuhaiTon, 1 },
            { Yaku.YakuhaiNan, 1 },
            { Yaku.YakuhaiSha, 1 },
            { Yaku.YakuhaiPei, 1 },
            { Yaku.Iipeikou, 1 },
            { Yaku.Ittsuu, 2 },
            { Yaku.SanshokuDoujun, 2 },
            { Yaku.Chanta, 2 },
            { Yaku.Junchan, 3 },
            { Yaku.Chiitoi, 2 },
            { Yaku.Toitoi, 2 },
            { Yaku.Sanankou, 2 },
            { Yaku.Sankantsu, 2 },
            { Yaku.SanshoukuDoukou, 2 },
            { Yaku.Honroutou, 2 },
            { Yaku.Shousangen, 2 },
            { Yaku.Honitsu, 3 },
            { Yaku.Chinitsu, 6 },
            { Yaku.Ryanpeikou, 3 },
            { Yaku.KazoeYakuman, 13 },
            { Yaku.KokushiMusou, 13 },
            { Yaku.Suuankou, 13 },
            { Yaku.Daisangen, 13 },
            { Yaku.Shousuushii, 13 },
            { Yaku.Daisuushii, 13 },
            { Yaku.Tsuuiisou, 13 },
            { Yaku.Chinroutou, 13 },
            { Yaku.Ryuuiisou, 13 },
            { Yaku.ChuurenPoutou, 13 },
            { Yaku.Suukantsu, 13 },
            { Yaku.Tenhou, 13 },
            { Yaku.Chiihou, 13 },
            { Yaku.NagashiMangan, 5 } };

        public CScoreEvaluator()
        {
            _ShantenEvaluator = new CShantenEvaluator();
            _BlockSorter = new CBlockSorter();
            _BlockParser = new CBlockParser();
            _TilesManager = new CTilesManager();
            _HandParser = new CHandParser();
            _YakuEvaluator = new CYakuEvaluator();
        }

        public Score EvaluateScore(Hand poHand)
        {

            if (_ShantenEvaluator.EvaluateShanten(poHand) != -1)
            {
                //TODO throw exception instead?
                return null;
            }

            Score oScore = new Score();

            List<List<Block>> oBlockCombinations = _BlockSorter.GetBlockCombinations(poHand);
            foreach (List<Block> oBlockCombination in oBlockCombinations)
            {
                Score tempScore = EvaluteScoreFromABlockCombination(poHand, oBlockCombination);
                if(tempScore.Han > oScore.Han || (tempScore.Han == oScore.Han && tempScore.Fu > oScore.Fu))
                {
                    oScore = tempScore;
                }
            }

            //Chiitoi and Kokushi won't have any block combinations
            if(oBlockCombinations.Count == 0)
            {
                Score tempScore = EvaluteScoreFromABlockCombination(poHand, null);
                if (tempScore.Han > oScore.Han || (tempScore.Han == oScore.Han && tempScore.Fu > oScore.Fu))
                {
                    oScore = tempScore;
                }
            }

            return oScore;
        }
        public Score EvaluteScoreFromABlockCombination(Hand poHand, List<Block> poBlockCombination)
        {
            if (_ShantenEvaluator.EvaluateShanten(poHand) != -1)
            {
                //TODO throw exception instead?
                return null;
            }
            
            List<Yaku> oYakuList = _YakuEvaluator.EvaluateYakusFromSingleBlockCombination(poHand, poBlockCombination);

            Score oScore = new Score();
            oScore.YakuList = oYakuList;
            int han = poHand.DoraCount + poHand.AkaDoraCount + poHand.UraDoraCount;


            Boolean bHasOpenBlocks = false;

            for(int i = 0; i < poHand.LockedBlocks.Count; i++)
            {
                if(poHand.LockedBlocks[i].IsOpen)
                {
                    bHasOpenBlocks = true;
                    break;
                }
            }

            Boolean bIsPinfu = false;
            Boolean bIsChiitoi = false;
            Boolean bIsKokushi = false;
            foreach (Yaku yaku in oYakuList)
            {
                
                int yakuHan = YAKUVALUE[yaku];
                if (yaku == Yaku.Pinfu)
                {
                    bIsPinfu = true;
                }
                else if (yaku == Yaku.Chiitoi)
                {
                    bIsChiitoi = true;
                }
                else if (yaku == Yaku.KokushiMusou)
                {
                    bIsKokushi = true;
                }
                else if ((yaku == Yaku.Chankan || yaku == Yaku.Junchan || yaku == Yaku.Honitsu || yaku == Yaku.Chinitsu || yaku == Yaku.Ittsuu || yaku == Yaku.SanshokuDoujun) && bHasOpenBlocks)
                {
                    yakuHan--;
                }
                else if (yaku == Yaku.YakuhaiTon && poHand.RoundWind == Enums.Wind.East && poHand.SeatWind == Enums.Wind.East)
                {
                    yakuHan++;
                }
                else if (yaku == Yaku.YakuhaiNan && poHand.RoundWind == Enums.Wind.South && poHand.SeatWind == Enums.Wind.South)
                {
                    yakuHan++;
                }
                else if (yaku == Yaku.YakuhaiSha && poHand.RoundWind == Enums.Wind.West && poHand.SeatWind == Enums.Wind.West)
                {
                    yakuHan++;
                }
                else if (yaku == Yaku.YakuhaiPei && poHand.RoundWind == Enums.Wind.North && poHand.SeatWind == Enums.Wind.North)
                {
                    yakuHan++;
                }
                oScore.HanBreakdown.Add(yaku.ToString() + " - " + yakuHan + " han");
                han += yakuHan;
            }
            oScore.Han = han;
            ScoreFu oScoreFu = CalculateFu(poHand, poBlockCombination, bIsPinfu, bIsChiitoi, bIsKokushi);
            oScore.Fu = oScoreFu.Fu;
            oScore.FuBreakdown = oScoreFu.FuBreakdown;
            double oPayment = 0;

            if(han == 5 || (han == 4 && oScore.Fu >= 40) || (han == 3 && oScore.Fu >= 70) )
            {
                oPayment = 2000; //mangan
            }else if(han == 6 || han == 7)
            {
                oPayment = 3000; //haneman, * 1.5
            }
            else if (han >= 8 && han <= 10)
            {
                oPayment = 4000; //baiman, * 2
            }
            else if (han == 11 || han == 12)
            {
                oPayment = 6000; //sanbaiman, * 3
            }
            else if (han >= 13)
            {
                oPayment = 8000; //yakuman, * 4
            }
            else
            {
                oPayment = oScore.Fu * Math.Pow(2, (oScore.Han + 2));
            }


            if (poHand.Agari == Agari.Ron)
            {
                oPayment  = oPayment* 4;
                if (poHand.SeatWind == Wind.East)
                {
                    oPayment = oPayment * 1.5;
                }
                oScore.SinglePayment = (int)(Math.Ceiling((decimal)oPayment / 100) * 100);
            }
            
            if (poHand.Agari == Agari.Tsumo)
            {
                if (poHand.SeatWind == Wind.East)
                {
                    oPayment = oPayment * 2;
                }
                else
                {
                    double oDealerPayment = 2 * oPayment;
                    oScore.AllPayment["Dealer"] = (int)(Math.Ceiling((decimal)oDealerPayment / 100) * 100);
                }
                oScore.AllPayment["Regular"] = (int)(Math.Ceiling((decimal)oPayment / 100) * 100);
            }
            return oScore;
        }
        public ScoreFu CalculateFu(Hand poHand, List<Block> poBlockCombination, Boolean pbIsPinfu = false, Boolean pbIsChiitoi = false, Boolean pbIsKokushiMusou = false)
        {
            ScoreFu oScoreFu = new ScoreFu();
            List<string> oFuBreakdown = new List<string>() { "Base fu - 20 fu" };
            if (pbIsChiitoi)
            {
                oScoreFu.Fu = 25;
                oScoreFu.FuBreakdown = new List<string>() { "Chiitoitsu fu - 20 fu" };
                return oScoreFu;
            }
            if (pbIsKokushiMusou)
            {
                oScoreFu.Fu = 20;
                return oScoreFu;
            }
            if (pbIsPinfu)
            {
                if(poHand.Agari == Agari.Tsumo)
                {
                    oScoreFu.Fu = 20;
                    oScoreFu.FuBreakdown = oFuBreakdown;
                    return oScoreFu;
                }
                oScoreFu.Fu = 30;
                oFuBreakdown.Add("Closed Ron - 10 fu");
                oScoreFu.FuBreakdown = oFuBreakdown;
                return oScoreFu;
            }
            int nFu = 20;
            if (poHand.Agari == Agari.Tsumo)
            {
                nFu += 2;
                oFuBreakdown.Add("Tsumo - 2 fu");
            }
            if (poHand.Agari == Agari.Ron && _TilesManager.IsHandClosed(poHand))
            {
                nFu += 10;
                oFuBreakdown.Add("Closed Ron - 10 fu");
            }
            Boolean bIsWinTileFuCounted = false;

            foreach(Block oBlock in poBlockCombination)
            {
                int nBlockFu = 0;
                if(oBlock.Type == Mentsu.Unknown)
                {
                    throw new Exception("Unknown Block Type");
                }
                
                if(oBlock.Type == Mentsu.Kantsu || oBlock.Type == Mentsu.Koutsu)
                {
                    nBlockFu = 2;
                    string sBlock = "";
                    if (oBlock.IsOpen == false && !(poHand.Agari == Agari.Ron && _TilesManager.ContainsTileOf(oBlock.Tiles, poHand.WinTile))) 
                    {
                        nBlockFu = nBlockFu * 2;
                        sBlock += "Closed ";
                    }
                    else
                    {
                        sBlock = "Open ";
                    }
                    if (_TilesManager.IsTerminalTile(oBlock.Tiles[0]) || _TilesManager.IsHonorTile(oBlock.Tiles[0]))
                    {
                        nBlockFu = nBlockFu * 2;
                        sBlock += "Terminal/Honor ";
                    }
                    if(oBlock.Type == Mentsu.Kantsu)
                    {
                        nBlockFu = nBlockFu * 4;
                        sBlock += "Quad ";
                    }
                    else
                    {
                        sBlock += "Triplet ";
                    }
                    sBlock += "- " + nBlockFu + " fu";
                    oFuBreakdown.Add(sBlock);
                }
                else if(oBlock.Type == Mentsu.Shuntsu)
                {
                    if(_TilesManager.IsTileAKanChan(oBlock.Tiles,poHand.WinTile) || _TilesManager.IsTileAPenChan(oBlock.Tiles, poHand.WinTile))
                    {
                        if (!bIsWinTileFuCounted)
                        {
                            nBlockFu += 2;
                            bIsWinTileFuCounted = true;
                            if (_TilesManager.IsTileAKanChan(oBlock.Tiles, poHand.WinTile))
                            {
                                oFuBreakdown.Add("Closed Wait - 2 fu");
                            }
                            else if (_TilesManager.IsTileAPenChan(oBlock.Tiles, poHand.WinTile))
                            {
                                oFuBreakdown.Add("Edge Wait - 2 fu");
                            }
                        }
                    }
                }
                else
                {
                    //Jantou is left
                    if (_TilesManager.ContainsTileOf(oBlock.Tiles, poHand.WinTile))
                    {
                        if (!bIsWinTileFuCounted)
                        {
                            nBlockFu += 2;
                            bIsWinTileFuCounted = true;
                            oFuBreakdown.Add("Pair Wait - 2 fu");
                        }
                    }
                    if (_TilesManager.IsDragonTile(oBlock.Tiles[0]))
                    {
                        nBlockFu += 2;
                        oFuBreakdown.Add("Dragon Pair - 2 fu");
                    }
                    if (oBlock.Tiles[0].CompareTo(WindEnumToTile(poHand.RoundWind)) == 0)
                    {
                        nBlockFu += 2;
                        oFuBreakdown.Add("Round Wind Pair - 2 fu");
                    }
                    if (oBlock.Tiles[0].CompareTo(WindEnumToTile(poHand.SeatWind)) == 0)
                    {
                        nBlockFu += 2;
                        oFuBreakdown.Add("Seat Wind Pair - 2 fu");
                    }
                }
                nFu += nBlockFu;
            }
            int oldFu = nFu;
            nFu = (int)(Math.Ceiling((decimal)nFu / 10) * 10);
            int roundUp = nFu - oldFu;
            oFuBreakdown.Add("Round Up - " + roundUp + " fu");
            oScoreFu.Fu = nFu;
            oScoreFu.FuBreakdown = oFuBreakdown;

            return oScoreFu;
        }
    }
}
