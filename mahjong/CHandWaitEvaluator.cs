using Mahjong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static Mahjong.Enums;

namespace Mahjong
{
    public class CHandWaitEvaluator
    {
        private CShantenEvaluator _ShantenEvaluator;
        private CTilesManager _TilesManager;
        

        public CHandWaitEvaluator()
        {
            _ShantenEvaluator = new CShantenEvaluator();
            _TilesManager = new CTilesManager();
        }

        //Don't accept hands with 14 tiles. Only accept hands with tiles that are 13.
        public List<Tile> EvaluateWaits(Hand poHand)
        {
            List<Tile> oWaits = new List<Tile>();

            int nCurShanten = _ShantenEvaluator.EvaluateShanten(poHand);

            if (nCurShanten == -1)
            {
                return oWaits;
            }

            List<string> oHonorList = new List<string>() { "p", "s", "m", "z"};

            for(int i = 1; i < 10; i++)
            {
                
                foreach (string sHonor in oHonorList)
                {
                    if(sHonor == "z" && i > 7)
                    {
                        continue;
                    }
                    Tile oTempTile = new Tile(i + sHonor);
                    poHand.Tiles.Add(oTempTile);
                    int nNewShanten = _ShantenEvaluator.EvaluateShanten(poHand);

                    if (nNewShanten == nCurShanten - 1)
                    {
                        oWaits.Add(oTempTile);
                    }
                    _TilesManager.RemoveSingleTileOf(poHand.Tiles, oTempTile);
                }
            }

            



            return oWaits;
            
        }
       
    }
}
