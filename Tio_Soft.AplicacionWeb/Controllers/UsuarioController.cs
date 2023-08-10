using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using Tio_Soft.AplicacionWeb.Models.ViewModels;
using Tio_Soft.AplicacionWeb.Utilidades.Response;
using Tio_Soft.BLL.Interfaces;
using Tio_Soft.Entity;

namespace Tio_Soft.AplicacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioServicio;
        private readonly IRolService _rolServicio;
        public UsuarioController(IMapper mapper, 
            IUsuarioService usuarioServicio, 
            IRolService rolServicio
            )
        {
            _mapper = mapper;
            _usuarioServicio = usuarioServicio;
            _rolServicio = rolServicio;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaRoles()
        {
            List<VMRol>vmListaRoles=_mapper.Map<List<VMRol>>(await _rolServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, vmListaRoles);
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMUsuario> vmUsuarioLista = _mapper.Map<List<VMUsuario>>(await _usuarioServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, new {data=vmUsuarioLista});
        }

        [HttpPost]
        public async Task<IActionResult> Crear(/*[FromForm]IFormFile foto,*/ [FromForm]string modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();
            try
            {
                VMUsuario vmUsuario = JsonConvert.DeserializeObject<VMUsuario>(modelo);

                /*string nombreFoto = "";
                Stream fotoStream = null;
                if(fotoStream != null)
                {
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extencion = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_en_codigo, extencion);
                    fotoStream = fotoStream.OpenReadStream();
                }*/

                string urlPlantillaCorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/EnviarClave?correo=[correo]&clave=[clave]";
                Usuario usuario_creado = await _usuarioServicio.Crear(_mapper.Map<Usuario>(vmUsuario), urlPlantillaCorreo/*fotoStream,NombreFoto,*/);

                vmUsuario = _mapper.Map<VMUsuario>(usuario_creado);

                gResponse.Estado = false;
                gResponse.Objeto = vmUsuario;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpPut]
        public async Task<IActionResult> Editar(/*[FromForm]IFormFile foto,*/ [FromForm] string modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();
            try
            {
                VMUsuario vmUsuario = JsonConvert.DeserializeObject<VMUsuario>(modelo);

                /*string nombreFoto = "";
                Stream fotoStream = null;
                if(fotoStream != null)
                {
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extencion = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_en_codigo, extencion);
                    fotoStream = fotoStream.OpenReadStream();
                }*/

                Usuario usuario_editado = await _usuarioServicio.Editar(_mapper.Map<Usuario>(vmUsuario)/*fotoStream,NombreFoto,*/);

                vmUsuario = _mapper.Map<VMUsuario>(usuario_editado);

                gResponse.Estado = false;
                gResponse.Objeto = vmUsuario;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult>Eliminar(int IdUsuario) {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _usuarioServicio.Eliminar(IdUsuario);
            }
            catch (Exception ex)
            {

                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
