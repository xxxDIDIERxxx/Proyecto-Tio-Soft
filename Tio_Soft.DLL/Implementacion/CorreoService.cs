using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;

using Tio_Soft.BLL.Interfaces;
using Tio_Soft.DAL.Interfaces;
using Tio_Soft.Entity;

namespace Tio_Soft.BLL.Implementacion
{
    public class CorreoService : ICorreoService
    {
        //contexto-referencia de GeneryRepository
        private readonly IGenericRepository<Configuracion> _repositorio;
        
        //constructor
        public CorreoService (IGenericRepository<Configuracion> repositorio)
        {
            _repositorio = repositorio;
        }
        

        //implemeta la interfaz de ICorreoService
        public async Task<bool> EnviarCorreo(string CorreoDestino, string Asunto, string Mensaje)
        {
            try
            {
                IQueryable<Configuracion> query = await _repositorio.Cunsultar(c => c.Recurso.Equals("Servicio_Correro"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);

                //credenciales
                var credenciales = new NetworkCredential(Config["correo"], Config["clave"]);

                //mensaje
                var correo = new MailMessage()
                {
                    From = new MailAddress(Config["correo"], Config["alias"]),
                    Subject = Asunto,
                    Body = Mensaje,
                    IsBodyHtml = true
                };

                correo.To.Add(new MailAddress(CorreoDestino));

                var clienteServidor = new SmtpClient()
                {
                    Host = Config["host"],
                    Port = int.Parse(Config["puerto"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                };

                //envia el correo
                clienteServidor.Send(correo);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
