using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models {
    public class Truck : Abstract_Model
    {
        public Truck(double x, double y, double z, double rotationX, double rotationY, double rotationZ)
        {
            this.type = "truck";
            this.guid = Guid.NewGuid();

            this._x = x;
            this._y = y;
            this._z = z;

            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;
         }
     }
}