namespace Tio_Soft.AplicacionWeb.Utilidades.Response
{
    //clase como respuesta a todas las solucitudes que se hagan
    public class GenericResponse<TObject>
    {
        public bool Estado { get; set; }
        public string? Mensaje { get; set; }
        public TObject? Objeto { get; set; }
        public List<TObject>? ListaObjeto { get; set; }
    }
}