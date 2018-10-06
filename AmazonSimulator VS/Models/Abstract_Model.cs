using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public abstract class Abstract_Model : IUpdatable {
        protected double _x = 0;
        protected double _y = 0;
        protected double _z = 0;
        protected double _rX = 0;
        protected double _rY = 0;
        protected double _rZ = 0;

        public string type { get; set; }
        public Guid guid { get; set; }
        public double x { get { return _x; } }
        public double y { get { return _y; } }
        public double z { get { return _z; } }
        public double rotationX { get { return _rX; } }
        public double rotationY { get { return _rY; } }
        public double rotationZ { get { return _rZ; } }

        public bool needsUpdate = true;
        // Using nodes instead of doubles, delete these doubles  later
        protected double target_x = 0;
        protected double target_y = 0;
        protected double target_z = 0;

        protected List<Node> route;
        protected Node TargetNode;

        protected List<Node> PathList;
        public bool destinationreached = true;
        public bool isMoving = false;

        // Speed may not be lower than 0,1
        public double speed = 0.3;

       /// <summary>
       /// Set the position of the model
       /// </summary>
       /// <param name="x"></param>
       /// <param name="y"></param>
       /// <param name="z"></param>
        public virtual void Move(double x, double y, double z) {
            this._x = x;
            this._y = y;
            this._z = z;

            needsUpdate = true;
        }
        /// <summary>
        /// Legacy function. Hah. Probably doesnt do anything anymore but leaving it here just in case
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public virtual void SetDestination(double x,double y ,double z)
        {
            target_x = x;
            target_y = y;
            target_z =  z;
            destinationreached = false;
        }
        /// <summary>
        /// Moves the model to the given coordinates
        /// </summary>
        /// <param name="xd">x coordintate</param>
        /// <param name="yd"> y coordinate</param>
        /// <param name="zd"> z coordinate</param>
        public virtual void MoveTo(double xd, double yd, double zd)
        {
            // Lots of statements to make sure the model moves properly
         if (!isMoving)
            {
                // If  this is the first time moving, set the target
                target_x = xd;
                target_y = yd;
                target_z = zd;
                isMoving = true;
                needsUpdate = true;
            }
            if (isMoving)
            {
                if (xd == this.x && yd == this.y && zd ==z)
                {
                    isMoving = false;
                    return;
                }
                // If already moving, move the model on the axis by the value of speed. ALso checks for positve and negative values
                if (x != target_x )
                {
                    if (target_x >x)
                    {
                        _x = x + speed;

                    }
                    else
                    {
                        _x = x - speed;
                    }
                    if (Math.Abs(target_x - x) < speed)
                    {
                        _x = target_x;
                        Console.WriteLine("Less than " + speed + " x diff");

                    }

                }
                if (y != target_y)
                {
                    if (target_y > y)
                    {
                        _y = y + speed;
                    }
                    else
                    {
                        _y = y - speed;
                    }
     
                    if (Math.Abs(target_y - y) < speed)
                    {

                        _y = target_y;
                        Console.WriteLine("Less than "+speed+" y diff");
                    }

                }
                if (z != target_z)
                {
                    if (target_z > z)
                    {
                        _z = z + speed;
                    }
                    else
                    {
                        _z = z - speed;
                    }
                    //  _z = z + speed;
                    if (Math.Abs(target_z - z) < speed)
                    {
                        _z = target_z;
                        Console.WriteLine("Less than " + speed + " z diff");
                    }

                }
                needsUpdate = true;
                return;
            }

            if (destinationreached)
            {
                // No need to move this update
                return;
            }


        }

        /// <summary>
        ///  Calls Moveto using a node instead of coordinates
        /// </summary>
        /// <param name="node"></param>
        public virtual void MoveTo(Node node)
        {
            MoveTo(node.x, node.y, node.z);
        }

        /// <summary>
        /// Sets the rotation of this model
        /// </summary>
        /// <param name="rotationX"></param>
        /// <param name="rotationY"></param>
        /// <param name="rotationZ"></param>
        public virtual void Rotate(double rotationX, double rotationY, double rotationZ) {
            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;

            needsUpdate = true;
        }
       /// <summary>
       /// Gets called every tick
       /// </summary>
       /// <param name="tick"></param>
       /// <returns></returns>
        public virtual bool Update(int tick)
        {
            if(needsUpdate) {
                needsUpdate = false;
                return true;
            }
            return false;
        }
    }
}