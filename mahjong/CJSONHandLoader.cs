using Mahjong.Model;
using System;
using System.IO;
using static Mahjong.Enums;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mahjong
{
    public class CJSONHandLoader
    {
        private CHandParser _HandParser;
        private CBlockParser _BlockParser;
        private CTilesManager _TilesManager;
        public CJSONHandLoader()
        {
            _HandParser = new CHandParser();
            _BlockParser = new CBlockParser();
            _TilesManager = new CTilesManager();
        }

        public Hand CreateHandFromJSONFile(string psJSONFilePath)
        {
            string json = File.ReadAllText(psJSONFilePath);
            return CreateHandFromJSONString(json);
        }

        public List<Hand> CreateHandsFromJSONFile(string psJSONFilePath)
        {
            string json = File.ReadAllText(psJSONFilePath);
            return CreateHandsFromJSONString(json);
        }

        public Hand CreateHandFromJSONString(string psJSON)
        {
            var dynamicJson = JsonConvert.DeserializeObject<dynamic>(psJSON);
            return CreateHandFromJSONObject(dynamicJson);
        }

        public List<Hand> CreateHandsFromJSONString(string psJSON)
        {
            List<Hand> oHandList = new List<Hand>();
            var dynamicJson = JsonConvert.DeserializeObject<dynamic>(psJSON);
            if(dynamicJson == null)
            {
                return oHandList;
            }
            if (dynamicJson.Hands == null)
            {
                return oHandList;
            }
            foreach(var jsonHand in dynamicJson.Hands)
            {
                oHandList.Add(CreateHandFromJSONObject(jsonHand));
            }
            return oHandList;
        }

        public Hand CreateHandFromJSONObject(dynamic dynamicJson)
        {
            Hand oHand = new Hand();

            if(dynamicJson == null)
            {
                return oHand;
            }

            String sHand = "";
            List<String> oOpenLockedBlocks = new List<String>();
            List<String> oClosedLockedBlocks = new List<String>();
            Tile oWinTile = null;
            Agari eAgari = Agari.Unknown;
            Wind eSeatWind = Wind.Unknown;
            Wind eRoundWind = Wind.Unknown;
            Boolean bIsRiichi = false;
            Boolean bIsDoubleRiichi = false;
            Boolean bIsIppatsu = false;
            Boolean bIsRinshan = false;
            int nDoraCount = 0;

            

            if (dynamicJson.Hand != null)
            {
                sHand = dynamicJson.Hand;
            }
            if (dynamicJson.LockedBlocks != null)
            {
                if (dynamicJson.LockedBlocks.OpenBlocks != null)
                {
                    oOpenLockedBlocks = dynamicJson.LockedBlocks.OpenBlocks.ToObject<List<String>>();
                }
                if (dynamicJson.LockedBlocks.ClosedBlocks != null)
                {
                    oClosedLockedBlocks = dynamicJson.LockedBlocks.ClosedBlocks.ToObject<List<String>>();
                }
            }
            if (dynamicJson.WinTile != null)
            {
                oWinTile = new Tile((string)dynamicJson.WinTile);
            }
            if (dynamicJson.Agari != null)
            {
                eAgari = AgariStringToEnum((string)dynamicJson.Agari);
            }
            if (dynamicJson.SeatWind != null)
            {
                eSeatWind = WindStringToEnum((string)dynamicJson.SeatWind);
            }
            if (dynamicJson.RoundWind != null)
            {
                eRoundWind = WindStringToEnum((string)dynamicJson.RoundWind);
            }
            if (dynamicJson.IsRiichi != null)
            {
                bIsRiichi = (Boolean)dynamicJson.IsRiichi;
            }
            if (dynamicJson.IsDoubleRiichi != null)
            {
                bIsDoubleRiichi = (Boolean)dynamicJson.IsDoubleRiichi;
            }
            if (dynamicJson.IsIppatsu != null)
            {
                bIsIppatsu = (Boolean)dynamicJson.IsIppatsu;
            }
            if (dynamicJson.IsRinshan != null)
            {
                bIsRinshan = (Boolean)dynamicJson.IsRinshan;
            }
            if (dynamicJson.DoraCount != null)
            {
                nDoraCount = (int)dynamicJson.DoraCount;
            }

            oHand.Tiles = _HandParser.ParseTileStringToTileList(sHand);
            foreach(String sBlock in oOpenLockedBlocks)
            {
                oHand.LockedBlocks.Add(_BlockParser.ParseBlock(sBlock, true));
            }
            foreach (String sBlock in oClosedLockedBlocks)
            {
                oHand.LockedBlocks.Add(_BlockParser.ParseBlock(sBlock, true));
            }
            oHand.WinTile = oWinTile;
            oHand.Agari = eAgari;
            oHand.RoundWind = eRoundWind;
            oHand.SeatWind = eSeatWind;
            oHand.IsRiichi = bIsRiichi;
            oHand.IsDoubleRiichi = bIsDoubleRiichi;
            oHand.IsRinshan = bIsRinshan;
            oHand.IsIppatsu = bIsIppatsu;
            oHand.DoraCount = nDoraCount;

            _TilesManager.SortTiles(oHand.Tiles);

            return oHand;
        }
    }
}
