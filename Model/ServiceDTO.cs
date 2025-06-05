using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class ServiceDTO
    {
        public int ID { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int CostService { get; set; }
        public int BonusCost { get; set; }
    }
}
