using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Tables
{
    [Table("Inventarios")]
    public class Inventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdInv { get; set; }
        public int? IdCat { get; set; }
        [MaxLength(80)]
        public string DesInv { get; set; } = "-";
        public decimal? CompInv { get; set; }
        public decimal? VendInv { get; set; }
        public bool? IsAct { get; set; } = true;
        public DateTime? RegDat { get; set; } = DateTime.Now;
        [MaxLength(60)]
        public string? Token { get; set; } = Guid.NewGuid().ToString();
    }

}
