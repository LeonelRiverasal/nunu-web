namespace nunu_panel.Models
{
    public class AnuncioDetalleModel
    {
        public int id_anuncio { get; set; }
        public string nombre_empresa_anunciante { get; set; }
        public string tipo_Oferta { get; set; }
        public string descripcion_oferta { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public DateTime vigencia_oferta { get; set; }        
         public string Imagen_anuncio { get; set; }

    }
}