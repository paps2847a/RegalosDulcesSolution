using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Tables
{
    [Table("Mensajes")]
    public class Mensaje
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMsg { get; set; }
        [MaxLength(80)]
        public string DesMsg { get; set; }
        public bool? IsAct { get; set; } = true;
        public DateTime? RegDat { get; set; } = DateTime.UtcNow;
        [MaxLength(80)]
        public string Token { get; set; } = Guid.CreateVersion7().ToString();
    }

}
