using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiBusCR.Models
{

    [Table("reportes_usuarios")]
    public class ReporteUsuario : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("ruta_id")]
        public string RutaId { get; set; }

        [Column("tipo_reporte")]
        public string TipoReporte { get; set; }

        [Column("comentario")]
        public string Comentario { get; set; }

        [Column("estado")]
        public string Estado { get; set; } // Pendiente, Resuelto, etc.
    }

}
