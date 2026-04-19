using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;



namespace MiBusCR.Models
{

    [Table("administradores")]
    public class Administrador : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("correo")]
        public string Correo { get; set; }

        [Column("contrasena")]
        public string Contrasena { get; set; }

        [Column("nombre_completo")]
        public string NombreCompleto { get; set; }

        [Column("rol")]
        public string Rol { get; set; }

        [Column("empresa_id")]
        public string EmpresaId { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }
    }

}
