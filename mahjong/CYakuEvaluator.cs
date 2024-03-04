using Mahjong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static Mahjong.Enums;

namespace Mahjong
{
    public class CYakuEvaluator
    {
        private CBlockSorter _BlockSorter;
        private CBlockParser _BlockParser;
        private CShantenEvaluator _ShantenEvaluator;
        private CTilesManager _TilesManager;
        private CHandParser _HandParser;

        public CYakuEvaluator()
        {
            _ShantenEvaluator = new CShantenEvaluator();
            _BlockSorter = new CBlockSorter();
            _BlockParser = new CBlockParser();
            _TilesManager = new CTilesManager();
            _HandParser = new CHandParser();
        }

        public List<List<Yaku>> EvaluateYaku(Hand poHand)
        {
            List<List<Yaku>> oYakuCombinations = new List<List<Yaku>>();

            if (_ShantenEvaluator.EvaluateShanten(poHand.Tiles) != -1)
            {
                return oYakuCombinations;
            }

            List<List<Block>> oBlockCombinations = _BlockSorter.GetBlockCombinations(poHand.Tiles);

            foreach (List<Block> oBlockCombination in oBlockCombinations)
            {
                oYakuCombinations.Add(EvaluateYakusFromSingleBlockCombination(poHand, oBlockCombination));
            }

            return oYakuCombinations;
        }

        //TODO: Think about the fu count for 222m77z789p34556s. If tsumo/riichi nomi win on a 4s, is that a kanchan wait for the 2fu or ryanmen wait with 0fu
        public List<Yaku> EvaluateYakusFromSingleBlockCombination(Hand poHand, List<Block> poBlockConfiguration)
        {
            List<Yaku> oYakuCombination = new List<Yaku>();

            //Evaluate Yakuman first because they have the highest priority
            #region Yakuman

            if (IsKokushiMusou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.KokushiMusou);
            }
            if (IsSuuankou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Suuankou);
            }
            if (IsDaisangen(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Daisangen);
            }
            if (IsDaisuushii(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Daisuushii);
            }
            else if (IsShousuushii(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Shousuushii);
            }
            if (IsTsuuiisou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Tsuuiisou);
            }
            if (IsChinroutou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Chinroutou);
            }
            if (IsRyuuiisou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Ryuuiisou);
            }
            if (IsChuurenPoutou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.ChuurenPotou);
            }
            if (IsSuukantsu(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Suukantsu);
            }
            #endregion

            //Yakuman takes highest priority
            if (oYakuCombination.Count > 0)
            {
                return oYakuCombination;
            }

            #region RegularYakus
            //Order of Yakus matter, for instance, Daisangen > Shousangen, Rynpeikou > Iipeikou, Junchan > Chanta
            if (IsMenzenTsumo(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Tsumo);

            }
            if (IsPinfu(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Pinfu);

            }
            if (IsTanyao(poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Tanyao);
            }

            if (IsSanshokuDoujun(poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.SanshokuDoujun);
            }else if (IsIttsuu(poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Ittsuu);
            }else if (IsJunchan(poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Junchan);
            }else if (IsChanta(poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Chanta);
            }

            if (IsRyanpeikou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Ryanpeikou);
            }
            else
            {
                if (IsChiitoi(poHand, poBlockConfiguration))
                {
                    oYakuCombination.Add(Yaku.Iipeikou);
                }
                if (IsIipeikou(poHand, poBlockConfiguration))
                {
                    oYakuCombination.Add(Yaku.Iipeikou);
                }
            }
            
            if (IsSankantsu(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Sankantsu);
            }else if (IsSanankou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Sanankou);
            }

            if (IsToitoi(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Toitoi);
            }

            if (IsHonroutou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Honroutou);
            }

            if (IsSanshokuDoukou(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.SanshoukuDoukou);
            }

            if (IsChinitsu(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Chinitsu);
            }else if (IsHonitsu(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.Honitsu);
            }

            if (IsHatsu(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.YakuhaiHatsu);
            }

            if (IsChun(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.YakuhaiChun);
            }

            if (IsHaku(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.YakuhaiHaku);
            }

            if (IsTon(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.YakuhaiTon);
            }

            if (IsNan(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.YakuhaiNan);
            }

            if (IsSha(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.YakuhaiSha);
            }

            if (IsPei(poHand, poBlockConfiguration))
            {
                oYakuCombination.Add(Yaku.YakuhaiPei);
            }
            #endregion

            return oYakuCombination;
        }

        #region Yaku Evaluator Functions

        #region Yakuman Evaluator Functions

        public Boolean IsKokushiMusou(Hand poHand, List<Block> poBlockCombination)
        {
            return _ShantenEvaluator.EvaluateShantenForKokushiMusou(poHand.Tiles) == -1;
        }

        public Boolean IsSuuankou(Hand poHand, List<Block> poBlockCombination)
        {
            if (!IsHandClosedAtTenpai(poHand))
            {
                return false;
            }

            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                if (poBlockCombination[i].Type == Mentsu.Shuntsu)
                {
                    return false;
                }
                //Win on a ron will cause the winning tile's block to be open
                if ((poBlockCombination[i].Type == Mentsu.Koutsu || poBlockCombination[i].Type == Mentsu.Kantsu) && poHand.Agari == Agari.Ron && poBlockCombination[i].Tiles[0].CompareTo(poHand.WinTile) == 0)
                //if ((poBlockCombination[i].Type == Mentsu.Koutsu && poBlockCombination[i].Type == Mentsu.Kantsu) && poBlockCombination[i].IsOpen == true)
                {
                    return false;
                }
            }
            return true;
        }

        public Boolean IsDaisangen(Hand poHand, List<Block> poBlockCombination)
        {
            Boolean bChunFound = false;
            Boolean bHakuFound = false;
            Boolean bHatsuFound = false;
            foreach (Block oBlock in poBlockCombination)
            {
                if (oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu)
                {
                    if (DragonTileToEnum(oBlock.Tiles[0]) == Dragon.Chun)
                    {
                        bChunFound = true;
                    }
                    if (DragonTileToEnum(oBlock.Tiles[0]) == Dragon.Haku)
                    {
                        bHakuFound = true;
                    }
                    if (DragonTileToEnum(oBlock.Tiles[0]) == Dragon.Hatsu)
                    {
                        bHatsuFound = true;
                    }
                }
            }
            return bChunFound && bHakuFound && bHatsuFound;
        }

        public Boolean IsDaisuushii(Hand poHand, List<Block> poBlockCombination)
        {
            Boolean bEastFound = false;
            Boolean bSouthFound = false;
            Boolean bWestFound = false;
            Boolean bNorthFound = false;
            foreach (Block oBlock in poBlockCombination)
            {
                if (oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu)
                {
                    if (WindTileToEnum(oBlock.Tiles[0]) == Wind.East)
                    {
                        bEastFound = true;
                    }
                    if (WindTileToEnum(oBlock.Tiles[0]) == Wind.South)
                    {
                        bSouthFound = true;
                    }
                    if (WindTileToEnum(oBlock.Tiles[0]) == Wind.West)
                    {
                        bWestFound = true;
                    }
                    if (WindTileToEnum(oBlock.Tiles[0]) == Wind.North)
                    {
                        bNorthFound = true;
                    }
                }
            }
            return bEastFound && bSouthFound && bWestFound && bNorthFound;
        }

        public Boolean IsShousuushii(Hand poHand, List<Block> poBlockCombination)
        {
            Boolean bEastFound = false;
            Boolean bSouthFound = false;
            Boolean bWestFound = false;
            Boolean bNorthFound = false;
            Boolean bJantouFound = false;
            foreach (Block oBlock in poBlockCombination)
            {
                if (oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu || oBlock.Type == Mentsu.Jantou)
                {
                    if (WindTileToEnum(oBlock.Tiles[0]) == Wind.East)
                    {
                        bEastFound = true;
                        if(oBlock.Type == Mentsu.Jantou)
                        {
                            bJantouFound = true;
                        }
                    }
                    if (WindTileToEnum(oBlock.Tiles[0]) == Wind.South)
                    {
                        bSouthFound = true;
                        if (oBlock.Type == Mentsu.Jantou)
                        {
                            bJantouFound = true;
                        }
                    }
                    if (WindTileToEnum(oBlock.Tiles[0]) == Wind.West)
                    {
                        bWestFound = true;
                        if (oBlock.Type == Mentsu.Jantou)
                        {
                            bJantouFound = true;
                        }
                    }
                    if (WindTileToEnum(oBlock.Tiles[0]) == Wind.North)
                    {
                        bNorthFound = true;
                        if (oBlock.Type == Mentsu.Jantou)
                        {
                            bJantouFound = true;
                        }
                    }
                }
            }
            return bEastFound && bSouthFound && bWestFound && bNorthFound && bJantouFound;
        }

        public Boolean IsTsuuiisou(Hand poHand, List<Block> poBlockCombination)
        {
            foreach (Tile oTile in poHand.Tiles)
            {
                if (!_TilesManager.IsHonorTile(oTile))
                {
                    return false;
                }
            }
            return true;
        }

        public Boolean IsChinroutou(Hand poHand, List<Block> poBlockCombination)
        {
            foreach (Tile oTile in poHand.Tiles)
            {
                if (!_TilesManager.IsTerminalTile(oTile))
                {
                    return false;
                }
            }
            return true;
        }

        public Boolean IsRyuuiisou(Hand poHand, List<Block> poBlockCombination)
        {
            List<Tile> oRyuuiisouTileList = _HandParser.ParseTileStringToTileList("23468s6z");
            foreach (Tile oTile in poHand.Tiles)
            {
                if (!oRyuuiisouTileList.Contains(oTile))
                {
                    return false;
                }
            }
            return true;
        }

        public Boolean IsChuurenPoutou(Hand poHand, List<Block> poBlockCombination)
        {
            int[] oChuurenArrayCount = new int[9];
            List<Suit> oSuitList = _TilesManager.GetSuitsFromTileList(poHand.Tiles);
            Boolean bTileRemainderFound = false;
            if(oSuitList.Count != 1)
            {
                return false;
            }
            if (oSuitList[0] == Suit.Honor)
            {
                return false;
            }
            foreach (Tile oTile in poHand.Tiles)
            {
                oChuurenArrayCount[oTile.num - 1]++;
                if(oTile.num == 1 || oTile.num == 9)
                {
                    if(oChuurenArrayCount[oTile.num - 1] == 4)
                    {
                        if(bTileRemainderFound)
                        {
                            return false;
                        }
                        bTileRemainderFound = true;
                    }
                }
                else
                {
                    if (oChuurenArrayCount[oTile.num - 1] == 2)
                    {
                        if (bTileRemainderFound)
                        {
                            return false;
                        }
                        bTileRemainderFound = true;
                    }
                }
            }
            return true;
        }

        public Boolean IsSuukantsu(Hand poHand, List<Block> poBlockCombination)
        {
            int count = 0;
            foreach (Block ooBlock in poHand.LockedBlocks)
            {
                if (ooBlock.Type == Mentsu.Kantsu)
                {
                    count++;
                }
            }
            return count == 4;
        }

        #endregion

        #region Regular Yaku Evaluator Functions

        public Boolean IsMenzenTsumo(Hand poHand, List<Block> poBlockCombination)
        {
            if (IsHandClosedAtTenpai(poHand) && poHand.Agari == Agari.Tsumo)
            {
                return true;
            }
            return false;
        }
        public Boolean IsPinfu(Hand poHand, List<Block> poBlockCombination)
        {
            if (!IsHandClosedAtTenpai(poHand))
            {
                return false;
            }

            Boolean bWinTileIsInRyanmen = false;

            foreach (Block oBlock in poBlockCombination)
            {
                switch (oBlock.Type)
                {
                    case Mentsu.Koutsu:
                        return false;
                    case Mentsu.Kantsu:
                        return false;
                    case Mentsu.Jantou:
                        if (_TilesManager.IsDragonTile(oBlock.Tiles[0]))
                        {
                            return false;
                        }
                        if (_TilesManager.IsWindTile(oBlock.Tiles[0]) && (WindTileToEnum(oBlock.Tiles[0]) == poHand.RoundWind || WindTileToEnum(oBlock.Tiles[0]) == poHand.SeatWind))
                        {
                            return false;
                        }
                        break;
                    case Mentsu.Shuntsu:
                        if (bWinTileIsInRyanmen == false && (oBlock.Tiles[0].CompareTo(poHand.WinTile) == 0 || oBlock.Tiles[2].CompareTo(poHand.WinTile) == 0))
                        {
                            bWinTileIsInRyanmen = true;
                        }
                        break;
                    default:
                        return false;
                }
            }
            if (bWinTileIsInRyanmen)
            {
                return true;
            }
            return false;
        }
        
        public Boolean IsTanyao(List<Block> poBlockCombination)
        {
            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                Block oBlock = poBlockCombination[i];
                for (int j = 0; j < oBlock.Tiles.Count; j++)
                {
                    if (oBlock.Tiles[j].num == 1 || oBlock.Tiles[j].num == 9 || oBlock.Tiles[j].suit == "z")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Boolean IsJunchan(List<Block> poBlockCombination)
        {
            //If it's all Koutsu, that's a yakuman.
            Boolean bHasShuntsu = false;
            
            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                Block oBlock = poBlockCombination[i];
                if(oBlock.Type == Mentsu.Shuntsu)
                {
                    bHasShuntsu = true;
                }
                Boolean bHasATerminal = false;
                for(int j = 0; j < oBlock.Tiles.Count; j++)
                {
                    if((oBlock.Tiles[j].num == 1 || oBlock.Tiles[j].num == 9) && oBlock.Tiles[j].suit != "z")
                    {
                        bHasATerminal = true;
                        break;
                    }
                }
                if (!bHasATerminal)
                {
                    return false;
                }
            }
            if (bHasShuntsu)
            {
                return true;
            }
            return false;
        }

        public Boolean IsChanta(List<Block> poBlockCombination)
        {
            //If it's all Koutsu, that's honroutou
            Boolean bHasShuntsu = false;
            Boolean bHasHonorBlock = false;

            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                Block oBlock = poBlockCombination[i];
                if (oBlock.Type == Mentsu.Shuntsu)
                {
                    bHasShuntsu = true;
                }

                if(oBlock.Tiles[0].suit == "z")
                {
                    bHasHonorBlock = true;
                }
                else
                {
                    Boolean bHasATerminal = false;
                    for (int j = 0; j < oBlock.Tiles.Count; j++)
                    {
                        if ((oBlock.Tiles[j].num == 1 || oBlock.Tiles[j].num == 9) && oBlock.Tiles[j].suit != "z")
                        {
                            bHasATerminal = true;
                            break;
                        }
                    }
                    if (!bHasATerminal)
                    {
                        return false;
                    }
                }

               
            }
            if (bHasShuntsu && bHasHonorBlock)
            {
                return true;
            }
            return false;
        }

        public Boolean IsSanshokuDoujun(List<Block> poBlockCombination)
        {            
            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                Block oBlock = poBlockCombination[i];
                if(oBlock.Type == Mentsu.Shuntsu)
                {
                    int nNumToCompare = oBlock.Tiles[0].num;
                    Boolean bPinzuFound = false;
                    Boolean bSouzuFound = false;
                    Boolean bManzuFound = false;

                    bPinzuFound = oBlock.Tiles[0].suit == "p" ? true : false;
                    bSouzuFound = oBlock.Tiles[0].suit == "s" ? true : false;
                    bManzuFound = oBlock.Tiles[0].suit == "m" ? true : false;

                    for (int j = i + 1; j < poBlockCombination.Count; j++)
                    {
                        Block oBlock2 = poBlockCombination[j];
                        if (oBlock2.Type == Mentsu.Shuntsu && oBlock2.Tiles[0].num == nNumToCompare)
                        {
                            bPinzuFound = oBlock2.Tiles[0].suit == "p" && bPinzuFound == false ? true : false;
                            bSouzuFound = oBlock2.Tiles[0].suit == "s" && bSouzuFound == false ? true : false;
                            bManzuFound = oBlock2.Tiles[0].suit == "m" && bManzuFound == false ? true : false;
                        }
                    }
                    if(bPinzuFound && bSouzuFound && bManzuFound)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Boolean IsIttsuu(List<Block> poBlockCombination)
        {
            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                Block oBlock = poBlockCombination[i];
                if (oBlock.Type == Mentsu.Shuntsu)
                {
                    string nSuitToCompare = oBlock.Tiles[0].suit;
                    Boolean b1Found = false;
                    Boolean b4Found = false;
                    Boolean b7Found = false;

                    b1Found = oBlock.Tiles[0].num == 1 ? true : false;
                    b4Found = oBlock.Tiles[0].num == 4 ? true : false;
                    b7Found = oBlock.Tiles[0].num == 7 ? true : false;

                    for (int j = i + 1; j < poBlockCombination.Count; j++)
                    {
                        Block oBlock2 = poBlockCombination[j];
                        if (oBlock2.Type == Mentsu.Shuntsu && oBlock2.Tiles[0].suit == nSuitToCompare)
                        {
                            b1Found = oBlock2.Tiles[0].num == 1 && b1Found == false ? true : false;
                            b4Found = oBlock2.Tiles[0].num == 4 && b4Found == false ? true : false;
                            b7Found = oBlock2.Tiles[0].num == 7 && b7Found == false ? true : false;
                        }
                    }
                    if (b1Found && b4Found && b7Found)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Boolean IsRyanpeikou(Hand poHand, List<Block> poBlockCombination)
        {
            if (!IsHandClosedAtTenpai(poHand))
            {
                return false;
            }

            int nShuntsuBlocks = poBlockCombination.Count(n => n.Type == Mentsu.Shuntsu);

            if (_ShantenEvaluator.EvaluateShantenForChiitoi(poHand.Tiles) == -1 && nShuntsuBlocks == 4)
            {
                return true;
            }
            return false;
        }

        public Boolean IsChiitoi(Hand poHand, List<Block> poBlockCombination)
        {
            if (_ShantenEvaluator.EvaluateShantenForChiitoi(poHand.Tiles) == -1)
            {
                return true;
            }
            return false;
        }

        public Boolean IsIipeikou(Hand poHand, List<Block> poBlockCombination)
        {
            if (!IsHandClosedAtTenpai(poHand))
            {
                return false;
            }

            Boolean bIipeikouFound = false;

            for(int i = 0; i < poBlockCombination.Count; i++)
            {
                if(poBlockCombination[i].Type != Mentsu.Shuntsu)
                {
                    continue;
                }
                for(int j = i + 1; j < poBlockCombination.Count; j++)
                {
                    if (poBlockCombination[j].Type != Mentsu.Shuntsu)
                    {
                        continue;
                    }
                    if(poBlockCombination[i].Tiles[0].CompareTo(poBlockCombination[j].Tiles[0]) == 0)
                    {
                        if(bIipeikouFound == true)
                        {
                            return false;
                        }
                        bIipeikouFound = true;
                    }
                }
                if (bIipeikouFound)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsToitoi(Hand poHand, List<Block> poBlockCombination)
        {
            Boolean bHasOpenBlocks = false;

            if(poHand.LockedBlocks.Count > 0)
            {
                bHasOpenBlocks = true;
            }
            
            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                if (poBlockCombination[i].Type == Mentsu.Shuntsu)
                {
                    return false;
                }
                //Win on a ron will cause the winning tile's block to be open
                if(bHasOpenBlocks == false && (poBlockCombination[i].Type == Mentsu.Koutsu && poBlockCombination[i].Type == Mentsu.Kantsu) && poHand.Agari == Agari.Ron && poBlockCombination[i].Tiles[0].CompareTo(poHand.WinTile) == 0)
                {
                    bHasOpenBlocks = true;
                }
            }
            if (bHasOpenBlocks)
            {
                return true;
            }
            return false;
        }

        public Boolean IsHonroutou(Hand poHand, List<Block> poBlockCombination)
        {
            //All honors or all terminals is a yakuman instead
            Boolean bHasHonor = false;
            Boolean bHasTerminal = false;
            for (int i = 0; i < poHand.Tiles.Count; i++)
            {

                if ((poHand.Tiles[i].num != 1 || poHand.Tiles[i].num != 9) && poHand.Tiles[i].suit != "z") {
                    return false;
                }
                if ((poHand.Tiles[i].num == 1 || poHand.Tiles[i].num == 9) && poHand.Tiles[i].suit != "z")
                {
                    bHasTerminal = true;
                }
                if(poHand.Tiles[i].suit == "z")
                {
                    bHasHonor = true;
                }
            }
            return bHasTerminal && bHasHonor;
        }

        public Boolean IsSanshokuDoukou(Hand poHand, List<Block> poBlockCombination)
        {

            int nNum1 = 0;
            int nCount1 = 0;
            int nNum2 = 0;
            int nCount2 = 0;
            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                if (poBlockCombination[i].Type == Mentsu.Koutsu || poBlockCombination[i].Type == Mentsu.Kantsu)
                {
                    if(nNum1 == 0)
                    {
                        nNum1 = poBlockCombination[i].Tiles[0].num;
                        nCount1++;
                    }else if(nNum1 == poBlockCombination[i].Tiles[0].num)
                    {
                        nCount1++;
                    }else if (nNum2 == 0)
                    {
                        nNum2 = poBlockCombination[i].Tiles[0].num;
                        nCount2++;
                    }
                    else if (nNum2 == poBlockCombination[i].Tiles[0].num)
                    {
                        nCount2++;
                    }
                }
            }
            return nCount1 == 3 || nCount2 == 3;
        }

        public Boolean IsSankantsu(Hand poHand, List<Block> poBlockCombination)
        {
            int count = 0;
            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                if (poBlockCombination[i].Type == Mentsu.Kantsu)
                {
                    count++;
                }
            }
            return count == 3;
        }

        public Boolean IsSanankou(Hand poHand, List<Block> poBlockCombination)
        {
            int count = 0;
            ////These two booleans are to help the scenario: Ron on a shape like 11155577s333345p with 3p as the agari tile. 
            Boolean bHasKoutsuWithWinningTileOnRon = false;
            Boolean bHasShuntsuWithWinningTileOnRon = false;
            for (int i = 0; i < poBlockCombination.Count; i++)
            {
                if (poBlockCombination[i].Type == Mentsu.Koutsu || poBlockCombination[i].Type == Mentsu.Kantsu)
                {
                    if (poBlockCombination[i].IsOpen == false)
                    {
                        count++;
                    }
                    if (poBlockCombination[i].Tiles[0].CompareTo(poHand.WinTile) == 0 && poHand.Agari == Agari.Ron)
                    {
                        count--;
                        bHasKoutsuWithWinningTileOnRon = true;
                    }
                }
                if (poBlockCombination[i].Type == Mentsu.Shuntsu && (poBlockCombination[i].Tiles[0].num <= poHand.WinTile.num && poBlockCombination[i].Tiles[0].num + 2 >= poHand.WinTile.num) && poHand.Agari == Agari.Ron)
                {
                    bHasShuntsuWithWinningTileOnRon = true;
                }

            }
            if(bHasKoutsuWithWinningTileOnRon && bHasShuntsuWithWinningTileOnRon)
            {
                count++;
            }

            return count == 3;
        }

        public Boolean IsChinitsu(Hand poHand, List<Block> poBlockCombination)
        {
            List<Suit> oSuitList = _TilesManager.GetSuitsFromTileList(poHand.Tiles);
            if(oSuitList.Count == 1 && oSuitList[0] != Suit.Honor)
            {
                return true;
            }
            return false;
        }

        public Boolean IsHonitsu(Hand poHand, List<Block> poBlockCombination)
        {
            List<Suit> oSuitList = _TilesManager.GetSuitsFromTileList(poHand.Tiles);
            if (oSuitList.Count == 2 && oSuitList.Contains(Suit.Honor))
            {
                return true;
            }
            return false;
        }

        public Boolean IsChun(Hand poHand, List<Block> poBlockCombination)
        {
           foreach(Block oBlock in poBlockCombination)
            {
                if((oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu) && DragonTileToEnum(oBlock.Tiles[0]) == Dragon.Chun)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsHaku(Hand poHand, List<Block> poBlockCombination)
        {
            foreach (Block oBlock in poBlockCombination)
            {
                if ((oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu) && DragonTileToEnum(oBlock.Tiles[0]) == Dragon.Haku)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsHatsu(Hand poHand, List<Block> poBlockCombination)
        {
            foreach (Block oBlock in poBlockCombination)
            {
                if ((oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu) && DragonTileToEnum(oBlock.Tiles[0]) == Dragon.Hatsu)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsTon(Hand poHand, List<Block> poBlockCombination)
        {
            if(poHand.RoundWind != Wind.East && poHand.SeatWind != Wind.East)
            {
                return false;
            }
            foreach (Block oBlock in poBlockCombination)
            {
                if ((oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu) && WindTileToEnum(oBlock.Tiles[0]) == Wind.East)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsNan(Hand poHand, List<Block> poBlockCombination)
        {
            if (poHand.RoundWind != Wind.South && poHand.SeatWind != Wind.South)
            {
                return false;
            }
            foreach (Block oBlock in poBlockCombination)
            {
                if ((oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu) && WindTileToEnum(oBlock.Tiles[0]) == Wind.South)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsSha(Hand poHand, List<Block> poBlockCombination)
        {
            if (poHand.RoundWind != Wind.West && poHand.SeatWind != Wind.West)
            {
                return false;
            }
            foreach (Block oBlock in poBlockCombination)
            {
                if ((oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu) && WindTileToEnum(oBlock.Tiles[0]) == Wind.West)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsPei(Hand poHand, List<Block> poBlockCombination)
        {
            if (poHand.RoundWind != Wind.North && poHand.SeatWind != Wind.North)
            {
                return false;
            }
            foreach (Block oBlock in poBlockCombination)
            {
                if ((oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu) && WindTileToEnum(oBlock.Tiles[0]) == Wind.North)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean IsShousangen(Hand poHand, List<Block> poBlockCombination)
        {
            Boolean bChunFound = false;
            Boolean bHakuFound = false;
            Boolean bHatsuFound = false;
            Boolean bJantouFound = false;
            foreach (Block oBlock in poBlockCombination)
            {
                if ((oBlock.Type == Mentsu.Koutsu || oBlock.Type == Mentsu.Kantsu || oBlock.Type == Mentsu.Jantou))
                {
                    if(DragonTileToEnum(oBlock.Tiles[0]) == Dragon.Chun)
                    {
                        bChunFound = true;
                        if(oBlock.Type == Mentsu.Jantou)
                        {
                            bJantouFound = true;
                        }
                    }
                    if (DragonTileToEnum(oBlock.Tiles[0]) == Dragon.Haku)
                    {
                        bHakuFound = true;
                        if (oBlock.Type == Mentsu.Jantou)
                        {
                            bJantouFound = true;
                        }
                    }
                    if (DragonTileToEnum(oBlock.Tiles[0]) == Dragon.Hatsu)
                    {
                        bHatsuFound = true;
                        if (oBlock.Type == Mentsu.Jantou)
                        {
                            bJantouFound = true;
                        }
                    }
                }
            }
            return bChunFound && bHakuFound && bHatsuFound && bJantouFound;
        }

        #endregion

        #endregion

        private Boolean IsHandClosedAtTenpai(Hand poHand)
        {
            foreach(Block oBlock in poHand.LockedBlocks)
            {
                if(oBlock.Type != Mentsu.Kantsu)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
