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
    public class Worker
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }

        public string Name { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }
        [Required]
        public Post Post { get; set; }
        public bool IsOnBookin { get; set; } = false;
        public byte[]? MyWorks { get; set; } = null;
        public string FullName
        {
            get
            {
                return $"{Name} {LastName}"; // Объединяем имя и фамилию
            }
        }
        public virtual User User { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
