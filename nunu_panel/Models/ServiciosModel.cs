namespace nunu_panel.Models;

public class ServicioProveedorModel
{
    public int id_servicio { get; set; }
    public int IdProveedor { get; set; }
    public ProveedorHpModel Proveedores { get; set; }
    public string tipo_servicio { get; set; }
    public string descripcion { get; set; }
    public string Duracion { get; set; }
    public string RangoPrecios { get; set; }
    public double Rating { get; set; }
    public string imagen_servicio { get; set; }
    public int IdCategoria { get; set; }
    public CategoriaModel Categoria { get; set; }
    public string MapsUrlServicio { get; set; }
}

public class ProveedorHpModel
{
    public int IdProveedor { get; set; }
    public string nombre_razon_social { get; set; }
    public string Correo { get; set; }
    public string Contrasena { get; set; }
    public string Telefono { get; set; }
    public string AntecedentesPenales { get; set; }
    public bool Verificado { get; set; }
    public DateTime FechaRegistro { get; set; }
    public bool Suscripcion { get; set; }
    public string Uid { get; set; }
    public string IdOneSignal { get; set; }
    public string FotoProveedor { get; set; }
    public string IneFrente { get; set; }
    public string IneReverso { get; set; }
    public string Curriculum { get; set; }
    public string ComprobanteDomicilio { get; set; }
    public double Rating { get; set; }
}

public class CategoriaModel
{
    public int id_categoria { get; set; }
    
    public string categoria { get; set; }

    public string? imagen_Categoria { get; set; }

    public IFormFile Imagen { get; set; }
}