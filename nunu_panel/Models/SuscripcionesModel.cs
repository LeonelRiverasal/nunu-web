namespace nunu_panel.Models
{
    public class SuscripcionesModel
    {
        public int id_proveedor{ get; set; }
        public string nombre_razon_social { get; set; }
        public string correo { get; set; }
        public bool verificado {get;set;}
        public string telefono { get; set; }
        public DateTime fecha_registro { get; set; }
        public bool suscripcion {get;set;}
        public string antecedentes_penales { get; set; }
        public string ine_frente { get; set; }
        public string ine_reverso { get; set; }
        public string curriculum { get; set; }
        public string comprobante_domicilio { get; set; }


    }
}