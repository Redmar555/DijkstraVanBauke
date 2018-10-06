using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public  class Storage
    {
        List<Rek> Stored = new List<Rek>();
        World w;

        double Max_Barrels;
        double position_x;
        double position_y;
        double position_z;

        double x_size;
        double z_size;

        public Node DropoffNode;
        /// <summary>
        /// Storage area to store barrels
        /// </summary>
        /// <param name="n">The node the robot ahs to reach to drop off it's load. </param>
        /// <param name="x_size">Horizontal size</param>
        /// <param name="z_size">Vertical size</param>
        /// <param name="x_position">position</param>
        /// <param name="y_position">position</param>
        /// <param name="z_position">position</param>
        /// <param name="currentworld">A reference to the current world object</param>
        public Storage(Node n,double x_size,double z_size, double x_position, double y_position, double z_position,World currentworld)
        {
            DropoffNode = n;
            position_x = x_position;
            position_y = y_position;
            position_z = z_position;

            this.x_size = x_size;
            this.z_size = z_size;

            Max_Barrels = Convert.ToInt64(Math.Floor(x_size / 2.5));
 
            this.w = currentworld;

            // Have the Storage element start off with a random number of Rek's
            Random r = new Random();
            int q = r.Next(1, 3);

            for (int i = 0; i < q; i++)
            {
                Rek newrek = new Rek((i*1.5)+position_x,0, position_z + 2.5,0,0,0);
                newrek.readyforpickup = false;

                // If i have spare time later, edit this to give it a random position in the list/ array
                Stored.Add(newrek);
            }
            RenderRekken();

        }
        /// <summary>
        /// Accept a Rek from a robot, and store it
        /// </summary>
        /// <param name="r">rek to be stored</param>
        public void AddRek(Rek r)
        {
            r.readyforpickup = false;
            for (int i = 0; i <10; i++)
            {
                if (Stored.ElementAtOrDefault(i) == null)
                {
                    r.Move((i*1.5)+position_x,0,position_z+2.5);
                    Stored.Add(r);
                    return;
                }
            }
        }
        /// <summary>
        /// Spawns the initial rekken in the Rekken list into the world
        /// </summary>
        public void RenderRekken()
        {
            foreach (var rek in Stored)
            {
                w.worldObjects.Add(rek);
            }
           
        }
        /// <summary>
        /// Returns a value  on whether or not the storage area is full
        /// </summary>
        /// <returns> Whether or not this storage area is full</returns>
        public bool IsFull()
        {
            if (Stored.Count >=Max_Barrels)
            {
                return true;
            }
            else { return false; }
          
        }

    }
   
}