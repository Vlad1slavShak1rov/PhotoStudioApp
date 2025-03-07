using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class AdditionalService
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Length(0, 128)]
        public string ServiceName { get; set; }

        [Length(0, 512)]
        public string Description { get; set; }

        [Required]
        public decimal Cost { get; set; }
    }
}
