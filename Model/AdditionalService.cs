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
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int BonusCost { get; set; }
        [Required]
        public decimal Cost { get; set; }

        public virtual List<Booking> Bookings { get; set; } = new();
    }
}
