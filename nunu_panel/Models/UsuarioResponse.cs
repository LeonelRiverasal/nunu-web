using System.Text.Json.Serialization;

namespace masajesWEB.Models.Usuarios
{
    public class UsuarioResponse
    {
        
        public int Id { get; set; }

        [JsonPropertyName("nombreCompleto")]
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        
        [JsonPropertyName("email")]
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        [JsonPropertyName("estado")]
        public string Estatus { get; set; }
        public string FotoPerfil { get; set; }
        public string Genero { get; set; }
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        public string TipoCliente { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool? Estacionamiento { get; set; }
        public string DatosClinicos { get; set; }
    }
}
