using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("WsGroups")]
    public class WsGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGrp { get; set; }
        [MaxLength(250)]
        public string IdWsGrp { get; set; }
        [MaxLength(80)]
        public string GrpNam { get; set; }
        public bool? IsAct { get; set; } = true;
        [MaxLength(80)]
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public DateTime? RegDat { get; set; } = DateTime.Now;
    }
}
