using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public  class Node
    {
        List<Node> neighbours = new List<Node>();
        public char name;
        public double x = 0;
        public double y = 0;
        public double z = 0;

        public Node(char name, double x, double y, double z)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.z = z;
        }


    }
   
}