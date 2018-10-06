using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models {
    public class Robot : Abstract_Model
    {
        bool atPickupPoint = true;
        public bool idle {get;set;}
        public Rek rekToCarry; // Rek wat de robot op moet halen
        Rek carriedRek; // Rek wat de robot vast heeft
        public Trein trainToLoad; // De trein welke geladen moet worden
        private World w;

        int position = 0;



    public Robot(double x, double y, double z, double rotationX, double rotationY, double rotationZ,World w)
        {
            this.w = w;
            route = new List<Node>();
            this.type = "robot";
            this.guid = Guid.NewGuid();

            this._x = x;
            this._y = y;
            this._z = z;

            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;
        }
        /// <summary>
        /// Main loop
        /// </summary>
        public void Main()
        {
            // If idle, dont do anything
            if (idle)
            {
                return;
            }

            if(route != null && route.Count > position)
            {
                this.MoveTo(route[position]);

                // Check if the robot has reached a node
                if (this.x == route[position].x && this.y == route[position].y && this.z == route[position].z)
                {
                    //Check if the robot is at it's destination
                    if (this.x == TargetNode.x && this.y == TargetNode.y && this.z == TargetNode.z)
                    {
                        if (carriedRek != null)
                        {
                            
                            if(this.trainToLoad != null)
                            {
                                this.trainToLoad.Load(carriedRek);
                                this.carriedRek = null;
                                this.trainToLoad = null;
                            }
                            else
                            {
                                DropOffRek(TargetNode);
                            }
                        }
                        else
                        {
                            PickupRek(rekToCarry);
                        }

                    }
                    // Else,check if this is the last stop
                    else if (route[route.Count - 1] == route[position])
                    {
                        if(carriedRek != null)
                        {
                            if (this.trainToLoad != null)
                            {
                                this.trainToLoad.Load(carriedRek);
                                this.carriedRek = null;
                                this.trainToLoad = null;
                                idle = true;
                            }
                            else
                            {
                                DropOffRek(route[position]);
                            }
                            
                        }
                        idle = true;
                        return;
                    }
                    position++;

                    this.isMoving = false;
                }
            }

        }
      
        /// <summary>
        /// Sets the route the robot has to take
        /// </summary>
        /// <param name="points">A list of charactrse the robot has to follow</param>
        /// <param name="target"> The point where the robot has to drop off it's load</param>
        public void SetRoute(List<char> points,char target)
        {
            this.route.Clear();
            this.position = 0;
            // Set the route the robot needs to take
            foreach (char char_point in points)
            {
                foreach (Node node_point in w.NodeList)
                {
                    if (char_point == node_point.name)
                    {
                        route.Add(node_point);
                    }
                }
                if (char_point == target)
                {
                    foreach (Node item in w.NodeList)
                    {
                        if (item.name == target)
                        {
                            TargetNode = item;
                        }
                    }
                    
                }
            }
            // Sets the target the robot has to drop it's load off

            Console.WriteLine();
        }

        /// <summary>
        ///Tell the robot to pick up a specific Rek
        /// </summary>
        public void PickupRek(Rek k)
        {
            if (k.readyforpickup == true)
            {
                carriedRek = k;
                // carriedRek.Move(this.x+30, this.y+30, this.z);
            }
            else
            {
                Console.WriteLine("Tried to pickup unready Rek");
            }
        }

        /// <summary>
        ///Tell the robot to pick up a nearby Rek
        /// </summary>
        public void PickupRek()
        {
            // Check if the robot is at the depot
            if (atPickupPoint)
            {
                foreach (var item in w.worldObjects)
                {
                    
                    if (item is Rek)
                    {
                        Rek q = (Rek)item;

                        if (q.readyforpickup == true)
                        {
                            carriedRek = q;
                           // carriedRek.Move(this.x+30, this.y+30, this.z);
                        }
                    }
                }
            }
        }
       /// <summary>
       /// Gets called when the robot reaches it's destination
       /// </summary>
      
  public void DropOffRek(Node DropOffAt)
        {
            // Check if the robot is actually carrying a Rek
            if (carriedRek!= null)
            {
                // Loop over all the storagespots, find the matching spot
                for (int i = 0; i < w.StorageSpots.Count; i++)
                {
                    if (DropOffAt == w.StorageSpots[i].DropoffNode)
                    {
                        w.StorageSpots[i].AddRek(carriedRek);
                        carriedRek = null;
                        break;
                    }
                }
                
                // Drop off or something
            }
        }
        public override bool Update(int tick)
        {
            Main();
            if(carriedRek != null)
            {
                carriedRek.Move(this.x, this.y+0.5, this.z);
            }
            if (needsUpdate)
            {
                needsUpdate = false;
                return true;
            }
            return false;
        }
    }
}