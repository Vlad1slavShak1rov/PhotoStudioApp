using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class HallDTO
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string? AvailableServices { get; set; }
    }
}
