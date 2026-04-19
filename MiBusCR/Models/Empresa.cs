using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace MiBusCR.Models
{

    [Table("empresas")]
    public class Empresa : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("nombre_empresa")]
        public string NombreEmpresa { get; set; }

        [Column("cedula_juridica")]
        public string CedulaJuridica { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }
    }

}
