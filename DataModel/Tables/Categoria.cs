using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Tables
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCat { get; set; }
        [Required(ErrorMessage = "Este Campo es necesitado")]
        public string DesCat { get; set; } = "-";
        public bool? IsAct { get; set; } = true;
        public DateTime? RegDat { get; set; } = DateTime.Now;
        [MaxLength(60)]
        public string? Token { get; set; } = Guid.NewGuid().ToString();
    }
}
