using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models {
    public class Rek : Abstract_Model
    {
        public bool readyforpickup = true;
        public Rek(double x, double y, double z, double rotationX, double rotationY, double rotationZ)
        {
            this.type = "rek";
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