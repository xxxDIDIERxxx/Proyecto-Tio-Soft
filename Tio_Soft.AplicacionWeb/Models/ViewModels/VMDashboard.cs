namespace Tio_Soft.AplicacionWeb.Models.ViewModels
{
    public class VMDashboard
    {
        public int TotalVenta { get; set; }
        public int TotalProductos { get; set; }
        public string? TotalIngresos { get; set; }
        public int TotalCategorias { get; set; }

        public List<VMVentasSemana> VentasUltimaSemana { get; set; }
        public List<VMProductosSemana> ProductosTopUltimaSemana { get; set; }
    }
}
