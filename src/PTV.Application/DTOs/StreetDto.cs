using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTV.Application.DTOs
{
    public class StreetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Geometry { get; set; } // WKT format
    }
}
