using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Model
{
    public class HistoryPointsReceived
    {
        [Key]
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int Point { get; set; }
        public Enums.TypeAdmission Type { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("CustomerID")]

        public string ShowInformation { get => $"Тип: {Type} -Количество баллов: {Point}- Дата: {Date}"; }
        public virtual Customer Customer { get; set; }
    }
}
