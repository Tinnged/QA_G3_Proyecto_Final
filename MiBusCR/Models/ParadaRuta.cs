using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiBusCR.Models
{

    [Table("paradas_ruta")]
    public class ParadaRuta : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("ruta_id")]
        public string RutaId { get; set; }

        [Column("parada_id")]
        public string ParadaId { get; set; }

        [Column("orden_parada")]
        public int OrdenParada { get; set; }

        [Reference(typeof(Parada))]
        public Parada DetalleParada { get; set; }

        [Column("no_es_la_primera", ignoreOnInsert: true, ignoreOnUpdate: true)]
        public bool NoEsLaPrimera { get; set; }

        [Column("no_es_la_ultima", ignoreOnInsert: true, ignoreOnUpdate: true)]
        public bool NoEsLaUltima { get; set; }

    }



}
