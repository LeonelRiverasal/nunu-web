namespace nunu_panel.Models
{
    public class ProveedorModel
    {
        public int id_proveedor { get; set; }
        public string nombre_razon_social { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public bool verificado { get; set; }
        public string telefono { get; set; } = string.Empty;
        public DateTime fecha_registro { get; set; }
        public string antecedentes_penales { get; set; } = string.Empty;
        public string ine_frente { get; set; } = string.Empty;
        public string ine_reverso { get; set; } = string.Empty;
        public string curriculum { get; set; } = string.Empty;
        public string comprobante_domicilio { get; set; } = string.Empty;
        public bool suspendido { get; set; }
        public string motivoSuspension { get; set; } = string.Empty;
    }
}
