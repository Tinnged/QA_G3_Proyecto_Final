using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MiBusCR.Models
{
    [Table("usuarios_prueba")]
    public class UsuarioPrueba : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("nombre_completo")]
        public string NombreCompleto { get; set; }
    }

}
