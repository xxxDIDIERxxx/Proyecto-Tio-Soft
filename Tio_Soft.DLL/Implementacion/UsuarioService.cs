using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tio_Soft.BLL.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Net;
using Tio_Soft.BLL.Interfaces;
using Tio_Soft.DAL.Interfaces;
using Tio_Soft.Entity;

namespace Tio_Soft.BLL.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repositorio;
        private readonly IUtilidadesService _utilidadesService;
        private readonly ICorreoService _correoService;
        //private readonly IFireBaseService  _fireBaseService;

        public UsuarioService(
                IGenericRepository<Usuario> repositorio,
                IUtilidadesService utilidadesService,
                ICorreoService correoService
            //IFireBaseService fireBaseService
            )
        {
            _repositorio = repositorio;
            _utilidadesService = utilidadesService;
            _correoService = correoService;
            //_fireBaseService = fireBaseService;
        }

        public async Task<List<Usuario>> Lista()
        {
            IQueryable<Usuario> query = await _repositorio.Cunsultar();
            return query.Include(r => r.IdRolNavigation).ToList();
        }

        public async Task<Usuario> Crear(Usuario entidad, Stream Foto = null, string NombreFoto = "", string UrlPlantillaCorreo = "")
        {
            Usuario usuario_existe = await _repositorio.Obtener(u => u.Correo == entidad.Correo);
            if (usuario_existe != null)
            {
                throw new TaskCanceledException("El Correo ya existe");
            }

            try
            {
                string clave_generada = _utilidadesService.GenerarClave();
                entidad.Clave = _utilidadesService.ConvertirSha256(clave_generada);

                /*entidad.NombreFoto = NombreFoto;
                if(Foto != null)
                {
                    string urlFoto = await _fireBaseService.SubirStorage(Foto, "carpeta_usuario", NombreFoto);
                    entidad.UrlFoto = urlFoto;
                }*/

                Usuario usuario_creado = await _repositorio.Crear(entidad);
                if (usuario_creado.IdUsuario == 0)
                {
                    throw new TaskCanceledException("No se puede crear el usuario");
                }

                //correo
                if (UrlPlantillaCorreo != "")
                {
                    UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[correo", usuario_creado.Correo).Replace("[clave", clave_generada);

                    string htmlCorreo = "";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlPlantillaCorreo);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //obtiene la respuesta de la solicitud que se hace anteriormente

                    if (response.StatusCode == HttpStatusCode.OK)//valilda el estado de la solicitud
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader readerStream = null;

                            if (response.CharacterSet == null)//valida caraceter especiales
                            {
                                //si no encuentra caracteres especiales
                                readerStream = new StreamReader(dataStream);
                            }
                            else
                            {
                                //si encuentra caracteres especiales l
                                readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));//da lectura a la propia pagina
                            }

                            htmlCorreo = readerStream.ReadToEnd();
                            response.Close();
                            readerStream.Close();

                        }
                    }
                    if (htmlCorreo != "")
                    {
                        await _correoService.EnviarCorreo(usuario_creado.Correo, "Cuenta Creada", htmlCorreo);
                    }
                }
                //consultar el usuario    
                IQueryable<Usuario> query = await _repositorio.Cunsultar(u => u.IdUsuario == usuario_creado.IdUsuario);
                //consultamos el usuario para incluirele el rol
                usuario_creado = query.Include(r => r.IdRolNavigation).First();

                return usuario_creado;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
            public async Task<Usuario> Editar(Usuario entidad, Stream Foto = null, string NombreFoto = "")
            {
                Usuario usuario_existe = await _repositorio.Obtener(u => u.Correo == entidad.Correo && u.IdUsuario != entidad.IdUsuario);

                if (usuario_existe != null)
                {
                    throw new TaskCanceledException("El correo ya existe");
                }

                try
                {
                    IQueryable<Usuario> queryUsuario = await _repositorio.Cunsultar(u => u.IdUsuario == entidad.IdUsuario);

                    Usuario usuario_editar = queryUsuario.First();
                    usuario_editar.Nombre = entidad.Nombre;
                    usuario_editar.Correo = entidad.Correo;
                    usuario_editar.Telefono = entidad.Telefono;
                    usuario_editar.IdRol = entidad.IdRol;

                    /*if (usuario_editar.NombreFoto == "")
                    {
                        usuario_editar.NombreFoto = NombreFoto;
                    }
                    if(Foto != null)
                    {
                        string urlFoto = await _fireBaseService.SubirStorage(Foto, "carpeta_usuario", usuario_editar.NombreFoto);
                        usuario_editar.UrlFoto = urlFoto;
                    }*/

                    bool respuesta = await _repositorio.Editar(usuario_editar);
                    if (respuesta)
                    {
                        throw new TaskCanceledException("No se pudo modificar el usuario");
                    }
                    Usuario usuario_editado = queryUsuario.Include(r => r.IdRolNavigation).First();

                    return usuario_editado;
                }
                catch
                {

                    throw;
                }
            }

            public async Task<bool> Eliminar(int IdUsuario)
            {
                try
                {
                    Usuario usuario_encontrado = await _repositorio.Obtener(u => u.IdUsuario == IdUsuario);

                    if (usuario_encontrado == null)
                    {
                        throw new TaskCanceledException("El usuario no existe");
                    }

                    //string nombreFoto = usuario_encontrado.NombreFoto;
                    bool respuesta = await _repositorio.Eliminar(usuario_encontrado);

                    //if (respuesta)
                    //{
                    //await _fireBaseService.ElmminarStorage("carpera_usuario", nombreFoto);
                    //}

                return true;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public async Task<Usuario> ObtenerPorCredenciales(string correo, string clave)
            {
                string clave_encriptada = _utilidadesService.ConvertirSha256(clave);
                
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.Correo.Equals(correo) 
                && u.Clave.Equals(clave_encriptada));

                return usuario_encontrado;
            }

            public async Task<Usuario> ObtenerPorId(int IdUsuario)
            {
                IQueryable<Usuario> query = await _repositorio.Cunsultar(u => u.IdUsuario == IdUsuario);
                Usuario resultado = query.Include(r => r.IdRolNavigation).FirstOrDefault();

                return resultado;
            }

            public async Task<bool> GuardarPerfil(Usuario entidad)
            {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.IdUsuario == entidad.IdUsuario);

                if (usuario_encontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                usuario_encontrado.Correo = entidad.Correo;
                usuario_encontrado.Telefono = entidad.Telefono;

                bool respuesta = await _repositorio.Editar(usuario_encontrado);

                return respuesta;
            }
            catch
            {

                throw;
            }
        }

        public async Task<bool> CambiarClave(int IdUsuario, string ClaveActual, string ClaveNueva)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.IdUsuario == IdUsuario);

                if (usuario_encontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                if (usuario_encontrado.Clave != _utilidadesService.ConvertirSha256(ClaveActual))
                {
                    throw new TaskCanceledException("La contraseña actual ingresada no es correcta");
                }

                usuario_encontrado.Clave = _utilidadesService.ConvertirSha256(ClaveNueva);

                bool respuesta = await _repositorio.Editar(usuario_encontrado);

                return respuesta;
            }
            catch(Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> RestablecerClave(string Correo, string UrlPlantillaCorreo)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.Correo == Correo);
                
                if(usuario_encontrado == null)
                {
                    throw new TaskCanceledException("No encontramos ningún usuario asociado al correo");
                }

                string clave_generada = _utilidadesService.GenerarClave();
                usuario_encontrado.Clave = _utilidadesService.ConvertirSha256(clave_generada);

                UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[clave", clave_generada);

                string htmlCorreo = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlPlantillaCorreo);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //obtiene la respuesta de la solicitud que se hace anteriormente

                if (response.StatusCode == HttpStatusCode.OK)//valilda el estado de la solicitud
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader readerStream = null;

                        if (response.CharacterSet == null)//valida caraceter especiales
                        {
                        //si no encuentra caracteres especiales
                            readerStream = new StreamReader(dataStream);
                        }
                            else
                            {
                                //si encuentra caracteres especiales l
                                readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));//da lectura a la propia pagina
                            }

                        htmlCorreo = readerStream.ReadToEnd();
                        response.Close();
                        readerStream.Close();

                    }
                }

                bool correo_enviado = false;

                if (htmlCorreo != "")
                {
                    correo_enviado = await _correoService.EnviarCorreo(Correo,"Contraseña Restablecida",htmlCorreo);
                }

                if (correo_enviado)
                {
                    throw new TaskCanceledException("Tenemos problemas. Por favor inténtalo de nuevo más tarde");
                }

                bool respuesta = await _repositorio.Editar(usuario_encontrado);

                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
 
}