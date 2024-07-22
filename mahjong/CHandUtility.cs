using Mahjong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static Mahjong.Enums;

namespace Mahjong
{
    public class CHandUtility
    {
        private CTilesManager _TilesManager;        

        public CHandUtility()
        {
            _TilesManager = new CTilesManager();
        }

        //
        public Hand Clone(Hand poHand)
        {
            Hand oHand = new Hand();

            foreach(Tile oTile in poHand.Tiles)
            {
                oHand.Tiles.Add(new Tile(oTile));
            }

            foreach (Tile oTile in poHand.DiscardedTiles)
            {
                oHand.DiscardedTiles.Add(new Tile(oTile));
            }

            foreach (Block oBlock in poHand.LockedBlocks)
            {
                Block oNewBlock = new Block();
                foreach(Tile oTile in oBlock.Tiles)
                {
                    oNewBlock.Tiles.Add(new Tile(oTile));
                }
                oNewBlock.Type = oBlock.Type;
                oNewBlock.IsOpen = oBlock.IsOpen;
                oNewBlock.KanType = oBlock.KanType;
                oHand.LockedBlocks.Add(oNewBlock);
            }

            oHand.IsIppatsu = poHand.IsIppatsu;
            oHand.Agari = poHand.Agari;
            oHand.SeatWind = poHand.SeatWind;
            oHand.RoundWind = poHand.RoundWind;
            oHand.IsRiichi = poHand.IsRiichi;
            oHand.IsDoubleRiichi = poHand.IsDoubleRiichi;
            oHand.IsIppatsu = poHand.IsIppatsu;
            oHand.IsRinshan = poHand.IsRinshan;
            oHand.IsChankan = poHand.IsChankan;
            oHand.IsHaitei = poHand.IsHaitei;
            oHand.IsHoutei = poHand.IsHoutei;
            oHand.DoraCount = poHand.DoraCount;
            oHand.AkaDoraCount = poHand.AkaDoraCount;
            oHand.UraDoraCount = poHand.UraDoraCount;

            return oHand;
            
        }
       
    }
}
