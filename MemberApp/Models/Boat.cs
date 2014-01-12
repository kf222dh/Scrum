using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemberApp.Models
{
    class Boat
    {
        public Boat(string type, double length, string info)
        {
            Type = type;
            Length = length;
            Info = info;
        }

        public Boat(string type)
        {
            Type = type;
            Length = new double();
            Info = String.Empty;
        }

        public string Info { get; set; }

        public string Type { get; set; }

        public double Length { get; set; }
    }
}
