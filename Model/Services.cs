using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class Services
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CostService { get; set; }

        public int BonusCost { get; set; }
        public virtual Booking Booking { get; set; }

    }
}
