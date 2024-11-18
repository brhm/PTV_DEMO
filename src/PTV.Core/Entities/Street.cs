using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTV.Core.Entities
{
    public class Street:BaseEntity
    {
        public string Name { get; set; } // Street name
        public LineString Geometry { get; set; } // Geometry as LineString
        public int Capacity { get; set; } // Number of vehicles per minute

    }
}
