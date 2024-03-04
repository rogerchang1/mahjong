using Mahjong.Model;
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
            { Yaku.ChuurenPotou, 13 },
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

            if (_ShantenEvaluator.EvaluateShanten(poHand.Tiles) != -1)
            {
                //TODO throw exception instead?
                return null;
            }

            Score oScore = new Score();

            List<List<Block>> oBlockCombinations = _BlockSorter.GetBlockCombinations(poHand.Tiles);
            foreach (List<Block> oBlockCombination in oBlockCombinations)
            {
                Score tempScore = EvaluteScoreFromABlockCombination(poHand, oBlockCombination);
            }
            return oScore;
        }
        public Score EvaluteScoreFromABlockCombination(Hand poHand, List<Block> poBlockCombination)
        {
            if (_ShantenEvaluator.EvaluateShanten(poHand.Tiles) != -1)
            {
                //TODO throw exception instead?
                return null;
            }
            
            List<Yaku> oYakuList = _YakuEvaluator.EvaluateYakusFromSingleBlockCombination(poHand, poBlockCombination);

            Score oScore = new Score();
            int han = 0;
            Boolean bIsPinfu = false;
            Boolean bIsChiitoi = false;
            foreach (Yaku yaku in oYakuList)
            {
                han += YAKUVALUE[yaku];
                if (yaku == Yaku.Pinfu)
                {
                    bIsPinfu = true;
                }
                else if (yaku == Yaku.Chiitoi)
                {
                    bIsChiitoi = true;
                }
            }
            oScore.Han = han;
            oScore.Fu = CalculateFu(poHand, poBlockCombination, bIsPinfu, bIsChiitoi);
            double oPayment = oScore.Fu * Math.Pow(2, (oScore.Han + 2));

            
            if (poHand.Agari == Agari.Ron)
            {
                oPayment  = oPayment* 4;
                if (poHand.SeatWind == Wind.East)
                {
                    oPayment = oPayment * 1.5;
                }
                
            }
            else if (poHand.Agari == Agari.Tsumo && poHand.SeatWind != Wind.East)
            {
                double oDealerPayment = 2 * oPayment;
                oScore.DealerPayment = (int)(Math.Ceiling((decimal)oDealerPayment / 100) * 100);
            }


            oScore.Payment = (int)(Math.Ceiling((decimal)oPayment / 100) * 100);
            

            return oScore;
        }
        public int CalculateFu(Hand poHand, List<Block> poBlockCombination, Boolean pbIsPinfu = false, Boolean pbIsChiitoi = false)
        {
            if (pbIsChiitoi)
            {
                return 25;
            }
            if (pbIsPinfu)
            {
                if(poHand.Agari == Agari.Tsumo)
                {
                    return 20;
                }
                return 30;
            }
            int nFu = 20;
            if (poHand.Agari == Agari.Tsumo)
            {
                nFu += 2;
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
                    if (oBlock.IsOpen == false && !(poHand.Agari == Agari.Ron && _TilesManager.ContainsTileOf(oBlock.Tiles, poHand.WinTile))) 
                    {
                        nBlockFu = nBlockFu * 2;
                    }
                    if (_TilesManager.IsTerminalTile(oBlock.Tiles[0]) || _TilesManager.IsHonorTile(oBlock.Tiles[0]))
                    {
                        nBlockFu = nBlockFu * 2;
                    }
                    if(oBlock.Type == Mentsu.Kantsu)
                    {
                        nBlockFu = nBlockFu * 4;
                    }
                }
                else if(oBlock.Type == Mentsu.Shuntsu)
                {
                    if(_TilesManager.IsTileAKanChan(oBlock.Tiles,poHand.WinTile) || _TilesManager.IsTileAPenChan(oBlock.Tiles, poHand.WinTile))
                    {
                        if (!bIsWinTileFuCounted)
                        {
                            nBlockFu += 2;
                            bIsWinTileFuCounted = true;
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
                        }
                    }
                    if (_TilesManager.IsDragonTile(oBlock.Tiles[0]))
                    {
                        nBlockFu += 2;
                    }
                    if (oBlock.Tiles[0].CompareTo(WindEnumToTile(poHand.RoundWind)) == 0)
                    {
                        nBlockFu += 2;
                    }
                    if (oBlock.Tiles[0].CompareTo(WindEnumToTile(poHand.SeatWind)) == 0)
                    {
                        nBlockFu += 2;
                    }
                }
                nFu += nBlockFu;
            }
            nFu = (int)(Math.Ceiling((decimal)nFu / 10) * 10);

            return nFu;
        }
    }
}
