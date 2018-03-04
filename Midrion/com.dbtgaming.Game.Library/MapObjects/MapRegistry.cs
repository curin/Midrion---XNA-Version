using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.dbtgaming.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace com.dbtgaming.Game.Library.MapObjects
{
    public class MapRegistry
    {
        private static List<MapData> _data;

        /// <summary>
        /// Data Files loaded which contain all map data
        /// </summary>
        public static List<MapData> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public static Map2 GetMap(string MapID)
        {
            Map2 mp = new Map2();
            mp.MapID = MapID;
            foreach (MapData dat in Data)
            {
                if (dat.Maps.ContainsKey(MapID))
                {
                    foreach (string key in dat.Maps[MapID].Regions.Keys)
                    {
                        if (!mp.Regions.ContainsKey(key))
                            mp.Regions.Add(key, GetRegionData(MapID + "." + key));
                    }
                }
            }
            return mp;
        }

        public static MagicTile[,] GenerateMagicTiles(Map2 Map)
        {
            MagicTile[,] tiles = new MagicTile[1, 1];
            tiles.Initialize();
            List<Point> PoPLocs = new List<Point>();
            MagicTile[,] temp;
            MagicTile temptile;
            int largestX = 0;
            int largestY = 0;
            foreach (MapRegion region in Map.Regions.Values)
            {
                if (largestX < region.Position.X + region.Size.X / 10 || largestY < region.Position.Y + region.Size.Y / 10)
                {
                    if (largestX < region.Position.X + region.Size.X / 10)
                        largestX = (int)(region.Position.X + region.Size.X / 10);

                    if (largestY < region.Position.Y + region.Size.Y / 10)
                        largestY = (int)(region.Position.Y + region.Size.Y / 10);

                    temptile = new MagicTile();
                    temp = new MagicTile[largestX, largestY];
                    temp.Initialize();
                    Array.Copy(tiles, temp, tiles.LongLength);
                    tiles = temp;
                }

                foreach (PlaceOfPower pop in region.PoPs)
                {
                    if (largestX < ((int)region.Position.X + pop.Location.X) / 10 || largestY < ((int)region.Position.Y + pop.Location.Y) / 10)
                    {
                        if (largestX < ((int)region.Position.X + pop.Location.X) / 10)
                            largestX = ((int)region.Position.X + pop.Location.X) / 10;

                        if (largestY < ((int)region.Position.Y + pop.Location.Y) / 10)
                            largestY = ((int)region.Position.Y + pop.Location.Y) / 10;

                        temp = new MagicTile[largestX, largestY];
                        Array.Copy(tiles, temp, tiles.LongLength);
                        tiles = temp;
                    }
                    temptile = new MagicTile();
                    temptile.MainMagic = pop.PlaceType;
                    temptile.MagicLevels = new Dictionary<PoPType, byte>();
                    temptile.MagicLevels.Add(pop.PlaceType, pop.Strength);
                    tiles[((int)region.Position.X + pop.Location.X) / 10, ((int)region.Position.Y + pop.Location.Y) / 10] = temptile;
                    PoPLocs.Add(new Point(pop.Location.X + (int)region.Position.X, (int)region.Position.Y + pop.Location.Y));
                }
            }

            List<int> dist = new List<int>();
            int i;
            int lowest = -1;
            float low2 = -1;
            List<PoPDistDat> closePoPs = new List<PoPDistDat>();
            PoPDistDat tempdist;
            float totaldist = 0;
            for (int x = 0; x < largestX; x++)
                for (int y = 0; y < largestY; y++)
                {
                    if (tiles[x, y].MainMagic == new MagicTile().MainMagic)
                    {
                        foreach (Point pnt in PoPLocs)
                        {
                            i = (int)Math.Sqrt((((pnt.X / 10) - x) * ((pnt.X / 10) - x)) + (((pnt.Y / 10) - y) * ((pnt.Y / 10) - y)));
                            dist.Add(i);
                            if (lowest == -1)
                                lowest = i;
                            else if (i < lowest)
                                lowest = i;
                            if (i > lowest && low2 == -1)
                                low2 = i;
                            else if (low2 > i && i != lowest)
                                low2 = i;
                        }

                        
                        for (int index = 0; index < dist.Count; index++)
                        {
                            if (dist[index] <= lowest * 3)
                            {
                                tempdist = new PoPDistDat();
                                tempdist.Distance = dist[index];
                                tempdist.Location = PoPLocs[index];
                                if (dist[index] == lowest)
                                    tempdist.Lowest = true;
                                closePoPs.Add(tempdist);
                                totaldist += dist[index];
                            }
                        }

                        temptile = new MagicTile();
                        temptile.MagicLevels = new Dictionary<PoPType, byte>();
                        foreach (PoPDistDat dat in closePoPs)
                        {
                            if (dat.Lowest)
                            {
                                temptile.MainMagic = tiles[((int)dat.Location.X) / 10, ((int)dat.Location.Y) / 10].MainMagic;
                            }
                            foreach (PoPType pop in tiles[((int)dat.Location.X) / 10, ((int)dat.Location.Y) / 10].MagicLevels.Keys)
                            {
                                if (totaldist == dat.Distance)
                                {
                                    if (temptile.MagicLevels.ContainsKey(pop))
                                        temptile.MagicLevels[pop] += (byte)((((lowest + low2) - dat.Distance)/ (lowest + low2)) * tiles[((int)dat.Location.X) / 10, ((int)dat.Location.Y) / 10].MagicLevels[pop]);
                                    else
                                        temptile.MagicLevels.Add(pop, (byte)((((lowest + low2) - dat.Distance) / (lowest + low2)) * tiles[((int)dat.Location.X) / 10, ((int)dat.Location.Y) / 10].MagicLevels[pop]));
                                }
                                else
                                {
                                    if (temptile.MagicLevels.ContainsKey(pop))
                                        temptile.MagicLevels[pop] += (byte)(((totaldist - dat.Distance) / totaldist) * tiles[((int)dat.Location.X) / 10, ((int)dat.Location.Y) / 10].MagicLevels[pop]);
                                    else
                                        temptile.MagicLevels.Add(pop, (byte)(((totaldist - dat.Distance) / totaldist) * tiles[((int)dat.Location.X) / 10, ((int)dat.Location.Y) / 10].MagicLevels[pop]));
                                }
                            }
                        }
                        tiles[x, y] = temptile;
                    }
                    lowest = -1;
                    dist = new List<int>();
                    closePoPs = new List<PoPDistDat>();
                    totaldist = 0;
                    low2 = -1;
                }
            return tiles;
        }

        private static MapRegion _tempRegion;

        public static MapRegion GetRegionData(string RegionID)
        {
            string mapid = RegionID.Split('.')[0];
            string region = RegionID.Split('.')[1];
            MapRegion mr = new MapRegion();

            foreach (MapData dat in Data)
            {
                if (dat.Maps.ContainsKey(mapid))
                {
                    if (dat.Maps[mapid].Regions.ContainsKey(region))
                    {
                        if (dat.Maps[mapid].Regions[region].OriginalRegion == mapid + "." + region)
                        {
                            _tempRegion = dat.Maps[mapid].Regions[region];
                            mr.Changes.AddRange(_tempRegion.Changes);
                            foreach (string st in _tempRegion.Doors.Keys)
                            {
                                mr.Doors.Add(st, _tempRegion.Doors[st]);
                            }

                            foreach (string key in _tempRegion.RegionObjects.Keys)
                            {
                                mr.RegionObjects.Add(key, _tempRegion.RegionObjects[key]);
                            }

                            foreach (ISprite2 sprite in _tempRegion.SpritesUsed)
                            {
                                if (!mr.SpritesUsed.Contains(sprite))
                                    mr.SpritesUsed.Add(sprite);
                            }
                        }
                        else
                        {
                            mr = dat.Maps[mapid].Regions[region];
                        }
                    }
                }
            }
            return mr;
        }
    }

    public struct PoPDistDat
    {
        private Point _location;
        private int _dist;
        private bool _lowest;

        public Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public int Distance
        {
            get
            {
                return _dist;
            }
            set
            {
                _dist = value;
            }
        }

        public bool Lowest
        {
            get
            {
                return _lowest;
            }
            set
            {
                _lowest = value;
            }
        }
    }

    public struct Map2
    {
        private string _mapID;
        private Dictionary<string, MapRegion> _regions;

        /// <summary>
        /// Full id of map (MapID)
        /// If this is the same as a previously loaded MapID, it will be considered a modification.
        /// </summary>
        public string MapID
        {
            get
            {
                return _mapID;
            }
            set
            {
                _mapID = value;
            }
        }

        /// <summary>
        /// Regions of map with RegionID as key (RegionID) 
        /// </summary>
        public Dictionary<string, MapRegion> Regions
        {
            get
            {
                return _regions;
            }
            set
            {
                _regions = value;
            }
        }
    }

    public struct MapData
    {
        private Dictionary<string, Map2> _maps;

        /// <summary>
        /// Maps found in Datafile using Mapid as key (MapID)
        /// </summary>
        public Dictionary<string, Map2> Maps
        {
            get
            {
                return _maps;
            }
            set
            {
                _maps = value;
            }
        }   
    }
}
