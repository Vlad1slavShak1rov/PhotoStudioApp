using PhotoStudioApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class WorkerDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public bool IsOnBookin { get; set; } = false;
        public byte[]? MyWorks { get; set; } = null;
        public Post Post { get; set; }
    }
}
