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
        [Length(0,64)]
        public string Name { get; set; }
        [Length(0, 64)]
        public string SecondName { get; set; }
        [Length(0, 64)]
        public string LastName { get; set; }
        [Required]
        public Post Post { get; set; }
        public string FullName
        {
            get
            {
                return $"{Name} {LastName}"; // Объединяем имя и фамилию
            }
        }
        public virtual User User { get; set; }
    }
}
