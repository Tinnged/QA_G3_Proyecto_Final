using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

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

        [Column("provincia_inicio")]
        public string ProvinciaInicio { get; set; }

        [Column("provincia_final")]
        public string ProvinciaFinal { get; set; }

        [Column("acepta_efectivo", ignoreOnInsert: true, ignoreOnUpdate: true)]
        public bool AceptaEfectivo { get; set; }

        [Column("acepta_tarjeta", ignoreOnInsert: true, ignoreOnUpdate: true)]
        public bool AceptaTarjeta { get; set; }

        [Column("acepta_sinpe", ignoreOnInsert: true, ignoreOnUpdate: true)]
        public bool AceptaSinpe { get; set; }

        [Column("monto_tarifa", ignoreOnInsert: true, ignoreOnUpdate: true)]
        public decimal MontoTarifa { get; set; }

    }

}
