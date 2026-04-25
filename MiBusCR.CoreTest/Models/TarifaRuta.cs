using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MiBusCR.Models
{
    [Table("tarifas_ruta")]
    public class TarifaRuta : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("ruta_id")]
        public string RutaId { get; set; }

        [Column("monto_tarifa")]
        public decimal MontoTarifa { get; set; }

        [Column("moneda")]
        public string Moneda { get; set; }

        [Column("acepta_efectivo")]
        public bool AceptaEfectivo { get; set; }

        [Column("acepta_tarjeta")]
        public bool AceptaTarjeta { get; set; }

        [Column("acepta_sinpe")]
        public bool AceptaSinpe { get; set; }
    }
}