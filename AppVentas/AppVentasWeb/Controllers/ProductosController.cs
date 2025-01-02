using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using AppVentasWeb.Helper;
using AppVentasWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace AppVentasWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductosController : Controller
    {
        private readonly DataContex _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;

        public ProductosController(DataContex context, ICombosHelper combosHelper, IBlobHelper blobHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Productos
                               .Include(c => c.ProductImages)
                               .Include(c => c.ProductCategories)
                               .ThenInclude(c => c.Categoria).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Producto product = await _context.Productos
                                .Include(p => p.ProductImages)
                                .Include(p => p.ProductCategories)
                                .ThenInclude(pc => pc.Categoria)
                                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateProductViewModel model = new()
            {
                Categorias = await _combosHelper.GetComboCategoriasAsync(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "productos");
                }

                Producto producto = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Stock = model.Stock,
                };

                producto.ProductCategories = new List<ProductCategory>()
                {
                    new ProductCategory
                    {
                        Categoria = await _context.Categorias.FindAsync(model.CategoriaId)
                    }
                };

                if (imageId != Guid.Empty)
                {
                    producto.ProductImages = new List<ProductImage>()
{
                     new ProductImage { ImageId = imageId }
};
                }
                try
                {
                    _context.Add(producto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                       
                        _flashMessage.Danger("Ya existe un producto con el mismo nombre.");
                    }
                    else
                    {
                       
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            model.Categorias = await _combosHelper.GetComboCategoriasAsync();
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Producto producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            EditProductoViewModel model = new()
            {
                Description = producto.Description,
                Id = producto.Id,
                Name = producto.Name,
                Price = producto.Price,
                Stock = producto.Stock,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            try
            {
                Producto producto = await _context.Productos.FindAsync(model.Id);
                producto.Description = model.Description;
                producto.Name = model.Name;
                producto.Price = model.Price;
                producto.Stock = model.Stock;
                _context.Update(producto);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                   
                    _flashMessage.Danger("Ya existe un producto con el mismo nombre.");
                }
                else
                {
                    _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    
                }
            }
            catch (Exception exception)
            {
           
                _flashMessage.Danger(exception.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Producto producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            AddProductImageViewModel model = new()
            {
                ProductoId = producto.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddProductImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "productos");
                }

                Producto producto = await _context.Productos.FindAsync(model.ProductoId);

                ProductImage productoImage = new()
                {
                    Producto = producto,
                    ImageId = imageId
                };

                try
                {
                    _context.Add(productoImage);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = model.ProductoId });
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductImage productImage = await _context.ProductImages
                                                .Include(p => p.Producto)
                                                .FirstOrDefaultAsync(p => p.Id == id);

            if (productImage == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(productImage.ImageId, "productos");

            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { Id = productImage.Producto.Id });
        }

        public async Task<IActionResult> AddCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Producto producto = await _context.Productos
                                      .Include(p => p.ProductCategories)
                                       .ThenInclude(p => p.Categoria)
                                       .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            List<Categoria> categorias = producto.ProductCategories.Select(pc => new Categoria
            {
                Id = pc.Categoria.Id,
                Nombre = pc.Categoria.Nombre,
            }).ToList();

            AddCategoryProductViewModel model = new()
            {
                ProductoId = producto.Id,
                Categories = await _combosHelper.GetComboCategoriasAsync(categorias),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddCategoryProductViewModel model)
        {
            Producto producto = await _context.Productos
                                         .Include(p => p.ProductCategories)
                                         .ThenInclude(p => p.Categoria)
                                         .FirstOrDefaultAsync(p => p.Id == model.ProductoId);

            if (ModelState.IsValid)
            {
                ProductCategory productCategory = new()
                {
                    Categoria = await _context.Categorias.FindAsync(model.CategoryId),
                    Producto = producto,
                };
                try
                {
                    _context.Add(productCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = producto.Id });
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            List<Categoria> categorias = producto.ProductCategories.Select(pc => new Categoria
            {
                Id = pc.Categoria.Id,
                Nombre = pc.Categoria.Nombre,
            }).ToList();

            model.Categories = await _combosHelper.GetComboCategoriasAsync(categorias);
            return View(model);
        }

        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ProductCategory productCategory = await _context.ProductCategories
                                                    .Include(pc => pc.Producto)
                                                    .FirstOrDefaultAsync(pc => pc.Id == id);

            if (productCategory == null)
            {
                return NotFound();
            }
            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { Id = productCategory.Producto.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Producto producto = await _context.Productos
            .Include(p => p.ProductCategories)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Producto model)
        {
            Producto producto = await _context.Productos
             .Include(p => p.ProductCategories)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == model.Id);

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            foreach (ProductImage productImage in producto.ProductImages)
            {
                await _blobHelper.DeleteBlobAsync(productImage.ImageId, "productos");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}