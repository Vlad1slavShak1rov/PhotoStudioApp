using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        [Length(0, 64)]
        public string Name { get; set; }
        [Length(0, 64)]
        public string SecondName { get; set; }
        [Length(0, 64)]
        public string LastName { get; set; }
        [Required]
        public string Contact { get; set; }

        public virtual User User { get; set; }
    }
}
