using Mahjong.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong
{
    public class CUkiereEvaluator
    {

        private CShantenEvaluator _ShantenEvaluator;
        private CTilesManager _TilesManager;

        public static List<string> AllTiles = new List<string>()
        {
            "1p","2p","3p","4p","5p","6p","7p","8p","9p",
            "1s","2s","3s","4s","5s","6s","7s","8s","9s",
            "1m","2m","3m","4m","5m","6m","7m","8m","9m",
            "1z","2z","3z","4z","5z","6z","7z"
        };

        public CUkiereEvaluator()
        {
            _ShantenEvaluator = new CShantenEvaluator();
            _TilesManager = new CTilesManager();
        }

        public List<Tile> EvaluateUkiere(List<Tile> poTileList)
        {
            List<Tile> oUkiereList = new List<Tile>();
            if(poTileList.Count != 13)
            {
                return oUkiereList;
            }

            int nCurrentShanten = _ShantenEvaluator.EvaluateShanten(poTileList);

            foreach (string sTile in AllTiles)
            {
                List<Tile> oTempTileList = _TilesManager.Clone(poTileList);
                Tile oTile = new Tile(sTile);
                if(_TilesManager.CountNumberOfTilesOf(oTempTileList,sTile) < 4)
                {
                    oTempTileList.Add(oTile);
                    int nNewShanten = _ShantenEvaluator.EvaluateShanten(oTempTileList);
                    if (nNewShanten < nCurrentShanten)
                    {
                        oUkiereList.Add(oTile);
                    }
                }
            }
            return oUkiereList;
        }
    }
}
