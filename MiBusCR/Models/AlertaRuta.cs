using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiBusCR.Models
{

    [Table("alertas_ruta")]
    public class AlertaRuta : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("ruta_id")]
        public string RutaId { get; set; }

        [Column("tipo_alerta")]
        public string TipoAlerta { get; set; }

        [Column("mensaje")]
        public string Mensaje { get; set; }

        [Column("esta_activa")]
        public bool EstaActiva { get; set; }

        [Column("fecha_expiracion")]
        public DateTime? FechaExpiracion { get; set; }
    }

}
