using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MiBusCR.Models
{

    [Table("rutas")]
    public class Ruta : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("destino")]
        public string Destino { get; set; }

        [Column("recorrido_ruta")]
        public string RecorridoRuta { get; set; } // Formato JSONB

        [Column("esta_activa")]
        public bool EstaActiva { get; set; }

        [Column("empresa_id")]
        public string EmpresaId { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }
    }

}
