using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class Review
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [ForeignKey("Booking")]
        public int BookingID { get; set; }

        [Range(1, 5)] 
        public int Rating { get; set; }

        [Length(0, 500)]
        public string ReviewText { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
