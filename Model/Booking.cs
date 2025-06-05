using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class Booking
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        [ForeignKey("Photograph")]
        public int PhotographID { get; set; }
        [ForeignKey("Visagiste")]
        public int VisagisteID { get; set; }
        [ForeignKey("Hall")]
        public int HallID{ get; set; }
        [ForeignKey("Services")]
        public int ServiceID { get; set; }
        public DateTime DateBooking { get;set; }
        [ForeignKey("AdditionalService")]
        public int? AdditionalServicesID { get; set; }
        [Required]
        public double CostServices { get; set; }

        public string GetNameBooking
        {
            get => $"Номер: {ID} Дата: {DateBooking}";
        }

        public virtual Customer Customer { get; set; }
        public virtual Worker Photograph { get; set; }  
        public virtual Worker Visagiste { get; set; }  
        public virtual Hall Hall { get; set; }
        public virtual Services Services { get; set; }
        public virtual List<AdditionalService> AdditionalService { get; set; }
        public virtual Review Review { get; set; }

         public virtual Payment Payment { get; set; }
    }
}
