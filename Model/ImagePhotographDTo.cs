using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class ImagePhotographDTo
    {
        [Key]
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public string Description { get; set; }
        public byte[]? MyWorks { get; set; } = null;
    }
}
