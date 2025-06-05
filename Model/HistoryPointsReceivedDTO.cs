using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class HistoryPointsReceivedDTO
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int Point { get; set; }
        public Enums.TypeAdmission Type { get; set; }
        public DateTime Date { get; set; }
    }
}
