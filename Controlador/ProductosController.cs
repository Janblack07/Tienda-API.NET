using API_TIENDA.Dto;
using API_TIENDA.Models;
using API_TIENDA.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_TIENDA.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly DbTienda _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductosController(DbTienda context, IWebHostEnvironment environment, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _environment = environment;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        [Route("AllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            try
            {
                var productos = _context.Productos.Select(p => new ProductosDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Descripcion = p.Descripcion,
                    ImageUrl = p.ImageUrl,
                    CategoriaId = p.CategoriaId // Asegúrate de incluir la categoría en la respuesta
            }).ToList();

                return Ok(new { message = "Todos Los Productos : ", productos }); // 200 OK
            }
            catch (Exception ex)
            {
                // Manejo de error inesperado
                return StatusCode(500, new { message = "Error al obtener los productos.", details = ex.Message }); // 500 Internal Server Error
            }
        }

        [HttpGet]
        [Route("SearchProduct/{id}")]
        public async Task<IActionResult> GetSearchProduct(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);

                if (producto == null)
                {
                    return NotFound(new { message = "Producto no encontrado." }); // 404 Not Found
                }

                var productoDto = new ProductosDto
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Descripcion = producto.Descripcion,
                    ImageUrl = producto.ImageUrl,
                    CategoriaId = producto.CategoriaId // Incluir categoría en la respuesta
                };

                return Ok(new { message = " El Producto es :", productoDto }); // 200 OK
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el producto.", details = ex.Message }); // 500 Internal Server Error
            }
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductoDto createProductoDto)
        {
            try
            {
                // Verificar si la categoría existe
                var categoria = await _context.Categorias.FindAsync(createProductoDto.CategoriaId);
                if (categoria == null)
                {
                    return BadRequest(new { message = "Categoría no encontrada." });
                }
                // Subir la imagen a Cloudinary
                string imageUrl = null;
                if (createProductoDto.Imagen != null)
                {
                    imageUrl = await _cloudinaryService.UploadImageAsync(createProductoDto.Imagen, "productos");
                }

                var producto = new Productos
                {
                    Nombre = createProductoDto.Nombre,
                    Precio = createProductoDto.Precio,
                    Descripcion = createProductoDto.Descripcion,
                    ImageUrl = imageUrl,
                    CategoriaId = createProductoDto.CategoriaId // Relacionar el producto con la categoría
                };

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSearchProduct), new { id = producto.Id },
                    new{message = "Producto añadido con éxito",producto});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el producto.", details = ex.Message });
            }
        }

        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductoDto updateProductoDto)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    return NotFound(new { message = "Producto no encontrado." });
                }
                // Verificar si la categoría existe antes de asignarla
                if (updateProductoDto.CategoriaId.HasValue)
                {
                    var categoria = await _context.Categorias.FindAsync(updateProductoDto.CategoriaId);
                    if (categoria == null)
                    {
                        return BadRequest(new { message = "Categoría no encontrada." });
                    }
                    producto.CategoriaId = updateProductoDto.CategoriaId.Value; // Asignar la nueva categoría
                }

                if (!string.IsNullOrEmpty(updateProductoDto.Nombre))
                    producto.Nombre = updateProductoDto.Nombre;
                if (updateProductoDto.Precio.HasValue)
                    producto.Precio = updateProductoDto.Precio.Value;
                if (!string.IsNullOrEmpty(updateProductoDto.Descripcion))
                    producto.Descripcion = updateProductoDto.Descripcion;

                // Subir la nueva imagen si se proporciona
                if (updateProductoDto.Imagen != null)
                {
                    // Eliminar la imagen anterior de Cloudinary
                    if (!string.IsNullOrEmpty(producto.ImageUrl))
                    {

                        // Asegúrate de que publicId sea solo el ID de la imagen sin extensión
                        var publicId = producto.ImageUrl.Split('/').Last().Split('.').First();

                        // Agregar la carpeta "productos/" de manera estática
                        var fullPublicId = $"productos/{publicId}";


                        // Aquí se pasa el publicId completo con la carpeta "productos/"
                        await _cloudinaryService.DeleteImageAsync(fullPublicId);
                    }

                    // Subir la nueva imagen a Cloudinary
                    producto.ImageUrl = await _cloudinaryService.UploadImageAsync(updateProductoDto.Imagen, "productos");
                }

                _context.Productos.Update(producto);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Se ha actualizado el Producto: ", producto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el producto.", details = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);

                if (producto == null)
                {
                    return NotFound(new { message = "Producto no encontrado." });
                }

                // Eliminar la imagen de Cloudinary si existe
                if (!string.IsNullOrEmpty(producto.ImageUrl))
                {
                    // Asegúrate de que publicId sea solo el ID de la imagen sin extensión
                    var publicId = producto.ImageUrl.Split('/').Last().Split('.').First();

                    // Agregar la carpeta "productos/" de manera estática
                    var fullPublicId = $"productos/{publicId}";


                    // Aquí se pasa el publicId completo con la carpeta "productos/"
                    await _cloudinaryService.DeleteImageAsync(fullPublicId);
                }

                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Se elimino el Producto : ", producto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el producto.", details = ex.Message });
            }
        }

    }
}
