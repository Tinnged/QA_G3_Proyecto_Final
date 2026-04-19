using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiBusCR.Models
{
    [Table("ubicaciones_buses")]
    public class UbicacionBus : BaseModel
    {
        [PrimaryKey("placa_bus", false)]
        public string PlacaBus { get; set; }

        [Column("ruta_id")]
        public string RutaId { get; set; }

        [Column("latitud_actual")]
        public double LatitudActual { get; set; }

        [Column("longitud_actual")]
        public double LongitudActual { get; set; }

        [Column("velocidad")]
        public float? Velocidad { get; set; }

        [Column("ultima_actualizacion")]
        public DateTime UltimaActualizacion { get; set; }
    }
}
