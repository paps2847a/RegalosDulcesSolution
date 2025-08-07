using Microsoft.EntityFrameworkCore;
using DataModel.Tables;

namespace DataPersistance
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Tamano> Tamanos { get; set; }
        public DbSet<WsGroup> WsGroups { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<Recordatorio> Recordatorios { get; set; }
    }
}
