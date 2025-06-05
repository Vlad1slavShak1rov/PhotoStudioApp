using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class BookingServiceDTO
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int PhotographID { get; set; }
        public int VisagisteID { get; set; }
        public int HallID { get; set; }
        public int ServiceID { get; set; }
        public DateTime DateBooking { get; set; }
        public int? AdditionalServicesID { get; set; }
        public double CostServices { get; set; }
    }
}
