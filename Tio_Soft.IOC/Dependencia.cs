using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tio_Soft.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Tio_Soft.DAL.Implementacion;
using Tio_Soft.DAL.Interfaces;
//using Tio_Soft.BLL.Implementacion;
//using Tio_Soft.BLL.Interfaces;


namespace Tio_Soft.IOC
{
    public static class Dependencia
    {
        //Metodo de extención
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<Tio_SoftContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ConexionDB"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();
        }
    }
}
