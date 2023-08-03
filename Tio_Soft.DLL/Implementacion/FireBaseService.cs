using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tio_Soft.BLL.Interfaces;
using Firebase.Auth;
using Firebase.Storage;
using Tio_Soft.Entity;
using Tio_Soft.DAL.Interfaces;
using Firebase.Auth.Providers;
using System.IO;

namespace Tio_Soft.BLL.Implementacion
{
    public class FireBaseService : IFireBaseService
    {
        private readonly IGenericRepository<Configuracion> _repositorio;

        public FireBaseService(IGenericRepository<Configuracion> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<string> SubirStorage(FileStream StreamArchivo, string CarpetaDestino, string NombreArchivo)
        {
            string UrlImagen = "";

            try
            {
                IQueryable<Configuracion> query = await _repositorio.Cunsultar(c => c.Recurso.Equals("FireBase_Storage"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);

                //Autorizacion
                var auth = new FirebaseAuthProvider(new FirebaseAuthConfig(Config["api_key"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);

                //token de cancelacion de origen
                var cancellation = new CancellationTokenSource();

                //tarea que ejecuta el servicio de FireBaseStorage
                var task = new FirebaseStorage(
                    Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        //funcion anonima qu ejecuta la tarea
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        //si hay algun error lo cancela
                        ThrowOnCancel = true
                    })
                    //crear las carpetas
                    .Child(Config[CarpetaDestino])
                    .Child(NombreArchivo)
                    .PutAsync(StreamArchivo, cancellation.Token);

                UrlImagen = await task;
            }
            catch
            {
                UrlImagen = "";
            }
            return UrlImagen;
        }


        public async Task<bool> EliminarStorage(string CarpetaDestino, string NombreArchivo)
        {
            try
            {
                IQueryable<Configuracion> query = await _repositorio.Cunsultar(c => c.Recurso.Equals("FireBase_Storage"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);

                //Autorizacion
                var auth = new FirebaseAuthProvider(new FirebaseAuthConfig(Config["api_key"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);

                //token de cancelacion de origen
                var cancellation = new CancellationTokenSource();

                //tarea que ejecuta el servicio de FireBaseStorage
                var task = new FirebaseStorage(
                    Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        //funcion anonima qu ejecuta la tarea
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        //si hay algun error lo cancela
                        ThrowOnCancel = true
                    })
                    //crear las carpetas
                    .Child(Config[CarpetaDestino])
                    .Child(NombreArchivo)
                    .DeleteAsync();

                await task;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}