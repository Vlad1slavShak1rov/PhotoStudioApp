using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class HallPhoto
    {
        [Key]
        public int Id { get; set; }
        public int HallId { get; set; }
        public byte[]? Image { get; set; }

        public virtual Hall Hall { get; set; }
    }
}
