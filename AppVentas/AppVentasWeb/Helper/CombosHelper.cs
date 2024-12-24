using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace AppVentasWeb.Helper
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContex _context;

        public CombosHelper(DataContex context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCategoriasAsync()
        {
            List<SelectListItem> list = await _context.Categorias.Select(x => new SelectListItem
            {
                Text = x.Nombre,
                Value = $"{x.Id}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "Seleccione una categoría...",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCiudadesAsync(int comunasId)
        {
            List<SelectListItem> list = await _context.Ciudades
           .Where(x => x.Comuna.Id == comunasId)
           .Select(x => new SelectListItem
           {
               Text = x.Nombre,
               Value = $"{x.Id}"
           })
           .OrderBy(x => x.Text)
           .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "Seleccione una ciudad...",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboComunasAsync(int regionesId)
        {
            List<SelectListItem> list = await _context.Comunas
             .Where(x => x.Region.Id == regionesId)
             .Select(x => new SelectListItem
             {
                 Text = x.Nombre,
                 Value = $"{x.Id}"
             })
             .OrderBy(x => x.Text)
             .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "Seleccione una comuna...",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboPaisesAsync()
        {
            List<SelectListItem> list = await _context.Paises.Select(x => new SelectListItem
            {
                Text = x.Nombre,
                Value = $"{x.Id}"
            })
           .OrderBy(x => x.Text)
           .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "Seleccione un país...",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboRegionesAsync(int paisId)
        {
            List<SelectListItem> list = await _context.Regiones
           .Where(x => x.Pais.Id == paisId)
           .Select(x => new SelectListItem
           {
               Text = x.Nombre,
               Value = $"{x.Id}"
           })
           .OrderBy(x => x.Text)
           .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "Seleccione una región...",
                Value = "0"
            });

            return list;
        }
    }
}