using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTV.Application.DTOs
{
    public class AddStreetRequest
    {
        public string Name { get; set; }
        public AddPointRequest StartPoint{ get; set; }
        public AddPointRequest EndPoint { get; set; }
        public int Capacity { get; set; }
    }
}
