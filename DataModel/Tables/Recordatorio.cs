using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Tables
{
    [Table("Recordatorios")]
    public class Recordatorio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRecord { get; set; }
        public int? IdMsg { get; set; }
        [MaxLength(80)]
        public string IdGrps { get; set; } = "-";
        public TimeSpan HourRecord { get; set; }
        public bool? IsAct { get; set; } = true;
        public DateTime? RegDat { get; set; } = DateTime.UtcNow;
        [MaxLength(80)]
        public string Token { get; set; } = Guid.CreateVersion7().ToString();

        [ForeignKey(nameof(IdMsg))]
        public virtual Mensaje? Mensaje { get; set; }
    }
}
