using System;
using System.Collections.Generic;
using System.Text;
using static Mahjong.Enums;
using Mahjong.Model;

namespace Mahjong
{
    public class CTableManager
    {

        public CTableManager()
        {

        }
        
        public void InitializeTable(Table poTable)
        {
            if(poTable == null)
            {
                throw new ArgumentNullException(nameof(poTable));
            }
            WallConfiguration oWallConfig = new WallConfiguration();
            LoadWall(poTable, oWallConfig);
        }

        public void DrawTileFromWallToHand(Table poTable, Hand poHand, int pnNumTiles)
        {
            if (poTable == null)
            {
                throw new ArgumentNullException(nameof(poTable));
            }
            if (poHand == null)
            {
                throw new ArgumentNullException(nameof(poHand));
            }
            if (pnNumTiles <= 0 || !CanDrawFromWall(poTable))
            {
                return;
            }
            int nMinNumTilesToDraw = Math.Min(pnNumTiles, poTable.Wall.Count - 14);
            for (int i = 0; i < nMinNumTilesToDraw; i++)
            {
                poHand.Tiles.Add(poTable.Wall[0]);
                poTable.Wall.RemoveAt(0);
            }
        }

        public Boolean CanDrawFromWall(Table poTable)
        {
            if (poTable == null)
            {
                throw new ArgumentNullException(nameof(poTable));
            }
            return poTable.Wall.Count - 14 > 0;
        }

        private void LoadWall(Table poTable, WallConfiguration poWallConfig)
        {
            if (poTable == null)
            {
                throw new ArgumentNullException(nameof(poTable));
            }
            if (poWallConfig == null)
            {
                poWallConfig = new WallConfiguration();
            }
            List<Tile> oTilesToLoad = new List<Tile>();
            Random rand = new Random();
            #region PopulateTilesToLoad
            for (int i = 1; i <= 9; i++)
            {
                if (poWallConfig.LoadPinzuTerminalsOnly && (i == 1 || i == 9))
                {
                    oTilesToLoad.Add(new Tile(i.ToString() + "p"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "p"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "p"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "p"));
                }
                else if (poWallConfig.LoadPinzu)
                {
                    oTilesToLoad.Add(new Tile(i.ToString() + "p"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "p"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "p"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "p"));
                }
                if (poWallConfig.LoadSouzuTerminalsOnly && (i == 1 || i == 9))
                {
                    oTilesToLoad.Add(new Tile(i.ToString() + "s"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "s"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "s"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "s"));
                }
                else if (poWallConfig.LoadSouzu)
                {
                    oTilesToLoad.Add(new Tile(i.ToString() + "s"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "s"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "s"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "s"));
                }
                if (poWallConfig.LoadManzuTerminalsOnly && (i == 1 || i == 9))
                {
                    oTilesToLoad.Add(new Tile(i.ToString() + "m"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "m"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "m"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "m"));
                }
                else if (poWallConfig.LoadManzu)
                {
                    oTilesToLoad.Add(new Tile(i.ToString() + "m"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "m"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "m"));
                    oTilesToLoad.Add(new Tile(i.ToString() + "m"));
                }
            }
            if (poWallConfig.LoadDragons)
            {
                oTilesToLoad.Add(new Tile("5z"));
                oTilesToLoad.Add(new Tile("5z"));
                oTilesToLoad.Add(new Tile("5z"));
                oTilesToLoad.Add(new Tile("5z"));
                oTilesToLoad.Add(new Tile("6z"));
                oTilesToLoad.Add(new Tile("6z"));
                oTilesToLoad.Add(new Tile("6z"));
                oTilesToLoad.Add(new Tile("6z"));
                oTilesToLoad.Add(new Tile("7z"));
                oTilesToLoad.Add(new Tile("7z"));
                oTilesToLoad.Add(new Tile("7z"));
                oTilesToLoad.Add(new Tile("7z"));
            }
            if (poWallConfig.LoadEast)
            {
                oTilesToLoad.Add(new Tile("1z"));
                oTilesToLoad.Add(new Tile("1z"));
                oTilesToLoad.Add(new Tile("1z"));
                oTilesToLoad.Add(new Tile("1z"));
            }
            if (poWallConfig.LoadSouzu)
            {
                oTilesToLoad.Add(new Tile("2z"));
                oTilesToLoad.Add(new Tile("2z"));
                oTilesToLoad.Add(new Tile("2z"));
                oTilesToLoad.Add(new Tile("2z"));
            }
            if (poWallConfig.LoadWest)
            {
                oTilesToLoad.Add(new Tile("3z"));
                oTilesToLoad.Add(new Tile("3z"));
                oTilesToLoad.Add(new Tile("3z"));
                oTilesToLoad.Add(new Tile("3z"));
            }
            if (poWallConfig.LoadNorth)
            {
                oTilesToLoad.Add(new Tile("4z"));
                oTilesToLoad.Add(new Tile("4z"));
                oTilesToLoad.Add(new Tile("4z"));
                oTilesToLoad.Add(new Tile("4z"));
            }
            #endregion

            #region RandomizeTilesInWall
            poTable.Wall.Clear();
            while(oTilesToLoad.Count > 0)
            {
                int rndIndex = rand.Next(0, oTilesToLoad.Count);
                poTable.Wall.Add(oTilesToLoad[rndIndex]);
                oTilesToLoad.RemoveAt(rndIndex);
            }
            #endregion

        }

    }
}
