using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTV.Core.Entities
{
    public class BaseEntity: IEntity
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }
}
