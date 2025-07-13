using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Tables
{
    [Table("Tamanos")]
    public class Tamano
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTam { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Este campo es necesario")]
        public string DesTam { get; set; } = "-";
        [Required(ErrorMessage = "Este campo es necesario")]
        public decimal? VendTam { get; set; }
        public bool? IsAct { get; set; } = true;
        public DateTime? RegDat { get; set; } = DateTime.Now;
        [MaxLength(60)]
        public string? Token { get; set; } = Guid.NewGuid().ToString();
    }
}
