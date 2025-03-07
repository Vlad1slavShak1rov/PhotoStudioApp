using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoStudioApp.Enums;

namespace PhotoStudioApp.Model
{
    public class Payment
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Booking")]
        public int BookingID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
