using System;
using System.Collections.Generic;

namespace Tio_Soft.Entity
{
    public partial class Menu
    {
        public Menu()
        {
            InverseIdMenuPadreNavigation = new HashSet<Menu>();
            RolMenus = new HashSet<RolMenu>();
        }

        public int IdMenu { get; set; }
        public string? Nombre { get; set; }
        public int? IdMenuPadre { get; set; }
        public string? Icono { get; set; }
        public string? Controlador { get; set; }
        public string? PaginaAccion { get; set; }
        public bool? EsActivo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Menu? IdMenuPadreNavigation { get; set; }
        public virtual ICollection<Menu> InverseIdMenuPadreNavigation { get; set; }
        public virtual ICollection<RolMenu> RolMenus { get; set; }
    }
}
