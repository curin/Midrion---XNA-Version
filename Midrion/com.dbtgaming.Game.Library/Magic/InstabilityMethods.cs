using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace com.dbtgaming.Game.Library.Magic
{
    public class InstabilityMethods
    {
        static int BasicMethod(Random rand, int instabilityPoints, IShape shpe)
        {
            return 0;
        }

        /// <summary>
        /// Move point method for instability
        /// </summary>
        /// <param name="rand">random used within magic manager</param>
        /// <param name="instabilityPoints">instability points available for this IShape</param>
        /// <param name="shpe">IShape with instability</param>
        /// <returns>a number</returns>
        public static int MovePoint(Random rand, int instabilityPoints, IShape shpe)
        {
            int originpnt = rand.Next(0, shpe.Points.Count - 1);
            double angle = rand.NextDouble() * 2 * Math.PI;
            int distance = 1;
            if (instabilityPoints >= 2560)
                distance = rand.Next(1, instabilityPoints / 2560);
            instabilityPoints -= distance * 2560;
            distance *= 10;
            Vector2 location = new Vector2((float)Math.Cos(angle) * distance, (float)Math.Sin(angle) * distance);
            location += shpe.Points[originpnt].Location;
            shpe.Points[originpnt]._moveToLocation = location;
            int speed = 1;
            Vector2 normchange = location - shpe.Points[originpnt].Location;
            normchange.Normalize();
            Vector2 change = location - shpe.Points[originpnt].Location;
            if (instabilityPoints >= 2560)
                speed = rand.Next(1, instabilityPoints / 2560);
            if (speed > change.X / normchange.X)
                speed = (int)change.X / (int)normchange.X;
            instabilityPoints -= speed * 2560;
            int speedy = 1;
            if (instabilityPoints >= 2560)
                speedy = rand.Next(1, instabilityPoints / 2560);
            if (speedy > change.Y / normchange.Y)
                speedy = (int)change.Y / (int)normchange.Y;
            instabilityPoints -= speedy * 2560;
            shpe.Points[originpnt]._speed = new Vector2(speed, speedy);
            return 0;
        }

        /// <summary>
        /// Add connection from instability
        /// </summary>
        /// <param name="rand">random used within magic manager</param>
        /// <param name="instabilityPoints">instability points available for this IShape</param>
        /// <param name="shpe">IShape with instability</param>
        /// <returns>a number</returns>
        public static int AddConnection(Random rand, int instabilityPoints, IShape shpe)
        {
            int pointtoconnect = rand.Next(0, shpe.Points.Count - 1);
            List<int> taken = new List<int>();
            //foreach (MagicPointConnection con2 in shpe.Connections)
            //{
            //    if (con2.PointIndex1 == pointtoconnect)
            //    {
            //        taken.Add(con2.PointIndex2);
            //    }
            //    if (con2.PointIndex2 == pointtoconnect)
            //    {
            //        taken.Add(con2.PointIndex1);
            //    }
            //}
            int max = ((int)Math.Pow(2, shpe.Points.Count - 1 - taken.Count));
            if (max > 0)
            {
                int connectionrand = rand.Next(0, max);
                double connectionsdub = Math.Log(connectionrand, 2);
                int connections = max - (int)Math.Round(connectionsdub, MidpointRounding.AwayFromZero);
                if (connections >= shpe.Points.Count || connections > connections * 8192)
                    connections = 1;
                //MagicPointConnection con = new MagicPointConnection();
                for (int i = 0; connections > i; i++)
                {
                    int point = rand.Next(0, shpe.Points.Count - 1 - i);
                    while (taken.Contains(point) && taken.Count < shpe.Points.Count)
                    {
                        if (point < shpe.Points.Count - 1)
                            point++;
                        else
                            point = 0;
                    }
                    //con = new MagicPointConnection(shpe.Points[pointtoconnect].Location, shpe.Points[point]._location, PointConnectionType.Straight, shpe.Points.Count - 1, point);
                    //shpe.Connections.Add(con);
                    taken.Add(point);
                }
                instabilityPoints -= connections * 8192;
            }
            else
            {
                instabilityPoints += 4096;
            }
            return 0;
        }

        /// <summary>
        /// add point from instability
        /// </summary>
        /// <param name="rand">random used within magic manager</param>
        /// <param name="instabilityPoints">instability points available for this IShape</param>
        /// <param name="shpe">IShape with instability</param>
        /// <returns>a number</returns>
        public static int AddPoint(Random rand, int instabilityPoints, IShape shpe)
        {
            int originpnt = rand.Next(0, shpe.Points.Count - 1);
            double angle = rand.NextDouble() * 2 * Math.PI;
            int distance = 1;
            if (instabilityPoints >= 1024)
                distance = rand.Next(1, instabilityPoints / 1024);
            instabilityPoints -= distance * 1024;
            distance *= 10;
            MagicPoint pnt = new MagicPoint();
            Vector2 location = new Vector2((float)Math.Cos(angle) * distance, (float)Math.Sin(angle) * distance);
            pnt.Location = shpe.Points[originpnt].Location + location;
            pnt.Connections = new List<int>();
            pnt.FrameMod = rand.Next(0, 5);
            pnt.RotationMod = (float)(2 * Math.PI * rand.NextDouble());
            pnt._moveToLocation = pnt.Location;
            pnt._rotdir = rand.Next(0, 2);
            if (pnt._rotdir == 0)
                pnt._rotdir = -1;
            else
                pnt._rotdir = 1;
            shpe.Points.Add(pnt);
            if (instabilityPoints > 16384)
            {
                if (rand.Next(0, 5) == 0)
                {
                    int max = ((int)Math.Pow(2, shpe.Points.Count - 1));
                    if (max > 0)
                    {
                        int connectionrand = rand.Next(0, max);
                        double connectionsdub = Math.Log(connectionrand, 2);
                        int connections = shpe.Points.Count - 1 - (int)Math.Round(connectionsdub, MidpointRounding.AwayFromZero);
                        List<int> taken = new List<int>();
                        if (connections >= shpe.Points.Count)
                            connections = 1;
                        //MagicPointConnection con = new MagicPointConnection();
                        for (int i = 0; connections > i; i++)
                        {
                            int point = rand.Next(0, shpe.Points.Count - 2 - i);
                            while (taken.Contains(point))
                            {
                                if (point < shpe.Points.Count - 2)
                                    point++;
                                else
                                    point = 0;
                            }
                            //con = new MagicPointConnection(location, shpe.Points[point]._location, PointConnectionType.Straight, shpe.Points.Count - 1, point);
                            //shpe.Connections.Add(con);
                            taken.Add(point);
                        }
                        instabilityPoints -= connections * 16384;
                    }
                }
            }
            return 0;
        }
    }
}
