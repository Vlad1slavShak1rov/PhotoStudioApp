using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class Hall
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Description { get; set; }
        public double Cost { get; set; }

        public virtual List<HallPhoto> HallPhotos { get; set; }

        public virtual List<Booking> Bookings { get; set; }
    }
}
