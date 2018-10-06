using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public  class Storage
    {
        public List<Rek> Stored = new List<Rek>();
        World w;
        double x_size;
        double z_size;

        double position_x;
        double position_y;
        double position_z;

        public Node DropoffNode;

        public Storage(Node n, double x, double y, double z,World currentworld)
        {
            DropoffNode = n;
            position_x = x;
            position_y = y;
            position_z = z;
            this.w = currentworld;

            // Have the Storage element start off with a random number of Rek's
            Random r = new Random();
            int q = r.Next(1, 5);
            x_size = 25;
            z_size = 5;

            for (int i = 0; i < q; i++)
            {
                Rek newrek = new Rek((i*2.5)+position_x,0, position_z + 2.5,0,0,0);
                newrek.readyforpickup = false;

                // If i have spare time later, edit this to give it a random position in the list/ array
                Stored.Add(newrek);
            }
            RenderRekken();

        }

        /*public void AddRek(Rek r)
        {
            r.readyforpickup = false;
            for (int i = 0; i < Stored.Count; i++)
            {
                if (Stored[i] == null)
                {
                    Stored[i] = r;
                    r.Move((i * 2.5) + position_x, 0, position_z + 2.5);
                }
            }
        }*/

        public void AddRek(Rek r)
        {
            r.readyforpickup = false;
            
            Stored.Add(r);
            r.Move((Stored.Count * 2.5) + position_x, 0, position_z + 2.5);
            
            
        }

        /// <summary>
        /// Spawns the rekken in the Rekken list into the world
        /// </summary>
        public void RenderRekken()
        {
            foreach (var rek in Stored)
            {
                w.worldObjects.Add(rek);
            }
           
        }

    }
   
}