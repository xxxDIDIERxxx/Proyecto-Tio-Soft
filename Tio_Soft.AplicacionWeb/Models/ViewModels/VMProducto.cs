namespace Tio_Soft.AplicacionWeb.Models.ViewModels
{
    public class VMProducto
    {
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }
        public int? IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Precio { get; set; }
        public int? EsActivo { get; set; }
    }
}
